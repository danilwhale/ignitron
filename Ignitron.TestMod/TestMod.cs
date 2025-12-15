using Allumeria;
using Allumeria.Blocks.Blocks;
using Allumeria.Items;
using Allumeria.Rendering;
using HarmonyLib;
using Ignitron.Aluminium.Assets;
using Ignitron.Aluminium.Assets.Descriptors;
using Ignitron.Aluminium.Assets.Providers;
using Ignitron.Aluminium.Atlases;
using Ignitron.Aluminium.Events;
using Ignitron.Aluminium.Extensions;
using Ignitron.Aluminium.Registries;
using Ignitron.Loader;

namespace Ignitron.TestMod;

public sealed class TestMod : IModEntrypoint
{
    private static Texture _awaTexture;

    public void Main(ModBox box)
    {
        Logger.Info($"mod is installed at {box.RootPath}");
        Harmony harmony = new("danilwhale.Ignitron.TestMod");
        harmony.PatchAll();

        AssetManager ass = new(Path.Join(box.RootPath, AssetManager.RootDirectory));
        AluminiumRegistries.Translators.Register(box.Metadata.Id, new TestTranslator(ass));

        ClientLoopEvents.Loading += _ =>
        {
            _awaTexture = ass.Load(
                Path.Join(AssetManager.TexturesDirectory, "awa.png"),
                new TextureAssetDescriptor(Flip: false, Clamp: true, Mipmaps: false),
                TextureAssetProvider.Default);
        };

        ClientLoopEvents.Loaded += _ =>
        {
            ImageSprite sprite = ass.Load(Path.Join(AssetManager.TexturesDirectory, "awa.png"), SpriteAssetProvider.Default);
            BlockExtensions.Add(() => new Block("mao")
                .MakeSolid()
                .SetTexture(TextureAtlases.Blocks.AddSprite("mao", sprite))
                .SetItemSprite(TextureAtlases.Items.AddSprite("mao", sprite))
                .SetCategory([ItemCategory.technical])
                .SetMaterial(BlockMaterial.dirt));
            ItemExtensions.Add(() => ItemExtensions.FromSprite(TextureAtlases.Items.AddSprite("awa", sprite), "awa")
                .SetStackSize(1)
                .SetCategory([ItemCategory.technical]));
        };

        ClientLoopEvents.LoadingRendered += (_, _) =>
        {
            TextureBatcher.batcher.Start(_awaTexture);
            TextureBatcher.batcher.AddQuadScaled(20, 20, 200, 32, 0, 0, 16, 16, 2, TextureBatcher.colorWhite);
            TextureBatcher.batcher.Finalise();
            TextureBatcher.batcher.DrawBatch();
        };

        WorldEvents.Loaded += (_, player) =>
        {
            Logger.Info($"loaded {player.uniqueCharID}");
        };

        PlayerEvents.Spawned += (player) =>
        {
            Logger.Info($"double new player alert: {player.uniqueCharID}");
        };
    }
}