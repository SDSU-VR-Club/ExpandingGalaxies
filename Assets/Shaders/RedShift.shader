Shader "Custom/RedShift"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CurrentDepthTexture("Current Depth", 2D) = "white" {}
        _LastDepthTexture("Last Depth", 2D) = "white" {}
	}
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			sampler2D _CurrentDepthTexture;
            sampler2D _LastDepthTexture;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float depth = tex2D(_CurrentDepthTexture, i.uv).rgb;
                float lastDepth = tex2D(_LastDepthTexture, i.uv).rgb;

                float diff = (depth - lastDepth);
                               
                col.r += diff;
                col.g -= diff;
                col.b -= diff;

                return 1-diff;
            }
            ENDCG
        }
    }
}
