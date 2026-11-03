// Overlay de tela CRT para UI: scanlines, vinheta, flicker e o efeito de "ligar o tubo".
// Vai numa Image por cima de todo o conteúdo da tela, com raycast desligado.
// O tempo vem do script CrtScreen (_UnscaledTime), porque com Time.timeScale = 0 o _Time do shader para.
Shader "UI/CrtScreen"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _OffColor ("Tubo desligado", Color) = (0.012, 0.07, 0.04, 1)
        _GlowColor ("Brilho ao ligar", Color) = (0.75, 1, 0.84, 1)
        _ScanlinePeriod ("Período das scanlines (px)", Float) = 3
        _ScanlineStrength ("Força das scanlines", Range(0, 1)) = 0.3
        _VignetteStrength ("Força da vinheta", Range(0, 1)) = 0.85
        _VignettePower ("Suavidade da vinheta", Range(0.05, 2)) = 0.35
        _Flicker ("Flicker", Range(0, 0.2)) = 0.03
        _PowerOn ("Ligado", Range(0, 1)) = 1
        _UnscaledTime ("Tempo (script)", Float) = 0

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            fixed4 _Color;
            fixed4 _OffColor;
            fixed4 _GlowColor;
            float _ScanlinePeriod;
            float _ScanlineStrength;
            float _VignetteStrength;
            float _VignettePower;
            float _Flicker;
            float _PowerOn;
            float _UnscaledTime;

            v2f vert(appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }

            float hash(float n)
            {
                return frac(sin(n) * 43758.5453);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Ligar o tubo: primeiro um ponto vira linha horizontal, depois a linha abre na vertical.
                float widen = smoothstep(0.0, 0.35, _PowerOn);
                float open = smoothstep(0.35, 1.0, _PowerOn);
                float2 halfSize = float2(lerp(0.004, 0.5, widen), lerp(0.003, 0.5, open));
                float2 d = abs(uv - 0.5);
                float inside = step(d.x, halfSize.x) * step(d.y, halfSize.y);

                // Scanline de 1px a cada _ScanlinePeriod px, em pixels de tela pra ficar nítida em qualquer escala.
                float scan = step(fmod(i.vertex.y, _ScanlinePeriod), 1.0) * _ScanlineStrength;

                float2 v = uv * (1.0 - uv);
                float vignette = (1.0 - saturate(pow(v.x * v.y * 16.0, _VignettePower))) * _VignetteStrength;

                float flicker = hash(floor(_UnscaledTime * 24.0)) * _Flicker;

                float darkness = saturate(scan + vignette + flicker);
                fixed4 screen = fixed4(0, 0, 0, darkness);

                // Enquanto abre, a faixa acesa brilha forte e vai apagando até o conteúdo aparecer.
                float glow = (1.0 - open) * inside;
                screen = lerp(screen, fixed4(_GlowColor.rgb, 1), glow);

                fixed4 col = lerp(_OffColor, screen, inside);
                col.a *= i.color.a;
                return col;
            }
            ENDCG
        }
    }
}
