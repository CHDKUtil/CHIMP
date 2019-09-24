namespace Chimp.Model
{
    sealed class StepData
    {
        public string? Name { get; set; }
        public string? Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Hidden { get; set; }
    }

    sealed class StepsData
    {
        public StepData[]? Steps { get; set; }
    }
}
