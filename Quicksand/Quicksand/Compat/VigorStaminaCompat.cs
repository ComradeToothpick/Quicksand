using Vigor.Behaviors;
using Vigor.API;
using System.Reflection;
using System.Runtime.CompilerServices;
using Vigor;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace Quicksand.Compat;

internal static class VigorStaminaCompat
{
    internal static bool IsExhausted(ICoreAPI api, EntityPlayer player)
    {
        if (!api.ModLoader.IsModEnabled("vigor")) return false;
        return InternalIsExhausted(api, player);
    }
    
    [MethodImpl( MethodImplOptions.NoInlining)]
    private static bool InternalIsExhausted(ICoreAPI api, EntityPlayer player)
    {
        VigorModSystem? modSystem = api.ModLoader.GetModSystem<VigorModSystem>();
        if (modSystem == null) return false;
        if (modSystem.ClientAPI == null)
        {
            modSystem.ClientAPI = new VigorAPI(api);
            api.Logger.Error("Quicksand: Vigor ClientAPI instance not set");
            //return;
        }

        return modSystem.ClientAPI.IsExhausted(player);
    }
    
    internal static bool StaminaCost(ICoreAPI api, EntityPlayer player, bool shouldDrain)
    {
        if (!api.ModLoader.IsModEnabled("vigor")) return false;
        InternalStaminaCost(api, player, shouldDrain);
        return true;
    }
    
    [MethodImpl( MethodImplOptions.NoInlining)]
    private static void InternalStaminaCost(ICoreAPI api, EntityPlayer player, bool shouldDrain)
    {
        VigorModSystem? modSystem = api.ModLoader.GetModSystem<VigorModSystem>();
        if (modSystem == null) return;
        if (modSystem.ClientAPI == null)
        {
            modSystem.ClientAPI = new VigorAPI(api);
            api.Logger.Error("Quicksand: Vigor ClientAPI instance not set");
            //return;
        }
        if (shouldDrain)
        {
            modSystem.ClientAPI.StartStaminaDrain(player, player.PlayerUID + ".quicksand", QuicksandModSystem.Config.struggleDrain);
        }
        else
        {
            modSystem.ClientAPI.StopStaminaDrain(player, player.PlayerUID + ".quicksand");
        }
    }
}