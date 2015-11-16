
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// FIRE TOWER SPELL
//
//  After painstakingly assembling the 5 elusive fragments of the legendary
//  Staff of Vexatious Immolation, it's time to USE it to summon a massive fireball
//  spitting edifice... and RUN!
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

$AFX_VERSION = (isFunction(afxGetVersion)) ? afxGetVersion() : 1.02;
$MIN_REQUIRED_VERSION = 2.0;

// Test version requirements for this script
if ($AFX_VERSION < $MIN_REQUIRED_VERSION)
{
  error("AFX script " @ fileName($afxAutoloadScriptFile) @ " is not compatible with AFX versions older than " @ $MIN_REQUIRED_VERSION @ ".");
  return;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

//
// Here we test if the script is being reloaded or if this is the
// first time the script has executed this mission.
//
$spell_reload = isObject(FireInTheSkySpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = FireInTheSkySpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;

  // Fire in the Sky uses elements from these other effects
  afxExecPrerequisite("flaming_stick_trick.cs", $afxAutoloadScriptFolder);
  afxExecPrerequisite("ring_of_fire.cs", $afxAutoloadScriptFolder);
  afxExecPrerequisite("shards_of_vesuvius.cs", $afxAutoloadScriptFolder);
  afxExecPrerequisite("up_in_flames.cs", $afxAutoloadScriptFolder);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GLOBALS

$FitS_CastingScale = 1.3;

$FitS_Cue_StaffRitual = 0.0;
$FitS_Cue_FireRing = $FitS_Cue_StaffRitual + FlamingStickTrickSpell.staffPlantTime;
$FitS_Cue_Eruption = $FitS_Cue_FireRing+4.0+3.3;
$FitS_Cue_TowerRise  = $FitS_Cue_FireRing+4.0;
$FitS_Cue_StaffStrike = $FitS_Cue_FireRing;

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SCRIPT EVENTS

// This script-event calls the function that will launch Flaming Stick Trick
datablock afxScriptEventData(FitS_launchStaffRitual_CE)
{
  methodName = "launchStaffRitual";
};
//
datablock afxEffectWrapperData(FitS_launchStaffRitual_EW)
{
  effect = FitS_launchStaffRitual_CE;
  constraint = "anchor";
  delay = $FitS_Cue_StaffRitual;
};

// This script-event calls the function that will launch Ring of Fire
datablock afxScriptEventData(FitS_launchFireRing_CE)
{
  methodName = "launchFireRing";
};
//
datablock afxEffectWrapperData(FitS_launchFireRing_EW)
{
  effect = FitS_launchFireRing_CE;
  constraint = "zodeAnchor";
  delay = $FitS_Cue_FireRing;
};

// This script-event calls the function that will launch Shards of Vesuvius
datablock afxScriptEventData(FitS_launchEruption_CE)
{
  methodName = "launchEruption";
};
//
datablock afxEffectWrapperData(FitS_launchEruption_EW)
{
  effect = FitS_launchEruption_CE;
  constraint = "strikeLoc";
  delay = $FitS_Cue_Eruption;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING ZODIAC EYES

//
// Just before the tower emerges, as glow-beams shoot up from the
//  earth and the camera shakes, the eyes of the bull-skull appear,
//  glowing.
//
// Although inefficient, the texture used is the same resolution as
//  the bull-skull zodiac texture, and if composited over the eyes
//  would fall naturally into the eye sockets.  However since most of
//  the eye texture is black space it is presumably wasting texture
//  memory.  But doing it this way makes the eyes very simple to
//  place in the scene as no new offsets need to be found.
//

// Skull Eyes Casting Zode
datablock afxZodiacData(FitS_CastingZode_Skull_Eyes_CE : FST_CastingZode_Skull_CE)
{  
  texture = %mySpellDataPath @ "/FST/zodiacs/fst_casting_skull_eyes";
  color = "1 1 1 0.8";
};
//
datablock afxEffectWrapperData(FitS_CastingZode_Skull_Eyes_EW : FST_CastingZode_Skull_EW)
{
  effect = FitS_CastingZode_Skull_Eyes_CE;
  delay = $FitS_Cue_StaffStrike+4.0;
  lifetime = 1.55; 
  fadeInTime = 0.5;
  fadeOutTime = 0.5;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING ZODIAC PENTAGRAMS

// see Flaming Stick Trick for comments
$FitS_pentSpinOff = 15;

// pentagram casting zodiac #2, still, then spinning
//  -- 'trackOrientConstraint' will force the zodiac to respond to the spin xfm
datablock afxXM_SpinData(FitS_CastingZode_Pentagram_Still_spin_XM)
{
  spinAxis = "0 0 1";
  spinAngle = $FitS_pentSpinOff + 0;
  spinRate = -1000;
  delay = 5.0;
  fadeInTime = 5.0;
  fadeOutTime = 0;
};
//
datablock afxZodiacData(FitS_CastingZode_Pentagram_Still_CE : FST_CastingZode_Pentagram_CE)
{  
  startAngle = (($FitS_Cue_StaffStrike-(3.0+0.25))*-60.0)+$FST_Pentagram_startAngle;
  rotationRate = 0;
  trackOrientConstraint = true;
};

datablock afxEffectWrapperData(FitS_CastingZode_Pentagram_Still_EW)
{
  effect = FitS_CastingZode_Pentagram_Still_CE;
  constraint = "strikeLoc";
  delay = $FitS_Cue_StaffStrike;
  fadeInTime = 0;
  fadeOutTime = 1;
  lifetime = 9.0;
  xfmModifiers[0] = FitS_CastingZode_Pentagram_Still_spin_XM;
};

// pentagram under zodiac #2
datablock afxZodiacData(FitS_CastingZode_Pentagram_Still_Under_CE : FST_CastingZode_Pentagram_Under_CE)
{  
  startAngle = (($FitS_Cue_StaffStrike-(3.0+0.25))*-60.0)+$FST_Pentagram_startAngle;
  rotationRate = 0;
  trackOrientConstraint = true;
};
//
datablock afxEffectWrapperData(FitS_CastingZode_Pentagram_Still_Under_EW : FitS_CastingZode_Pentagram_Still_EW)
{
  effect = FitS_CastingZode_Pentagram_Still_Under_CE;
};

datablock afxZodiacData(FitS_CastingZode_Reveal_orangish_CE : FST_CastingZode_Reveal_CE)
{  
  color = "1 0.65 0.23 1.0";
};
datablock afxEffectWrapperData(FitS_CastingZode_FinalGlow_EW)
{
  effect = FitS_CastingZode_Reveal_orangish_CE;
  constraint = "zodeAnchor";
  delay = $FitS_Cue_TowerRise+0.6;
  lifetime = 5.0;
  fadeInTime = 0.5;
  fadeOutTime = 0.25;
};

datablock afxZodiacData(FitS_CastingZode_RevealFire_orangish_CE : FitS_CastingZode_Reveal_orangish_CE)
{  
  texture = %mySpellDataPath @ "/FST/zodiacs/fst_casting_wreath_reveal";
};
datablock afxEffectWrapperData(FitS_CastingZode_Fire_FinalGlow_EW : FitS_CastingZode_FinalGlow_EW)
{
  effect = FitS_CastingZode_RevealFire_orangish_CE;
};

datablock afxEffectWrapperData(FitS_CastingZode_Skull_FinalGlow_EW : FitS_CastingZode_FinalGlow_EW)
{
  effect = FST_CastingZode_GlowSkull_CE;
  constraint = "skullGlowAnchor";
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING BEAMS

datablock afxXM_SpinData(FitS_PentagramBeamB_spin_XM : FitS_CastingZode_Pentagram_Still_spin_XM)
{
  spinAngle = $FitS_pentSpinOff + 3;
};
//
datablock afxModelData(FitS_PentagramBeamB_CE)
{
  shapeFile = %mySpellDataPath @ "/FT/models/fits_pentaBeamB.dts";
  sequence = "beam";
  sequenceRate = 1.0;
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  useVertexAlpha = true;
};
datablock afxEffectWrapperData(FitS_PentagramBeamB_EW : FitS_CastingZode_Pentagram_Still_EW)
{
  effect = FitS_PentagramBeamB_CE;
  scaleFactor = 2.6*$FitS_CastingScale;
  fadeInTime = 0.5;
  xfmModifiers[0] = FitS_PentagramBeamB_spin_XM;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// RING OF FIRE

datablock afxZodiacData(FitS_SummonFireZode_Glow_CE)
{  
  texture = %mySpellDataPath @ "/RoF/zodiacs/rof_ember14_spider";
  radius = 10.0*$FitS_CastingScale;
  startAngle = 0;
  rotationRate = 0.0;  
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
  showOnInteriors = false;
};

datablock afxZodiacData(FitS_SummonFireZode_RingGlow_CE)
{  
  texture = %mySpellDataPath @ "/RoF/zodiacs/rof_ember_ring";
  radius = 10.0*$FitS_CastingScale;
  startAngle = 0;
  rotationRate = 0.0;  
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
  showOnInteriors = false;
};

datablock afxEffectWrapperData(FitS_SummonFireZode_Ring_FinalGlow_EW)
{
  effect = FitS_SummonFireZode_RingGlow_CE;
  constraint = "zodeAnchor";
  delay = $FitS_Cue_TowerRise+0.6;
  lifetime = 5.0;
  fadeInTime = 0.75;
  fadeOutTime = 0.25;
};
datablock afxEffectWrapperData(FitS_SummonFireZode_FinalGlow_EW)
{
  effect = FitS_SummonFireZode_Glow_CE;
  constraint = "zodeAnchor";
  delay = $FitS_Cue_TowerRise+0.6;
  lifetime = 5.0;
  fadeInTime = 0.75;
  fadeOutTime = 0.25;
};

datablock afxModelData(FitS_FinalBeams_CE)
{
  shapeFile = %mySpellDataPath @ "/FT/models/fits_finalBeams.dts";
  sequence = "beam";
  sequenceRate = 1.0/3.0;
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  useVertexAlpha = true;
};
//
datablock afxEffectWrapperData(FitS_FinalBeams_EW)
{
  effect = FitS_FinalBeams_CE;
  constraint = "zodeAnchor";
  scaleFactor = 1.6*$FitS_CastingScale;
  delay = $FitS_Cue_TowerRise+0.6+0.4;
  fadeinTime = 0.2;
  fadeOutTime = 0.3;
  lifetime = ((65/30)*3.0)-0.3  -2.0;
  sortPriority = -10;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Camera Shake

/*
datablock afxCameraShakeData(FitS_CamShake_Rumble1_CE)
{
   frequency = "10.0 10.0 0.0";
   amplitude = "1.0 1.0 0.0";
   radius = 25.0;
   falloff = 5.0;
};
//
datablock afxEffectWrapperData(FitS_CamShake_Rumble1_EW)
{
  effect = FitS_CamShake_Rumble1_CE;
  posConstraint = "strikeLoc";
  posConstraint2 = camera;
  delay  = $FitS_Cue_StaffStrike+4.0;  // timed with eyes
  lifetime = 20.0;
  //fadeInTime = 0.0;
};

datablock afxCameraShakeData(FitS_CamShake_Rumble2_CE)
{
   frequency = "10.0 10.0 10.0";
   amplitude = "5.0 5.0 2.5";
   radius = 25.0;
   falloff = 5.0;
};
//
datablock afxEffectWrapperData(FitS_CamShake_Rumble2_EW)
{
  effect = FitS_CamShake_Rumble2_CE;
  posConstraint = "strikeLoc";
  posConstraint2 = camera;
  delay  = $FitS_Cue_TowerRise+0.6+1.2;  // timed with 
  lifetime = 20.0;
  //fadeInTime = 0.0;
};

datablock afxCameraShakeData(FitS_CamShake_Rumble3_CE)
{
   frequency = "10.0 10.0 10.0";
   amplitude = "10.0 10.0 5.0";
   radius = 25.0;
   falloff = 5.0;
};
//
datablock afxEffectWrapperData(FitS_CamShake_Rumble3_EW)
{
  effect = FitS_CamShake_Rumble3_CE;
  posConstraint = "strikeLoc";
  posConstraint2 = camera;
  delay  = $FitS_Cue_TowerRise+0.6+1.2+1.5;  // timed with 
  lifetime = 20.0;
  //fadeInTime = 0.0;
};

datablock afxCameraShakeData(FitS_CamShake_Rumble4_CE)
{
   frequency = "10.0 10.0 10.0";
   amplitude = "20.0 20.0 10.0";
   radius = 25.0;
   falloff = 5.0;
};
//
datablock afxEffectWrapperData(FitS_CamShake_Rumble4_EW)
{
  effect = FitS_CamShake_Rumble4_CE;
  posConstraint = "strikeLoc";
  posConstraint2 = camera;
  delay  = $FitS_Cue_TowerRise+0.6+1.2+3.0;  // timed with 
  lifetime = 20.0;
  //fadeInTime = 0.0;
};
*/

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// TOWER ELEMENTS

// The model of the tower.
//  -- this model is created on the server
//
datablock afxStaticShapeData(FitS_Tower_CE)
{
  shapeFile = %mySpellDataPath @ "/FT/models/fits_fireTower.dts";
  sequence = "arise";
  sequenceRate = 1.0;
  shadowEnable = false;
  shadowCanMove = true;
  emap = true;
  doSpawn = true;
  silentBBoxValidation = true;
  remapTextureTags = "FT_stone6.png:towerShiny FT_stone6.png:towerDrab";
};
//
datablock afxEffectWrapperData(FitS_Tower_EW)
{
  effect = FitS_Tower_CE;
  constraint = "strikeLoc";
  effectName = "FireTower";
  ghostIsConstraintSrc = true;
  delay = $FitS_Cue_TowerRise+3.5;
  fadeInTime  = 0;
  fadeOutTime = 0;
  lifetime    = 11; 
};

function FitS_Tower_CE::onSpawn(%this, %tower, %effect_name)
{
  // Here we setup scripts and effects for the active tower
  FitS_LightTheTorch(%tower);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SCRIPT EVENTS

// One difficulty with this spell is that we want the caster to run
// forward and plant the staff in a location that is predetermined at
// the start of the spell. While we've locked the position of the
// caster, the user can still reorient it and this causes misalignment 
// the staff planting animation. If we were to also lock the caster's
// orientation, the camera would locked and this seems a little too harsh
// for the user. 
//
// Instead, we sample the caster's transform at the beginning of the spell
// and then reassign it to the caster a couple of times before the staff
// planting animation runs. It's far from perfect but it makes a pretty
// good interim solution.


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// PHYSICAL ZONE

// A cylidrical PhysicalZone is used to push loose objects out of 
// the way of the emerging tower.

datablock afxXM_WorldOffsetData(FitS_PZ_offset_XM)
{
  worldOffset = "0.0 0.0 -1.0";
};
//
datablock afxPhysicalZoneData(FitS_PhysZone_CE)
{
  velocityMod = 1;
  gravityMod = 1;
  appliedForce = "5000 5000 0";
  forceType = "cylindrical";
  polyhedron = "-12 12 0 24 0 0 0 -24 0 0 0 12";
};
//
datablock afxEffectWrapperData(FitS_PhysZone_EW)
{
  effect = FitS_PhysZone_CE;
  constraint = "strikeLoc";
  delay = $FitS_Cue_FireRing + (340/30 - 271/30);
  lifetime = 8;
  xfmModifiers[0] = FitS_PZ_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/fits_lighting_tge_sub.cs");
    exec("./audio/fits_audio_sub.cs");
  case "TGEA":
    exec("./lighting/fits_lighting_tgea_sub.cs");
    exec("./audio/fits_audio_sub.cs");
 case "T3D":
    exec("./lighting/fits_lighting_t3d_sub.cs");
    exec("./audio/fits_audio_sub.cs");
}

// exec Fireball Shooting sub-script. 
exec("./special/fits_fireball_sub.cs");

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// FIRE TOWER SPELL
//

datablock afxMagicSpellData(FireInTheSkySpell)
{
  echoPacketUsage = 20;
  castingDur = "$$ %%._castDur";

     // Flaming Stick Trick //
  addCastingEffect = FitS_launchStaffRitual_EW;

    // casting zodiac pentagrams (still, then spinning) //
  addCastingEffect = FitS_CastingZode_Pentagram_Still_Under_EW;
  addCastingEffect = FitS_CastingZode_Pentagram_Still_EW;
  addCastingEffect = FitS_PentagramBeamB_EW;

    // Ring of Fire //
  addCastingEffect = FitS_launchFireRing_EW;

    // physical zone //
  addCastingEffect = FitS_PhysZone_EW;

  // disabled by MAD
  //addCastingEffect = FitS_CamShake_Rumble1_EW;
  //addCastingEffect = FitS_CamShake_Rumble2_EW;
  //addCastingEffect = FitS_CamShake_Rumble3_EW;
  //addCastingEffect = FitS_CamShake_Rumble4_EW;

  addCastingEffect = FitS_CastingZode_Skull_Eyes_EW;
  addCastingEffect = FitS_CastingZode_FinalGlow_EW;
  addCastingEffect = FitS_CastingZode_Fire_FinalGlow_EW;
  addCastingEffect = FitS_CastingZode_Skull_FinalGlow_EW;
  addCastingEffect = FitS_SummonFireZode_Ring_FinalGlow_EW;
  addCastingEffect = FitS_SummonFireZode_FinalGlow_EW;
  addCastingEffect = FitS_FinalBeams_EW;

    // Shards of Vesuvius //
  addCastingEffect = FitS_launchEruption_EW;

    // the tower rises //
  addCastingEffect = FitS_Tower_EW;  
};

// sounds and lights added via sub-script functions //
FitS_add_Lighting_FX(FireInTheSkySpell);
FitS_add_Audio_FX(FireInTheSkySpell);
FitS_add_fireball_Audio_FX(FitS_FireBall_Shooter);

datablock afxRPGMagicSpellData(FireInTheSkySpell_RPG)
{
  spellName = "Fire in the Sky";
  desc = "After painstakingly assembling the 5 elusive fragments of the " @
         "legendary <font:Arial Italic:16>Staff of Vexatious Immolation<font:Arial:16>, " @ 
         "it's time to USE it to summon a massive fireball spitting edifice... and RUN!" @
         "\n" @
         "\n<font:Arial Italic:14>spell design and concept: <font:Arial:14>Matthew Durante" @
         "\n<font:Arial Italic:14>sound design: <font:Arial:14>Matt Pacyga";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/FT/icons/ft";
  manaCost = 10;
  castingDur = 5.2;
  _castDur = 5.2;
  _skull = 1;
  _beam = 1;
  _sym = "pent";
};

function FireInTheSkySpell::onActivate(%this, %spell, %caster, %target)
{
  Parent::onActivate(%this, %spell, %caster, %target);

  // Setup some fixed constraints relative to the caster.
  %spell.addConstraint(%caster.getTransform(), "anchor");
  %spell.addConstraint(FlamingStickTrickSpell.calcStrikeLoc(%caster), "strikeLoc");
  %spell.addConstraint(FlamingStickTrickSpell.calcZodeAnchor(%caster), "zodeAnchor");
  %spell.addConstraint(FlamingStickTrickSpell.calcSkullAnchor(%caster), "skullAnchor");
  %spell.addConstraint(FlamingStickTrickSpell.calcSkullGlowAnchor(%caster), "skullGlowAnchor");
}

function FireInTheSkySpell::onInterrupt(%this, %spell, %caster)
{
  Parent::onInterrupt(%this, %spell, %caster);
  if (isObject(%spell.fst_spell))
    %spell.fst_spell.interrupt();
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SCRIPT EVENTS

// This method is called when it's time to launch the staff ritual effect.
//
function FireInTheSkySpell::launchStaffRitual(%this, %spell, %caster, %consObj, %xfm, %data)
{
  %fst_spell = new afxMagicSpell() {
    datablock = FlamingStickTrickSpell;
    caster = %caster;
    _castDur = 0;
    _skull = 1;
    _beam = 1;
    _sym = "pent";
    _cine = 0;
    _conjure = 0;
  };
  %spell.fst_spell = %fst_spell;
}

// This method is called when it's time to launch the ring-of-fire effect.
//
function FireInTheSkySpell::launchFireRing(%this, %spell, %caster, %consObj, %xfm, %data)
{
  %rof_spell = new afxMagicSpell() {
    datablock = RingOfFireSpell;
    caster = %caster;
    _castDur = 0;
    _anim = 0;
    _casterAnchor = 0;
    _flavor = "spider";
    _ringDur = 3.1;
  };

  %rof_spell.addConstraint(%xfm, "RingAnchor");
}

// This method is called when it's time to launch the eruption effect.
//
function FireInTheSkySpell::launchEruption(%this, %spell, %caster, %consObj, %xfm, %data)
{
  %sov_spell = new afxMagicSpell() {
    datablock = ShardsOfVesuviusSpell;
    caster = %caster;
  };
  %sov_spell.addConstraint(%xfm, "freeTarget");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectWrapperData(FT_TowerProp_EW)
{
  effect = FitS_Tower_CE;
  constraint = "anchor";
  effectName = "FireTowerProp";
  ghostIsConstraintSrc = true;
  delay = 0;
  fadeInTime  = 2;
  fadeOutTime = 0;
  lifetime = 8; 
};

datablock afxEffectronData(FireTowerPropEffectron)
{
  duration = 1.0;
  addEffect = FT_TowerProp_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxMagicSpellData(FireTowerPropSpell)
{
  castingDur = 0;
};

datablock afxRPGMagicSpellData(FireTowerPropSpell_RPG)
{
  spellName = "Fire Tower Prop";
  desc = "Directly place an active Fire Tower.";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/FT/icons/ft";
  manaCost = 10;
  castingDur = 0;
  target = "free";
};

function FireTowerPropSpell::onLaunch(%this, %spell, %caster, %target, %missile)
{
  Parent::onLaunch(%this, %spell, %caster, %target, %missile);

  %anchor = %spell.freeTarget SPC getWords(%caster.getTransform(), 3);
  %effectron = startEffectron(FireTowerPropEffectron, %anchor, "anchor");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

if ($spell_reload)
{
  // Removes then adds all reloaded datablocks
  touchDataBlocks();
}
else
{
  // save script filename and data path for reloads
  FireInTheSkySpell.scriptFile = $afxAutoloadScriptFile;
  FireInTheSkySpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
  {
    addDemoSpellbookSpell(FireInTheSkySpell, FireInTheSkySpell_RPG);
    addDemoSpellbookSpell(FireTowerPropSpell, FireTowerPropSpell_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
