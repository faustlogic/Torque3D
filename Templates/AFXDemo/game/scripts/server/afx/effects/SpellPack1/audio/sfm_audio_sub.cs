
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SUMMON FECKLESS MOTH (audio)
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

datablock SFXProfile(SFM_ZodiacSnd_CE)
{
   fileName = %mySpellDataPath @ "/SFM/sounds/SFM_zodiac_bed.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(SFM_ZodiacSnd_EW)
{
  effect = SFM_ZodiacSnd_CE;
  constraint = caster;
  delay = 0.0;
  lifetime = 10.734;
};

datablock SFXProfile(SFM_PodGrowSnd_CE)
{
   fileName = %mySpellDataPath @ "/SFM/sounds/SFM_suckInPodGrow.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(SFM_PodGrowSnd_EW)
{
  effect = SFM_PodGrowSnd_CE;
  constraint = caster;
  delay = 7.2;
  lifetime = 5.536;
};

datablock SFXProfile(SFM_PodExplodeSnd_CE)
{
   fileName = %mySpellDataPath @ "/SFM/sounds/SFM_PodExplode.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock afxEffectWrapperData(SFM_PodExplodeSnd_EW)
{
  effect = SFM_PodExplodeSnd_CE;
  constraint = caster;
  delay = 10.6;
  lifetime = 1.701;
};

datablock SFXProfile(SFM_SwarmsSnd_CE)
{
   fileName = %mySpellDataPath @ "/SFM/sounds/SFM_miniMoths.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(SFM_SwarmsSnd_EW)
{
  effect = SFM_SwarmsSnd_CE;
  constraint = caster;
  delay = 3.2;
  lifetime = 7.147;
};

datablock SFXProfile(SFM_GiantMothSnd_CE)
{
   fileName = %mySpellDataPath @ "/SFM/sounds/SFM_giantMoth.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(SFM_GiantMothSnd_EW)
{
  effect = SFM_GiantMothSnd_CE;
  constraint = caster;
  delay = 11.25;
  lifetime = 5.415;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function SFM_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(SFM_ZodiacSnd_EW);
  %spell_data.addCastingEffect(SFM_PodGrowSnd_EW);
  %spell_data.addCastingEffect(SFM_PodExplodeSnd_EW);
  %spell_data.addCastingEffect(SFM_SwarmsSnd_EW);
  %spell_data.addCastingEffect(SFM_GiantMothSnd_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
