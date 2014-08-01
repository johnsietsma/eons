Shader "Custom/Diffuse - Screen Space View Additive" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ShadowTex ("Shadow (RGB)", 2D) = "white" {}
		_ScreenSpaceFactor ("Screen Space Factor", Float) = 4
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
		    float3 viewDir;
		};

		sampler2D _MainTex;
		sampler2D _ShadowTex;
		fixed _ScreenSpaceFactor;
		fixed _ScreenSpaceAmount;


	    half4 LightingScreenSpaceTexture (SurfaceOutputUV s, half3 lightDir, half atten) {
	        fixed diff = max (0, dot (s.Normal, lightDir));

	        // Simple Lambert
	        fixed4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 2);
			c.a = s.Alpha;

			// Add in the shadow tex
            half4 cShadow = tex2D (_ShadowTex, s.uvScreen) * _ScreenSpaceAmount;

			half3 h = normalize (s.viewDir);
			float nh = max (0, dot (s.Normal, h));


	        fixed diffView = dot(s.Normal, normalize(s.viewDir));
	        fixed diffNorm = diffView * 0.5 + 0.5;
	        diffNorm = pow( diffNorm, _ScreenSpaceFactor );
	        c.rgb += cShadow.rgb * diffNorm;

			return c;
	    }

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
			float3 viewDir;
		};

		void surf (Input IN, inout SurfaceOutputUV o) {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.uvScreen = IN.screenPos.xy / IN.screenPos.w;
			o.viewDir = IN.viewDir;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
