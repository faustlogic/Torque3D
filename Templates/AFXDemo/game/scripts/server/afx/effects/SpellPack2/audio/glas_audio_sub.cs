
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// GREEN LEGS AND SCRAM (audio)
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

datablock SFXProfile(GLaS_Zing_SND)
{
   fileName = %mySpellDataPath @ "/GLaS/sounds/glas_zing.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(GLaS_Zing_SND_EW)
{
  effect = GLaS_Zing_SND;
  posConstraint = "target";
  delay = 0.0;
  lifetime = 3;
};

datablock SFXProfile(GLaS_Crash_SND)
{
   fileName = %mySpellDataPath @ "/GLaS/sounds/glas_crash16.ogg";
   description = SpellAudioCasting_loud_AD;
   preload = true;
};
datablock afxEffectWrapperData(GLaS_Crash_SND_EW)
{
  effect = GLaS_Crash_SND;
  posConstraint = "target";
  delay = 5;
  lifetime = 3.0;
};
datablock afxEffectWrapperData(GLaS_CrashLanding_SND_EW)
{
  effect = GLaS_Crash_SND;
  posConstraint = "target";
  delay = 0;
  lifetime = 3.0;
};

datablock SFXProfile(GLaS_Snare_SND)
{
   fileName = %mySpellDataPath @ "/GLaS/sounds/glas_snare.ogg";
   description = SpellAudioCasting_loud_AD;
   preload = true;
};
datablock afxEffectWrapperData(GLaS_Snare1_SND_EW)
{
  effect = GLaS_Snare_SND;
  posConstraint = "target";
  delay = 0;
  lifetime = 1.0;
  scaleFactor = 0.5;
};
datablock afxEffectWrapperData(GLaS_Snare2_SND_EW : GLaS_Snare1_SND_EW)
{
  delay = 0.1;
  scaleFactor = 0.6;
};
datablock afxEffectWrapperData(GLaS_Snare3_SND_EW : GLaS_Snare1_SND_EW)
{
  delay = 0.2;
  scaleFactor = 0.7;
};
datablock afxEffectWrapperData(GLaS_Snare4_SND_EW : GLaS_Snare1_SND_EW)
{
  delay = 0.3;
  scaleFactor = 1.0;
};

datablock afxAudioBank(GLaS_FootDrums_CE)
{
  playIndex = "$$ getRandom(0,5)";
  path = %mySpellDataPath @ "/GLaS/sounds";
  filenames[0] = "glas_rimshot.ogg";
    filenames[1] = "glas_snare.ogg";
    filenames[2] = "glas_hhho.ogg";
    filenames[3] = "glas_hh.ogg";
    filenames[4] = "glas_ride.ogg";
    filenames[5] = "glas_basskick.ogg";
  description = SpellAudioCasting_loud_AD;
  preload = true;
};
datablock afxEffectWrapperData(GLaS_FootDrums_EW)
{
  effect = GLaS_FootDrums_CE;
  posConstraint = "target";
  lifetime = 3.0;
  scaleFactor = "$$ getRandomF(0.8,1.0)";
};

datablock afxPhraseEffectData(GLaS_phrase_effect_JUMP_CE)
{
  triggerMask = $AFX::PLAYER_JUMP_S_TRIGGER;
  addEffect = GLaS_Zing_SND_EW;
};  
datablock afxEffectWrapperData(GLaS_phrase_effect_JUMP_EW)
{
  effect = GLaS_phrase_effect_JUMP_CE;
  posConstraint = target;
};

datablock afxPhraseEffectData(GLaS_phrase_effect_LAND_CE)
{
  triggerMask = $AFX::PLAYER_LANDING_C_TRIGGER;
  addEffect = GLaS_CrashLanding_SND_EW;
};  
datablock afxEffectWrapperData(GLaS_phrase_effect_LAND_EW)
{
  effect = GLaS_phrase_effect_LAND_CE;
  posConstraint = target;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function GLaS_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(GLaS_Snare1_SND_EW);
  %spell_data.addCastingEffect(GLaS_Snare2_SND_EW);
  %spell_data.addCastingEffect(GLaS_Snare3_SND_EW);
  %spell_data.addCastingEffect(GLaS_Snare4_SND_EW);
  %spell_data.addImpactEffect(GLaS_Zing_SND_EW);
  %spell_data.addImpactEffect(GLaS_Crash_SND_EW);
  %spell_data.addLingerEffect(GLaS_phrase_effect_LAND_EW);
  %spell_data.addLingerEffect(GLaS_phrase_effect_JUMP_EW);
}

function GLaS_add_footstep_Audio_FX(%lf_data, %rt_data)
{
  %lf_data.addEffect(GLaS_FootDrums_EW);
  %rt_data.addEffect(GLaS_FootDrums_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
