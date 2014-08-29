Shader "Custom/Force Field"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
		_Layer1Tex ("Rotating Layer", 2D) = "black" {}
		_Layer2Tex ("Scrolling Layer", 2D) = "black" {}
		_NoiseTex ("Distortion Layer", 2D) = "black" {}
		_Color ("Tint Color", Color) = (1,1,1,1)
		_ShapeDistortionSpeed ("Shape Distortion Speed", Float) = 2.0
		_ShapeDistortionAmount ("Shape Distortion Amount", Float) = 2.0
		_RotationSpeed ("Rotation Speed", Float) = 2.0
		_ScrollSpeed ("Scroll Speed", Float) = 2.0
		_Intensity ("Intensity", Float) = 1
		_DistortionStrength ("Distortion Strength", Float) = 1
	}

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Offset -1, -1
		//Blend SrcAlpha OneMinusSrcAlpha
		Blend One One

		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
    				float3 normal : NORMAL;
					fixed4 color : COLOR;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					half2 texcoord0 : TEXCOORD0;
					half2 texcoord1 : TEXCOORD1;
					half2 texcoord2 : TEXCOORD2;
					fixed4 color : COLOR;
				};

				sampler2D _MainTex;
				sampler2D _Layer1Tex;
				sampler2D _Layer2Tex;
				sampler2D _NoiseTex;
				float4 _MainTex_ST;
				float4 _NoiseTex_ST;
				fixed4 _Color;
				float _ShapeDistortionSpeed;
				float _ShapeDistortionAmount;
				float _RotationSpeed;
				float _ScrollSpeed;
				float _Intensity;
				float _DistortionStrength;

				v2f vert (appdata_t v)
				{
		            float sinX = sin ( _RotationSpeed * _Time );
		            float cosX = cos ( _RotationSpeed * _Time );
		            float sinY = sin ( _RotationSpeed * _Time );
		            float2x2 rotationMatrix = float2x2( cosX, -sinX, sinY, cosX);

					v2f o;
					float sinShape = sin ( _ShapeDistortionSpeed * _Time );
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.vertex.xyz += v.normal * sinShape * _ShapeDistortionAmount;

					o.texcoord0 = TRANSFORM_TEX(v.texcoord, _MainTex);
		            o.texcoord1.xy = mul ( v.texcoord.xy - half2(0.5,0.5), rotationMatrix ) + half2(0.5,0.5);
		            o.texcoord2 = o.texcoord0;
		            o.texcoord2.x += _Time * _ScrollSpeed;
					o.color = v.color;
					return o;
				}

				float rand(float3 co)
				{
    				return frac(sin( dot(co.xyz ,float3(12.9898,78.233,45.5432) )) * 43758.5453);
				}

				fixed4 frag (v2f i) : COLOR
				{
					half4 offsetColor1 = tex2D(_NoiseTex, i.texcoord0 + _Time.xz);
				    half4 offsetColor2 = tex2D(_NoiseTex, i.texcoord0 - _Time.yx);

				    half2 uv = i.texcoord0;
    				uv.x += ((offsetColor1.r + offsetColor2.r) - 1) * _DistortionStrength;
    				uv.y += ((offsetColor1.g + offsetColor2.g) - 1) * _DistortionStrength;

					fixed4 main = tex2D(_MainTex, uv);
					fixed4 l1 = tex2D(_Layer1Tex, i.texcoord1);
					fixed4 l2 = tex2D(_Layer2Tex, i.texcoord2);


					fixed4 col = main+l1+l2;
					fixed4 c = half4( lerp(col.rgb, col.rgb * _Color.rgb, main.a), main.a ) * _Intensity;
					return c;
				}
			ENDCG
		}
	}

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			AlphaTest Greater .01
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse

			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
