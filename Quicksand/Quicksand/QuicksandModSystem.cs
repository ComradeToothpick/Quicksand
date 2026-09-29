using Vintagestory.API.Common;

namespace Quicksand;

public class QuicksandModSystem : ModSystem
{
    public override void Start(ICoreAPI api)
    {
        base.Start(api);
        api.RegisterBlockClass("BlockQuicksand", typeof(BlockQuicksand));
    }
}
