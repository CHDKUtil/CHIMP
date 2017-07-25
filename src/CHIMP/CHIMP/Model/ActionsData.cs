namespace Chimp.Model
{
    sealed class ActionData
    {
        public string Name { get; set; }
        public string Assembly { get; set; }
        public string Namespace { get; set; }
    }

    sealed class ActionsData
    {
        public ActionData[] Actions { get; set; }
    }
}
