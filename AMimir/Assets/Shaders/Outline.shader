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
            #pragma vertex SpriteVert
            #pragma fragment FaceFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"
            
            float4 _MainTex_TexelSize;

            fixed4 FaceFrag(v2f IN) : SV_Target
            {
                float width = 10;
                fixed4 color = _Color;
                fixed4 center = SampleSpriteTexture(IN.texcoord);
                fixed4 up = SampleSpriteTexture(IN.texcoord + fixed2(0,_MainTex_TexelSize.y*width));
                fixed4 down = SampleSpriteTexture(IN.texcoord + fixed2(0,-_MainTex_TexelSize.y*width));
                fixed4 left = SampleSpriteTexture(IN.texcoord + fixed2(-_MainTex_TexelSize.x*width,0));
                fixed4 right = SampleSpriteTexture(IN.texcoord + fixed2(_MainTex_TexelSize.x*width,0));

                fixed alpha = (center.a * 0.5 + clamp(center.a - up.a * down.a * left.a * right.a, 0, 1))/2;

                color.a = alpha;
                color.rgb *= color.a;
                return color;
            }
            ENDCG
        }
    }
}