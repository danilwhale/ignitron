## ignitron
modloader for [allumeria](https://unobtainablemelon.itch.io/allumeria)

### building

building the loader itself is simple as:

```
dotnet build Ignitron.Loader/Ignitron.Loader.csproj
```

### mods

#### configuring

first of all, you need to download the game (modloader is targetted for 0.8.3, but should work with any version). after downloading, unpack the game somewhere, then install [sfextract](https://github.com/Droppers/SingleFileExtractor?tab=readme-ov-file#install) and run `sfextract PocketBlocks.exe -o unpacked` in the game directory.

now, go to /Directory.Build.props, and edit `GameDirectory` and `UnpackedGameDirectory` to paths of directories with `PocketBlocks.exe` and `PocketBlocks.dll` (should be `<GAME_DIRECTORY>/unpacked`, if you followed previous step) respectively. now you're done with configuring the workspace

#### building

afer finishing [configuring](#configuring), just run:

```
dotnet build
```

and mods should copy into your `<GAME_DIRECTORY>/mods` directory automatically.

### making new mod

make sure to do [configuring](#configuring)! make a new project in `Mods/` directory, and you should automatically have game's assembly included + modloader

to make modloader detect your mod, make `Metadata.json` file with contents:

```json
{
  "$schema": "https://github.com/danilwhale/ignitron/blob/master/Metadata.schema.json",
  "AssemblyFile": "<your_project_name>.dll",
  "Id": "<unique_mod_id>",
  "Name": "<mod_name>",
  "Author": "<your_username>",
  "Version": "<major.minor.build.revision>",
  "GameVersion": "<target_game_version>"
}
```

you can find more properties in the schema json! you can view [LocaleLoader's metadata file](Mods/LocaleLoader/Metadata.json) as an example.

now you can make the class for your mod! create a new class and inherit `Mod` class:

```cs
public sealed class MyMod : Mod
{
  public override void Initialize()
  {
    Console.WriteLine("Hello, World!");
  }
}
```

finally, you can build your mod:

```
dotnet build Mods/MyMod/MyMod.csproj
```

or use your IDE gui. you can compile entire solution, but this will also copy my mods into your game instance.

now try starting the game with debugging and view the console output, you should see your "Hello, World!" in it!
