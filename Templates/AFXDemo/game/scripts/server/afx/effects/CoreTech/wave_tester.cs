
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// WAVE TESTER
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

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

//
// Here we test if the script is being reloaded or if this is the
// first time the script has executed this mission.
//
$spell_reload = isObject(WaveTesterSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = WaveTesterSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxXM_WorldOffsetData(WVT_Z_Fixed_XM)
{
  worldOffset = "0 0 3";
};

datablock afxXM_SpinData(WVT_SpinEmitter_XM)
{
  spinAngle = 0;
  spinRate = 30;
};

datablock afxXM_SpinData(WVT_SpinCompass_XM)
{
  spinAngle = 5;
  spinRate = 30;
};

datablock afxXM_LocalOffsetData(WVT_Radius_XM)
{
  localOffset = "0 10 0";
};

datablock afxXM_WaveScalarData(WVT_Wave1_XM)
{
  a = -1.2;
  b = 1.2;
  parameter = "pos";
  op = "add";
  axis = "0 0 1";
  axisIsLocal = true;
  waveform = "sine";
  speed = 1;
  speedVariance = 0.0;
  dutyCycle = 1;
  phaseShift = 0.0;
  dutyShift = 0.0;
  offDutyT = 0.0;
  acceleration = 0.0;
  wavesPerPulse = 1;
  wavesPerRest = 0;
  restDuration = 0.0;
  restDurationVariance = 0.0;
};

datablock afxXM_WaveScalarData(WVT_OriWave1_XM : WVT_Wave1_XM)
{
  a = -180;
  b = 0;
  axis = "0 1 0";
  axisIsLocal = false;
  op = "mult";
  parameter = "ori";
};

datablock afxXM_WaveRiderColorData(WVT_ColorWaveRider1_XM)
{
  a = "1 0 0 1";
  b = "0 1 1 1";
  op = "replace";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Stationary Dots (Particles)

datablock ParticleData(WVT_Dot_P)
{
  textureName          = %mySpellDataPath @ "/Shared/particles/harddot";
  dragCoeffiecient     = 0;
  windCoefficient      = 0;
  gravityCoefficient   = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 12000;
  spinRandomMin        = 0;
  spinRandomMax        = 0;
  colors[0]            = "$$ %%._color";
  colors[1]            = "$$ %%._color";
  sizes[0]             = 0.07;
  sizes[1]             = 0.07;
  times[0]             = 0.0;
  times[1]             = 1.0;
};

datablock ParticleEmitterData(WVT_Dot_E)
{
  overrideAdvance   = true;
  ejectionPeriodMS  = 8;
  ejectionVelocity  = 0.0;
  velocityVariance  = 0.0;
  thetaMin          = 0.0;
  thetaMax          = 0.0;
  particles         = "WVT_Dot_P";
  fadeAlpha         = true;
};

datablock afxEffectWrapperData(WVT_Dot_EW)
{
  effect = WVT_Dot_E;
  constraint = "anchor";
  lifetime = 12;
  xfmModifiers[0] = WVT_Z_Fixed_XM;
  xfmModifiers[1] = WVT_SpinEmitter_XM;
  xfmModifiers[2] = WVT_Radius_XM;
  xfmModifiers[3] = WVT_Wave1_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Orientation Compass (ZodiacPlane)

datablock afxZodiacPlaneData(WVT_arrowZode_CE)
{
  texture = %mySpellDataPath @ "/Shared/zodiacs/compass_arrow";
  rotationRate = 0;
  doubleSided = true;
  radius = 1;
  blend = "additive";
  faceDir = "forward";
  useFullTransform = true;
  trackOrientConstraint = true; 
};
datablock afxEffectWrapperData(WVT_arrowZode_EW)
{
  effect = WVT_arrowZode_CE;
  constraint = "anchor";
  lifetime = 10;
  fadeInTime = 2;
  fadeOutTime = 2;

  xfmModifiers[0] = WVT_Z_Fixed_XM;
  xfmModifiers[1] = WVT_SpinCompass_XM;
  xfmModifiers[2] = WVT_Radius_XM;
  xfmModifiers[3] = WVT_OriWave1_XM;
  xfmModifiers[4] = WVT_ColorWaveRider1_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// spell

datablock afxMagicSpellData(WaveTesterSpell)
{
  castingDur = 0.0;

  addCastingEffect = WVT_Dot_EW;
  addCastingEffect = WVT_arrowZode_EW;
};
//
datablock afxRPGMagicSpellData(WaveTesterSpell_RPG)
{
  spellName = "Wave Tester";
  desc = "Visualizes wave modifier settings by drawing " @ 
         "the shape of its wave in a ring around the spellcaster. " @ 
         "Modify the spell's wave xmod, recast, and observe the changes.\n\n" @
         "[testing effect]";
  iconBitmap = %mySpellDataPath @ "/WVT/icons/wvt"; 
  sourcePack = "Core Tech";
  target = "self";
  castingDur = WaveTesterSpell.castingDur;
};

//~~~~~~~~~~~~~~~~~~~~//

$my_index = 0;
$my_colors[0] = "1 1 0 1";
$my_colors[1] = "0 1 0 1";
$my_colors[2] = "1 0 0 1";
$my_colors[3] = "0 1 1 1";

function WaveTesterSpell::onActivate(%this, %spell, %caster, %target)
{
  Parent::onActivate(%this, %spell, %caster, %target);

  %spell.addConstraint(%caster.getTransform(), "anchor");

  %spell._color = $my_colors[$my_index % 4];
  $my_index++;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

if ($spell_reload)
{
  // Removes then adds all reloaded datablocks
  touchDataBlocks();
}
else
{
  // save script filename and data path for reloads
  WaveTesterSpell.scriptFile = $afxAutoloadScriptFile;
  WaveTesterSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(WaveTesterSpell, WaveTesterSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

