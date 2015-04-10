
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// Fire in the Sky (Materials)
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

new Material(FitS_TowerShiny_MTL)
{
   mapTo = "towerShiny";
   diffuseMap[0] = "FT_stone6";
   //bumpTex[0] = "normals";
   cubemap = WChrome;
   pixelSpecular[0] = true;
   specular[0] = "0.5 0.5 0.5 0.5";
   specularPower[0] = 8.0;
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = true;
};

new Material(FitS_TowerDrab_MTL)
{
   mapTo = "towerDrab";
   diffuseMap[0] = "FT_stone6";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = true;
};

new Material(FitS_FinalBeamA_MTL)
{
   mapTo = "FT_finalBeamA_1.png";
   diffuseMap[0] = "FT_finalBeamA_1";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(FitS_FinalBeamB_MTL)
{
   mapTo = "FT_finalBeamB_1.png";
   diffuseMap[0] = "FT_finalBeamB_1";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//


