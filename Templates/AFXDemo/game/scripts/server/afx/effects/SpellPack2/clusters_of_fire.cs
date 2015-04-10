
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// CLUSTERS OF FIRE -- [reusable element]
//
// Ignites a scattering of small spot fires with tendrils of smoke.
//
// JTF Notes:
//    -- look into replacing oscillator xmods with wave xmods
//    -- needs some sound... probably just some subtle fire sizzle
//
//    instantiation function:
//      startClustersOfFire(%anchor, %count, %opaque_data, %pickPosFunc)
//              %anchor -- a contraint to anchor the fires to
//               %count -- the number of fires
//         %opaque_data -- range value or argument blindly passed as 3rd arg to %pickPosFunc 
//         %pickPosFunc -- optional function for choosing fire locations func(%count, %i, %opaque_data)
//          
//    parameters:
//      _n          (integer)       default=10
//
//    internal:
//      _firePos[##]
//      c_fireDelay[##]
//      c_fireLifetime[##]
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
$spell_reload = isObject(ClustersOfFireSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = ClustersOfFireSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;

  afxExecPrerequisite("sp2_shared_fx.cs", $afxAutoloadScriptFolder);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GROUND FIRES

datablock afxXM_LocalOffsetData(COF_offset_00_XM)
{
  localOffset = "$$ %%._firePos[##]";
};

datablock afxXM_GroundConformData(COF_height_XM)
{
  height = 0.1;
};

datablock afxEffectWrapperData(COF_Fire_00_EW)
{
  effect = SP2_GroundFire_RND_E; // shared fire emitter
  constraint = "anchor";
  effectName = "$$ Fire##";
  isConstraintSrc = true;
  delay = "$$ %%.c_fireDelay[##]";
  fadeInTime = 1.0;
  fadeOutTime = 1.0;
  lifetime = "$$ %%.c_fireLifetime[##]";
  xfmModifiers[0] = COF_offset_00_XM;
  xfmModifiers[1] = COF_height_XM;
};

datablock afxXM_LocalOffsetData(COF_Tendril_offset_XM)
{
  localOffset = "0 0 0.2";
};
datablock afxXM_OscillateData(COF_Tendril_waver_XM)
{
  mask = $afxXfmMod::ORI;
  axis  = "$$ getRandomDir(\"0 0 1\", 90, 90)";
  min   = "$$ getRandomF(-80.0,0)";
  max   = "$$ getRandomF(0,80.0)";
  speed = "$$ getRandomF(0.3, 8.0)";
};
datablock afxEffectWrapperData(COF_Tendril_A_00_EW)
{
  effect = SP2_TendrilSmokeRND_E;
  constraint = "$$ \"#effect.Fire##\"";
  delay = "$$ %%.c_fireDelay[##] + 0.3";
  lifetime = "$$ %%.c_fireLifetime[##]";
  fadeInTime = 1.0;
  fadeOutTime = 1.0;
  xfmModifiers[0] = COF_Tendril_offset_XM;
  xfmModifiers[1] = COF_Tendril_waver_XM;
  xfmModifiers[2] = COF_Tendril_waver_XM;
};
datablock afxEffectWrapperData(COF_Tendril_B_00_EW : COF_Tendril_A_00_EW)
{
  xfmModifiers[0] = COF_Tendril_offset_XM;
  xfmModifiers[1] = COF_Tendril_waver_XM;
  xfmModifiers[2] = COF_Tendril_waver_XM;
  xfmModifiers[3] = COF_Tendril_waver_XM;
};

datablock afxEffectWrapperData(COF_Tendril_C_00_EW : COF_Tendril_A_00_EW)
{
  effectEnabled = "$$ (getRandom(1,3) == 1)";
};

datablock afxXM_OscillateData(COF_GroundFireBounce_00_XM)
{
  mask = $afxXfmMod::SCALE;
  axis  = "1 0 0";
  min = "$$ getRandomF(0.65,0.8)";
  max = "$$ getRandomF(0.85,1.2)";
  speed = "$$ getRandomF(10.0,20.0)";
};
datablock afxZodiacData(COF_GroundFireBounceZode_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/CoF/zodiacs/cof_fire_bounce";
  radius = 0.75;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1 1 1 1";
  blend = additive;
};
datablock afxEffectWrapperData(COF_FireBounce_00_EW)
{
  effect = COF_GroundFireBounceZode_CE;
  constraint = "$$ \"#effect.Fire##\"";
  delay = "$$ %%.c_fireDelay[##] + 0.3";
  lifetime = "$$ %%.c_fireLifetime[##]";
  fadeInTime = 1.0;
  fadeOutTime = 1.0;
  xfmModifiers[0] = COF_GroundFireBounce_00_XM;
  xfmModifiers[1] = COF_GroundFireBounce_00_XM;
};

// _n, c_fireDelay[##], c_fireLifetime[##], _firePos[##]
datablock afxEffectGroupData(COF_GroundFire_EG)
{
  assignIndices = true;
  count = "$$ %%._n";
  addEffect = COF_Fire_00_EW;
  addEffect = COF_Tendril_A_00_EW;
  addEffect = COF_Tendril_B_00_EW;
  addEffect = COF_Tendril_C_00_EW;
  addEffect = COF_FireBounce_00_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CLUSTERS OF FIRE EFFECTRON
//
datablock afxEffectronData(ClustersOfFireEffectron)
{
  echoPacketUsage = 20;
  clientScriptFile = %mySpellDataPath @ "/CoF/cof_client.cs";
  clientInitFunction = "CoF_clientInit";
  duration = 1.0;
  numLoops = 1;
  addEffect = COF_GroundFire_EG;

    // default parameter settings //
  _n = 10; 
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CLUSTERS OF FIRE SPELL
//    a simple spell that allows placing of clusters using
//    a free-targeting selectron.
//

datablock afxMagicSpellData(ClustersOfFireSpell)
{
  castingDur = 0;
};

datablock afxRPGMagicSpellData(ClustersOfFireSpell_RPG)
{
  spellName = "Clusters of Fire";
  desc = "Ignites a scattering of small spot fires with tendrils of smoke." @
         "\n\n" @
         "[reusable element]";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/CoF/icons/cof";
  manaCost = 10;
  castingDur = 0;
  target = "free";
};

function ClustersOfFireSpell::onLaunch(%this, %spell, %caster, %target, %missile)
{
  Parent::onLaunch(%this, %spell, %caster, %target, %missile);

  // creates an anchor transform using target position + caster orientation
  %anchor = %spell.freeTarget SPC getWords(%caster.getTransform(), 3);

  startClustersOfFire(%anchor, getRandom(8,14), getRandomF(1.0,6.0));
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// INSTANTIATION FUNCTION

function startClustersOfFire(%anchor, %count, %opaque_data, %pickPosFunc)
{
  if (%pickPosFunc $= "")
    %pickPosFunc = CoF_pickPosFunc;

  %effectron = new afxEffectron() 
  {
    datablock = ClustersOfFireEffectron;
    _n = %count;
  };

  for (%i = 0; %i < %count; %i++)
  {
    %effectron.c_fireDelay[%i] = 1.2;
    %effectron.c_fireLifetime[%i] = 15;
    %effectron._firePos[%i] = call(%pickPosFunc, %count, %i, %opaque_data);
  }

  %effectron.addConstraint(%anchor, "anchor");

  return %effectron;
}

// default scattering callback
function CoF_pickPosFunc(%count, %i, %range)
{
  %rand_dir = VectorScale(VectorNormalize(getRandomDir("0 0 1", 90, 90)), getRandomF(1.0, %range));
  %x = mFloatLength(getWord(%rand_dir, 0), 2);
  %y = mFloatLength(getWord(%rand_dir, 1), 2);
  return %x SPC %y SPC "0";
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
  ClustersOfFireSpell.scriptFile = $afxAutoloadScriptFile;
  ClustersOfFireSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
  {
    addDemoSpellbookSpell(ClustersOfFireSpell, ClustersOfFireSpell_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
