
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// MAPLELEAF FRAG (lighting)
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

datablock afxXM_LocalOffsetData(MLF_MainZodeRevealLight_offset_XM)
{
  localOffset = "0 2 -4";
};
datablock afxXM_SpinData(MLF_MainZodeRevealLight_spin1_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 0;
  spinRate = -30;
};
datablock afxXM_SpinData(MLF_MainZodeRevealLight_spin2_XM : MLF_MainZodeRevealLight_spin1_XM)
{
  spinAngle = 120;
};
datablock afxXM_SpinData(MLF_MainZodeRevealLight_spin3_XM : MLF_MainZodeRevealLight_spin1_XM)
{
  spinAngle = 240;
};
datablock afxXM_AimData(MLF_MainZodeRevealLight_aim_XM)
{
  aimZOnly = false;
};

datablock afxT3DSpotLightData(MLF_MainZodeRevealLight_CE)
{
  range = 7;
  color = "1.0 1.0 1.0";
  brightness = 2.5;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(MLF_MainZodeRevealLight_1_EW)
{
  effect = MLF_MainZodeRevealLight_CE;
  posConstraint = caster;
  posConstraint2 = "caster.#center";
  delay = 0.0;
  lifetime = 0.75;
  fadeInTime = 0.5;
  fadeOutTime = 0.25;
  xfmModifiers[0] = MLF_MainZodeRevealLight_spin1_XM;
    xfmModifiers[1] = MLF_MainZodeRevealLight_offset_XM;
    xfmModifiers[2] = MLF_MainZodeRevealLight_aim_XM;
};
datablock afxEffectWrapperData(MLF_MainZodeRevealLight_2_EW : MLF_MainZodeRevealLight_1_EW)
{
  xfmModifiers[0] = MLF_MainZodeRevealLight_spin2_XM;
};
datablock afxEffectWrapperData(MLF_MainZodeRevealLight_3_EW : MLF_MainZodeRevealLight_1_EW)
{
  xfmModifiers[0] = MLF_MainZodeRevealLight_spin3_XM;
};

//~~~~~~~~~~~~~~~~~~~~//

// this offset defines the lights distance beneath the terrain
datablock afxXM_LocalOffsetData(MLF_CastingZodeLight_offset_XM)
{
  localOffset = "0 0 -2";
};

datablock afxT3DPointLightData(MLF_CastingZodeLight_CE)
{
  radius = 5.0;
  color = "1.0 1.0 0.0";
  brightness = 5;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(MLF_CastingZodeLight_EW : MLF_ZodeEffect1_EW)
{
  effect = MLF_CastingZodeLight_CE;
  xfmModifiers[0] = MLF_CastingZodeLight_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//

//
// purple lights in each hand represent light cast 
// by the purple sparkles. these have a long fade-in
// time that gradually builds up the brightness.
//

datablock afxT3DPointLightData(MLF_MagicLight_CE)
{
  radius = 2.0;
  color = "0.25 0.0 0.5";
  brightness = 3;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(MLF_MagicLight_lf_hand_EW)
{
  effect = MLF_MagicLight_CE;
  constraint = "caster.Bip01 L Hand"; 
  delay    = 0.0;
  lifetime = 5.0;
  fadeInTime  = 4.33;
  fadeOutTime = 0.3;
};
datablock afxEffectWrapperData(MLF_MagicLight_rt_hand_EW : MLF_MagicLight_lf_hand_EW)
{
  constraint = "caster.Bip01 R Hand"; 
  lifetime = 6.2;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock LightAnimData(MLF_ClapLight_ANI)
{
  animEnabled = true;
  minBrightness = 0.9;
  maxBrightness = 1.95;
};

datablock LightFlareData(MLF_ClapLight_FLARE)
{
  overallScale = 1;
  flareEnabled = true;
  flareTexture = %mySpellDataPath @ "/MLF/lights/MLF_clapLightFlare";
  elementRect[0] = "0 0 128 128";
  elementDist[0] = 0.0;
  elementScale[0] = 3.5;
  elementTint[0] = "0.9 0.4 1.0";
  elementRotate[0] = false;
  elementUseLightColor[0] = true;
};

datablock afxT3DPointLightData(MLF_ClapLight_CE)
{
  radius = 9.0;
  color = "0.9 0.4 1.0";
  brightness = 1.5;
  flareType = MLF_ClapLight_FLARE;
  animate = true;
  animationType = MLF_ClapLight_ANI;
  animationPeriod = 0.1;
  animationPhase = 0.0;
  castShadows = false; // T3D Off (temporarily)
  localRenderViz = false;
};

datablock afxEffectWrapperData(MLF_ClapLight_EW)
{
  effect = MLF_ClapLight_CE;
  constraint = caster; 
  delay    = 4.9;
  lifetime = 0.3;
  fadeInTime  = 0.1;
  fadeOutTime = 0.6;
  xfmModifiers[0] = MLF_SparkleClap_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DPointLightData(MLF_MagicMissileLight1_CE)
{
  radius = 7.0;
  color = "0.5 0.0 1.0";
  brightness = 1.0;
  castShadows = false;
  localRenderViz = false;
};

datablock afxT3DPointLightData(MLF_MagicMissileLight2_CE)
{
  radius = 5.0;
  color = "1.0 1.0 1.0";
  brightness = 0.75;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(MLF_MagicMissileLight1_EW : MLF_MagicMissileBOT_EW)
{
  effect = MLF_MagicMissileLight1_CE;
};

datablock afxEffectWrapperData(MLF_MagicMissileLight2_EW : MLF_MagicMissileBOT_EW)
{
  effect = MLF_MagicMissileLight2_CE;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DPointLightData(MLF_TornadoLight_CE)
{
  radius = 24.0;
  color = "1.0 1.0 0.7";
  brightness = 0.8;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(MLF_TornadoLight_EW)
{
  effect = MLF_TornadoLight_CE;
  constraint = missile;
  delay = 0.75;
  fadeInTime = 1.5;
  fadeOutTime = 0.25;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DPointLightData(MLF_FunnelImpactLight_CE)
{
  radius = 4.0;
  color = "1.0 1.0 0.7";
  brightness = 1.0;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(MLF_FunnelImpactLight_EW)
{
  effect = MLF_FunnelImpactLight_CE;
  posConstraint = "impactedObject";
  lifetime = 0.5;
  delay = 0;
  fadeInTime = 0.5;
  fadeOutTime = 1.5;
  xfmModifiers[0] = MLF_LeafFunnelImpact_3_offset_XM;
  execConditions[0] = $AFX::IMPACTED_SOMETHING;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function MLF_add_Lighting_FX(%spell_data)
{
  %spell_data.addCastingEffect(MLF_MainZodeRevealLight_1_EW);
  %spell_data.addCastingEffect(MLF_MainZodeRevealLight_2_EW);
  %spell_data.addCastingEffect(MLF_MainZodeRevealLight_3_EW);

  %spell_data.addCastingEffect(MLF_CastingZodeLight_EW);
  %spell_data.addCastingEffect(MLF_MagicLight_lf_hand_EW);
  %spell_data.addCastingEffect(MLF_MagicLight_rt_hand_EW);

  %spell_data.addCastingEffect(MLF_ClapLight_EW);

  %spell_data.addDeliveryEffect(MLF_MagicMissileLight1_EW);
  %spell_data.addDeliveryEffect(MLF_MagicMissileLight2_EW);
  %spell_data.addDeliveryEffect(MLF_TornadoLight_EW);
  %spell_data.addImpactEffect(MLF_FunnelImpactLight_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
