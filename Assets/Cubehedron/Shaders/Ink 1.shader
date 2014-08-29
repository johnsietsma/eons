Shader "Custom/Ink 1" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_InkColor ("Ink Color", Color) = (0,0,0,0)
		_InkCutoff ("Ink Cutoff", Float) = 0.5
		_ShadowCutoff ("Shadow Cutoff", Float) = 0.5
		_InkRamp ("Ink Ramp", 2D) = "black"
		_EdgeWobbleFactor ("Edge Wobble Factor", Float) = 0.1
		_TurbulenceTex ("Turbulence", 2D) = "white"
		_TurbulenceFactor ("Turbulence Factor", Float) = 0.1
		_PigmentDispertionFactor ("Pigment Dispersion Factor", Float) = 0.1
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Cull Back
		ZWrite On
		ColorMask RGB

		CGPROGRAM
		#pragma surface surf Ink1
		#pragma target 3.0

		#include "InkLighting.cginc"

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutputUV o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.uv = IN.uv_MainTex;
		}
		ENDCG
	}
	Fallback "VertexLit"
	//FallBack "Diffuse"
}
