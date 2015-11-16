
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// INSECTOPLASM (lighting)
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

datablock afxXM_LocalOffsetData(IOP_CastingZodeRevealLight_offset_XM)
{
  localOffset = "0 2 -4";
};
datablock afxXM_SpinData(IOP_CastingZodeRevealLight_spin1_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 0;
  spinRate = -30;
};
datablock afxXM_SpinData(IOP_CastingZodeRevealLight_spin2_XM : IOP_CastingZodeRevealLight_spin1_XM)
{
  spinAngle = 120;
};
datablock afxXM_SpinData(IOP_CastingZodeRevealLight_spin3_XM : IOP_CastingZodeRevealLight_spin1_XM)
{
  spinAngle = 240;
};
datablock afxXM_AimData(IOP_CastingZodeRevealLight_aim_XM)
{
  aimZOnly = false;
};

datablock afxT3DSpotLightData(IOP_CastingZodeRevealLight_CE)
{
  range = 7;
  color = "2.5 2.5 2.5";
  brightness = 0.5;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(IOP_CastingZodeRevealLight_1_EW)
{
  effect = IOP_CastingZodeRevealLight_CE;
  posConstraint = caster;
  posConstraint2 = "caster.#center";
  delay = 0.0;
  lifetime = 0.7;
  fadeInTime = 0.50;
  fadeOutTime = 0.20;
  xfmModifiers[0] = IOP_CastingZodeRevealLight_spin1_XM;
    xfmModifiers[1] = IOP_CastingZodeRevealLight_offset_XM;
    xfmModifiers[2] = IOP_CastingZodeRevealLight_aim_XM;
};
datablock afxEffectWrapperData(IOP_CastingZodeRevealLight_2_EW : IOP_CastingZodeRevealLight_1_EW)
{
  xfmModifiers[0] = IOP_CastingZodeRevealLight_spin2_XM;
};
datablock afxEffectWrapperData(IOP_CastingZodeRevealLight_3_EW : IOP_CastingZodeRevealLight_1_EW)
{
  xfmModifiers[0] = IOP_CastingZodeRevealLight_spin3_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

// this offset defines the lights distance beneath the terrain
datablock afxXM_LocalOffsetData(IOP_CastingZodeLight_offset_XM)
{
  localOffset = "0 0 -4";
};

datablock afxT3DPointLightData(IOP_CastingZodeLight_CE)
{
  radius = 7;
  color = "2.5 0.05 1.0";
  brightness = 2;
  castShadows = false;
  localRenderViz = false;
};
//
datablock afxEffectWrapperData(IOP_CastingZodeLight_EW)
{
  effect = IOP_CastingZodeLight_CE;
  posConstraint = caster;
  delay = 0.25;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 15.5;
  xfmModifiers[0] = IOP_CastingZodeLight_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

// this offset defines the lights distance beneath the terrain
datablock afxXM_LocalOffsetData(IOP_BeamLight_offset_XM)
{
  localOffset = "0 0 -4";
};

datablock afxT3DPointLightData(IOP_BeamLight_CE)
{
  radius = 8;
  color = "3.0 3.0 3.0";
  brightness = 1;
  castShadows = false;
  localRenderViz = false;
};
//
datablock afxEffectWrapperData(IOP_BeamLight_EW : IOP_BeamZodiac_EW)
{
  effect = IOP_BeamLight_CE;
  xfmModifiers[0] = "IOP_BeamLight_offset_XM";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GIANT INSECT LIGHTS

//  
// Corresponding to each magic insect emitter (see "GIANT INSECT
// MAGIC (FIRE)" above) is a light, that adds a nice effect as the 
// monster twists near the ground or close to a building.  The 
// radiuses of the lights shrink the further they are along the back,
// as do the particle sizes.
//
// These lights are very expensive in terms of frame rate, and
// therefore not all can be used.  "levelOfDetailRange" is used to 
// partially control this.
//

// Insect Light Variables
//
%IOP_GiantInsectLight_RadiusFactor = 5.5;
%IOP_GiantInsectLight_IntensityFactor = 0.3;

// Insect Lights
//
datablock afxT3DPointLightData(IOP_GiantInsectLight_1_CE)
{
  radius = %IOP_GiantInsectMagic_1_Size*%IOP_GiantInsectLight_RadiusFactor;
  color = "2.5 1.93 0.42";
  castShadows = false; // T3D Off (temporarily)
  localRenderViz = false;
};
datablock afxT3DPointLightData(IOP_GiantInsectLight_4_CE : IOP_GiantInsectLight_1_CE)
{
  radius = %IOP_GiantInsectMagic_4_Size*%IOP_GiantInsectLight_RadiusFactor;
  castShadows = false;
};
datablock afxT3DPointLightData(IOP_GiantInsectLight_7_CE : IOP_GiantInsectLight_1_CE)
{
  radius = %IOP_GiantInsectMagic_7_Size*%IOP_GiantInsectLight_RadiusFactor;
  castShadows = false;
};
datablock afxT3DPointLightData(IOP_GiantInsectLight_10_CE : IOP_GiantInsectLight_1_CE)
{
  radius = %IOP_GiantInsectMagic_10_Size*%IOP_GiantInsectLight_RadiusFactor;
  castShadows = false;
};
datablock afxT3DPointLightData(IOP_GiantInsectLight_13_CE : IOP_GiantInsectLight_1_CE)
{
  radius = %IOP_GiantInsectMagic_13_Size*%IOP_GiantInsectLight_RadiusFactor;
  castShadows = false;
};
datablock afxT3DPointLightData(IOP_GiantInsectLight_16_CE : IOP_GiantInsectLight_1_CE)
{
  radius = %IOP_GiantInsectMagic_16_Size*%IOP_GiantInsectLight_RadiusFactor;
  castShadows = false;
};
datablock afxT3DPointLightData(IOP_GiantInsectLight_19_CE : IOP_GiantInsectLight_1_CE)
{
  radius = %IOP_GiantInsectMagic_19_Size*%IOP_GiantInsectLight_RadiusFactor;
  castShadows = false;
};

datablock afxEffectWrapperData(IOP_GiantInsectLight_1_EW)
{
  effect = IOP_GiantInsectLight_1_CE;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_1_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_1_Delay;
  scaleFactor   = %IOP_GiantInsectLight_IntensityFactor;
  levelOfDetailRange = 0;
};
datablock afxEffectWrapperData(IOP_GiantInsectLight_4_EW : IOP_GiantInsectLight_1_EW)
{
  effect = IOP_GiantInsectLight_4_CE;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_4_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_4_Delay;
  levelOfDetailRange = 2;
};
datablock afxEffectWrapperData(IOP_GiantInsectLight_7_EW : IOP_GiantInsectLight_1_EW)
{
  effect = IOP_GiantInsectLight_7_CE;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_7_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_7_Delay;
  levelOfDetailRange = 2;
};
datablock afxEffectWrapperData(IOP_GiantInsectLight_10_EW : IOP_GiantInsectLight_1_EW)
{
  effect = IOP_GiantInsectLight_10_CE;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_10_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_10_Delay;
  levelOfDetailRange = 1;
};
datablock afxEffectWrapperData(IOP_GiantInsectLight_13_EW : IOP_GiantInsectLight_1_EW)
{
  effect = IOP_GiantInsectLight_13_CE;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_13_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_13_Delay;
  levelOfDetailRange = 2;
};
datablock afxEffectWrapperData(IOP_GiantInsectLight_16_EW : IOP_GiantInsectLight_1_EW)
{
  effect = IOP_GiantInsectLight_16_CE;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_16_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_16_Delay;
  levelOfDetailRange = 2;
};
datablock afxEffectWrapperData(IOP_GiantInsectLight_19_EW : IOP_GiantInsectLight_1_EW)
{
  effect = IOP_GiantInsectLight_19_CE;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_19_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_19_Delay;
  levelOfDetailRange = 1;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock LightFlareData(IOP_GiantInsectMagicImpactLight_small_FLARE)
{
  overallScale = 10.0;
  flareEnabled = true;
  flareTexture = %mySpellDataPath @ "/IOP/lights/IOP_corona";
  elementRect[0] = "0 0 128 128";
  elementDist[0] = 0.0;
  elementScale[0] = 2.0;
  elementTint[0] = "1.0 0.773 0.168";
  elementRotate[0] = false;
  elementUseLightColor[0] = true;
};
datablock LightFlareData(IOP_GiantInsectMagicImpactLight_medium_FLARE : IOP_GiantInsectMagicImpactLight_small_FLARE)
{
   overallScale = 20.0;
};
datablock LightFlareData(IOP_GiantInsectMagicImpactLight_large_FLARE : IOP_GiantInsectMagicImpactLight_small_FLARE)
{
   overallScale = 30.0;
};

datablock afxT3DPointLightData(IOP_GiantInsectMagicImpactLight_small_CE)
{
  radius = 7.0;
  color = "1.0 0.773 0.168";
  brightness = 0.8;
  flareType = IOP_GiantInsectMagicImpactLight_small_FLARE;
  castShadows = false;
  localRenderViz = false;
};
datablock afxT3DPointLightData(IOP_GiantInsectMagicImpactLight_medium_CE : IOP_GiantInsectMagicImpactLight_small_CE)
{
  radius = 12.0;
  flareType = IOP_GiantInsectMagicImpactLight_medium_FLARE;
};
datablock afxT3DPointLightData(IOP_GiantInsectMagicImpactLight_large_CE : IOP_GiantInsectMagicImpactLight_small_CE)
{
  radius = 20.0;
  flareType = IOP_GiantInsectMagicImpactLight_large_FLARE;
};

datablock afxXM_AimData(IOP_GiantInsectMagicImpactLight_aim_XM)
{
  aimZOnly = false;
};
datablock afxXM_LocalOffsetData(IOP_GiantInsectMagicImpactLight_offset_XM)
{
  localOffset = "0 5.0 0.2";
};

%IOP_GiantInsectMagicImpactLight_fadeout_small  = 0.15;
%IOP_GiantInsectMagicImpactLight_fadeout_medium = 0.30;
%IOP_GiantInsectMagicImpactLight_fadeout_large  = 0.50;

datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_1_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_large_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_large;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_2_EW : IOP_GiantInsectMagicImpact_2_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_small_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_small;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_3_EW : IOP_GiantInsectMagicImpact_3_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_medium_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_medium;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_4_EW : IOP_GiantInsectMagicImpact_4_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_small_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_small;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_5_EW : IOP_GiantInsectMagicImpact_5_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_large_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_large;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_6_EW : IOP_GiantInsectMagicImpact_6_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_medium_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_medium;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_7_EW : IOP_GiantInsectMagicImpact_7_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_small_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_small;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_8_EW : IOP_GiantInsectMagicImpact_8_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_small_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_small;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_9_EW : IOP_GiantInsectMagicImpact_9_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_medium_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_medium;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_10_EW : IOP_GiantInsectMagicImpact_10_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_large_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_large;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_11_EW : IOP_GiantInsectMagicImpact_11_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_small_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_small;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_12_EW : IOP_GiantInsectMagicImpact_12_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_large_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_large;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_13_EW : IOP_GiantInsectMagicImpact_13_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_medium_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_medium;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_14_EW : IOP_GiantInsectMagicImpact_14_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_large_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_large;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_15_EW : IOP_GiantInsectMagicImpact_15_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_medium_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_medium;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_16_EW : IOP_GiantInsectMagicImpact_16_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_large_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_large;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_17_EW : IOP_GiantInsectMagicImpact_17_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_large_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_large;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_18_EW : IOP_GiantInsectMagicImpact_18_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_medium_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_medium;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpactLight_19_EW : IOP_GiantInsectMagicImpact_19_EW)
{
  effect = IOP_GiantInsectMagicImpactLight_small_CE;
  fadeInTime = 0.05;
  lifetime = 0.05;
  fadeOutTime = %IOP_GiantInsectMagicImpactLight_fadeout_small;
  xfmModifiers[0] = IOP_GiantInsectMagicImpactLight_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectMagicImpactLight_offset_XM;
  posConstraint2 = "camera";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function IOP_add_Lighting_FX(%spell_data)
{
  %spell_data.addCastingEffect(IOP_CastingZodeRevealLight_1_EW);
  %spell_data.addCastingEffect(IOP_CastingZodeRevealLight_2_EW);
  %spell_data.addCastingEffect(IOP_CastingZodeRevealLight_3_EW);
  %spell_data.addCastingEffect(IOP_CastingZodeLight_EW);
  %spell_data.addCastingEffect(IOP_BeamLight_EW);

  %spell_data.addDeliveryEffect(IOP_GiantInsectLight_1_EW);
  %spell_data.addDeliveryEffect(IOP_GiantInsectLight_4_EW);
  %spell_data.addDeliveryEffect(IOP_GiantInsectLight_7_EW);
  %spell_data.addDeliveryEffect(IOP_GiantInsectLight_10_EW);
  %spell_data.addDeliveryEffect(IOP_GiantInsectLight_13_EW);
  %spell_data.addDeliveryEffect(IOP_GiantInsectLight_16_EW);
  %spell_data.addDeliveryEffect(IOP_GiantInsectLight_19_EW);

  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_1_EW);
  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_2_EW);
  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_3_EW);
  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_4_EW);
  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_5_EW);
  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_6_EW);
  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_7_EW);
  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_8_EW);
  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_9_EW);
  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_10_EW);
  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_11_EW);
  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_12_EW);
  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_13_EW);
  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_14_EW);
  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_15_EW);
  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_16_EW);
  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_17_EW);
  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_18_EW);
  %spell_data.addImpactEffect(IOP_GiantInsectMagicImpactLight_19_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
