
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// CHILL KILL (Materials)
//    This file contains shader and material definitions compatible
//    with the T3D engine.
//
// Copyright (C) 2015 Faust Logic, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Shaders

/*
%shader_path = expandFilename("./shaders/");

new ShaderData(CK_iceShards_Shader)
{
   DXVertexShaderFile   = %shader_path @ "CK_ice_V.hlsl";
   DXPixelShaderFile    = %shader_path @ "CK_ice_P.hlsl";

   OGLVertexShaderFile  = %shader_path @ "CK_ice_V.glsl";
   OGLPixelShaderFile   = %shader_path @ "CK_ice_P.glsl";
   OGLSamplerNames[0]   = "$refractMap";
   OGLSamplerNames[1]   = "$colorMap";
   OGLSamplerNames[2]   = "$bumpMap";

   pixVersion = 2.0;
};
*/

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Materials and CustomMaterials

new Material(CK_SnowBall_MTL)
{
   mapTo = "CK_SnowBall.png";
   diffuseMap[0] = "CK_SnowBall";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

//~~~~~~~~~~~~~~~~~~~~//

new Material(CK_IceCrystalsGlint_MTL)
{
   mapTo = "CK_IceCrystalsGlint";
   diffuseMap[0] = "CK_IceCrystalsA";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

//~~~~~~~~~~~~~~~~~~~~//

new Material(CK_IceCrystalsA_MTL)
{
   mapTo = "CK_IceCrystalsA.png";
   diffuseMap[0] = "CK_IceCrystalsB_alpha";
   translucent = true;
   translucentZWrite = false;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

new Material(CK_IceCrystalsB_MTL)
{
   mapTo = "CK_IceCrystalsB.png";
   diffuseMap[0] = "CK_IceCrystalsB_alpha";
   translucent = true;
   translucentZWrite = false;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

/*
new CustomMaterial(CK_IceCrystalsA_CMTL)
{
   mapTo = "CK_IceCrystalsA.png";
   texture[0] = "$backbuff";
   texture[1] = "CK_IceCrystalsB_alpha";
   texture[2] = "CK_IceCrystals_Bump";
   shader = CK_iceShards_Shader;
   refract = true;
   translucentBlendOp = LerpAlpha;
   translucentZWrite = true;
   specular[0] = "1 1 1 1";
   specularPower[0] = 16.0;
   version = 2.0;
};

new CustomMaterial(CK_IceCrystalsB_CMTL)
{
   mapTo = "CK_IceCrystalsB.png";
   texture[0] = "$backbuff";
   texture[1] = "CK_IceCrystalsB_alpha";
   texture[2] = "CK_IceCrystals_Bump";
   shader = CK_iceShards_Shader;
   refract = true;
   translucentBlendOp = LerpAlpha;
   translucentZWrite = true;
   specular[0] = "1 1 1 1";
   specularPower[0] = 16.0;
   version = 2.0;
};
*/

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//


