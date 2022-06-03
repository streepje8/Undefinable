Shader "Unlit/StencilFilter"
{
    Properties{
        [IntRange]_PortalID("Filter ID", Range(0,255)) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry-1"}
        LOD 100
        ColorMask 0
        Blend Zero One
        ZWrite Off

        Stencil {
            Ref [_PortalID]
            Comp Always
            Pass replace
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return 0;
            }
            ENDCG
        }
    }
}
