
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX - PIXEL SHADER
//
// afxZodiac_Interior_P.hlsl
//   This is the pixel shader for rendering zodiacs on interiors.
//   
// Copyright (C) Faust Logic, Inc.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

struct ConnectData
{
   float4 hpos         : POSITION;
   float2 texCoord     : TEXCOORD0;
};

struct Fragout
{
   float4 col : COLOR0;
};

Fragout main( ConnectData IN,
              uniform sampler2D zodiacMap       : register(S0),
              uniform float4    zodiacColor     : register(C2)
)
{
   Fragout OUT;
   
   OUT.col = zodiacColor*tex2D(zodiacMap, IN.texCoord);
   
   return OUT;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
