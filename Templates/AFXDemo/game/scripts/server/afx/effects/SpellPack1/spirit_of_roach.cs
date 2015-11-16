
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SPIRIT OF ROACH SPELL
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
$MIN_REQUIRED_VERSION = 1.12;

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
$spell_reload = isObject(SpiritOfRoachSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = SpiritOfRoachSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

$BrightLighting_mask = 0x800000; // BIT(23); 

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING ZODIAC

// this mooring anchors the casting zodiacs and measures
// their altitudes

datablock afxMooringData(SoR_CastingZodeMooring_CE)
{
  networking = $AFX::CLIENT_ONLY;
  displayAxisMarker = false;
};
datablock afxEffectWrapperData(SoR_CastingZodeMooring_EW)
{
  effect = SoR_CastingZodeMooring_CE;
  posConstraint = caster;
  effectName = "CastingZodeMooring";
  isConstraintSrc = true;
  lifetime = 4.0;
  xfmModifiers[0] = SHARED_freeze_AltitudeConform_XM;
};

datablock afxZodiacData(SoR_ZodeReveal_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/SoR/zodiacs/SoR_caster_reveal";
  radius = 3.0;
  startAngle = 7.5; //0.0+7.5
  rotationRate = -30.0;  
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(SoR_ZodeReveal_EW)
{
  effect = SoR_ZodeReveal_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = 0.0; //0.01;
  lifetime = 0.75;
  fadeInTime = 0.5;
  fadeOutTime = 0.25;
};

// purple and blue primary zodiac
datablock afxZodiacData(SoR_Zode1_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/SoR/zodiacs/SoR_caster";
  radius = 3.0;
  startAngle = 0.0;
  rotationRate = -30.0;
  color = "1.0 1.0 1.0 0.9";
  blend = additive;
};
//
datablock afxEffectWrapperData(SoR_Zode1_EW)
{
  effect = SoR_Zode1_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = 0.25;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 2.5;
};

// Main Zode Underglow
//  Glowing zodiacs blended additively tend to be washed-out atop
//  light groundplanes, as is the case with TLK and the sand-colored
//  ground in the demo.  To make them visually "pop" the ground must
//  be darkened, and this is done here.  The "underglow" zodiac is
//  a copy of the glow zodiac that is blended normally.  Because the
//  glow zodiacs have halos extending beyond their opaque regions
//  that blend with black, the ground is subtly darkened.  As the
//  glow is layered atop it -- it pops!  Increasing the color value
//  increases the effect.
datablock afxZodiacData(SoR_Zode1_underglow_CE : SoR_Zode1_CE)
{
  color = "0.75 0.75 0.75 0.75";
  blend = normal;
};
//
datablock afxEffectWrapperData(SoR_Zode1_underglow_EW : SoR_Zode1_EW)
{
  effect = SoR_Zode1_underglow_CE;
  execConditions = $BrightLighting_mask;
};

// purple runes (inner ring) and skulls (outer ring)
datablock afxZodiacData(SoR_Zode2_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/SoR/zodiacs/zode_text";
  radius = 3.0;
  startAngle = 0.0;
  rotationRate = 20.0; //60
  color = "0.861 0.0 0.9 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(SoR_Zode2_EW)
{
  effect = SoR_Zode2_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = 0.25;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 2.5;
};

// sketchy white glyph symbols
datablock afxZodiacData(SoR_Zode3_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/SoR/zodiacs/zode_symbols";
  radius = 3.0;
  startAngle = 0.0;
  rotationRate = -30.0;
  color = "1.0 1.0 1.0 0.5";
  blend = additive;
};
//
datablock afxEffectWrapperData(SoR_Zode3_EW)
{
  effect = SoR_Zode3_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = 0.25;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 2.5;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPELLCASTER ANIMATION

datablock afxAnimClipData(SoR_Summon_Clip_CE)
{
  clipName = "summon";
  rate = 0.35;
};

datablock afxEffectWrapperData(SoR_Summon_Clip_EW)
{
  effect = SoR_Summon_Clip_CE;
  constraint = "caster";
  lifetime = 1.75;
  delay = 1.0;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// FALLOUT RING

datablock afxZodiacData(SoR_FalloutZode1_CE)
{  
  texture = %mySpellDataPath @ "/SoR/zodiacs/SoR_castingA";
  radius = 1.0;
  startAngle = 0.0;
  rotationRate = 80.0; //40.0;
  color = "1.0 1.0 1.0 0.3";
  blend = additive;
  growthRate = 20.0; //30.0;
  showOnInteriors = false;
};
//
datablock afxEffectWrapperData(SoR_FalloutZode1_EW)
{
  effect = SoR_FalloutZode1_CE;
  posConstraint = "caster";
  delay = 1.25;
  fadeInTime = 0.25;
  fadeOutTime = 2.75;
  lifetime = 1.25;
  xfmModifiers[0] = SHARED_freeze_XM;
};

datablock afxZodiacData(SoR_FalloutZode2_CE : SoR_FalloutZode1_CE)
{  
  texture = %mySpellDataPath @ "/SoR/zodiacs/SoR_castingB";
  rotationRate = -80.0; //-40.0;
  showOnInteriors = false;
};
//
datablock afxEffectWrapperData(SoR_FalloutZode2_EW : SoR_FalloutZode1_EW)
{
  effect = SoR_FalloutZode2_CE;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// ROACH HALO

//
// A neon-like roach icon hovers above the target's head for a
// a shorttime. Note the use of a camera aim constraint to force
// the 2D icon to always face the camera.
//

datablock afxModelData(SoR_Halo_CE)
{
   shapeFile = %mySpellDataPath @ "/SoR/models/SoR_plane.dts";
   forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
};

datablock afxXM_WorldOffsetData(SoR_Halo_offset_XM)
{
  worldOffset = "0.0 0.0 3.4";
};
datablock afxXM_AimData(SoR_Halo_aim_XM)
{
  aimZOnly = false;
};
datablock afxEffectWrapperData(SoR_Halo_EW)
{
  effect        = SoR_Halo_CE;

  posConstraint = "impactedObject";
  posConstraint2 = camera;
  delay = 0.5;
  fadeInTime  = 0.5;
  fadeOutTime = 0.5;
  lifetime = 2.5;
  scaleFactor = 1.4;
  xfmModifiers[0] = SoR_Halo_offset_XM;
  xfmModifiers[1] = SoR_Halo_aim_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SHIELD BUBBLE

//
// A bubble of protection with insect wing details surrounds
// the target for a short time.
//

datablock afxModelData(SoR_Shield_CE)
{
  shapeFile = %mySpellDataPath @ "/SoR/models/SoR_sphere.dts";
  sequence = "bubble";
  alphaMult = 0.7;
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
};

datablock afxXM_WorldOffsetData(SoR_Shield_offset_XM)
{
  worldOffset = "0.0 0.0 -0.55";
};
datablock afxXM_SpinData(SoR_Shield_spin_XM)
{
  spinAxis = "0 0 1";
  spinRate = 120.0;
};
datablock afxEffectWrapperData(SoR_Shield_EW)
{
  effect = SoR_Shield_CE;
  posConstraint = "impactedObject";
  fadeInTime  = 0.5;
  fadeOutTime = 0.5;
  lifetime = 5.5;
  xfmModifiers[0] = SoR_Shield_offset_XM;
  xfmModifiers[1] = SoR_Shield_spin_XM;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// TARGET ZODIAC

// this mooring anchors the target zodiacs and measures
// their altitudes

datablock afxMooringData(SoR_TargetZodeMooring_CE)
{
  networking = $AFX::CLIENT_ONLY;
  displayAxisMarker = false;
};
datablock afxEffectWrapperData(SoR_TargetZodeMooring_EW)
{
  effect = SoR_TargetZodeMooring_CE;
  posConstraint = target;
  effectName = "TargetZodeMooring";
  isConstraintSrc = true;
  lifetime = 7.0;
  xfmModifiers[0] = SHARED_AltitudeConform_XM;
};

//
// This is a simple barbed zodiac that appears beneath the target
// of Spirit of Roach. Since this spell can be cast upon oneself,
// the spellcaster can also be the target. This zodiac is designed
// to combine well with the casting zodiac.
//

// the reveal glow for the target's zodiac
datablock afxZodiacData(SoR_TargetRevealZode_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/SoR/zodiacs/SoR_target_reveal";
  radius = 1.59;
  startAngle = 18.75;
  rotationRate = -75.0; //-45.0; //-30.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(SoR_TargetRevealZode_EW)
{
  effect = SoR_TargetRevealZode_CE;
  posConstraint = "#effect.TargetZodeMooring";
  borrowAltitudes = true;
  lifetime = 0.75;
  fadeInTime = 0.5;
  fadeOutTime = 0.25;
  delay = 0.0;
};

// the barbed zodiac ring that appears under the target
datablock afxZodiacData(SoR_TargetZode_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/SoR/zodiacs/SoR_target";
  radius = 1.59; //3.0;
  startAngle = 0.0;
  rotationRate = -75.0; //-45.0; //-30.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(SoR_TargetZode_EW)
{
  effect = SoR_TargetZode_CE;
  posConstraint = "#effect.TargetZodeMooring";
  borrowAltitudes = true;
  delay = 0.25;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 4.75;
};

// Target Zode Underglow
datablock afxZodiacData(SoR_TargetZode_underglow_CE : SoR_TargetZode_CE)
{
  color = "0.5 0.5 0.5 0.5";
  blend = normal;
};
//
datablock afxEffectWrapperData(SoR_TargetZode_underglow_EW : SoR_TargetZode_EW)
{
  effect = SoR_TargetZode_underglow_CE;
  execConditions = $BrightLighting_mask;
};

/*
datablock afxGuiControllerData(SoR_CastingBar_CE)
{
  controlName = SpellCastBar2;
  preservePosition = true;
  controllingClientOnly = true;
};
datablock afxEffectWrapperData(SoR_CastingBar_EW)
{
  effect = SoR_CastingBar_CE;
  posConstraint = "caster";
  lifetime = 1.0;
  fadeInTime = 0.25;
  fadeOutTime = 1;
};
*/

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/sor_lighting_tge_sub.cs");
    exec("./audio/sor_audio_sub.cs");
  case "TGEA":
    exec("./lighting/sor_lighting_tgea_sub.cs");
    exec("./audio/sor_audio_sub.cs");
  case "T3D":
    exec("./lighting/sor_lighting_t3d_sub.cs");
    exec("./audio/sor_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPIRIT OF ROACH
//

datablock afxMagicSpellData(SpiritOfRoachSpell)
{
  reagentCost = 2;
  reagentName = "Cockroach Carapace";

    // warmup //
  castingDur = 1.0;

  //addCastingEffect = SoR_CastingBar_EW;

    // casting zodiac //
  addCastingEffect = SoR_CastingZodeMooring_EW;
  addCastingEffect = SoR_ZodeReveal_EW;
  addCastingEffect = SoR_Zode1_underglow_EW;
  addCastingEffect = SoR_Zode1_EW;
  addCastingEffect = SoR_Zode2_EW;
  addCastingEffect = SoR_Zode3_EW;
    // spellcaster animation //
  addCastingEffect = SoR_Summon_Clip_EW;
    // fallout ring //
  addCastingEffect = SoR_FalloutZode1_EW;
  addCastingEffect = SoR_FalloutZode2_EW;

    // roach icon halo //
  addImpactEffect = SoR_Halo_EW;
    // shield bubble //
  addImpactEffect = SoR_Shield_EW;
    // target zodiac //
  addImpactEffect = SoR_TargetZodeMooring_EW;
  addImpactEffect = SoR_TargetRevealZode_EW;
  addImpactEffect = SoR_TargetZode_underglow_EW;
  addImpactEffect = SoR_TargetZode_EW;
};

// sounds and lights added via sub-script functions //
SoR_add_Lighting_FX(SpiritOfRoachSpell);
SoR_add_Audio_FX(SpiritOfRoachSpell);

datablock afxRPGMagicSpellData(SpiritOfRoachSpell_RPG)
{
  spellName = "Spirit of Roach";
  desc = "The proclivity of the lowly cockroach to RESIST all imaginable forms of MAGIC " @
         "is now yours. Respect the Cockroach!" @
         "\n" @
         "\nspell design: Matthew Durante" @
         "\nsound effects: Dave Schroeder" @ 
         "\nspell concept: Jeff Faust";
  sourcePack = "Spell Pack 1";
         
  iconBitmap = %mySpellDataPath @ "/SoR/icons/sor";
  target = "friend";
  canTargetSelf = true;
  range = 40;
  manaCost = 10;
  reagentCost = 2;
  reagentName = "Cockroach Carapace";
  castingDur = SpiritOfRoachSpell.castingDur;
};

// set a level of detail
function SpiritOfRoachSpell::onActivate(%this, %spell, %caster, %target)
{
  Parent::onActivate(%this, %spell, %caster, %target);

  if (isObject(theLevelInfo))
  {
    if (theLevelInfo.hasBrightLighting)
      %spell.setExecConditions($BrightLighting_mask);
  }
  else if (isObject(MissionInfo))
  {
    if (MissionInfo.hasBrightLighting)
      %spell.setExecConditions($BrightLighting_mask);
  }
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
  SpiritOfRoachSpell.scriptFile = $afxAutoloadScriptFile;
  SpiritOfRoachSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(SpiritOfRoachSpell, SpiritOfRoachSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
