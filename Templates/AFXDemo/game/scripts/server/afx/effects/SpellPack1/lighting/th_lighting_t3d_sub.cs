
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// THOR'S HAMMER (lighting)
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

datablock afxXM_LocalOffsetData(TH_MainZodeRevealLight_offset_XM)
{
  localOffset = "0 2 -4";
};
datablock afxXM_SpinData(TH_MainZodeRevealLight_spin1_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 0;
  spinRate = -30;
};
datablock afxXM_SpinData(TH_MainZodeRevealLight_spin2_XM : TH_MainZodeRevealLight_spin1_XM)
{
  spinAngle = 120;
};
datablock afxXM_SpinData(TH_MainZodeRevealLight_spin3_XM : TH_MainZodeRevealLight_spin1_XM)
{
  spinAngle = 240;
};
datablock afxXM_AimData(TH_MainZodeRevealLight_aim_XM)
{
  aimZOnly = false;
};

datablock afxT3DSpotLightData(TH_MainZodeRevealLight_CE)
{
  range = 7;
  color = "1.0 1.0 1.0";
  brightness = 2.0;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(TH_MainZodeRevealLight_1_EW)
{
  effect = TH_MainZodeRevealLight_CE;
  posConstraint = caster;
  posConstraint2 = "caster.#center";
  delay = 0.0;
  lifetime = 0.75;
  fadeInTime = 0.5;
  fadeOutTime = 0.25;
  xfmModifiers[0] = TH_MainZodeRevealLight_spin1_XM;
    xfmModifiers[1] = TH_MainZodeRevealLight_offset_XM;
    xfmModifiers[2] = TH_MainZodeRevealLight_aim_XM;
};
datablock afxEffectWrapperData(TH_MainZodeRevealLight_2_EW : TH_MainZodeRevealLight_1_EW)
{
  xfmModifiers[0] = TH_MainZodeRevealLight_spin2_XM;
};
datablock afxEffectWrapperData(TH_MainZodeRevealLight_3_EW : TH_MainZodeRevealLight_1_EW)
{
  xfmModifiers[0] = TH_MainZodeRevealLight_spin3_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

// this offset defines the lights distance beneath the terrain
datablock afxXM_LocalOffsetData(TH_CastingZodeLight_offset_XM)
{
  localOffset = "0 0 -2";
};

datablock afxT3DPointLightData(TH_CastingZodeLight_CE)
{
  radius = 5.0;
  color = "0.620 0.402 1.0";
  brightness = 8;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(TH_CastingZodeLight_EW : TH_ZodeEffect1_EW)
{
  effect = TH_CastingZodeLight_CE;
  xfmModifiers[0] = TH_CastingZodeLight_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DPointLightData(TH_ZodeLightningFlashLight_CE)
{
  radius = 5.5;
  color = "0.0 0.53 1.0";
  brightness = 7.5;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(TH_ZodeLightningFlash1_EW : TH_ZodeLightningEffect1_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.15;
  fadeOutTime = 0.15;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash2_EW : TH_ZodeLightningEffect2_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.05;
  fadeOutTime = 0.05;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash3_EW : TH_ZodeLightningEffect3_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.10;
  fadeOutTime = 0.10;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash4_EW : TH_ZodeLightningEffect4_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.10;
  fadeOutTime = 0.10;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash5_EW : TH_ZodeLightningEffect5_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.15;
  fadeOutTime = 0.15;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash6_EW : TH_ZodeLightningEffect6_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.05;
  fadeOutTime = 0.05;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash7_EW : TH_ZodeLightningEffect7_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.05;
  fadeOutTime = 0.05;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash8_EW : TH_ZodeLightningEffect8_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.10;
  fadeOutTime = 0.10;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash9_EW : TH_ZodeLightningEffect9_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.15;
  fadeOutTime = 0.15;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash10_EW : TH_ZodeLightningEffect10_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.05;
  fadeOutTime = 0.05;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash11_EW : TH_ZodeLightningEffect11_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.10;
  fadeOutTime = 0.10;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash12_EW : TH_ZodeLightningEffect12_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.15;
  fadeOutTime = 0.15;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash13_EW : TH_ZodeLightningEffect13_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.15;
  fadeOutTime = 0.15;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash14_EW : TH_ZodeLightningEffect14_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.05;
  fadeOutTime = 0.05;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash15_EW : TH_ZodeLightningEffect15_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.10;
  fadeOutTime = 0.10;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash16_EW : TH_ZodeLightningEffect16_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.15;
  fadeOutTime = 0.15;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash17_EW : TH_ZodeLightningEffect17_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.05;
  fadeOutTime = 0.05;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash18_EW : TH_ZodeLightningEffect18_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.15;
  fadeOutTime = 0.15;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash19_EW : TH_ZodeLightningEffect19_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.10;
  fadeOutTime = 0.10;
};
datablock afxEffectWrapperData(TH_ZodeLightningFlash20_EW : TH_ZodeLightningEffect20_EW)
{
  effect = TH_ZodeLightningFlashLight_CE;
  fadeInTime  = 0.05;
  fadeOutTime = 0.05;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

// All 10 lightning-flashes, re-using the same light, each with its 
//  own local offset; the timing was derived in Maya to match the 
//  animated visibility in the dts model

datablock afxT3DPointLightData(TH_CasterLightningFlashLight_CE)
{
  radius = 8.0;
  color = "0.0 0.08 1.0";
  brightness = 1.0;
  castShadows = false; // T3D Off (temporarily)
  localRenderViz = false;
};

datablock afxEffectWrapperData(TH_CasterLightningFlashLight1_EW)
{
  effect = TH_CasterLightningFlashLight_CE;
  constraint = caster;
  fadeInTime  = 0.12;
  fadeOutTime = 0.12;
  delay = 3.27;
  lifetime = 0.16;
  xfmModifiers[0] = TH_CasterLightningFlash_offset1_XM;
};
datablock afxEffectWrapperData(TH_CasterLightningFlashLight2_EW : TH_CasterLightningFlashLight1_EW)
{
  delay = 3.83;
  lifetime = 0.23;
  xfmModifiers[0] = TH_CasterLightningFlash_offset2_XM;
};
datablock afxEffectWrapperData(TH_CasterLightningFlashLight3_EW : TH_CasterLightningFlashLight1_EW)
{
  delay = 3.97;  
  lifetime = 0.23;
  xfmModifiers[0] = TH_CasterLightningFlash_offset3_XM;
};
datablock afxEffectWrapperData(TH_CasterLightningFlashLight4_EW : TH_CasterLightningFlashLight1_EW)
{
  delay = 4.07;  
  lifetime = 0.20;
  xfmModifiers[0] = TH_CasterLightningFlash_offset4_XM;
};
datablock afxEffectWrapperData(TH_CasterLightningFlashLight5_EW : TH_CasterLightningFlashLight1_EW)
{
  delay = 5.37;  
  lifetime = 0.23;
  xfmModifiers[0] = TH_CasterLightningFlash_offset5_XM;
};
datablock afxEffectWrapperData(TH_CasterLightningFlashLight6_EW : TH_CasterLightningFlashLight1_EW)
{
  delay = 6.20;  
  lifetime = 0.27;
  xfmModifiers[0] = TH_CasterLightningFlash_offset6_XM;
};
datablock afxEffectWrapperData(TH_CasterLightningFlashLight7_EW : TH_CasterLightningFlashLight1_EW)
{
  delay = 6.23;  
  lifetime = 0.27;
  xfmModifiers[0] = TH_CasterLightningFlash_offset7_XM;
};
datablock afxEffectWrapperData(TH_CasterLightningFlashLight8_EW : TH_CasterLightningFlashLight1_EW)
{
  delay = 6.27;  
  lifetime = 0.27;
  xfmModifiers[0] = TH_CasterLightningFlash_offset8_XM;
};
datablock afxEffectWrapperData(TH_CasterLightningFlashLight9_EW : TH_CasterLightningFlashLight1_EW)
{
  delay = 6.6;  
  lifetime = 0.23;
  xfmModifiers[0] = TH_CasterLightningFlash_offset9_XM;
};
datablock afxEffectWrapperData(TH_CasterLightningFlashLight10_EW : TH_CasterLightningFlashLight1_EW)
{
  delay = 6.77;  
  lifetime = 0.20;
  xfmModifiers[0] = TH_CasterLightningFlash_offset10_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DPointLightData(TH_CasterLightningFlashWhiteLight_CE)
{
  radius = 3.0;
  color = "1.0 1.0 1.0";
  brightness = 2.0;
  castShadows = false; // T3D Off (temporarily)
  localRenderViz = false;
};

datablock afxEffectWrapperData(TH_CasterLightningFlashWhiteLight1_EW : TH_CasterLightningFlashLight1_EW)
{
  effect = TH_CasterLightningFlashWhiteLight_CE;
};
datablock afxEffectWrapperData(TH_CasterLightningFlashWhiteLight2_EW : TH_CasterLightningFlashLight2_EW)
{
  effect = TH_CasterLightningFlashWhiteLight_CE;
};
datablock afxEffectWrapperData(TH_CasterLightningFlashWhiteLight3_EW : TH_CasterLightningFlashLight3_EW)
{
  effect = TH_CasterLightningFlashWhiteLight_CE;
};
datablock afxEffectWrapperData(TH_CasterLightningFlashWhiteLight4_EW : TH_CasterLightningFlashLight4_EW)
{
  effect = TH_CasterLightningFlashWhiteLight_CE;
};
datablock afxEffectWrapperData(TH_CasterLightningFlashWhiteLight5_EW : TH_CasterLightningFlashLight5_EW)
{
  effect = TH_CasterLightningFlashWhiteLight_CE;
};
datablock afxEffectWrapperData(TH_CasterLightningFlashWhiteLight6_EW : TH_CasterLightningFlashLight6_EW)
{
  effect = TH_CasterLightningFlashWhiteLight_CE;
};
datablock afxEffectWrapperData(TH_CasterLightningFlashWhiteLight7_EW : TH_CasterLightningFlashLight7_EW)
{
  effect = TH_CasterLightningFlashWhiteLight_CE;
};
datablock afxEffectWrapperData(TH_CasterLightningFlashWhiteLight8_EW : TH_CasterLightningFlashLight8_EW)
{
  effect = TH_CasterLightningFlashWhiteLight_CE;
};
datablock afxEffectWrapperData(TH_CasterLightningFlashWhiteLight9_EW : TH_CasterLightningFlashLight9_EW)
{
  effect = TH_CasterLightningFlashWhiteLight_CE;
};
datablock afxEffectWrapperData(TH_CasterLightningFlashWhiteLight10_EW : TH_CasterLightningFlashLight10_EW)
{
  effect = TH_CasterLightningFlashWhiteLight_CE;
};  

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DPointLightData(TH_LightFromAbove_CE)
{
  radius = 30.0;
  color = "1.0 1.0 0.6";
  brightness = 1.0;
  castShadows = false; // T3D Off (temporarily)
  localRenderViz = false;
};

datablock afxEffectWrapperData(TH_LightFromAbove_EW)
{
  effect = TH_LightFromAbove_CE;
  constraint = caster;
  delay = 0.0;
  fadeInTime = 1.0;
  fadeOutTime = 0.5;
  lifetime = 2.5;
  xfmModifiers[0] = "TH_LightFromAbove_offset2_XM";
  propagateTimeFactor = true;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DPointLightData(TH_HammerStrikeLight_CE)
{
  radius = 40.0;
  color = "1.0 1.0 1.0";
  brightness = 1.0;
  castShadows = false; // T3D Off (temporarily)
  localRenderViz = false;
};

datablock afxEffectWrapperData(TH_HammerStrikeLight_EW : TH_ZodeStrike1_Effect_EW)
{
  effect = TH_HammerStrikeLight_CE;
  lifetime = 0.1;
  fadeOutTime = 0.2;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function TH_add_Lighting_FX(%spell_data)
{
  %spell_data.addCastingEffect(TH_MainZodeRevealLight_1_EW);
  %spell_data.addCastingEffect(TH_MainZodeRevealLight_2_EW);
  %spell_data.addCastingEffect(TH_MainZodeRevealLight_3_EW);
  %spell_data.addCastingEffect(TH_CastingZodeLight_EW);

  %spell_data.addCastingEffect(TH_ZodeLightningFlash1_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash2_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash3_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash4_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash5_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash6_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash7_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash8_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash9_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash10_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash11_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash12_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash13_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash14_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash15_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash16_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash17_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash18_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash19_EW);
  %spell_data.addCastingEffect(TH_ZodeLightningFlash20_EW);

  %spell_data.addCastingEffect(TH_CasterLightningFlashLight1_EW);
  %spell_data.addCastingEffect(TH_CasterLightningFlashLight2_EW);
  %spell_data.addCastingEffect(TH_CasterLightningFlashLight3_EW);
  %spell_data.addCastingEffect(TH_CasterLightningFlashLight4_EW);
  %spell_data.addCastingEffect(TH_CasterLightningFlashLight5_EW);
  %spell_data.addCastingEffect(TH_CasterLightningFlashLight6_EW);
  %spell_data.addCastingEffect(TH_CasterLightningFlashLight7_EW);
  %spell_data.addCastingEffect(TH_CasterLightningFlashLight8_EW);
  %spell_data.addCastingEffect(TH_CasterLightningFlashLight9_EW);
  %spell_data.addCastingEffect(TH_CasterLightningFlashLight10_EW);

  %spell_data.addCastingEffect(TH_CasterLightningFlashWhiteLight1_EW);
  %spell_data.addCastingEffect(TH_CasterLightningFlashWhiteLight2_EW);
  %spell_data.addCastingEffect(TH_CasterLightningFlashWhiteLight3_EW);
  %spell_data.addCastingEffect(TH_CasterLightningFlashWhiteLight4_EW);
  %spell_data.addCastingEffect(TH_CasterLightningFlashWhiteLight5_EW);
  %spell_data.addCastingEffect(TH_CasterLightningFlashWhiteLight6_EW);
  %spell_data.addCastingEffect(TH_CasterLightningFlashWhiteLight7_EW);
  %spell_data.addCastingEffect(TH_CasterLightningFlashWhiteLight8_EW);
  %spell_data.addCastingEffect(TH_CasterLightningFlashWhiteLight9_EW);
  %spell_data.addCastingEffect(TH_CasterLightningFlashWhiteLight10_EW);

  %spell_data.addCastingEffect(TH_LightFromAbove_EW);
  %spell_data.addCastingEffect(TH_HammerStrikeLight_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
