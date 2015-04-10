
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// Thsi script supports the Datablock Cache system.
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

// DATABLOCK CACHE CODE <<

// This function is an alternate to the common serverCmdMissionStartPhase1Ack()
// found in core/scripts/server/missionDownload.cs. This version is called when
// we want to load datablocks from a cache rather than have them transmitted
// from the server.
// 
function serverCmdMissionStartPhase1Ack_UseCache(%client, %seq)
{
   // Make sure to ignore calls from a previous mission load
   if (%seq != $missionSequence || !$MissionRunning)
      return;
   if (%client.currentPhase != 0)
      return;
   %client.currentPhase = 1;

   // Start with the CRC
   %client.setMissionCRC( $missionCRC );

   // MODIFIED CODE <<
   echo("<<<< client will load datablocks from a cache >>>>");
   echo("    <<<< skipping datablock transmission >>>>");
   %client.onBeginDatablockCacheLoad($missionSequence);
   /* ORIGINAL CODE
   // Send over the datablocks...
   // OnDataBlocksDone will get called when have confirmation
   // that they've all been received.
   %client.transmitDataBlocks($missionSequence);
   */
   // MODIFIED CODE >>
}

function GameConnection::onBeginDatablockCacheLoad( %this, %missionSequence )
{
   // Make sure to ignore calls from a previous mission load
   if (%missionSequence != $missionSequence)
      return;
   if (%this.currentPhase != 1)
      return;
   %this.currentPhase = 1.5;
   commandToClient(%this, 'MissionStartPhase1_LoadCache', $missionSequence, $Server::MissionFile);
}

// This function overrides a function with the same name found in
// core/scripts/server/missionDownload.cs. When datablock caching is enabled
// on the server, it tells the server to save the cache as necessary,
// calculates the cache's CRC, and sends it to the client along with
// other mission info.
function GameConnection::loadMission(%this)
{
   // MODIFIED CODE <<
   %cache_crc = "";

   if ($Pref::Server::EnableDatablockCache)
   {
      if (!isDatablockCacheSaved())
      {
         echo("<<<< saving server datablock cache >>>>");
         %this.saveDatablockCache();
      }

      if (isFile($Pref::Server::DatablockCacheFilename))
      {
         %cache_crc = getDatablockCacheCRC();
         echo("    <<<< sending CRC to client:" SPC %cache_crc SPC ">>>>");
      }
   }
   // MODIFIED CODE >>

   // Send over the information that will display the server info
   // when we learn it got there, we'll send the data blocks
   %this.currentPhase = 0;
   if (%this.isAIControlled())
   {
      // Cut to the chase...
      %this.onClientEnterGame();
   }
   else
   {
      commandToClient(%this, 'MissionStartPhase1', $missionSequence,
         $Server::MissionFile, MissionGroup.musicTrack, %cache_crc);
      echo("*** Sending mission load to client: " @ $Server::MissionFile);
   }
}
// DATABLOCK CACHE CODE >>

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
