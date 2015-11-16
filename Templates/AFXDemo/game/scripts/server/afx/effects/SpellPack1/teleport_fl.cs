
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// TELEPORT TO FAUSTLOGIC.COM SPELL
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
$MIN_REQUIRED_VERSION = 1.12;

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
$spell_reload = isObject(TeleportFLSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = TeleportFLSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxAnimClipData(TFL_SummonClip_CE)
{
  clipName = "summon";
  rate = 2.0;
};
//
datablock afxEffectWrapperData(TFL_SummonClip_EW)
{
  effect = TFL_SummonClip_CE;
  constraint = "caster";
};

datablock afxZodiacData(TFL_Zode_CE : SHARED_ZodiacBase_CE)
{
  texture = %mySpellDataPath @ "/TFL/zodiacs/fl_logo_zode";
  radius = 2.5;
  startAngle = 0;
  rotationRate = 0.0;
  color = "1.0 0.3 0.0 0.8";
};
datablock afxEffectWrapperData(TFL_Zode_EW)
{
  effect = TFL_Zode_CE;
  constraint = caster;
  xfmModifiers[0] = SHARED_AltitudeConform_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// spell

datablock afxMagicSpellData(TeleportFLSpell)
{
  allowMovementInterrupts = false;
  castingDur = 1.0;

  addCastingEffect = TFL_SummonClip_EW;
  addCastingEffect = TFL_Zode_EW;
};
//
datablock afxRPGMagicSpellData(TeleportFLSpell_RPG)
{
  spellName = "Teleport to FaustLogic.com";
  desc = "Open browser to the Faust Logic homepage.\n\n[novelty spell]"; 
  sourcePack = "Spell Pack 1";
  iconBitmap = %mySpellDataPath @ "/TFL/icons/tfl";
  target = "nothing";
  manaCost = 0;
  castingDur = TeleportFLSpell.castingDur;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// scripting

function TeleportFLSpell::onImpact(%this, %spell, %caster, %impObj, %impPos, %impNorm)
{
  if (%caster.client)
    commandToClient(%caster.client, 'OpenWebPage', "Teleport to FaustLogic.com", 
                    "Visit Faust Logic website?", "http://www.faustlogic.com");
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
  TeleportFLSpell.scriptFile = $afxAutoloadScriptFile;
  TeleportFLSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(TeleportFLSpell, TeleportFLSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//


