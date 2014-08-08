//Shows the grayscale of the depth from the camera.

Shader "Custom/DepthShader"
{
SubShader {
    Tags { "RenderType"="Opaque" }
    Pass {
        Fog { Mode Off }
CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

struct v2f {
    float4 pos : SV_POSITION;
    fixed4 color : COLOR0;
};

v2f vert (appdata_base v) {
    v2f o;
    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
    //o.color = dot (v.normal, float3(0,1,1));
    //o.color = dot (v.normal, mul(_Object2World, float4(1,1,1,1)).xyz );
    o.color = v.normal.z;
    return o;
}

half4 frag(v2f i) : COLOR {
    return i.color;
}
ENDCG
    }
}
}
