
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SOUL SUCKING JERK (lighting)
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

datablock afxT3DPointLightData(SSJ_HandLight_CE)
{
  radius = 5;
  color = (1/255) SPC (178/255) SPC (136/255);
  brightness = 2;
  castShadows = false; // T3D Off (temporarily)
  localRenderViz = false;
};

datablock afxEffectWrapperData(SSJ_HandLight_EW : SSJ_SkeletalHand_RT_EW) 
{
  effect = SSJ_HandLight_CE;
  posConstraint = "#effect.SkeletalHand_RT.mount_midA"; 
  fadeInTime = 2.0;
  effectName = "";
  isConstraintSrc = false;
  constraint = "";
};

//~~~~~~~~~~~~~~~~~~~~//

datablock LightFlareData(SSJ_AppearanceFlareLight_FLARE)
{
  overallScale = 1.0;
  flareEnabled = true;
  flareTexture = %mySpellDataPath @ "/Shared/lights/sp2_coronaFlare";
  elementRect[0] = "0 0 128 128";
  elementDist[0] = 0.0;
  elementScale[0] = 40.0;
  elementTint[0] = "1 1 1";
  elementRotate[0] = false;
  elementUseLightColor[0] = true;
};

datablock afxT3DPointLightData(SSJ_AppearanceFlareLight_CE)
{
  radius = 10;
  color = (1/255) SPC (178/255) SPC (136/255);
  brightness = 1;
  castShadows = false;
  localRenderViz = false;
  flareType = SSJ_AppearanceFlareLight_FLARE;
  flareScale = 1;
};

datablock afxEffectWrapperData(SSJ_AppearanceFlareLight_EW) 
{
  effect = SSJ_AppearanceFlareLight_CE;
  posConstraint = "#effect.SkeletalHand_RT.mount_midA"; 
  delay = 2.167;
  lifetime = 0.1;
  fadeInTime  = 0.1;
  fadeOutTime = 0.2;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DPointLightData(SSJ_LockOnLight_CE)
{
  radius = 7;
  color = (1/255) SPC (178/255) SPC (136/255);
  brightness = 1.5;
  castShadows = false; // T3D Off (temporarily)
  localRenderViz = false;
};

datablock afxEffectWrapperData(SSJ_LockOnLight_EW : SSJ_LockOnFlare_EW) 
{
  effect = SSJ_LockOnLight_CE;
  posConstraint2 = "";
  xfmModifiers[0] = "";
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DPointLightData(SSJ_SoulLight_CE)
{
  radius = 20;
  color = "1.0 1.0 1.0";
  brightness = 2;
  castShadows = false; // T3D Off (temporarily)
  localRenderViz = false;
};

datablock afxEffectWrapperData(SSJ_SoulLight_EW) 
{
  effect = SSJ_SoulLight_CE;
  posConstraint = missile;
  fadeInTime = 0.5;
  fadeOutTime = 0.5;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function SSJ_add_hand_Lighting_FX(%effect_data)
{
  %effect_data.addEffect(SSJ_HandLight_EW);
  %effect_data.addEffect(SSJ_AppearanceFlareLight_EW);
}

function SSJ_add_Lighting_FX(%spell_data)
{
  %spell_data.addCastingEffect(SSJ_LockOnLight_EW);
  %spell_data.addDeliveryEffect(SSJ_SoulLight_EW);
}
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
