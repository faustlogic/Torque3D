
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// This script initializes input bindings for the AFX Demo.
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

if ( isObject( moveMap ) )
   moveMap.delete();
new ActionMap(moveMap);


//------------------------------------------------------------------------------
// Non-remapable binds
//------------------------------------------------------------------------------

function escapeFromGame()
{
   if ( $Server::ServerType $= "SinglePlayer" )
      MessageBoxYesNo( "Exit", "Exit from this Mission?", "disconnect();", "");
   else
      MessageBoxYesNo( "Disconnect", "Disconnect from the server?", "disconnect();", "");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// MOVEMENT CONTROL

$movementSpeed = 0.6; //1; // m/s

function setSpeed(%speed)
{
  if (%speed)
    $movementSpeed = %speed;
}

// MOVE FORWARD
$move_fwd_1 = false;
$move_fwd_2 = false;
$move_fwd_3 = false;

function moveforward()
{
  if ($move_fwd_1 || $move_fwd_2 || $move_fwd_3)
    $mvForwardAction = $movementSpeed;
  else
    $mvForwardAction = 0;
}
//
function moveforward_1(%val)
{
  $move_fwd_1 = %val;
  moveforward();
}
//
function moveforward_2(%val)
{
  $move_fwd_2 = %val;
  moveforward();
}
//
function moveforward_3(%val)
{
  $move_fwd_3 = %val;
  moveforward();
}

// MOVE BACKWARD
$move_bkwd_1 = false;
$move_bkwd_2 = false;

function movebackward()
{
  if ($move_bkwd_1 || $move_bkwd_2)
    $mvBackwardAction = $movementSpeed;
  else
    $mvBackwardAction = 0;
}
//
function movebackward_1(%val)
{
  $move_bkwd_1 = %val;
  movebackward();
}
//
function movebackward_2(%val)
{
  $move_bkwd_2 = %val;
  movebackward();
}

// TURN LEFT
$turn_lf_1 = false;
$turn_lf_2 = false;

function turnleft()
{
  if ($turn_lf_1 || $turn_lf_2)
    $mvYawRightSpeed = $Pref::Input::KeyboardTurnSpeed;
  else
    $mvYawRightSpeed = 0;
}
//
function turnleft_1(%val)
{
  $turn_lf_1 = %val;
  turnleft();
}
//
function turnleft_2(%val)
{
  $turn_lf_2 = %val;
  turnleft();
}

// TURN RIGHT
$turn_rt_1 = false;
$turn_rt_2 = false;

function turnright()
{
  if ($turn_rt_1 || $turn_rt_2)
    $mvYawLeftSpeed = $Pref::Input::KeyboardTurnSpeed;
  else
    $mvYawLeftSpeed = 0;
}
function turnright_1(%val)
{
  $turn_rt_1 = %val;
  turnright();
}
//
function turnright_2(%val)
{
  $turn_rt_2 = %val;
  turnright();
}

// MOVE LEFT
$move_lf_1 = false;
$move_lf_2 = false;

function moveleft()
{
  if ($move_lf_1 || $move_lf_2)
    $mvLeftAction = $movementSpeed;
  else
    $mvLeftAction = 0;
}
//
function moveleft_1(%val)
{
  $move_lf_1 = %val;
  moveleft();
}
//
function moveleft_2(%val)
{
  $move_lf_2 = %val;
  moveleft();
}

// MOVE RIGHT
$move_rt_1 = false;
$move_rt_2 = false;

function moveright()
{
  if ($move_rt_1 || $move_rt_2)
    $mvRightAction = $movementSpeed;
  else
    $mvRightAction = 0;
}
//
function moveright_1(%val)
{
  $move_rt_1 = %val;
  moveright();
}
//
function moveright_2(%val)
{
  $move_rt_2 = %val;
  moveright();
}

// JUMP
function jump(%val)
{
  if (isObject(ServerConnection.player) && ServerConnection.player.isEnabled())
  {
    if (%val == 1)
    {
      if (($mvTriggerCount2 % 2) == 0)
        $mvTriggerCount2++;
    }
    else
    {
      if (($mvTriggerCount2 % 2) == 1)
        $mvTriggerCount2++;
    }
  }
  else
  {
    if (%val)
      commandToServer('RespawnPlayerAfterDeath');
  }
}

function fireKey(%val)
{
   $mvTriggerCount0++;
}

function fireAltKey(%val)
{
   $mvTriggerCount1++;
}

function fx_triggerTest(%val)
{
  switch_fxTrigger(%val != 0);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// MOUSE CONTROL

function getMouseAdjustAmount(%val)
{
   // based on a default camera fov of 90'
   return(%val * ($cameraFov / 90) * 0.01) * $pref::Input::LinkMouseSensitivity;
}

$accum_yaw = 0;
$accum_pitch = 0;

function yaw(%val)
{
  %amt = getMouseAdjustAmount(%val);
  $mvYaw += %amt;
  $accum_yaw += %amt; 
}

function pitch(%val)
{
  %amt = getMouseAdjustAmount(%val);
  $mvPitch += %amt;
  $accum_pitch += %amt;
}

$mouseLButtonDown = false;
$mouseRButtonDown = false;
$mouseLRButtonsDown = false;
$mouseLRButtonsWasDown = false;

function testForMouseMoveForward()
{
  if ($mouseLRButtonsDown != $mouseLRButtonsWasDown)
  {
    $mouseLRButtonsWasDown = $mouseLRButtonsDown;
    if ($mouseLRButtonsDown)
    {
      ServerConnection.clearPreSelectedObj();
      moveforward_3(1);
    }
    else
    {
      moveforward_3(0);
    }
  }
}

function mouse_btn0(%val)
{   
  $mouseLButtonDown = %val;
  $mouseLRButtonsDown = ($mouseLButtonDown && $mouseRButtonDown);

  if (afxInFreeTargetingMode())
  {
    if (%val)
    {
      $accum_yaw = 0;
      $accum_pitch = 0;
      hideCursor(); // cursorOff();
    }
    else
    {
      if ($accum_yaw <= 0.02 && $accum_yaw >= -0.02 && $accum_pitch <= 0.02 && $accum_pitch >= -0.02)
        afxFinishFreeTargetingMode();
      showCursor(); // cursorOn();
    }
  }
  else
  {
    if (%val)
    {
      $accum_yaw = 0;
      $accum_pitch = 0;
      ServerConnection.setPreSelectedObjFromRollover();
      hideCursor(); // cursorOff();
    }
    else
    {
      if ($accum_yaw > 0.02 || $accum_yaw < -0.02 || $accum_pitch > 0.02 || $accum_pitch < -0.02)
        ServerConnection.clearPreSelectedObj();
      else
        ServerConnection.setSelectedObjFromPreSelected();
      showCursor(); // cursorOn();
    }
  }

  testForMouseMoveForward();
}

function mouse_btn1(%val)
{  
  $mouseRButtonDown = %val;
  $mouseLRButtonsDown = ($mouseLButtonDown && $mouseRButtonDown);
  
  if (%val)
  {
    //if (afxInFreeTargetingMode())
    //  afxCancelFreeTargetingMode();
    hideCursor(); // cursorOff();
  }
  else   
    showCursor(); // cursorOn();

  testForMouseMoveForward();
}

function escape_key(%val)
{
  if (!%val)
  {
    if (afxInFreeTargetingMode())
      afxCancelFreeTargetingMode();
    else
      escapeFromGame();
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

function spellbank_pageup(%val)
{
  if (%val)
    SpellBank.prevPage();
}

function spellbank_pagedn(%val)
{
  if (%val)
    SpellBank.nextPage();
}

function interruptSpellcastingAction(%val)
{
  if (%val)
    commandToServer('InterruptSpellcasting');
}

function inflictDamageAction(%val)
{
  if (%val)
    commandToServer('InflictDamage');
}

function nextSeleStyle(%val)
{
  if (%val)
    gotoNextSelectronStyle();
}

function prevSeleStyle(%val)
{
  if (%val)
    gotoPreviousSelectronStyle();
}


function castGreatBall(%val)
{
  if (%val)
  {
    // get selected object's ghost index
    %sel_obj = ServerConnection.getSelectedObj();
    if (%sel_obj != -1)
      %sel_obj_ghost = ServerConnection.GetGhostIndex(%sel_obj);
    else
      %sel_obj_ghost = -1;

    // call to cast spell on server
    commandToServer('DoGreatBallCast', %sel_obj_ghost);
  }
}

function pushPhraseTester(%val)
{
  if (%val)
  {
    commandToServer('DoPhraseTesterPush');
  }
}

function haltPhraseTester(%val)
{
  if (%val)
  {
    commandToServer('DoPhraseTesterHalt');
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

// Zoom and FOV functions

if($Pref::player::CurrentFOV $= "")
   $Pref::player::CurrentFOV = 45;

function resetCurrentFOV()
{
}
function turnOffZoom()
{
}
function setZoomFOV(%val)
{
   if(%val)
      toggleZoomFOV();
}
    
$ZoomOn = false;

function toggleZoom( %val )
{
   if ( %val )
   {
      $ZoomOn = true;
      setFov( $Pref::player::CurrentFOV );
   }
   else
   {
      $ZoomOn = false;
      setFov( $Pref::player::DefaultFov );
   }
}

// Camera & View functions

function toggleFreeLook( %val )
{
   if ( %val )
      $mvFreeLook = true;
   else
      $mvFreeLook = false;
}

function toggleCamera(%val)
{
   if (%val)
   {
      commandToServer('ToggleCamera');
      if (ServerConnection.getSelectedObj() == ServerConnection.player)
        ServerConnection.clearSelectedObj();
   }
}

function stepTo(%val)
{
   if (%val)
     commandToServer('dollyThirdPersonCam', true);
}

function stepFro(%val)
{
   if (%val)
     commandToServer('dollyThirdPersonCam', false);
}

function stepCamLookLeft(%val)
{
   if (%val)
     commandToServer('lookThirdPersonCam', true);
}

function stepCamLookRight(%val)
{
   if (%val)
     commandToServer('lookThirdPersonCam', false);
}

// Message HUD functions

function pageMessageHudUp( %val )
{
   if ( %val )
      pageUpMessageHud();
}

function pageMessageHudDown( %val )
{
   if ( %val )
      pageDownMessageHud();
}

function resizeMessageHud( %val )
{
   if ( %val )
      cycleMessageHudSize();
}

// Demo recording functions

function startRecordingDemo( %val )
{
   if ( %val )
      startDemoRecord();
}

function stopRecordingDemo( %val )
{
   if ( %val )
      stopDemoRecord();
}

// Helper Functions

function dropCameraAtPlayer(%val)
{
   if (%val)
      commandToServer('DropCameraAtPlayer');
}

function dropPlayerAtCamera(%val)
{
   if (%val)
      commandToServer('DropPlayerAtCamera');
}


function bringUpOptions(%val)
{
   if (%val)
      Canvas.pushDialog(OptionsDlg);
}

$GUI_HIDDEN = false;

function toggle_ui_controls(%val)
{
  if (%val)
  {
    if (!$GUI_HIDDEN)
    {
      SpellBank.visible = false;
      SpellCastBar.visible = false;
      ScreenMessageBox.visible = false;
      InfoBoxBackdrop.visible = false;
      InfoBox.visible = false;
      PlayerStatusbar.visible = false;
      TargetStatusbar.visible = false;
      AFX_HUD_Logo.visible = false;
      $GUI_HIDDEN = true;
    }
    else
    {
      SpellBank.visible = true;
      ScreenMessageBox.visible = true;
      PlayerStatusbar.visible = true;
      AFX_HUD_Logo.visible = true;
      $GUI_HIDDEN = false;
    }
  }
}

//------------------------------------------------------------------------------
// Debugging Functions
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
//
// Start profiler by pressing ctrl f3
// ctrl f3 - starts profile that will dump to console and file
// 
function doProfile(%val)
{
   if (%val)
   {
      // key down -- start profile
      echo("Starting profile session...");
      profilerReset();      
      profilerEnable(true);
   }
   else
   {
      // key up -- finish off profile
      echo("Ending profile session...");
      
      profilerDumpToFile("profilerDumpToFile" @ getSimTime() @ ".txt");
      profilerEnable(false);
   }
}

GlobalActionMap.bind(keyboard, "ctrl F3", doProfile);

// Misc.

function celebrationWave(%val)
{
   if(%val)
      commandToServer('playCel', "wave");
}

function celebrationSalute(%val)
{
   if(%val)
      commandToServer('playCel', "salute");
}

function suicide(%val)
{
   if(%val)
      commandToServer('Suicide');
}

function net_graph(%val)
{
   if (%val)
      NetGraph::toggleNetGraph();
}


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

GlobalActionMap.bindCmd(keyboard, "alt enter",  "",     "toggleFullScreen();");

GlobalActionMap.bind(keyboard,    tilde,    toggleConsole);

moveMap.bind(keyboard, escape,      escape_key);

moveMap.bind(keyboard, "ctrl w",    celebrationWave);
moveMap.bind(keyboard, "ctrl s",    celebrationSalute);
moveMap.bind(keyboard, "ctrl k",    suicide);
moveMap.bind(keyboard, "n",         net_graph);

moveMap.bind(keyboard,  F3,         startRecordingDemo);
moveMap.bind(keyboard,  F4,         stopRecordingDemo);
moveMap.bind(keyboard,  F5,         toggleParticleEditor);
moveMap.bind(keyboard,  F7,         dropPlayerAtCamera);
moveMap.bind(keyboard,  F8,         dropCameraAtPlayer);

                // MAIN MOVEMENT KEYS //
moveMap.bind(keyboard,  w,          moveforward_1);
moveMap.bind(keyboard,  s,          movebackward_1);
moveMap.bind(keyboard,  a,          turnleft_1);
moveMap.bind(keyboard,  d,          turnright_1);
moveMap.bind(keyboard,  q,          moveleft_1);
moveMap.bind(keyboard,  e,          moveright_1);
moveMap.bind(keyboard,  space,      jump);

                // SECONDARY MOVEMENT KEYS //
moveMap.bind(keyboard,  up,         moveforward_2);
moveMap.bind(keyboard,  down,       movebackward_2);
moveMap.bind(keyboard,  left,       turnleft_2);
moveMap.bind(keyboard,  right,      turnright_2);

                // SPELLCASTING KEYS //
  // pageup/pagedown will cycle through different spellbanks in the gui
moveMap.bind(keyboard,  pageup,     spellbank_pageup);
moveMap.bind(keyboard,  pagedown,   spellbank_pagedn);
  // The 'i' key interrupts active spellcasting by the player character.
moveMap.bind(keyboard,  i,          interruptSpellcastingAction);
  // Use 'f' to bypass spellbook and directly cast "Flame Broil" spell.
moveMap.bind(keyboard,  f,          castGreatBall);
  // The ctrl-d key inflicts 10 damage to the player character. Useful
  // for testing spell interrupts from damage. (try on "Summon Feckless Moth")
moveMap.bind(keyboard,  "ctrl d",   inflictDamageAction);
  // The keys, semicolon and ctrl-semicolon, go with the
  // PhraseTester spell. semicolon pushes the phrase-tester
  // into its next phrase. ctrl-semicolon kills the whole spell.
moveMap.bind(keyboard,  "semicolon",        pushPhraseTester);
moveMap.bind(keyboard,  "ctrl semicolon",   haltPhraseTester);

                // CAMERA KEYS //
moveMap.bind( keyboard, "v",        toggleFreeLook);
moveMap.bind(keyboard,  "alt c",    toggleCamera);
moveMap.bind(keyboard,  "z",   toggleZoom);
  // home/end keys decrease/increase camera distance from the player
  // character in the same way that mouse-wheel does, (very useful if
  // your mouse lacks a wheel.)
moveMap.bind(keyboard,  home,       stepTo );
moveMap.bind(keyboard,  end,        stepFro );

                // MOUSE CONTROL //
moveMap.bind(mouse,     xaxis,      yaw);
moveMap.bind(mouse,     yaxis,      pitch);
moveMap.bind(mouse,     button0,    mouse_btn0);
moveMap.bind(mouse,     button1,    mouse_btn1);

                // OTHER KEYS //
moveMap.bind(keyboard,  "ctrl o",   bringUpOptions);
  // It's not perfect (some things may pop in) but 'h' key is  useful
  // for hiding interface elements for a clean screenshot.
moveMap.bind(keyboard,  h,          toggle_ui_controls);
  // Use 't' to cycle through selectron styles
moveMap.bind(keyboard,  t,          nextSeleStyle);
moveMap.bind(keyboard,  "shift t",  prevSeleStyle);

moveMap.bind(keyboard, "ctrl -", fireKey);
moveMap.bind(keyboard, "ctrl =", fireAltKey);

moveMap.bind(keyboard, m, fx_triggerTest);


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function toggleFirstPersonPOV(%val)
{
   if (%val)
     commandToServer('ToggleFirstPersonPOV');
}

moveMap.bind(keyboard,  "tab",      toggleFirstPersonPOV);

