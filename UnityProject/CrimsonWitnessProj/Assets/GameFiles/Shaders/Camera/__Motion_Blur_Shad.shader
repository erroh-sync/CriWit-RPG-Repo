Shader "CrimWitt/ScreenFX/Motion Blur"
{
	Properties
	{
		_MainTex ("_MainTexture", 2D) = "white" {}
		_BufferTex ("_BufferTexture", 2D) = "white" {}
		_BlurFactor("_BlurFactor", float) = 0.35
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
			sampler2D _BufferTex;
			float _BlurFactor;

			fixed4 frag (v2f i) : SV_Target
			{ 
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 bufcol = tex2D(_BufferTex, i.uv);

				//return col + (bufcol * _BlurFactor * 0.15);
				return lerp(col,bufcol, _BlurFactor) + (_BlurFactor * 0.08);
			}
			ENDCG
		}
	}
}
