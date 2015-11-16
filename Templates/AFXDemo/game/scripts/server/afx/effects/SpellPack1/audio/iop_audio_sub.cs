
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// INSECTOPLASM (audio)
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

// missile sound //
datablock SFXProfile(IOP_BugLoopSnd_CE)
{
  fileName = %mySpellDataPath @ "/IOP/sounds/IOP_bug_loop.ogg";
  description = SpellAudioMissileLoop_loud_AD;
  preload = true;
};

// caster sounds

datablock SFXProfile(IOP_ZodiacSnd_CE)
{
   fileName = %mySpellDataPath @ "/IOP/sounds/IOP_zodiac.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(IOP_ZodiacSnd_EW)
{
  effect = IOP_ZodiacSnd_CE;
  constraint = "caster";
  delay = 0.0;
  lifetime = 1.079;
};

datablock SFXProfile(IOP_ConjureSnd_CE)
{
   fileName = %mySpellDataPath @ "/IOP/sounds/IOP_conjure.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(IOP_ConjureSnd_EW)
{
  effect = IOP_ConjureSnd_CE;
  constraint = "caster";
  delay = 0.0;
  lifetime = 14.7;
};

datablock SFXProfile(IOP_BeetlesSnd_CE)
{
   fileName = %mySpellDataPath @ "/IOP/sounds/IOP_beetles.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(IOP_BeetlesSnd_EW)
{
  effect = IOP_BeetlesSnd_CE;
  constraint = "caster";
  delay = 0.85;
  lifetime = 7.613;
};

datablock SFXProfile(IOP_LevitationSnd_CE)
{
   fileName = %mySpellDataPath @ "/IOP/sounds/IOP_levitation.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(IOP_LevitationSnd_EW)
{
  effect = IOP_LevitationSnd_CE;
  constraint = "caster";
  delay = 7.6;
  lifetime = 8.993;
};

datablock SFXProfile(IOP_LaunchSnd_CE)
{
   fileName = %mySpellDataPath @ "/IOP/sounds/IOP_launch.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(IOP_LaunchSnd_EW)
{
  effect = IOP_LaunchSnd_CE;
  constraint = "caster";
  delay = 13.2;
  lifetime = 1.628;
};

datablock SFXProfile(IOP_BugSnd_CE)
{
   fileName = %mySpellDataPath @ "/IOP/sounds/IOP_bug_loop.ogg";
   description = SpellAudioMissileLoop_loud_AD;
   preload = true;
};

datablock afxEffectWrapperData(IOP_BugSnd_EW)
{
  effect = IOP_BugSnd_CE;
  constraint = "caster";
  delay = 13.5;
  lifetime = 1.8;
  fadeInTime = 0.5;
  fadeOutTime = 1.4;
};

// missle sounds //

datablock SFXProfile(IOP_SwoopSnd_CE)
{
   fileName = %mySpellDataPath @ "/IOP/sounds/IOP_pre_impact_swoop.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(IOP_SwoopSnd_EW)
{
  effect = IOP_SwoopSnd_CE;
  constraint = "missile";
  delay = 3;
  lifetime = 3.092;
};

// impact sounds //

datablock SFXProfile(IOP_ImpactSnd_CE)
{
   fileName = %mySpellDataPath @ "/IOP/sounds/IOP_impact.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};

datablock afxEffectWrapperData(IOP_ImpactSnd_1_EW)
{
  effect = IOP_ImpactSnd_CE;
  constraint = "impactedObject";
  delay = 0.0;
  lifetime = 2.344;
};

datablock afxEffectWrapperData(IOP_ImpactSnd_2_EW)
{
  effect = IOP_ImpactSnd_CE;
  constraint = "impactedObject";
  delay = 0.6;
  lifetime = 2.344;
  scaleFactor = 0.9;
};

datablock afxEffectWrapperData(IOP_ImpactSnd_3_EW)
{
  effect = IOP_ImpactSnd_CE;
  constraint = "impactedObject";
  delay = 1.0;
  lifetime = 2.344;
  scaleFactor = 0.75;
};

datablock SFXProfile(IOP_SplatSnd_CE)
{
   fileName = %mySpellDataPath @ "/IOP/sounds/IOP_splat.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};

datablock afxEffectWrapperData(IOP_SplatSnd_EW)
{
  effect = IOP_SplatSnd_CE;
  constraint = "impactedObject";
  delay = 1.6;
  lifetime = 1.263;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function IOP_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(IOP_ZodiacSnd_EW);
  %spell_data.addCastingEffect(IOP_ConjureSnd_EW);
  %spell_data.addCastingEffect(IOP_BeetlesSnd_EW);
  %spell_data.addCastingEffect(IOP_LevitationSnd_EW);
  %spell_data.addCastingEffect(IOP_LaunchSnd_EW);
  %spell_data.addCastingEffect(IOP_BugSnd_EW);
  %spell_data.addDeliveryEffect(IOP_SwoopSnd_EW);
  %spell_data.addImpactEffect(IOP_ImpactSnd_1_EW);
  %spell_data.addImpactEffect(IOP_ImpactSnd_2_EW);
  %spell_data.addImpactEffect(IOP_ImpactSnd_3_EW);
  %spell_data.addImpactEffect(IOP_SplatSnd_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
