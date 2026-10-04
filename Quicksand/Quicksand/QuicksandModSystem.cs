using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;

namespace Quicksand;

public class QuicksandModSystem : ModSystem
{
    private ICoreClientAPI capi;
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
}