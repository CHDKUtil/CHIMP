namespace Net.Chdk.Providers.Software
{
    interface IProductModuleProvider
    {
        string Path { get; }
        string Extension { get; }

        string GetModuleName(string filePath);
        string GetModuleTitle(string moduleName);
        string GetModuleId(string moduleName);
    }
}
