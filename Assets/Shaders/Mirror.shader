Shader "Unlit/Mirror"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "black"{}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float3 normal : TEXCOORD3;
            };

            sampler2D _MainTex;
            sampler2D _MirrorTexture;
            float4 _MirrorTexture_ST;
            sampler2D _NormalMap;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv = TRANSFORM_TEX(v.uv, _MirrorTexture);
                o.screenPos = ComputeScreenPos(o.vertex);
                float3x3 matrixModelInv = unity_WorldToObject;
                o.normal = normalize(mul(v.normal, matrixModelInv));
                return o;
            }

            float diffuse(float3 lightDir, float3 normal)
            {
                return saturate(dot(lightDir, normal));
            }

            float specular(float3 lightDir, float3 camDir, float3 normal, float strength)
            {
                float3 reflLightDir = reflect(lightDir, normal);
                return pow(saturate(dot(reflLightDir, camDir)), strength);
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 screenUv = i.screenPos.xy / i.screenPos.w;
                float4 col = lerp(tex2D(_MainTex, screenUv), tex2D(_MirrorTexture, i.uv), 0.5f) * (diffuse(_WorldSpaceLightPos0, i.normal) + specular(_WorldSpaceLightPos0, normalize(i.worldPos - _WorldSpaceCameraPos), i.normal, 32.0f));
                return col;
            }
            ENDCG
        }
    }
}
