
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// REAPER MADDNESS (audio)
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
// Sounds

datablock SFXProfile(RM_ZodiacSnd_CE)
{
   fileName = %mySpellDataPath @ "/RM/sounds/RM_zodiac.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(RM_ZodiacSnd_EW)
{
  effect = RM_ZodiacSnd_CE;
  constraint = "caster";
  delay = 0.0;
  lifetime = 5.757;
};

datablock SFXProfile(RM_ConjureSnd_CE)
{
   fileName = %mySpellDataPath @ "/RM/sounds/RM_conjure.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(RM_ConjureSnd_EW)
{
  effect = RM_ConjureSnd_CE;
  constraint = "caster";
  delay = 0.5;
  lifetime = 3.761;
};

datablock SFXProfile(RM_FlamesSnd_CE)
{
   fileName = %mySpellDataPath @ "/RM/sounds/RM_fireballs.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(RM_FlamesSnd_EW)
{
  effect = RM_FlamesSnd_CE;
  constraint = "caster";
  delay = 1.8;
  lifetime = 2.147;
};

datablock SFXProfile(RM_TargetZodiacSnd_CE)
{
   fileName = %mySpellDataPath @ "/RM/sounds/RM_targetbase.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(RM_TargetZodiacSnd_EW)
{
  effect = RM_TargetZodiacSnd_CE;
  constraint = "target";
  delay = 2.25;
  lifetime = 4.595;
};


datablock SFXProfile(RM_InRing1Snd_CE)
{
   fileName = %mySpellDataPath @ "/RM/sounds/RM_inring1.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(RM_InRing1Snd_EW)
{
  effect = RM_InRing1Snd_CE;
  constraint = "target";
  delay = 4.0;
  lifetime = 1.066;
};

datablock SFXProfile(RM_InRings2_3Snd_CE)
{
   fileName = %mySpellDataPath @ "/RM/sounds/RM_inrings2_3.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(RM_InRing2Snd_EW)
{
  effect = RM_InRings2_3Snd_CE;
  constraint = "target";
  delay = 4.6;
  lifetime = 1.263;
};

datablock SFXProfile(RM_OutRingSnd_CE)
{
   fileName = %mySpellDataPath @ "/RM/sounds/RM_outring_impact.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(RM_OutRingSnd_EW)
{
  effect = RM_OutRingSnd_CE;
  constraint = "target";
  delay = 5.75;
  lifetime = 5.414;
};


datablock SFXProfile(RM_SpellEndSnd_CE)
{
   fileName = %mySpellDataPath @ "/RM/sounds/RM_spellend.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(RM_SpellEndSnd_EW)
{
  effect = RM_SpellEndSnd_CE;
  constraint = "target";
  delay = 5.5;
  lifetime = 1.342;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function RM_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(RM_ZodiacSnd_EW);
  %spell_data.addCastingEffect(RM_ConjureSnd_EW);
  %spell_data.addCastingEffect(RM_FlamesSnd_EW);
  %spell_data.addCastingEffect(RM_TargetZodiacSnd_EW);
  %spell_data.addCastingEffect(RM_InRing1Snd_EW);
  %spell_data.addCastingEffect(RM_InRing2Snd_EW);
  %spell_data.addCastingEffect(RM_OutRingSnd_EW);
  %spell_data.addLingerEffect(RM_SpellEndSnd_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
