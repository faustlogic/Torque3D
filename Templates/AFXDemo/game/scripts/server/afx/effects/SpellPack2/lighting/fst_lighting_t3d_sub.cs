
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// FLAMING STICK TRICK (lighting)
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


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING ZODIAC LIGHTS

//
// For many of the zodiacs, lights are used to simulate light cast by
//  the zodiacs onto the caster and into the environment.
//

datablock afxXM_LocalOffsetData(FST_CastingZodeRevealLight_offset_XM)
{
  localOffset = "0" SPC (2.0*$FST_CastingScale) SPC (-4.0*$FST_CastingScale);
};
datablock afxXM_SpinData(FST_CastingZodeRevealLight_spin1_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 0;
  spinRate = -30;
};
datablock afxXM_SpinData(FST_CastingZodeRevealLight_spin2_XM : FST_CastingZodeRevealLight_spin1_XM)
{
  spinAngle = 120;
};
datablock afxXM_SpinData(FST_CastingZodeRevealLight_spin3_XM : FST_CastingZodeRevealLight_spin1_XM)
{
  spinAngle = 240;
};
datablock afxXM_AimData(FST_CastingZodeRevealLight_aim_XM)
{
  aimZOnly = false;
};

// main zode reveal light
datablock afxT3DSpotLightData(FST_CastingZodeRevealLight_CE)
{
  range = 12;
  color = "2.5 2.5 2.5";
  brightness = 1;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(FST_CastingZodeRevealLight_1_EW)
{
  effect = FST_CastingZodeRevealLight_CE;
  posConstraint = "zodeAnchor";
  posConstraint2 = "zodeAnchor"; // note - zodeAnchor works here because posConstraint will be offset 
  delay = $FST_Cue_RevealZodiac;
  lifetime = 2.2;
  fadeInTime = 0.5;
  fadeOutTime = 0.25;
  xfmModifiers[0] = FST_CastingZodeRevealLight_spin1_XM;
    xfmModifiers[1] = FST_CastingZodeRevealLight_offset_XM;
    xfmModifiers[2] = FST_CastingZodeRevealLight_aim_XM;
};
datablock afxEffectWrapperData(FST_CastingZodeRevealLight_2_EW : FST_CastingZodeRevealLight_1_EW)
{
  xfmModifiers[0] = FST_CastingZodeRevealLight_spin2_XM;
};
datablock afxEffectWrapperData(FST_CastingZodeRevealLight_3_EW : FST_CastingZodeRevealLight_1_EW)
{
  xfmModifiers[0] = FST_CastingZodeRevealLight_spin3_XM;
};

//~~~~~~~~~~~~~~~~~~~~//

// this offset defines the lights distance beneath the terrain
datablock afxXM_LocalOffsetData(FST_CastingZodeLight_offset_XM)
{
  localOffset = "0 0" SPC (-4.0*$FST_CastingScale);
};

datablock afxT3DPointLightData(FST_CastingZodeLight_CE)
{
  radius = 7*$FST_CastingScale;
  color = "1.5 0.6 0.3";
  brightness = 1;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(FST_CastingZodeLight_EW : FST_CastingZode_Main_EW)
{
  effect = FST_CastingZodeLight_CE;
  xfmModifiers[0] = FST_CastingZodeLight_offset_XM;
};

datablock afxT3DPointLightData(FST_PentagramZodeLight_CE)
{
  radius = 5*$FST_CastingScale;
  color = "1.5 1.48 0.01";
  brightness = 1;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(FST_PentagramZodeLight_EW : FST_CastingZode_Pentagram_EW)
{
  effect = FST_PentagramZodeLight_CE;
  xfmModifiers[0] = FST_CastingZodeLight_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock LightFlareData(FST_StaffFire_Flare_FLARE)
{
  overallScale = 1.0;
  flareEnabled = true;
  flareTexture = %mySpellDataPath @ "/Shared/lights/sp2_falloffFlare";
  elementRect[0] = "0 0 256 256";
  elementDist[0] = 0.0;
  elementScale[0] = 6.0;
  elementTint[0] = "1 1 1";
  elementRotate[0] = true;
  elementUseLightColor[0] = true;
};

datablock LightAnimData(FST_StaffFire_Flare_ANI)
{
  animEnabled = true;
  minBrightness = 0.7;
  maxBrightness = 1.0;
};

datablock afxT3DPointLightData(FST_StaffFire_Flare_CE)
{
  radius = 6;
  color = "1.0 0.6 0.0";
  brightness = 10;
  castShadows = false; // T3D_OFF (temporarily)
  localRenderViz = false;
  flareType = FST_StaffFire_Flare_FLARE;
  flareScale = 1;
  animate = true;
  animationType = FST_StaffFire_Flare_ANI;
  animationPeriod = 0.3;
  animationPhase = 0.0;
};

datablock afxEffectWrapperData(FST_StaffFire_Flare_Top_EW : FST_StaffFireBig_Top_EW)
{
  effect = FST_StaffFire_Flare_CE;
  constraint = "#effect.FireStaff.MountTop";
  delay = $FST_Cue_StaffAppearance;
  lifetime = 0.4;
  fadeInTime  = 0.1;
  fadeOutTime = 0.5;
  xfmModifiers[0] = FST_StaffFire_Top_offset_XM;
}; 

datablock afxEffectWrapperData(FST_StaffFire_Flare_Bot_EW : FST_StaffFireBig_Bot_EW)
{
  effect = FST_StaffFire_Flare_CE;
  constraint = "#effect.FireStaff.MountBottom";
  delay = $FST_Cue_StaffAppearance;
  lifetime = 0.4;
  fadeInTime  = 0.1;
  fadeOutTime = 0.5;
  xfmModifiers[0] = FST_StaffFire_Bot_offset_XM;
}; 

//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DPointLightData(FST_StaffFire_Light_CE)
{
  radius = 3;
  color = "1.0 0.6 0.1";
  brightness = 3;
  castShadows = false;
  localRenderViz = false;
};
datablock afxT3DPointLightData(FST_StaffFire_BigLight_CE : FST_StaffFire_Light_CE)
{
  radius = 7.0;
};

/*
datablock sgLightObjectData(FST_StaffFire_Light_CE)
{
  LightOn = true;
  CastsShadows = false;
  Radius = 3.0;
  Brightness = 3.0;
  Colour = "1.0 0.6 0.1";
  LightingModelName = "Original Stock";
  lightIlluminationMask = $AFX::ILLUM_DTS | $AFX::ILLUM_DIF; // TGEA (ignored by TGE)
};
datablock sgLightObjectData(FST_StaffFire_BigLight_CE : FST_StaffFire_Light_CE)
{
  Radius = 7.0;
};
*/

datablock afxEffectWrapperData(FST_StaffFire_Light_Top_EW : FST_StaffFire_Top_EW)
{
  effect = FST_StaffFire_Light_CE;
  fadeInTime  = 0.5;
  fadeOutTime = 0.5;
}; 
datablock afxEffectWrapperData(FST_StaffFire_Light_Bot_EW : FST_StaffFire_Bot_EW)
{
  effect = FST_StaffFire_Light_CE;
  fadeInTime  = 0.5;
  fadeOutTime = 0.5;
}; 
datablock afxEffectWrapperData(FST_StaffStickFire_Light_EW : FST_StaffStickFire_Top_EW)
{
  effect = FST_StaffFire_BigLight_CE;
  lifetime = $FST_StuckStaff_Life+0.5;
  fadeInTime  = 0.5;
  fadeOutTime = 0.5;
}; 

//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DPointLightData(FST_StaffStrikeLight_CE)
{
  radius = 5;
  color = "1.0 1.0 1.0";
  brightness = 3;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(FST_StaffStrikeLight_EW)
{
  effect = FST_StaffStrikeLight_CE;
  posConstraint = "strikeLoc";
  delay = $FST_Cue_StaffPlant-(2/30);
  lifetime = 0.89;
  fadeinTime = 0.3;
  fadeOutTime = 0.3;
};

datablock afxEffectWrapperData(FST_StaffIncinerateLight_EW : FST_StaffStrikeZodiacC_EW)
{
  effect = FST_StaffStrikeLight_CE;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function FST_add_Lighting_FX(%spell_data)
{
  %spell_data.addCastingEffect(FST_CastingZodeRevealLight_1_EW);
  %spell_data.addCastingEffect(FST_CastingZodeRevealLight_2_EW);
  %spell_data.addCastingEffect(FST_CastingZodeRevealLight_3_EW);
  %spell_data.addCastingEffect(FST_CastingZodeLight_EW);
  %spell_data.addCastingEffect(FST_PentagramZodeLight_EW);
  %spell_data.addCastingEffect(FST_StaffFire_Flare_Top_EW);
  %spell_data.addCastingEffect(FST_StaffFire_Flare_Bot_EW);
  %spell_data.addCastingEffect(FST_StaffFire_Light_Top_EW);
  %spell_data.addCastingEffect(FST_StaffFire_Light_Bot_EW);
  %spell_data.addCastingEffect(FST_StaffStickFire_Light_EW);
  %spell_data.addCastingEffect(FST_StaffStrikeLight_EW);
  %spell_data.addCastingEffect(FST_StaffIncinerateLight_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
