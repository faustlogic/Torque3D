
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// WANDERING WISPS (audio)
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

datablock SFXDescription(WW_WispLoop_AD : SpellAudioDefault_AD)
{
  isLooping          = true;
  ReferenceDistance  = 5.0;
  MaxDistance        = 80.0;
};
datablock SFXProfile(WW_WispLoop_SND)
{
   fileName = %mySpellDataPath @ "/WW/sounds/ww_wisp_loop.ogg";
   description = WW_WispLoop_AD;
   preload = true;
};

datablock SFXProfile(WW_WispDone_SND)
{
   fileName = %mySpellDataPath @ "/WW/sounds/ww_wisp_done.ogg";
   description = SpellAudioDefault_AD;
   preload = true;
};

datablock afxAudioBank(WW_WispSpawnRand_SND)
{
  playIndex = "$$ getRandom(0,5)";
  path = %mySpellDataPath @ "/WW/sounds";
  filenames[0] = "ww_wisp_spawn_R1.ogg";
    filenames[1] = "ww_wisp_spawn_R2.ogg";
    filenames[2] = "ww_wisp_spawn_R3.ogg";
    filenames[3] = "ww_wisp_spawn_R4.ogg";
    filenames[4] = "ww_wisp_spawn_R5.ogg";
    filenames[5] = "ww_wisp_spawn_R6.ogg";
  description = SpellAudioCasting_loud_AD;
  preload = true;
};

datablock SFXDescription(WW_WispChirpLoop_AD : SpellAudioDefault_AD)
{
  isLooping          = true;
  ReferenceDistance  = 10.0;
  MaxDistance        = 80.0;
};
datablock SFXProfile(WW_WispChirpLoop_SND)
{
   fileName = %mySpellDataPath @ "/WW/sounds/ww_wisp_chirp.ogg";
   description = WW_WispChirpLoop_AD;
   preload = true;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

// wisp audio
datablock afxEffectWrapperData(WW_WispLoop_SND_00_EW)
{
  effect = WW_WispLoop_SND;
  constraint = "$$ \"#effect.GroundMagic_MooringA_##\"";
  delay = "$$ ##*0.7";
  fadeInTime = 1.0;
  fadeOutTime = 0.5;
  lifetime = "$$ %%._wispLife[##] - 0.5";
};

datablock afxEffectWrapperData(WW_WispChirpLoop_SND_00_EW)
{
  effect = WW_WispChirpLoop_SND;
  constraint = "$$ \"#effect.GroundMagic_MooringA_##\"";
  delay = "$$ ##*0.7 + getRandomF(2.0,5.0)";
  fadeInTime = 1.0;
  fadeOutTime = 0.5;
  lifetime = "$$ %%._wispLife[##] - 0.5";
};


datablock afxXM_FreezeData(WW_WispDone_Freeze_00_XM)
{
  mask = $afxXfmMod::POS;
  delay = "$$ ##*0.7 + %%._wispLife[##] - 0.5";
};
datablock afxEffectWrapperData(WW_WispDone_SND_00_EW)
{
  effect = WW_WispDone_SND;
  constraint = "$$ \"#effect.GroundMagic_MooringA_##\"";
  delay = "$$ ##*0.7 + %%._wispLife[##] - 0.5";
  lifetime = 1.6;
  xfmModifiers[0] = WW_WispDone_Freeze_00_XM;
};

datablock afxEffectWrapperData(WW_WispSpawnRand_SND_00_EW)
{
  effect = WW_WispSpawnRand_SND;
  constraint = "$$ \"#effect.GroundMagic_MooringA_##\"";
  delay = "$$ ##*0.7";
  lifetime = 1;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function WW_add_group_Audio_FX(%group_data)
{
  %group_data.addEffect(WW_WispSpawnRand_SND_00_EW);
  %group_data.addEffect(WW_WispLoop_SND_00_EW);
  %group_data.addEffect(WW_WispChirpLoop_SND_00_EW);
  %group_data.addEffect(WW_WispDone_SND_00_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
