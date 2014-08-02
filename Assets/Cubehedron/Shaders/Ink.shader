Shader "Custom/Ink" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_InkColor ("Ink Color", Color) = (0,0,0,0)
		_InkCutoff ("Ink Cutoff", Range(0,1)) = 0.5
		_NoiseTex ("Noise Tex", 2D) = "white" {}
		_EdgeWobbleFactor ("Edge Wobble Factor", Float) = 2
		_TurbulenceTex ("Turbulence", 2D) = "white"
		_TurbulenceFactor ("Turbulence Factor", Float) = 2
		_PigmentDispertionFactor ("Pigment Dispersion Factor", Float) = 2
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Ink

		sampler2D _MainTex;
		fixed4 _InkColor;
		fixed _InkCutoff;
		sampler2D _NoiseTex;
		fixed _EdgeWobbleFactor;
		sampler2D _TurbulenceTex;
		fixed _TurbulenceFactor;
		fixed _PigmentDispertionFactor;

		struct SurfaceOutputUV
		{
		    fixed3 Albedo;
		    fixed3 Normal;
		    fixed3 Emission;
		    half Specular;
		    fixed Gloss;
		    fixed Alpha;

		    float3 viewDir;
		    fixed2 uv;
		};


	    half4 LightingInk (SurfaceOutputUV s, half3 lightDir, half atten) {
	        fixed normalLight = dot (s.Normal, lightDir);

	        // Half-lambert, range 0-1
	        normalLight = (normalLight*0.5) + 0.5;

	        // Alter the edge of the ink by a noise texture
	        fixed noise = tex2D(_NoiseTex,s.uv ).r;

	        // The higher the factor, the less random variation
	        fixed wobble = pow( noise, _EdgeWobbleFactor );

	        normalLight += wobble;

	        fixed4 c;
	        c.rgb = s.Albedo;
	        c.a = s.Alpha;

	        if( normalLight < _InkCutoff ) {
	        	// Simulate pigment dispertion with a noise tex to offset uv
		        fixed pg = pow( noise, _PigmentDispertionFactor );
		        fixed2 uv2 = s.uv + fixed2(pg,pg);

	        	// Simulate ink turbulence by darkening
	        	fixed t = tex2D(_TurbulenceTex, uv2 ).r;
	        	t -= 0.5;
	        	t = pow( t, _TurbulenceFactor );

	        	c.rgb = lerp( c.rgb, _InkColor, 0.5+t );

   				fixed diffView = dot(s.Normal, normalize(s.viewDir));
		        if( diffView < 0.15 ) {
		        	c.rgb = 0;
	    	    }
	        }


	    	return c;
	    }


		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
		};

		void surf (Input IN, inout SurfaceOutputUV o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.viewDir = IN.viewDir;
			o.uv = IN.uv_MainTex;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
