# preparing workspace

> [!NOTE]
> if you're feeling to target 'retro' versions (before 0.10), you should read [ancient readme guide](https://github.com/danilwhale/ignitron/blob/7a70196e36a65ebd7c7378ba54ad2c6dd738d1f3/README.md) for now until i write it a separate guide later

## prerequsites

1. [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
2. Allumeria 0.10 or above
3. reading comprehension

## adding the game to `PATH`

obviously, you need to find the game directory first.
you can find guide on this in [the 'finding the game directory' section of the player installation guide](../player/install.md#finding-the-game-directory)

### windows

1. open 'Run' dialog (`⊞ Win` + `R`)
2. type `sysdm.cpl` and press `Enter`
3. open `Advanced` tab
4. press `Environment Variables...`
5. in the `User variable for ...` section press `New...`
6. for `Variable name` put `ALLUMERIA_GAME_DIR`
7. for `Variable value` put path of the previously found game directory
8. press `OK` for all windows

### linux

i haven't switched to linux *yet*, so no guide for that, sadly. you can check https://wiki.archlinux.org/title/Environment_variables for now or wait for someone to contribute the guide later :)

now you can start [developing a mod](develop.md)