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
> as of 0.9.10, you can ***NOT*** use Harmony patches or MonoMod in production environment.
> see https://github.com/MonoMod/MonoMod/issues/129 for more information.
> hopefully, this *should* be resolved in 0.10
>
> as a workaround, you can do [configuration](#configuring) steps and run the game using
> ```
> dotnet unpacked/Allumeria.dll
> ```

#### workspace configuration

> [!IMPORTANT]
> you need to have [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) installed!

1. download [allumeria](https://unobtainablemelon.itch.io/allumeria) (modloader is targetted for 0.9.10, which is latest
   as of 22-10-2025)
2. unpack it somewhere
3. ***[install sfextract](https://github.com/Droppers/SingleFileExtractor?tab=readme-ov-file#install)***
   - `dotnet tool install -g sfextract` (if you're too lazy to click the link)
4. unpack the game executable
    - `sfextract Allumeria.exe -o unpacked`
5. set environment variables
   - `ALLUMERIA_GAME_DIR` = `<GAME_DIRECTORY>` (directory with `Allumeria.exe`)
   - `ALLUMERIA_UNPACKED_DIR` = `<GAME_DIRECTORY>/unpacked` (if you followed previous step. otherwise, directory with `Allumeria.dll` and similar)

> [!IMPORTANT]
> you won't be able to run the game as is. you need to edit `Allumeria.runtimeconfig.json` a bit: replace
`includedFrameworks` with `frameworks`; also set environment variable `DOTNET_READYTORUN` to `0`.

#### building

```
dotnet build Ignitron.Loader/Ignitron.Loader.csproj
```

#### developing a mod

use [mod template](https://github.com/danilwhale/ignitron-mod-template) to make a new mod!

you can use my existing mods as reference, such as [localeloader](https://github.com/danilwhale/localeloader)