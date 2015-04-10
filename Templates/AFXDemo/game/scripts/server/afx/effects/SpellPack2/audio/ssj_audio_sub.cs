
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SOUL SUCKING JERK (audio)
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

// CASTING

datablock SFXProfile(SSJ_Casting_SND)
{
   fileName = %mySpellDataPath @ "/SSJ/sounds/ssj_casting.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(SSJ_Casting_SND_EW)
{
  effect = SSJ_Casting_SND;
  posConstraint = "caster.Bip01 R Hand";
  delay = 0.95;
  lifetime = 4.0;
};

// IMPACT

datablock SFXProfile(SSJ_Impact_SND)
{
   fileName = %mySpellDataPath @ "/SSJ/sounds/ssj_impact.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock afxEffectWrapperData(SSJ_Impact_SND_EW)
{
  effect = SSJ_Impact_SND;
  posConstraint = "target";
  delay = 2.15;
  lifetime = 1.0;
};

datablock SFXProfile(SSJ_GhostHands_Hit_SND)
{
   fileName = %mySpellDataPath @ "/SSJ/sounds/ssj_ghost_hands_hit.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock afxEffectWrapperData(SSJ_GhostHands_Hit_SND_EW)
{
  effect = SSJ_GhostHands_Hit_SND;
  posConstraint = "target";
  delay = 3.17;
  lifetime = 2.0;
};

datablock SFXProfile(SSJ_GhostHands_Hit2_SND : SSJ_GhostHands_Hit_SND)
{
   fileName = %mySpellDataPath @ "/SSJ/sounds/ssj_ghost_hands_hit02.ogg";
};
datablock afxEffectWrapperData(SSJ_GhostHands_Hit2_SND_EW : SSJ_GhostHands_Hit_SND_EW)
{
  effect = SSJ_GhostHands_Hit2_SND;
  lifetime = 1.0;
};

datablock SFXProfile(SSJ_Soul_Ascend_SND)
{
   fileName = %mySpellDataPath @ "/SSJ/sounds/ssj_soul_ascend.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock afxEffectWrapperData(SSJ_Soul_Ascend_SND_EW)
{
  effect = SSJ_Soul_Ascend_SND;
  posConstraint = "missile";
  delay = 0.17;
  lifetime = 1.6;
};

datablock SFXProfile(SSJ_Soul_Release_SND : SSJ_Soul_Ascend_SND)
{
  fileName = %mySpellDataPath @ "/SSJ/sounds/ssj_soul_release.ogg";
};
datablock afxEffectWrapperData(SSJ_Soul_Release_SND_EW : SSJ_Soul_Ascend_SND_EW)
{
  effect = SSJ_Soul_Release_SND;
};

datablock SFXProfile(SSJ_Soul_Scream_SND)
{
   fileName = %mySpellDataPath @ "/SSJ/sounds/ssj_soul_scream.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock afxEffectWrapperData(SSJ_Soul_Scream_SND_EW)
{
  effect = SSJ_Soul_Scream_SND;
  posConstraint = "target";
  delay = 3.17;
  lifetime = 4.0;
};

// CATCH

datablock SFXProfile(SSJ_Soul_Catch_SND)
{
   fileName = %mySpellDataPath @ "/SSJ/sounds/ssj_soul_catch.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock afxEffectWrapperData(SSJ_Soul_Catch_SND_EW)
{
  effect = SSJ_Soul_Catch_SND;
  posConstraint = "impactedObject";
  delay = 0;
  lifetime = 1.0;
};

// LOOPS

datablock SFXProfile(SSJ_Soul_SND)
{
   fileName = %mySpellDataPath @ "/SSJ/sounds/ssj_soul_loop.ogg";
   description = SpellAudioLoop_AD;
   preload = true;
};
datablock afxEffectWrapperData(SSJ_Soul_SND_EW)
{
  effect = SSJ_Soul_SND;
  posConstraint = "missile";
  fadeInTime = 1.0;
  fadeOutTime = 2.0;
};

datablock SFXProfile(SSJ_SoulChoir_SND)
{
   fileName = %mySpellDataPath @ "/SSJ/sounds/ssj_soul_choir_loop.ogg";
   description = SpellAudioLoop_AD;
   preload = true;
};
datablock afxEffectWrapperData(SSJ_SoulChoir_SND_EW)
{
  effect = SSJ_SoulChoir_SND;
  posConstraint = "missile";
  fadeInTime = 1.0;
  fadeOutTime = 2.0;
};

datablock SFXProfile(SSJ_CasterAtmosphere_SND)
{
   fileName = %mySpellDataPath @ "/SSJ/sounds/ssj_caster_loop.ogg";
   description = SpellAudioLoop_AD;
   preload = true;
};
datablock afxEffectWrapperData(SSJ_CasterAtmosphere_SND_EW)
{
  effect = SSJ_CasterAtmosphere_SND;
  posConstraint = "caster";
  delay = 1.0;
  fadeInTime = 1.0;
  fadeOutTime = 2.0;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function SSJ_add_hand_Audio_FX(%effect_data)
{
  %effect_data.addEffect(SSJ_CasterAtmosphere_SND_EW);
}

function SSJ_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(SSJ_Casting_SND_EW);
  %spell_data.addCastingEffect(SSJ_Impact_SND_EW);
  %spell_data.addCastingEffect(SSJ_GhostHands_Hit_SND_EW);
  %spell_data.addCastingEffect(SSJ_GhostHands_Hit2_SND_EW);
  %spell_data.addCastingEffect(SSJ_Soul_Scream_SND_EW);

  %spell_data.addDeliveryEffect(SSJ_Soul_Ascend_SND_EW);
  %spell_data.addDeliveryEffect(SSJ_Soul_Release_SND_EW);
  %spell_data.addDeliveryEffect(SSJ_Soul_SND_EW);
  %spell_data.addDeliveryEffect(SSJ_SoulChoir_SND_EW);

  %spell_data.addImpactEffect(SSJ_Soul_Catch_SND_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
