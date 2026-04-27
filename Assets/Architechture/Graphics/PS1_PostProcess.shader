Shader "Hidden/PS1_PostProcess"
{
    Properties
    {
        // Эти строки создают ползунки в Инспекторе материала
        _DitherStrength("Dither Strength", Range(0, 1)) = 0.05
        _ColorDepth("Color Depth (Steps)", Range(2, 64)) = 20
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZTest Always ZWrite Off Cull Off

        Pass
        {
            Name "PS1PostProcess"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            // Описываем переменные для связи с блоком Properties
            float _DitherStrength;
            float _ColorDepth;

            static const float4x4 bayerMatrix = float4x4(
                0.0, 8.0, 2.0, 10.0,
                12.0, 4.0, 14.0, 6.0,
                3.0, 11.0, 1.0, 9.0,
                15.0, 7.0, 13.0, 5.0
            ) / 16.0;

            half4 Frag (Varyings input) : SV_Target
            {
                float2 uv = input.texcoord;
                half4 color = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv);

                // Дизеринг
                uint2 pixelPos = uint2(uv * _ScreenParams.xy);
                float dither = bayerMatrix[pixelPos.x % 4][pixelPos.y % 4];

                // Используем ползунок силы дизеринга
                color.rgb += (dither - 0.5) * _DitherStrength;

                // Квантование цвета (используем ползунок шагов)
                color.rgb = floor(color.rgb * _ColorDepth) / _ColorDepth;

                return color;
            }
            ENDHLSL
        }
    }
}