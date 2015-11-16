
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// LIGHT MY FIRE (audio)
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
// SOUNDS

// hands fire //

datablock SFXProfile(LMF_HandsFire_Snd_CE)
{
   fileName = %mySpellDataPath @ "/LMF/sounds/LMF_hands_fire.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(LMF_HandsFire_LF_Snd_EW)
{
  effect = LMF_HandsFire_Snd_CE;
  constraint = "caster.Bip01 L Hand";
  lifetime = 3.583;
  delay = 0.4;
};
datablock afxEffectWrapperData(LMF_HandsFire_RT_Snd_EW)
{
  effect = LMF_HandsFire_Snd_CE;
  constraint = "caster.Bip01 R Hand";
  lifetime = 3.583;
  delay = 0.15;
};

// main fire // 

datablock SFXDescription(LMF_FireLoop_Snd_AD : SpellAudioLoop_AD)
{
  ReferenceDistance  = 10.0;
};
datablock SFXProfile(LMF_FireLoop_Snd_CE)
{
   fileName = %mySpellDataPath @ "/LMF/sounds/LMF_fire_loop.ogg";
   description = LMF_FireLoop_Snd_AD;
   preload = true;
};
datablock afxEffectWrapperData(LMF_FireLoop_Snd_EW)
{
  effect = LMF_FireLoop_Snd_CE;
  posConstraint = "#scene.CampFire";
  lifetime = 11.1;
  delay = 0.5;
  fadeInTime  = 2.0;
  fadeOutTime = 1.0;
};

// sparks //

datablock SFXProfile(LMF_Accent1_Snd_CE)
{
   fileName = %mySpellDataPath @ "/LMF/sounds/LMF_accent_1.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};

datablock SFXProfile(LMF_Accent2_Snd_CE)
{
   fileName = %mySpellDataPath @ "/LMF/sounds/LMF_accent_2.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};

datablock afxEffectWrapperData(LMF_Accent1_A_Snd_EW)
{
  effect = LMF_Accent1_Snd_CE;
  posConstraint = "#scene.CampFire";
  lifetime = 1.435;
  delay    = 5.0+0.10;
  scaleFactor = 0.8;
  xfmModifiers[0] = "LMF_Fire_Offset_Center_XM";
  xfmModifiers[1] = "LMF_FireBurst_Offset1_XM";
};

datablock afxEffectWrapperData(LMF_Accent1_B_Snd_EW)
{
  effect = LMF_Accent1_Snd_CE;
  posConstraint = "#scene.CampFire";
  lifetime = 1.435;
  delay    = 7.0+0.10;
  scaleFactor = 0.6;
  xfmModifiers[0] = "LMF_Fire_Offset_Center_XM";
  xfmModifiers[1] = "LMF_FireBurst_Offset2_XM";
};
datablock afxEffectWrapperData(LMF_Accent1_B2_Snd_EW : LMF_Accent1_B_Snd_EW)
{
  effect = LMF_Accent2_Snd_CE;
  posConstraint = "#scene.CampFire";
  lifetime = 1.584;
};

datablock afxEffectWrapperData(LMF_Accent1_C_Snd_EW)
{
  effect = LMF_Accent1_Snd_CE;
  posConstraint = "#scene.CampFire";
  lifetime = 1.435;
  delay    = 7.5+0.06;
  scaleFactor = 0.7;
  xfmModifiers[0] = "LMF_Fire_Offset_Center_XM";
  xfmModifiers[1] = "LMF_FireBurst_Offset3_XM";
};

datablock afxEffectWrapperData(LMF_Accent1_D_Snd_EW)
{
  effect = LMF_Accent2_Snd_CE;
  posConstraint = "#scene.CampFire";
  lifetime = 1.584;
  delay    = 9.1+0.15;
  scaleFactor = 0.6;
  xfmModifiers[0] = "LMF_Fire_Offset_Center_XM";
  xfmModifiers[1] = "LMF_FireBurst_Offset4_XM";
};

datablock afxEffectWrapperData(LMF_Accent1_E_Snd_EW)
{
  effect = LMF_Accent1_Snd_CE;
  posConstraint = "#scene.CampFire";
  lifetime = 1.435;
  delay    = 10.9+0.09;
  scaleFactor = 0.7;
  xfmModifiers[0] = "LMF_Fire_Offset_Center_XM";
  xfmModifiers[1] = "LMF_FireBurst_Offset5_XM";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function LMF_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(LMF_HandsFire_LF_Snd_EW);
  %spell_data.addCastingEffect(LMF_HandsFire_RT_Snd_EW);
  %spell_data.addLingerEffect(LMF_FireLoop_Snd_EW);
  %spell_data.addLingerEffect(LMF_Accent1_A_Snd_EW);
  %spell_data.addLingerEffect(LMF_Accent1_B_Snd_EW);
  %spell_data.addLingerEffect(LMF_Accent1_B2_Snd_EW);
  %spell_data.addLingerEffect(LMF_Accent1_C_Snd_EW);
  %spell_data.addLingerEffect(LMF_Accent1_D_Snd_EW);
  %spell_data.addLingerEffect(LMF_Accent1_E_Snd_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//