
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SPIRIT OF ROACH (audio)
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

// caster sounds

datablock SFXProfile(SoR_ZodiacSnd_CE)
{
   fileName = %mySpellDataPath @ "/SoR/sounds/sor_zodiac.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(SoR_ZodiacSnd_EW)
{
  effect = SoR_ZodiacSnd_CE;
  constraint = "caster";
  delay = 0.0;
  lifetime = 1.999;
};

datablock SFXProfile(SoR_ConjureSnd_CE)
{
   fileName = %mySpellDataPath @ "/SoR/sounds/sor_conjure.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(SoR_ConjureSnd_EW)
{
  effect = SoR_ConjureSnd_CE;
  constraint = "caster";
  delay = 0.8;
  lifetime = 2.141;
};

// target sounds //

datablock SFXProfile(SoR_TargetBugoffSnd_CE)
{
   fileName = %mySpellDataPath @ "/SoR/sounds/sor_targ_bugoff.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(SoR_TargetBugoffSnd_EW)
{
  effect = SoR_TargetBugoffSnd_CE;
  constraint = "impactedObject";
  delay = 3.0;
  scaleFactor = 0.8;
  lifetime = 0.269;
};

datablock SFXProfile(SoR_TargetEndSnd_CE)
{
   fileName = %mySpellDataPath @ "/SoR/sounds/sor_targ_end.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(SoR_TargetEndSnd_EW)
{
  effect = SoR_TargetEndSnd_CE;
  constraint = "impactedObject";
  delay = 5.5;
  scaleFactor = 0.8;
  lifetime = 1.096;
};

datablock SFXProfile(SoR_TargetManifestSnd_CE)
{
   fileName = %mySpellDataPath @ "/SoR/sounds/sor_targ_manif.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(SoR_TargetManifestSnd_EW)
{
  effect = SoR_TargetManifestSnd_CE;
  constraint = "impactedObject";
  delay = 0.5;
  scaleFactor = 0.8;
  lifetime = 2.494;
};

datablock SFXProfile(SoR_TargetManifestLoop_CE)
{
   fileName = %mySpellDataPath @ "/SoR/sounds/sor_targ_manif_loop.ogg";
   description = SpellAudioLoop_AD;
   preload = true;
};
datablock afxEffectWrapperData(SoR_TargetManifestLoop_EW)
{
  effect = SoR_TargetManifestLoop_CE;
  constraint = "impactedObject";
  delay = 1.0;
  lifetime = 4.5;
  fadeOutTime = 1.2;
  scaleFactor = 0.8;
};

datablock SFXProfile(SoR_TargetZodeSnd_CE)
{
   fileName = %mySpellDataPath @ "/SoR/sounds/sor_targ_zode.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(SoR_TargetZodeSnd_EW)
{
  effect = SoR_TargetZodeSnd_CE;
  constraint = "impactedObject";
  delay = 0;
  scaleFactor = 0.8;
  lifetime = 1.483;
};


// shockwave sounds //

datablock afxXM_ShockwaveData(SoR_Shockwave_XM)
{
  rate = 20.0;
  aimZOnly = true;
};
datablock afxXM_GroundConformData(SoR_Shockwave_Ground_XM)
{
  height = 1.0;
  conformToInteriors = false;
};

datablock SFXProfile(SoR_ShockwaveSnd_CE)
{
   fileName = %mySpellDataPath @ "/SoR/sounds/sor_ring_loop.ogg";
   description = SpellAudioShockwaveLoop_soft_AD;
   preload = true;
};
datablock afxEffectWrapperData(SoR_ShockwaveSnd_EW)
{
  effect = SoR_ShockwaveSnd_CE;
  delay = 1.35;
  lifetime = 3.0;
  fadeInTime = 0.5;
  fadeOutTime = 1.0;

  posConstraint = "caster";
  posConstraint2 = "listener";
  xfmModifiers[0] = SoR_Shockwave_XM;
  xfmModifiers[1] = SoR_Shockwave_Ground_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function SoR_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(SoR_ZodiacSnd_EW);
  %spell_data.addCastingEffect(SoR_ConjureSnd_EW);
  %spell_data.addCastingEffect(SoR_ShockwaveSnd_EW);
  %spell_data.addImpactEffect(SoR_TargetZodeSnd_EW);
  %spell_data.addImpactEffect(SoR_TargetManifestSnd_EW);
  %spell_data.addImpactEffect(SoR_TargetManifestLoop_EW);
  %spell_data.addImpactEffect(SoR_TargetBugoffSnd_EW);
  %spell_data.addImpactEffect(SoR_TargetEndSnd_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
