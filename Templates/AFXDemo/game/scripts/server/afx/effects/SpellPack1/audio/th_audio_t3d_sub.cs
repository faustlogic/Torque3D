
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// THOR'S HAMMER (audio)
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

datablock SFXProfile(TH_ZodiacSnd_CE)
{
   fileName = %mySpellDataPath @ "/TH/sounds/TH_zodiac.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(TH_ZodiacSnd_EW)
{
  effect = TH_ZodiacSnd_CE;
  constraint = caster;
  delay = 0.0;
  //scaleFactor = 0.8;
  lifetime = 3.543;
};

datablock SFXProfile(TH_BodyfallSnd_CE)
{
   fileName = %mySpellDataPath @ "/TH/sounds/TH_bodyfall.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(TH_BodyfallSnd_EW)
{
  effect = TH_BodyfallSnd_CE;
  constraint = caster;
  delay = 8.8;
  //scaleFactor = 0.8;
  lifetime = 0.603;
};

datablock SFXProfile(TH_DropcatchSnd_CE)
{
   fileName = %mySpellDataPath @ "/TH/sounds/TH_dropcatch.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(TH_DropcatchSnd_EW)
{
  effect = TH_DropcatchSnd_CE;
  constraint = caster;
  delay = 1.8;
  //scaleFactor = 0.8;
  lifetime = 1.526;
};

//datablock SFXDescription(TH_SpellAudioCasting_loud_AD : SpellAudioCasting_loud_AD)
//{
//  volume = 2.0;
//};
datablock SFXProfile(TH_ImpactSnd_CE)
{
   fileName = %mySpellDataPath @ "/TH/sounds/TH_impact_louder.ogg"; //TH_impact.ogg";
   description = SpellAudioCasting_loud_AD;
   preload = true;
};
datablock afxEffectWrapperData(TH_ImpactSnd_EW)
{
  effect = TH_ImpactSnd_CE;
  constraint = caster;
  delay = 7.3;
  //scaleFactor = 0.8;
  //scaleFactor = 2.0;
  lifetime = 3.525;
};

datablock SFXProfile(TH_ThunderElecSnd_CE)
{
   fileName = %mySpellDataPath @ "/TH/sounds/TH_thunderElec_bed.ogg";
   description = SpellAudioCasting_loud_AD;
   preload = true;
};
datablock afxEffectWrapperData(TH_ThunderElecSnd_EW)
{
  effect = TH_ThunderElecSnd_CE;
  constraint = caster;
  delay = 0.0;
  //scaleFactor = 0.8;
  lifetime = 7.867;
};

datablock SFXProfile(TH_TossSnd_CE)
{
   fileName = %mySpellDataPath @ "/TH/sounds/TH_toss.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(TH_TossSnd_EW)
{
  effect = TH_TossSnd_CE;
  constraint = caster;
  delay = 5.9;
  //scaleFactor = 0.8;
  lifetime = 2.15;
};

datablock SFXProfile(TH_WindupSnd_CE)
{
   fileName = %mySpellDataPath @ "/TH/sounds/TH_windup.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(TH_WindupSnd_EW)
{
  effect = TH_WindupSnd_CE;
  constraint = caster;
  delay = 2.2;
  //scaleFactor = 0.8;
  lifetime = 3.346;
};

datablock afxXM_LocalOffsetData(TH_ThunderSnd_offset1_XM)
{
  localOffset = "50 3 50";
};
datablock SFXProfile(TH_ThunderSnd_CE)
{
   fileName = %mySpellDataPath @ "/TH/sounds/thunder4.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock afxEffectWrapperData(TH_ThunderSnd_EW)
{
  effect = TH_ThunderSnd_CE;
  constraint = caster;
  delay = 12.2;
  lifetime = 5.207;
  xfmModifiers[0] = TH_ThunderSnd_offset1_XM;
};

datablock afxXM_LocalOffsetData(TH_ThunderSnd_offset2_XM)
{
  localOffset = "-40 -3 40";
};
datablock SFXProfile(TH_ThunderSnd2_CE)
{
   fileName = %mySpellDataPath @ "/TH/sounds/thunder1.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock afxEffectWrapperData(TH_Thunder2Snd_EW)
{
  effect = TH_ThunderSnd2_CE;
  constraint = caster;
  delay = 11.0;
  //scaleFactor = 0.5;
  lifetime = 3.233;
  xfmModifiers[0] = TH_ThunderSnd_offset2_XM;
};


datablock SFXDescription(TH_SpellAudioCasting_soft_AD : SpellAudioCasting_AD)
{
  volume = 0.06;  // threshold = .06-.07
};
datablock SFXDescription(TH_SpellAudioCasting_softer_AD : SpellAudioCasting_AD)
{
  volume = 0.01;
};

datablock SFXProfile(TH_CasterLightningSnd_1_CE)
{
  fileName = %mySpellDataPath @ "/TH/sounds/TH_lightningEdit_1-4.ogg";
  description = SpellAudioCasting_AD;
  preload = true;
};
datablock SFXProfile(TH_CasterLightningSnd_2_CE)
{
  fileName = %mySpellDataPath @ "/TH/sounds/TH_lightningEdit_2-4.ogg";
  description = SpellAudioCasting_AD;
  preload = true;
};
datablock SFXProfile(TH_CasterLightningSnd_3_CE)
{
  fileName = %mySpellDataPath @ "/TH/sounds/TH_lightningEdit_3-2.ogg";
  description = SpellAudioCasting_AD;
  preload = true;
};
datablock SFXProfile(TH_CasterLightningSnd_4_CE)
{
  fileName = %mySpellDataPath @ "/TH/sounds/TH_lightningEdit_4.ogg";
  description = SpellAudioCasting_AD;
  preload = true;
};
datablock SFXProfile(TH_CasterLightningSnd_5_CE)
{
  fileName = %mySpellDataPath @ "/TH/sounds/TH_lightningEdit_5-2.ogg";
  description = SpellAudioCasting_AD;
  preload = true;
};

//datablock SFXProfile(TH_CasterLightningSnd_1_softer_CE : TH_CasterLightningSnd_1_soft_CE)
//{
//  fileName = %mySpellDataPath @ "/TH/sounds/TH_lightningEdit_1-4--soft.ogg";
//  //description = TH_SpellAudioCasting_softer_AD;
//};
//datablock SFXProfile(TH_CasterLightningSnd_2_softer_CE : TH_CasterLightningSnd_2_soft_CE)
//{
//  fileName = %mySpellDataPath @ "/TH/sounds/TH_lightningEdit_2-4--soft.ogg";//--softer.ogg";
//  //description = TH_SpellAudioCasting_softer_AD;
//};
//datablock SFXProfile(TH_CasterLightningSnd_3_softer_CE : TH_CasterLightningSnd_3_soft_CE)
//{
//  fileName = %mySpellDataPath @ "/TH/sounds/TH_lightningEdit_3-2--soft.ogg";
//  //description = TH_SpellAudioCasting_softer_AD;
//};
//datablock SFXProfile(TH_CasterLightningSnd_4_softer_CE : TH_CasterLightningSnd_4_soft_CE)
//{
//  fileName = %mySpellDataPath @ "/TH/sounds/TH_lightningEdit_4--soft.ogg";
//  //description = TH_SpellAudioCasting_softer_AD;
//};
//datablock SFXProfile(TH_CasterLightningSnd_5_softer_CE : TH_CasterLightningSnd_5_soft_CE)
//{
//  fileName = %mySpellDataPath @ "/TH/sounds/TH_lightningEdit_5-2--soft.ogg";
//  //description = TH_SpellAudioCasting_softer_AD;
//};

%TH_CasterLightningSnd_1_lifetime = 0.5018;
%TH_CasterLightningSnd_2_lifetime = 0.4233;
%TH_CasterLightningSnd_3_lifetime = 0.5018;
%TH_CasterLightningSnd_4_lifetime = 1.1304;
%TH_CasterLightningSnd_5_lifetime = 0.9977;

%TH_ZodeLightning_volume   = 0.70; //0.85;
%TH_CasterLightning_volume = 0.86; //0.95;

datablock afxEffectWrapperData(TH_ZodeLightningSnd1_EW : TH_ZodeLightningEffect1_EW)
{
  effect = TH_CasterLightningSnd_1_CE;
  lifetime = %TH_CasterLightningSnd_1_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd2_EW : TH_ZodeLightningEffect2_EW)
{
  effect = TH_CasterLightningSnd_2_CE;
  lifetime = %TH_CasterLightningSnd_2_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd3_EW : TH_ZodeLightningEffect3_EW)
{
  effect = TH_CasterLightningSnd_4_CE;
  lifetime = %TH_CasterLightningSnd_4_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd4_EW : TH_ZodeLightningEffect4_EW)
{
  effect = TH_CasterLightningSnd_3_CE;
  lifetime = %TH_CasterLightningSnd_3_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd5_EW : TH_ZodeLightningEffect5_EW)
{
  effect = TH_CasterLightningSnd_5_CE;
  lifetime = %TH_CasterLightningSnd_5_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd6_EW : TH_ZodeLightningEffect6_EW)
{
  effect = TH_CasterLightningSnd_2_CE;
  lifetime = %TH_CasterLightningSnd_2_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd7_EW : TH_ZodeLightningEffect7_EW)
{
  effect = TH_CasterLightningSnd_2_CE;
  lifetime = %TH_CasterLightningSnd_2_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd8_EW : TH_ZodeLightningEffect8_EW)
{
  effect = TH_CasterLightningSnd_4_CE;
  lifetime = %TH_CasterLightningSnd_4_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd9_EW : TH_ZodeLightningEffect9_EW)
{
  effect = TH_CasterLightningSnd_5_CE;
  lifetime = %TH_CasterLightningSnd_5_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd10_EW : TH_ZodeLightningEffect10_EW)
{
  effect = TH_CasterLightningSnd_1_CE;
  lifetime = %TH_CasterLightningSnd_1_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd11_EW : TH_ZodeLightningEffect11_EW)
{
  effect = TH_CasterLightningSnd_4_CE;
  lifetime = %TH_CasterLightningSnd_4_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd12_EW : TH_ZodeLightningEffect12_EW)
{
  effect = TH_CasterLightningSnd_5_CE;
  lifetime = %TH_CasterLightningSnd_5_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd13_EW : TH_ZodeLightningEffect13_EW)
{
  effect = TH_CasterLightningSnd_5_CE;
  lifetime = %TH_CasterLightningSnd_5_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd14_EW : TH_ZodeLightningEffect14_EW)
{
  effect = TH_CasterLightningSnd_1_CE;
  lifetime = %TH_CasterLightningSnd_1_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd15_EW : TH_ZodeLightningEffect15_EW)
{
  effect = TH_CasterLightningSnd_3_CE;
  lifetime = %TH_CasterLightningSnd_3_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd16_EW : TH_ZodeLightningEffect16_EW)
{
  effect = TH_CasterLightningSnd_5_CE;
  lifetime = %TH_CasterLightningSnd_5_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd17_EW : TH_ZodeLightningEffect17_EW)
{
  effect = TH_CasterLightningSnd_2_CE;
  lifetime = %TH_CasterLightningSnd_2_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd18_EW : TH_ZodeLightningEffect18_EW)
{
  effect = TH_CasterLightningSnd_5_CE;
  lifetime = %TH_CasterLightningSnd_5_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd19_EW : TH_ZodeLightningEffect19_EW)
{
  effect = TH_CasterLightningSnd_4_CE;
  lifetime = %TH_CasterLightningSnd_4_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_ZodeLightningSnd20_EW : TH_ZodeLightningEffect20_EW)
{
  effect = TH_CasterLightningSnd_3_CE;
  lifetime = %TH_CasterLightningSnd_3_lifetime;
  scaleFactor = %TH_ZodeLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};

/*
datablock afxEffectWrapperData(TH_CasterLightningSnd1_EW : TH_CasterLightningFlashLight1_EW)
{
  effect = TH_CasterLightningSnd_1_CE;
  lifetime = %TH_CasterLightningSnd_1_lifetime;
  scaleFactor = %TH_CasterLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_CasterLightningSnd2_EW : TH_CasterLightningFlashLight2_EW)
{
  effect = TH_CasterLightningSnd_3_CE;
  lifetime = %TH_CasterLightningSnd_3_lifetime;
  scaleFactor = %TH_CasterLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_CasterLightningSnd3_EW : TH_CasterLightningFlashLight3_EW)
{
  effect = TH_CasterLightningSnd_2_CE;
  lifetime = %TH_CasterLightningSnd_2_lifetime;
  scaleFactor = %TH_CasterLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_CasterLightningSnd4_EW : TH_CasterLightningFlashLight4_EW)
{
  effect = TH_CasterLightningSnd_5_CE;
  lifetime = %TH_CasterLightningSnd_5_lifetime;
  scaleFactor = %TH_CasterLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_CasterLightningSnd5_EW : TH_CasterLightningFlashLight5_EW)
{
  effect = TH_CasterLightningSnd_4_CE;
  lifetime = %TH_CasterLightningSnd_4_lifetime;
  scaleFactor = %TH_CasterLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_CasterLightningSnd6_EW : TH_CasterLightningFlashLight6_EW)
{
  effect = TH_CasterLightningSnd_3_CE;
  lifetime = %TH_CasterLightningSnd_3_lifetime;
  scaleFactor = %TH_CasterLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_CasterLightningSnd7_EW : TH_CasterLightningFlashLight7_EW)
{
  effect = TH_CasterLightningSnd_1_CE;
  lifetime = %TH_CasterLightningSnd_1_lifetime;
  scaleFactor = %TH_CasterLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_CasterLightningSnd8_EW : TH_CasterLightningFlashLight8_EW)
{
  effect = TH_CasterLightningSnd_5_CE;
  lifetime = %TH_CasterLightningSnd_5_lifetime;
  scaleFactor = %TH_CasterLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_CasterLightningSnd9_EW : TH_CasterLightningFlashLight9_EW)
{
  effect = TH_CasterLightningSnd_2_CE;
  lifetime = %TH_CasterLightningSnd_2_lifetime;
  scaleFactor = %TH_CasterLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
datablock afxEffectWrapperData(TH_CasterLightningSnd10_EW : TH_CasterLightningFlashLight10_EW)
{
  effect = TH_CasterLightningSnd_1_CE;
  lifetime = %TH_CasterLightningSnd_1_lifetime;
  scaleFactor = %TH_CasterLightning_volume;
  fadeInTime = 0;
  fadeOutTime = 0;
};
*/

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function TH_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(TH_ZodiacSnd_EW);
  %spell_data.addCastingEffect(TH_BodyfallSnd_EW);
  %spell_data.addCastingEffect(TH_DropcatchSnd_EW);
  %spell_data.addCastingEffect(TH_ImpactSnd_EW);
  %spell_data.addCastingEffect(TH_ThunderElecSnd_EW);
  %spell_data.addCastingEffect(TH_TossSnd_EW);
  %spell_data.addCastingEffect(TH_WindupSnd_EW);
  %spell_data.addCastingEffect(TH_ThunderSnd_EW);
  %spell_data.addCastingEffect(TH_Thunder2Snd_EW);

  %spell_data.addCastingEffect(TH_ZodeLightningSnd1_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd2_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd3_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd4_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd5_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd6_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd7_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd8_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd9_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd10_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd11_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd12_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd13_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd14_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd15_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd16_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd17_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd18_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd19_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningSnd20_EW);

  /*
  %spell_data.addCastingEffect(TH_CasterLightningSnd1_EW);
  %spell_data.addCastingEffect(TH_CasterLightningSnd2_EW);
  %spell_data.addCastingEffect(TH_CasterLightningSnd3_EW);
  %spell_data.addCastingEffect(TH_CasterLightningSnd4_EW);
  %spell_data.addCastingEffect(TH_CasterLightningSnd5_EW);
  %spell_data.addCastingEffect(TH_CasterLightningSnd6_EW);
  %spell_data.addCastingEffect(TH_CasterLightningSnd7_EW);
  %spell_data.addCastingEffect(TH_CasterLightningSnd8_EW);
  %spell_data.addCastingEffect(TH_CasterLightningSnd9_EW);
  %spell_data.addCastingEffect(TH_CasterLightningSnd10_EW);
  */
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
