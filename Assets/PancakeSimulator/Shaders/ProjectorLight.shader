// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ProjectorLight" {
	Properties{
		_MainTex("MainTexture", 2D) = "white" {}
		_FalloffTex("FallOff", 2D) = "white" {}
	}

	Subshader{
		Tags{ "Queue" = "Geometry" }
		Pass{
			ZWrite Off
			Blend One One
			

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"

			struct v2f {
				float4 uv0 : TEXCOORD0;
				float4 uvFalloff : TEXCOORD1;
				UNITY_FOG_COORDS(2)
				float4 pos : SV_POSITION;
			};

			float4x4 unity_Projector;
			float4x4 unity_ProjectorClip;

			v2f vert(float4 vertex : POSITION)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				o.uv0 = mul(unity_Projector, vertex);
				o.uvFalloff = mul(unity_ProjectorClip, vertex);
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}

			sampler2D _MainTex;
			sampler2D _FalloffTex;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 mainColor = tex2Dproj(_MainTex, UNITY_PROJ_COORD(i.uv0));
				//texS.a = 1.0-texS.a;

				fixed4 fColor = tex2Dproj(_FalloffTex, UNITY_PROJ_COORD(i.uvFalloff));
				//lerp(fixed4(1, 1, 1, 0), texS, texF.a);
				fixed4 res = mainColor * fColor;

				return res;
			}
			ENDCG
		}
	}
}