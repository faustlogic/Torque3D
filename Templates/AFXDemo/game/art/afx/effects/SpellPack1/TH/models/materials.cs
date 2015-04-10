
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// THOR'S HAMMER (Materials)
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

new Material(TH_auraA_MTL)
{
   mapTo = "TH_auraA.png";
   diffuseMap[0] = "TH_auraA";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(TH_auraB_MTL : TH_auraA_MTL)
{
   mapTo = "TH_auraB.png";
   diffuseMap[0] = "TH_auraB";
};

new Material(TH_casterGlowA_MTL)
{
   mapTo = "TH_casterGlowA.png";
   diffuseMap[0] = "TH_casterGlowA";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(TH_casterGlowB_MTL : TH_casterGlowA_MTL)
{
   mapTo = "TH_casterGlowB.png";
   diffuseMap[0] = "TH_casterGlowB";
};

new Material(TH_casterGlowC_MTL : TH_casterGlowA_MTL)
{
   mapTo = "TH_casterGlowC.png";
   diffuseMap[0] = "TH_casterGlowC";
};

new Material(TH_casterLightningA_MTL)
{
   mapTo = "TH_casterLightningA.png";
   diffuseMap[0] = "TH_casterLightningA";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(TH_casterLightningB_MTL)
{
   mapTo = "TH_casterLightningB.png";
   diffuseMap[0] = "TH_casterLightningB";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(TH_casterLightningC_MTL)
{
   mapTo = "TH_casterLightningC.png";
   diffuseMap[0] = "TH_casterLightningC";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(TH_glowball_MTL)
{
   mapTo = "TH_glowball.png";
   diffuseMap[0] = "TH_glowball";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(TH_hammer_MTL)
{
   mapTo = "TH_hammer.png";
   diffuseMap[0] = "TH_hammer";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = true;
};

new Material(TH_lightbeam_MTL)
{
   mapTo = "TH_lightbeam.png";
   diffuseMap[0] = "TH_lightbeam";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//


