Shader "Custom/Ink" {
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
		#pragma surface surf Ink
		#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _InkColor;
		fixed _InkCutoff;
		fixed _ShadowCutoff;
		sampler2D _InkRamp;
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

		    fixed2 uv;
		};

	    half4 LightingInk (SurfaceOutputUV s, half3 lightDir, half3 viewDir, half atten) {
	    	fixed4 c;
	        fixed NdotL = dot (s.Normal, lightDir);

	        // Half-lambert, range 0-1
	        fixed halfLambert = (NdotL*0.5) + 0.5;

	    	fixed turb = tex2D(_TurbulenceTex,s.uv ).r;

	    	// A random shadow wobble to give more organic edges. [-0.5,0.5]
	    	fixed wobble = (turb-0.5) * _EdgeWobbleFactor;
	    	halfLambert += wobble;

	        // Make the shadows we get from atten brighter.
	    	fixed shadow = (atten+0.5) * 0.5;

	    	// Prefer self-shadowing to atten light shadows
	    	if( halfLambert < _ShadowCutoff || shadow < _InkCutoff ) {
	    		fixed lNorm = (halfLambert-_ShadowCutoff)*(1/_ShadowCutoff);
	    		shadow = tex2D( _InkRamp, fixed2(0.5,0.5) );// * atten * 2;

	    		// Simulate pigment dispersion with a noise tex to offset uv
		    	fixed pg = (turb-0.5) * _PigmentDispertionFactor;
			    fixed2 uv2 = s.uv + fixed2(pg);

		    	// Simulate ink turbulence by darkening
		        fixed t = tex2D(_TurbulenceTex, uv2 ).r;
		        t -= 0.5;
		        t *= _TurbulenceFactor;

		        //shadow += t;

		        //shadow = lNorm;
	    	}
	    	else {
	    		shadow = shadow * atten * 2;
	    		//shadow = tex2D( _InkRamp, fixed2(1-shadow) );
	    	}


	    	c.rgb = shadow;// * _InkColor * _LightColor0.rgb * s.Albedo;
	        c.a = s.Alpha;

	    	return c;
	    }


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
