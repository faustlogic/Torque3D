
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SUMMON FECKLESS MOTH (lighting)
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

datablock afxXM_LocalOffsetData(SFM_MainZodeRevealLight_offset_XM)
{
  localOffset = "0 2 -4";
};
datablock afxXM_SpinData(SFM_MainZodeRevealLight_spin1_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 0;
  spinRate = -30;
};
datablock afxXM_SpinData(SFM_MainZodeRevealLight_spin2_XM : SFM_MainZodeRevealLight_spin1_XM)
{
  spinAngle = 120;
};
datablock afxXM_SpinData(SFM_MainZodeRevealLight_spin3_XM : SFM_MainZodeRevealLight_spin1_XM)
{
  spinAngle = 240;
};
datablock afxXM_AimData(SFM_MainZodeRevealLight_aim_XM)
{
  aimZOnly = false;
};

datablock afxT3DSpotLightData(SFM_MainZodeRevealLight_CE)
{
  range = 7;
  color = "2.5 2.5 2.5";
  brightness = 0.5;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(SFM_MainZodeRevealLight_1_EW)
{
  effect = SFM_MainZodeRevealLight_CE;
  posConstraint = caster;
  posConstraint2 = "caster.#center";
  delay = 0.0;
  lifetime = 0.75;
  fadeInTime = 0.5;
  fadeOutTime = 0.25;
  xfmModifiers[0] = SFM_MainZodeRevealLight_spin1_XM;
    xfmModifiers[1] = SFM_MainZodeRevealLight_offset_XM;
    xfmModifiers[2] = SFM_MainZodeRevealLight_aim_XM;
};
datablock afxEffectWrapperData(SFM_MainZodeRevealLight_2_EW : SFM_MainZodeRevealLight_1_EW)
{
  xfmModifiers[0] = SFM_MainZodeRevealLight_spin2_XM;
};
datablock afxEffectWrapperData(SFM_MainZodeRevealLight_3_EW : SFM_MainZodeRevealLight_1_EW)
{
  xfmModifiers[0] = SFM_MainZodeRevealLight_spin3_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxXM_LocalOffsetData(SFM_CastingZodeLight_offset2_XM)
{
  localOffset = "0 0 -2";
};

datablock afxT3DPointLightData(SFM_CastingZodeLight_CE)
{
  radius = 5.0;
  color = "0.451 0.79 1.0";
  brightness = 8;
  castShadows = false;
  localRenderViz = false;
};
  
datablock afxEffectWrapperData(SFM_CastingZodeLight_EW : SFM_Zode1_EW)
{
  effect = SFM_CastingZodeLight_CE;
  xfmModifiers[0] = SFM_CastingZodeLight_offset2_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

%SFM_LarvePulseDelay = 1.0;
%SFM_LarvePulseLight_delay_1 = %SFM_LarvePulseDelay + (0/30);
%SFM_LarvePulseLight_delay_2 = %SFM_LarvePulseDelay + (38/30);
%SFM_LarvePulseLight_delay_3 = %SFM_LarvePulseDelay + (84/30);
%SFM_LarvePulseLight_delay_4 = %SFM_LarvePulseDelay + (127/30);
%SFM_LarvePulseLight_delay_5 = %SFM_LarvePulseDelay + (166/30);
%SFM_LarvePulseLight_delay_6 = %SFM_LarvePulseDelay + (202/30);
%SFM_LarvePulseLight_delay_7 = %SFM_LarvePulseDelay + (234/30);

// lifetime including fade-out
%SFM_LarvePulseLight_lifetime_1 = (38-0)/30;
%SFM_LarvePulseLight_lifetime_2 = (84-38)/30;
%SFM_LarvePulseLight_lifetime_3 = (127-84)/30;
%SFM_LarvePulseLight_lifetime_4 = (166-127)/30;
%SFM_LarvePulseLight_lifetime_5 = (202-166)/30;
%SFM_LarvePulseLight_lifetime_6 = (234-202)/30;
%SFM_LarvePulseLight_lifetime_7 = (315-234)/30;

%SFM_LarvePulseLight_fadein_1 = %SFM_LarvePulseLight_lifetime_1 * 0.65;
%SFM_LarvePulseLight_fadein_2 = %SFM_LarvePulseLight_lifetime_2 * 0.65;
%SFM_LarvePulseLight_fadein_3 = %SFM_LarvePulseLight_lifetime_3 * 0.65;
%SFM_LarvePulseLight_fadein_4 = %SFM_LarvePulseLight_lifetime_4 * 0.65;
%SFM_LarvePulseLight_fadein_5 = %SFM_LarvePulseLight_lifetime_5 * 0.65;
%SFM_LarvePulseLight_fadein_6 = %SFM_LarvePulseLight_lifetime_6 * 0.65;
%SFM_LarvePulseLight_fadein_7 = %SFM_LarvePulseLight_lifetime_7 * 0.65;

%SFM_LarvePulseLight_fadeout_1 = %SFM_LarvePulseLight_lifetime_1-%SFM_LarvePulseLight_fadein_1;
%SFM_LarvePulseLight_fadeout_2 = %SFM_LarvePulseLight_lifetime_2-%SFM_LarvePulseLight_fadein_2;
%SFM_LarvePulseLight_fadeout_3 = %SFM_LarvePulseLight_lifetime_3-%SFM_LarvePulseLight_fadein_3;
%SFM_LarvePulseLight_fadeout_4 = %SFM_LarvePulseLight_lifetime_4-%SFM_LarvePulseLight_fadein_4;
%SFM_LarvePulseLight_fadeout_5 = %SFM_LarvePulseLight_lifetime_5-%SFM_LarvePulseLight_fadein_5;
%SFM_LarvePulseLight_fadeout_6 = %SFM_LarvePulseLight_lifetime_6-%SFM_LarvePulseLight_fadein_6;
%SFM_LarvePulseLight_fadeout_7 = %SFM_LarvePulseLight_lifetime_7-%SFM_LarvePulseLight_fadein_7;

%SFM_LarvePulseLight_radius_1 = 2.0;
%SFM_LarvePulseLight_radius_2 = 3.0;
%SFM_LarvePulseLight_radius_3 = 4.0;
%SFM_LarvePulseLight_radius_4 = 5.0;
%SFM_LarvePulseLight_radius_5 = 6.0;
%SFM_LarvePulseLight_radius_6 = 7.0;
%SFM_LarvePulseLight_radius_7 = 8.0;

%SFM_LarvePulseLight_intensity_1 = 0.25;
%SFM_LarvePulseLight_intensity_2 = 0.5;
%SFM_LarvePulseLight_intensity_3 = 0.75;
%SFM_LarvePulseLight_intensity_4 = 1.0;
%SFM_LarvePulseLight_intensity_5 = 1.25;
%SFM_LarvePulseLight_intensity_6 = 1.5;
%SFM_LarvePulseLight_intensity_7 = 1.75;

datablock afxT3DPointLightData(SFM_LarvePulseLight_1_CE)
{
  radius = %SFM_LarvePulseLight_radius_1;
  color = "0.91 0.08 0.49";
  brightness = %SFM_LarvePulseLight_intensity_1;
  castShadows = false; // T3D Off (temporarily)
  localRenderViz = false;
};
datablock afxT3DPointLightData(SFM_LarvePulseLight_2_CE : SFM_LarvePulseLight_1_CE)
{
  radius = %SFM_LarvePulseLight_radius_2;
  brightness = %SFM_LarvePulseLight_intensity_2;
};
datablock afxT3DPointLightData(SFM_LarvePulseLight_3_CE : SFM_LarvePulseLight_1_CE)
{
  radius = %SFM_LarvePulseLight_radius_3;
  brightness = %SFM_LarvePulseLight_intensity_3;
};
datablock afxT3DPointLightData(SFM_LarvePulseLight_4_CE : SFM_LarvePulseLight_1_CE)
{
  radius = %SFM_LarvePulseLight_radius_4;
  brightness = %SFM_LarvePulseLight_intensity_4;
};
datablock afxT3DPointLightData(SFM_LarvePulseLight_5_CE : SFM_LarvePulseLight_1_CE)
{
  radius = %SFM_LarvePulseLight_radius_5;
  brightness = %SFM_LarvePulseLight_intensity_5;
};
datablock afxT3DPointLightData(SFM_LarvePulseLight_6_CE : SFM_LarvePulseLight_1_CE)
{
  radius = %SFM_LarvePulseLight_radius_6;
  brightness = %SFM_LarvePulseLight_intensity_6;
};
datablock afxT3DPointLightData(SFM_LarvePulseLight_7_CE : SFM_LarvePulseLight_1_CE)
{
  radius = %SFM_LarvePulseLight_radius_7;
  brightness = %SFM_LarvePulseLight_intensity_8;
};

/*
datablock sgLightObjectData(SFM_LarvePulseLight_ANIM_7_CE)
{
  CastsShadows = true;
  Radius = 7;
  Brightness = %SFM_LarvePulseLight_intensity_7;
  Colour = "0.91 0.08 0.49";
  LightingModelName = "Original Advanced";  

  AnimRadius = true;
  LerpRadius = true;
  MinRadius = %SFM_LarvePulseLight_radius_7-1.5;
  MaxRadius = %SFM_LarvePulseLight_radius_7+0.25;
  RadiusKeys = "ZAZ";
  RadiusTime = %SFM_LarvePulseLight_lifetime_7*2;
};
*/

datablock afxEffectWrapperData(SFM_LarvePulseLight_1_EW)
{
  effect = SFM_LarvePulseLight_1_CE;
  posConstraint = caster;
  delay = %SFM_LarvePulseLight_delay_1;
  fadeInTime  = %SFM_LarvePulseLight_fadein_1;
  fadeOutTime = %SFM_LarvePulseLight_fadeout_1;
  lifetime = %SFM_LarvePulseLight_lifetime_1-%SFM_LarvePulseLight_fadeout_1;
  xfmModifiers[0] = SFM_Cocoon_offset_XM;
};
datablock afxEffectWrapperData(SFM_LarvePulseLight_2_EW : SFM_LarvePulseLight_1_EW)
{
  effect = SFM_LarvePulseLight_2_CE;
  delay = %SFM_LarvePulseLight_delay_2;
  fadeInTime  = %SFM_LarvePulseLight_fadein_2;
  fadeOutTime = %SFM_LarvePulseLight_fadeout_2;
  lifetime = %SFM_LarvePulseLight_lifetime_2-%SFM_LarvePulseLight_fadeout_2;
};
datablock afxEffectWrapperData(SFM_LarvePulseLight_3_EW : SFM_LarvePulseLight_1_EW)
{
  effect = SFM_LarvePulseLight_3_CE;
  delay = %SFM_LarvePulseLight_delay_3;
  fadeInTime  = %SFM_LarvePulseLight_fadein_3;
  fadeOutTime = %SFM_LarvePulseLight_fadeout_3;
  lifetime = %SFM_LarvePulseLight_lifetime_3-%SFM_LarvePulseLight_fadeout_3;
};
datablock afxEffectWrapperData(SFM_LarvePulseLight_4_EW : SFM_LarvePulseLight_1_EW)
{
  effect = SFM_LarvePulseLight_4_CE;
  delay = %SFM_LarvePulseLight_delay_4;
  fadeInTime  = %SFM_LarvePulseLight_fadein_4;
  fadeOutTime = %SFM_LarvePulseLight_fadeout_4;
  lifetime = %SFM_LarvePulseLight_lifetime_4-%SFM_LarvePulseLight_fadeout_4;
};
datablock afxEffectWrapperData(SFM_LarvePulseLight_5_EW : SFM_LarvePulseLight_1_EW)
{
  effect = SFM_LarvePulseLight_5_CE;
  delay = %SFM_LarvePulseLight_delay_5;
  fadeInTime  = %SFM_LarvePulseLight_fadein_5;
  fadeOutTime = %SFM_LarvePulseLight_fadeout_5;
  lifetime = %SFM_LarvePulseLight_lifetime_5-%SFM_LarvePulseLight_fadeout_5;
};
datablock afxEffectWrapperData(SFM_LarvePulseLight_6_EW : SFM_LarvePulseLight_1_EW)
{
  effect = SFM_LarvePulseLight_6_CE;
  delay = %SFM_LarvePulseLight_delay_6;
  fadeInTime  = %SFM_LarvePulseLight_fadein_6;
  fadeOutTime = %SFM_LarvePulseLight_fadeout_6;
  lifetime = %SFM_LarvePulseLight_lifetime_6-%SFM_LarvePulseLight_fadeout_6;
};

if (true)
{
datablock afxEffectWrapperData(SFM_LarvePulseLight_7_EW : SFM_LarvePulseLight_1_EW)
{
  effect = SFM_LarvePulseLight_7_CE;
  delay = %SFM_LarvePulseLight_delay_7;
  fadeInTime  = %SFM_LarvePulseLight_fadein_7;
  fadeOutTime = %SFM_LarvePulseLight_fadeout_7;
  lifetime = %SFM_LarvePulseLight_lifetime_7-%SFM_LarvePulseLight_fadeout_7;
};
}
else
{
datablock afxEffectWrapperData(SFM_LarvePulseLight_7_EW : SFM_LarvePulseLight_1_EW)
{
  effect = SFM_LarvePulseLight_ANIM_7_CE;
  delay = %SFM_LarvePulseLight_delay_7;
  fadeInTime  = %SFM_LarvePulseLight_fadein_7;
  fadeOutTime = %SFM_LarvePulseLight_fadeout_7;
  lifetime = %SFM_LarvePulseLight_lifetime_7-%SFM_LarvePulseLight_fadeout_7;
};
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock LightFlareData(SFM_ExplosionLight_FLARE)
{
  overallScale = 1;
  flareEnabled = true;
  flareTexture = %mySpellDataPath @ "/SFM/lights/SFM_corona";
  elementRect[0] = "0 0 128 128";
  elementDist[0] = 0.0;
  elementScale[0] = 15;
  elementTint[0] = "0.91 0.08 0.49";
  elementRotate[0] = false;
  elementUseLightColor[0] = true;
};

datablock afxT3DPointLightData(SFM_ExplosionLight_flare_CE)
{
  radius = 12;
  color = "0.91 0.08 0.49";
  brightness = 3.0;
  flareType = SFM_ExplosionLight_FLARE;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(SFM_ExplosionLight_EW : SFM_LarvaExplosion_EW)
{
  effect = SFM_ExplosionLight_flare_CE;
  delay = 10.5+0.4;
  fadeInTime  = 0.15;
  lifetime = 0.66-0.1;
  fadeOutTime = 0.35;  
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function SFM_add_Lighting_FX(%spell_data)
{
  %spell_data.addCastingEffect(SFM_MainZodeRevealLight_1_EW);
  %spell_data.addCastingEffect(SFM_MainZodeRevealLight_2_EW);
  %spell_data.addCastingEffect(SFM_MainZodeRevealLight_3_EW);
  %spell_data.addCastingEffect(SFM_CastingZodeLight_EW);

  %spell_data.addCastingEffect(SFM_LarvePulseLight_1_EW);
  %spell_data.addCastingEffect(SFM_LarvePulseLight_2_EW);
  %spell_data.addCastingEffect(SFM_LarvePulseLight_3_EW);
  %spell_data.addCastingEffect(SFM_LarvePulseLight_4_EW);
  %spell_data.addCastingEffect(SFM_LarvePulseLight_5_EW);
  %spell_data.addCastingEffect(SFM_LarvePulseLight_6_EW);
  %spell_data.addCastingEffect(SFM_LarvePulseLight_7_EW);

  %spell_data.addCastingEffect(SFM_ExplosionLight_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
