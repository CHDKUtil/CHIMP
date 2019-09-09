namespace Net.Chdk.Detectors.Camera
{
    public interface IFilePatternProvider
    {
        string[] Patterns { get; }
        string PatternsDescription { get; }
    }
}
