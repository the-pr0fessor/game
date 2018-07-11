Shader "Custom/ScreenBlend" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}
		_OtherTex("Other Texture", 2D) = "white" {}
		_BlendValue("Blend Value", Float) = 0.5
		_Direction("Direction", Int) = 1
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _OtherTex;
			float _BlendValue;
			int _Direction;

			float4 frag(v2f_img i) : COLOR{
				// Fade
				/*float4 other;
				other = tex2D(_OtherTex, i.uv);

				float4 main;
				main = tex2D(_MainTex, i.uv);

				float4 res = (_BlendValue * main) + ((1 - _BlendValue) * other);*/

				float4 res;
				/*res.x = i.uv.x;
				res.y = i.uv.y;
				res.z = 0.0f;
				res.w = 1.0f;*/

				float x = i.uv.x;
				float y = i.uv.y;
				//x = x * x;
				//y = y * y;
				// x + y for circle

				if (_Direction == -1) {
					y = 1.0f - y;
				}

				if (y > _BlendValue) {
					res = tex2D(_OtherTex, i.uv);
				}
				else {					
					res = tex2D(_MainTex, i.uv);
				}

				return res;
			}
		ENDCG
		}
	}
}