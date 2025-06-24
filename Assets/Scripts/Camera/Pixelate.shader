Shader "Custom/PixelateCustom"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelSize ("Pixel Size (px)", Float) = 8
        _PixelateOffset ("Pixelate Offset", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // x=1/width, y=1/height
            float _PixelSize;
            float _PixelateOffset;

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;

                // Offset opcional para suavizar o desplazar píxeles
                float2 offset = float2(_PixelateOffset, _PixelateOffset);

                // Calcula la coordenada pixelada con offset
                float2 pixelSizeUV = _PixelSize * _MainTex_TexelSize.xy;

                float2 pixelatedUV = floor((uv + offset) / pixelSizeUV) * pixelSizeUV;

                return tex2D(_MainTex, pixelatedUV);
            }
            ENDCG
        }
    }
}
