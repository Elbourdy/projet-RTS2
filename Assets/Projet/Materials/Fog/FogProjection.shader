Shader "Custom/FogProjection"
{
	Properties
	{
		_PrevTexture("Previous Texture", 2D) = "white" {}
		_CurrTexture("Current Texture", 2D) = "white" {}
		_Colore("Color", Color) = (0, 0, 0, 0)
		_Blend("Blend", Float) = 0
	}
		SubShader
		{
			Tags { "Queue" = "Transparent+100" } 

			Pass
			{
				ZWrite Off
				Blend SrcAlpha OneMinusSrcAlpha
				ZTest Equal

				HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#pragma shader_feature_local FSR_PROJECTOR_FOR_LWRP
				#pragma multi_compile_local _ FSR_RECEIVER
				#pragma multi_compile_fog
				#include "Assets/ProjectorForLWRP-master-universalrp/ProjectorForLWRP-master-universalrp/Shaders/P4LWRP.cginc"
				#include "UnityCG.cginc"


				struct appdata
				{
					float4 vertex : POSITION;
					float4 uv : TEXCOORD0;
				};

				struct v2f
				{
					float4 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};


				sampler2D _CurrTexture;
				sampler2D _PrevTexture;
				CBUFFER_START(UnityPerMaterial) 
				fixed4 _Colore;
				float _Blend;
				CBUFFER_END
				

				P4LWRP_V2F_PROJECTOR vert(float4 vertex : POSITION)
				{
					P4LWRP_V2F_PROJECTOR o;
					fsrTransformVertex(vertex, o.pos, o.uvShadow);
					UNITY_TRANSFER_FOG(o, o.pos);
					return o;
				}

				fixed4 frag(P4LWRP_V2F_PROJECTOR i) : SV_Target
				{
					float aPrev = tex2Dproj(_PrevTexture, i.uvShadow).a;
					float aCurr = tex2Dproj(_CurrTexture, i.uvShadow).a;

					fixed a = lerp(aPrev, aCurr, _Blend);

					
					_Colore.a = max(0, _Colore.a - a);
					return _Colore;
				}
				ENDHLSL
			}
		}
}

