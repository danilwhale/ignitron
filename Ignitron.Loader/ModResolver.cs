using System.Text.Json;
using Allumeria;
using Ignitron.Loader.Metadata;
using Ignitron.Loader.Metadata.Json;

namespace Ignitron.Loader;

internal sealed class ModResolver(IgnitronLoader loader)
{
    private readonly List<ModBox> _foundMods = [];
    private readonly HashSet<string> _foundModIds = [];

    public List<ModBox> Resolve(string rootPath)
    {
        // include allumeria by default
        _foundMods.Add(new ModBox(new AllumeriaModMetadata(), loader.GamePath));

        try
        {
            CollectMods(rootPath);
        }
        catch (Exception ex)
        {
            throw new ModResolverException("Unexpected exception when collecting mods", ex);
        }

        try
        {
            ResolveDependencies();
        }
        catch (Exception ex)
        {
            throw new ModResolverException("Unexpected exception when resolving dependencies", ex);
        }

        return _foundMods;
    }

    private void CollectMods(string rootPath)
    {
        foreach (string modRootPath in Directory.GetDirectories(rootPath))
        {
            string metadataPath = Path.Join(modRootPath, "Metadata.json");
            if (!File.Exists(metadataPath))
            {
                // not a mod directory
                continue;
            }

            // deserialize metadata
            using FileStream stream = File.OpenRead(metadataPath);
            using JsonDocument metadataDoc = JsonDocument.Parse(stream);
            
            IModMetadata metadata;
            if (!metadataDoc.RootElement.TryGetProperty("schema_version", out JsonElement schemaVersionElement) ||
                !schemaVersionElement.TryGetInt32(out int schemaVersion))
            {
                throw new InvalidOperationException("Invalid metadata definition");
            }

            if (schemaVersion == 2)
            {
                metadata = metadataDoc.Deserialize<JsonV2ModMetadata>()!;
            }
            else
            {
                throw new InvalidOperationException("Invalid schema version. Are you trying to run mod made for newer version?");
            }
            
            if (metadata == null)
            {
                throw new InvalidOperationException($"Deserialized '{metadataPath}' is null");
            }

            // make sure mod hasn't been loaded before
            if (_foundModIds.Contains(metadata.Id))
            {
                throw new InvalidOperationException($"'{metadata.Id}' has been duplicated!");
            }

            string assemblyPath = Path.Join(modRootPath, metadata.AssemblyRelativePath);
            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException($"Assembly file at '{assemblyPath}' doesn't exist");
            }

            // include
            _foundMods.Add(new ModBox(metadata, modRootPath, assemblyPath));
            _foundModIds.Add(metadata.Id);
        }
    }

    private void ResolveDependencies()
    {
        foreach (ModBox mod in _foundMods)
        {
            foreach (IModDependency dependency in mod.Metadata.Dependencies)
            {
                ModBox? found = null;
                foreach (ModBox dependencyMod in _foundMods)
                {
                    if (dependencyMod.Metadata.Id.Equals(dependency.Id, StringComparison.Ordinal))
                    {
                        found = dependencyMod;
                        break;
                    }
                }

                if (found == null)
                {
                    switch (dependency.Type)
                    {
                        case ModDependencyType.Mandatory:
                            throw new InvalidOperationException($"'{mod.Metadata.Id}' is missing dependency: '{dependency.Id}' {dependency.Version}");
                        case ModDependencyType.Recommended:
                            Logger.Info($"'{mod.Metadata.Id}' recommends installing '{dependency.Id}' {dependency.Version}");
                            break;
                    }
                }
                else
                {
                    switch (dependency.Type)
                    {
                        case ModDependencyType.NotRecommended:
                            Logger.Warn($"'{mod.Metadata.Id}' suggests removing '{dependency.Id}' {dependency.Version}");
                            break;
                        case ModDependencyType.Incompatible:
                            throw new InvalidOperationException($"'{mod.Metadata.Id}' isn't compatible with '{dependency.Id}' {dependency.Version}");
                        case ModDependencyType.Mandatory
                            when !dependency.Version.Equals(found.Metadata.Version):
                            throw new InvalidOperationException(
                                $"'{mod.Metadata.Id}' needs '{dependency.Id}' of version {dependency.Version}, but {found.Metadata.Version} is installed instead");
                        case ModDependencyType.Optional or ModDependencyType.Recommended
                            when !dependency.Version.Equals(found.Metadata.Version):
                            Logger.Warn(
                                $"'{mod.Metadata.Id}' suggests '{dependency.Id}' of version {dependency.Version}, but {found.Metadata.Version} is installed instead");
                            break;
                    }
                }
            }
        }
    }
}