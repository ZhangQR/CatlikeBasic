Shader "Graph/PointShaderGPU"
{
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
                float4 vertex : SV_POSITION;
                float4 worldPosition : TEXCOORD0;
            };

            StructuredBuffer<float3> _Buffer;
            float _Scale;

            v2f vert (appdata v,uint instanceID : SV_InstanceID)
            {
                v2f o;
                float4 vertex = mul(unity_WorldToObject,float4(_Buffer[instanceID],1));
                float4x4 scaleMatrix = float4x4(float4(_Scale,0,0,0),float4(0,_Scale,0,0),float4(0,0,_Scale,0),float4(0,0,0,1));
                float4 scalePosition = mul(scaleMatrix,v.vertex);
                o.vertex = UnityObjectToClipPos(vertex + scalePosition);
                o.worldPosition = mul(unity_ObjectToWorld,scalePosition);
                o.worldPosition = o.worldPosition + float4(_Buffer[instanceID].xyz,1);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 col = i.worldPosition.xyz * 0.5 + 0.5;
                return fixed4(col,1);
            }
            ENDCG
        }
    }
}
