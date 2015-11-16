
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// ASTRAL PASSPORT (lighting)
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

datablock afxXM_LocalOffsetData(AP_TeleportZodeRevealLight_disappear_offset_XM)
{
  localOffset = "0 2 -4";
};
datablock afxXM_SpinData(AP_TeleportZodeRevealLight_disappear_spin1_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 0;
  spinRate = -30;
};
datablock afxXM_SpinData(AP_TeleportZodeRevealLight_disappear_spin2_XM : AP_TeleportZodeRevealLight_disappear_spin1_XM)
{
  spinAngle = 120;
};
datablock afxXM_SpinData(AP_TeleportZodeRevealLight_disappear_spin3_XM : AP_TeleportZodeRevealLight_disappear_spin1_XM)
{
  spinAngle = 240;
};
datablock afxXM_AimData(AP_TeleportZodeRevealLight_disappear_aim_XM)
{
  aimZOnly = false;
};

datablock afxT3DSpotLightData(AP_TeleportZodeRevealLight_disappear_CE)
{
  range = 10;
  color = "2.5 2.5 2.5";
  brightness = 1;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(AP_TeleportZodeRevealLight_disappear_1_EW)
{
  effect = AP_TeleportZodeRevealLight_disappear_CE;
  posConstraint = caster;
  posConstraint2 = "caster.#center";
  lifetime = 0.75;
  fadeInTime = 0.5;
  fadeOutTime = 0.25;
  xfmModifiers[0] = AP_TeleportZodeRevealLight_disappear_spin1_XM;
  xfmModifiers[1] = AP_TeleportZodeRevealLight_disappear_offset_XM;
  xfmModifiers[2] = AP_TeleportZodeRevealLight_disappear_aim_XM;
};
datablock afxEffectWrapperData(AP_TeleportZodeRevealLight_disappear_2_EW : AP_TeleportZodeRevealLight_disappear_1_EW)
{
  xfmModifiers[0] = AP_TeleportZodeRevealLight_disappear_spin2_XM;
};
datablock afxEffectWrapperData(AP_TeleportZodeRevealLight_disappear_3_EW : AP_TeleportZodeRevealLight_disappear_1_EW)
{
  xfmModifiers[0] = AP_TeleportZodeRevealLight_disappear_spin3_XM;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DPointLightData(AP_TeleportZodeLight_disappear_CE)
{
  radius = 9;
  color = "1.0 0.5 1.0";
  brightness = 1;
  castShadows = false;
  localRenderViz = false;
};

// this offset defines the lights distance beneath the terrain
datablock afxXM_LocalOffsetData(AP_TeleportZodeLight_disappear_offset_XM)
{
  localOffset = "0 0 -2";
};

datablock afxEffectWrapperData(AP_TeleportZodeLight_disappear_EW)
{
  effect = AP_TeleportZodeLight_disappear_CE;
  posConstraint = caster;
  lifetime = 3.3;
  delay    = 0.25;  
  fadeInTime  = 0.75;
  fadeOutTime = 0.10;
  xfmModifiers[0] = AP_TeleportZodeLight_disappear_offset_XM;
};
datablock afxEffectWrapperData(AP_TeleportZodeLight_reappear_EW : AP_TeleportZodeLight_disappear_EW)
{
  delay = 0.25; // 0.25+%AP_ReappearTimeOffset;
};


//~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectWrapperData(AP_TeleportZodeRevealLight_reappear_1_EW : AP_TeleportZodeRevealLight_disappear_1_EW)
{
  delay = 0;
};
datablock afxEffectWrapperData(AP_TeleportZodeRevealLight_reappear_2_EW : AP_TeleportZodeRevealLight_reappear_1_EW)
{
  xfmModifiers[0] = AP_TeleportZodeRevealLight_disappear_spin2_XM;
};
datablock afxEffectWrapperData(AP_TeleportZodeRevealLight_reappear_3_EW : AP_TeleportZodeRevealLight_reappear_1_EW)
{
  xfmModifiers[0] = AP_TeleportZodeRevealLight_disappear_spin3_XM;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectWrapperData(AP_TeleportZodeFlashLight1_disappear_1_EW) // #LIGHT EW
{
  effect = AP_TeleportZodeRevealLight_disappear_CE;
  posConstraint = caster;
  posConstraint2 = "caster.#center";
  delay = 1.28;
  lifetime = 0.50;
  fadeInTime = 0.05;
  fadeOutTime = 0.30;
  xfmModifiers[0] = AP_TeleportZodeRevealLight_disappear_spin1_XM;
  xfmModifiers[1] = AP_TeleportZodeRevealLight_disappear_offset_XM;
  xfmModifiers[2] = AP_TeleportZodeRevealLight_disappear_aim_XM;
};
datablock afxEffectWrapperData(AP_TeleportZodeFlashLight1_disappear_2_EW : AP_TeleportZodeFlashLight1_disappear_1_EW) // #LIGHT EW
{
  xfmModifiers[0] = AP_TeleportZodeRevealLight_disappear_spin2_XM;
};
datablock afxEffectWrapperData(AP_TeleportZodeFlashLight1_disappear_3_EW : AP_TeleportZodeFlashLight1_disappear_1_EW) // #LIGHT EW
{
  xfmModifiers[0] = AP_TeleportZodeRevealLight_disappear_spin3_XM;
};

datablock afxEffectWrapperData(AP_TeleportZodeFlashLight2_disappear_1_EW) // #LIGHT EW
{
  effect = AP_TeleportZodeRevealLight_disappear_CE;
  posConstraint = caster;
  posConstraint2 = "caster.#center";
  delay = 2.80;
  lifetime = 0.50;
  fadeInTime = 0.05;
  fadeOutTime = 0.30;
  xfmModifiers[0] = AP_TeleportZodeRevealLight_disappear_spin1_XM;
  xfmModifiers[1] = AP_TeleportZodeRevealLight_disappear_offset_XM;
  xfmModifiers[2] = AP_TeleportZodeRevealLight_disappear_aim_XM;
};
datablock afxEffectWrapperData(AP_TeleportZodeFlashLight2_disappear_2_EW : AP_TeleportZodeFlashLight2_disappear_1_EW) // #LIGHT EW
{
  xfmModifiers[0] = AP_TeleportZodeRevealLight_disappear_spin2_XM;
};
datablock afxEffectWrapperData(AP_TeleportZodeFlashLight2_disappear_3_EW : AP_TeleportZodeFlashLight2_disappear_1_EW) // #LIGHT EW
{
  xfmModifiers[0] = AP_TeleportZodeRevealLight_disappear_spin3_XM;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectWrapperData(AP_TeleportZodeFlashLight1_reappear_1_EW)
{
  effect = AP_TeleportZodeRevealLight_disappear_CE;
  posConstraint = caster;
  posConstraint2 = "caster.#center";
  delay = 0.65;
  lifetime = 0.8;
  fadeInTime = 0.05;
  fadeOutTime = 0.15;
  xfmModifiers[0] = AP_TeleportZodeRevealLight_disappear_spin1_XM;
  xfmModifiers[1] = AP_TeleportZodeRevealLight_disappear_offset_XM;
  xfmModifiers[2] = AP_TeleportZodeRevealLight_disappear_aim_XM;
};
datablock afxEffectWrapperData(AP_TeleportZodeFlashLight1_reappear_2_EW : AP_TeleportZodeFlashLight1_reappear_1_EW)
{
  xfmModifiers[0] = AP_TeleportZodeRevealLight_disappear_spin2_XM;
};
datablock afxEffectWrapperData(AP_TeleportZodeFlashLight1_reappear_3_EW : AP_TeleportZodeFlashLight1_reappear_1_EW)
{
  xfmModifiers[0] = AP_TeleportZodeRevealLight_disappear_spin3_XM;
};

datablock afxEffectWrapperData(AP_TeleportZodeFlashLight2_reappear_1_EW)
{
  effect = AP_TeleportZodeRevealLight_disappear_CE;
  posConstraint = caster;
  posConstraint2 = "caster.#center";
  delay = 2.80;
  lifetime = 0.50;
  fadeInTime = 0.05;
  fadeOutTime = 0.30;
  xfmModifiers[0] = AP_TeleportZodeRevealLight_disappear_spin1_XM;
  xfmModifiers[1] = AP_TeleportZodeRevealLight_disappear_offset_XM;
  xfmModifiers[2] = AP_TeleportZodeRevealLight_disappear_aim_XM;
};
datablock afxEffectWrapperData(AP_TeleportZodeFlashLight2_reappear_2_EW : AP_TeleportZodeFlashLight2_reappear_1_EW)
{
  xfmModifiers[0] = AP_TeleportZodeRevealLight_disappear_spin2_XM;
};
datablock afxEffectWrapperData(AP_TeleportZodeFlashLight2_reappear_3_EW : AP_TeleportZodeFlashLight2_reappear_1_EW)
{
  xfmModifiers[0] = AP_TeleportZodeRevealLight_disappear_spin3_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

// add effects to disappear spell datablock
function AP_disappear_add_Lighting_FX(%spell_data)
{
  %spell_data.addCastingEffect(AP_TeleportZodeRevealLight_disappear_1_EW);
  %spell_data.addCastingEffect(AP_TeleportZodeRevealLight_disappear_2_EW);
  %spell_data.addCastingEffect(AP_TeleportZodeRevealLight_disappear_3_EW);
  %spell_data.addCastingEffect(AP_TeleportZodeLight_disappear_EW);
  %spell_data.addCastingEffect(AP_TeleportZodeFlashLight1_disappear_1_EW);
  %spell_data.addCastingEffect(AP_TeleportZodeFlashLight1_disappear_2_EW);
  %spell_data.addCastingEffect(AP_TeleportZodeFlashLight1_disappear_3_EW);
  %spell_data.addCastingEffect(AP_TeleportZodeFlashLight2_disappear_1_EW);
  %spell_data.addCastingEffect(AP_TeleportZodeFlashLight2_disappear_2_EW);
  %spell_data.addCastingEffect(AP_TeleportZodeFlashLight2_disappear_3_EW);
}

// add effects to reappear spell datablock
function AP_reappear_add_Lighting_FX(%spell_data)
{
  %spell_data.addLingerEffect(AP_TeleportZodeRevealLight_reappear_1_EW);
  %spell_data.addLingerEffect(AP_TeleportZodeRevealLight_reappear_2_EW);
  %spell_data.addLingerEffect(AP_TeleportZodeRevealLight_reappear_3_EW);
  %spell_data.addLingerEffect(AP_TeleportZodeLight_reappear_EW);
  %spell_data.addLingerEffect(AP_TeleportZodeFlashLight1_reappear_1_EW);
  %spell_data.addLingerEffect(AP_TeleportZodeFlashLight1_reappear_2_EW);
  %spell_data.addLingerEffect(AP_TeleportZodeFlashLight1_reappear_3_EW);
  %spell_data.addLingerEffect(AP_TeleportZodeFlashLight2_reappear_1_EW);
  %spell_data.addLingerEffect(AP_TeleportZodeFlashLight2_reappear_2_EW);
  %spell_data.addLingerEffect(AP_TeleportZodeFlashLight2_reappear_3_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
