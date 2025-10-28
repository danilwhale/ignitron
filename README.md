## ignitron

modloader for [allumeria](https://unobtainablemelon.itch.io/allumeria)

### installing

#### download artifact

go to [Releases page](https://github.com/danilwhale/ignitron/releases/), find version that corresponds your game version
(all releases have their target version specified in parenthesis: `0.4.0 (0.10)`).
now look for `Ignitron.Loader.zip` link, under version changelog, and click on it

#### actually installing

make `mods` folder in the game directory, and unpack `Ignitron.Loader.zip` into it.

done!

### for developers

> [!CAUTION]
> ***until 0.10***, you can ***NOT*** use Harmony patches or MonoMod in production environment.
> see https://github.com/MonoMod/MonoMod/issues/129 for more information.
>
> go [here](https://github.com/danilwhale/ignitron/blob/7a70196e36a65ebd7c7378ba54ad2c6dd738d1f3/README.md) to view
> steps for pre-0.10 setup

#### workspace configuration

> [!IMPORTANT]
> you need to have [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) installed!

1. get and download [allumeria](https://store.steampowered.com/app/3516590/Allumeria/) (modloader is targetted for 0.10,
   which is latest
   as of 25-10-2025)
2. set environment variable `ALLUMERIA_GAME_DIR` to game installation directory

> [!TIP]
> you can find game installation directory by going in Steam library:
> 1. select Allumeria Playtest in game library
> 2. press gear icon on right side of the window
> 3. Manage...
> 4. View local files...

#### building

```
dotnet build Ignitron.Loader/Ignitron.Loader.csproj
```

#### developing a mod

use [the mod template](https://github.com/danilwhale/ignitron-mod-template) to make a new mod!