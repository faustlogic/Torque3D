
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// FIRE IN THE SKY (lighting)
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

datablock LightFlareData(FitS_FireballLight_FLARE)
{
  overallScale = 1.0;
  flareEnabled = true;
  flareTexture = %mySpellDataPath @ "/Shared/lights/sp2_falloffFlare";
  elementRect[0] = "0 0 256 256";
  elementDist[0] = 0.0;
  elementScale[0] = 16.0;
  elementTint[0] = "1 1 1";
  elementRotate[0] = false;
  elementUseLightColor[0] = true;
};

datablock LightAnimData(FitS_FireballLight_ANI)
{
  animEnabled = true;
  minBrightness = 0.7;
  maxBrightness = 1.0;
};

// A light that's mounted at the top of the tower.
datablock afxT3DPointLightData(FitS_FireballLight_CE)
{
  radius = 8;
  color = "1.0 0.6 0.0";
  brightness = 5;
  castShadows = false;
  localRenderViz = false;
  flareType = FitS_FireballLight_FLARE;
  flareScale = 1;
  animate = true;
  animationType = FitS_FireballLight_ANI;
  animationPeriod = 0.3;
  animationPhase = 0.0;
};

datablock afxEffectWrapperData(FitS_FireballLight_EW)
{
  effect = FitS_FireballLight_CE;
  posConstraint = "#ghost.FireTower.mountFireball";
  delay       = $FSK_Cue_FireRing + (10.0 - 271/30);
  fadeInTime  = 1;
  fadeOutTime = 1;
  lifetime    = 20;
}; 

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function FitS_add_Lighting_FX(%spell_data)
{
  %spell_data.addCastingEffect(FitS_FireballLight_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
