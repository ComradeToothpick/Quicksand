using Vigor.Behaviors;
using System.Reflection;
using System.Runtime.CompilerServices;
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
}