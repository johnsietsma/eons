Shader "Custom/Diffuse Additive - X Flipped" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_AdditiveTex ("Additive (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		//BlendOp Sub

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _AdditiveTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float2 uv = IN.uv_MainTex;
			uv = float2(1-uv.s, uv.t);
			half4 c = tex2D (_MainTex, uv);
			half4 cAdd = tex2D (_AdditiveTex, IN.uv_MainTex);
			o.Albedo = c.rgb + cAdd.rgb;
			o.Alpha = c.a + cAdd.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
