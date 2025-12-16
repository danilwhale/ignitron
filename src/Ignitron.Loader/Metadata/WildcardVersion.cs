using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Ignitron.Loader.Metadata;

/// <summary>
/// Represents a version filter defined using wildcards. Can be used for filtering <see cref="Version"/>
/// </summary>
/// <param name="major">Major build of the version, or <c>null</c> for any</param>
/// <param name="minor">Minor build of the version, or <c>null</c> for any</param>
/// <param name="patch">Patch build of the version, or <c>null</c> for any</param>
/// <param name="revision">Revision build of the version, or <c>null</c> for any</param>
[JsonConverter(typeof(WildcardVersionJsonConverter))]
public readonly struct WildcardVersion(uint? major = null, uint? minor = null, uint? patch = null, uint? revision = null)
    : IEquatable<WildcardVersion>, IEquatable<Version>, ISpanParsable<WildcardVersion>
{
    /// <summary>
    /// Filter that accepts any version
    /// </summary>
    public static readonly WildcardVersion Any = new();

    /// <summary>
    /// Major build of the version. <c>null</c> to accept version with any major build
    /// </summary>
    public readonly uint? Major = major;

    /// <summary>
    /// Minor build of the version. <c>null</c> to accept version with any minor build
    /// </summary>
    public readonly uint? Minor = minor;

    /// <summary>
    /// Patch build of the version. <c>null</c> to accept version with any patch build
    /// </summary>
    public readonly uint? Patch = patch;

    /// <summary>
    /// Revision build of the version. <c>null</c> to accept version with any major build
    /// </summary>
    public readonly uint? Revision = revision;

    /// <summary>
    /// Initialises a new instance of the <see cref="WildcardVersion"/> class that accepts any version
    /// </summary>
    public WildcardVersion() : this(major: null)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WildcardVersion"/> class that accepts versions using given components
    /// </summary>
    public WildcardVersion(scoped ReadOnlySpan<uint?> components)
        : this(components[0], components[1], components[2], components[3])
    {
    }

    /// <inheritdoc />
    public bool Equals(WildcardVersion other)
    {
        return (Major is null || other.Major is null || Major == other.Major) &&
               (Minor is null || other.Minor is null || Minor == other.Minor) &&
               (Patch is null || other.Patch is null || Patch == other.Patch) &&
               (Revision is null || other.Revision is null || Revision == other.Revision);
    }

    /// <inheritdoc />
    public bool Equals(Version? other)
    {
        return (other is null && Major is null && Minor is null && Patch is null && Revision is null) ||
               (other is not null &&
                (Major is null || Major == other.Major) &&
                (Minor is null || Minor == other.Minor) &&
                (Patch is null || Patch == other.Build) &&
                (Revision is null || Revision == other.Revision));
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return (obj is WildcardVersion otherMod && Equals(otherMod)) ||
               (obj is Version otherVer && Equals(otherVer));
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Major, Minor, Patch, Revision);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Major?.ToString(CultureInfo.InvariantCulture) ?? "*"}.{Minor?.ToString(CultureInfo.InvariantCulture) ?? "*"}.{Patch?.ToString(CultureInfo.InvariantCulture) ?? "*"}.{Revision?.ToString(CultureInfo.InvariantCulture) ?? "*"}";
    }

    /// <summary>Compares two values to determine equality.</summary>
    /// <param name="left">The value to compare with <paramref name="right" />.</param>
    /// <param name="right">The value to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> is equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
    public static bool operator ==(WildcardVersion left, WildcardVersion right)
    {
        return left.Equals(right);
    }

    /// <summary>Compares two values to determine inequality.</summary>
    /// <param name="left">The value to compare with <paramref name="right" />.</param>
    /// <param name="right">The value to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> is not equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
    public static bool operator !=(WildcardVersion left, WildcardVersion right)
    {
        return !left.Equals(right);
    }

    /// <inheritdoc />
    public static WildcardVersion Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        Span<uint?> components = stackalloc uint?[4];

        int i = 0;
        foreach (Range componentRange in s.Split('.'))
        {
            if (i >= 4) throw new ArgumentException("Version string contains more than 4 numbers", nameof(s));

            ReadOnlySpan<char> componentChars = s[componentRange];
            if (componentChars is not "*") // skip wildcard char
                components[i] = uint.Parse(componentChars);
            i++;
        }

        return new WildcardVersion(components);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public static WildcardVersion Parse(string? s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return Parse(s.AsSpan(), provider);
    }

    /// <inheritdoc />
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