//-----------------------------------------------------------------------------
// Torque
// Copyright GarageGames, LLC 2011
//-----------------------------------------------------------------------------

function CrossbowProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
   // Apply damage to the object all shape base objects
   if (%col.getType() & $TypeMasks::ShapeBaseObjectType)
      %col.damage(%obj,%pos,%this.directDamage,"CrossbowBolt");

   // Radius damage is a support scripts defined in radiusDamage.cs
   // Push the contact point away from the contact surface slightly
   // along the contact normal to derive the explosion center. -dbs
   radiusDamage(%obj, %pos, %this.damageRadius, %this.radiusDamage, "Radius", %this.areaImpulse);
}

function CrossbowImage::onFire(%this, %obj, %slot)
{
   %projectile = %this.projectile;

   // Decrement inventory ammo. The image's ammo state is update
   // automatically by the ammo inventory hooks.
   %obj.decInventory(%this.ammo,1);

   // Determine initial projectile velocity based on the 
   // gun's muzzle point and the object's current velocity
   %muzzleVector = %obj.getMuzzleVector(%slot);
   %objectVelocity = %obj.getVelocity();
   %muzzleVelocity = VectorAdd(
      VectorScale(%muzzleVector, %projectile.muzzleVelocity),
      VectorScale(%objectVelocity, %projectile.velInheritFactor));

   // Create the projectile object
   %p = new (%this.projectileType)() {
      dataBlock        = %projectile;
      initialVelocity  = %muzzleVelocity;
      initialPosition  = %obj.getMuzzlePoint(%slot);
      sourceObject     = %obj;
      sourceSlot       = %slot;
      client           = %obj.client;
   };
   MissionCleanup.add(%p);
   return %p;
}
