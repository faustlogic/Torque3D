
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// INSECTOPLASM (Materials)
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

new Material(IOP_beamA_MTL)
{
   mapTo = "IOP_beamA.png";
   diffuseMap[0] = "IOP_beamA";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(IOP_beamB_MTL)
{
   mapTo = "IOP_beamB.png";
   diffuseMap[0] = "IOP_beamB";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(IOP_beamC_MTL)
{
   mapTo = "IOP_beamC.png";
   diffuseMap[0] = "IOP_beamC";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(IOP_beamRing_MTL)
{
   mapTo = "IOP_beamRing.png";
   diffuseMap[0] = "IOP_beamRing";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(IOP_bug_MTL)
{
   mapTo = "IOP_bug.png";
   diffuseMap[0] = "IOP_bug";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

new Material(IOP_bugPulse_MTL)
{
   mapTo = "IOP_bugPulse.png";
   diffuseMap[0] = "IOP_bugPulse";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(IOP_bugPulseSparkle_MTL)
{
   mapTo = "IOP_bugPulseSparkle.png";
   diffuseMap[0] = "IOP_bugPulseSparkle";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(IOP_bugSymbolA_MTL)
{
   mapTo = "IOP_bugSymbolA.png";
   diffuseMap[0] = "IOP_bugSymbolA";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(IOP_bugSymbolB_MTL)
{
   mapTo = "IOP_bugSymbolB.png";
   diffuseMap[0] = "IOP_bugSymbolB";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(IOP_insectSparkle_MTL)
{
   mapTo = "IOP_insectSparkle.png";
   diffuseMap[0] = "IOP_insectSparkle";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(IOP_insectHead_MTL)
{
   mapTo = "IOP_insectHead.png";
   diffuseMap[0] = "IOP_insectHead";
   translucent = true;
   translucentBlendOp = AddAlpha;
   translucentZWrite = true;
   castShadows = false;
};

new Material(IOP_insectEye_MTL)
{
   mapTo = "IOP_insectEye";
   diffuseMap[0] = "IOP_insectHead";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

new Material(IOP_insectJaws_MTL)
{
   mapTo = "IOP_insectJaws";
   diffuseMap[0] = "IOP_insectHead";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

new Material(IOP_insectSegment_MTL)
{
   mapTo = "IOP_insectSegment.png";
   diffuseMap[0] = "IOP_insectSegment";
   translucent = false;
   translucentBlendOp = LerpAlpha;
   translucentZWrite = true;
   castShadows = false;
};

new Material(IOP_insectWings_MTL)
{
   mapTo = "IOP_insectWings.png";
   diffuseMap[0] = "IOP_insectWings";
   translucent = true;
   translucentBlendOp = AddAlpha;
   translucentZWrite = true;
   castShadows = false;
};

new Material(IOP_insectWings_transA_MTL)
{
   mapTo = "IOP_insectWings_transA.png";
   diffuseMap[0] = "IOP_insectWings_transA";
   translucent = true;
   translucentBlendOp = AddAlpha;
   translucentZWrite = true;
   castShadows = false;
};

new Material(IOP_insectWings_transB_MTL)
{
   mapTo = "IOP_insectWings_transB.png";
   diffuseMap[0] = "IOP_insectWings_transB";
   translucent = true;
   translucentBlendOp = AddAlpha;
   translucentZWrite = true;
   castShadows = false;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
