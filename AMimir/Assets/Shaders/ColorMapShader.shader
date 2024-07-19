Shader "Vanessa/SpritesWithShadow"
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
        _ShadowColor ("Shadow Color", Color) = (0,0,0,1)
        _ShadowOffset ("Shadow Offset", Float) = 1
        _LightColor ("Light Color", Color) = (1, 1, 1, 1)
        _DarkColor ("Dark Color", Color) = (0, 0, 0, 1)
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
            #pragma vertex ShadowVert
            #pragma fragment ShadowFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

            float4 _ShadowColor;
            float _ShadowOffset;

            v2f ShadowVert(appdata_t IN)
            {
                v2f OUT;

                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
                OUT.vertex = UnityObjectToClipPos(OUT.vertex);
                OUT.vertex += mul(unity_CameraProjection,
                                  float4(-0.1 * _ShadowOffset, _ProjectionParams.x * -0.1 * _ShadowOffset, 0, 0));
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color * _RendererColor;

                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            fixed4 ShadowFrag(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture(IN.texcoord).a * _ShadowColor;
                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment ColorMapFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

            float4 _LightColor;
            float4 _DarkColor;
            
            fixed4 ColorMapFrag(v2f IN) : SV_Target
            {
                fixed4 tex_color = SampleSpriteTexture(IN.texcoord);
                const int count = 3;
                const fixed4 colors[count] = {_DarkColor, IN.color, _LightColor};
                const fixed scaled_time = tex_color.r * (count-1);
                int time_index = floor(scaled_time);
                const fixed time = scaled_time - time_index;
                fixed4 color = lerp(colors[time_index], colors[time_index+1], time);
                
                fixed4 c;
                c.a = tex_color.a;
                c.rgb = color.rgb;
                c.rgb *= tex_color.a;
                return c;
            }
            ENDCG
        }
    }
}