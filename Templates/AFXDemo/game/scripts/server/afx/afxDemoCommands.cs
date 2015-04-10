//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// Some AFX related script commands.
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

// AFX-OVERRIDE -- This function overrides the stock version in commands.cs
// Called from client: commandToServer('ToggleCamera')
function serverCmdToggleCamera(%client)
{
  if (!isObject(%client.camera))
  {
    error("Server command 'ToggleCamera': client camera is undefined.");
    return;
  }

  if (!isObject(%client.player))
  {
    error("Server command 'ToggleCamera': client player is undefined.");
    return;
  }

  if (%client.getControlObject() == %client.player)
  {
    %client.camera.setFlyMode();
    %control = %client.camera;
  }
  else
  {
    %client.camera.setCameraSubject(%client.player); 
    %client.camera.setThirdPersonMode();
    %control = %client.player;
  }
  %client.setControlObject(%control); 

  clientCmdSyncEditorGui();
}

// AFX-OVERRIDE -- This function overrides the stock version in commands.cs
// Called from client: commandToServer('DropPlayerAtCamera')
function serverCmdDropPlayerAtCamera(%client)
{
  if (!isObject(%client.camera))
  {
    error("Server command 'DropPlayerAtCamera': client camera is undefined.");
    return;
  }

  if (!isObject(%client.player))
  {
    error("Server command 'DropPlayerAtCamera': client player is undefined.");
    return;
  }

  if (%client.getControlObject() == %client.player)
  {
    %client.camera.setFlyMode();
    %client.player.setTransform(%client.camera.getTransform());
    %client.player.setVelocity("0 0 0");
    %client.camera.setThirdPersonMode();
  }
  else
  {
    %client.player.setTransform(%client.camera.getTransform());
    %client.player.setVelocity("0 0 0");
    %client.setControlObject(%client.player);
    %client.camera.setThirdPersonMode();
  }

  clientCmdSyncEditorGui();
}

// AFX-OVERRIDE -- This function overrides the stock version in commands.cs
// Called from client: commandToServer('DropCameraAtPlayer')
function serverCmdDropCameraAtPlayer(%client)
{
  if (!isObject(%client.camera))
  {
    error("Server command 'DropCameraAtPlayer': client camera is undefined.");
    return;
  }

  if (!isObject(%client.player))
  {
    error("Server command 'DropCameraAtPlayer': client player is undefined.");
    return;
  }

  if (%client.getControlObject() == %client.player)
  {
    %client.camera.setFlyMode();
    %client.camera.setTransform(%client.player.getEyeTransform());
    %client.camera.setVelocity("0 0 0");
    %client.setControlObject(%client.camera);
  }
  else
  {
    %client.camera.setTransform(%client.player.getEyeTransform());
    %client.camera.setVelocity("0 0 0");
  }

  clientCmdSyncEditorGui();
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// MAIN SPELLCASTING COMMAND FROM CLIENT

// Called from client: commandToServer('CastSpellbookSpell', %book_slot, %target_ghost)
function serverCmdCastSpellbookSpell(%client, %book_slot, %target_ghost)
{
  if (%target_ghost != -1)
    %target = %client.ResolveGhost(%target_ghost);
  else
    %target = -1;

  afxPerformSpellbookCast(%client.player, %book_slot, %target, %client);
}

// Called from client: commandToServer('CastFreeTargetingSpellbookSpell', %book_slot, %free_target)
function serverCmdCastFreeTargetingSpellbookSpell(%client, %book_slot, %free_target)
{
  afxPerformFreeTargetingSpellbookCast(%client.player, %book_slot, %free_target, %client);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// RESPAWN PLAYER COMMAND
//   called by jump key when dead.

// Called from client: commandToServer('RespawnPlayerAfterDeath')
function serverCmdRespawnPlayerAfterDeath(%client)
{
  // clear out old linkages between player and client
  if (isObject(%client.player))
  {
    %client.player.client = "";
    %client.player = "";
  }

  // Find a spawn point for the player
  %playerSpawnPoint = pickPlayerSpawnPoint($Game::DefaultPlayerSpawnGroups);
  // Spawn a player for this client using the found %playerSpawnPoint
  %client.spawnPlayer(%playerSpawnPoint);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AFX CAMERA DOLLY 
//   usually by MouseWheel or Home/End keys

function dollyCamAwayFromPlayer(%client)
{ 
  %distance = %client.camera.getThirdPersonDistance();
  %distance += 0.5;
  %client.camera.setThirdPersonDistance(%distance);
}

function dollyCamTowardPlayer(%client)
{   
  %distance = %client.camera.getThirdPersonDistance();
  %distance -= 0.5;
  %client.camera.setThirdPersonDistance(%distance);
}

function serverCmdDollyThirdPersonCam(%client, %toward)
{
  if (%toward)
    dollyCamTowardPlayer(%client);
  else
    dollyCamAwayFromPlayer(%client); 
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AFX CAMERA LOOK 

function serverCmdLookThirdPersonCam(%client, %left)
{
  %angle = %client.camera.getThirdPersonAngle();
  if (%left)
    %angle -= 5;
  else
    %angle += 5;
  %client.camera.setThirdPersonAngle(%angle);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AFX CAMERA FIRST-PERSON TOGGLE

// Note - the third-person camera auto-switches to and from
// first person mode based on the value of the third-person
// distance. The threshold is 1.0. Values less than the threshold
// force a switch to first-person-mode, values greater than
// or equal to 1.0, force a switch to third-person mode.

function serverCmdToggleFirstPersonPOV(%client)
{
  // get 3rd-person camera offset 
  %cam_dist = %client.camera.getThirdPersonDistance();

  // save 3rd-person distance, force first-person on
  if (%cam_dist >= 1.0)
  {
    %client.save_3rdperson_dist = %cam_dist;
    %cam_dist = 0.1;
  }
  // restore 3rd-person distance, force first-person off
  else if (%client.save_3rdperson_dist !$= "")
  {
    %cam_dist = (%client.save_3rdperson_dist !$= "") ? %client.save_3rdperson_dist : 3;
  }

  // set new 3rd-person distance
  %client.camera.setThirdPersonDistance(%cam_dist);
  %client.camera.setThirdPersonSnap();
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function sendPlayerSpellBookToClient(%client, %spellbook)
{
  %ghost_idx = %client.GetGhostIndex(%spellbook);
  if (%ghost_idx == -1)
    schedule(100, 0, "sendPlayerSpellBookToClient", %client, %spellbook);
  else
    commandToClient(%client, 'SetPlayerSpellBook', %ghost_idx);
}

function sendClientPlayerToClient(%client, %player)
{
  %ghost_idx = %client.GetGhostIndex(%player);
  if (%ghost_idx == -1)
    schedule(100, 0, "sendClientPlayerToClient", %client, %player);
  else
    commandToClient(%client, 'SetClientPlayer', %ghost_idx);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
