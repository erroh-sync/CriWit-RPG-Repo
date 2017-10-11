Shader "CrimWitt/GUI/Quickstats Bar"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BGTex("GridTexture", 2D) = "white" {}
		_BorderTex("BorderTexture", 2D) = "white" {}
		_Color("Tint", Color) = (1.000000,1.000000,1.000000,1.000000)
		_Brightness("Brightness", Float) = 3
		_barPulseSpeed("Bar Pulse Speed", Float) = 8
		_barScrollSpeed("Bar Scroll Speed", Float) = 2
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 200

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

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
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _BGTex;
			sampler2D _BorderTex;
			float4 _MainTex_ST;
			float4 _Color;
			float _Brightness;
			float _barPulseSpeed;
			float _barScrollSpeed;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// The Mask
				fixed2 maskUV = i.uv;
				maskUV.x += -_Time * _barPulseSpeed;

				// The Bar Part
				fixed2 barUV = i.uv;
				barUV.x += -_Time * _barScrollSpeed;
				fixed4 barcol = tex2D(_MainTex, barUV) * tex2D(_BorderTex, maskUV).g;
				barcol.a *= tex2D(_BorderTex, i.uv).b;

				// Finalisation
				fixed4 col = lerp(barcol, tex2D(_BGTex, i.uv), 1 - barcol.a);
				col.a = tex2D(_BorderTex, i.uv).r;
				col *= _Color;
				return col;
			}
			ENDCG
		}
	}
}
