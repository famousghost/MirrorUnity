Shader "Unlit/Mirror"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _MirrorTexture;
            float4 _MirrorTexture_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MirrorTexture);
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 screenUv = i.screenPos.xy / i.screenPos.w;
                float4 col = lerp(tex2D(_MainTex, screenUv), tex2D(_MirrorTexture, i.uv), 0.5f);
                return col;
            }
            ENDCG
        }
    }
}
