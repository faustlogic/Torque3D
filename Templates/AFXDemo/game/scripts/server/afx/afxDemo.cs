
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// Functions for AFX Demo
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

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// demo spellbook management

$Book_Max_Spells = 12*12;
$Book_First_User_Slot = 3*12; // set to 0 for AFXDemo Lite Template

function loadDemoSpellbook(%book)
{
  for (%i = 0; %i < $Book_Max_Spells; %i++)
  {
    if ($Book_Rpg[%i] !$= "")
    {
      %book.rpgSpells[%i] = $Book_Rpg[%i];
      %book.spells[%i] = $Book_Rpg[%i].spellFXData;
    }
  }

  deleteVariables("$Book_Rpg*");
}

function clearDemoSpellbookData()
{
  deleteVariables("$Book_PageNames*");
}

function setDemoSpellbookBankName(%page, %name)
{
  $Book_PageNames[%page] = %name;
}

function addDemoSpellbookPlaceholder(%placeholder, %page, %slot)
{
  if (%placeholder.getClassName() !$= "afxRPGMagicSpellData")
    return;

  %placeholder.isPlaceholder = true;

  %idx = %page*12 + %slot;
  if (%idx >= 0 && %idx < $Book_Max_Spells)
  {
    $Book_Rpg[%idx] = %placeholder;
  }
}

function addDemoSpellbookSpell(%spell_db, %rpg_db)
{
  if (!isObject(%spell_db) || !isObject(%rpg_db))
    return;

  %spell_name = %rpg_db.spellName;
  if (%spell_name $= "")
    return;

  // find a placeholder with a matching name
  %idx = -1;
  %cap = 12*12;
  for (%i = 0; %i  < $Book_Max_Spells; %i++)
  {
    if ($Book_Rpg[%i] !$= "" && $Book_Rpg[%i].spellName $= %spell_name && $Book_Rpg[%i].isPlaceholder)
    {
      %idx = %i;
      break;
    }
  }

  // if placeholder was found, replace it
  if (%idx >= 0)
  {
    $Book_Rpg[%idx] = %rpg_db;
    %rpg_db.spellFXData = %spell_db;
    return;
  }

  // if no placeholder was found, use first empty slot
  // beginning with 4th spellbank.
  %idx = -1;
  for (%i = $Book_First_User_Slot; %i  < $Book_Max_Spells; %i++)
  {
    if ($Book_Rpg[%i] $= "")
    {
      %idx = %i;
      break;
    }
  }

  // if empty slot was found, insert spell there
  if (%idx >= 0)
  {
    $Book_Rpg[%idx] = %rpg_db;
    %rpg_db.spellFXData = %spell_db;
    return;
  }

  error("Demo Spellbook is full. Cannot add spell, \"" @ %spell_name @ "\".");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

// demo sci-fi mode

function enableSciFiMode(%client)
{
  if (%client.player != 0)
  {
    echo("Enable Sci-Fi Mode");
    if (isFunction(equipSciFiOrc))
      equipSciFiOrc(%client.player);
    serverCmdSetSelectronStyle(%client, "SCI-FI", false);
  }
}

function disableSciFiMode(%client)
{
  if (%client.player != 0)
  {
    echo("Disable Sci-Fi Mode");
    if (isFunction(unequipSciFiOrc))
      unequipSciFiOrc(%client.player);
    serverCmdSetSelectronStyle(%client, "AFX Default", false);
  }
}

function serverCmdOnSpellbankChange(%client, %old_bank, %new_bank)
{
  %spellbook = afxDemoSpellBookData;

  if (!isObject(%spellbook))
  {
    error("serverCommand('onSpellbankChange'): failed to find spellbook.");
    return;
  }


  if ($Book_PageNames[%new_bank] !$= "")
    commandToClient(%client, 'DisplayScreenMessage', $Book_PageNames[%new_bank], clear);
  else
    commandToClient(%client, 'DisplayScreenMessage', "Spellbank" SPC %new_bank, clear);

  if ($Book_PageNames[%old_bank] $= "SCI-FI Mode")
    disableSciFiMode(%client);

  if ($Book_PageNames[%new_bank] $= "SCI-FI Mode")
    enableSciFiMode(%client);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

function GameConnection::onDeathAFX(%this, %sourceObject, %sourceClient, %damageType, %damLoc)
{
  // clear connections camera
  %this.clearCameraObject();

  if (isObject(%this.player))
  {
    %corpse = %this.player;

    // interrupt active spellcasting
    if (isObject(%corpse.spellBeingCast))
    {
      %spell = %corpse.spellBeingCast;
      %corpse.spellBeingCast = "";
      %spell.interrupt();
    }

    // clear out the name on the corpse
    %corpse.setShapeName("Dead Orc");
    schedule(1000, 0, afxBroadcastTargetStatusbarReset);

    // zero out energy level
    %corpse.setEnergyLevel(0);

    // switch the client over to the death cam and unhook the player object.
    if (isObject(%this.camera))
    {
      %this.camera.setOrbitMode(%corpse, %corpse.getTransform(), 0.5, 4.5, 4.5);
      %this.setControlObject(%this.camera);
    }
  }

  // Dole out points and display an appropriate message
  if (%damageType $= "Suicide" || %sourceClient == %this)
  {
    %this.incScore(-1);
    messageAll('MsgClientKilled', '%1 takes his own life!', %this.name);
  }
  else if (%sourceClient !$= "") //AFX Patch Update
  {
    %sourceClient.incScore(1);
    messageAll('MsgClientKilled', '%1 gets nailed by %2!', %this.name, %sourceClient.name);
    if (%sourceClient.score >= $Game::EndGameScore)
      cycleGame();
  }
}

function afxBroadcastTargetStatusbarReset()
{
  %count = ClientGroup.getCount();
  for (%i = 0; %i < %count; %i++)
  {
    %cl = ClientGroup.getObject(%i);
    if( !%cl.isAIControlled() )
      commandToClient(%cl, 'ResetTargetStatusbarLabel');
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
