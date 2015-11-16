
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SOUL MINE SOLO
//
//  Maybe you can't sing, but if you've got soul, or at least some Soul Substance,
//  bury it CAREFULLY and then leave it for someone else to trip over.
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
$spell_reload = isObject(SoulMineSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = SoulMineSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;

  afxExecPrerequisite("clusters_of_fire.cs", $afxAutoloadScriptFolder);
  afxExecPrerequisite("soul_nuke.cs", $afxAutoloadScriptFolder);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxZodiacData(SM_MineZodiacGlow_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/SM/zodiacs/sm_glowing_symbol";
  radius = 2.0;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1 1 1 1";
  blend = additive;
  trackOrientConstraint = true;
  interiorHorizontalOnly = true;
};

datablock afxZodiacData(SM_MineZodiacBlack_CE : SM_MineZodiacGlow_CE)
{  
  texture = %mySpellDataPath @ "/SM/zodiacs/sm_black_symbol";
  blend = normal;
};

datablock afxXM_SpinData(SM_MineZodiacGlow_spinA_XM)
{
  spinAxis = "0 0 1";
  spinRate = 300;
  spinAngle = -300*0.3;
  fadeOutTime = 0;
  lifetime = 0.3;
};

datablock afxEffectWrapperData(SM_MineZodiacGlow_EW)
{
  effect = SM_MineZodiacGlow_CE;
  constraint = "mine";
  
  delay = 0;
  fadeInTime = 0.2;
  fadeOutTime = 0.4;
  lifetime = 0.3;
  
  xfmModifiers[0] = SM_MineZodiacGlow_spinA_XM;
};

datablock afxEffectWrapperData(SM_MineZodiacBlack_EW)
{
  effect = SM_MineZodiacBlack_CE;
  constraint = "mine";
  delay = 0.3;
  fadeInTime = 0.5;
  fadeOutTime = 1.0;
};

// tmp...
datablock afxScriptEventData(SM_BurySoulMine_Script_CE)
{
  methodName = "BurySoulMine";
};
//
datablock afxEffectWrapperData(SM_BurySoulMine_Script_EW)
{
  effect = SM_BurySoulMine_Script_CE;
  constraint = "mine";
  delay = 1.0;  
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock TriggerData(SoulMineTrigger)
{
   tickPeriodMS = 1000;
};

function SoulMineTrigger::onEnterTrigger(%this, %trigger, %obj)
{
  %xfm = %trigger.getTransform();

  %mine_trip = startEffectron(SoulMineTripEffectron, %xfm, "mine");

  %exp_scale = getRandomF(0.8, 2.5);
  schedule(getRandom(100,600), 0, tripSoulNuke, %xfm, %exp_scale);

  %n_fires = mCeil(((%exp_scale-0.8)/(2.5-0.8))*10);
  %radius = 6.0*%exp_scale;

  %cluster_fires = startClustersOfFire(%xfm, %n_fires, %radius);

  if (isObject(%trigger.soulMineEffectron))
  {
    %trigger.soulMineEffectron.interrupt();
    %trigger.soulMineEffectron = "";
  }

  %trigger.schedule(500, "delete");
}

function tripSoulNuke(%xfm, %exp_scale)
{
  %mine_explosion = startEffectron(SoulNukeEffectron, %xfm, "mine");
  %mine_explosion._expScale = %exp_scale;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SOUL MINE EFFECTRON
//
datablock afxEffectronData(SoulMineEffectron)
{
  echoPacketUsage = 20;
  duration = -1;
  numLoops = 1;
  execOnNewClients = true;
  
    // mine zodiacs //
  addEffect = SM_MineZodiacGlow_EW;
  addEffect = SM_MineZodiacBlack_EW;
  addEffect = SM_BurySoulMine_Script_EW;
};

function SoulMineEffectron::BurySoulMine(%this, %soul_mine, %cons, %xfm, %data)
{
  %mine_trigger = new Trigger() 
  {
    datablock = "SoulMineTrigger";
    position = getWords(%xfm, 0,2);
    scale = "2 2 2";
    polyhedron = "-0.5 0.5 -0.5" SPC "1 0 0" SPC "0 -1 0" SPC "0 0 1";
  };

  %mine_trigger.soulMineEffectron = %soul_mine;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SOUL MINE TRIP EFFECTRON
//
datablock afxEffectronData(SoulMineTripEffectron)
{
  addEffect = SN_LaserWave_L2_SND_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SOUL MINE SPELL
//

datablock afxMagicSpellData(SoulMineSpell)
{
  castingDur = 0;
};

datablock afxRPGMagicSpellData(SoulMineSpell_RPG)
{
  spellName = "Soul Mine Solo";
  desc = "Maybe you can't sing, but if you've got soul, or at least some Soul Substance, " @
         "bury it CAREFULLY and then leave it for someone else to trip over.\n" @
         "\n" @
         "<font:Arial Italic:14>spell design: <font:Arial:14>Matthew Durante\n" @
         "<font:Arial Italic:14>sound effects: <font:Arial:14>Matt Pacyga\n" @ 
         "<font:Arial Italic:14>spell concept: <font:Arial:14>Matthew Durante and Jeff Faust";  
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/SM/icons/sm";
  manaCost = 10;
  castingDur = 0;
  target = "free";
};

function SoulMineSpell::onLaunch(%this, %spell, %caster, %target, %missile)
{
  Parent::onLaunch(%this, %spell, %caster, %target, %missile);

  %anchor = %spell.freeTarget SPC getWords(%caster.getTransform(), 3);

  %effectron = startEffectron(SoulMineEffectron, %anchor, "mine");
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
  SoulMineSpell.scriptFile = $afxAutoloadScriptFile;
  SoulMineSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
  {
    addDemoSpellbookSpell(SoulMineSpell, SoulMineSpell_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
