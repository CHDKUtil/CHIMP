﻿using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Detectors.Card;
using Net.Chdk.Model.Card;
using Net.Chdk.Watchers.Volume;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Chimp.Controllers
{
    sealed class CardController : Controller<CardController, CardViewModel>
    {
        protected override bool CanSkipStep => ViewModel.SelectedItem != null;

        private SynchronizationContext SynchronizationContext { get; }
        private ICardDetector CardDetector { get; }
        private IVolumeWatcher VolumeWatcher { get; }
        private IVolumeContainer VolumeContainer { get; }
        private IPartitionService PartitionService { get; }
        private IBootService BootService { get; }
        private IScriptService ScriptService { get; }

        public CardController(SynchronizationContext synchronizationContext, ICardDetector cardDetector, IVolumeWatcher volumeWatcher, IVolumeContainer volumeContainer, IPartitionService partitionService, IBootService bootService, IScriptService scriptService,
            MainViewModel mainViewModel, IStepProvider stepProvider, string stepName, ILoggerFactory loggerFactory)
            : base(mainViewModel, stepProvider, stepName, loggerFactory)
        {
            SynchronizationContext = synchronizationContext;
            CardDetector = cardDetector;
            VolumeWatcher = volumeWatcher;
            VolumeContainer = volumeContainer;
            PartitionService = partitionService;
            BootService = bootService;
            ScriptService = scriptService;
        }

        protected override void Initialize()
        {
            base.Initialize();

            ViewModel = CreateViewModel();

            VolumeWatcher.Initialize();
            VolumeWatcher.VolumeAdded += VolumeWatcher_VolumeAdded;
            VolumeWatcher.VolumeRemoved += VolumeWatcher_VolumeRemoved;
            VolumeWatcher.Start();

            UpdateEject();
            UpdateCanContinue();
            UpdateIsPaused();
        }

        public override void Dispose()
        {
            base.Dispose();

            VolumeWatcher.Stop();
            VolumeWatcher.VolumeAdded -= VolumeWatcher_VolumeAdded;
            VolumeWatcher.VolumeRemoved -= VolumeWatcher_VolumeRemoved;
            VolumeWatcher.Dispose();
        }

        protected override void EnterStep()
        {
            Subscribe2();
            UpdateCanContinue();
            UpdateIsPaused();
        }

        protected override void LeaveStep()
        {
            Unsubscribe2();
        }

        private void Card_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ViewModel.SelectedItem = ViewModel.Items.Count == 1
                ? ViewModel.Items.Single()
                : null;
            ViewModel.IsSelect = ViewModel.Items.Count > 1;
        }

        private void Card_SelectedItemChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ViewModel.SelectedItem != null)
                Logger.LogObject(LogLevel.Information, "Selected {0}", ViewModel.SelectedItem.Info);

            UpdateEject();
            UpdateCanContinue();
            UpdateIsPaused();

            StepViewModel.SelectedItem = StepViewModel[StepName];
        }

        private void VolumeWatcher_VolumeAdded(object sender, string driveLetter)
        {
            SynchronizationContext.Post(new SendOrPostCallback(_ => AddCard(driveLetter)), sender);
        }

        private void VolumeWatcher_VolumeRemoved(object sender, string driveLetter)
        {
            SynchronizationContext.Post(new SendOrPostCallback(_ => RemoveCard(driveLetter)), sender);
        }

        private CardViewModel CreateViewModel()
        {
            var cards = CardDetector.GetCards();

            foreach (var card in cards)
                Logger.LogObject(LogLevel.Information, "Detected {0}", card);

            var vms = cards
                .Select(CreateCardItem)
                .Where(vm => vm != null);
            var viewModel = new CardViewModel
            {
                Items = new ObservableCollection<CardItemViewModel>(vms)
            };

            viewModel.SelectedItem = viewModel.Items.Count == 1
                ? viewModel.Items.Single()
                : null;
            viewModel.IsSelect = viewModel.Items.Count > 1;

            Subscribe2();

            return viewModel;
        }

        private void AddCard(string driveLetter)
        {
            var card = CardDetector.GetCard(driveLetter);
            if (card == null)
            {
                Logger.LogWarning("Non-removable {0}", driveLetter);
                return;
            }

            var vm = ViewModel.Items.SingleOrDefault(c => c.Info.DriveLetter.Equals(driveLetter));
            if (vm != null)
            {
                Logger.LogError("Already added {0}", driveLetter);
                return;
            }

            Logger.LogObject(LogLevel.Information, "Added {0}", card);

            ViewModel.Items.Add(CreateCardItem(card));
        }

        private void RemoveCard(string driveLetter)
        {
            var vm = ViewModel.Items.SingleOrDefault(c => c.Info.DriveLetter.Equals(driveLetter));
            if (vm == null)
            {
                Logger.LogError("Already removed {0}", driveLetter);
                return;
            }

            Logger.LogObject(LogLevel.Information, "Removed {0}", vm.Info);

            ViewModel.Items.Remove(vm);
        }

        private void Subscribe2()
        {
            if (ViewModel != null)
            {
                ViewModel.Items.CollectionChanged += Card_CollectionChanged;
                PropertyChangedEventManager.AddHandler(ViewModel, Card_SelectedItemChanged, nameof(CardViewModel.SelectedItem));
            }
        }

        private void Unsubscribe2()
        {
            if (ViewModel != null)
            {
                ViewModel.Items.CollectionChanged -= Card_CollectionChanged;
                PropertyChangedEventManager.RemoveHandler(ViewModel, Card_SelectedItemChanged, nameof(CardViewModel.SelectedItem));
            }
        }

        private void UpdateCanContinue()
        {
            StepViewModel.CanContinue = ViewModel?.SelectedItem != null;
        }

        private void UpdateIsPaused()
        {
            MainViewModel.IsWarning = ViewModel?.SelectedItem == null;
        }

        private void UpdateEject()
        {
            if (ViewModel.SelectedItem != null)
            {
                var volume = VolumeContainer.GetVolume(ViewModel.SelectedItem.Info.DriveLetter);
                var isHotplug = volume.IsHotplugDevice();
                EjectViewModel.IsEject = !isHotplug;
                EjectViewModel.IsCompleted = isHotplug;
            }
        }

        private CardItemViewModel CreateCardItem(CardInfo card)
        {
            try
            {
                if (card.Capacity == null)
                {
                    var volume = VolumeContainer.GetVolume(card.DriveLetter);
                    card.Capacity = volume.Disk.DiskSize;
                }
            }
            catch (FileNotFoundException)
            {
                return null;
            }

            // Catch pCloud drive error
            PartitionType[] partTypes;
            bool? switched;
            try
            {
                partTypes = PartitionService.GetPartitionTypes(card.DriveLetter);
                switched = PartitionService.TestSwitchedPartitions(partTypes);
            }
            catch (COMException)
            {
                return null;
            }

            return new CardItemViewModel
            {
                Info = card,
                DisplayName = GetDisplayName(card),
                PartitionTypes = partTypes,
                Switched = switched,
                Bootable = BootService.TestBootable(card, card.FileSystem),
                Scriptable = ScriptService.TestScriptable(card, card.FileSystem),
                FileSystem = GetFileSystem(card, partTypes, switched),
            };
        }

        private static string GetDisplayName(CardInfo card)
        {
            return !string.IsNullOrEmpty(card.Label)
                ? string.Format(Resources.Card_Drive_Format, card.Label, card.DriveLetter)
                : card.DriveLetter;
        }

        private static string GetFileSystem(CardInfo card, PartitionType[] partTypes, bool? switched)
        {
            return switched == null
                ? card.FileSystem
                : string.Join("\n", GetFileSystems(partTypes));
        }

        private static IEnumerable<string> GetFileSystems(PartitionType[] partTypes)
        {
            return partTypes
                .Select(GetFileSystem)
                .Where(s => s != null);
        }

        private static string GetFileSystem(PartitionType partType)
        {
            switch (partType)
            {
                case PartitionType.None:
                    return null;
                case PartitionType.PrimaryFAT:
                case PartitionType.PrimaryFAT_0:
                    return "FAT";
                case PartitionType.PrimaryFAT32:
                case PartitionType.PrimaryFAT32_2:
                    return "FAT32";
                case PartitionType.ExFAT:
                    return "exFAT";
                default:
                    return Resources.Card_FileSystem_Unknown_Text;
            }
        }
    }
}
