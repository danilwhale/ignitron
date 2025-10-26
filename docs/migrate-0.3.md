# migrating 0.1-0.3 to 0.4.0

> [!TIP]
> you can look in [test mod](https://github.com/danilwhale/ignitron/tree/master/Ignitron.TestMod) as example of new
> format

## updating `Metadata.json`

metadata schema has been overhauled: now it uses `snake_case`, you can specify entrypoint types, specify type of dependency and describe multiple
contributors rather than only the author

1. update `$schema` to `https://raw.githubusercontent.com/danilwhale/ignitron/refs/heads/master/Metadata.schema.json`
2. put `"schema_version": 2` below `$schema`
3. rename existing properties:
   1. `AssemblyPath` to `assembly_relative_path`
   2. `Id` to `id`
   3. `Name` to `display_name`
   4. `Version` to `version`
   5. `Description` to `description`
   6. `Dependencies` to `dependencies`
4. replace `"Author": <author>` with:
   ```json
   "contributors": [
     {
       "name": <author>,
       "role": "developer"
     }
   ]
   ```
5. update dependency properties:
   1. rename `Id` to `id`
   2. rename `Version` to `version`
   3. replace `"Optional": true` with `"type": "optional"`
6. put entrypoints property:
   ```json
   "entrypoints": [
     "<main_class_namespace>.<main_class_name>"
   ]
   ```
   replace `<main_class_namespace>` with namespace of your entrypoint class, for example: `Ignitron.TestMod`.
   replace `<main_class_name>` with name of your entrypoint class, for example: `TestMod`

## updating code

### project configuration (`.csproj` or `Directory.Build.props`)

1. update git submodule or git clone
   1. submodule can be updated using `git submodule update --remote`
   2. clone can be updated using `git pull`
2. if you did ***not*** use `build/Mod.targets` then you need to manually replace `Ignitron.Loader.API.csproj` reference with `Ignitron.Loader.csproj`
3. remove `UnpackedGameDirectory` property as it's not used anymore
4. remove references to harmony if you had any and add `harmony` dependency to metadata instead:
   ```json
   {
     "id": "harmony",
     "version": "2.4.1"
   }
   ```

### scripts (`.cs`)

1. remove *all* usages of `Ignitron.Loader.API`
2. replace `Ignitron.Loader.API.Mod` with `Ignitron.Loader.IModEntrypoint`:
   1. implement `IModEntrypoint.Main(ModBox)` instead of overriding `Mod.Initialize()`
   2. `Mod.Metadata` has been removed in favor of `IgnitronLoader.TryGetMod(string, out ModBox?)`
3. replace `Ignitron.Loader.API.ModLibrary` with following alternatives:
   1. `ModLibrary.Mods` -> `IgnitronLoader.Mods`
   2. `ModLibrary.FirstOrDefault(string)` -> `IgnitronLoader.TryGetMod(string, out ModBox?)`
   3. `ModLibrary.FirstOrDefault(System.Func<Mod, bool>)` doesn't have direct alternative, iterate over `IgnitronLoader.Mods` and select manually
   4. `ModLibrary.Add(Mod)` doesn't have alternatives
4. replace `Ignitron.Loader.API.ModLoader` with `Ignitron.Loader.IgnitronLoader`:
   1. `ModLoader.Version` -> `IgnitronLoader.Version`
   2. `ModLoader.GameVersion` -> `IgnitronLoader.GameVersion`
5. replace `Ignitron.Loader.API.ModMetadata` with `Ignitron.Loader.Metadata.IModMetadata`:
   1. `ModMetadata.AssemblyFile` -> `IModMetadata.AssemblyRelativePath`
   2. `ModMetadata.Name` -> `IModMetadata.DisplayName`
   3. `ModMetadata.Author` has been replaced with `IModMetadata.Contributors`, which is collection of `Ignitron.Loader.Metadata.IModContributor`. if you want to get single string you can try the following:
       ```cs
      IModMetadata metadata = ...;
      string author = string.Join(", ", metadata.Contributors.Select(c => c.Name));
      ```
6. replace `Ignitron.Loader.API.ModDependency` with `Ignitron.Loader.Metadata.IModDependency`:
   1. `ModMetadata.Optional` has been removed in favor of `ModDependencyType IModDependency.Type`
      ```cs
      // pre-0.4
      if (dependency.Optional) ...

      // 0.4
      if (dependency.Type == ModDependencyType.Optional) ...
      ```
7. `Ignitron.Loader.API.Versioning` classes have been moved to `Ignitron.Loader.Metadata`