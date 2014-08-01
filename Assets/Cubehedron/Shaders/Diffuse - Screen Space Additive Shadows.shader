Shader "Custom/Diffuse - Screen Space Additive Shadows" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ShadowTex ("Shadow (RGB)", 2D) = "white" {}
		_ScreenSpaceAmount ("Screen Space Amount", Float) = 0.1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf ScreenSpaceTexture

		struct SurfaceOutputUV
		{
		    fixed3 Albedo;
		    fixed3 Normal;
		    fixed3 Emission;
		    half Specular;
		    fixed Gloss;
		    fixed Alpha;

		    fixed2 uvScreen;
		};

		sampler2D _MainTex;
		sampler2D _ShadowTex;
		fixed _ScreenSpaceAmount;

	    half4 LightingScreenSpaceTexture (SurfaceOutputUV s, half3 lightDir, half atten) {
	        fixed diff = max (0, dot (s.Normal, lightDir));

	        // Simple Lambert
	        fixed lightValue = (diff * atten * 2);
			fixed4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * lightValue;
			c.a = s.Alpha;

			// Add in the shadow tex
			fixed invLightValue = 1-(lightValue*0.5);
            half4 cShadow = tex2D (_ShadowTex, s.uvScreen);

            c.rgb += (cShadow.rgb * invLightValue * _ScreenSpaceAmount);

			return c;
	    }

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
		};

		void surf (Input IN, inout SurfaceOutputUV o) {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.uvScreen = IN.screenPos.xy / IN.screenPos.w;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
