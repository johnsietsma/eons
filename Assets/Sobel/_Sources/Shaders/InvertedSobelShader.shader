Shader "Hidden/InvertedSobelEffect" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform float4 _MainTex_TexelSize;


// Converted from SobelEdgeDetection.fx
half4 sobelEdgeDetection(float2 cTex)
{
	float width  = _MainTex_TexelSize.x * 2.0f;
	float height = _MainTex_TexelSize.y * 2.0f;

	float2 sampleOffsets[8] ={
					-width,	-height, 	// upper row
					 0.0,		-height,	 	
					 width,	-height,	
					-width,	 0.0,		// center row
					 width,	 0.0,
					-width,	 height,	// bottom row
					 0.0,		 height,
					 width,	 height,  	
				};


	int i =0;
	float4 c = .5;
	float2 texCoords;
	float4 texSamples[8];
	float4 vertGradient;
	float4 horzGradient;

	
	for(i =0; i < 8; i++)
	{
		texCoords = cTex + sampleOffsets[i]; // add sample offsets stored in c10-c17 (inclusive)
		// take sample
		texSamples[i] = tex2D(_MainTex, texCoords);
		// convert to b&w
		texSamples[i] = dot(texSamples[i], .333333f);
	}
	
	// VERTICAL Gradient
	vertGradient = -(texSamples[0] + texSamples[5] + 2*texSamples[3]);
	vertGradient += (texSamples[2] + texSamples[7] + 2*texSamples[4]);
	// Horizontal Gradient
	horzGradient = -(texSamples[0] + texSamples[2] + 2*texSamples[1]);
	horzGradient += (texSamples[5] + texSamples[7] + 2*texSamples[6]);
	
	// we could approximate by adding the abs value..but we have the horse power

	c = 1.0 - sqrt( horzGradient*horzGradient + vertGradient*vertGradient );
	return c;

}

half4 frag (v2f_img i) : COLOR
{
	half4 orig = tex2D(_MainTex, i.uv);
	half4 color = sobelEdgeDetection( i.uv );
	
	return orig * color;
}
ENDCG

	}
}

Fallback off

}
