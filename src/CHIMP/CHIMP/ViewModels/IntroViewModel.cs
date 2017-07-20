using Chimp.Model;

namespace Chimp.ViewModels
{
    public sealed class IntroViewModel : ViewModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public Tip[] Tips { get; set; }
    }
}