Shader "Vanessa/Objects"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

        // Shadow
        [ToggleOff(SHADOW_ON)] _ShadowOn ("Shadow Enabled", Float) = 1
        _ShadowColor ("Shadow Color", Color) = (0,0,0,1)
        [PerRendererData] _ShadowOffset("Shadow Offset", Float) = 1

        // Breathing
        [ToggleOff(BREATHING_ON)] _BreathingOn ("Breathing Enabled", Float) = 1
        _BreathAmplitude("Breath Amplitude", Float) = 0
        _BreathSpeed("Breath Speed", Float) = 0
        [PerRendererData] _BreathNoise("Shadow Noise", float) = 0

        // Color Gradient
        [ToggleOff(COLOR_GRADIENT_ON)] _ColorGradientOn ("Color Gradient Enabled", Float) = 0
        _GradientLight ("Gradient Light", Color) = (1, 1, 1, 1)
        _GradientShadow ("Gradient Shadow", Color) = (0, 0, 0, 1)
        
        [PerRendererData] _OverlayTex ("Overlay Sprite Texture", 2D) = "black" {}
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma multi_compile_local _ SHADOW_ON
            #pragma vertex ShadowVert
            #pragma fragment ShadowFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #pragma multi_compile_local _ BREATHING_ON
            #pragma multi_compile_local _ COLOR_GRADIENT_ON
            #include "UnitySprites.cginc"
            #include "ShaderMaths.cginc"

            float4 _ShadowColor;
            float _ShadowOffset;

            #ifndef BREATHING_ON
            float _BreathAmplitude;
            float _BreathSpeed;
            float _BreathNoise;
            #endif

            v2f ShadowVert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = IN.vertex;

                #ifndef SHADOW_ON
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.vertex = UnityFlipSprite(IN.vertex
                                             #ifndef BREATHING_ON
                                             * get_scaling(_Time.y + _BreathNoise, _BreathAmplitude, _BreathSpeed)
                                             #endif
                                             , _Flip);
                OUT.vertex = UnityObjectToClipPos(OUT.vertex);
                OUT.vertex += mul(unity_CameraProjection,
                                  float4(-0.1 * _ShadowOffset, _ProjectionParams.x * -0.1 * _ShadowOffset, 0, 0));
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color * _RendererColor;

                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif
                #endif

                return OUT;
            }

            fixed4 ShadowFrag(v2f IN) : SV_Target
            {
                #ifndef SHADOW_ON
                fixed4 c = SampleSpriteTexture(IN.texcoord).a * _ShadowColor;
                c.rgb *= c.a;
                return c;
                #else
                return fixed4(0,0,0,0);
                #endif
            }
            ENDCG
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex FaceVert
            #pragma fragment FaceFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #pragma multi_compile_local _ BREATHING_ON
            #pragma multi_compile_local _ COLOR_GRADIENT_ON
            #include "UnitySprites.cginc"
            #include "ShaderMaths.cginc"

            #ifndef BREATHING_ON
            float _BreathAmplitude;
            float _BreathSpeed;
            float _BreathNoise;
            #endif

            v2f FaceVert(appdata_t IN)
            {
                v2f OUT;

                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.vertex = UnityFlipSprite(IN.vertex
                                             #ifndef BREATHING_ON
                                             * get_scaling(_Time.y + _BreathNoise, _BreathAmplitude, _BreathSpeed)
                                             #endif
                                             , _Flip);
                OUT.vertex = UnityObjectToClipPos(OUT.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color * _RendererColor;

                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            #ifndef COLOR_GRADIENT_ON
            float4 _GradientLight;
            float4 _GradientShadow;
            #endif

            sampler2D _OverlayTex;

            fixed4 FaceFrag(v2f IN) : SV_Target
            {
                #ifndef COLOR_GRADIENT_ON
                fixed4 tex_color = SampleSpriteTexture(IN.texcoord);
                fixed4 overlay_color = tex2D (_OverlayTex, IN.texcoord);
                const int count = 3;
                const fixed4 colors[count] = {_GradientShadow, IN.color, _GradientLight};
                const fixed scaled_time = tex_color.r * (count - 1);
                int time_index = floor(scaled_time);
                const fixed time = scaled_time - time_index;
                fixed4 color = lerp(colors[time_index], colors[time_index + 1], time);

                fixed4 c;
                c.a = tex_color.a;
                c.rgb = lerp(color.rgb, overlay_color.rgb, overlay_color.a);
                c.rgb *= tex_color.a;
                return c;
                #else
                return SpriteFrag(IN);
                #endif
            }
            ENDCG
        }
    }
}