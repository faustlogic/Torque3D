 
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// This script implements standard afxMagicSpell callouts, script functions that are
// called from C++ code at key points in a Magic Spells's lifetime. Spell-specific 
// variations can be made for customized behaviors.
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
//  Default callout script methods called from afxMagicSpell

function afxMagicSpellData::onActivate(%this, %spell, %caster, %target)
{
  //echo("Default afxMagicSpellData::onActivate()");

  // attach active spell to caster. this prevent overlapping casts.
  if (%caster.spellBeingCast $= "")
    %caster.spellBeingCast = %spell;

  // set mana recharge-rate to zero while casting
  %caster.setRechargeRate(0.0);

  // initiate global cooldown on spellbook
  if (isObject(%caster.activeSpellbook))
    %caster.activeSpellbook.startAllSpellCooldown();
}

function afxMagicSpellData::onLaunch(%this, %spell, %caster, %target, %missile)
{
  if (!isObject(%caster))
    return false;

  if (%caster.spellBeingCast == %spell)
    %caster.spellBeingCast = "";

  %caster.setRechargeRate(%caster.getDataBlock().rechargeRate);

  %rpg_data = %spell.extra;
  if (isObject(%rpg_data))
  {
    %mana_cost = %rpg_data.manaCost;
    %mana_pool = %caster.getEnergyLevel();
    %caster.setEnergyLevel(%mana_pool - %mana_cost);
  }

  return true;
}

function afxMagicSpellData::onImpact(%this, %spell, %caster, %impObj, %impPos, %impNorm)
{
  //echo("Default afxMagicSpellData::onImpact()");

  %rpg_data = %spell.extra;
  if (%rpg_data.directDamage != 0 || %rpg_data.areaDamageRadius > 0)
  {
    %dd_amt = %rpg_data.directDamage;
    %ad_amt = %rpg_data.areaDamage;
    %ad_rad = %rpg_data.areaDamageRadius;
    %ad_imp = %rpg_data.areaDamageImpulse;

    %this.onDamage(%spell, "directDamage", "spell", %impObj, %dd_amt, 0,
                   %impPos, %ad_amt, %ad_rad, %ad_imp);               
  }
}

function afxMagicSpellData::onInterrupt(%this, %spell, %caster)
{
  //echo("Default afxMagicSpellData::onInterrupt()");

  if (isObject(%caster))
  {
    if (%caster.spellBeingCast == %spell)
      %caster.spellBeingCast = "";
    %caster.setRechargeRate(%caster.getDataBlock().rechargeRate);
  }
}

function afxMagicSpellData::onDeactivate(%this, %spell)
{
  //echo("Default afxMagicSpellData::onDeactivate()");
}

function afxMagicSpellData::readyToCast(%this, %caster, %target)
{
  //echo("Default afxMagicSpellData::readyToCast()");
  return true;
}

function afxMagicSpellData::onDamage(%this, %spell, %label, %flavor, %damaged_obj, 
                                     %amount, %count, %pos, %ad_amount, %radius, %impulse)
{
  // deal the direct damage
  if (isObject(%damaged_obj) && (%damaged_obj.getType() & $TypeMasks::PlayerObjectType))
    %damaged_obj.damage(%spell, %pos, %amount, %flavor);

  // deal area damage
  if (%radius > 0)
  {
    radiusDamage(%spell, %pos, %radius, %ad_amount, %flavor, %impulse);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
