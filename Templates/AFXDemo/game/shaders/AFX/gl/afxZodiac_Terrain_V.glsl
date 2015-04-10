
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX - VERTEX SHADER
//
// afxZodiac_Terrain_V.glsl
//   This is the vertex shader for rendering zodiacs on terrain.
//   
// Copyright (C) Faust Logic, Inc.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

uniform mat4 modelview;

varying vec2 texCoord;
varying vec4 color0;
varying vec4 position;

//~~~~~~~~~~~~~~~~~~~~//

void main()
{
   texCoord = gl_MultiTexCoord0.st;
   gl_Position = modelview * gl_Vertex;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
