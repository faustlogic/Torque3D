
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// This script implements an object selection system and support for using
// selectron effects for selection highlighting and free-target placement.
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

if (isObject(PlayGui) && PlayGui.isMethod("pushTargetingMode"))
  PlayGui.pushTargetingMode($AFX::TARGETING_STANDARD, $AFX::TARGET_CHECK_POLL);

// Object Selection

/* Uncomment this to create scripted responses to rollover callouts.
function GameConnection::onObjectRollover(%this, %obj)
{
}
*/

function GameConnection::onObjectSelected(%this, %obj)
{
  if (isObject(TargetStatusbarLabel))
    TargetStatusbarLabel.setName(%obj.getShapeName());

  if (isObject(TargetStatusbar))
    TargetStatusbar.visible = !$GUI_HIDDEN;
  if (isObject(TargetHealthStatusBar))
    TargetHealthStatusBar.setShape(%obj);
  if (isObject(TargetEnergyStatusBar))
    TargetEnergyStatusBar.setShape(%obj);

  if (%obj.selectron == 0 && ($pref::AFX::clickToTargetSelf || %obj != %this.getControlObject()))
  {
    %selectron = startSelectron(%obj, $afxCurrentSelectronStyle);
    if (%selectron != 0)
      %selectron.addConstraint(%obj, "selected");
    %obj.selectron = %selectron;
  }  
}

function GameConnection::onObjectDeselected(%this, %obj)
{
  if (isObject(TargetStatusbar))
    TargetStatusbar.visible = false;
  if (isObject(TargetHealthStatusBar))
    TargetHealthStatusBar.clearShape();
  if (isObject(TargetEnergyStatusBar))
    TargetEnergyStatusBar.clearShape();

  if (%obj.selectron != 0 &&isObject(%obj.selectron))
  {
    %obj.selectron.stopSelectron();
    %obj.selectron = 0;
  }
}

function GameConnection::resetSelection(%this)
{
  %sel_obj = %this.getSelectedObj();
  if (isObject(%sel_obj))
     %this.setSelectedObj(%sel_obj);
}

// called from server: commandToClient(%client, 'ResetTargetStatusbarLabel')
function clientCmdResetTargetStatusbarLabel()
{
  if (isObject(TargetStatusbarLabel))
  {
    %sel_obj = ServerConnection.getSelectedObj();
    if (isObject(%sel_obj))
      TargetStatusbarLabel.setName(%sel_obj.getShapeName());
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Selectron Management

$afxCurrentSelectronStyle = 0;

function resetSelectronStyle()
{
  $afxCurrentSelectronStyle = 0;
}

function gotoNextSelectronStyle()
{
  commandToServer('NextSelectronStyle', $afxCurrentSelectronStyle, false, true);
}

function gotoPreviousSelectronStyle()
{
  commandToServer('NextSelectronStyle', $afxCurrentSelectronStyle, true, true);
}

// called from server: commandToClient(%client, 'UpdateSelectronStyle', %style_name, %style_id, %display_msg)
function clientCmdUpdateSelectronStyle(%style_name, %style_id, %display_msg)
{
  if (%display_msg)
  {
    clientCmdDisplayScreenMessage("Now using" SPC %style_name SPC "style selectrons.", clear);
  }

  $afxCurrentSelectronStyle = %style_id;

  ServerConnection.resetSelection();
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

$afxFreeTargetingSelectron = "";
$afxFreeTargetingBookSlot = "";

function afxStartFreeTargetingMode(%book_slot, %free_target_style)
{
  ServerConnection.clearSelectedObj();
  PlayGui.pushTargetingMode($AFX::TARGETING_FREE);
  if (!isObject($afxFreeTargetingSelectron))
  {
    %selectron = startSelectron(0, %free_target_style);
    if (!isObject(%selectron) && %free_target_style != 0)
      %selectron = startSelectron(0, 0);
    if (isObject(%selectron))
    {
      %selectron.addConstraint("360.173 309.959 217.714", "selected");
      $afxFreeTargetingSelectron = %selectron;
    }
    else
    {
      $afxFreeTargetingSelectron = "";
    }
  } 

  $afxFreeTargetingSelectron.book_slot = %book_slot;
  $afxFreeTargetingBookSlot = %book_slot;
}

function afxFinishFreeTargetingMode()
{
  if ($afxFreeTargetingBookSlot !$= "")
  {
    commandToServer('CastFreeTargetingSpellbookSpell', $afxFreeTargetingBookSlot, getFreeTargetPosition());
    $afxFreeTargetingBookSlot = "";
  }

  if (isObject($afxFreeTargetingSelectron))
  {
    $afxFreeTargetingSelectron.stopSelectron();
    $afxFreeTargetingSelectron = "";
  }
  PlayGui.popTargetingMode();
}

function afxCancelFreeTargetingMode()
{
  PlayGui.popTargetingMode();
  if (isObject($afxFreeTargetingSelectron))
  {
    $afxFreeTargetingSelectron.stopSelectron();
    $afxFreeTargetingSelectron = "";
    $afxFreeTargetingBookSlot = "";
  }
}

function afxInFreeTargetingMode()
{
  return (PlayGui.getTargetingMode() == $AFX::TARGETING_FREE);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function switch_fxTrigger(%set_state)
{
  if (afxInFreeTargetingMode())
  {
    if (isObject($afxFreeTargetingSelectron))
    {
      if (%set_state)
        $afxFreeTargetingSelectron.setTriggerBit(23);   // mask 0x800000
      else
        $afxFreeTargetingSelectron.clearTriggerBit(23); // mask 0x800000    
    }
    return;
  }

  %sel_obj = ServerConnection.getSelectedObj();
  if (isObject(%sel_obj) && isObject(%sel_obj.selectron))
  {
    if (%set_state)
      %sel_obj.selectron.setTriggerBit(23);   // mask 0x800000
    else
      %sel_obj.selectron.clearTriggerBit(23); // mask 0x800000
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//


