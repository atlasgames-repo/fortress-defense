Shader "Hero Editor/Hue Paint"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [PerRendererData] _Color ("Color", Color) = (1,1,1,1)
        [PerRendererData] _Hue ("Hue", float) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            struct appdata
            {
                float2 uv : TEXCOORD0;
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID // use this to access instanced properties in the fragment shader.
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
                UNITY_DEFINE_INSTANCED_PROP(float, _Hue)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                return o;
            }

            float3 ApplyHue(float3 aColor, float aHue)
            {
                float angle = radians(aHue * 360);
                float3 k = float3(0.57735, 0.57735, 0.57735);
                float cosAngle = cos(angle);
                return aColor * cosAngle + cross(k, aColor) * sin(angle) + k * dot(k, aColor) * (1 - cosAngle);
            }
            
            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
                col.rgb = ApplyHue(col.rgb, UNITY_ACCESS_INSTANCED_PROP(Props, _Hue));
                return col;
            }
            ENDCG
        }
    }
}
