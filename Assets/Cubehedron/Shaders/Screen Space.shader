Shader "Custom/Screen Space" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ShadowTex ("Shadow (RGB)", 2D) = "white" {}
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

	    half4 LightingScreenSpaceTexture (SurfaceOutputUV s, half3 lightDir, half atten) {
	        fixed diff = max (0, dot (s.Normal, lightDir));

	        // Simple Lambert
			fixed4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 2);
			c.a = s.Alpha;

            half4 cShadow = tex2D (_ShadowTex, s.uvScreen);
            float brightness = Luminance( c.rgb );
            brightness += 0.95;
            c.rgb = c.rgb + cShadow * clamp(1-brightness, 0, 1);

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
