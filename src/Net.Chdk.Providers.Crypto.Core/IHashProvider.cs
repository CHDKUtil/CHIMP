namespace Net.Chdk.Providers.Crypto
{
    public interface IHashProvider
    {
        string GetHashString(byte[] buffer, string hashName);
    }
}
