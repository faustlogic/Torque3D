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

// Inventory items.  These objects rely on the item & inventory support
// system defined in item.cs and inventory.cs

//-----------------------------------------------------------------------------
// Health Patches cannot be picked up and are not meant to be added to inventory.
// Health is applied automatically when an objects collides with a patch.
//-----------------------------------------------------------------------------

function HealthPotion::onCollision(%this, %obj, %col)
{
  if (%col.getState() !$= "Dead" ) 
  {
    if (%col.getDamageLevel() > 0)
      %col.setDamageLevel(%col.getDamageLevel() - %this.repairAmount);

    serverPlay3D(HealthUseSound,%obj.getTransform());

    %obj.delete();
  }
  
   /*
   // Apply health to colliding object if it needs it.
   // Works for all shapebase objects.
   if (%col.getDamageLevel() != 0 && %col.getState() !$= "Dead")
   {
      %col.applyRepair(%this.repairAmount);

      serverPlay3D(HealthUseSound, %obj.getTransform());

      %obj.delete();
   }
   */
}


