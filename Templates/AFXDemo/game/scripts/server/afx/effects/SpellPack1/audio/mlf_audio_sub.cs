
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// MAPLELEAF FRAG (audio)
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

datablock SFXProfile(MLF_ZodiacSnd_CE)
{
   fileName = %mySpellDataPath @ "/MLF/sounds/MLF_zodiac.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(MLF_ZodiacSnd_EW)
{
  effect = MLF_ZodiacSnd_CE;
  constraint = caster;
  delay = 0.0;
  lifetime = 1.129;
};

datablock SFXProfile(MLF_WindbedSnd_CE)
{
   fileName = %mySpellDataPath @ "/MLF/sounds/MLF_windbed.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(MLF_WindbedSnd_EW)
{
  effect = MLF_WindbedSnd_CE;
  constraint = caster;
  delay = 0.0;
  lifetime = 7.029;
};

datablock SFXProfile(MLF_Jump1aSnd_CE)
{
   fileName = %mySpellDataPath @ "/MLF/sounds/MLF_JUMP1a.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(MLF_Jump1aSnd_EW)
{
  effect = MLF_Jump1aSnd_CE;
  constraint = caster;
  delay = 0.9;
  lifetime = 1.229;
};

datablock SFXProfile(MLF_Jump2aSnd_CE)
{
   fileName = %mySpellDataPath @ "/MLF/sounds/MLF_JUMP2a.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(MLF_Jump2aSnd_EW)
{
  effect = MLF_Jump2aSnd_CE;
  constraint = caster;
  delay = 2.5;
  lifetime = 1.509;
};

datablock SFXProfile(MLF_Jump3aSnd_CE)
{
   fileName = %mySpellDataPath @ "/MLF/sounds/MLF_JUMP3a.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(MLF_Jump3aSnd_EW)
{
  effect = MLF_Jump3aSnd_CE;
  constraint = caster;
  delay = 4.2;
  lifetime = 5.64;
};

datablock SFXProfile(MLF_Land1Snd_CE)
{
   fileName = %mySpellDataPath @ "/MLF/sounds/MLF_Land1.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(MLF_Land1Snd_EW)
{
  effect = MLF_Land1Snd_CE;
  constraint = caster;
  delay = 1.4;
  lifetime = 1.663;
};

datablock SFXProfile(MLF_Land2Snd_CE)
{
   fileName = %mySpellDataPath @ "/MLF/sounds/MLF_Land2.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(MLF_Land2Snd_EW)
{
  effect = MLF_Land2Snd_CE;
  constraint = caster;
  delay = 3.4;
  lifetime = 1.751;
};

datablock SFXProfile(MLF_HandclapSnd_CE)
{
   fileName = %mySpellDataPath @ "/MLF/sounds/MLF_Handclap.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(MLF_HandclapSnd_EW)
{
  effect = MLF_HandclapSnd_CE;
  constraint = caster;
  delay = 4.9;
  lifetime = 2.849;
};

datablock SFXProfile(MLF_Castland1aSnd_CE)
{
   fileName = %mySpellDataPath @ "/MLF/sounds/MLF_castland1a.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(MLF_Castland1aSnd_EW)
{
  effect = MLF_Castland1aSnd_CE;
  constraint = caster;
  delay = 8.2;
  lifetime = 0.754;
};

datablock SFXProfile(MLF_Castland2Snd_CE)
{
   fileName = %mySpellDataPath @ "/MLF/sounds/MLF_castland2.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(MLF_Castland2Snd_EW)
{
  effect = MLF_Castland2Snd_CE;
  constraint = caster;
  delay = 9.2;
  lifetime = 0.363;
};

datablock SFXProfile(MLF_TornadoLoop_CE)
{
   fileName = %mySpellDataPath @ "/MLF/sounds/MLF_loop.ogg";
   description = SpellAudioMissileLoop_AD;
   preload = true;
};

datablock SFXProfile(MLF_TargetlandSnd_CE)
{
   fileName = %mySpellDataPath @ "/MLF/sounds/MLF_targetland.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(MLF_TargetlandSnd_EW)
{
  effect = MLF_TargetlandSnd_CE;
  constraint = "impactedObject";
  delay = 1.6;
  lifetime = 0.467;
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
};

datablock SFXProfile(MLF_LastBloodSnd_CE)
{
   fileName = %mySpellDataPath @ "/MLF/sounds/MLF_lastblood.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
//
datablock afxEffectWrapperData(MLF_LastBlood1Snd_EW)
{
  effect = MLF_LastBloodSnd_CE;
  constraint = "impactedObject";
  delay = 0.2;
  lifetime = 0.788;
  lifeConstraint = "impactedObject";
  lifeConditions = $AFX::ALIVE;
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
};
datablock afxEffectWrapperData(MLF_LastBlood2Snd_EW : MLF_LastBlood1Snd_EW)
{
  delay = 0.4;
};
datablock afxEffectWrapperData(MLF_LastBlood3Snd_EW : MLF_LastBlood1Snd_EW)
{
  delay = 1.6;
};
datablock afxEffectWrapperData(MLF_LastBlood4Snd_EW : MLF_LastBlood1Snd_EW)
{
  delay = 2.0;
};
datablock afxEffectWrapperData(MLF_LastBlood5Snd_EW : MLF_LastBlood1Snd_EW)
{
  delay = 2.2;
};

datablock SFXProfile(MLF_Kill_Snd_CE)
{
   fileName = %mySpellDataPath @ "/MLF/sounds/MLF_kill_big.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(MLF_Kill_Snd_EW)
{
  effect = MLF_Kill_Snd_CE;
  constraint = "impactedObject";
  delay = 0.2;
  lifetime = 6.126;
  lifeConstraint = "impactedObject";
  lifeConditions = $AFX::DYING;
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
};

datablock SFXProfile(MLF_ImpactSnd_CE)
{
   fileName = %mySpellDataPath @ "/MLF/sounds/MLF_impact.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock afxEffectWrapperData(MLF_ImpactSnd_EW)
{
  effect = MLF_ImpactSnd_CE;
  constraint = "impactPoint";
  delay = 0;
  lifetime = 2.327;
  execConditions[0] = $AFX::IMPACTED_SOMETHING;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function MLF_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(MLF_ZodiacSnd_EW);
  %spell_data.addCastingEffect(MLF_WindbedSnd_EW);
  %spell_data.addCastingEffect(MLF_Jump1aSnd_EW);
  %spell_data.addCastingEffect(MLF_Jump2aSnd_EW);
  %spell_data.addCastingEffect(MLF_Jump3aSnd_EW);
  %spell_data.addCastingEffect(MLF_Land1Snd_EW);
  %spell_data.addCastingEffect(MLF_Land2Snd_EW);
  %spell_data.addCastingEffect(MLF_HandclapSnd_EW);
  %spell_data.addCastingEffect(MLF_Castland1aSnd_EW);
  %spell_data.addCastingEffect(MLF_Castland2Snd_EW);
  %spell_data.addImpactEffect(MLF_ImpactSnd_EW);
  %spell_data.addImpactEffect(MLF_TargetlandSnd_EW);
  %spell_data.addImpactEffect(MLF_LastBlood1Snd_EW);
  %spell_data.addImpactEffect(MLF_LastBlood2Snd_EW);
  %spell_data.addImpactEffect(MLF_LastBlood3Snd_EW);
  %spell_data.addImpactEffect(MLF_LastBlood4Snd_EW);
  %spell_data.addImpactEffect(MLF_LastBlood5Snd_EW);
  %spell_data.addImpactEffect(MLF_Kill_Snd_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
