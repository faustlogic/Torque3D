
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SHARDS OF VESUVIUS (audio)
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

datablock SFXProfile(SoV_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SoV/sounds/sov_explosion_L1.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock SFXProfile(SoV_Explosion_L2_SND : SoV_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SoV/sounds/sov_explosion_L2.ogg";
};
datablock SFXProfile(SoV_Explosion_L3_SND : SoV_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SoV/sounds/sov_explosion_L3.ogg";
};
datablock SFXProfile(SoV_Explosion_L4_SND : SoV_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SoV/sounds/sov_explosion_L4.ogg";
};
datablock SFXProfile(SoV_Explosion_L5_SND : SoV_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SoV/sounds/sov_explosion_L5.ogg";
};

datablock SFXProfile(SoV_Rumble_L1_SND : SoV_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SoV/sounds/sov_rumble_L1.ogg";
};
datablock SFXProfile(SoV_Rumble_L2_SND : SoV_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SoV/sounds/sov_rumble_L2.ogg";
};
datablock SFXProfile(SoV_Rumble_L3_SND : SoV_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SoV/sounds/sov_rumble_L3.ogg";
};
datablock SFXProfile(SoV_Rumble_L4_SND : SoV_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SoV/sounds/sov_rumble_L4.ogg";
};
datablock SFXProfile(SoV_Rumble_L5_SND : SoV_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SoV/sounds/sov_rumble_L5.ogg";
};

datablock SFXProfile(SoV_FlareUp_L1_SND : SoV_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SoV/sounds/sov_flareUp_L1.ogg";
};

datablock SFXProfile(SoV_FlareUp_L2_SND : SoV_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SoV/sounds/sov_flareUp_L2.ogg";
};

datablock SFXProfile(SoV_CometLaunch_SND : SoV_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SoV/sounds/sov_cometLaunch.ogg";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectWrapperData(SoV_Explosion_L1_SND_EW)
{
  effect = SoV_Explosion_L1_SND;
  delay = 0.0;
  lifetime = 10;
  posConstraint = "freeTarget";
};

datablock afxEffectWrapperData(SoV_Explosion_L2_SND_EW : SoV_Explosion_L1_SND_EW)
{
  effect = SoV_Explosion_L2_SND;
};

datablock afxEffectWrapperData(SoV_Explosion_L3_SND_EW : SoV_Explosion_L1_SND_EW)
{
  effect = SoV_Explosion_L3_SND;
};

datablock afxEffectWrapperData(SoV_Explosion_L4_SND_EW : SoV_Explosion_L1_SND_EW)
{
  effect = SoV_Explosion_L4_SND;
};

datablock afxEffectWrapperData(SoV_Explosion_L5_SND_EW : SoV_Explosion_L1_SND_EW)
{
  effect = SoV_Explosion_L5_SND;
};

datablock afxEffectWrapperData(SoV_Rumble_L1_SND_EW : SoV_Explosion_L1_SND_EW)
{
  effect = SoV_Rumble_L1_SND;
};

datablock afxEffectWrapperData(SoV_Rumble_L2_SND_EW : SoV_Explosion_L1_SND_EW)
{
  effect = SoV_Rumble_L2_SND;
};

datablock afxEffectWrapperData(SoV_Rumble_L3_SND_EW : SoV_Explosion_L1_SND_EW)
{
  effect = SoV_Rumble_L3_SND;
};

datablock afxEffectWrapperData(SoV_Rumble_L4_SND_EW : SoV_Explosion_L1_SND_EW)
{
  effect = SoV_Rumble_L4_SND;
};

datablock afxEffectWrapperData(SoV_Rumble_L5_SND_EW : SoV_Explosion_L1_SND_EW)
{
  effect = SoV_Rumble_L5_SND;
};

datablock afxEffectWrapperData(SoV_FlareUp_L1_SND_EW : SoV_Explosion_L1_SND_EW)
{
  effect = SoV_FlareUp_L1_SND;
};

datablock afxEffectWrapperData(SoV_FlareUp_L2_SND_EW : SoV_Explosion_L1_SND_EW)
{
  effect = SoV_FlareUp_L2_SND;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function SoV_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(SoV_Explosion_L1_SND_EW);
  %spell_data.addCastingEffect(SoV_Explosion_L2_SND_EW);
  //%spell_data.addCastingEffect(SoV_Explosion_L3_SND_EW);
  %spell_data.addCastingEffect(SoV_Explosion_L4_SND_EW);
  %spell_data.addCastingEffect(SoV_Explosion_L5_SND_EW);
  %spell_data.addCastingEffect(SoV_Rumble_L1_SND_EW);
  %spell_data.addCastingEffect(SoV_Rumble_L2_SND_EW);
  %spell_data.addCastingEffect(SoV_Rumble_L3_SND_EW);
  %spell_data.addCastingEffect(SoV_Rumble_L4_SND_EW);
  //%spell_data.addCastingEffect(SoV_Rumble_L5_SND_EW);
  //%spell_data.addCastingEffect(SoV_FlareUp_L1_SND_EW);
  //%spell_data.addCastingEffect(SoV_FlareUp_L2_SND_EW);
 }

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

