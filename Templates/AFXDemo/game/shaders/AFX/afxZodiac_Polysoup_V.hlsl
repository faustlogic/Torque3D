
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX - VERTEX SHADER
//
// afxZodiac_Polysoup_P.hlsl
//   This is the pixel shader for rendering zodiacs on polysoup models.
//   
// Copyright (C) Faust Logic, Inc.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

struct VertData
{
   float4 position        : POSITION;
   float4 color           : COLOR0;
   float2 texCoord        : TEXCOORD0;
};

struct ConnectData
{
   float4 hpos         : POSITION;
   float2 texCoord     : TEXCOORD0;
};

//-----------------------------------------------------------------------------
// Main
//-----------------------------------------------------------------------------
ConnectData main( VertData IN,
                  uniform float4x4 modelView : register(C0)
                  )
{
   ConnectData OUT;

   OUT.hpos = mul(modelView, IN.position); 
   OUT.texCoord = IN.texCoord;
    
   return OUT;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
