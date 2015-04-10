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

//----------------------------------------------------------------------------
// Mission Loading & Mission Info
// The mission loading server handshaking is handled by the
// core/scripts/client/missingLoading.cs.  This portion handles the interface
// with the game GUI.
//----------------------------------------------------------------------------

//----------------------------------------------------------------------------
// Loading Phases:
// Phase 1: Download Datablocks
// Phase 2: Download Ghost Objects
// Phase 3: Scene Lighting

//----------------------------------------------------------------------------
// Phase 1
//----------------------------------------------------------------------------

// DATABLOCK CACHE MOD <<
// Adds parameter %alt_msg for optionally providing an alternative message to 
// the loading progress bar. 
function onMissionDownloadPhase1(%missionName, %musicTrack, %alt_msg)
/* ORIGINAL CODE
function onMissionDownloadPhase1(%missionName, %musicTrack)
*/
// DATABLOCK CACHE MOD >>
{   
   // Load the post effect presets for this mission.
   %path = "levels/" @ fileBase( %missionName ) @ $PostFXManager::fileExtension;
   if ( isScriptFile( %path ) )
      postFXManager::loadPresetHandler( %path ); 
   else
      PostFXManager::settingsApplyDefaultPreset();
               
   // Close and clear the message hud (in case it's open)
   if ( isObject( MessageHud ) )
      MessageHud.close();

   // Reset the loading progress controls:
   if ( !isObject( LoadingProgress ) )
      return;
	  
   LoadingProgress.setValue(0);
   // DATABLOCK CACHE MOD <<
   // If a value for %alt_msg is specified, it will replace the default value
   // of "LOADING DATABLOCKS". 
   LoadingProgressTxt.setValue((%alt_msg $= "") ? "LOADING DATABLOCKS" : %alt_msg);
   /* ORIGINAL CODE
   LoadingProgressTxt.setValue("LOADING DATABLOCKS");
   */
   // DATABLOCK CACHE MOD >>
   Canvas.repaint();
}

function onPhase1Progress(%progress)
{
   if ( !isObject( LoadingProgress ) )
      return;
      
   LoadingProgress.setValue(%progress);
   Canvas.repaint(33);
}

function onPhase1Complete()
{
   if ( !isObject( LoadingProgress ) )
      return;
	  
   LoadingProgress.setValue( 1 );
   Canvas.repaint();
}

//----------------------------------------------------------------------------
// Phase 2
//----------------------------------------------------------------------------

function onMissionDownloadPhase2()
{
   if ( !isObject( LoadingProgress ) )
      return;
      
   LoadingProgress.setValue(0);
   LoadingProgressTxt.setValue("LOADING OBJECTS");
   Canvas.repaint();
}

function onPhase2Progress(%progress)
{
   if ( !isObject( LoadingProgress ) )
      return;
        
   LoadingProgress.setValue(%progress);
   Canvas.repaint(33);
}

function onPhase2Complete()
{
   if ( !isObject( LoadingProgress ) )
      return;
	  
   LoadingProgress.setValue( 1 );
   Canvas.repaint();
}   

function onFileChunkReceived(%fileName, %ofs, %size)
{
   if ( !isObject( LoadingProgress ) )
      return;     

   LoadingProgress.setValue(%ofs / %size);
   LoadingProgressTxt.setValue("Downloading " @ %fileName @ "...");
}

//----------------------------------------------------------------------------
// Phase 3
//----------------------------------------------------------------------------

function onMissionDownloadPhase3()
{
   if ( !isObject( LoadingProgress ) )
      return;
      
   LoadingProgress.setValue(0);
   LoadingProgressTxt.setValue("LIGHTING MISSION");
   Canvas.repaint();
}

function onPhase3Progress(%progress)
{
   if ( !isObject( LoadingProgress ) )
      return;
	  
   LoadingProgress.setValue(%progress);
   Canvas.repaint(33);
}

function onPhase3Complete()
{
   $lightingMission = false;

   if ( !isObject( LoadingProgress ) )
      return;
	  
   LoadingProgressTxt.setValue("STARTING MISSION");
   LoadingProgress.setValue( 1 );
   Canvas.repaint();
}

//----------------------------------------------------------------------------
// Mission loading done!
//----------------------------------------------------------------------------

function onMissionDownloadComplete()
{
   // Client will shortly be dropped into the game, so this is
   // good place for any last minute gui cleanup.
}


//------------------------------------------------------------------------------
// Before downloading a mission, the server transmits the mission
// information through these messages.
//------------------------------------------------------------------------------

addMessageCallback( 'MsgLoadInfo', handleLoadInfoMessage );
addMessageCallback( 'MsgLoadDescripition', handleLoadDescriptionMessage );
addMessageCallback( 'MsgLoadInfoDone', handleLoadInfoDoneMessage );
addMessageCallback( 'MsgLoadFailed', handleLoadFailedMessage );

//------------------------------------------------------------------------------

function handleLoadInfoMessage( %msgType, %msgString, %mapName ) 
{
   // Make sure the LoadingGUI is displayed
   if (Canvas.getContent() != LoadingGui.getId())
   {
      loadLoadingGui("LOADING MISSION FILE");
   }
   
	// Clear all of the loading info lines:
	for( %line = 0; %line < LoadingGui.qLineCount; %line++ )
		LoadingGui.qLine[%line] = "";
	LoadingGui.qLineCount = 0;
}

//------------------------------------------------------------------------------

function handleLoadDescriptionMessage( %msgType, %msgString, %line )
{
	LoadingGui.qLine[LoadingGui.qLineCount] = %line;
	LoadingGui.qLineCount++;

   // Gather up all the previous lines, append the current one
   // and stuff it into the control
	%text = "<spush><font:Arial:16>";
	
	for( %line = 0; %line < LoadingGui.qLineCount - 1; %line++ )
		%text = %text @ LoadingGui.qLine[%line] @ " ";
   %text = %text @ LoadingGui.qLine[%line] @ "<spop>";
}

//------------------------------------------------------------------------------

function handleLoadInfoDoneMessage( %msgType, %msgString )
{
   // This will get called after the last description line is sent.
}

//------------------------------------------------------------------------------

function handleLoadFailedMessage( %msgType, %msgString )
{
   MessageBoxOK( "Mission Load Failed", %msgString NL "Press OK to return to the Main Menu", "disconnect();" );
}

// DATABLOCK CACHE MOD <<

$loadFromDatablockCache = false;

//
// This function overrides a function with the same name found in
// core/scripts/client/missionDownload.cs. This replacement decides if the
// client can use a datablock cache instead of requiring transmission
// of datablocks form the server.
// 
function clientCmdMissionStartPhase1(%seq, %missionName, %musicTrack, %cache_crc)
{
  // These need to come after the cls.
  echo ("*** New Mission: " @ %missionName);
  echo ("*** Phase 1: Download Datablocks & Targets");

  $loadFromDatablockCache = false;
  if ($pref::Client::EnableDatablockCache)
  {
    %cache_filename = $pref::Client::DatablockCacheFilename;

    // if cache CRC is provided, check for validity
    if (%cache_crc !$= "")
    {
      // check for existence of cache file
      if (isFile(%cache_filename))
      { 
        // here we are not comparing the CRC of the cache itself, but the CRC of
        // the server cache (stored in the header) when these datablocks were
        // transmitted.
        %my_cache_crc = extractDatablockCacheCRC(%cache_filename);
        echo("<<<< client cache CRC:" SPC %my_cache_crc SPC ">>>>");
        echo("<<<< comparing CRC codes:" SPC "s:" @ %cache_crc SPC "c:" @ %my_cache_crc SPC ">>>>");
        if (%my_cache_crc == %cache_crc)
        {
          echo("<<<< cache CRC codes match, datablocks will be loaded from local cache. >>>>");
          $loadFromDatablockCache = true;
        }
        else
        {
          echo("<<<< cache CRC codes differ, datablocks will be transmitted and cached. >>>>" SPC %cache_crc);
          setDatablockCacheCRC(%cache_crc);
        }
      }
      else
      {
        echo("<<<< client datablock cache does not exist, datablocks will be transmitted and cached. >>>>");
        setDatablockCacheCRC(%cache_crc);
      }
    }
    else
    {
      echo("<<<< server datablock caching is disabled, datablocks will be transmitted. >>>>");
    }
    if ($loadFromDatablockCache)
    {
      // skip datablock transmission and initiate a cache load
      onMissionDownloadPhase1(%missionName, %musicTrack, "LOADING CACHED DATABLOCKS");
      commandToServer('MissionStartPhase1Ack_UseCache', %seq);
      return;
    }
  }
  else if (%cache_crc !$= "")
  {
    echo("<<<< client datablock caching is disabled, datablocks will be transmitted. >>>>");
  }

  // initiate a datablock transmission
  onMissionDownloadPhase1(%missionName, %musicTrack);
  commandToServer('MissionStartPhase1Ack', %seq);
}

$AFX_tempDisableVSync = false;

function clientCmdMissionStartPhase1_LoadCache(%seq, %missionName)
{
  if ($pref::Client::EnableDatablockCache && $loadFromDatablockCache)
  {
    if (!$pref::Video::disableVerticalSync)
    {
      warn("Disabling Vertical Sync during datablock cache load to avoid significant slowdown.");
      $AFX_tempDisableVSync = true;

      $pref::Video::disableVerticalSync = true;
      Canvas.resetVideoMode();
    }

    echo("<<<< Loading Datablocks From Cache >>>>");
    if (ServerConnection.loadDatablockCache_Begin())
    {
      schedule(10, 0, "updateLoadDatablockCacheProgress", %seq, %missionName);
    }
  }
}

function updateLoadDatablockCacheProgress(%seq, %missionName)
{
   if (ServerConnection.loadDatablockCache_Continue())
   {
      $loadDatablockCacheProgressThread = schedule(10, 0, "updateLoadDatablockCacheProgress", %seq, %missionName);
      return;
   }
 
   if ($AFX_tempDisableVSync)
   {
     warn("Restoring Vertical Sync setting.");
     $AFX_tempDisableVSync = false;

     $pref::Video::disableVerticalSync = false;
     Canvas.resetVideoMode();
   }

   echo("<<<< Finished Loading Datablocks From Cache >>>>");
   clientCmdMissionStartPhase2(%seq,%missionName);
}
// DATABLOCK CACHE MOD >>

