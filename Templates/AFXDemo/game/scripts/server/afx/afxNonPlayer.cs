
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// These are the standard AIPlayer callouts
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

function NonPlayerData::onReachDestination(%this, %obj)
{
  if (%obj.path !$= "") 
  {
    if (%obj.currentNode == %obj.targetNode)
      %this.onEndOfPath(%obj, %obj.path);
    else
      %obj.moveToNextNode();
  }
}

function NonPlayerData::onMoveStuck(%this, %obj)
{
  //echo("NonPlayerData::onMoveStuck() -- " @ %obj @ " -- " @ %obj.getShapeName());
}

function NonPlayerData::onTargetEnterLOS(%this, %obj)
{
  //echo("NonPlayerData::onTargetEnterLOS() -- " @ %obj @ " -- " @ %obj.getShapeName());
}

function NonPlayerData::onTargetExitLOS(%this, %obj)
{
  //echo("NonPlayerData::onTargetExitLOS() -- " @ %obj @ " -- " @ %obj.getShapeName());
}

// These are the script-extended AIPlayer methods

function NonPlayerData::onEndOfPath(%this, %obj, %path)
{
  //echo("NonPlayerData::onEndOfPath() -- " @ %obj @ " -- " @ %obj.getShapeName());
  %obj.nextTask();
}

function NonPlayerData::onRemove(%this, %npc)
{
  if (isFunction(unequipSciFiOrc))
    unequipSciFiOrc(%npc);

  if (isObject(%npc.lingering_spell))
  {
    %npc.lingering_spell.interrupt();
    %npc.lingering_spell = "";
  }

  Parent::onRemove(%this, %npc);
}

// This is a ShapeBase callout

function NonPlayerData::onEndSequence(%this, %obj, %slot)
{
  %obj.stopThread(%slot);
  %obj.nextTask();
}

// AIPlayer script-extended methods

function AIPlayer::spawnAt(%name, %xfm)
{
  %npc = new AiPlayer()
  {
    datablock = NonPlayerData;
    path = "";
  };
  MissionCleanup.add(%npc);

  %npc.setShapeName(%name);
  %npc.setTransform(%xfm);
  %npc.setMoveSpeed(0.4);

  return %npc;
}

function AIPlayer::followPath(%this, %path, %node)
{
  %this.stopThread(0);

  if (!isObject(%path))
  {
    %this.path = "";
    return;
  }

  // wrap-around?
  if (%node >= %path.getCount())
    %node = %path.getCount()-1;

  %this.targetNode = %node;

  // alter path?
  if (%this.path !$= %path)
  {
    %this.path = %path;
    %this.moveToNode(0);
  }
  else
    %this.moveToNode(%this.currentNode);
}

function AIPlayer::moveToNextNode(%this)
{
  if (%this.pathTraversal $= "randomized")
  {
    %next_waypoint = %this.currentNode;
    while (%next_waypoint == %this.currentNode)
      %next_waypoint = getRandom(%this.path.getCount() - 1);
    %this.moveToNode(%next_waypoint);
  }
  else // if (%this.pathTraversal $= "sequential")
  {
    // increasing traversal
    if (%this.targetNode < 0 || %this.currentNode < %this.targetNode) 
    {
      if (%this.currentNode < %this.path.getCount() - 1)
        %this.moveToNode(%this.currentNode + 1);
      else
        %this.moveToNode(0);
    }
    // decreasing traversal
    else
    {
      if (%this.currentNode == 0)
        %this.moveToNode(%this.path.getCount() - 1);
      else
        %this.moveToNode(%this.currentNode - 1);
    }
  }
}

function AIPlayer::moveToNode(%this, %index)
{
  // Move to the given path node index
  %this.currentNode = %index;
  %node = %this.path.getObject(%index);
  %this.setMoveDestination(%node.getTransform(), %index == %this.targetNode);
}

//~~~~~~~~~~~~~~~~~~~~//

function AIPlayer::pushTask(%this, %method)
{
  if (%this.taskIndex $= "")
  {
    %this.taskIndex = 0;
    %this.taskCurrent = -1;
  }

  %this.task[%this.taskIndex] = %method; 
  %this.taskIndex++;

  if (%this.taskCurrent == -1)
    %this.executeTask(%this.taskIndex - 1);
}

function AIPlayer::clearTasks(%this)
{
  %this.taskIndex = 0;
  %this.taskCurrent = -1;
}

function AIPlayer::nextTask(%this)
{
  if (%this.taskCurrent != -1)
  {
    if (%this.taskCurrent < %this.taskIndex - 1)
      %this.executeTask(%this.taskCurrent++);
    else
      %this.taskCurrent = -1;
  }
}

function AIPlayer::executeTask(%this,%index)
{
  %this.taskCurrent = %index;
  eval(%this.getId() @ "." @ %this.task[%index] @ ";");
}

function AIPlayer::singleShot(%this)
{
  // The shooting delay is used to pulse the trigger
  %this.setImageTrigger(0,true);
  %this.setImageTrigger(0,false);
  %this.trigger = %this.schedule(%this.shootingDelay,singleShot);
}

//~~~~~~~~~~~~~~~~~~~~//

function AIPlayer::wait(%this,%time)
{
  %this.schedule(%time*1000, "nextTask");
}

function AIPlayer::done(%this,%time)
{
  %this.schedule(0, "delete");
}

function AIPlayer::fire(%this, %val)
{
  if (%val) 
  {
    cancel(%this.trigger);
    %this.singleShot();
  }
  else
    cancel(%this.trigger);

  %this.nextTask();
}

function AIPlayer::aimAt(%this, %object)
{
  %this.setAimObject(%object);
  %this.nextTask();
}

function AIPlayer::animate(%this, %seq)
{
  %this.setActionThread(%seq);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
