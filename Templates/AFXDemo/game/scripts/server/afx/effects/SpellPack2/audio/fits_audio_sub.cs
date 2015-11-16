
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// FIRE IN THE SKY (audio)
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

datablock SFXDescription(FitS_LoudDrums_AD : SpellAudioDefault_AD)
{
  ReferenceDistance= 60.0;
  MaxDistance= 120.0;
};
datablock SFXProfile(FitS_Drums_SND)
{
   fileName = %mySpellDataPath @ "/FT/sounds/fits_drums.ogg";
   description = FitS_LoudDrums_AD;
   preload = true;
};

datablock SFXProfile(FitS_FinalBeam_L1_SND)
{
   fileName = %mySpellDataPath @ "/FT/sounds/fits_finalBeam_L1.ogg";
   description = SpellAudioCasting_loud_AD;
   preload = true;
};
datablock SFXProfile(FitS_FinalBeam_L2_SND : FitS_FinalBeam_L1_SND)
{
   fileName = %mySpellDataPath @ "/FT/sounds/fits_finalBeam_L2.ogg";
};
datablock SFXProfile(FitS_FinalBeam_L3_SND : FitS_FinalBeam_L1_SND)
{
   fileName = %mySpellDataPath @ "/FT/sounds/fits_finalBeam_L3.ogg";
};
datablock SFXProfile(FitS_Tower_L1_SND : FitS_FinalBeam_L1_SND)
{
   fileName = %mySpellDataPath @ "/FT/sounds/fits_tower_L1.ogg";
};
datablock SFXProfile(FitS_Tower_L2_SND : FitS_FinalBeam_L1_SND)
{
   fileName = %mySpellDataPath @ "/FT/sounds/fits_tower_L2.ogg";
};
datablock SFXProfile(FitS_TowerChorus_SND : FitS_FinalBeam_L1_SND)
{
   fileName = %mySpellDataPath @ "/FT/sounds/fits_towerChorus.ogg";
};

datablock SFXProfile(FitS_RingRumble_SND : FitS_FinalBeam_L1_SND)
{
   fileName = %mySpellDataPath @ "/FT/sounds/fits_ringRumble.ogg";
};
datablock SFXProfile(FitS_TowerRumble_SND : FitS_FinalBeam_L1_SND)
{
   fileName = %mySpellDataPath @ "/FT/sounds/fits_towerRumble.ogg";
};

datablock SFXProfile(FitS_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/FT/sounds/fits_explosion_L1.ogg";
   description = SpellAudioCasting_loud_AD;
   preload = true;
};
datablock SFXProfile(FitS_Explosion_L2_SND : FitS_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/FT/sounds/fits_explosion_L2.ogg";
};
datablock SFXProfile(FitS_Explosion_L3_SND : FitS_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/FT/sounds/fits_explosion_L3.ogg";
};
datablock SFXProfile(FitS_Explosion_L4_SND : FitS_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/FT/sounds/fits_explosion_L4.ogg";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectWrapperData(FitS_Drums_SND_EW)
{
  effect = FitS_Drums_SND;
  delay = 0.35;
  lifetime = 35;
  posConstraint = "strikeLoc";
};

datablock afxEffectWrapperData(FitS_FinalBeam_L1_SND_EW : FitS_Drums_SND_EW)
{
  effect = FitS_FinalBeam_L1_SND;
  delay = 13.35;
  lifetime = 1;
};

datablock afxEffectWrapperData(FitS_FinalBeam_L2_SND_EW : FitS_Drums_SND_EW)
{
  effect = FitS_FinalBeam_L2_SND;
  delay = 14.7;
  lifetime = 2.65;
};

datablock afxEffectWrapperData(FitS_FinalBeam_L3_SND_EW : FitS_Drums_SND_EW)
{
  effect = FitS_FinalBeam_L3_SND;
  delay = 13.7;
  lifetime = 4;
};

datablock afxEffectWrapperData(FitS_Tower_L1_SND_EW : FitS_Drums_SND_EW)
{
  effect = FitS_Tower_L1_SND;
  delay = 17.35;
  lifetime = 5.5;
};
datablock afxEffectWrapperData(FitS_Tower_L2_SND_EW : FitS_Drums_SND_EW)
{
  effect = FitS_Tower_L2_SND;
  delay = 17.35;
  lifetime = 7;
};
datablock afxEffectWrapperData(FitS_TowerChorus_SND_EW : FitS_Drums_SND_EW)
{
  effect = FitS_TowerChorus_SND;
  delay = 15.35;
  lifetime = 12.0;
};

datablock afxEffectWrapperData(FitS_RingRumble_SND_EW : FitS_Drums_SND_EW)
{
  effect = FitS_RingRumble_SND;
  delay = 10.0;
  lifetime = 2.5;
};

datablock afxEffectWrapperData(FitS_TowerRumble_SND_EW : FitS_Drums_SND_EW)
{
  effect = FitS_TowerRumble_SND;
  delay = 17.35;
  lifetime = 12;
};

datablock afxEffectWrapperData(FitS_Explosion_L1_SND_EW : FitS_Drums_SND_EW)
{
  effect = FitS_Explosion_L1_SND;
  delay = 16.35;
  lifetime = 6.0;
};
datablock afxEffectWrapperData(FitS_Explosion_L2_SND_EW : FitS_Explosion_L1_SND_EW)
{
  effect = FitS_Explosion_L2_SND;
  lifetime = 4.0;
};
datablock afxEffectWrapperData(FitS_Explosion_L3_SND_EW : FitS_Explosion_L1_SND_EW)
{
  effect = FitS_Explosion_L3_SND;
  lifetime = 4.0;
};
datablock afxEffectWrapperData(FitS_Explosion_L4_SND_EW : FitS_Explosion_L1_SND_EW)
{
  effect = FitS_Explosion_L4_SND;
  lifetime = 4.0;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// FIREBALL AUDIO

datablock SFXDescription(FitS_FireballLaunch_AD : SpellAudioDefault_AD)
{
  ReferenceDistance  = 5.0;
  MaxDistance        = 100.0;
};
datablock SFXProfile(FitS_FireballLaunch_SND)
{
   fileName = %mySpellDataPath @ "/FT/sounds/fits_fireball_launch.ogg";
   description = FitS_FireballLaunch_AD;
   preload = true;
};
datablock afxEffectWrapperData(FitS_FireballLaunch_SND_EW)
{
  effect = FitS_FireballLaunch_SND;
  lifetime = 3;
  posConstraint = "missile";
};

datablock SFXProfile(FitS_FireballHit_SND)
{
   fileName = %mySpellDataPath @ "/FT/sounds/fits_fireball_hit.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock afxEffectWrapperData(FitS_FireballHit_SND_EW)
{
  effect = FitS_FireballHit_SND;
  lifetime = 2;
  delay = 0.3;
  posConstraint = "impactPoint";
};

datablock SFXProfile(FitS_FireballHit2_SND)
{
   fileName = %mySpellDataPath @ "/FT/sounds/fits_fireball_hit2.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock afxEffectWrapperData(FitS_FireballHit2_SND_EW)
{
  effect = FitS_FireballHit2_SND;
  lifetime = 2;
  delay = 0;
  posConstraint = "impactPoint";
};

datablock SFXDescription(FitS_FireballLoop_AD : SpellAudioDefault_AD)
{
  isLooping          = true;
  ReferenceDistance  = 5.0;
  MaxDistance        = 80.0;
};
datablock SFXProfile(FitS_FireballLoop_SND)
{
   fileName = %mySpellDataPath @ "/FT/sounds/fits_fireball_loop.ogg";
   description = FitS_FireballLoop_AD;
   preload = true;
};
datablock afxEffectWrapperData(FitS_FireballLoop_SND_EW)
{
  effect = FitS_FireballLoop_SND;
  delay = 1;
  fadeInTime = 1;
  posConstraint = "missile";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function FitS_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(FitS_Drums_SND_EW);
  %spell_data.addCastingEffect(FitS_RingRumble_SND_EW);
  %spell_data.addCastingEffect(FitS_FinalBeam_L1_SND_EW);
  %spell_data.addCastingEffect(FitS_FinalBeam_L2_SND_EW);
  %spell_data.addCastingEffect(FitS_FinalBeam_L3_SND_EW);
  %spell_data.addCastingEffect(FitS_Tower_L1_SND_EW);
  %spell_data.addCastingEffect(FitS_Tower_L2_SND_EW);
  %spell_data.addCastingEffect(FitS_TowerChorus_SND_EW);
  %spell_data.addCastingEffect(FitS_TowerRumble_SND_EW);

  %spell_data.addCastingEffect(FitS_Explosion_L1_SND_EW);
  %spell_data.addCastingEffect(FitS_Explosion_L2_SND_EW);
  %spell_data.addCastingEffect(FitS_Explosion_L3_SND_EW);
  %spell_data.addCastingEffect(FitS_Explosion_L4_SND_EW);
}

function FitS_add_fireball_Audio_FX(%spell_data)
{
  %spell_data.addDeliveryEffect(FitS_FireballLaunch_SND_EW);
  %spell_data.addDeliveryEffect(FitS_FireballLoop_SND_EW);
  %spell_data.addImpactEffect(FitS_FireballHit_SND_EW);
  %spell_data.addImpactEffect(FitS_FireballHit2_SND_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
