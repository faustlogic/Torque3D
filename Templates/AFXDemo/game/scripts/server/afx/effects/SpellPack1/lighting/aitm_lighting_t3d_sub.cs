
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// ARCANE IN THE MEMBRANE (lighting)
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

datablock afxT3DSpotLightData(AitM_CasterCrazyHeadLight_CE)
{
  range = 15;
  color = "1.0 1.0 1.0";
  brightness = 3.0;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(AitM_CasterCrazyHeadLight_EW)
{
  effect = AitM_CasterCrazyHeadLight_CE;
  posConstraint = "caster.Bip01 Head";
  posConstraint2 = "camera"; // aim
  delay       = 0;
  fadeInTime  = 1.0;
  fadeOutTime = 2.0;
  lifetime    = 1.0;
  xfmModifiers[0] = AitM_CasterCrazyHead_offset_XM;
  xfmModifiers[1] = AitM_CasterCrazyHead_aim_XM;
  xfmModifiers[2] = AitM_CasterCrazyHead_path_XM;  
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DPointLightData(AitM_CasterHandLight_CE)
{
  radius = 4.5;
  color = "1.0 0.62 0.2";
  brightness = 0.8;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(AitM_CasterHandLight_lf_hand_EW)
{
  effect = AitM_CasterHandLight_CE;
  constraint = "caster.Bip01 L Hand";
  lifetime = 2.3;
  delay = 3.0;
  fadeInTime = 0.3;
  fadeOutTime = 0.3;
};

datablock afxEffectWrapperData(AitM_CasterHandLight_rt_hand_EW : AitM_CasterHandLight_lf_hand_EW)
{
  constraint = "caster.Bip01 R Hand";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock LightFlareData(AitM_TeethFlare_FLARE)
{
  overallScale = 1.0;
  flareEnabled = true;
  flareTexture = %mySpellDataPath @ "/AitM/lights/AitM_teethFlare";

  elementRect[0] = "0 0 256 256";
  elementDist[0] = 0.0;
  elementScale[0] = 25.0*0.3;
  elementTint[0] = "1 1 1";
  elementRotate[0] = false;
  elementUseLightColor[0] = false;
};
datablock afxT3DPointLightData(AitM_ConfusionCloudFlashLight1_CE)
{
  radius = 25.0*0.3;
  color = "1 1 1";
  brightness = 1;
  flareType = AitM_TeethFlare_FLARE;
  flareScale = 1;
  castShadows = false;
  localRenderViz = false;
};
datablock afxXM_LocalOffsetData(AitM_ConfusionCloudFlash_offset_XM)
{
  localOffset = "0 0 3.0";
};
datablock afxEffectWrapperData(AitM_ConfusionCloudFlash1_EW)
{
  effect = AitM_ConfusionCloudFlashLight1_CE;
  posConstraint = "impactedObject";
  lifetime = 0.8;
  delay = 4.15;
  fadeInTime = 0.3;
  fadeOutTime = 0.3;
  xfmModifiers[0] = AitM_ConfusionCloudFlash_offset_XM;
};

datablock LightFlareData(AitM_Corona_FLARE)
{
  overallScale = 5.0;
  flareEnabled = true;
  flareTexture = %mySpellDataPath @ "/AitM/lights/AitM_corona";

  elementRect[0] = "0 0 128 128";
  elementDist[0] = 0.0;
  elementScale[0] = 25.0*0.3;
  elementTint[0] = "1 0 1";
  elementRotate[0] = true;
  elementUseLightColor[0] = true;
};
datablock afxT3DPointLightData(AitM_ConfusionCloudFlashLight2_CE)
{
  radius = 25.0*0.3;
  color = "1 0 1";
  brightness = 1;
  flareType = AitM_Corona_FLARE;
  castShadows = false;
  localRenderViz = false;
};
datablock afxEffectWrapperData(AitM_ConfusionCloudFlash2_EW)
{
  effect = AitM_ConfusionCloudFlashLight2_CE;
  posConstraint = "impactedObject";
  lifetime = 0.4;
  delay = 4.35;
  fadeInTime = 0.1;
  fadeOutTime = 1.0;
  xfmModifiers[0] = AitM_ConfusionCloudFlash_offset_XM;  
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock LightAnimData(AitM_MaskA_ANI)
{
  animEnabled = true;
  minBrightness = 0.4;
  maxBrightness = 1;
};
datablock LightFlareData(AitM_MaskCoronaA_FLARE)
{
  overallScale = 2;
  flareEnabled = true;
  flareTexture = %mySpellDataPath @ "/AitM/lights/AitM_corona";
  elementRect[0] = "0 0 128 128";
  elementDist[0] = 0.0;
  elementScale[0] = 7;
  elementTint[0] = "0.1 0 1";
  elementRotate[0] = true;
  elementUseLightColor[0] = true;
};
datablock afxT3DPointLightData(AitM_TargetMaskLightA_CE)
{
  radius = 7;
  color = "0.1 0 1";
  brightness = 1;
  flareType = AitM_MaskCoronaA_FLARE;
  animate = true;
  animationType = AitM_MaskA_ANI;
  animationPeriod = 0.8;
  animationPhase = 0.0;
  castShadows = false;
  localRenderViz = false;
};
datablock afxEffectWrapperData(AitM_TargetMaskLightA_EW)
{
  effect = AitM_TargetMaskLightA_CE;
  posConstraint = "#effect.TargetCrazyMaskA";
  lifetime = 11.0;
  delay = 6.0;
  fadeInTime = 0.5;
  fadeOutTime = 1.0;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock LightFlareData(AitM_MaskCoronaB_FLARE : AitM_MaskCoronaA_FLARE)
{
  elementTint[0] = "0.8 0 1";
};
datablock afxT3DPointLightData(AitM_TargetMaskLightB_CE : AitM_TargetMaskLightA_CE)
{
  color = "0.8 0 1";
  flareType = AitM_MaskCoronaB_FLARE;
  animationPeriod = 0.6;
  animationPhase = 0.5;
};
datablock afxEffectWrapperData(AitM_TargetMaskLightB_EW)
{
  effect = AitM_TargetMaskLightB_CE;
  posConstraint = "#effect.TargetCrazyMaskB";
  lifetime = 11.0;
  delay = 6.85;
  fadeInTime = 0.5;
  fadeOutTime = 1.0;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function AitM_add_Lighting_FX(%spell_data)
{
  %spell_data.addCastingEffect(AitM_CasterCrazyHeadLight_EW);

  %spell_data.addCastingEffect(AitM_CasterHandLight_lf_hand_EW);
  %spell_data.addCastingEffect(AitM_CasterHandLight_rt_hand_EW);

  %spell_data.addImpactEffect(AitM_ConfusionCloudFlash1_EW);
  %spell_data.addImpactEffect(AitM_ConfusionCloudFlash2_EW);
  %spell_data.addImpactEffect(AitM_TargetMaskLightA_EW);
  %spell_data.addImpactEffect(AitM_TargetMaskLightB_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
