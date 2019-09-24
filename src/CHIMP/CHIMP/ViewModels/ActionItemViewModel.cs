namespace Chimp.ViewModels
{
    sealed class ActionItemViewModel
    {
        public string? DisplayName { get; set; }
        public IAction? Action { get; set; }
    }
}
