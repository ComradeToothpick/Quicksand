using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace Quicksand.Compat;

internal static class StaminaCompat
{
    private const string Action = "quicksand";

    internal static bool IsExhausted(ICoreAPI api, Entity entity)
    {
        var tree = Push(api, entity, "playeraction:check", tree => tree.SetBool("continuous", true));
        return tree != null && !tree.GetBool("allowed", true);
    }

    internal static void StaminaCost(ICoreAPI api, Entity entity, bool shouldDrain)
    {
        if (!shouldDrain)
        {
            Push(api, entity, "playeraction:stopped", null);
            return;
        }

        Push(api, entity, "playeraction:started", tree =>
        {
            tree.SetFloat("stamina", QuicksandModSystem.Config.struggleDrain);
            tree.SetString("category", "Block");
        });
    }

    private static ITreeAttribute? Push(ICoreAPI api, Entity entity, string eventName, System.Action<ITreeAttribute>? fill)
    {
        if (entity is not EntityPlayer player || string.IsNullOrEmpty(player.PlayerUID)) return null;

        var tree = new TreeAttribute();
        tree.SetString("playeruid", player.PlayerUID);
        tree.SetString("action", Action);
        fill?.Invoke(tree);
        api.Event.PushEvent(eventName, tree);
        return tree;
    }
}