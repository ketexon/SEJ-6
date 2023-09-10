Shader "Hidden/Custom/Spin"
{
    HLSLINCLUDE 
#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        // Lerp the pixel color with the luminance using the _Blend uniform.
        float _Intensity;

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            float2 centerOffset = i.texcoord - float2(0.5, 0.5);
            float distanceFromCenter = length(centerOffset);
            float theta = atan2(centerOffset.y, centerOffset.x);
            float newTheta = theta + 6.14 * _Intensity * distanceFromCenter;
        float2 spunTexCoord = distanceFromCenter * float2(cos(newTheta), sin(newTheta)) + float2(0.5, 0.5);
            float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, spunTexCoord);
            //color.rgb = lerp(color.rgb, float3(i.texcoord.x, 0, 0), _Intensity.xxx);
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
