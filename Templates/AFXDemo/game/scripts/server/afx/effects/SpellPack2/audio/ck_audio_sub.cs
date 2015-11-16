
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// CHILL KILL (audio)
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

datablock SFXProfile(CK_IceBallSpikes_SND)
{
   fileName = %mySpellDataPath @ "/CK/sounds/ck_iceball_spikes.ogg";
   description = SpellAudioDefault_AD;
   preload = true;
};

datablock SFXProfile(CK_IceBallForming_SND)
{
   fileName = %mySpellDataPath @ "/CK/sounds/ck_iceball_forming.ogg";
   description = SpellAudioDefault_AD;
   preload = true;
};

datablock SFXProfile(CK_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/CK/sounds/ck_explosion_L1.ogg";
   description = SpellAudioDefault_AD;
   preload = true;
};
datablock SFXProfile(CK_Explosion_L2_SND : CK_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/CK/sounds/ck_explosion_L2.ogg";
};
datablock SFXProfile(CK_Explosion_L3_SND : CK_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/CK/sounds/ck_explosion_L3.ogg";
};
datablock SFXProfile(CK_Explosion_L4_SND : CK_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/CK/sounds/ck_explosion_L4.ogg";
};

datablock SFXProfile(CK_Ice_Explode_L1_SND)
{
   fileName = %mySpellDataPath @ "/CK/sounds/ck_ice_explode_L1.ogg";
   description = SpellAudioDefault_AD;
   preload = true;
};
datablock SFXProfile(CK_Ice_Explode_L2_SND : CK_Ice_Explode_L1_SND)
{
   fileName = %mySpellDataPath @ "/CK/sounds/ck_ice_explode_L2.ogg";
};
datablock SFXProfile(CK_Ice_Crash_SND : CK_Ice_Explode_L1_SND)
{
   fileName = %mySpellDataPath @ "/CK/sounds/ck_ice_crash.ogg";
};
datablock SFXProfile(CK_Ice_Sharp_SND : CK_Ice_Explode_L1_SND)
{
   fileName = %mySpellDataPath @ "/CK/sounds/ck_ice_sharp.ogg";
};
datablock SFXProfile(CK_Ice_SharpMix_SND : CK_Ice_Explode_L1_SND)
{
   fileName = %mySpellDataPath @ "/CK/sounds/ck_ice_sharp_mix.ogg";
};
datablock SFXProfile(CK_Ice_Cracking_SND : CK_Ice_Explode_L1_SND)
{
   fileName = %mySpellDataPath @ "/CK/sounds/ck_ice_cracking.ogg";
};

datablock SFXProfile(CK_Blood_L1_SND)
{
   fileName = %mySpellDataPath @ "/CK/sounds/ck_blood_L1.ogg";
   description = SpellAudioDefault_AD;
   preload = true;
};
datablock SFXProfile(CK_Blood_L2_SND : CK_Blood_L1_SND)
{
   fileName = %mySpellDataPath @ "/CK/sounds/ck_blood_L2.ogg";
};
datablock SFXProfile(CK_Blood_L3_SND : CK_Blood_L1_SND)
{
   fileName = %mySpellDataPath @ "/CK/sounds/ck_blood_L3.ogg";
};

datablock SFXProfile(CK_IceBall_Throw_SND)
{
   fileName = %mySpellDataPath @ "/CK/sounds/ck_iceball_throw.ogg";
   description = SpellAudioDefault_AD;
   preload = true;
};

datablock SFXProfile(CK_WindLoop_SND)
{
   fileName = %mySpellDataPath @ "/CK/sounds/ck_wind_loop.ogg";
   description = SpellAudioLoop_AD;
   preload = true;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectWrapperData(CK_IceBallForming_SND_EW)
{
  effect = CK_IceBallForming_SND;
  constraint = "caster";
  delay = 3.2;
  lifetime = 2.0;
};

datablock afxEffectWrapperData(CK_IceBallSpikes_SND_EW)
{
  effect = CK_IceBallSpikes_SND;
  constraint = "caster";
  delay = 4.8;
  lifetime = 0.7;
};

datablock afxEffectWrapperData(CK_Explosion_L1_SND_EW)
{
  effect = CK_Explosion_L1_SND;
  constraint = "caster";
  delay = 0.4;
  lifetime = 2.0;
};

datablock afxEffectWrapperData(CK_Explosion_L2_SND_EW)
{
  effect = CK_Explosion_L2_SND;
  constraint = "caster";
  delay = 1.4;
  lifetime = 4.0;
};

datablock afxEffectWrapperData(CK_Explosion_L3_SND_EW)
{
  effect = CK_Explosion_L3_SND;
  constraint = "caster";
  delay = 0.2;
  lifetime = 2.4;
};

datablock afxEffectWrapperData(CK_Explosion_L4_SND_EW)
{
  effect = CK_Explosion_L4_SND;
  constraint = "caster";
  delay = 2.2;
  lifetime = 3.2;
};

datablock afxEffectWrapperData(CK_Ice_Explode_L1_SND_EW)
{
  effect = CK_Ice_Explode_L1_SND;
  posConstraint = "impactPoint";
  delay = 0.0;
  lifetime = 1.0;
};
datablock afxEffectWrapperData(CK_Ice_Explode_L2_SND_EW : CK_Ice_Explode_L1_SND_EW)
{
  effect = CK_Ice_Explode_L2_SND;
  //delay = 0.2;
  delay = 0.1;
  lifetime = 1.7;
};
datablock afxEffectWrapperData(CK_Ice_Crash_SND_EW : CK_Ice_Explode_L1_SND_EW)
{
  effect = CK_Ice_Crash_SND;
  //delay = 0.1;
  lifetime = 0.6;
};
datablock afxEffectWrapperData(CK_Ice_Sharp_SND_EW : CK_Ice_Explode_L1_SND_EW)
{
  effect = CK_Ice_Sharp_SND;
  //delay = 0.2;
  delay = 0.1;
  lifetime = 0.9;
};
datablock afxEffectWrapperData(CK_Ice_SharpMix_SND_EW : CK_Ice_Explode_L1_SND_EW)
{
  effect = CK_Ice_SharpMix_SND;
  //delay = 0.1;
  lifetime = 1.6;
};
datablock afxEffectWrapperData(CK_Ice_Cracking_SND_EW : CK_Ice_Explode_L1_SND_EW)
{
  effect = CK_Ice_Cracking_SND;
  //delay = 0.1;
  lifetime = 1.2;
};

datablock afxEffectWrapperData(CK_Blood_L1_SND_EW)
{
  effect = CK_Blood_L1_SND;
  posConstraint = "impactPoint";
  delay = 0.1;
  lifetime = 1.5;
};
datablock afxEffectWrapperData(CK_Blood_L2_SND_EW)
{
  effect = CK_Blood_L2_SND;
  posConstraint = "impactPoint";
  delay = 0.2;
  lifetime = 1.5;
};
datablock afxEffectWrapperData(CK_Blood_L3_SND_EW)
{
  effect = CK_Blood_L3_SND;
  posConstraint = "impactPoint";
  delay = 0.35;
  lifetime = 2.0;
};

datablock afxEffectWrapperData(CK_IceBall_Throw_SND_EW)
{
  effect = CK_IceBall_Throw_SND;
  posConstraint = "missile";
  delay = 0.0;
  lifetime = 1.9;
};

datablock afxEffectWrapperData(CK_WindLoop_SND_EW)
{
  effect = CK_WindLoop_SND;
  posConstraint = "impactedObject";
  delay = 0.0;
  lifetime = 8.0;
  fadeInTime = 1.0;
  fadeOutTime = 2.0;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function CK_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(CK_IceBallForming_SND_EW);
  %spell_data.addCastingEffect(CK_IceBallSpikes_SND_EW);
  %spell_data.addCastingEffect(CK_Explosion_L1_SND_EW);
  %spell_data.addCastingEffect(CK_Explosion_L2_SND_EW);
  %spell_data.addCastingEffect(CK_Explosion_L3_SND_EW);
  %spell_data.addCastingEffect(CK_Explosion_L4_SND_EW);

  %spell_data.addImpactEffect(CK_Ice_Explode_L1_SND_EW);
  %spell_data.addImpactEffect(CK_Ice_Explode_L2_SND_EW);
  %spell_data.addImpactEffect(CK_Ice_Crash_SND_EW);
  %spell_data.addImpactEffect(CK_Ice_Sharp_SND_EW);
  %spell_data.addImpactEffect(CK_Ice_SharpMix_SND_EW);
  %spell_data.addImpactEffect(CK_Ice_Cracking_SND_EW);
  %spell_data.addImpactEffect(CK_Blood_L1_SND_EW);
  %spell_data.addImpactEffect(CK_Blood_L2_SND_EW);
  %spell_data.addImpactEffect(CK_Blood_L3_SND_EW);

  %spell_data.addLingerEffect(CK_WindLoop_SND_EW);

  %spell_data.addLaunchEffect(CK_IceBall_Throw_SND_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
