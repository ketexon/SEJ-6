using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(SpinRenderer), PostProcessEvent.AfterStack, "Custom/Spin")]
public sealed class Spin : PostProcessEffectSettings
{
    [Range(-1f, 1f), Tooltip("Spin effect intensity.")]
    public FloatParameter intensity = new FloatParameter { value = 0.5f };

    [ColorUsage(showAlpha: false), Tooltip("Color 1")]
    public ColorParameter color1 = new ColorParameter { value = Color.red };

    [ColorUsage(showAlpha: false), Tooltip("Color 2")]
    public ColorParameter color2 = new ColorParameter { value = Color.blue };
}
public sealed class SpinRenderer : PostProcessEffectRenderer<Spin>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Spin"));
        sheet.properties.SetFloat("_Intensity", settings.intensity);
        sheet.properties.SetColor("_Color1", settings.color1);
        sheet.properties.SetColor("_Color2", settings.color2);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}