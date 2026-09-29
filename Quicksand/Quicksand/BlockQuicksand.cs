using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace Quicksand;

public class BlockQuicksand : Block
{
    private float stillSinkSpeed = 0.002f;
    private float maxSinkSpeed = 0.007f;
    private float climbSpeed = 0.004f;
    private float maxHorizontalSpeed = 0.03f;
    private float friction = 0.99f;

    public override void OnLoaded(ICoreAPI api)
    {
        base.OnLoaded(api);

        if (Attributes == null) return;

        stillSinkSpeed = Attributes["stillSinkSpeed"].AsFloat(stillSinkSpeed);
        maxSinkSpeed = Attributes["maxSinkSpeed"].AsFloat(maxSinkSpeed);
        climbSpeed = Attributes["climbSpeed"].AsFloat(climbSpeed);
        maxHorizontalSpeed = Attributes["maxHorizontalSpeed"].AsFloat(maxHorizontalSpeed);
    }

    public override void OnEntityInside(IWorldAccessor world, Entity entity, BlockPos pos)
    {
        base.OnEntityInside(world, entity, pos);

        if (entity == null || !entity.Alive) return;
        
        EntityControls? controls = (entity as EntityAgent)?.Controls;
        if (controls != null && (controls.NoClip || controls.IsFlying || controls.DetachedMode)) return;

        bool climbing = controls != null && controls.Jump;
        bool wading = controls != null && controls.TriesToMove;
        double targetVerticalSpeed = climbing ? climbSpeed : -SinkSpeed(controls);

        Vec3d motion = entity.Pos.Motion;

        motion.Y = wading ? 2 * -stillSinkSpeed : targetVerticalSpeed;
        if (entity.ApplyGravity)
        {
            float gravityTick = world.Side == EnumAppSide.Client ? 1f / 60f : GlobalConstants.PhysicsFrameTime;
            motion.Y += GlobalConstants.GravityPerSecond * gravityTick;
        }
        entity.OnGround = false;
        double horLength = Math.Sqrt(motion.X * motion.X + motion.Z * motion.Z);
        if (climbing ? horLength > maxHorizontalSpeed / 4 : horLength > maxHorizontalSpeed)
        {
            double scale = climbing ? maxHorizontalSpeed / (4 * horLength) : maxHorizontalSpeed / horLength;
            motion.X *= climbing ? scale / 2 : scale;
            motion.Z *= climbing ? scale / 2 : scale;
        }
        motion.X *= friction;
        motion.Z *= friction;

        if (world.Side == EnumAppSide.Server && IsEyeInside(entity, pos))
        {
            EntityBehaviorBreathe? breathe = entity.GetBehavior<EntityBehaviorBreathe>();
            if (breathe != null) breathe.HasAir = false;
        }
    }

    private static bool IsEyeInside(Entity entity, BlockPos pos)
    {
        int eyeX = (int)(entity.Pos.X + entity.LocalEyePos.X);
        int eyeY = (int)(entity.Pos.InternalY + entity.LocalEyePos.Y);
        int eyeZ = (int)(entity.Pos.Z + entity.LocalEyePos.Z);

        return pos.X == eyeX && pos.InternalY == eyeY && pos.Z == eyeZ;
    }

    private double SinkSpeed(EntityControls? controls)
    {
        double struggle = 0;
        if (controls != null && controls.TriesToMove) struggle = controls.Sprint ? 1.0 : 0.5;
        return stillSinkSpeed + (maxSinkSpeed - stillSinkSpeed) * struggle;
    }

    public override bool TryPlaceBlockForWorldGen(IBlockAccessor blockAccessor, BlockPos pos, BlockFacing onBlockFace, IRandom worldgenRandom, BlockPatchAttributes? attributes = null)
    {
        BlockPos surfacePos = pos.DownCopy();
        Block surfaceBlock = blockAccessor.GetBlock(surfacePos);
        if (surfaceBlock == null) return false;

        if (surfaceBlock.BlockMaterial != EnumBlockMaterial.Sand && surfaceBlock.BlockMaterial != EnumBlockMaterial.Gravel)
        {
            return false;
        }

        blockAccessor.SetBlock(BlockId, surfacePos);
        return true;
    }
}
