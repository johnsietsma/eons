Shader "Custom/Diffuse - Combine" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_OverlayTex ("Base (RGB)", 2D) = "white" {}
		_LerpValue ("Lerp Value", range(0,1)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _OverlayTex;
		fixed _LerpValue;

		struct Input {
			float2 uv_MainTex;
			float2 uv_OverlayTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c1 = tex2D (_MainTex, IN.uv_MainTex);
			half4 c2 = tex2D (_OverlayTex, IN.uv_OverlayTex);
			o.Albedo = lerp( c1.rgb, c2.rgb, _LerpValue );
			o.Alpha = c1.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
