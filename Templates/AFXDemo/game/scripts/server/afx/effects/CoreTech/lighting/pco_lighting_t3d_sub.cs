
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// PRESTO CHANGE ORC (lighting)
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

datablock LightFlareData(PCO_WandFlare_FLARE)
{
  overallScale = 1.0;
  flareEnabled = true;
  flareTexture = %mySpellDataPath @ "/PCO/lights/PCO_corona";

  elementRect[0] = "0 0 128 128";
  elementDist[0] = 0.0;
  elementScale[0] = 16.0;
  elementTint[0] = "1 1 1";
  elementRotate[0] = false;
  elementUseLightColor[0] = true;
};

datablock afxT3DPointLightData(PCO_WandFlare_CE)
{
  radius = 10;
  color = "0.5 0.5 0.5";
  brightness = 1;
  //color = "1 0 0";
  brightness = 20;
  flareType = PCO_WandFlare_FLARE;
  castShadows = false;
  localRenderViz = false;
};

/*
datablock sgLightObjectData(PCO_WandFlare_CE)
{
  CastsShadows = false;
  Radius = 10;
  Brightness = 1;
  Colour = "0.5 0.5 0.5";

  FlareOn = true;
  LinkFlare = true;
  FlareBitmap = "common/lighting/corona";
  NearSize = 4;
  FarSize  = 3;
  NearDistance = 2;
  FarDistance  = 50;
};
*/

datablock afxEffectWrapperData(PCO_WandFlare_EW : PCO_WandSparkles_EW)
{
  effect = PCO_WandFlare_CE;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DPointLightData(PCO_ExplosionLight_CE)
{
  radius = 10;
  color = "1.0 0.8 0.8";
  brightness = 5;
  castShadows = false;
  localRenderViz = false;
};

datablock afxXM_LocalOffsetData(PCO_ExplosionLight_offset_XM)
{
  localOffset = "0 0 1.5";
};
datablock afxEffectWrapperData(PCO_ExplosionLight_EW)
{
  effect = PCO_ExplosionLight_CE;
  posConstraint = "impactPoint";
  delay = (42/30);
  lifetime = 0.05;
  fadeInTime  = 0.05;
  fadeOutTime = 0.6;
  xfmModifiers[0] = PCO_ExplosionLight_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

// add effects to main spell datablock
function PCO_add_Lighting_FX(%spell_data)
{
  %spell_data.addCastingEffect(PCO_WandFlare_EW);
  %spell_data.addCastingEffect(PCO_ExplosionLight_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
