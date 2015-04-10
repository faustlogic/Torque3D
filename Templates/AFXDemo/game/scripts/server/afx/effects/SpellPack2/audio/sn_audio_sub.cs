
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SOUL NUKE (audio)
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

datablock SFXProfile(SN_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SN/sounds/sn_explosion_L1.ogg";
   description = SpellAudioDefault_AD;
   preload = true;
};
datablock SFXProfile(SN_Explosion_L2_SND : SN_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SN/sounds/sn_explosion_L2.ogg";
};
datablock SFXProfile(SN_LaserWave_L1_SND : SN_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SN/sounds/sn_laser_wave_L1.ogg";
};
datablock SFXProfile(SN_LaserWave_L2_SND : SN_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SN/sounds/sn_laser_wave_L2.ogg";
};
datablock SFXProfile(SN_LowRumble_SND : SN_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SN/sounds/sn_low_rumble.ogg";
};
datablock SFXProfile(SN_Rumble_SND : SN_Explosion_L1_SND)
{
   fileName = %mySpellDataPath @ "/SN/sounds/sn_rumble.ogg";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectWrapperData(SN_Explosion_L1_SND_EW)
{
  effect = SN_Explosion_L1_SND;
  posConstraint = "mine";
  delay = 0;
  lifetime = 2.5;
};
datablock afxEffectWrapperData(SN_Explosion_L2_SND_EW : SN_Explosion_L1_SND_EW)
{
  effect = SN_Explosion_L2_SND;
  lifetime = 1.5;
};
datablock afxEffectWrapperData(SN_LowRumble_SND_EW : SN_Explosion_L1_SND_EW)
{
  effect = SN_LowRumble_SND;
  lifetime = 2.0;
};
datablock afxEffectWrapperData(SN_Rumble_SND_EW : SN_Explosion_L1_SND_EW)
{
  effect = SN_Rumble_SND;
  lifetime = 3.0;
};
datablock afxEffectWrapperData(SN_LaserWave_L1_SND_EW : SN_Explosion_L1_SND_EW)
{
  effect = SN_LaserWave_L1_SND;
  lifetime = 1.5;
};
datablock afxEffectWrapperData(SN_LaserWave_L2_SND_EW : SN_Explosion_L1_SND_EW)
{
  effect = SN_LaserWave_L2_SND;
  lifetime = 2.2;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function SN_add_Audio_FX(%effect_data)
{
  %effect_data.addEffect(SN_Explosion_L1_SND_EW);
  %effect_data.addEffect(SN_Explosion_L2_SND_EW);
  %effect_data.addEffect(SN_LowRumble_SND_EW);
  %effect_data.addEffect(SN_Rumble_SND_EW);
  %effect_data.addEffect(SN_LaserWave_L1_SND_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
