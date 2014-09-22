Shader "Custom/Diffuse - Screen Space" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ScreenSpaceTex ("Screen Space (RGB)", 2D) = "white" {}
		_ScreenSpaceAmount ("Screen Space Amount", Float) = 0.1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _ScreenSpaceTex;
		fixed _ScreenSpaceAmount;

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
		};

		void surf (Input IN, inout SurfaceOutput o) {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);

			fixed2 uvScreen = IN.screenPos.xy / IN.screenPos.w;
            half4 cSS = tex2D (_ScreenSpaceTex, uvScreen);
			o.Albedo = lerp( c.rgb, cSS.rgb, _ScreenSpaceAmount );
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
