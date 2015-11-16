
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// ASTRAL PASSPORT (audio)
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
// CASTING SOUNDS

// Leaving

datablock SFXProfile(AP_ZodeSnd_LV_CE)
{
   fileName = %mySpellDataPath @ "/AP/sounds/AP_leave_zodiac.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(AP_ZodeSnd_LV_EW)
{
  effect = AP_ZodeSnd_LV_CE;
  constraint = "caster";
  delay = 0;
  lifetime = 1.697;
};

datablock SFXProfile(AP_ConjureSnd_LV_CE)
{
   fileName = %mySpellDataPath @ "/AP/sounds/AP_leave_conjure1.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(AP_ConjureSnd_LV_EW)
{
  effect = AP_ConjureSnd_LV_CE;
  constraint = "caster";
  delay = 0.4;
  lifetime = 3.178;
};

datablock SFXProfile(AP_Spinup_LV_CE)
{
   fileName = %mySpellDataPath @ "/AP/sounds/AP_leave_spinUp.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(AP_Spinup_LV_EW)
{
  effect = AP_Spinup_LV_CE;
  constraint = "caster";
  delay = 1.4;
  lifetime = 1.996;
};

datablock SFXProfile(AP_Spindown_LV_CE)
{
   fileName = %mySpellDataPath @ "/AP/sounds/AP_leave_SpinDown.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(AP_Spindown_LV_EW)
{
  effect = AP_Spindown_LV_CE;
  constraint = "caster";
  delay = 2.8;
  lifetime = 2.318;
};

// Arriving

datablock SFXProfile(AP_ZodeSnd_ARV_CE)
{
   fileName = %mySpellDataPath @ "/AP/sounds/AP_arrive_zodiac.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(AP_ZodeSnd_ARV_EW)
{
  effect = AP_ZodeSnd_ARV_CE;
  constraint = "caster";
  //lifetime = 1.7;
  lifetime = 1.734;
  delay = %AP_ReappearTimeOffset;
};

datablock SFXProfile(AP_Spinup_ARV_CE)
{
   fileName = %mySpellDataPath @ "/AP/sounds/AP_arrive_SpinUp.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(AP_Spinup_ARV_EW)
{
  effect = AP_Spinup_ARV_CE;
  constraint = "caster";
  //lifetime = 2.0;
  lifetime = 2.799;
  delay = %AP_ReappearTimeOffset + 1.4;
};

datablock SFXProfile(AP_Spindown_ARV_CE)
{
   fileName = %mySpellDataPath @ "/AP/sounds/AP_arrive_SpinDownImpact.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(AP_Spindown_ARV_EW)
{
  effect = AP_Spindown_ARV_CE;
  constraint = "caster";
  //lifetime = 2.32;
  lifetime = 2.001;
  delay = %AP_ReappearTimeOffset + 2.5;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function AP_disappear_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(AP_ZodeSnd_LV_EW);
  %spell_data.addCastingEffect(AP_ConjureSnd_LV_EW);
  %spell_data.addCastingEffect(AP_Spinup_LV_EW);
  %spell_data.addCastingEffect(AP_Spindown_LV_EW);
}

function AP_reappear_add_Audio_FX(%spell_data)
{
  %spell_data.addLingerEffect(AP_ZodeSnd_ARV_EW);
  %spell_data.addLingerEffect(AP_Spinup_ARV_EW);
  %spell_data.addLingerEffect(AP_Spindown_ARV_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
