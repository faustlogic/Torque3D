
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// UP IN FLAMES -- [reusable element]
//
// Instantly engulfs a target player in fire.
//
//    parameters:
//      _dur
//      _damage
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
$spell_reload = isObject(UpInFlamesSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = UpInFlamesSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;

  afxExecPrerequisite("sp2_shared_fx.cs", $afxAutoloadScriptFolder);
}

//~~~~~~~~~~~~~~~~~~~~//
// Victim Animation

datablock afxAnimClipData(UIF_Victim_Clip_CE)
{
  clipName = "uif_hit";
  ignoreCorpse = true;
  rate = 1.0;
};
datablock afxEffectWrapperData(UIF_Victim_Clip_EW)
{
  effect = UIF_Victim_Clip_CE;
  constraint = "victim";
  lifetime = "$$ %%._dur";
  delay = 0.0;
};

//~~~~~~~~~~~~~~~~~~~~//
// Victim Fire

datablock ParticleData(UIF_Victim_Fire_A_P : SP2_Fire_A_P)
{
  sizes[0] = 3.0;
};
datablock ParticleData(UIF_Victim_Fire_B_P : SP2_Fire_B_P)
{
  sizes[0] = 3.0;
};
datablock ParticleEmitterData(UIF_Victim_Fire_E)
{
  ejectionPeriodMS = 10;
  periodVarianceMS = 4;
  ejectionVelocity = 4;
  velocityVariance = 4;
  thetaMin = 0.0;
  thetaMax = 0.0;
  fadeSize = true;
  blendStyle = "PREMULTALPHA";
  particles = "UIF_Victim_Fire_A_P UIF_Victim_Fire_B_P";
};

datablock afxXM_AimData(UIF_Victim_Fire_Aim_XM)
{
  aimZOnly = true;
};
datablock afxXM_VelocityOffsetData(UIF_Victim_Fire_velocityoffset_XM)
{
  offsetFactor = 0.10;
  offsetPos2 = false;
  normalize = false;
};
datablock afxXM_OscillateData(UIF_Victim_Fire_oscX_XM)
{
  mask = $afxXfmMod::POS;
  min   = "-0.1 0 0"; 
  max   = "0.1 0 0"; 
  speed = 17;
};
datablock afxXM_OscillateData(UIF_Victim_Fire_oscY_XM)
{
  mask = $afxXfmMod::POS;
  min   = "0 -0.2 0"; 
  max   = "0 0.4 0";
  speed = 20;
};
datablock afxXM_OscillateData(UIF_Victim_Fire_oscZ_XM)
{
  mask = $afxXfmMod::POS;
  min   = "0 0 -0.1"; 
  max   = "0 0 0.3"; 
  speed = 12;
};

datablock afxEffectWrapperData(UIF_Victim_Fire_EW)
{
  effect = UIF_Victim_Fire_E;
  constraint = "victim.Bip01 Pelvis";
  delay = 0;
  lifetime = "$$ %%._dur";
  fadeInTime = 0.25;
  fadeOutTime = 0.25;
  xfmModifiers[0] = UIF_Victim_Fire_velocityoffset_XM;
  xfmModifiers[1] = UIF_Victim_Fire_Aim_XM;
  xfmModifiers[2] = UIF_Victim_Fire_oscX_XM;
  xfmModifiers[3] = UIF_Victim_Fire_oscY_XM;  
  xfmModifiers[4] = UIF_Victim_Fire_oscZ_XM;
};

//~~~~~~~~~~~~~~~~~~~~//
// Victim Damage

datablock afxDamageData(UIF_Victim_DOT_CE)
{
  label = "rof_dot";
  flavor = "fire";
  directDamage = 7;
  directDamageRepeats = "$$ mFloor(%%._dur) + 1";
};
datablock afxEffectWrapperData(UIF_Victim_DOT_EW)
{
  effect = UIF_Victim_DOT_CE;
  posConstraint = "victim";
  posConstraint2 = "victim"; 
  lifetime = "$$ %%._dur";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/uif_lighting_tge_sub.cs");
    exec("./audio/uif_audio_sub.cs");
  case "TGEA":
    exec("./lighting/uif_lighting_tgea_sub.cs");
    exec("./audio/uif_audio_sub.cs");
 case "T3D":
    exec("./lighting/uif_lighting_t3d_sub.cs");
    exec("./audio/uif_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// UP IN FLAMES EFFECTRON

datablock afxEffectronData(UpInFlamesEffectron)
{
  execOnNewClients = true;

  addEffect = UIF_Victim_Clip_EW;
  addEffect = UIF_Victim_Fire_EW;
  addEffect = UIF_Victim_DOT_EW;

    // param defaults //
  _dur = 10.0;
  _damage = 7.0;
};

// sounds and lights added via sub-script functions //
UIF_add_Lighting_FX(UpInFlamesEffectron);
UIF_add_Audio_FX(UpInFlamesEffectron);

//~~~~~~~~~~~~~~~~~~~~//
// Callouts

function UpInFlamesEffectron::onDamage(%this, %spell, %label, %flavor, %damaged_obj, %amount, 
                                       %count, %pos, %ad_amount, %radius, %impulse)
{
  // deal the damage
  if (isObject(%damaged_obj) && (%damaged_obj.getType() & $TypeMasks::PlayerObjectType))
    %damaged_obj.damage(%spell, %pos, %amount, %flavor);
}

function UpInFlamesEffectron::onDeactivate(%this, %effectron)
{
  %effectron.uif_victim.uif_fire = "";
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// UP IN FLAMES SPELL
//

datablock afxMagicSpellData(UpInFlamesSpell)
{
  castingDur = 0;
};

datablock afxRPGMagicSpellData(UpInFlamesSpell_RPG)
{
  spellName = "Up In Flames";
  desc = "Instantly engulfs a target player in fire." @
         "\n\n" @
         "[reusable element]";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/UIF/icons/uif";
  manaCost = 10;
  castingDur = 0;
  target = "enemy";
  canTargetSelf = true;
};

function UpInFlamesSpell::onLaunch(%this, %spell, %caster, %target, %missile)
{
  Parent::onLaunch(%this, %spell, %caster, %target, %missile);
  %effectron = startEffectron(UpInFlamesEffectron, %target, "victim");

  //UAISK+AFX Interop Changes: Start
  if ($UAISK_Is_Available && %target.isBot)
    confuseBot(%target, $Sim::Time + UpInFlamesEffectron._dur + 0.4, %target.detDis);
  //UAISK+AFX Interop Changes: End
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// FIERY POTATO SPELL
//

datablock afxMagicSpellData(FieryPotatoSpell)
{
  castingDur = 0;
};

datablock afxRPGMagicSpellData(FieryPotatoSpell_RPG)
{
  spellName = "Fiery Potato";
  desc = "Engulfs a target player in fire, then targets another randomly, then another, and so on..." @
         "\n\n" @
         "[experimental spell]";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/UIF/icons/uif";
  manaCost = 10;
  castingDur = 0;
  target = "enemy";
  canTargetSelf = true;
};

function remapFieryPotato(%effectron)
{
  if (!isObject(%effectron))
    return;

  %n_inside = 0;
  InitContainerRadiusSearch(%effectron.uif_pos, 200, $TypeMasks::PlayerObjectType);
  while ((%in_obj = containerSearchNext()) != 0) 
  {
    %inside[%n_inside] = %in_obj;
    %n_inside++;
  }

  %victim = %effectron.uif_last_victim;
  while (%victim == %effectron.uif_last_victim)
  {
    %idx = getRandom(0,%n_inside-1);
    %victim = %inside[%idx];
  }

  %effectron.uif_last_victim = %victim;
  %effectron.remapConstraint(%victim, "victim");

  schedule(1000, 0, remapFieryPotato, %effectron);
}

function FieryPotatoSpell::onLaunch(%this, %spell, %caster, %target, %missile)
{
  Parent::onLaunch(%this, %spell, %caster, %target, %missile);

  %effectron = new afxEffectron() 
  {
    datablock = UpInFlamesEffectron;
    uif_pos = %caster.getPosition();
    uif_last_victim = %target;
    _dur = 20.0;
    _damage = 50.0;
  };
  %effectron.addConstraint(%target, "victim");

  schedule(1000, 0, remapFieryPotato, %effectron);
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
  UpInFlamesSpell.scriptFile = $afxAutoloadScriptFile;
  UpInFlamesSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
  {
    addDemoSpellbookSpell(UpInFlamesSpell, UpInFlamesSpell_RPG);
    addDemoSpellbookSpell(FieryPotatoSpell, FieryPotatoSpell_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
