
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// TRY FLYING
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
$spell_reload = isObject(TryFlying);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = TryFlying.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxXM_AimData(TFLY_aim_XM)
{
  aimZOnly = false;
};

datablock afxPhysicalZoneData(TFLY_PhysZone_CE)
{
  velocityMod = "1";
  gravityMod = "0.2";
  appliedForce = "0 -400 300";
  orientForce = true;
  polyhedron = " 0.0000000  0.0000000 0.0000000" SPC
               " 1.0000000  0.0000000 0.0000000" SPC
               " 0.0000000  -1.0000000 0.0000000" SPC
               " 0.0000000  0.0000000 1.0000000";
};
//
datablock afxEffectWrapperData(TFLY_PhysZone_EW)
{
  effect = TFLY_PhysZone_CE;
  posConstraint = "target";
  orientConstraint = "#effect.Caster_Mooring";
};

datablock afxMooringData(TFLY_Mooring_CE)
{
  displayAxisMarker = false;
};
//
datablock afxEffectWrapperData(TFLY_Mooring_EW)
{
  effect = TFLY_Mooring_CE;
  effectName = "Caster_Mooring";
  isConstraintSrc = true;
  posConstraint = "target.Eye";
  posConstraint2 = "clientCamera";
  xfmModifiers[0] = TFLY_aim_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// spell

datablock afxMagicSpellData(TryFlying)
{
  lingerDur = $AFX::INFINITE_TIME;
  execOnNewClients = true;
  allowMovementInterrupts = false;

  addLingerEffect = TFLY_Mooring_EW;
  addLingerEffect = TFLY_PhysZone_EW;
};
//
datablock afxRPGMagicSpellData(TryFlying_RPG)
{
  spellName = "Try Flying";
  desc = "This experimental spell uses a PhysicalZone effect for controlled FLIGHT. " @
         "Cast this spell, then jump to get started. STEER using the third-person camera.\n\n" @
         "[experimental effect]"; 
  sourcePack = "Core Tech";
  iconBitmap = %mySpellDataPath @ "/TFLY/icons/tfly";
  target = "self";
  manaCost = 10;
  castingDur = TryFlying.castingDur;
};

function TryFlying::onImpact(%this, %spell, %caster, %impObj, %impPos, %impNorm)
{
  if (isObject(%impObj))
  {
    if (isObject(%impObj.tfly_spell))
    {
      // note - this interrupts both the new instance and the old
      %spell.interrupt();
      %impObj.tfly_spell.interrupt();
      %impObj.tfly_spell = "";
    }
    else
    {
      %impObj.tfly_spell = %spell;
    }
  }
}

function TryFlying::onActivate(%this, %spell, %caster, %target)
{
  Parent::onActivate(%this, %spell, %caster, %target);

  %client_camera = %caster.client.getCameraObject();
  if (!isObject(%client_camera))
     error("FAILED to find spellcaster's camera");
  else
     %spell.addConstraint(%client_camera, "clientCamera");
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
  TryFlying.scriptFile = $afxAutoloadScriptFile;
  TryFlying.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(TryFlying, TryFlying_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

