
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX - PIXEL SHADER
//
// Copyright (C) Faust Logic, Inc.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

uniform sampler2D refractMap, colorMap, bumpMap;
uniform vec4 specularColor;
uniform float specularPower;
uniform float visibility;

varying vec2 texCoord;
varying vec3 outEyePos, normal, pos;
varying vec4 hpos2;
varying vec3 screenNorm, lightVec;

void main()
{
   vec4 base_txr = texture2D(colorMap, texCoord);
   vec3 bumpNorm = texture2D(bumpMap, texCoord*3.0).rgb*1.92 - 1.0;
   //vec3 bumpNorm = texture2D(bumpMap, texCoord).rgb * 2.0 - 1.0;

   vec3 eyeVec = normalize(outEyePos - pos);

   //vec2 texCoord2;
   //texCoord2.x = clamp(dot(bumpNorm, eyeVec), 0.0, 1.0) - (0.5/128.0);
   //texCoord2.y = 0.0;
   
   //vec4 grad = texture2D(colorMap, texCoord2);
   
   vec3 refractVec = refract(vec3(0.0, 0.0, 1.0), normalize(bumpNorm), 1.0);
   
   vec2 tc;
   tc = vec2( (  hpos2.x + refractVec.x) / (hpos2.w),
              ( -hpos2.y + refractVec.y) / (hpos2.w));
   
   tc = clamp((tc + 1.0)*0.5, 0.0, 1.0);
   
   gl_FragColor = mix(texture2D(refractMap, tc), base_txr, base_txr.a*0.7);
   //gl_FragColor = texture2D(refractMap, tc) + grad;
   

   vec3 halfAng = normalize(eyeVec + lightVec);
   float specular = clamp(dot(bumpNorm, halfAng), 0.0, 1.0);
   specular = pow(specular, specularPower);
   gl_FragColor.rgb += specularColor.rgb * specular;

   gl_FragColor.a *= visibility;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
