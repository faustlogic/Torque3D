
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// FLAMING STICK TRICK (Materials)
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

new Material(FST_CastingBeamA_MTL)
{
   mapTo = "FT_castingBeamA_1.png";
   diffuseMap[0] = "FT_castingBeamA_1";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(FST_PentagramBeamA_MTL)
{
   mapTo = "FT_pentagramBeamA_1.png";
   diffuseMap[0] = "FT_pentagramBeamA_1";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(FST_Staff_MTL)
{
   mapTo = "FT_staff1.png";
   diffuseMap[0] = "FT_staff1";
   translucent = false;
   translucentZWrite = false;
   translucentBlendOp = LerpAlpha;
   castShadows = true;
};

new Material(FST_StaffStikeGlow_MTL)
{
   mapTo = "FT_strikeGlow_1.png";
   diffuseMap[0] = "FT_strikeGlow_1";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//


