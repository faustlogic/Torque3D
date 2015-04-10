
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SELECTRONS (SpellPack2)
//
//  This script implements the following selectrons:
//
//  Up Up Up
//    Uses zodiacs with zodiac-planes to create the appearance of rings emerging from
//    the ground and lifting to the height of the selected player. Also shows a
//    billboard arrow when the "m" key is held down.
//
//  Flame On
//    Invokes the Up In Flames element on selected objects.
//
//  Wisp Light
//    Intended as a free-targeting selectron, this one features a wispy light
//    than can be used to illuminate and explore dark corners. Lighting effect
//    is increased when the "m" key is held down.
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

$AFX_VERSION = (isFunction(afxGetVersion)) ? afxGetVersion() : 1.02;
$MIN_REQUIRED_VERSION = 2.0;

// Test version requirements for this script
if ($AFX_VERSION < $MIN_REQUIRED_VERSION)
{
  error("AFX script " @ fileName($afxAutoloadScriptFile) @ " is not compatible with AFX versions older than " @ $MIN_REQUIRED_VERSION @ ".");
  return;
}

// the selectrons in this script use elements from these other effects
afxExecPrerequisite("sp2_shared_fx.cs", $afxAutoloadScriptFolder);
afxExecPrerequisite("up_in_flames.cs", $afxAutoloadScriptFolder);

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

$thisEngine = afxGetEngine();

// style numbers
$UpUpUp_Style = 20;
$FlameOn_Style = 21;

%mySelectronDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder @ "/SELE";

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

$isTGEA = (afxGetEngine() $= "TGEA");

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// UP UP UP Selectron Style
//
//    This selectron style makes use of zodiac-planes that lift off from
//    matching zodiacs projected onto the ground. It also features an
//    overhead arrow billoard that appears in reponse to holding down 
//    a keyboard key (m).
//

$UpUpUp_Zode_RotationRate = 45.0;

// Main Selectron Zodiac (rotating clockwise)

datablock afxZodiacData(UpUpUp_CW_Zode_CE : SHARED_SelectronZodiac_CE)
{  
  texture = %mySelectronDataPath @ "/up_up_up/uuu_ring";
  radius = 1.8;
  startAngle = 0.0;
  rotationRate = $UpUpUp_Zode_RotationRate;
  color = "1.0 1.0 1.0 0.3";
  blend = additive;
  distanceMax = ($thisEngine $= "T3D") ? 75 : 200;
  distanceFalloff = ($thisEngine $= "T3D") ? 30 : 180;
};
datablock afxEffectWrapperData(UpUpUp_CW_Zode_EW)
{
  effect = UpUpUp_CW_Zode_CE;
  posConstraint = selected;
  fadeInTime = 0.20;
  fadeOutTime = 0.20;
  effectName = "MainZode";
  isConstraintSrc = true;
  xfmModifiers[0] = SHARED_AltitudeConform_XM;
};

// Main Selectron Zodiac (rotating counter-clockwise)

datablock afxZodiacData(UpUpUp_CCW_Zode_CE : UpUpUp_CW_Zode_CE)
{  
  rotationRate = -$UpUpUp_Zode_RotationRate;
};
datablock afxEffectWrapperData(UpUpUp_CCW_Zode_EW)
{
  effect = UpUpUp_CCW_Zode_CE;
  posConstraint = "#effect.MainZode";
  borrowAltitudes = true;
  fadeInTime = 0.20;
  fadeOutTime = 0.20;
};

// Zodiac Plane

datablock afxZodiacPlaneData(UpUpUp_Lifting_Ring_CE)
{  
  texture = %mySelectronDataPath @ "/up_up_up/uuu_ring";
  radius = 1.8;
  startAngle = 0.0;
  rotationRate = $UpUpUp_Zode_RotationRate;
  color = "1.0 1.0 1.0 0.6";
  blend = additive;
  doubleSided = true;
  faceDir = up;
};

// lifts ring up from ground
datablock afxXM_WaveScalarData(UpUpUp_Lifting_Wave_XM)
{
  a = 0.0;
  b = 3.5;
  parameter = "pos";
  op = "add";
  axis = "0 0 1";
  waveform = "sine";
  dutyCycle = 2;
  speed = 0.5;
};
// gradually fades out the ring 
datablock afxXM_WaveRiderScalarData(UpUpUp_Fading_Wave_XM)
{
  parameter = "vis";
  a = 1.0;
  b = 0.0;
  op = "mult";
};
// initial fade-in near ground
datablock afxXM_WaveScalarData(UpUpUp_FadeIn_Wave_XM)
{
  parameter = "vis";
  a = 0;
  b = 1.0;
  op = "mult";
  waveform = "sine";
  dutyCycle = 2;
  speed = 1;
  offDutyT = 1.0;
  wavesPerPulse = 1;
  wavesPerRest = 1;
};

// Three Ascending Rings 

datablock afxEffectWrapperData(UpUpUp_Lifting_Ring1_EW)
{
  effect = UpUpUp_Lifting_Ring_CE;
  posConstraint = selected;
  fadeInTime = 0.20;
  fadeOutTime = 0.20;
  xfmModifiers[0] = UpUpUp_Lifting_Wave_XM;
    xfmModifiers[1] = UpUpUp_Fading_Wave_XM;
    xfmModifiers[2] = UpUpUp_FadeIn_Wave_XM;
};
datablock afxEffectWrapperData(UpUpUp_Lifting_Ring2_EW : UpUpUp_Lifting_Ring1_EW)
{
  delay = 0.35;
};
datablock afxEffectWrapperData(UpUpUp_Lifting_Ring3_EW : UpUpUp_Lifting_Ring1_EW)
{
  delay = 0.7;
};

// Triggered Overhead Arrow

datablock afxXM_BoxConformData(UpUpUp_Arrow_Boxtop_XM)
{
  boxAlignment = "+z";
};
datablock afxXM_WaveScalarData(UpUpUp_Arrow_Bobbing_XM)
{
  a = 0.7;
  b = 1.0;
  parameter = "pos";
  op = "add";
  axis = "0 0 1";
  waveform = "sine";
  speed = 0.6;
};

datablock afxBillboardData(UpUpUp_OverHead_Arrow_CE)
{
  texture = %mySelectronDataPath @ "/up_up_up/uuu_down_arrow";
  color = "1 1 1 1";
  dimensions = "1.1 1.1";
};
datablock afxEffectWrapperData(UpUpUp_OverHead_Arrow_EW)
{
  effect = UpUpUp_OverHead_Arrow_CE;
  posConstraint = "selected";
  fadeInTime = 0.5;
  fadeOutTime = 0.5;
  xfmModifiers[0] = UpUpUp_Arrow_Boxtop_XM;
  xfmModifiers[1] = UpUpUp_Arrow_Bobbing_XM;
};

// Phrase Effect (responds to continuous trigger)

datablock afxPhraseEffectData(UpUpUp_fx_trigger_CE)
{
  triggerMask = 0x800000;
  ignoreChoreographerTriggers = false;
  ignoreConstraintTriggers = true;
  ignorePlayerTriggers = true;
  phraseType = "continuous";

  addEffect = UpUpUp_OverHead_Arrow_EW;
};
datablock afxEffectWrapperData(UpUpUp_fx_trigger_EW)
{
  effect = UpUpUp_fx_trigger_CE;
  posConstraint = "selected";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxSelectronData(UpUpUp_SELE)
{
  selectionTypeStyle = $UpUpUp_Style;
  selectionTypeMask = $TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType;

  mainDur = $AFX::INFINITE_TIME;

  addMainEffect = UpUpUp_CW_Zode_EW;
  addMainEffect = UpUpUp_CCW_Zode_EW;
  
  addMainEffect = UpUpUp_Lifting_Ring1_EW;
  addMainEffect = UpUpUp_Lifting_Ring2_EW;
  addMainEffect = UpUpUp_Lifting_Ring3_EW;

  addMainEffect = UpUpUp_fx_trigger_EW;
};

// Add styles to the demo's selectron manager. (This is only
// needed to allow selectron cycling using the 't' key.)
// 
addDemoSelectronStyle("UP UP UP",     $UpUpUp_Style);

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// FLAME ON Selectron Style
//

datablock afxEffectWrapperData(FLON_Victim_Fire_EW)
{
  effect = UIF_Victim_Fire_E;
  constraint = "selected.Bip01 Pelvis";
  delay = 0;
  lifetime = -1;
  fadeInTime = 0.25;
  fadeOutTime = 0.25;
  xfmModifiers[0] = UIF_Victim_Fire_velocityoffset_XM;
    xfmModifiers[1] = UIF_Victim_Fire_Aim_XM;
    xfmModifiers[2] = UIF_Victim_Fire_oscX_XM;
    xfmModifiers[3] = UIF_Victim_Fire_oscY_XM;  
    xfmModifiers[4] = UIF_Victim_Fire_oscZ_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxSelectronData(FlameOn_SELE)
{
  selectionTypeStyle = $FlameOn_Style;
  selectionTypeMask = $TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType;

  mainDur = $AFX::INFINITE_TIME;

  addMainEffect = FLON_Victim_Fire_EW;
};

// Add styles to the demo's selectron manager. (This is only
// needed to allow selectron cycling using the 't' key.)
// 
addDemoSelectronStyle("Flame On", $FlameOn_Style);

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

// wisp magic-fire particles
datablock ParticleData(WispLight_MagicFire_P)
{
  textureName = %mySelectronDataPath @ "/wisp/wisp_sele_fire";
  dragCoeffiecient     = 0.5;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 500;
  lifetimeVarianceMS   = 300;
  spinRandomMin        = -300;
  spinRandomMax        = 300;
  colors[0]            = "0.5 0.5 0.5 1.0";
    colors[1]            = "1.0 1.0 1.0 1.0";
    colors[2]            = "0.0 0.0 0.0 0.0";
  sizeBias             = 1.0;
  sizes[0]             = 1.0;
    sizes[1]             = 0.5; 
    sizes[2]             = 0;
  times[0]             = 0.0;
    times[1]             = 0.5;
    times[2]             = 1.0;
  constrainPos         = true;
};

datablock ParticleData(WispLight_MagicFire_Big_P : WispLight_MagicFire_P)
{
  sizeBias = 3.0;
};

datablock ParticleData(WispLight_MagicCloud_P)
{
  textureName = %mySelectronDataPath @ "/wisp/wisp_sele_cloud";
  dragCoeffiecient     = 0.5;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 1000;
  lifetimeVarianceMS   = 500;
  spinRandomMin        = -100;
  spinRandomMax        = 100;
  colors[0]            = "0.0 0.0 0.0 0.0";
    colors[1]            = "1.0 1.0 1.0 0.3";
    colors[2]            = "0.0 0.0 0.0 0.0";
  sizeBias             = 4.0;
  sizes[0]             = 1.0;
    sizes[1]             = 2.0; 
    sizes[2]             = 3.0;
  times[0]             = 0.0;
    times[1]             = 0.5;
    times[2]             = 1.0;
};
datablock ParticleData(WispLight_MagicCloud_Big_P : WispLight_MagicCloud_P)
{
  sizeBias = 7.0;
};

datablock afxParticleEmitterVectorData(WispLight_MagicFire_E)
{
  ejectionPeriodMS      = 50;
  periodVarianceMS      = 40;
  ejectionVelocity      = 0;
  velocityVariance      = 0;
  particles             = "WispLight_MagicFire_P";
  vector                = "0 0 1";
  vectorIsWorld         = true;
  blendStyle            = "ADDITIVE";
};
datablock afxParticleEmitterVectorData(WispLight_MagicFire_Big_E : WispLight_MagicFire_E)
{
  particles = "WispLight_MagicFire_Big_P";
};

datablock afxParticleEmitterConeData(WispLight_MagicCloud_E)
{
  vector                = "0 0 1";
  spreadMin             = 175.0;
  spreadMax             = 180.0;
  ejectionPeriodMS      = 25;
  periodVarianceMS      = 10;
  ejectionVelocity      = 4.0;
  velocityVariance      = 2.0;
  particles             = "WispLight_MagicCloud_P";
  ejectionOffset        = 2.0;
  blendStyle            = "ADDITIVE";
};

datablock afxParticleEmitterConeData(WispLight_MagicCloud_Big_E : WispLight_MagicCloud_E)
{
  ejectionPeriodMS      = 10;
  periodVarianceMS      = 5;
  ejectionVelocity      = 9.0;
  velocityVariance      = 3.0;
  particles             = "WispLight_MagicCloud_Big_P";
};

datablock afxXM_LocalOffsetData(WispLight_radial_offset_XM)
{
  localOffset = "0.1 0 0";
};

datablock afxXM_GroundConformData(WispLight_ground_XM)
{
  height = 0.15;
};

datablock afxXM_LocalOffsetData(WispLight_offset_XM)
{
  localOffset = "0 0 0.15";
};
datablock afxXM_LocalOffsetData(WispLight_offset2_XM)
{
  localOffset = "0 0 0.15";
};

datablock afxXM_SpinData(WispLight_spin_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 0;
  spinRate = 300*2;
};

datablock afxXM_WaveScalarData(WispLight_wave_XM)
{
  a = 0.0;
  b = 0.25;
  parameter = "pos";
  op = "add";
  axis = "0 0 1";
  waveform = "sine";
  speed = 2.0;
};

datablock afxEffectWrapperData(WispLight_Flame_EW)
{
  effect = WispLight_MagicFire_E;
  constraint = "freeTarget";
  delay = 0.5;
  effectName = "WispLight";
  isConstraintSrc = true;
  xfmModifiers[1] = WispLight_offset_XM;
    xfmModifiers[0] = WispLight_wave_XM;
    xfmModifiers[2] = WispLight_spin_XM;
    xfmModifiers[3] = WispLight_radial_offset_XM;
};

datablock afxXM_AimData(WispLight_Cloud_Aim_XM)
{
  aimZOnly = false;
};

datablock afxEffectWrapperData(WispLight_Cloud_EW)
{
  effect = WispLight_MagicCloud_E;
  constraint = "#effect.WispLight";
  delay = 0.5;
};

datablock afxXM_WaveScalarData(WispLight_Zode_Wave1_XM)
{
  parameter = "vis";
  waveform = "sawtooth";
  a = 1.0;
  b = 0.0;
  op = "mult";
  speed = 2.0;
};
datablock afxXM_WaveScalarData(WispLight_Zode_Wave2_XM)
{
  parameter = "scale";
  waveform = "sawtooth";
  a = 0;
  b = 1.0;
  bVariance = 0.5;
  op = "mult";
  speed = 2.0;
};
datablock afxZodiacData(WispLight_Zode_CE : SHARED_SelectronZodiac_CE)
{  
  texture = %mySelectronDataPath @ "/wisp/wisp_sele_zode";
  radius = 1.8;
  startAngle = 0.0;
  rotationRate = 0;
  color = "1.0 1.0 1.0 0.3";
  blend = additive;
  distanceMax = ($thisEngine $= "T3D") ? 75 : 200;
  distanceFalloff = ($thisEngine $= "T3D") ? 30 : 180;
};
datablock afxEffectWrapperData(WispLight_ZodiacA_EW)
{
  effect = WispLight_Zode_CE;
  constraint = "#effect.WispLight";
  delay = 0.9;
  
  xfmModifiers[0] = WispLight_Zode_Wave1_XM;
  xfmModifiers[1] = WispLight_Zode_Wave2_XM;
};
datablock afxEffectWrapperData(WispLight_ZodiacB_EW : WispLight_ZodiacA_EW)
{
  delay = 0.9+0.05;
};
datablock afxEffectWrapperData(WispLight_ZodiacC_EW : WispLight_ZodiacA_EW)
{
  delay = 0.9+0.10;
};

datablock afxXM_WaveColorData(WispLight_Light_Wave_XM)
{
  a = "0.30 0.30 0.30 0.30";
  b = "0.20 0.20 0.20 0.20";
  op = "replace";
  waveform = "sine";
  speed = 2.0;
};

if ($thisEngine !$= "T3D")
{
  %WispLight_Light_intensity = 0.3;
  datablock afxLightData(WispLight_Light_CE)
  {
    type = "Point";
    radius = 3;
    sgCastsShadows = true;
    sgDoubleSidedAmbient = true;

    sgLightingModelName = ($isTGEA) ? "Original Advanced" : "Near Linear";
    color = (255/255)*%WispLight_Light_intensity SPC
            (255/255)*%WispLight_Light_intensity SPC 
            (255/255)*%WispLight_Light_intensity; 
    lightIlluminationMask = $AFX::ILLUM_DTS | $AFX::ILLUM_DIF; // TGEA (ignored by TGE)
  };

  datablock afxEffectWrapperData(WispLight_Light_EW : WispLight_Cloud_EW)
  {
    effect = WispLight_Light_CE;
    xfmModifiers[0] = WispLight_Light_Wave_XM;
  };

  $WispLight_Light = WispLight_Light_EW;

  //~~~~~~~~~~~~~~~~~~~~//

  %WispLight_SelectLight_intensity = 0.7;
  datablock afxLightData(WispLight_SelectLight_CE)
  {
    type = "Point";
    radius = 5;
    sgCastsShadows = true;
    sgDoubleSidedAmbient = true;

    sgLightingModelName = ($isTGEA) ? "Original Advanced" : "Near Linear";
    color = (255/255)*%WispLight_SelectLight_intensity SPC
            (255/255)*%WispLight_SelectLight_intensity SPC 
            (255/255)*%WispLight_SelectLight_intensity;  
    lightIlluminationMask = $AFX::ILLUM_DTS | $AFX::ILLUM_DIF; // TGEA (ignored by TGE)
  };

  datablock afxEffectWrapperData(WispLight_SelectLight_EW)
  {
    effect = WispLight_SelectLight_CE;
    posConstraint = "freeTarget";
    lifetime = 0.35;
    fadeInTime = 0.35;
    fadeOutTime = 0.35;
  };

  $WispLight_SelectLight = WispLight_SelectLight_EW;

  //~~~~~~~~~~~~~~~~~~~~//

  datablock afxLightData(WispLight_TriggeredLight_CE : WispLight_SelectLight_CE)
  {
    radius = 15; //20;
  };
  datablock afxEffectWrapperData(WispLight_TriggeredLight_EW : WispLight_SelectLight_EW)
  {
    effect = WispLight_TriggeredLight_CE;
    lifetime = $AFX::INFINITE_TIME;
  };

  $WispLight_TriggeredLight = WispLight_TriggeredLight_EW;
}
else // T3D lighting is currently unavailable
{
  $WispLight_Light = "";
  $WispLight_SelectLight = "";
  $WispLight_TriggeredLight = "";
}

datablock afxXM_LocalOffsetData(WispLight_triggered_offset_XM)
{
  localOffset = "0 0 0.45";
  fadeInTime = 0.3;
};
datablock afxEffectWrapperData(WispLight_Flame_Triggered_EW : WispLight_Flame_EW)
{
  effect = WispLight_MagicFire_Big_E;
  delay = 0;  
  effectName = "";
  isConstraintSrc = false;
  
  xfmModifiers[1] = WispLight_triggered_offset_XM;
};

datablock afxEffectWrapperData(WispLight_Cloud_Triggered_EW : WispLight_Cloud_EW)
{
  effect = WispLight_MagicCloud_Big_E; 
  delay = 0.0;
};

datablock afxZodiacData(WispLight_Zode_Triggered_CE : WispLight_Zode_CE)
{  
  radius = 1.8*3; 
};
datablock afxEffectWrapperData(WispLight_ZodiacA_Triggered_EW : WispLight_ZodiacA_EW)
{
  effect = WispLight_Zode_Triggered_CE;
};
datablock afxEffectWrapperData(WispLight_ZodiacB_Triggered_EW : WispLight_ZodiacB_EW)
{
  effect = WispLight_Zode_Triggered_CE;
};
datablock afxEffectWrapperData(WispLight_ZodiacC_Triggered_EW : WispLight_ZodiacC_EW)
{
  effect = WispLight_Zode_Triggered_CE;
};

datablock afxPhraseEffectData(WispLight_fx_trigger_CE)
{
  triggerMask = 0x800000;
  ignoreChoreographerTriggers = false;
  ignoreConstraintTriggers = true;
  ignorePlayerTriggers = true;
  phraseType = "continuous";
 
  addEffect = $WispLight_TriggeredLight;
  addEffect = WispLight_Flame_Triggered_EW;
  addEffect = WispLight_Cloud_Triggered_EW;
  addEffect = WispLight_ZodiacA_Triggered_EW;
  addEffect = WispLight_ZodiacB_Triggered_EW;
  addEffect = WispLight_ZodiacC_Triggered_EW;
};
datablock afxEffectWrapperData(WispLight_fx_trigger_EW)
{
  effect = WispLight_fx_trigger_CE;
};

datablock afxSelectronData(WispLight_SELE)
{
  selectionTypeStyle = 1;
  selectionTypeMask = 0;

  mainDur = $AFX::INFINITE_TIME;

  addMainEffect = WispLight_Flame_EW;
  addMainEffect = WispLight_Cloud_EW;
  addMainEffect = WispLight_ZodiacA_EW;
  addMainEffect = WispLight_ZodiacB_EW;
  addMainEffect = WispLight_ZodiacC_EW;
  addMainEffect = $WispLight_Light;
  
  addMainEffect = WispLight_fx_trigger_EW;
  
  addDeselectEffect = $WispLight_SelectLight;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
