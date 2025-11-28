## ignitron

modloader for [allumeria](https://unobtainablemelon.itch.io/allumeria)

### installing

#### download artifact

go to [Releases page](https://github.com/danilwhale/ignitron/releases/), find version that corresponds your game version
(all releases have their target version specified in parenthesis: `0.4.0 (0.10-0.11)`).
now look for `Ignitron.Loader.zip` link, under version changelog, and click on it

#### actually installing

make `mods` folder in the game directory and unpack `Ignitron.Loader.zip` into it.

done!

### for developers

> [!IMPORTANT]
> since 0.5.0, you **CAN NOT** use modloader with versions before 0.12 or MP TEST due to upgrade to .NET 10.0 from .NET 9.0

#### workspace configuration

> [!IMPORTANT]
> you need to have [.NET SDK **10.0**](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) installed!

1. get and download [allumeria](https://store.steampowered.com/app/3516590/Allumeria/) (modloader is targetted for MP TEST, which is in closed beta as of 2025-11-28)
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