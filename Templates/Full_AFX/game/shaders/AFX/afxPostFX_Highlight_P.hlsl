//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// This Post-Effect Shader is adapted from the resource,
// "Silhoute selection via postFX for Torque3D" posted by Konrad Kiss.
// http://www.garagegames.com/community/resources/view/17821
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

#include "shaders/common/postFX/postFx.hlsl"  
#include "shadergen:/autogenConditioners.h"  
  
float4 main( PFXVertToPix IN,   
             uniform sampler2D highlightBuffer :register(S0), 
				 uniform sampler2D backBuffer : register(S1),
				 uniform float2 targetSize : register(C0) ) : COLOR0  
{  
	float4 bufferColor = tex2D(backBuffer, IN.uv0);
	float4 highlightColor = tex2D(highlightBuffer, IN.uv0);

   if (highlightColor.a > 0.0)
      bufferColor.rgb = clamp(highlightColor.a*(bufferColor.rgb*1.4 + 0.05), 0, 1);

   //if (highlightColor.r + highlightColor.g + highlightColor.b > 0.0)
   //   bufferColor.rgb = clamp(bufferColor.rgb*1.4 + 0.05, 0, 1);

	return bufferColor;
}  