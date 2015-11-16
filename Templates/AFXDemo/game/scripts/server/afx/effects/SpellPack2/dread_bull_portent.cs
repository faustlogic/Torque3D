
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// DREAD BULL PORTENT -- [reusable element]
//
// Tags the ground with a glowing bull symbol.
//
// Uses a special wisp-like free-targeting selectron when available.
//
//    parameters:
//      _n_pulses
//      _shake 
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
$spell_reload = isObject(DreadBullPortentSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = DreadBullPortentSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

%DBP_TimeScale = 2.0;

// this offset centers the bull symbol
datablock afxXM_LocalOffsetData(DBP_BullZode_offset_XM)
{
  localOffset = "0.0 0.8 0.0";
};

// these wave xmods shakes the symbol when it appears
datablock afxXM_WaveScalarData(DBP_Bull_wave1_XM)
{
  a = -0.02*2;
  b = 0.04*2;
  parameter = "pos";
  op = "add";
  axis = "1 0 0";
  axisIsLocal = true;
  waveform = "sine";
  speed = 70/6.3;
  speedVariance = 0.0;
  offDutyT = 0.5;
  fadeInTime = 0.3;
  lifetime = 1.5;
  fadeOutTime = 1.0;
};
datablock afxXM_WaveScalarData(DBP_Bull_wave2_XM : DBP_Bull_wave1_XM)
{
  axis = "0 1 0";
  speed = 60/6.3;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxZodiacData(DBP_BullGlowZode_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/DBP/zodiacs/dbp_glowing_bull";
  radius = 1.9;
  startAngle = 0;
  rotationRate = 0.0;
  color = "1 1 1 1.0";
  blend = additive;
  trackOrientConstraint = true;
};
datablock afxEffectWrapperData(DBP_BullGlowZode_EW)
{
  effect = DBP_BullGlowZode_CE;
  constraint = "anchor";
  delay = 0;
  fadeInTime = 1.0;
  fadeOutTime = 0.5;
  lifetime = 2.0;
  xfmModifiers[0] = DBP_BullZode_offset_XM;
  xfmModifiers[1] = DBP_Bull_wave1_XM;
  xfmModifiers[2] = DBP_Bull_wave2_XM;
  // these subs keep-or-clear the wave xmods according to _shake
  xfmModifiers[1] = "$$ (%%._shake == true) ? \"~~\" : \"~0\"";
  xfmModifiers[2] = "$$ (%%._shake == true) ? \"~~\" : \"~0\"";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxZodiacData(DBP_BullHotZode_CE : DBP_BullGlowZode_CE)
{  
  texture = %mySpellDataPath @ "/DBP/zodiacs/dbp_hot_bull";
  color = "1 1 1 1";
  blend = additive;
};
datablock afxEffectWrapperData(DBP_BullHotZode_EW)
{
  effect = DBP_BullHotZode_CE;
  constraint = "anchor";
  delay = 0;
  fadeInTime = 0.5;
  fadeOutTime = 5.0;
  lifetime = 0.5;
  xfmModifiers[0] = DBP_BullZode_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxZodiacData(DBP_CharredOutline_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/DBP/zodiacs/dbp_charred_ol_bull";
  radius = 1.9;
  startAngle = 0;
  rotationRate = 0.0;
  color = "1 1 1 1";
  blend = normal;
  trackOrientConstraint = true;
};
datablock afxEffectWrapperData(DBP_CharredOutline_EW)
{
  effect = DBP_CharredOutline_CE;
  constraint = "anchor";
  delay = 3.0+2;
  fadeInTime = 1.0;
  fadeOutTime = 1.0;
  lifetime = (10*%DBP_TimeScale) + 1.0;
  xfmModifiers[0] = DBP_BullZode_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// fixed blotchy magenta symbol
//
datablock afxZodiacData(DBP_BullZode_CE : DBP_CharredOutline_CE)
{  
  texture = %mySpellDataPath @ "/DBP/zodiacs/dbp_color_bull_a";
  color = "1 1 1 0.5";
  blend = additive;
};
datablock afxEffectWrapperData(DBP_BullZode_EW)
{
  effect = DBP_BullZode_CE;
  constraint = "anchor";
  delay = 0+2;
  fadeInTime = 1.0;
  fadeOutTime = 1.0;
  lifetime = (10*%DBP_TimeScale)-1.0;
  xfmModifiers[0] = DBP_BullZode_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// shimmering blotchy highlights (randomized)
//
datablock afxZodiacData(DBP_BullZode123_CE : DBP_BullZode_CE)
{
  texture = "$$ \"" @ %mySpellDataPath @ "/DBP/zodiacs/dbp_blotchy_bull_\" @ getRandom(1,3)";
  color = "1 1 1 0.5";
};
datablock afxEffectWrapperData(DBP_BullZodeA_shimmer_00_EW)
{
  effect = DBP_BullZode123_CE;
  constraint = "anchor"; 
  delay = "$$ ##";
  lifetime = 1;
  fadeInTime = 1;
  fadeOutTime = 1;
  lifetimeBias = "$$ getRandomF(0.3, 1.0)";
  xfmModifiers[0] = DBP_BullZode_offset_XM;
};
datablock afxEffectGroupData(DBP_BullZodeA_shimmer_EG)
{
  assignIndices = true;
  count = 20;
  addEffect = DBP_BullZodeA_shimmer_00_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// shimmering solid magenta bull symbol
//
datablock afxZodiacData(DBP_BullZodeB_CE : DBP_BullZode_CE)
{
  texture = %mySpellDataPath @ "/DBP/zodiacs/dbp_color_bull_b";
  color = "1 1 1 0.3";
};
datablock afxEffectWrapperData(DBP_BullZodeB_shimmer_00_EW)
{
  effect = DBP_BullZodeB_CE;
  constraint = "anchor";
  delay = "$$ ##*2.0";
  lifetime = 1;
  fadeInTime = 1;
  fadeOutTime = 1;
  lifetimeBias = "$$ getRandomF(0.7, 1.4)";
  xfmModifiers[0] = DBP_BullZode_offset_XM;
};
datablock afxEffectGroupData(DBP_BullZodeB_shimmer_EG)
{
  assignIndices = true;
  count = 10;
  addEffect = DBP_BullZodeB_shimmer_00_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// this greenish bull symbol pulses
// _n_pulses times.
//
datablock afxZodiacData(DBP_GreenBull_CE : DBP_BullZode_CE)
{  
  texture = %mySpellDataPath @ "/DBP/zodiacs/dbp_green_bull";
  color = "1 1 1 1";
  blend = additive;
};
datablock afxEffectWrapperData(DBP_GreenBull_pulse_00_EW)
{
  effect = DBP_GreenBull_CE;
  constraint = "anchor";
  delay = "$$ 5.75 + ##*0.7";
  fadeInTime = 0.1;
  fadeOutTime = 0.3;
  lifetime = "$$ (## < %%._n_pulses-1) ? 0.8 : 1.3";
  xfmModifiers[0] = DBP_BullZode_offset_XM;
};
datablock afxEffectGroupData(DBP_GreenBull_pulse_EG)
{
  assignIndices = true;
  count = "$$ %%._n_pulses";
  addEffect = DBP_GreenBull_pulse_00_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// DREAD BULL PORTENT EFFECTRON
//   This is the main part of the Dread Bull Portent element
//   which is implemented using an effectron.
//
datablock afxEffectronData(DreadBullPortentEffectron)
{
  echoPacketUsage = 20;
  execOnNewClients = true;

  addEffect = DBP_CharredOutline_EW;    
  addEffect = DBP_BullZode_EW;
  addEffect = DBP_BullZodeA_shimmer_EG;
  addEffect = DBP_BullZodeB_shimmer_EG;
  addEffect = DBP_GreenBull_pulse_EG;

  addEffect = DBP_BullGlowZode_EW;
  addEffect = DBP_BullHotZode_EW;

    // param defaults //
  _n_pulses = 7;
  _shake = 0;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// DREAD BULL PORTENT SPELL
//   This is a simple spell front-end to the main effectron,
//   DreadBullPortentEffectron. It's purpose is to place a 
//   free-targeting demo of Dread Bull Portent in the spellbank
//   interface.
//
datablock afxMagicSpellData(DreadBullPortentSpell)
{
  castingDur = 0;
};

datablock afxRPGMagicSpellData(DreadBullPortentSpell_RPG)
{
  spellName = "Dread Bull Portent";
  desc = "Tags the ground with a glowing bull symbol." @
         "\n\n" @
         "Uses a special wisp-like free-targeting selectron when available." @
         "\n\n" @
         "[reusable element]";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/MoK/icons/sms";
  manaCost = 10;
  castingDur = 0;
  target = "free";
  freeTargetStyle = 1;
};

function DreadBullPortentSpell::onLaunch(%this, %spell, %caster, %target, %missile)
{
  Parent::onLaunch(%this, %spell, %caster, %target, %missile);

  %anchor = %spell.freeTarget SPC getWords(%caster.getTransform(), 3);

  %effectron = startEffectron(DreadBullPortentEffectron, %anchor, "anchor");
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
  DreadBullPortentSpell.scriptFile = $afxAutoloadScriptFile;
  DreadBullPortentSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
  {
    addDemoSpellbookSpell(DreadBullPortentSpell, DreadBullPortentSpell_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
