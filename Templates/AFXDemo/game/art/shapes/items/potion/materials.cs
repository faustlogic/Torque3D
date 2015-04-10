
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
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

singleton Material(Potion_Liquid_MTL)
{
   mapTo = "redLiquid";
   diffuseMap[0] = "redLiquid";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = true;
};

singleton Material(Potion_Glass_MTL)
{
   mapTo = "redGlass";
   diffuseMap[0] = "redGlass";
   translucent = true;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

singleton Material(Potion_Cross_MTL)
{
   mapTo = "healthBB";
   diffuseMap[0] = "healthBB";
   translucent = true;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

singleton Material(Potion_Spec_MTL)
{
   mapTo = "fakeSpec";
   diffuseMap[0] = "fakeSpec";
   emissive[0] = true;
   translucent = true;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

singleton Material(Potion_Base_MTL)
{
   mapTo = "vialBase";
   diffuseMap[0] = "vialBase";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};
