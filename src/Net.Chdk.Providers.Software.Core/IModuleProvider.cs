namespace Net.Chdk.Providers.Software
{
    public interface IModuleProvider
    {
        string GetPath(string productName);
        string GetExtension(string productName);
        string GetModuleName(string productName, string filePath);
        string GetModuleTitle(string productName, string moduleName);
        string GetModuleId(string productName, string moduleName);
    }
}
