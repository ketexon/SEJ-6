Shader "Hidden/Custom/Spin"
{
    HLSLINCLUDE 
#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        // Lerp the pixel color with the luminance using the _Blend uniform.
        float _Intensity;
        float4 _Color1;
        float4 _Color2;


        float4 Frag(VaryingsDefault i) : SV_Target
        {
            // the colors use a gaussian function
            // the sigmainv is the factor of the -x^2 in exp(sigma*-x^2)
            const float colorSigmaInv = 3;
            const float spinIntensityMult = 0.1;
            // the spin rotates about the center with more intensity the farther from the center
            float2 centerOffset = i.texcoord - float2(0.5, 0.5);
            float distanceFromCenter = length(centerOffset);
            float theta = atan2(centerOffset.y, centerOffset.x);
            float newTheta = theta + 6.14 * _Intensity * spinIntensityMult * distanceFromCenter;
            float2 spunTexCoord = distanceFromCenter * float2(cos(newTheta), sin(newTheta)) + float2(0.5, 0.5);
            float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, spunTexCoord);
            //color.rgb = lerp(color.rgb, float3(i.texcoord.x, 0, 0), _Intensity.xxx);
            float color1Theta = _Time[1] * 6.28 / 4;
            float color2Theta = _Time[1] * 6.28 / 4.5;
            float2 color1Pivot = float2(0.5, 0.5) + 0.25 * float2(cos(color1Theta), sin(color1Theta));
            float2 color2Pivot = float2(0.5, 0.5) + 0.25 * float2(sin(color2Theta), cos(color2Theta));
            float color1Distance = length(spunTexCoord - color1Pivot);
            float color2Distance = length(spunTexCoord - color2Pivot);
            float3 color1Lerp = lerp(color.rgb, _Color1.rgb, exp(-color1Distance * color1Distance * colorSigmaInv) * abs(_Intensity));
            float3 color2Lerp = lerp(color.rgb, _Color2.rgb, exp(-color2Distance * color2Distance * colorSigmaInv) * abs(_Intensity));
            color.rgb = (color1Lerp + color2Lerp) / 2;
            return color;
        }
    ENDHLSL
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment Frag
            ENDHLSL
        }
    }
}
