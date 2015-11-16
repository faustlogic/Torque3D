
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// LIGHT MY FIRE (lighting)
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
// CASTING HAND FIRE LIGHTS

//
// Lights constrained to the caster's hands flash on and off in 
// conjunction with the fire hand particles above.  Multiple lights
// are used, fading in and out using the fade parameters of 
// afxEffectWrapperData, to create this effect.
//

datablock afxXM_LocalOffsetData(LMF_CastingFireLight_offset1_XM)
{
  localOffset = "0.1 -0.3 0.0";
};
datablock afxXM_LocalOffsetData(LMF_CastingFireLight_offset2_XM)
{
  localOffset = "-0.2 0.1 0.2";
};
datablock afxXM_LocalOffsetData(LMF_CastingFireLight_offset3_XM)
{
  localOffset = "0.2 -0.1 -0.3";
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DPointLightData(LMF_FireShadowLight_A_CE)
{
  radius = 3;
  color = "1.0 0.9 0.3";
  brightness = 3;
  castShadows = false; // T3D Off (temporarily)
  localRenderViz = false;
};

datablock afxT3DPointLightData(LMF_FireShadowLight_B_CE)
{
  radius = 2;
  color = "1.0 0.5 0.1";
  brightness = 3;
  castShadows = false; // T3D Off (temporarily)
  localRenderViz = false;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectWrapperData(LMF_CastingFireLight_lf_hand_1_EW)
{
  effect = LMF_FireShadowLight_A_CE;
  constraint = "caster.Bip01 L Hand";
  lifetime = 0.2;
  delay = 0.1;
  fadeInTime = 0.05;
  fadeOutTime = 0.05;
};
datablock afxEffectWrapperData(LMF_CastingFireLight_lf_hand_2_EW : LMF_CastingFireLight_lf_hand_1_EW)
{
  effect = LMF_FireShadowLight_B_CE;
  delay = 0.15;
  xfmModifiers[0] = "LMF_CastingFireLight_offset2_XM";
};
datablock afxEffectWrapperData(LMF_CastingFireLight_lf_hand_3_EW : LMF_CastingFireLight_lf_hand_1_EW)
{
  delay = 0.35;
  xfmModifiers[0] = "LMF_CastingFireLight_offset1_XM";
};

datablock afxEffectWrapperData(LMF_CastingFireLight_rt_hand_1_EW)
{
  effect = LMF_FireShadowLight_B_CE;
  constraint = "caster.Bip01 R Hand";
  lifetime = 0.2;
  delay = 0.15;
  fadeInTime = 0.05;
  fadeOutTime = 0.05;
  xfmModifiers[0] = "LMF_CastingFireLight_offset1_XM";
};
datablock afxEffectWrapperData(LMF_CastingFireLight_rt_hand_2_EW : LMF_CastingFireLight_rt_hand_1_EW)
{
  effect = LMF_FireShadowLight_A_CE;
  delay = 0.25;
};
datablock afxEffectWrapperData(LMF_CastingFireLight_rt_hand_3_EW : LMF_CastingFireLight_rt_hand_1_EW)
{
  delay = 0.4;
  xfmModifiers[0] = "LMF_CastingFireLight_offset2_XM";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CAMPFIRE LIGHTS

//
// For each of the ten fire emitters defined above, there is a
// corresponding light.  Their timing and placement with transform
// modifiers is identical to the emitters.  In addition, new paths
// are used to jiggle the lights up and down which randomizes the
// size of the circle of light cast on the ground for a nice effect.
//

// three campfire lights of varying sizes and colors

datablock afxT3DPointLightData(LFM_FireLight_A_CE)
{
  radius = 4.8;
  color = "0.5 0.45 0.15";
  brightness = 0.5;
  castShadows = false;
  localRenderViz = false;
};
datablock afxT3DPointLightData(LFM_FireLight_B_CE : LFM_FireLight_A_CE)
{
  radius = 7.2;
  color = "0.5 0.3 0.05";
};
datablock afxT3DPointLightData(LFM_FireLight_C_CE : LFM_FireLight_A_CE)
{
  radius = 2.8;
  color = "0.5 0.45 0.4";
};

// local offset to place lights elevated in the campfires center
datablock afxXM_LocalOffsetData(LMF_FireLight_Offset_Center_XM)
{
  localOffset = "0 0.6 2.0";
};

// paths to jiggle the lights up and down
datablock afxPathData(LMF_FireLight_PathA)
{
  points = "0 0  1.0 " @
           "0 0 -0.3 " @
           "0 0  0.2 " @
           "0 0 -0.2 " @
           "0 0  1.0";
  loop = cycle;
  lifetime = 0.5;
};
datablock afxPathData(LMF_FireLight_PathB)
{
  points = "0 0  0.1 " @
           "0 0  0.7 " @
           "0 0 -0.5 " @
           "0 0  0.1";
  loop = cycle;
  lifetime = 0.4;
};
datablock afxPathData(LMF_FireLight_PathC)
{
  points = "0 0 -0.5 " @
           "0 0  0.3 " @
           "0 0  0.0 " @
           "0 0  0.1 " @
           "0 0 -0.5";
  loop = cycle;
  lifetime = 0.6;
};
datablock afxXM_PathConformData(LMF_FireLight_pathA_XM)
{
  paths = "LMF_FireLight_PathA";
};
datablock afxXM_PathConformData(LMF_FireLight_pathB_XM)
{
  paths = "LMF_FireLight_PathB";
};
datablock afxXM_PathConformData(LMF_FireLight_pathC_XM)
{
  paths = "LMF_FireLight_PathC";
};

// campfire light 1
datablock afxEffectWrapperData(LMF_fireLight1_EW)
{
  effect = LFM_FireLight_A_CE;
  posConstraint = "#scene.CampFire";
  lifetime = 5.0;
  delay    = %LMF_fireDelay_1;
  fadeInTime  = 1.5;
  fadeOutTime = 1.5;
  xfmModifiers[0] = "LMF_FireLight_Offset_Center_XM";
  xfmModifiers[1] = "LMF_Fire_Spin1_XM";
  xfmModifiers[2] = "LMF_Fire_pathA_XM";
  xfmModifiers[3] = "LMF_FireLight_pathA_XM";
};
// campfire light 2
datablock afxEffectWrapperData(LMF_fireLight2_EW : LMF_fireLight1_EW)
{
  effect = LFM_FireLight_B_CE;
  delay    = %LMF_fireDelay_2;
  xfmModifiers[0] = "LMF_FireLight_Offset_Center_XM";
  xfmModifiers[1] = "LMF_Fire_Spin2_XM";
  xfmModifiers[2] = "LMF_Fire_pathB_XM";
  xfmModifiers[3] = "LMF_FireLight_pathB_XM";
};
// campfire light 3
datablock afxEffectWrapperData(LMF_fireLight3_EW : LMF_fireLight1_EW)
{
  effect = LFM_FireLight_C_CE;
  delay    = %LMF_fireDelay_3;
  xfmModifiers[0] = "LMF_FireLight_Offset_Center_XM";
  xfmModifiers[1] = "LMF_Fire_Spin3_XM";
  xfmModifiers[2] = "LMF_Fire_pathC_XM";
  xfmModifiers[3] = "LMF_FireLight_pathC_XM";
};
// campfire light 4
datablock afxEffectWrapperData(LMF_fireLight4_EW : LMF_fireLight1_EW)
{
  effect = LFM_FireLight_B_CE;
  delay    = %LMF_fireDelay_4;
  xfmModifiers[0] = "LMF_FireLight_Offset_Center_XM";
  xfmModifiers[1] = "LMF_Fire_Spin4_XM";
  xfmModifiers[2] = "LMF_Fire_pathD_XM";
  xfmModifiers[3] = "LMF_FireLight_pathA_XM";
};
// campfire light 5
datablock afxEffectWrapperData(LMF_fireLight5_EW : LMF_fireLight1_EW)
{
  effect = LFM_FireLight_A_CE;
  delay    = %LMF_fireDelay_5;
  xfmModifiers[0] = "LMF_FireLight_Offset_Center_XM";
  xfmModifiers[1] = "LMF_Fire_Spin5_XM";
  xfmModifiers[2] = "LMF_Fire_pathB_XM";
  xfmModifiers[3] = "LMF_FireLight_pathB_XM";
};
// campfire light 6
datablock afxEffectWrapperData(LMF_fireLight6_EW : LMF_fireLight1_EW)
{
  effect = LFM_FireLight_B_CE;
  delay    = %LMF_fireDelay_6;
  xfmModifiers[0] = "LMF_FireLight_Offset_Center_XM";
  xfmModifiers[1] = "LMF_Fire_Spin6_XM";
  xfmModifiers[2] = "LMF_Fire_pathA_XM";
  xfmModifiers[3] = "LMF_FireLight_pathB_XM";
};
// campfire light 7
datablock afxEffectWrapperData(LMF_fireLight7_EW : LMF_fireLight1_EW)
{
  effect = LFM_FireLight_A_CE;
  delay    = %LMF_fireDelay_7;
  xfmModifiers[0] = "LMF_FireLight_Offset_Center_XM";
  xfmModifiers[1] = "LMF_Fire_Spin7_XM";
  xfmModifiers[2] = "LMF_Fire_pathC_XM";
  xfmModifiers[3] = "LMF_FireLight_pathA_XM";
};
// campfire light 8
datablock afxEffectWrapperData(LMF_fireLight8_EW : LMF_fireLight1_EW)
{
  effect = LFM_FireLight_C_CE;
  delay    = %LMF_fireDelay_8;
  xfmModifiers[0] = "LMF_FireLight_Offset_Center_XM";
  xfmModifiers[1] = "LMF_Fire_Spin8_XM";
  xfmModifiers[2] = "LMF_Fire_pathA_XM";
  xfmModifiers[3] = "LMF_FireLight_pathC_XM";
};
// campfire light 9
datablock afxEffectWrapperData(LMF_fireLight9_EW : LMF_fireLight1_EW)
{
  effect = LFM_FireLight_A_CE;
  delay    = %LMF_fireDelay_9;
  xfmModifiers[0] = "LMF_FireLight_Offset_Center_XM";
  xfmModifiers[1] = "LMF_Fire_Spin9_XM";
  xfmModifiers[2] = "LMF_Fire_pathB_XM";
  xfmModifiers[3] = "LMF_FireLight_pathA_XM";
};
// campfire light 10
datablock afxEffectWrapperData(LMF_fireLight10_EW : LMF_fireLight1_EW)
{
  effect = LFM_FireLight_B_CE;
  delay    = %LMF_fireDelay_10;
  xfmModifiers[0] = "LMF_FireLight_Offset_Center_XM";
  xfmModifiers[1] = "LMF_Fire_Spin10_XM";
  xfmModifiers[2] = "LMF_Fire_pathD_XM";
  xfmModifiers[3] = "LMF_FireLight_pathB_XM";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

datablock afxPathData(LMF_FireShadowLight_1_Path)
{
  points = " 0    0    0"   SPC
     " 0.1 -0.3  0.8" SPC
     "-0.3  0.2 -0.6" SPC
     " 0.0 -0.6  0.4" SPC
     " -0.7 0.4 -0.8" SPC
     " 0    0    0";
  lifetime = 0.25*2.0;
  loop = cycle;
  mult = 0.30*0.25;
};
//
datablock afxXM_PathConformData(LMF_FireShadowLight_1_path_XM)
{
  paths = "LMF_FireShadowLight_1_Path";
};

datablock afxPathData(LMF_FireShadowLight_2_Path)
{
  points = " 0    0    0"   SPC
     " 0.4  0.7 -0.3" SPC
     "-0.3  0.0  0.4" SPC
     " 0.2  0.4 -0.8" SPC
     "-0.4 -0.8  0.5" SPC
     " 0    0    0";
  lifetime = 0.20*2.0;
  loop = cycle;
  mult = 0.25*0.25;
};
//
datablock afxXM_PathConformData(LMF_FireShadowLight_2_path_XM)
{
  paths = "LMF_FireShadowLight_2_Path";
};

//~~~~~~~~~~~~~~~~~~~~//

datablock LightAnimData(LMF_FireShadowLight_ANI)
{
  animEnabled = true;
  //minBrightness = 1.2;
  //maxBrightness = 2;
  minBrightness = 0.5;
  maxBrightness = 1.0;
};

datablock afxT3DPointLightData(LMF_FireShadowLight_CE)
{
  radius = 8;
  color = "1.0 0.7 0.2"; 
  brightness = 1;
  animate = true;
  animationType = LMF_FireShadowLight_ANI;
  animationPeriod = 0.25;
  // RadiusTime = 0.35; // if radius can be animated, this would be the period
  animationPhase = 0.0;
  castShadows = false;  // T3D Off (temporarily)
  localRenderViz = false;
};

datablock afxT3DPointLightData(LMF_FireShadowLight2_CE : LMF_FireShadowLight_CE)
{
  animationPeriod = 0.35;
  // RadiusTime = 0.5; // if radius can be animated, this would be the period
};

datablock afxEffectWrapperData(LMF_FireShadowLight_1_EW)
{
  effect = LMF_FireShadowLight_CE;
  posConstraint = "#scene.CampFire";
  lifetime = 9.0;
  delay    = %LMF_fireDelay_1;
  fadeInTime  = 0.5;
  fadeOutTime = 0.5;
  xfmModifiers[0] = "LMF_FireLight_Offset_Center_XM";
  xfmModifiers[1] = "LMF_FireShadowLight_1_path_XM";
};

datablock afxEffectWrapperData(LMF_FireShadowLight_2_EW : LMF_FireShadowLight_1_EW)
{
  effect = LMF_FireShadowLight2_CE;
  xfmModifiers[1] = "LMF_FireShadowLight_2_path_XM";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CAMPFIRE BURST LIGHTS

//
// Coordinated with the fire-bursts above are these lights that flash
// on and off quickly.
// 

datablock afxT3DPointLightData(LFM_FireBurstLight_CE)
{
  radius = 6;
  color = "0.5 0.5 0.3"; 
  brightness = 3.0;
  castShadows = false;
  localRenderViz = false;
};

// fire burst light 1
datablock afxEffectWrapperData(LMF_fireBurstLight1_EW : LMF_fireBurst1_EW)
{
  effect = LFM_FireBurstLight_CE;
  fadeInTime  = 0.1;
  fadeOutTime = 0.1;
};
// fire burst light 2
datablock afxEffectWrapperData(LMF_fireBurstLight2_EW : LMF_fireBurst2_EW)
{
  effect = LFM_FireBurstLight_CE;
  fadeInTime  = 0.1;
  fadeOutTime = 0.1;
};
// fire burst light 3
datablock afxEffectWrapperData(LMF_fireBurstLight3_EW : LMF_fireBurst3_EW)
{
  effect = LFM_FireBurstLight_CE;
  fadeInTime  = 0.1;
  fadeOutTime = 0.1;
};
// fire burst light 4
datablock afxEffectWrapperData(LMF_fireBurstLight4_EW : LMF_fireBurst4_EW)
{
  effect = LFM_FireBurstLight_CE;
  fadeInTime  = 0.1;
  fadeOutTime = 0.1;
};
// fire burst light 5
datablock afxEffectWrapperData(LMF_fireBurstLight5_EW : LMF_fireBurst5_EW)
{
  effect = LFM_FireBurstLight_CE;
  fadeInTime  = 0.1;
  fadeOutTime = 0.1;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

// add effects to main spell datablock
function LMF_add_Lighting_FX(%spell_data)
{
  %spell_data.addCastingEffect(LMF_CastingFireLight_lf_hand_1_EW);
  %spell_data.addCastingEffect(LMF_CastingFireLight_lf_hand_2_EW);
  %spell_data.addCastingEffect(LMF_CastingFireLight_lf_hand_3_EW);
  %spell_data.addCastingEffect(LMF_CastingFireLight_rt_hand_1_EW);
  %spell_data.addCastingEffect(LMF_CastingFireLight_rt_hand_2_EW);
  %spell_data.addCastingEffect(LMF_CastingFireLight_rt_hand_3_EW);

  //%spell_data.addCastingEffect(LMF_LightBeam_EW);

  %spell_data.addLingerEffect(LMF_fireLight1_EW);
  %spell_data.addLingerEffect(LMF_fireLight2_EW);
  %spell_data.addLingerEffect(LMF_fireLight3_EW);
  %spell_data.addLingerEffect(LMF_fireLight4_EW);
  %spell_data.addLingerEffect(LMF_fireLight5_EW);
  %spell_data.addLingerEffect(LMF_fireLight6_EW);
  %spell_data.addLingerEffect(LMF_fireLight7_EW);
  %spell_data.addLingerEffect(LMF_fireLight8_EW);
  %spell_data.addLingerEffect(LMF_fireLight9_EW);
  %spell_data.addLingerEffect(LMF_fireLight10_EW);

  %spell_data.addLingerEffect(LMF_FireShadowLight_1_EW);
  %spell_data.addLingerEffect(LMF_FireShadowLight_2_EW);

  %spell_data.addLingerEffect(LMF_fireBurstLight1_EW);
  %spell_data.addLingerEffect(LMF_fireBurstLight2_EW);
  %spell_data.addLingerEffect(LMF_fireBurstLight3_EW);
  %spell_data.addLingerEffect(LMF_fireBurstLight4_EW);
  %spell_data.addLingerEffect(LMF_fireBurstLight5_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
