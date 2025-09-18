using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Ignitron.Loader.API.Versioning;

/// <summary>
/// version struct that supports wildcards.
/// for example, 1.* will be [1, null, null, null], * will be [null, null, null, null], 1.*.0 will be [1, null, 0, null]
/// </summary>
[JsonConverter(typeof(WildcardVersionJsonConverter))]
public readonly struct WildcardVersion(uint? major = null, uint? minor = null, uint? patch = null, uint? revision = null) 
    : IEquatable<WildcardVersion>, IEquatable<Version>, ISpanParsable<WildcardVersion>
{
    public static readonly WildcardVersion Any = new();
    
    public readonly uint? Major = major, Minor = minor, Patch = patch, Revision = revision;

    public WildcardVersion() : this(major: null)
    {
    }

    public WildcardVersion(scoped ReadOnlySpan<uint?> components)
        : this(components[0], components[1], components[2], components[3])
    {
    }

    public bool Equals(WildcardVersion other)
    {
        return (Major is null || other.Major is null || Major == other.Major) &&
               (Minor is null || other.Minor is null || Minor == other.Minor) &&
               (Patch is null || other.Patch is null || Patch == other.Patch) &&
               (Revision is null || other.Revision is null || Revision == other.Revision);
    }

    public bool Equals(Version? other)
    {
        return (other is null && Major is null && Minor is null && Patch is null && Revision is null) ||
               (other is not null &&
                (Major is null || Major == other.Major) &&
                (Minor is null || Minor == other.Minor) &&
                (Patch is null || Patch == other.Build) &&
                (Revision is null || Revision == other.Revision));
    }

    public override bool Equals(object? obj)
    {
        return (obj is WildcardVersion otherMod && Equals(otherMod)) ||
               (obj is Version otherVer && Equals(otherVer));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Major, Minor, Patch, Revision);
    }

    public override string ToString()
    {
        return $"{Major?.ToString(CultureInfo.InvariantCulture) ?? "*"}.{Minor?.ToString(CultureInfo.InvariantCulture) ?? "*"}.{Patch?.ToString(CultureInfo.InvariantCulture) ?? "*"}.{Revision?.ToString(CultureInfo.InvariantCulture) ?? "*"}";
    }

    public static bool operator ==(WildcardVersion left, WildcardVersion right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(WildcardVersion left, WildcardVersion right)
    {
        return !left.Equals(right);
    }

    public static WildcardVersion Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        Span<uint?> components = stackalloc uint?[4];

        int i = 0;
        foreach (Range componentRange in s.Split('.'))
        {
            if (i >= 4) throw new ArgumentException("version string contains more than 4 numbers", nameof(s));
            
            ReadOnlySpan<char> componentChars = s[componentRange];
            if (componentChars is not "*") // skip wildcard char
                components[i] = uint.Parse(componentChars);
            i++;
        }

        return new WildcardVersion(components);
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out WildcardVersion result)
    {
        Span<uint?> components = stackalloc uint?[4];
        
        int i = 0;
        foreach (Range componentRange in s.Split('.'))
        {
            if (i >= 4)
            {
                result = default;
                return false;
            }
            
            ReadOnlySpan<char> componentChars = s[componentRange];
            if (componentChars is "*") continue; // skip wildcard char
            if (!uint.TryParse(componentChars, out uint component))
            {
                result = default;
                return false;
            }

            components[i++] = component;
        }
        
        result = new WildcardVersion(components);
        return true;
    }

    public static WildcardVersion Parse(string? s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out WildcardVersion result)
    {
        if (s is null)
        {
            result = Any;
            return false;
        }

        return TryParse(s.AsSpan(), provider, out result);
    }
}