//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// This is the original Post-Effect Shader used in the resource,
// "Silhoute selection via postFX for Torque3D" posted by Konrad Kiss.
// http://www.garagegames.com/community/resources/view/17821
// (currently not used for default AFX selection-highlighting)
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

#include "shaders/common/postFX/postFx.hlsl"  
#include "shadergen:/autogenConditioners.h"  
  
float4 main( PFXVertToPix IN,   
             uniform sampler2D highlightBuffer :register(S0), 
				 uniform sampler2D backBuffer : register(S1),
				 uniform float2 targetSize : register(C0) ) : COLOR0  
{  
   float2 offsets[9] = {  
      float2( 0.0,  0.0),  
      float2(-1.0, -1.0),  
      float2( 0.0, -1.0),  
      float2( 1.0, -1.0),  
      float2( 1.0,  0.0),  
      float2( 1.0,  1.0),  
      float2( 0.0,  1.0),  
      float2(-1.0,  1.0),  
      float2(-1.0,  0.0),  
   };

   float2 PixelSize = 1.0 / targetSize;  

	float avgval = 0;

   for(int i = 0; i < 9; i++)  
   {  
      float2 uv = IN.uv0 + offsets[i] * PixelSize;  
      float4 cpix = float4( tex2D( highlightBuffer, uv ).rrr, 1.0 );
		avgval += clamp(cpix.r*256, 0, 1);
	}
	
	avgval /= 9;

	float vis = round(1.0-(abs(frac(avgval)-0.5)*2));

	float4 bb = tex2D(backBuffer, IN.uv0);
	float4 outlineColor = float4(vis, 0, 0, vis);
	float4 overlayColor = float4(avgval, 0, 0, avgval);
	//float4 outlineColor = float4(vis*0.5, vis*0.5, vis*0.5, vis*0.5);
	//float4 overlayColor = float4(avgval, avgval, avgval, avgval);

	overlayColor *= 0.4;

	bb = lerp(bb, overlayColor, overlayColor.a);

	outlineColor = lerp(bb, outlineColor, outlineColor.a);

	return outlineColor;
}  