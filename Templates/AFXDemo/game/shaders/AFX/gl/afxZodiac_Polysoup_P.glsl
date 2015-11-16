
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX - PIXEL SHADER
//
// afxZodiac_Polysoup_P.glsl
//   This is the pixel shader for rendering zodiacs on polysoup models.
//   
// Copyright (C) Faust Logic, Inc.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

uniform sampler2D zodiacMap;
uniform vec4 zodiacColor;

varying vec4 hpos;
varying vec2 texCoord;

//~~~~~~~~~~~~~~~~~~~~//

void main()
{
   gl_FragColor = zodiacColor*texture2D(zodiacMap, texCoord);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
