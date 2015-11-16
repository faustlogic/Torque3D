
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// BRON-Y-ORC STOMP (audio)
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
// STOMP SOUNDS

// 3 AUDIO FOOTSTEP VARIATIONS

// LOUDEST for heavy impacts
datablock SFXProfile(BYOS_FootImpactSnd_A_CE)
{
   fileName = %mySpellDataPath @ "/BYOS/sounds/byos_crunch_1.ogg";
   description = SpellAudioCasting_loud_AD;
   preload = true;
};
datablock afxEffectWrapperData(BYOS_FootImpactSnd_LF_A_EW)
{
  effectEnabled = "$$ %%._triggerScale[##] > 0.8";
  effect = BYOS_FootImpactSnd_A_CE;
  lifetime = 0.8616;
  posConstraint = "target.Bip01 L Foot";
};
datablock afxEffectWrapperData(BYOS_FootImpactSnd_RT_A_EW : BYOS_FootImpactSnd_LF_A_EW)
{
  posConstraint = "target.Bip01 R Foot";
};

// MEDIUM for medium impacts
datablock SFXProfile(BYOS_FootImpactSnd_B_CE)
{
   fileName = %mySpellDataPath @ "/BYOS/sounds/byos_crunch_2.ogg";
   description =SpellAudioCasting_loud_AD;
   preload = true;
};
datablock afxEffectWrapperData(BYOS_FootImpactSnd_LF_B_EW)
{
  effectEnabled = "$$ (%%._triggerScale[##] > 0.6) &&  (%%._triggerScale[##] <= 0.8)";
  effect = BYOS_FootImpactSnd_B_CE;
  lifetime = 0.6634;
  posConstraint = "target.Bip01 L Foot";
};
datablock afxEffectWrapperData(BYOS_FootImpactSnd_RT_B_EW : BYOS_FootImpactSnd_LF_B_EW)
{
  posConstraint = "target.Bip01 R Foot";
};

// SOFTEST for light impacts
datablock SFXProfile(BYOS_FootImpactSnd_C_CE)
{
   fileName = %mySpellDataPath @ "/BYOS/sounds/byos_crunch_3.ogg";
   description = SpellAudioCasting_loud_AD;
   preload = true;
};
datablock afxEffectWrapperData(BYOS_FootImpactSnd_LF_C_EW)
{
  effectEnabled = "$$ %%._triggerScale[##] <= 0.6";
  effect = BYOS_FootImpactSnd_C_CE;
  lifetime =  0.6533;
  posConstraint = "target.Bip01 L Foot";
};
datablock afxEffectWrapperData(BYOS_FootImpactSnd_RT_C_EW : BYOS_FootImpactSnd_LF_C_EW)
{
  posConstraint = "target.Bip01 R Foot";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// 18 AUDIO GRUNT VARIATIONS

datablock afxAudioBank(BYOS_VocalSnd_RunGrunts_CE)
{
  playIndex = "$$ %%._triggerRandomSnd[##]";
  path = %mySpellDataPath @ "/BYOS/sounds";
  filenames[0] = "byos_orc_vox_grunt_1.ogg";
    filenames[1] = "byos_orc_vox_grunt_2.ogg";
    filenames[2] = "byos_orc_vox_grunt_3.ogg";
    filenames[3] = "byos_orc_vox_grunt_4.ogg";
    filenames[4] = "byos_orc_vox_grunt_5.ogg";
    filenames[5] = "byos_orc_vox_grunt_6.ogg";
    filenames[6] = "byos_orc_vox_grunt_7.ogg";
    filenames[7] = "byos_orc_vox_grunt_8.ogg";
    filenames[8] = "byos_orc_vox_grunt_9.ogg";
    filenames[9] = "byos_orc_vox_grunt_10.ogg";
    filenames[10] = "byos_orc_vox_grunt_11.ogg";
    filenames[11] = "byos_orc_vox_grunt_12.ogg";
    filenames[12] = "byos_orc_vox_grunt_13.ogg";
    filenames[13] = "byos_orc_vox_grunt_14.ogg";
    filenames[14] = "byos_orc_vox_grunt_15.ogg";
    filenames[15] = "byos_orc_vox_grunt_16.ogg";
    filenames[16] = "byos_orc_vox_grunt_17.ogg";
    filenames[17] = "byos_orc_vox_grunt_18.ogg";
    filenames[18] = "byos_orc_vox_yell.ogg";
  description = SpellAudioCasting_loud_AD;
  preload = true;
};
datablock afxEffectWrapperData(BYOS_VocalSnd_RunGrunts_EW)
{
  effectEnabled = "$$ %%._useVoices == true";
  effect = BYOS_VocalSnd_RunGrunts_CE;
  posConstraint = target;
  lifetime =  2.0;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxAudioBank(BYOS_VocalSnd_LandingGrunts_CE)
{
  playIndex = "$$ %%._triggerRandomSndLAND";
  path = %mySpellDataPath @ "/BYOS/sounds";
  filenames[0] = "byos_orc_vox_landing_grunt_1.ogg";
    filenames[1] = "byos_orc_vox_landing_grunt_2.ogg";
    filenames[2] = "byos_orc_vox_landing_grunt_3.ogg";
    filenames[3] = "byos_orc_vox_die.ogg";
    filenames[4] = "byos_orc_vox_move_fool.ogg";
    filenames[5] = "byos_orc_vox_move_it.ogg";
  description = SpellAudioCasting_loud_AD;
  preload = true;
};
datablock afxEffectWrapperData(BYOS_VocalSnd_LandingGrunts_EW)
{
  effectEnabled = "$$ %%._useVoices == true";
  effect = BYOS_VocalSnd_LandingGrunts_CE;
  posConstraint = target;
  lifetime =  2.0;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// JUMP LANDING SOUNDS

datablock SFXProfile(BYOS_LandImpactSnd_A_CE)
{
  fileName = %mySpellDataPath @ "/BYOS/sounds/byos_landing_impact_2.ogg";
  description =SpellAudioCasting_loud_AD;
  preload = true;
};
datablock SFXProfile(BYOS_LandImpactSnd_B_CE)
{
  fileName = %mySpellDataPath @ "/BYOS/sounds/byos_landing_impact_3.ogg";
  description =SpellAudioCasting_loud_AD;

  preload = true;
};

datablock afxEffectWrapperData(BYOS_LandImpactSnd_A_EW)
{
  effectEnabled = "$$ %%._triggerScaleLAND < 0.8";
  effect = BYOS_LandImpactSnd_A_CE;
  posConstraint = "target";
  lifetime = 2.0510;
};
datablock afxEffectWrapperData(BYOS_LandImpactSnd_B_EW)
{
  effectEnabled = "$$ %%._triggerScaleLAND >= 0.8";
  effect = BYOS_LandImpactSnd_B_CE;
  posConstraint = "target";
  lifetime = 2.3499; 
};

//~~~~~~~~~~~~~~~~~~~~//

datablock SFXProfile(BYOS_VocalSnd_KorkStomp_CE)
{
  fileName = %mySpellDataPath @ "/BYOS/sounds/byos_orc_vox_kork_stomp.ogg";
  description = SpellAudioCasting_loud_AD;
  preload = true;
};
datablock afxEffectWrapperData(BYOS_VocalSnd_KorkStomp_EW)
{
  effectEnabled = "$$ %%._useVoices == true";
  effect = BYOS_VocalSnd_KorkStomp_CE;
  posConstraint = target;
  delay = 0;  
  lifetime = 2.5908;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function BYOS_add_footstep_Audio_FX(%phrase_lf, %phrase_rt)
{
  %phrase_lf.addEffect(BYOS_FootImpactSnd_LF_A_EW);
  %phrase_lf.addEffect(BYOS_FootImpactSnd_LF_B_EW);
  %phrase_lf.addEffect(BYOS_FootImpactSnd_LF_C_EW);
  %phrase_lf.addEffect(BYOS_VocalSnd_RunGrunts_EW);

  %phrase_rt.addEffect(BYOS_FootImpactSnd_RT_A_EW);
  %phrase_rt.addEffect(BYOS_FootImpactSnd_RT_B_EW);
  %phrase_rt.addEffect(BYOS_FootImpactSnd_RT_C_EW);
  %phrase_rt.addEffect(BYOS_VocalSnd_RunGrunts_EW);
}

function BYOS_add_landing_Audio_FX(%phrase_landing)
{
  %phrase_landing.addEffect(BYOS_LandImpactSnd_A_EW);
  %phrase_landing.addEffect(BYOS_LandImpactSnd_B_EW);
  %phrase_landing.addEffect(BYOS_VocalSnd_LandingGrunts_EW);
}

function BYOS_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(BYOS_VocalSnd_KorkStomp_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
