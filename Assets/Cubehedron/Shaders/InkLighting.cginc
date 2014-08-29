#ifndef INK_LIGHTING_INCLUDED
#define INK_LIGHTING_INCLUDED

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


half4 LightingInk2 (SurfaceOutputUV s, half3 lightDir, half3 viewDir, half atten) {
        fixed NdotL = dot (s.Normal, lightDir);

	fixed diff = max (0, NdotL);
    fixed halfDiff = (NdotL*0.5) + 0.5;

	fixed turb = tex2D(_TurbulenceTex,s.uv ).r;

	fixed shadow = (diff * atten * 2);

	fixed4 c;
	c.rgb = s.Albedo * _LightColor0.rgb * shadow;
	c.a = s.Alpha;

	fixed pg = (turb-0.5) * _PigmentDispertionFactor;
	fixed2 uv2 = s.uv + fixed2(pg);

	c.rgb -= (1-shadow) * tex2D( _TurbulenceTex, uv2 ).rgb;

	return c;
}

half4 LightingInk1 (SurfaceOutputUV s, half3 lightDir, half3 viewDir, half atten) {
	fixed4 c;
    fixed NdotL = dot (s.Normal, lightDir);

    // Half-lambert, range 0-1
    fixed halfLambert = (NdotL*0.5) + 0.5;

	fixed turb = tex2D(_TurbulenceTex,s.uv ).r;

	// A random shadow wobble to give more organic edges. [-0.5,0.5]
	fixed wobble = (turb-0.5) * _EdgeWobbleFactor;
	halfLambert += wobble;

	fixed shadow = atten;

	// Prefer self-shadowing to atten light shadows
	if( halfLambert < _ShadowCutoff || shadow < _InkCutoff ) {
		fixed lNorm = (halfLambert-_ShadowCutoff)*(1/_ShadowCutoff);
		shadow = tex2D( _InkRamp, fixed2(halfLambert) );// * atten * 2;

		// Simulate pigment dispersion with a noise tex to offset uv
    	fixed pg = (turb-0.5) * _PigmentDispertionFactor;
	    fixed2 uv2 = s.uv + fixed2(pg);

    	// Simulate ink turbulence by darkening
        fixed t = tex2D(_TurbulenceTex, uv2 ).r;
        t -= 0.5;
        t *= _TurbulenceFactor;

        shadow += t;
	}
	else {
		shadow = atten * 2;
	}


	c.rgb = s.Albedo * _LightColor0.rgb * shadow * _InkColor;
    c.a = s.Alpha;

	return c;
}


#endif
