// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "SOM/VignetteShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "" {}
	}
	
	CGINCLUDE
	
	#include "UnityCG.cginc"
	
	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};
	
	sampler2D _MainTex;
	
	float4 _MainTex_TexelSize;
	float vignetteIntensity;
    
	v2f vert( appdata_img v ) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		
		o.uv = v.texcoord.xy;
		return o;
	} 
	
	half4 frag(v2f i) : SV_Target {
		half2 uv = i.uv;
		
		half4 color = tex2D (_MainTex, uv);
        
        float mask = 0;
        if (vignetteIntensity >= 50) {
            mask = 0;
        } else if (vignetteIntensity <= -50) {
            mask = 100;
        } else {
            half2 coords = i.uv;
            coords = (coords - 0.5);
            half coordDot = dot (coords, coords);
            mask = 1.0 - coordDot * vignetteIntensity;
        }
        
		return color * mask;
	}

	ENDCG 
	
Subshader {
 Pass {
      ZTest Always Cull Off ZWrite Off

      CGPROGRAM
      
      #pragma vertex vert
      #pragma fragment frag

      ENDCG
  }
}
	
Fallback off

} // shader
