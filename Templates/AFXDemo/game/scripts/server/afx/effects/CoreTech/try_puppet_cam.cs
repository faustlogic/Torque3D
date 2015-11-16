
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// TRY PUPPET CAM
//
//    Experimental spell that attaches camera to a target looking back at the caster. 
//    Cast again to end the effect. A target is required to turn on the effect, but
//    target is optional when shutting it off.
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
$spell_reload = isObject(TryPuppetCamSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = TryPuppetCamSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

// this offset controls camera height above target
datablock afxXM_LocalOffsetData(PUPCAM_offset_XM)
{
  localOffset = "0.0 0.0 2.8";
};

// used to aim back at caster
datablock afxXM_AimData(PUPCAM_aim_XM)
{
  aimZOnly = false;
};

// this offset occurs after the aim to pull back the
// camera a little bit
datablock afxXM_LocalOffsetData(PUPCAM_offset2_XM)
{
  localOffset = "0.0 -4.0 0.0";
};

datablock afxCameraPuppetData(PUPCAM_Mover_CE)
{
  cameraSpec = "camera";
  //networking = $AFX::SERVER_AND_CLIENT;
  networking = $AFX::CLIENT_ONLY;
};
//
datablock afxEffectWrapperData(PUPCAM_Mover_CW)
{
  effect = PUPCAM_Mover_CE;
  posConstraint = "target";
  posConstraint2 = "caster.#center";
  xfmModifiers[0] = PUPCAM_offset_XM;
  xfmModifiers[1] = PUPCAM_aim_XM;
  xfmModifiers[2] = PUPCAM_offset2_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// spell

datablock afxMagicSpellData(TryPuppetCamSpell)
{
  lingerDur = $AFX::INFINITE_TIME;
  allowMovementInterrupts = false;

  addLingerEffect = PUPCAM_Mover_CW;
};
//
datablock afxRPGMagicSpellData(TryPuppetCamSpell_RPG)
{
  spellName = "Try Puppet Cam";
  desc = "Experimental spell that attaches camera to a target looking back at the caster. " @ 
         "Cast again to end the effect. A target is required to turn on the effect, " @
         "but target is optional when shutting it off." @
         "\n\n" @
         "[experimental effect]"; 
  sourcePack = "Core Tech";
  target = "enemy";
  targetOptional = true; // allows effect shutoff even without a selection
  manaCost = 10;
  castingDur = TryPuppetCamSpell.castingDur;
};

// if spell is active on client, kill it instead of casting it
function TryPuppetCamSpell::readyToCast(%this, %caster, %target)
{
  %client = %caster.client;
  if (isObject(%caster.client.puppet_cam))
  {
    %caster.client.puppet_cam.interruptStage();
    %caster.client.puppet_cam = "";
    return false;
  }

  return true;
}

function TryPuppetCamSpell::onActivate(%this, %spell, %caster, %target)
{
  Parent::onActivate(%this, %spell, %caster, %target);
  // setup an explicit client
  %spell.addExplicitClient(%caster.client);
}

function TryPuppetCamSpell::onImpact(%this, %spell, %caster, %impObj, %impPos, %impNorm)
{
  if (isObject(%impObj))
  {
    %caster.client.puppet_cam = %spell;
  }
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
  TryPuppetCamSpell.scriptFile = $afxAutoloadScriptFile;
  TryPuppetCamSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(TryPuppetCamSpell, TryPuppetCamSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

