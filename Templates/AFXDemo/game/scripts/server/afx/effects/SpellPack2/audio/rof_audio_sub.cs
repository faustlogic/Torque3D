
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// RING OF FIRE (audio)
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

datablock SFXProfile(RoF_Ignite_SND)
{
   fileName = %mySpellDataPath @ "/RoF/sounds/rof_ignite.ogg";
   description = SpellAudioDefault_AD;
   preload = true;
};

datablock SFXProfile(RoF_ringExpand_L1_SND)
{
   fileName = %mySpellDataPath @ "/RoF/sounds/rof_ringExpand_L1.ogg";
   description = SpellAudioDefault_AD;
   preload = true;
};
datablock SFXProfile(RoF_ringExpand_L2_SND : RoF_ringExpand_L1_SND)
{
   fileName = %mySpellDataPath @ "/RoF/sounds/rof_ringExpand_L2.ogg";
};
datablock SFXProfile(RoF_ringExpand_L3_SND : RoF_ringExpand_L1_SND)
{
   fileName = %mySpellDataPath @ "/RoF/sounds/rof_ringExpand_L3.ogg";
};
datablock SFXProfile(RoF_ringExpand_L4_SND : RoF_ringExpand_L1_SND)
{
   fileName = %mySpellDataPath @ "/RoF/sounds/rof_ringExpand_L4.ogg";
};
datablock SFXProfile(RoF_ringExpand_L5_SND : RoF_ringExpand_L1_SND)
{
   fileName = %mySpellDataPath @ "/RoF/sounds/rof_ringExpand_L5.ogg";
};
datablock SFXProfile(RoF_ringExpand_L6_SND : RoF_ringExpand_L1_SND)
{
   fileName = %mySpellDataPath @ "/RoF/sounds/rof_ringExpand_L6.ogg";
};

datablock SFXProfile(RoF_fireLoop_L1_SND)
{
   fileName = %mySpellDataPath @ "/RoF/sounds/rof_fireLoop_L1.ogg";
   description = SpellAudioLoop_AD;
   preload = true;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectWrapperData(RoF_fireIgniters_SND_00_EW)
{
  effectEnabled = "$$ ((## % 2) != 1)";
  scaleFactor = "$$ getRandomF(0.5, 1)";
  effect = RoF_Ignite_SND;
  lifetime = 1;
  fadeInTime = 0.4;
  delay = "$$ 0.4 + (0.1*##)";
  constraint = "$$ \"#effect.Swirl_Mooring##\"";
};

// Ignite Audio 
datablock afxEffectWrapperData(RoF_ringExpand_L1_SND_EW)
{
  effect = RoF_ringExpand_L1_SND;
  delay = 0.15;
  lifetime = 5;
  posConstraint = "caster";
};
datablock afxEffectWrapperData(RoF_ringExpand_L2_SND_EW : RoF_ringExpand_L1_SND_EW)
{
  effect = RoF_ringExpand_L2_SND;
  delay = 0.15;
};
datablock afxEffectWrapperData(RoF_ringExpand_L3_SND_EW : RoF_ringExpand_L1_SND_EW)
{
  effect = RoF_ringExpand_L3_SND;
  fadeInTime = 0.5;
  delay = 0.0;
};
datablock afxEffectWrapperData(RoF_ringExpand_L4_SND_EW : RoF_ringExpand_L1_SND_EW)
{
  effect = RoF_ringExpand_L4_SND;
  delay = 0.15;
};
datablock afxEffectWrapperData(RoF_ringExpand_L5_SND_EW : RoF_ringExpand_L1_SND_EW)
{
  effect = RoF_ringExpand_L5_SND;
  delay = 0.15;
};
datablock afxEffectWrapperData(RoF_ringExpand_L6_SND_EW : RoF_ringExpand_L1_SND_EW)
{
  effect = RoF_ringExpand_L6_SND;
  delay = 0.15;
};

datablock afxXM_LocalOffsetData(RoF_fireLoop_Offset_Swirl_XM)
{
  localOffset = "$$ 0 SPC (%%._ringRadius) SPC 0";
  fadeInTime = 0.6;
};
datablock afxEffectWrapperData(RoF_fireLoop_Swirl_SND_EW)
{
  effect = RoF_fireLoop_L1_SND;
  delay = 2.5;
  lifetime = "$$ %%._ringDur - 3.2";
  fadeOutTime = 1.5;
  posConstraint = "RingAnchor";
  posConstraint2 = "listener";
  xfmModifiers[0] = "RoF_Aim_XM";
  xfmModifiers[1] = "RoF_fireLoop_Offset_Swirl_XM";
};

// Ignite Audio 
datablock afxXM_LocalOffsetData(RoF_fireLoop_Spider_Offset_XM)
{
  localOffset = "$$ 0 SPC (%%._ringRadius) SPC 0";
  fadeInTime = 0.6;
};
datablock afxEffectWrapperData(RoF_fireLoop_Spider_SND_EW)
{
  effect = RoF_fireLoop_L1_SND;
  delay = 0.6;
  lifetime = "$$ %%._ringDur - 0.6";
  fadeOutTime = 1.5;
  posConstraint = "RingAnchor";
  posConstraint2 = "listener";
  xfmModifiers[0] = "RoF_Aim_XM";
  xfmModifiers[1] = "RoF_fireLoop_Spider_Offset_XM";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

// Ignite Audio Group (swirl)
datablock afxEffectGroupData(RoF_Ignite_SND_Swirl_EG)
{
  groupEnabled = "$$ %%._swirl == true";
  count = 1;
  addEffect = RoF_ringExpand_L1_SND_EW;
  addEffect = RoF_ringExpand_L2_SND_EW;
  addEffect = RoF_ringExpand_L3_SND_EW;
  addEffect = RoF_ringExpand_L4_SND_EW;
  addEffect = RoF_ringExpand_L5_SND_EW;
  addEffect = RoF_ringExpand_L6_SND_EW;
  addEffect = RoF_fireLoop_Swirl_SND_EW;
};

// Ignite Audio Group (spider)
datablock afxEffectGroupData(RoF_Ignite_SND_Spider_EG)
{
  groupEnabled = "$$ %%._spider == true";
  count = 1;
  addEffect = RoF_fireLoop_Spider_SND_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function RoF_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(RoF_Ignite_SND_Swirl_EG);
  %spell_data.addCastingEffect(RoF_Ignite_SND_Spider_EG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
