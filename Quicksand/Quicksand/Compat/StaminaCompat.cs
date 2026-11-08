using System.Runtime.CompilerServices;
using Stamina;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace Quicksand.Compat;

internal static class StaminaCompat
{
    internal static bool IsExhausted(ICoreAPI api, Entity entity)
    {
        if (!api.ModLoader.IsModEnabled("stamina")) return false;
        return InternalIsExhausted(api, entity);
    }
    
    [MethodImpl( MethodImplOptions.NoInlining)]
    private static bool InternalIsExhausted(ICoreAPI api, Entity entity)
    {
        StaminaModSystem? modSystem = api.ModLoader.GetModSystem<StaminaModSystem>();
        if (modSystem == null) return false;
        
        return StaminaApi.IsExhausted(entity);
    }
    
    internal static bool StaminaCost(ICoreAPI api, Entity entity, bool shouldDrain)
    {
        if (!api.ModLoader.IsModEnabled("stamina")) return false;
        InternalStaminaCost(api, entity, shouldDrain);
        return true;
    }
    
    [MethodImpl( MethodImplOptions.NoInlining)]
    private static void InternalStaminaCost(ICoreAPI api, Entity entity, bool shouldDrain)
    {
        StaminaModSystem? modSystem = api.ModLoader.GetModSystem<StaminaModSystem>();
        if (modSystem == null) return;
        
        if (shouldDrain)
        {
            StaminaApi.StartDrain(entity, entity.EntityId + ".quicksand", QuicksandModSystem.Config.struggleDrain);
        }
        else
        {
            StaminaApi.StopDrain(entity, entity.EntityId + ".quicksand");
        }
    }
}