
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SPIRIT OF ROACH (lighting)
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

datablock afxXM_LocalOffsetData(SoR_MainZodeRevealLight_offset_XM)
{
  localOffset = "0 2 -4";
};
datablock afxXM_SpinData(SoR_MainZodeRevealLight_spin1_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 0;
  spinRate = -30;
};
datablock afxXM_SpinData(SoR_MainZodeRevealLight_spin2_XM : SoR_MainZodeRevealLight_spin1_XM)
{
  spinAngle = 120;
};
datablock afxXM_SpinData(SoR_MainZodeRevealLight_spin3_XM : SoR_MainZodeRevealLight_spin1_XM)
{
  spinAngle = 240;
};
datablock afxXM_AimData(SoR_MainZodeRevealLight_aim_XM)
{
  aimZOnly = false;
};

// main zode reveal light
datablock afxT3DSpotLightData(SoR_MainZodeRevealLight_CE)
{
  range = 10;
  color = "1.0 1.0 1.0";
  brightness = 2.0;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(SoR_MainZodeRevealLight_1_EW : SoR_ZodeReveal_EW)
{
  effect = SoR_MainZodeRevealLight_CE;
  posConstraint = caster;
  posConstraint2 = "caster.#center";
  xfmModifiers[0] = SoR_MainZodeRevealLight_spin1_XM;
  xfmModifiers[1] = SoR_MainZodeRevealLight_offset_XM;
  xfmModifiers[2] = SoR_MainZodeRevealLight_aim_XM;
};
datablock afxEffectWrapperData(SoR_MainZodeRevealLight_2_EW : SoR_MainZodeRevealLight_1_EW)
{
  xfmModifiers[0] = SoR_MainZodeRevealLight_spin2_XM;
};
datablock afxEffectWrapperData(SoR_MainZodeRevealLight_3_EW : SoR_MainZodeRevealLight_1_EW)
{
  xfmModifiers[0] = SoR_MainZodeRevealLight_spin3_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

// this offset defines the lights distance beneath the terrain  
datablock afxXM_LocalOffsetData(SoR_CastingZodeLight_offset_XM)
{
  localOffset = "0 0 -2";
};

datablock afxT3DPointLightData(SoR_CastingZodeLight_CE)
{
  radius = 4.5;
  color = "0.067 0.698 0.773";
  brightness = 8;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(SoR_CastingZodeLight_EW : SoR_Zode1_EW)
{
  effect = SoR_CastingZodeLight_CE;
  xfmModifiers[0] = SoR_CastingZodeLight_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

// this offset defines the lights distance beneath the terrain
datablock afxXM_LocalOffsetData(SoR_FalloutLight_offset_XM)
{
  localOffset = "0 0 -2";
};

datablock afxT3DPointLightData(SoR_FalloutLight_CE)
{
  radius = 5;
  color = "1.0 1.0 1.0 ";
  brightness = 8;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(SoR_FalloutLight_EW)
{
  effect = SoR_FalloutLight_CE;
  posConstraint = caster;
  delay = 1.25;
  fadeInTime = 0.50;
  fadeOutTime = 0.5;
  lifetime = 0.50;
  xfmModifiers[0] = SoR_FalloutLight_offset_XM;  
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

// this represents light cast from the halo
datablock afxT3DPointLightData(SoR_RoachLight_CE)
{
  radius = 5;
  color = "0.48 0.085 0.5";
  brightness = 1;
  castShadows = false;
  localRenderViz = false;
};

datablock afxXM_WorldOffsetData(SoR_RoachLight_offset_XM)
{
  worldOffset = "0.0 0.0 3.0";
};
datablock afxEffectWrapperData(SoR_RoachLight_EW)
{
  effect = SoR_RoachLight_CE;
  posConstraint = "impactedObject";  
  delay = 0.5;
  fadeInTime  = 0.5;
  fadeOutTime = 0.5;
  lifetime = 2.5;
  xfmModifiers[0] = SoR_RoachLight_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

// this represents lighting from the target zodiac reveal
datablock afxT3DPointLightData(SoR_RevealLight_CE)
{
  radius = 3;
  color = "1.0 1.0 1.0";
  brightness = 4;
  castShadows = false;
  localRenderViz = false;
};

datablock afxXM_WorldOffsetData(SoR_RevealLight_offset_XM)
{
  worldOffset = "0.0 0.0 1.0";
};
datablock afxEffectWrapperData(SoR_RevealLight_EW)
{
  effect = SoR_RevealLight_CE;
  posConstraint = "impactedObject";  
  delay = 0.0;
  fadeInTime  = 0.4;
  fadeOutTime = 0.75;
  lifetime = 0.75;
  xfmModifiers[0] = SoR_RevealLight_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//


// this offset defines the lights distance beneath the terrain
datablock afxXM_LocalOffsetData(SoR_TargetZodeLight_offset_XM)
{
  localOffset = "0 0 -1";
};

datablock afxT3DPointLightData(SoR_TargetZodeLight_CE)
{
  radius = 3.5;
  color = "0.067 0.698 0.773";
  brightness = 12;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(SoR_TargetZodeLight_EW : SoR_TargetZode_EW)
{
  effect = SoR_TargetZodeLight_CE;
  xfmModifiers[0] = SoR_TargetZodeLight_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function SoR_add_Lighting_FX(%spell_data)
{
  %spell_data.addLingerEffect(SoR_MainZodeRevealLight_1_EW);
  %spell_data.addLingerEffect(SoR_MainZodeRevealLight_2_EW);
  %spell_data.addLingerEffect(SoR_MainZodeRevealLight_3_EW);

  %spell_data.addLingerEffect(SoR_CastingZodeLight_EW);
  %spell_data.addLingerEffect(SoR_FalloutLight_EW);

  %spell_data.addImpactEffect(SoR_RoachLight_EW);
  %spell_data.addImpactEffect(SoR_RevealLight_EW);
  %spell_data.addImpactEffect(SoR_TargetZodeLight_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
