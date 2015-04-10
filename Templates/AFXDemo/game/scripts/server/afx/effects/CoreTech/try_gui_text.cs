
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// TRY GUI TEXT
//
// Experimental effect for testing Gui Text effects. Cast this spell on a
// target to toggle the visibility of its overhead name label.
//
// This spell demonstrates the use of afxGuiText effects in conjunction with the
// afxGuiTextHud, a gui-control that must be defined as part of the active gui.
// 
// afxGuiTextHud works a lot like the stock Torque guiShapeNameHud except the visibility,
// color, and position of individual labels can be managed using afxGuiText effects.
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
$spell_reload = isObject(TryGuiTextSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = TryGuiTextSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// LABEL (gui text)
//

//
// Values for the constraint and the vertical offset are chosen
// to reproduce similar results to the labels shown by the stock
// guiShapeNameHud with the usual settings. 
//

datablock afxXM_WorldOffsetData(GUIT_text_offset_XM)
{
  worldOffset = "0.0 0.0 0.5";
};

//
// #ShapeName is a special token that indicates that the constraint object's
// ShapeName should be used for the text.
// 

datablock afxGuiTextData(GUIT_Label_CE)
{
  text = "#ShapeName";
  color = "$$ getColorFromHSV(getRandom(360),1,1)"; // picks random hue
};

datablock afxEffectWrapperData(GUIT_Label_EW)
{
  effect = GUIT_Label_CE;
  posConstraint = "target.Eye";
  xfmModifiers[0] = GUIT_text_offset_XM;
  fadeInTime = 2;
  fadeOutTime = 2;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// spell

datablock afxMagicSpellData(TryGuiTextSpell)
{
  lingerDur = $AFX::INFINITE_TIME;
  execOnNewClients = true;

  addLingerEffect = GUIT_Label_EW;

  allowMovementInterrupts = false;
};
//
datablock afxRPGMagicSpellData(TryGuiTextSpell_RPG)
{
  spellName = "Try Gui Text";
  desc = "Experimental effect for testing Gui Text effects. " @ 
         "Cast this spell on a target to toggle " @
         "the visibility of its overhead name label." @ 
         "\n\n" @ 
         "Note: The text from this effect appears exclusively on the client " @
         "associated with the caster." @
         "\n\n" @
         "[experimental effect]"; 
  sourcePack = "Core Tech";
  iconBitmap = %mySpellDataPath @ "/GUIT/icons/guit";
  target = "friend";
  canTargetSelf = true;
  manaCost = 10;
  castingDur = TryGuiTextSpell.castingDur;
};

function TryGuiTextSpell::onActivate(%this, %spell, %caster, %target)
{
  Parent::onActivate(%this, %spell, %caster, %target);
  // setup an explicit client
  %spell.addExplicitClient(%caster.client);
}

function TryGuiTextSpell::onImpact(%this, %spell, %caster, %impObj, %impPos, %impNorm)
{
  if (isObject(%impObj))
  {
    %client = %caster.client;
    // check if there is already an active label for this client
    if (isObject(%impObj.guit_label[%client]))
    {
      // abort current spell early since we really want to kill an existing
      // instance of the spell.
      %spell.interrupt();

      // kill existing spell with interruptStage() for a gentle fadeout, rather than
      // a harsh cut-off.
      %impObj.guit_label[%client].interruptStage();
      %impObj.guit_label[%client] = "";
    }
    else
    {
      // attach spell handle to impact object so we can shut it off later with
      // a recast.
      %impObj.guit_label[%client] = %spell;
    }
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
  TryGuiTextSpell.scriptFile = $afxAutoloadScriptFile;
  TryGuiTextSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(TryGuiTextSpell, TryGuiTextSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

