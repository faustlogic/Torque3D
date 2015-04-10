
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// REAPER MADNESS (lighting)
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

datablock afxXM_LocalOffsetData(RM_MainZodeRevealLight_offset_XM)
{
   localOffset = "0 2 -4";
};
datablock afxXM_SpinData(RM_MainZodeRevealLight_spin1_XM)
{
   spinAxis = "0 0 1";
   spinAngle = 0;
   spinRate = -30;
};
datablock afxXM_SpinData(RM_MainZodeRevealLight_spin2_XM : RM_MainZodeRevealLight_spin1_XM)
{
   spinAngle = 120;
};
datablock afxXM_SpinData(RM_MainZodeRevealLight_spin3_XM : RM_MainZodeRevealLight_spin1_XM)
{
   spinAngle = 240;
};
datablock afxXM_AimData(RM_MainZodeRevealLight_aim_XM)
{
  aimZOnly = false;
};

datablock afxT3DSpotLightData(RM_MainZodeRevealLight_CE)
{
  range = 10;
  color = "1.0 1.0 1.0";
  brightness = 2.5;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(RM_MainZodeRevealLight_1_EW : RM_ZodeReveal_EW)
{
  effect = RM_MainZodeRevealLight_CE;
  posConstraint = caster;
  posConstraint2 = "caster.#center";
  xfmModifiers[0] = RM_MainZodeRevealLight_spin1_XM;
  xfmModifiers[1] = RM_MainZodeRevealLight_offset_XM;
  xfmModifiers[2] = RM_MainZodeRevealLight_aim_XM;
};
datablock afxEffectWrapperData(RM_MainZodeRevealLight_2_EW : RM_MainZodeRevealLight_1_EW)
{
  xfmModifiers[0] = RM_MainZodeRevealLight_spin2_XM;
};
datablock afxEffectWrapperData(RM_MainZodeRevealLight_3_EW : RM_MainZodeRevealLight_1_EW)
{
  xfmModifiers[0] = RM_MainZodeRevealLight_spin3_XM;
};

//~~~~~~~~~~~~~~~~~~~~//

%RM_CastingZodeLight_LMODELS_intensity  = 25.0;

// this offset defines the lights distance beneath the terrain
datablock afxXM_LocalOffsetData(RM_CastingZodeLight_offset_XM)
{
  localOffset = "0 0 -2";
};

datablock afxT3DPointLightData(RM_CastingZodeLight_CE)
{
  radius = 3;
  color = "1.0 0.392 0.416";
  brightness = 8;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(RM_CastingZodeLight_EW : RM_Zode1_EW)
{
  effect = RM_CastingZodeLight_CE;
  xfmModifiers[0] = RM_CastingZodeLight_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock LightFlareData(RM_FireLight_FLARE)
{
  overallScale = 1.0;
  flareEnabled = true;
  flareTexture = %mySpellDataPath @ "/RM/lights/RM_lightFalloffMono";
  elementRect[0] = "0 0 256 256";
  elementDist[0] = 0.0;
  elementScale[0] = 2.0;
  elementTint[0] = "1.0 0.7 0.0";
  elementRotate[0] = false;
  elementUseLightColor[0] = true;
};

datablock afxT3DPointLightData(RM_FireLight_CE)
{
  radius = 2;
  color = "1.0 0.7 0.0";
  brightness = 0.2;
  castShadows = false;
  localRenderViz = false;
  flareType = RM_FireLight_FLARE;
};

datablock afxEffectWrapperData(RM_FireLight1_EW)
{
  effect = RM_FireLight_CE;
  posConstraint = caster;
  delay = 1.8;
  lifetime = 2.0;
  fadeInTime  = 0.3;
  fadeOutTime = 0.3;
  xfmModifiers[0] = RM_Fire1_offset_XM;
  xfmModifiers[1] = RM_FireFlicker1_path_XM;
};
datablock afxEffectWrapperData(RM_FireLight2_EW)
{
  effect = RM_FireLight_CE;
  posConstraint = caster;
  delay = 1.8;
  lifetime = 2.0;
  fadeInTime  = 0.3;
  fadeOutTime = 0.3;
  xfmModifiers[0] = RM_Fire2_offset_XM;
  xfmModifiers[1] = RM_FireFlicker2_path_XM;
};
datablock afxEffectWrapperData(RM_FireLight3_EW)
{
  effect = RM_FireLight_CE;
  posConstraint = caster;
  delay = 1.8;
  lifetime = 2.0;
  fadeInTime  = 0.3;
  fadeOutTime = 0.3;
  xfmModifiers[0] = RM_Fire3_offset_XM;
  xfmModifiers[1] = RM_FireFlicker3_path_XM;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxXM_LocalOffsetData(RM_CorpseDevilRevealLight_offset_XM)
{
  localOffset = "0 2 -4";
};
datablock afxXM_SpinData(RM_CorpseDevilRevealLight_spin1_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 0;
  spinRate = -30;
};
datablock afxXM_SpinData(RM_CorpseDevilRevealLight_spin2_XM : RM_CorpseDevilRevealLight_spin1_XM)
{
  spinAngle = 120;
};
datablock afxXM_SpinData(RM_CorpseDevilRevealLight_spin3_XM : RM_CorpseDevilRevealLight_spin1_XM)
{
  spinAngle = 240;
};
datablock afxXM_AimData(RM_CorpseDevilRevealLight_aim_XM)
{
  aimZOnly = false;
};

datablock afxT3DSpotLightData(RM_CorpseDevilRevealLight_CE)
{
  range = 10;
  color = "1.0 1.0 1.0";
  brightness = 2.5;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(RM_CorpseDevilRevealLight_1_EW : RM_CorpseDevilReveal_EW)
{
  effect = RM_CorpseDevilRevealLight_CE;
  posConstraint = target;
  posConstraint2 = "target.#center";
  xfmModifiers[0] = RM_CorpseDevilRevealLight_spin1_XM;
  xfmModifiers[1] = RM_CorpseDevilRevealLight_offset_XM;
  xfmModifiers[2] = RM_CorpseDevilRevealLight_aim_XM;
};
datablock afxEffectWrapperData(RM_CorpseDevilRevealLight_2_EW : RM_CorpseDevilRevealLight_1_EW)
{
  xfmModifiers[0] = RM_CorpseDevilRevealLight_spin2_XM;
};
datablock afxEffectWrapperData(RM_CorpseDevilRevealLight_3_EW : RM_CorpseDevilRevealLight_1_EW)
{
  xfmModifiers[0] = RM_CorpseDevilRevealLight_spin3_XM;
};

//~~~~~~~~~~~~~~~~~~~~//


// this offset defines the lights distance beneath the terrain
datablock afxXM_LocalOffsetData(RM_CorpseDevilLight_offset_XM)
{
  localOffset = "0 0 -4";
};
datablock afxXM_LocalOffsetData(RM_CorpseDevilLight_offset2_XM)
{
  localOffset = "0 0 -2";
};

datablock afxT3DPointLightData(RM_CorpseDevilLight_CE)
{
  radius = 5;
  color = "0.769 0.220 0.302";
  brightness = 10;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(RM_CorpseDevilLight_EW : RM_CorpseDevil_EW)
{
  effect = RM_CorpseDevilLight_CE;
  xfmModifiers[0] = RM_CorpseDevilLight_offset2_XM;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DPointLightData(RM_CorpseSunburstLight_CE)
{
  radius = 15;
  color = "1.0 1.0 1.0";
  brightness = 5;
  castShadows = false;
  localRenderViz = false;
};

datablock afxXM_LocalOffsetData(RM_CorpseSunburstLight_offset_XM)
{
  localOffset = "0.0 0.0 2.0";
};

datablock afxEffectWrapperData(RM_CorpseSunburstLight_EW)
{
  effect = RM_CorpseSunburstLight_CE;
  posConstraint = "target";
  delay = 5.5;
  fadeInTime  = 4.0;
  fadeOutTime = 0.5;
  lifetime = 4.2;

  xfmModifiers[0] = RM_CorpseSunburstLight_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

// add effects to main spell datablock
function RM_add_Lighting_FX(%spell_data)
{
  %spell_data.addCastingEffect(RM_MainZodeRevealLight_1_EW);
  %spell_data.addCastingEffect(RM_MainZodeRevealLight_2_EW);
  %spell_data.addCastingEffect(RM_MainZodeRevealLight_3_EW);
  %spell_data.addCastingEffect(RM_CastingZodeLight_EW);

  %spell_data.addCastingEffect(RM_FireLight1_EW);
  %spell_data.addCastingEffect(RM_FireLight2_EW);
  %spell_data.addCastingEffect(RM_FireLight3_EW);

  %spell_data.addCastingEffect(RM_CorpseDevilRevealLight_1_EW);
  %spell_data.addCastingEffect(RM_CorpseDevilRevealLight_2_EW);
  %spell_data.addCastingEffect(RM_CorpseDevilRevealLight_3_EW);
  %spell_data.addCastingEffect(RM_CorpseDevilLight_EW);
  %spell_data.addCastingEffect(RM_CorpseSunburstLight_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
