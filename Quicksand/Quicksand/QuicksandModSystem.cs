using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;

namespace Quicksand;

public class QuicksandModSystem : ModSystem
{
    private ICoreClientAPI capi;
    //Commenting out blockgen stuff for now
    private ICoreServerAPI sapi;
    private bool clientBlind = false;
    private IBlockAccessor chunkGenBlockAccessor;
    private IBlockAccessor worldBlockAccessor;
    
    private const int MAX_CHESTS_PER_CHUNK = 1;
    private const float CHEST_SPAWN_PROBABILITY = 0.80f;
    private int chunkSize;
    private ISet<string> treeTypes;
    
    
    public override void Start(ICoreAPI api)
    {
        base.Start(api);
        api.RegisterBlockClass("BlockQuicksand", typeof(BlockQuicksand));
        BlockQuicksand.vigorEnabled = api.ModLoader.IsModEnabled("vigor");
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        base.StartClientSide(api);
        capi = api;
    }
    
    public override void StartServerSide(ICoreServerAPI api)
    {
        base.StartServerSide(api);
        sapi = api;
        sapi.Event.ChunkColumnGeneration(OnChunkColumnGeneration, EnumWorldGenPass.PreDone, "standard");
        sapi.Event.GetWorldgenBlockAccessor(OnWorldGenBlockAccessor);

    }

    private void OnChunkColumnGeneration(IChunkColumnGenerateRequest request)
    {
        
    }

    private void OnWorldGenBlockAccessor(IChunkProviderThread chunkProvider)
    {
        chunkGenBlockAccessor = chunkProvider.GetBlockAccessor(true);
    }
}