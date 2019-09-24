using Chimp.Model;

namespace Chimp.ViewModels
{
    public sealed class IntroViewModel : ViewModel
    {
        public IntroViewModel(string title, string message, Tip[] tips)
        {
            Title = title;
            Message = message;
            Tips = tips;
        }

        public string Title { get; }
        public string Message { get; }
        public Tip[] Tips { get; }
    }
}