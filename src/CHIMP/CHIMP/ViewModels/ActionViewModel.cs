namespace Chimp.ViewModels
{
    sealed class ActionViewModel : ItemsViewModel<ActionItemViewModel>
    {
        public string? Title { get; set; }

        public static ActionViewModel? Get(MainViewModel mainViewModel) => mainViewModel.Get<ActionViewModel>("Action");
    }
}
