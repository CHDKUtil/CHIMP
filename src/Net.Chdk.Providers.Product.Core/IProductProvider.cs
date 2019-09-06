namespace Net.Chdk.Providers.Product
{
    public interface IProductProvider
    {
        string[] GetCategoryNames();
        string[] GetProductNames();
        string GetCategoryName(string productName);
    }
}
