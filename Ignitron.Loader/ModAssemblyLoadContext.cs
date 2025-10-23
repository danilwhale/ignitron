using System.IO.Compression;
using System.Reflection;
using System.Runtime.Loader;

namespace Ignitron.Loader;

internal sealed class ModAssemblyLoadContext(ModBox mod) : AssemblyLoadContext, IDisposable
{
    public readonly ZipArchive? Archive = mod.AssemblyPath == null ? ZipFile.OpenRead(mod.RootPath) : null;
    private readonly AssemblyDependencyResolver? _resolver = mod.AssemblyPath != null ? new AssemblyDependencyResolver(mod.AssemblyPath) : null;

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        // do NOT load already loaded assembles
        // if we do, everything is FUCKED
        foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (ass.FullName.Equals(assemblyName.FullName, StringComparison.Ordinal))
            {
                return null;
            }
        }
        
        // if mod is an archive we need to load assemblies from it
        if (mod.AssemblyPath == null)
        {
            // this is really scuffed but should work
            // it *will* explode if you do some magic with .deps.json
            string assemblyPath =
                !string.IsNullOrEmpty(assemblyName.CultureName) && !string.Equals(assemblyName.CultureName, "neutral", StringComparison.OrdinalIgnoreCase)
                    ? Path.Join(assemblyName.CultureName, assemblyName.Name + ".dll")
                    : assemblyName.Name + ".dll";
            ZipArchiveEntry? assemblyEntry = Archive!.GetEntry(assemblyPath);
            if (assemblyEntry == null) return null;

            // now we need to decompress the assembly
            using Stream entryStream = assemblyEntry.Open();
            using MemoryStream decompStream = new();
            entryStream.CopyTo(decompStream);
            decompStream.Position = 0;

            // finally we can load it
            return LoadFromStream(decompStream);
        }
        else
        {
            // load normally using resolver
            string? assemblyPath = _resolver!.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }
    }

    public void Dispose()
    {
        Archive?.Dispose();
    }
}