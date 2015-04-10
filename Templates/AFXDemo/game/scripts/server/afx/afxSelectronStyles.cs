
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
// demo selectron management

$Total_Sele_Styles = 0;
$Sele_Style_Names[0] = "";
$Sele_Style_Ids[0] = "";

function resetDemoSelectronStyles()
{
  $Total_Sele_Styles = 0;
}

function addDemoSelectronStyle(%sele_name, %sele_id)
{
  $Sele_Style_Names[$Total_Sele_Styles] = %sele_name;
  $Sele_Style_Ids[$Total_Sele_Styles] = %sele_id;
  $Total_Sele_Styles++;
}

function serverCmdNextSelectronStyle(%client, %current_style, %do_prev, %display_msg)
{
  %idx = -1;
  for (%i = 0; %i < $Total_Sele_Styles; %i++)
  {
    if (%current_style == $Sele_Style_Ids[%i])
    {
      %idx = %i;
      break;
    }
  }

  if (%idx == -1)
  {
    if ($Total_Sele_Styles == 0)
      return;
    %idx = (%do_prev) ? 0 : $Total_Sele_Styles;
  }


  if (%do_prev)
  {
    if (%idx == 0)
      %idx = $Total_Sele_Styles;
    %idx--;
  }
  else
  {
    %idx++;
    if (%idx >= $Total_Sele_Styles)
      %idx = 0;
  }

  commandToClient(%client, 'UpdateSelectronStyle', $Sele_Style_Names[%idx], $Sele_Style_Ids[%idx], %display_msg);
}

function serverCmdSetSelectronStyle(%client, %style_name, %display_msg)
{
  %idx = -1;
  for (%i = 0; %i < $Total_Sele_Styles; %i++)
  {
    if (%style_name $= $Sele_Style_Names[%i])
    {
      %idx = %i;
      break;
    }
  }

  if (%idx == -1)
  {
    if ($Total_Sele_Styles == 0)
      return;
    %idx = 0;
  }

  commandToClient(%client, 'UpdateSelectronStyle', $Sele_Style_Names[%idx], $Sele_Style_Ids[%idx], %display_msg);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

$afxPickTeleportDest_Tries = 0;

function afxPickTeleportDest(%current_pos)
{ 
  %dist = 0;
  %n_tries = 0;

  while (%dist < 15 && %n_tries < 5)
  {
    %spawnPoint = pickPlayerSpawnPoint("MissionGroup/TeleportSpots");

    // if %spawnPoint is an object, grab it's transform
    if (getWordCount(%spawnPoint) == 1 && isObject(%spawnPoint))
      %teleport_loc = %spawnPoint.getTransform();
    else // otherwise assume it's a transform or point
      %teleport_loc = %spawnPoint;

    %dist = VectorDist(%current_pos, %teleport_loc);
    %n_tries++;
  }

  return %teleport_loc;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

function afxBurnCorpse(%corpse)
{
  if (isObject(%corpse) && %corpse.getClassName() $= "Player")
  {
    // fade and delete corpse
    %corpse.schedule(0, "startFade", 1000, 0, true);
    %corpse.schedule(1000+1000, "delete");

    // if player is still associated with corpse, spawn a new one
    if (isObject(%corpse.client) && %corpse.client.player == %corpse)
    {
      %corpse.client.schedule(2000, "spawnPlayer", pickPlayerSpawnPoint($Game::DefaultPlayerSpawnGroups));
      %corpse.client.player = "";
      %corpse.client = "";
    }

    return;
  }
  
  // let NonPlayerWrangler handle this corpse burning
  if (isObject(NonPlayerWrangler))
    NonPlayerWrangler.burnCorpse(%corpse);
}

function afxResurrectCorpse(%corpse)
{
  if (isObject(%corpse) && %corpse.getClassName() $= "Player")
  {
    // if player is still associated with corpse, spawn a new one
    if (isObject(%corpse.client) && %corpse.client.player == %corpse)
    {
      %corpse.client.schedule(250, "spawnPlayer", %corpse.getTransform(), "Player", %corpse.getDatablock());
      %corpse.client.player = "";
      %corpse.client = "";
    }

    // fade and delete corpse 
    %corpse.schedule(0, "startFade", 500, 0, true);
    %corpse.schedule(500, "delete");

    return;
  }
  
  // let NonPlayerWrangler handle this resurrection
  if (isObject(NonPlayerWrangler))
    NonPlayerWrangler.resurrectCorpse(%corpse);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
