
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
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

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function PlayerDataAFX::onAdd(%this,%obj)
{
   // Vehicle timeout
   %obj.mountVehicle = true;

   // Default dynamic armor stats
   %obj.setRechargeRate(%this.rechargeRate);
   %obj.setRepairRate(%this.repairRate);
}

function PlayerDataAFX::onRemove(%this, %obj)
{
  if (isFunction(unequipSciFiOrc))
    unequipSciFiOrc(%obj);

  if (isObject(%obj.lingering_spell))
  {
    %obj.lingering_spell.interrupt();
    %obj.lingering_spell = "";
  }

  if (%obj.client.player == %obj)
    %obj.client.player = 0;
}

//~~~~~~~~~~~~~~~~~~~~//

function PlayerDataAFX::onCollision(%this,%obj,%col)
{
   if (%obj.getState() $= "Dead")
      return;

    // Try and pickup all items
    if (%col.getClassName() $= "Item")
    {
        %obj.pickup(%col);
        return;
    }


    // Mount vehicles
    if (%col.getType() & $TypeMasks::GameBaseObjectType)
    {
        %db = %col.getDataBlock();
        if ((%db.getClassName() $= "WheeledVehicleData") && %obj.mountVehicle && %obj.getState() $= "Move" && %col.mountable)
        {
            // Only mount drivers for now.
            %node = 0;
            %col.mountObject(%obj, %node);
            %obj.mVehicle = %col;
        }
    }
}

function PlayerDataAFX::onImpact(%this, %obj, %collidedObject, %vec, %vecLen)
{
   %obj.damage(0, VectorAdd(%obj.getPosition(),%vec),
      %vecLen * %this.speedDamageScale, "Impact");
}

//~~~~~~~~~~~~~~~~~~~~//

function PlayerDataAFX::damage(%this, %obj, %sourceObject, %position, %damage, %damageType)
{
   if (%obj.getState() $= "Dead")
      return;

   // setting damage-level directly allows negative damage for healing
   %obj.setDamageLevel(%obj.getDamageLevel() + %damage);

   %location = "Body";

   // handle possible spell interruption due to damage
   afxTestSpellcastingDamageInterruption(%obj, %damage, %damageType);

   // Deal with client callbacks here because we don't have this
   // information in the onDamage or onDisable methods
   %client = %obj.client;
   %sourceClient = %sourceObject ? %sourceObject.client : "";

   if (%obj.getState() $= "Dead")
   {
     %obj.setShapeName("Dead Orc");
     %obj.setRepairRate(0);
     %obj.setEnergyLevel(0);
     %obj.setRechargeRate(0);
	 
	 if (isObject(%client))
         %client.onDeathAFX(%sourceObject, %sourceClient, %damageType, %location);
   }
}

function PlayerDataAFX::onDamage(%this, %obj, %delta)
{
    // If flying_damage_text.cs is loaded, displayFlyingDamageText() exists
    // and we call it to have the damage amount shown as an effect.
    if (%delta >= 1 && isFunction(displayFlyingDamageText))
      displayFlyingDamageText(%obj, mFloor(%delta));

   // This method is invoked by the ShapeBase code whenever the
   // object's damage level changes.
   if (%delta > 0 && %obj.getState() !$= "Dead") {

      // Increment the flash based on the amount.
      %flash = %obj.getDamageFlash() + ((%delta / %this.maxDamage) * 2);
      if (%flash > 0.75)
         %flash = 0.75;
      %obj.setDamageFlash(%flash);

      // If the pain is excessive, let's hear about it.
      if (%delta > 10)
         %obj.playPain();
   }
}

function PlayerDataAFX::onDisabled(%this,%obj,%state)
{
   // The player object sets the "disabled" state when damage exceeds
   // it's maxDamage value.  This is method is invoked by ShapeBase
   // state mangement code.

   // If we want to deal with the damage information that actually
   // caused this death, then we would have to move this code into
   // the script "damage" method.
   %obj.playDeathCry();
   %obj.playDeathAnimation();
   %obj.setDamageFlash(0.75);

   // Release the main weapon trigger
   %obj.setImageTrigger(0,false);

   // Unlike the default onDisabled(), we don't do any
   // automatic corpse disposal.
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

