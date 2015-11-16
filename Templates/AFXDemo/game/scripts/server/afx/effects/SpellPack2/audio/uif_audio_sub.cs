
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// UP IN FLAMES (audio)
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

datablock SFXProfile(UIF_FireLoop_L1_SND)
{
  fileName = %mySpellDataPath @ "/UIF/sounds/uif_fireLoop_L1.ogg";
  description = SpellAudioLoop_AD;
  preload = true;
};
datablock SFXProfile(UIF_FireLoop_L2_SND : UIF_FireLoop_L1_SND)
{
  fileName = %mySpellDataPath @ "/UIF/sounds/uif_fireLoop_L2.ogg";
};
datablock SFXProfile(UIF_FireIgnite_L1_SND)
{
  fileName = %mySpellDataPath @ "/UIF/sounds/uif_ignite_L1.ogg";
  description = SpellAudioCasting_AD;
  preload = true;
};
datablock SFXProfile(UIF_FireIgnite_L2_SND : UIF_FireIgnite_L1_SND)
{
  fileName = %mySpellDataPath @ "/UIF/sounds/uif_ignite_L2.ogg";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectWrapperData(UIF_FireLoop_LF_SND_EW)
{
  effect = UIF_FireLoop_L1_SND;
  delay = 0.0;
  lifetime = "$$ %%._dur - 1.0";
  fadeOutTime = 1.5;
  posConstraint = "victim.Bip01 L Hand";
};

datablock afxEffectWrapperData(UIF_FireLoop_RT_SND_EW : UIF_FireLoop_LF_SND_EW)
{
  effect = UIF_FireLoop_L2_SND;
  fadeOutTime = 1.0;
  posConstraint = "victim.Bip01 R Hand";
};

datablock afxEffectWrapperData(UIF_FireIgnite_LF_SND_EW)
{
  effect = UIF_FireIgnite_L1_SND;
  delay = 0.0;
  lifetime = 0.5;
  posConstraint = "victim.Bip01 L Hand";
};

datablock afxEffectWrapperData(UIF_FireIgnite_RT_SND_EW)
{
  effect = UIF_FireIgnite_L2_SND;
  delay = 0.25;
  lifetime = 0.5;
  posConstraint = "victim.Bip01 R Hand";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function UIF_add_Audio_FX(%effect_data)
{
  %effect_data.addEffect(UIF_FireLoop_LF_SND_EW);
  %effect_data.addEffect(UIF_FireLoop_RT_SND_EW);
  %effect_data.addEffect(UIF_FireIgnite_LF_SND_EW);
  %effect_data.addEffect(UIF_FireIgnite_RT_SND_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
