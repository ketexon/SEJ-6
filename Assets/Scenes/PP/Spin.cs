using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(SpinRenderer), PostProcessEvent.AfterStack, "Custom/Spin")]
public sealed class Spin : PostProcessEffectSettings
{
    [Range(-1f, 1f), Tooltip("Spin effect intensity.")]
    public FloatParameter intensity = new FloatParameter { value = 0.5f };
}
public sealed class SpinRenderer : PostProcessEffectRenderer<Spin>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Spin"));
        sheet.properties.SetFloat("_Intensity", settings.intensity);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}