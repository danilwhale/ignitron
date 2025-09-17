using StbImageSharp;

namespace ContentAPI;

public interface IIconRegister
{
    Icon? Register(string key, ImageResult image);
    Icon? this[string key] { get; }
}