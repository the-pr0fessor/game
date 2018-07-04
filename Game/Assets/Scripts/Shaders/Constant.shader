// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Constant" {
	Properties {
		_TheColour("Colour", Color) = (1, 0, 0, 1)
	}

	SubShader
	{
		
		Pass {
			CGPROGRAM
			half4 _TheColour;

			#pragma vertex vert             
			#pragma fragment frag

			struct vertInput {
				float4 pos : POSITION;
			};

			struct vertOutput {
				float4 pos : SV_POSITION;
			};

			vertOutput vert(vertInput input) {
				vertOutput o;
				o.pos = UnityObjectToClipPos(input.pos);
				return o;
			}

			half4 frag(vertOutput output) : COLOR{
				return _TheColour;
			}
			ENDCG
		}
	}
}
