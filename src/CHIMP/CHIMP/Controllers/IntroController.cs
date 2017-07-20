using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Chimp.Controllers
{
	sealed class IntroController : Controller<IntroController, IntroViewModel>
    {
        protected override string StepName => "Intro";
        protected override bool CanSkipStep => true;
        protected override bool SkipStep => Settings.Default.SkipIntroStep;

        public IntroController(MainViewModel viewModel, ILoggerFactory loggerFactory)
            : base(viewModel, loggerFactory)
        {
        }

        protected override void Initialize()
        {
			base.Initialize();
			ViewModel = CreateViewModel();
        }

        protected override void EnterStep()
        {
            StepViewModel.CanContinue = true;
        }

        private IntroViewModel CreateViewModel()
        {
            if (SkipStep)
                return null;
            return new IntroViewModel
            {
                Title = string.Format(Resources.Intro_Welcome_Format, Resources._Title),
                Message = Resources.Intro_Message_Text,
                Tips = JsonConvert.DeserializeObject<Model.Tip[]>(Resources.intro_tips),
            };
        }
    }
}
