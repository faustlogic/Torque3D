
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// MARK OF KORK (lighting)
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

datablock LightFlareData(MoK_HandMagicFlashLight_FLARE)
{
  overallScale = 1.0;
  flareEnabled = true;
  flareTexture = %mySpellDataPath @ "/Shared/lights/sp2_coronaFlare";
  elementRect[0] = "0 0 128 128";
  elementDist[0] = 0.0;
  elementScale[0] = 20.0;
  elementTint[0] = "1 1 1";
  elementRotate[0] = true;
  elementUseLightColor[0] = true;
};

datablock LightAnimData(MoK_HandMagicFlashLight_ANI)
{
  animEnabled = true;
  minBrightness = 0.5;
  maxBrightness = 1.0;
};

datablock afxT3DPointLightData(MoK_HandMagicFlashLight_CE)
{
  radius = 10;
  color = "1 1 1";
  brightness = 2;
  castShadows = false;
  localRenderViz = false;
  flareType = MoK_HandMagicFlashLight_FLARE;
  flareScale = 1;
  animate = true;
  animationType = MoK_HandMagicFlashLight_ANI;
  animationPeriod = 0.1;
  animationPhase = 0.0;
};

datablock afxEffectWrapperData(MoK_HandMagicFlashLight_EW) // MoK and SMS
{
  constraint = "caster.Bip01 L Hand";
  effect = MoK_HandMagicFlashLight_CE;
  delay = (40/30);
  lifetime = 8/30;
  fadeInTime = 5/30;
  fadeOutTime = 10/30;
  xfmModifiers[0] = MoK_Hand_Palm_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxT3DPointLightData(MoK_HandMagicLight_CE)
{
  radius = 5;
  color = "$$ (%%._sms) ? \"1.0 0.4 1.0\" : \"1.0 0.67 0.36\"";
  brightness = 1.3;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(MoK_HandMagicLight_EW : MoK_HandMagic_lfhand_EW) // MoK and SMS
{
  effect = MoK_HandMagicLight_CE;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// STROBE LIGHTS

datablock afxXM_WaveScalarData(MoK_Strobe1_Vis_XM)
{
  a = 0;
  b = 1;
  parameter = "vis";
  op = "multiply";
  waveform = "sine";
  speed = 1.0/(0.7*0.15*2.0);
  restDuration = 3.0*0.15 - 0.7*0.15*2.0;
  restDurationVariance = 0.1;
};

datablock afxXM_WaveRiderScalarData(MoK_Strobe1_Rad_XM)
{
  a = 1;
  b = 6.25;
  bVariance = 1.25;
  parameter = "scale";
  op = "multiply";
  waveform = "one";
  offDutyT = 1;
};

datablock LightFlareData(MoK_OrcBeamFlashLight_A2_FLARE)
{
  overallScale = 1.0;
  flareEnabled = true;
  flareTexture = %mySpellDataPath @ "/Shared/lights/sp2_coronaFlare";
  elementRect[0] = "0 0 128 128";
  elementDist[0] = 0.0;
  elementScale[0] = 5.0;
  elementTint[0] = "1 1 1";
  elementRotate[0] = false;
  elementUseLightColor[0] = true;
};

datablock afxT3DPointLightData(MoK_OrcBeamFlashLight_A2_CE)
{
  radius = 1;
  color = "1.0 0.925 0.725";
  brightness = 0.6;
  castShadows = false;
  localRenderViz = false;
  flareType = MoK_OrcBeamFlashLight_A2_FLARE;
  flareScale = 1;
};

datablock afxEffectWrapperData(MoK_OrcBeamStrobeLight_A1_EW)
{
  effectEnabled = "$$ !%%._sms";
  effect = MoK_OrcBeamFlashLight_A2_CE;
  posConstraint = "#effect.Hand_Mooring";
  delay = 120/30;
  lifetime = 2.0;
  fadeOutTime = 0.2;
  xfmModifiers[0] = MoK_OrcBeams_offset_XM;
  xfmModifiers[1] = MoK_Strobe1_Vis_XM;
  xfmModifiers[2] = MoK_Strobe1_Rad_XM;
};

datablock afxEffectWrapperData(MoK_OrcBeamStrobeLight_A2_EW : MoK_OrcBeamStrobeLight_A1_EW)
{
  delay = 120/30 + 0.15;
  lifetime = 2.0 - 0.15;
};

datablock afxEffectWrapperData(MoK_OrcBeamStrobeLight_A3_EW : MoK_OrcBeamStrobeLight_A1_EW)
{
  delay = 120/30 + 2*0.15;
  lifetime = 2.0 - 2*0.15;
};


//~~~~~~~~~~~~~~~~~~~~//

datablock afxXM_WaveScalarData(MoK_Strobe2_Vis_XM : MoK_Strobe1_Vis_XM)
{
  speed = 1.0/(0.7*0.1*2.0);
  restDuration = 3.0*0.1 - 0.7*0.1*2.0;
  restDurationVariance = 0.08;
};

datablock afxXM_WaveRiderScalarData(MoK_Strobe2_Rad_XM : MoK_Strobe1_Rad_XM)
{
  b = 7.5;
  bVariance = 1.5;
};

datablock LightFlareData(MoK_OrcBeamFlashLight_B2_FLARE : MoK_OrcBeamFlashLight_A2_FLARE)
{
  elementScale[0] = 8.0;
};

datablock afxT3DPointLightData(MoK_OrcBeamFlashLight_B2_CE : MoK_OrcBeamFlashLight_A2_CE)
{
  brightness = 1.05;
  flareType = MoK_OrcBeamFlashLight_B2_FLARE;
};

datablock afxEffectWrapperData(MoK_OrcBeamStrobeLight_B1_EW)
{
  effectEnabled = "$$ !%%._sms";
  effect = MoK_OrcBeamFlashLight_B2_CE;
  posConstraint = "#effect.Hand_Mooring";
  delay = 180/30;
  lifetime = 0.9;
  fadeOutTime = 0.3;
  xfmModifiers[0] = MoK_OrcBeams_offset_XM;
  xfmModifiers[1] = MoK_Strobe2_Vis_XM;
  xfmModifiers[2] = MoK_Strobe2_Rad_XM;
};
datablock afxEffectWrapperData(MoK_OrcBeamStrobeLight_B2_EW : MoK_OrcBeamStrobeLight_B1_EW)
{
  delay = 180/30 + 0.1;
  lifetime = 0.9 - 0.1;
};
datablock afxEffectWrapperData(MoK_OrcBeamStrobeLight_B3_EW : MoK_OrcBeamStrobeLight_B1_EW)
{
  delay = 180/30 + 2*0.1;
  lifetime = 0.9  - 2*0.1;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function MoK_add_Lighting_FX(%spell_data)
{
  %spell_data.addCastingEffect(MoK_HandMagicFlashLight_EW);
  %spell_data.addCastingEffect(MoK_HandMagicLight_EW);

    // erratic strobe light (_mok) //
  %spell_data.addCastingEffect(MoK_OrcBeamStrobeLight_A1_EW);
  %spell_data.addCastingEffect(MoK_OrcBeamStrobeLight_A2_EW);
  %spell_data.addCastingEffect(MoK_OrcBeamStrobeLight_A3_EW);

    // faster strobe light (_mok) //
  %spell_data.addCastingEffect(MoK_OrcBeamStrobeLight_B1_EW);
  %spell_data.addCastingEffect(MoK_OrcBeamStrobeLight_B2_EW);
  %spell_data.addCastingEffect(MoK_OrcBeamStrobeLight_B3_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
