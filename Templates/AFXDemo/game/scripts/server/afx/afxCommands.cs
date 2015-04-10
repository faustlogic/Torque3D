
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// This script implements some client-callable functions for AFX.
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
// CASTING GBoF by KEY (example)

// Called from client: commandToServer('DoGreatBallCast', %target_ghost)
function serverCmdDoGreatBallCast(%client, %target_ghost)
{
  if (%target_ghost != -1)
    %target = %client.ResolveGhost(%target_ghost);
  else
    %target = -1;

  if (!isObject(GreatBallSpell_RPG) || !isObject(GreatBallSpell))
  {
    echo("Great Ball of Fire spell is not currently loaded.");
    DisplayScreenMessage(%client, "Great Ball of Fire spell is not currently loaded.");
    return;
  }
  
  DisplayScreenMessage(%client, "Casting" SPC "\"" @ GreatBallSpell_RPG.spellName @  "\" spell.");
  afxPerformSpellCast(%client.player, GreatBallSpell_RPG, GreatBallSpell, %target, %client);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPECIAL PHRASE-TESTER KEY COMMANDS

function serverCmdDoPhraseTesterPush(%client)
{
  performPhraseTesterPush(%client.player);
}

function serverCmdDoPhraseTesterHalt(%client)
{
  performPhraseTesterHalt(%client.player);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPELL INTERRUPTION BY KEY
//   only interrupts during casting stage

function serverCmdInterruptSpellcasting(%client)
{
  performSpellCastingInterrupt(%client);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// DIRECT DAMAGE INFLICTED BY KEY
//   for testing spellcasting interrupts
//   due to taking damage

function serverCmdInflictDamage(%client)
{
  if (isObject(%client.player))
    %client.player.damage(0, 0, 10, "User");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function serverCmdDoKeyTargetSelection(%client)
{
   %start = %client.player.getEyePoint();
   %vector = %client.player.getEyeVector();
   %vectorN = VectorNormalize(%vector);
   %vectorS = VectorScale(%vectorN,%distance);
   %center = VectorAdd(%vectorS,%start);  // Center of our radius search

   InitContainerRadiusSearch(%center, 24, 
      $TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType | $TypeMasks::VehicleObjectType);

   while ((%target = containerSearchNext()) != 0)
   {
      if (%target != %client.player)
      {
         // found a suitable target -- select it and return
         %client.setSelectedObj(%target, true);
         return;
      }
   }

   // no suitable target found -- clear selection
   %client.clearSelectedObj(true);
}
