# developing a mod

> [!IMPORTANT]
> read [workspace preparation steps](workspace.md) first

## creating a project

### using template (recommended)

using [the template](https://github.com/danilwhale/ignitron-mod-template) is recommended over manual setup for new developers and to match latest production version of the modloader.
there's own guide for template usage in its repo.

### manual setup

1. create a new .NET 9.0 class library project
   - this step depends on what you use, but generic way is `dotnet new classlib -f net9.0 -n <NAME>`
2. clone ignitron locally or add it as a git submodule
   - clone: `git clone https://github.com/danilwhale/ignitron.git`
   - submodule (needs git repo): `git submodule add https://github.com/danilwhale/ignitron.git`
3. add `<Import Project="<RELATIVE_IGNITRON_PATH>build\Ignitron.Sdk.Mod.targets"/>` in your project file (`.csproj`)
4. create class for your mod and make it implement `Ignitron.Loader.IModEntrypoint`
5. create `Metadata.json` in the project directory and fill it according to [the schema](https://github.com/danilwhale/ignitron/blob/master/Metadata.schema.json). you can find an example in `src/Ignitron.TestMod` or the previously mentioned template