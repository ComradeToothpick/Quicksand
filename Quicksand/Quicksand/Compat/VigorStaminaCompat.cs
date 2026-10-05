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
    internal static bool IsExhausted(ICoreAPI api, Entity entity)
    {
        if (!api.ModLoader.IsModEnabled("vigor")) return false;
        return InternalIsExhausted(entity);
    }
    
    [MethodImpl( MethodImplOptions.NoInlining)]
    private static bool InternalIsExhausted(Entity entity)
    {
        EntityBehaviorVigorStamina? behavior = (EntityBehaviorVigorStamina?) entity.GetBehavior("vigorstamina");
        return behavior?.IsExhausted ?? false;
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
        VigorModSystem modSystem = api.ModLoader.GetModSystem<VigorModSystem>();
        
        if (shouldDrain)
        {
            modSystem.ClientAPI
                .StartStaminaDrain(player, player.PlayerUID + ".quicksand", 5f);
        }
        else
        {
            modSystem.ClientAPI.StopStaminaDrain(player, player.PlayerUID + ".quicksand");
        }
            
    }
}