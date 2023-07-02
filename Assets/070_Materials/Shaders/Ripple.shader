Shader "Custom/Ripple" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Frequency ("Frequency", Range(0.1, 10)) = 1
        _Amplitude ("Amplitude", Range(0.1, 10)) = 1
        _Speed ("Speed", Range(0.1, 10)) = 1
        _Transparency ("Transparency", Range(0, 1)) = 1
    }
    
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma alpha:fade

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Frequency;
            float _Amplitude;
            float _Speed;
            float _Transparency;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // calculate ripple
                float2 ripple = i.uv;
                float dist = distance(i.uv, float2(0.5, 0.5));
                ripple += sin(dist * _Frequency - _Time * _Speed) * _Amplitude * dist;

                // sample texture using ripple uv
                fixed4 col = tex2D(_MainTex, ripple);
                col.a *= _Transparency;
                return col;
            }
            ENDCG
        }
    } 
    FallBack "Diffuse"
}