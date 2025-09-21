## ignitron

modloader for [allumeria](https://unobtainablemelon.itch.io/allumeria)

### installing

#### download artifact

go to [Releases page](https://github.com/danilwhale/ignitron/releases/), find version that corresponds your game version
(all releases have their target version specified in parenthesis: `0.2.0 (0.9.x)`).
now look for `Ignitron.Loader.zip` link, under version changelog, and click on it

#### actually installing

make `mods` folder in the game directory, and unpack `Ignitron.Loader.zip` into it.

done!

### for developers

> [!CAUTION]
> as of 0.9.4, you can ***NOT*** use Harmony patches or MonoMod in production environment.
> see https://github.com/MonoMod/MonoMod/issues/129 for more information.
> hopefully, this will be resolved in steam playtest release.
>
> as a workaround, you can do [configuration](#configuring) steps and run the game using
> ```
> dotnet unpacked/Allumeria.dll
> ```

#### workspace configuration

> [!IMPORTANT]
> you need to have [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) installed!

first of all, you need to download the game (modloader is targetted for 0.9.4, but should work with any version after
0.9). after downloading, unpack the game somewhere, then
[install sfextract](https://github.com/Droppers/SingleFileExtractor?tab=readme-ov-file#install) and run:
```
sfextract Allumeria.exe -o unpacked
```
in the game directory.

> [!IMPORTANT]
> you won't be able to run the game as is. you need to edit `Allumeria.runtimeconfig.json` a bit: replace
`includedFrameworks` with `frameworks`; also set environment variable `DOTNET_READYTORUN` to `0`.

now, go to `/Directory.Build.props`, uncomment and edit `GameDirectory` and `UnpackedGameDirectory` to paths of
directories with
`Allumeria.exe` and `Allumeria.dll` (should be `<GAME_DIRECTORY>/unpacked`, if you followed previous step)
respectively. now you're done with configuring the workspace

#### building

```
dotnet build Ignitron.Loader/Ignitron.Loader.csproj
```

#### developing a mod

use [mod template](https://github.com/danilwhale/ignitron-mod-template) to make a new mod!

you can use my existing mods as reference, such as [localeloader](https://github.com/danilwhale/localeloader)