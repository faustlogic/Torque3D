//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
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
//-----------------------------------------------------------------------------

// ----------------------------------------------------------------------------
// AFXDemoGame
// ----------------------------------------------------------------------------
// Depends on methods found in gameCore.cs.  Those added here are specific to
// this game type and/or over-ride the "default" game functionaliy.
//
// The desired Game Type must be added to each mission's LevelInfo object.
//   - gameType = "AFXDemo";
// If this information is missing then the GameCore will default to AFXDemo.
// ----------------------------------------------------------------------------

function AFXDemoGame::onMissionLoaded(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> AFXDemoGame::onMissionLoaded");

   $Server::MissionType = "AFXDemo";
   parent::onMissionLoaded(%game);
}

function AFXDemoGame::onMissionEnded(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> AFXDemoGame::onMissionEnded");
   parent::onMissionEnded(%game);

   // DATABLOCK CACHE CODE <<
   if ($Pref::Server::EnableDatablockCache)
      resetDatablockCache();
   // DATABLOCK CACHE CODE >>

   onMissionEnded_AFX(); // AFX
}

function AFXDemoGame::initGameVars(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> AFXDemoGame::initGameVars");

   //-----------------------------------------------------------------------------
   // What kind of "player" is spawned is either controlled directly by the
   // SpawnSphere or it defaults back to the values set here. This also controls
   // which SimGroups to attempt to select the spawn sphere's from by walking down
   // the list of SpawnGroups till it finds a valid spawn object.
   // These override the values set in core/scripts/server/spawn.cs
   //-----------------------------------------------------------------------------
   
   // Leave $Game::defaultPlayerClass and $Game::defaultPlayerDataBlock as empty strings ("")
   // to spawn a the $Game::defaultCameraClass as the control object.
   $Game::defaultPlayerClass = "Player";
   $Game::defaultPlayerDataBlock = "OrcMageData";
   $Game::defaultPlayerSpawnGroups = "PlayerSpawnPoints PlayerDropPoints";

   //-----------------------------------------------------------------------------
   // What kind of "camera" is spawned is either controlled directly by the
   // SpawnSphere or it defaults back to the values set here. This also controls
   // which SimGroups to attempt to select the spawn sphere's from by walking down
   // the list of SpawnGroups till it finds a valid spawn object.
   // These override the values set in core/scripts/server/spawn.cs
   //-----------------------------------------------------------------------------
   $Game::defaultCameraClass = "Camera";
   $Game::defaultCameraDataBlock = "Observer";
   $Game::defaultCameraSpawnGroups = "CameraSpawnPoints PlayerSpawnPoints PlayerDropPoints";

   // Set the gameplay parameters
   %game.duration = 30 * 60;
   %game.endgameScore = 20;
   %game.endgamePause = 10;
   %game.allowCycling = false;   // Is mission cycling allowed?
}

function AFXDemoGame::startGame(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> AFXDemoGame::startGame");

   parent::startGame(%game);

   start_NonPlayerWrangler();
   
   /*
   // AFX DEMO MOD <<
   // This spawns an AI that runs around and functions as a moving target.
   %path_group = "MissionGroup/NPCPaths";
   AIPlayer::spawn(%path_group.getObject(0));
   // AFX DEMO MOD >>
   */
}

function AFXDemoGame::spawnPlayer(%game, %client, %spawnPoint, %noControl)
{
    echo (%game @"\c4 -> "@ %game.class @" -> AFXDemoGame::spawnPlayer");

    // parent::spawnPlayer(%game, %spawnPoint);

   if (isObject(%client.player))
   {
      // The client should not already have a player. Assigning
      // a new one could result in an uncontrolled player object.
      error("Attempting to create a player for a client that already has one!");
   }

   // Attempt to treat %spawnPoint as an object
   if (getWordCount(%spawnPoint) == 1 && isObject(%spawnPoint))
   {
      // Defaults
      %spawnClass      = $Game::DefaultPlayerClass;
      %spawnDataBlock  = $Game::DefaultPlayerDataBlock;

      // Overrides by the %spawnPoint
      if (isDefined("%spawnPoint.spawnClass"))
      {
         %spawnClass = %spawnPoint.spawnClass;
         %spawnDataBlock = %spawnPoint.spawnDatablock;
      }
      else if (isDefined("%spawnPoint.spawnDatablock"))
      {
         // This may seem redundant given the above but it allows
         // the SpawnSphere to override the datablock without
         // overriding the default player class
         %spawnDataBlock = %spawnPoint.spawnDatablock;
      }

      %spawnProperties = %spawnPoint.spawnProperties;
      %spawnScript     = %spawnPoint.spawnScript;

      //%spawnProperties = "client = " @ %client @ ";" @ %spawnProperties; 

      // Spawn with the engine's Sim::spawnObject() function
      %player = spawnObject(%spawnClass, %spawnDataBlock, "",
                            %spawnProperties, %spawnScript);

      // If we have an object do some initial setup
      if (isObject(%player))
      {
         // Pick a location within the spawn sphere.
         %spawnLocation = GameCore::pickPointInSpawnSphere(%player, %spawnPoint);
         %player.setTransform(%spawnLocation);
         /*
         // Set the transform to %spawnPoint's transform
         %player.setTransform(%spawnPoint.getTransform());
         */
      }
      else
      {
         // If we weren't able to create the player object then warn the user
         // When the player clicks OK in one of these message boxes, we will fall through
         // to the "if (!isObject(%player))" check below.
         if (isDefined("%spawnDatablock"))
         {
               MessageBoxOK("Spawn Player Failed",
                             "Unable to create a player with class " @ %spawnClass @
                             " and datablock " @ %spawnDatablock @ ".\n\nStarting as an Observer instead.",
                             "");
         }
         else
         {
               MessageBoxOK("Spawn Player Failed",
                              "Unable to create a player with class " @ %spawnClass @
                              ".\n\nStarting as an Observer instead.",
                              "");
         }
      }
   }
   else
   {
      
      // Create a default player
      %player = spawnObject($Game::DefaultPlayerClass, $Game::DefaultPlayerDataBlock);
      
      if (!%player.isMemberOfClass("Player"))
         warn("Trying to spawn a class that does not derive from Player.");

      // Treat %spawnPoint as a transform
      %player.setTransform(%spawnPoint);
   }

   // If we didn't actually create a player object then bail
   if (!isObject(%player))
   {
      // Make sure we at least have a camera
      %client.spawnCamera(%spawnPoint);

      return;
   }

   // AFX SCRIPT BLOCK <<
   // Create player's spellbook
   %my_spellbook = new afxSpellBook() 
   {
     dataBlock = afxDemoSpellBookData;
   };
   MissionCleanup.add(%my_spellbook);
   %client.spellbook = %my_spellbook;

   schedule(0, 0, "sendClientPlayerToClient", %client, %player);
   schedule(0, 0, "sendPlayerSpellBookToClient", %client, %my_spellbook);
   // AFX SCRIPT BLOCK >>

   // Update the default camera to start with the player
   if (isObject(%client.camera) && !isDefined("%noControl"))
   {
      if (%player.getClassname() $= "Player")
         %client.camera.setTransform(%player.getEyeTransform());
      else
         %client.camera.setTransform(%player.getTransform());

      // AFX SCRIPT BLOCK <<
      // We set the camera system to run in 3rd person mode around the %player
      %client.camera.setCameraSubject(%player);   
      %client.camera.setThirdPersonMode();
      %client.camera.setThirdPersonOffset("0 0 3"); // this is default 3rd-person offset
      %client.camera.setThirdPersonDistance(3); // this is default 3rd-person distance
      %client.camera.setThirdPersonSnap();
      %client.setCameraObject(%client.camera);
      // AFX SCRIPT BLOCK >>
   }

   // Add the player object to MissionCleanup so that it
   // won't get saved into the level files and will get
   // cleaned up properly
   MissionCleanup.add(%player);

   // Store the client object on the player object for
   // future reference
   %player.client = %client;
   
   // If the player's client has some owned turrets, make sure we let them
   // know that we're a friend too.
   if (%client.ownedTurrets)
   {
      for (%i=0; %i<%client.ownedTurrets.getCount(); %i++)
      {
         %turret = %client.ownedTurrets.getObject(%i);
         %turret.addToIgnoreList(%player);
      }
   }

   // Player setup...
   if (%player.isMethod("setShapeName"))
      %player.setShapeName(%client.playerName);

   if (%player.isMethod("setEnergyLevel"))
      %player.setEnergyLevel(%player.getDataBlock().maxEnergy);
      
   // AFX SCRIPT BLOCK <<
   if (%player.isMethod("setLookAnimationOverride"))
      %player.setLookAnimationOverride(true);
   // AFX SCRIPT BLOCK >>      

   if (!isDefined("%client.skin"))
   {
      // Determine which character skins are not already in use
      %availableSkins = %player.getDatablock().availableSkins;             // TAB delimited list of skin names
      %count = ClientGroup.getCount();
      for (%cl = 0; %cl < %count; %cl++)
      {
         %other = ClientGroup.getObject(%cl);
         if (%other != %client)
         {
            %availableSkins = strreplace(%availableSkins, %other.skin, "");
            %availableSkins = strreplace(%availableSkins, "\t\t", "");     // remove empty fields
         }
      }

      // Choose a random, unique skin for this client
      %count = getFieldCount(%availableSkins);
      %client.skin = addTaggedString( getField(%availableSkins, getRandom(%count)) );
   }

   %player.setSkinName(%client.skin);

   // Give the client control of the player
   %client.player = %player;

   // Give the client control of the camera if in the editor
   if( $startWorldEditor )
   {
      %control = %client.camera;
      %control.mode = "Fly"; // %control.setFlyMode();
      EditorGui.syncCameraGui();
   }
   else
      %control = %player;

   // Allow the player/camera to receive move data from the GameConnection.  Without this
   // the user is unable to control the player/camera.
   if (!isDefined("%noControl"))
      %client.setControlObject(%control);
}

function AFXDemoGame::endGame(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> AFXDemoGame::endGame");

   parent::endGame(%game);

   // AFX SCRIPT BLOCK <<
   if (isObject(NonPlayerWrangler))
      stop_NonPlayerWrangler();
   // AFX SCRIPT BLOCK >>
}

function AFXDemoGame::onGameDurationEnd(%game)
{
   echo (%game @"\c4 -> "@ %game.class @" -> AFXDemoGame::onGameDurationEnd");

   parent::onGameDurationEnd(%game);
}

function AFXDemoGame::onClientEnterGame(%game, %client)
{
   echo (%game @"\c4 -> "@ %game.class @" -> AFXDemoGame::onClientEnterGame");

   //parent::onClientEnterGame(%game, %client);

   // This function currently relies on some helper functions defined in
   // core/scripts/server/spawn.cs. For custom spawn behaviors one can either
   // override the properties on the SpawnSpheres or directly override the
   // functions themselves.

   // This is where we create the afxCamera
   %client.camera = new afxCamera()
   {
      datablock = DefaultAFXCameraData;
   };

   MissionCleanup.add( %client.camera );
   %client.camera.scopeToClient(%client);

   // Start everyone with a zero score when they connect
   %client.score = 0;

   // Find a spawn point for the player
   %playerSpawnPoint = pickPlayerSpawnPoint($Game::DefaultPlayerSpawnGroups);
   // Spawn a camera for this client using the found %spawnPoint
   %client.spawnPlayer(%playerSpawnPoint);

   if (theLevelInfo.startupEffectsFunc !$= "")
   {
     call(theLevelInfo.startupEffectsFunc);
   }
}

function GameCore::loadOut(%game, %player)
{
   echo (%game @"\c4 -> "@ %game.class @" -> AFXDemoGame::loadOut");

   //parent::loadOut(%game, %client);
}

function AFXDemoGame::onClientLeaveGame(%game, %client)
{
   echo (%game @"\c4 -> "@ %game.class @" -> AFXDemoGame::onClientLeaveGame");

   parent::onClientLeaveGame(%game, %client);
}
