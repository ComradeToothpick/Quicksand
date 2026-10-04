using Vintagestory.API.Client;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

namespace Quicksand;

public class QuicksandOverlay : GuiDialog
{
    public override string ToggleKeyCombinationCode => "quicksandblindness";
    
    public override EnumDialogType DialogType => EnumDialogType.HUD;

    public override bool Focusable => false;

    public override bool PrefersUngrabbedMouse => false;

    public override double DrawOrder => -0.04;
    
    private float alpha = 1f;

    public QuicksandOverlay(ICoreClientAPI capi) : base(capi)
    {
        SetupDiaglog();
    }

    private void SetupDiaglog()
    {
        ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(EnumDialogArea.CenterBottom);
        ElementBounds textBounds = ElementBounds.Fixed(0, 0, 30, 10);
        ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
        bgBounds.BothSizing = ElementSizing.FitToChildren;
        bgBounds.WithChildren(textBounds);
        SingleComposer = capi.Gui.CreateCompo("Quicksand", dialogBounds)//The actual GUI should be hidden behind the quickbar
            .AddShadedDialogBG(bgBounds)
            .AddStaticText("Blindness!", CairoFont.WhiteDetailText(), textBounds)
            .Compose();
    }
    
    private void RenderBlack()
    {
        if (!(capi.World is ClientMain world))
            return;
        world.Render2DTexture(world.WhiteTexture(), 0.0f, 0.0f, capi.Render.FrameWidth, capi.Render.FrameHeight, 50f, new Vec4f(0.0f, 0.0f, 0.0f, alpha));
    }
    public override void OnRenderGUI(float deltaTime)
    {
        RenderBlack();
        base.OnRenderGUI(deltaTime);
    }
}