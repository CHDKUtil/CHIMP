using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Chimp.Controllers
{
    sealed class IntroController : Controller<IntroController, IntroViewModel>
    {
        protected override bool CanSkipStep => true;

        public IntroController(MainViewModel mainViewModel, IStepProvider stepProvider, string stepName, ILoggerFactory loggerFactory)
            : base(mainViewModel, stepProvider, stepName, loggerFactory)
        {
        }

        protected override void Initialize()
        {
			base.Initialize();
			ViewModel = CreateViewModel();
        }

        protected override void EnterStep()
        {
            StepViewModel!.CanContinue = true;
        }

        private IntroViewModel? CreateViewModel()
        {
            if (SkipStep)
                return null;
            var title = string.Format(Resources.Intro_Welcome_Format, Resources._Title);
            var message = Resources.Intro_Message_Text;
            var tips = JsonConvert.DeserializeObject<Model.Tip[]>(Resources.intro_tips);
            return new IntroViewModel(title, message, tips);
        }
    }
}
