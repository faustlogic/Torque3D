
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// FLAMING STICK TRICK (audio)
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

datablock SFXProfile(FST_StaffFireStart_SND)
{
   fileName = %mySpellDataPath @ "/FST/sounds/fst_staffFire_Start.ogg";
   description = SpellAudioCasting_loud_AD;
   preload = true;
};
datablock SFXProfile(FST_staffFireCrackle_SND : FST_StaffFireStart_SND)
{
   fileName = %mySpellDataPath @ "/FST/sounds/fst_staffFire.ogg";
};
datablock SFXProfile(FST_StaffFireEnd_SND : FST_StaffFireStart_SND)
{
   fileName = %mySpellDataPath @ "/FST/sounds/fst_staffFire_End.ogg";
};

datablock SFXProfile(FST_CastBeam_L1_SND)
{
   fileName = %mySpellDataPath @ "/FST/sounds/fst_castBeam_L1.ogg";
   description = SpellAudioCasting_loud_AD;
   preload = true;
};
datablock SFXProfile(FST_CastBeam_L2_SND : FST_CastBeam_L1_SND)
{
   fileName = %mySpellDataPath @ "/FST/sounds/fst_castBeam_L2.ogg";
};

datablock SFXProfile(FST_PentBeamA_L1_SND : FST_CastBeam_L1_SND)
{
   fileName = %mySpellDataPath @ "/FST/sounds/fst_pentBeamA_L1.ogg";
};
datablock SFXProfile(FST_PentBeamA_L2_SND : FST_CastBeam_L1_SND)
{
   fileName = %mySpellDataPath @ "/FST/sounds/fst_pentBeamA_L2.ogg";
};

datablock SFXProfile(FST_PentBeamB_L1_SND : FST_CastBeam_L1_SND)
{
   fileName = %mySpellDataPath @ "/FST/sounds/fst_pentBeamB_L1.ogg";
};
datablock SFXProfile(FST_PentBeamB_L2_SND : FST_CastBeam_L1_SND)
{
   fileName = %mySpellDataPath @ "/FST/sounds/fst_pentBeamB_L2.ogg";
};


datablock SFXProfile(FST_staffPlant_L1_SND : FST_CastBeam_L1_SND)
{
   fileName = %mySpellDataPath @ "/FST/sounds/fst_staffPlant_L1.ogg";
};
datablock SFXProfile(FST_staffPlant_L2_SND : FST_staffPlant_L1_SND)
{
   fileName = %mySpellDataPath @ "/FST/sounds/fst_staffPlant_L2.ogg";
};
datablock SFXProfile(FST_staffPlant_L3_SND : FST_staffPlant_L1_SND)
{
   fileName = %mySpellDataPath @ "/FST/sounds/fst_staffPlant_L3.ogg";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectWrapperData(FST_CastBeam_L1_SND_EW)
{
  effect = FST_CastBeam_L1_SND;
  delay = 0.35;
  lifetime = 2.5;
  posConstraint = "zodeAnchor";
};
datablock afxEffectWrapperData(FST_CastBeam_L2_SND_EW : FST_CastBeam_L1_SND_EW)
{
  effect = FST_CastBeam_L2_SND;
  lifetime = 2.5;
};
datablock afxEffectWrapperData(FST_PentBeamA_L1_SND_EW)
{
  effectEnabled = "$$ %%._beam";
  effect = FST_PentBeamA_L1_SND;
  delay = 0.35 + 2.5;
  lifetime = 6.5;
  posConstraint = "strikeLoc";
};
datablock afxEffectWrapperData(FST_PentBeamA_L2_SND_EW : FST_PentBeamA_L1_SND_EW)
{
  effectEnabled = "$$ %%._beam";
  effect = FST_PentBeamA_L2_SND;
  lifetime = 2.0;
};

datablock afxEffectWrapperData(FST_staffPlant_L1_SND_EW)
{
  effect = FST_staffPlant_L1_SND;
  delay = 9.04;
  lifetime = 1.5;
  posConstraint = "strikeLoc";
};
datablock afxEffectWrapperData(FST_staffPlant_L2_SND_EW : FST_staffPlant_L1_SND_EW)
{
  effect = FST_staffPlant_L2_SND;
  lifetime = 2.3;
};
datablock afxEffectWrapperData(FST_staffPlant_L3_SND_EW : FST_staffPlant_L1_SND_EW)
{
  effect = FST_staffPlant_L3_SND;
  lifetime = 2.0;
};

datablock afxEffectWrapperData(FST_PentBeamB_L1_SND_EW)
{
  effectEnabled = "$$ %%._beam";
  effect = FST_PentBeamB_L1_SND;
  delay = 11.85;
  lifetime = 0.9;
  posConstraint = "strikeLoc";
};
datablock afxEffectWrapperData(FST_PentBeamB_L2_SND_EW)
{
  effectEnabled = "$$ %%._beam";
  effect = FST_PentBeamB_L2_SND;
  delay = 11.85;
  lifetime = 1;
  posConstraint = "strikeLoc";
};


datablock afxEffectWrapperData(FST_StaffFire_Top_SND_EW : FST_StaffFire_Top_EW)
{
  effect = FST_StaffFireStart_SND;
  lifetime = 1.3;
  //fadeInTime = 2.0;
};
datablock afxEffectWrapperData(FST_StaffFire_Bot_SND_EW : FST_StaffFire_Bot_EW)
{
  effect = FST_StaffFireStart_SND;
  lifetime = 1.3;
  //fadeInTime = 2.0;
};

datablock afxEffectWrapperData(FST_StaffFireCrackle_Top_SND_EW : FST_StaffFire_Top_EW)
{
  effect = FST_staffFireCrackle_SND;
  delay = 6.6;
  lifetime = 2;
};

datablock afxEffectWrapperData(FST_StaffFireCrackle_Bot_SND_EW : FST_StaffFire_Bot_EW)
{
  effect = FST_staffFireCrackle_SND;
  delay = 7.0;
  lifetime = 2;
};

datablock afxEffectWrapperData(FST_StaffFirePlant_SND_EW)
{
  effect = FST_StaffFireEnd_SND;
  posConstraint = "strikeLoc";
  //delay = $FST_Cue_StaffPlant;
  delay = $FST_Cue_StaffVanish-0.5;
  lifetime = 1.8;
  //fadeInTime = 2.0;
};

datablock afxEffectGroupData(FST_FireInTheSky_SND_EG)
{
  groupEnabled = "$$ %%._conjure != true";
  addEffect = FST_CastBeam_L1_SND_EW;
  addEffect = FST_CastBeam_L2_SND_EW;
  addEffect = FST_PentBeamA_L1_SND_EW;
  addEffect = FST_PentBeamA_L2_SND_EW;
  addEffect = FST_PentBeamB_L1_SND_EW;
  addEffect = FST_PentBeamB_L2_SND_EW;
  addEffect = FST_staffPlant_L1_SND_EW;
  addEffect = FST_staffPlant_L2_SND_EW;
  addEffect = FST_staffPlant_L3_SND_EW;
  addEffect = FST_StaffFire_Top_SND_EW;
  addEffect = FST_StaffFire_Bot_SND_EW;
  addEffect = FST_StaffFireCrackle_Top_SND_EW;
  addEffect = FST_StaffFireCrackle_Bot_SND_EW;
  addEffect = FST_StaffFirePlant_SND_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

datablock SFXProfile(FST_Conjure_Enter_SND)
{
   fileName = %mySpellDataPath @ "/FST/sounds/fst_conjure_enter.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(FST_Conjure_Enter_SND_EW)
{
  effect = FST_Conjure_Enter_SND;
  delay = 0.5;
  lifetime = 3.0;
  posConstraint = "zodeAnchor";
};

datablock SFXProfile(FST_Conjure_Wobble_SND)
{
   fileName = %mySpellDataPath @ "/FST/sounds/fst_conjure_wobble.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(FST_Conjure_Wobble_SND_EW)
{
  effect = FST_Conjure_Wobble_SND;
  delay = 1.5;
  lifetime = 8.0;
  posConstraint = "zodeAnchor";
};

datablock SFXProfile(FST_Conjure_LtExplosion_SND)
{
   fileName = %mySpellDataPath @ "/FST/sounds/fst_conjure_lt_explosion.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(FST_Conjure_LtExplosion_SND_EW)
{
  effect = FST_Conjure_LtExplosion_SND;
  delay = 9.5;
  lifetime = 2.0;
  posConstraint = "zodeAnchor";
};

datablock SFXProfile(FST_Conjure_LtExplosion2_SND)
{
   fileName = %mySpellDataPath @ "/FST/sounds/fst_conjure_lt_explosion2.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(FST_Conjure_LtExplosion2_SND_EW)
{
  effect = FST_Conjure_LtExplosion2_SND;
  delay = 9;
  lifetime = 2.0;
  posConstraint = "zodeAnchor";
};


datablock SFXProfile(FST_Conjure_Exit_SND)
{
   fileName = %mySpellDataPath @ "/FST/sounds/fst_conjure_exit.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(FST_Conjure_Exit_SND_EW)
{
  effect = FST_Conjure_Exit_SND;
  delay = 12;
  lifetime = 1.0;
  posConstraint = "zodeAnchor";
};

datablock SFXProfile(FST_Conjure_Disappear_SND)
{
   fileName = %mySpellDataPath @ "/FST/sounds/fst_conjure_disappear.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(FST_Conjure_Disappear_SND_EW)
{
  effect = FST_Conjure_Disappear_SND;
  delay = 11.5;
  lifetime = 3.0;
  posConstraint = "zodeAnchor";
};


//~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectGroupData(FST_Conjuring_SND_EG)
{
  groupEnabled = "$$ %%._conjure == true";
  addEffect = FST_Conjure_Enter_SND_EW;
  addEffect = FST_Conjure_Wobble_SND_EW;
  addEffect = FST_Conjure_LtExplosion_SND_EW;
  addEffect = FST_Conjure_LtExplosion2_SND_EW;
  addEffect = FST_Conjure_Exit_SND_EW;
  addEffect = FST_Conjure_Disappear_SND_EW;

  addEffect = FST_staffPlant_L3_SND_EW;
  addEffect = FST_StaffFire_Top_SND_EW;
  addEffect = FST_StaffFire_Bot_SND_EW;
  addEffect = FST_StaffFireCrackle_Top_SND_EW;
  addEffect = FST_StaffFireCrackle_Bot_SND_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function FST_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(FST_FireInTheSky_SND_EG);
  %spell_data.addCastingEffect(FST_Conjuring_SND_EG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
