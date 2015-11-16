
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// GREAT BALL OF FIRE (audio)
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

datablock SFXProfile(GBoF_ZodeSnd_CE)
{
   fileName = %mySpellDataPath @ "/GBoF/sounds/gbof_zodiac.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(GBoF_ZodeSnd_EW)
{
  effect = GBoF_ZodeSnd_CE;
  constraint = "caster";
  delay = 0;
  lifetime = 1.907;
};

datablock SFXProfile(GBoF_ConjureSnd_CE)
{
   fileName = %mySpellDataPath @ "/GBoF/sounds/gbof_conjure.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(GBoF_ConjureSnd_EW)
{
  effect = GBoF_ConjureSnd_CE;
  constraint = "caster";
  delay = 0.5;
  lifetime = 4.159;
};

datablock SFXProfile(GBoF_Conjure2Snd_CE)
{
  fileName = %mySpellDataPath @ "/GBoF/sounds/gbof_conjure2.ogg";
  description = SpellAudioCasting_AD;
  preload = true;
};
datablock afxEffectWrapperData(GBoF_Conjure2Snd_EW)
{
  effect = GBoF_Conjure2Snd_CE;
  constraint = "caster";
  delay = 4.2;
  lifetime = 3.076;
};

datablock SFXProfile(GBoF_FireBallSnd_CE)
{
   fileName = %mySpellDataPath @ "/GBoF/sounds/projectile_loopFire1a_SR.ogg";
   description = SpellAudioMissileLoop_AD;
   preload = true;
};
datablock afxEffectWrapperData(GBoF_FireBallSnd_EW)
{
  effect = GBoF_FireBallSnd_CE;
  constraint = "caster";
  delay = 4.6;
  lifetime = 2.0;
  fadeoutTime = 0.5;
};

datablock SFXProfile(GBoF_ImpactSnd_CE)
{
   fileName = %mySpellDataPath @ "/GBoF/sounds/gbof_impact1.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock afxEffectWrapperData(GBoF_ImpactSnd_EW)
{
  effect = GBoF_ImpactSnd_CE;
  constraint = "impactPoint";
  delay = 0;
  lifetime = 7.271; 
  execConditions[0] = $AFX::IMPACTED_SOMETHING;
};

datablock SFXProfile(GBoF_Impact2Snd_CE)
{
   fileName = %mySpellDataPath @ "/GBoF/sounds/gbof_Impactextra.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock afxEffectWrapperData(GBoF_Impact2Snd_EW)
{
  effect = GBoF_Impact2Snd_CE;
  constraint = "impactPoint";
  delay = 0.4;
  lifetime = 2.288;
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
};
datablock afxEffectWrapperData(GBoF_Impact3Snd_EW : GBoF_Impact2Snd_EW)
{
  delay = 1.2;
};


datablock afxXM_ShockwaveData(GBoF_Shockwave_XM)
{
  rate = 30.0;
  aimZOnly = true;
};
datablock afxXM_GroundConformData(GBoF_Shockwave_Ground_XM)
{
  height = 1.0;
  conformToInteriors = false;
};

datablock SFXProfile(GBoF_ShockwaveSnd_CE)
{
   fileName = %mySpellDataPath @ "/GBoF/sounds/shwave_loop_SR.ogg";
   description = SpellAudioShockwaveLoop_AD;
   preload = true;
};
datablock afxEffectWrapperData(GBoF_ShockwaveSnd_EW)
{
  effect = GBoF_ShockwaveSnd_CE;
  delay = 1.0;
  lifetime = 4.0;
  fadeInTime = 0.5;
  fadeOutTime = 1.0;

  // the following causes the shockwave sound to move
  // directly toward the listener while tracking the
  // slow white shockwave ring
  posConstraint = "impactedObject";
  posConstraint2 = "listener";
  xfmModifiers[0] = GBoF_Shockwave_XM;
  xfmModifiers[1] = GBoF_Shockwave_Ground_XM;
  execConditions[0] = $AFX::IMPACTED_TARGET;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function GBoF_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(GBoF_ZodeSnd_EW);
  %spell_data.addCastingEffect(GBoF_ConjureSnd_EW);
  %spell_data.addCastingEffect(GBoF_Conjure2Snd_EW);
  %spell_data.addCastingEffect(GBoF_FireBallSnd_EW);
  %spell_data.addImpactEffect(GBoF_ImpactSnd_EW);
  %spell_data.addImpactEffect(GBoF_Impact2Snd_EW);
  %spell_data.addImpactEffect(GBoF_Impact3Snd_EW);
  %spell_data.addImpactEffect(GBoF_ShockwaveSnd_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
