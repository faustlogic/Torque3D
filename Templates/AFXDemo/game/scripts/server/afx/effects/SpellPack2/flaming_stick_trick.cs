
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// FLAMING STICK TRICK SPELL
//
//  A fiery staff ritual of summoning... it's a little bit like Me 'Ol Bam-Boo but here,
//  the staff is on fire.
//
//    Parameters:
//      _skull    (bool)    -- enables/disables bull-skull zodiac elements
//      _beam     (bool)    -- enables/disables symbol beams
//      _sym      (string)  -- substring for choosing symbol assets (1 of "pent", "swirl")
//      _conjure  (bool)    -- enables/disables conjuring of a health potion item
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

exec("./special/fst_cinematic_sub.cs");

//
// Here we test if the script is being reloaded or if this is the
// first time the script has executed this mission.
//
$spell_reload = isObject(FlamingStickTrickSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = FlamingStickTrickSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GLOBALS

$BrightLighting_mask = 0x800000; // BIT(23);

// important timing cues

$FST_Cue_Start = 0.0; 
$FST_Cue_RevealZodiac     = $FST_Cue_Start + 0.4;
$FST_Cue_MainZodiac       = $FST_Cue_Start + 1.5+0.25;
$FST_Cue_GlowFire         = $FST_Cue_Start + 2.5;
$FST_Cue_RevealSymbol     = $FST_Cue_Start + 3.0;
$FST_Cue_Symbol           = $FST_Cue_Start + 3.0+0.25;
$FST_Cue_StaffAppearance  = $FST_Cue_Start + 5.67;
$FST_Cue_StaffPlant       = $FST_Cue_Start + 9.04;
$FST_Cue_CasterAnimEnd    = $FST_Cue_Start + 11.37;
$FST_Cue_StaffVanish      = $FST_Cue_Start + 12.04;
$FST_Cue_End              = $FST_Cue_Start + 13.0;

$FST_CastingOffset_Y = 4.05; // offset of main zodiacs from caster
$FST_CastingScale = 1.3;

$FST_StuckStaff_Life = $FST_Cue_StaffVanish - $FST_Cue_StaffPlant;
$FST_CasterAnim_Life = $FST_Cue_CasterAnimEnd - $FST_Cue_Start;

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTER MOORING

//
// In order to lock all spell effects into their proper positions and
//  orientations relative to the caster, "frozen moorings" are used.
//  The moorings are constrained to the caster and are timed to 
//  appear immediately and last through the duration of the spell.
//  But, so that the moorings do not continue to update as the caster
//  moves and rotates, a freeze modifier is used.  The end result is
//  to capture the caster's initial position and orientation.
//
// Therefore (nearly) all effects that are a part of this spell
//  constrain to these moorings and not the caster.  Without them,
//  the slightest movement or re-orientation of the orc would cause
//  effects to appear misaligned over the casting duration.
//
// Two versions of the moorings are required, one on the client, one
//  on the server.  Most effects constrain to the client version, but
//  the Tower itself is a StaticShape and must constrain to a mooring
//  that exists server-side.
//

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPELLCASTER ANIMATION

//
// < Adjusting the caster's orientation by moving the player-camera or
//    using the left-right arrow keys causes problems with the choreography
//    of the animation, as the staff strike position becomes misaligned.
//    Can this be locked? >
//

datablock afxAnimClipData(FST_Casting_Clip_CE)
{
  clipName = "fst";
  ignoreCorpse = true;
  rate = 1.0;
};
//
datablock afxEffectWrapperData(FST_Casting_Clip_EW)
{
  effect = FST_Casting_Clip_CE;
  constraint = caster;
  delay = $FST_Cue_Start;
  lifetime = $FST_CasterAnim_Life;
};

datablock afxAnimLockData(FST_AnimLock_CE)
{
  priority = 0;
};
//
datablock afxEffectWrapperData(FST_AnimLock_EW)
{
  effect = FST_AnimLock_CE;
  constraint = caster;
  delay = $FST_Cue_Start;
  lifetime = $FST_CasterAnim_Life;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING ZODIACS

//
// The casting zodiac for this spell is very complex, consisting of
//  an embedded zodiac design with many secondary embellishments.
//  Essentially though there are two elements:
//   1. A primary zodiac in the shape of a bull's head with flaming
//        horns
//   2. An embedded pentagram zodiac that spins
//  In this section, the first is defined.
//
// Given how the design evolved, the primary zodiac is actually
//  seperated into two parts: one that contains the circular horns
//  (the "Main" zodiac), and another that is the bull's head.
//  Offsets are used carefully to make both line-up.
//
// An interesting aspect of this zodiac as compared to other AFX
//  casting zodiacs is that it is offset from the caster, rather than
//  centered at his feet.
//

// Main White Reveal Glow
datablock afxZodiacData(FST_CastingZode_Reveal_CE)
{  
  texture = %mySpellDataPath @ "/FST/zodiacs/fst_casting_horns_reveal";
  radius = 4.0*$FST_CastingScale;
  startAngle = 0;
  rotationRate = 0.0;  
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
  showOnInteriors = false;
};
//
datablock afxEffectWrapperData(FST_CastingZode_Reveal_EW)
{
  effect = FST_CastingZode_Reveal_CE;
  constraint = "zodeAnchor";
  delay = $FST_Cue_RevealZodiac;
  lifetime = 2.2;
  fadeInTime = 0.5;
  fadeOutTime = 0.25;
};

// Main Casting Zodiac
datablock afxZodiacData(FST_CastingZode_Main_CE)
{  
  texture = %mySpellDataPath @ "/FST/zodiacs/fst_casting_wreath";
  radius = 4.0*$FST_CastingScale;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 0.9";
  blend = additive;
  showOnInteriors = false;
};
//
datablock afxEffectWrapperData(FST_CastingZode_Main_EW)
{
  effect = FST_CastingZode_Main_CE;
  constraint = "zodeAnchor";
  delay = $FST_Cue_MainZodiac;
  lifetime = $FST_Cue_End - $FST_Cue_MainZodiac;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
};

// Skull Casting Zode
datablock afxZodiacData(FST_CastingZode_Skull_CE)
{  
  texture = %mySpellDataPath @ "/FST/zodiacs/fst_casting_skull";
  radius = 2.75*$FST_CastingScale;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 0.9";
  blend = additive;
  showOnInteriors = false;
};
//
datablock afxEffectWrapperData(FST_CastingZode_Skull_EW)
{
  effectEnabled = "$$ %%._skull";
  effect = FST_CastingZode_Skull_CE;
  constraint = "skullAnchor";
  delay = $FST_Cue_MainZodiac;
  lifetime = $FST_Cue_End - $FST_Cue_MainZodiac;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
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
datablock afxZodiacData(FST_CastingZode_Main_underglow_CE : FST_CastingZode_Main_CE)
{
  color = "0.7 0.7 0.7 0.7";
  blend = normal;
};
//
datablock afxEffectWrapperData(FST_CastingZode_Main_underglow_EW : FST_CastingZode_Main_EW)
{
  effect = FST_CastingZode_Main_underglow_CE;
  delay = $FST_Cue_MainZodiac + 0.25;
  execConditions = $BrightLighting_mask;
};

datablock afxZodiacData(FST_CastingZode_Skull_underglow_CE : FST_CastingZode_Skull_CE)
{
  color = "0.7 0.7 0.7 0.7";
  blend = normal;
};
//
datablock afxEffectWrapperData(FST_CastingZode_Skull_underglow_EW : FST_CastingZode_Skull_EW)
{
  effect = FST_CastingZode_Skull_underglow_CE;
  delay = $FST_Cue_MainZodiac + 0.25;
  execConditions = $BrightLighting_mask;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING ZODIAC GLOWS

//
// The horns of the bull as incorporated into the main casting zodiac
//  have a fiery design, and to accent this, secondary glow zodiacs
//  are used to make it seem as if the fire undulates.  This glowing
//  is carried through in a second set of glows that create a halo
//  around the bull's head and up through the inside of the main
//  zodiac.
//
// Each pulsation is it's own zodiac, fading in-and-out, and so
//  multiple zodiacs are required.  Variables are used to time the
//  individual glows, in an attempt to create an interesting psuedo-
//  random pattern.
//

datablock afxZodiacData(FST_CastingZode_GlowFire_CE : FST_CastingZode_Reveal_CE)
{  
  texture = %mySpellDataPath @ "/FST/zodiacs/fst_casting_wreath_reveal";
};
datablock afxEffectWrapperData(FST_CastingZode_GlowFire_VIS_EW)
{
  effect = FST_CastingZode_GlowFire_CE;
  constraint = "zodeAnchor";
  delay = $FST_Cue_GlowFire;
  lifetime = 8.25;
  visibilityKeys  = "0.0:0.0" SPC
                        "0.50:1.0 0.75:1.0" SPC
                    "1.0:0.0 1.20:0.0" SPC
                        "1.45:1.0 1.70:1.0" SPC 
                    "1.83:0.5"  SPC
                        "2.2:1.0 2.95:1.0" SPC 
                    "3.1:0.6" SPC
                        "3.25:1.0 3.5:1.0" SPC 
                    "3.53:0.7" SPC
                        "3.8:1.0 4.05:1.0" SPC 
                    "4.25:0.2" SPC
                        "4.7:1.0 5.45:1.0" SPC
                    "5.55:0.5" SPC
                        "5.65:1.0 5.90:1.0" SPC 
                    "5.98:0.6" SPC
                        "6.3:1.0" SPC 
                    "6.53:0.1" SPC
                        "7.0:1.0" SPC 
                    "7.13:0.5" SPC
                        "7.25:1.0 8.25:1.0" SPC 
                    "8.5:0.0";
};

datablock afxZodiacData(FST_CastingZode_GlowSkull_CE : FST_CastingZode_Reveal_CE)
{  
  texture = %mySpellDataPath @ "/FST/zodiacs/fst_horned_bull_reveal";
  radius = 5.0*$FST_CastingScale;
  color = "1 1 1 1.0";
};
datablock afxEffectWrapperData(FST_CastingZode_GlowSkull_VIS_EW : FST_CastingZode_GlowFire_VIS_EW)
{
  effectEnabled = "$$ %%._skull";
  effect = FST_CastingZode_GlowSkull_CE;
  constraint = "skullGlowAnchor";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING ZODIAC PENTAGRAMS

//
// Within the circular area bordered by the horns, a spinning
//  pentagram appears.  This is the point, the focus of the magic.
//  The caster stands above it as he summons the fire-staff, then
//  with a pounce strikes that staff into the pentagram's center and
//  its spinning stops.  But as the world begins to rumble the
//  pentagram starts spinning again, now faster and faster...  From
//  it the Fire Tower rises!
//
// Two main zodiacs are used to produce this effect, along with glow
//  geometries that are defined later (see "XXX").  The first spins
//  at a constant rate and disappears when the staff strikes.  The
//  second starts off still, but by using a Spin modifier with
//  animated timing parameters, it starts to spin faster and faster
//  just before the tower emergence effects begin.
//
// Arcane-FX was long overdue its first pentagram...
//

$FST_Pentagram_startAngle = -10.0; // needed to get the pentagram centered when staff strikes...

// pentagram white reveal glow
datablock afxZodiacData(FST_CastingZode_Pentagram_Reveal_CE)
{  
  texture = "$$ \"" @ %mySpellDataPath @ "/FST/zodiacs/fst_\" @ " @ "%%._sym" @ " @ \"_symbol_reveal\"";
  radius = 1.8*$FST_CastingScale;
  startAngle = 15.0+$FST_Pentagram_startAngle; // -rotationRate * mainZodeDelay
  rotationRate = -60.0;  
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
  showOnInteriors = false;
};
//
datablock afxEffectWrapperData(FST_CastingZode_Pentagram_Reveal_EW)
{
  effect = FST_CastingZode_Pentagram_Reveal_CE;
  posConstraint = "strikeLoc";
  delay = $FST_Cue_RevealSymbol;
  lifetime = 1.55;
  fadeInTime = 0.5;
  fadeOutTime = 0.25;
};

// pentagram casting zodiac #1, constant spinning
datablock afxZodiacData(FST_CastingZode_Pentagram_CE)
{  
  texture = "$$ \"" @ %mySpellDataPath @ "/FST/zodiacs/fst_\" @ " @ "%%._sym" @ " @ \"_symbol\"";
  radius = 1.8*$FST_CastingScale;
  startAngle = $FST_Pentagram_startAngle;
  rotationRate = -60.0;
  color = "1.0 1.0 1.0 0.9";
  blend = additive;
  showOnInteriors = false;
};
//
datablock afxEffectWrapperData(FST_CastingZode_Pentagram_EW)
{
  effect = FST_CastingZode_Pentagram_CE;
  posConstraint = "strikeLoc";
  delay = $FST_Cue_Symbol;
  lifetime = $FST_Cue_StaffPlant - $FST_Cue_Symbol; 
  fadeInTime = 0.75;
  fadeOutTime = 0;
};

// pentagram under zodiac #1
datablock afxZodiacData(FST_CastingZode_Pentagram_Under_CE)
{  
  texture = "$$ \"" @ %mySpellDataPath @ "/FST/zodiacs/fst_\" @ " @ "%%._sym" @ " @ \"_symbol_under\"";
  radius = 2.0*$FST_CastingScale;
  startAngle = $FST_Pentagram_startAngle;
  rotationRate = -60.0;
  color = "0.0 0.0 0.0 0.4";
  blend = normal;
  showOnInteriors = false;
};
//
datablock afxEffectWrapperData(FST_CastingZode_Pentagram_Under_EW : FST_CastingZode_Pentagram_EW)
{
  effect = FST_CastingZode_Pentagram_Under_CE;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxModelData(FST_CastingBeamA_CE)
{
  shapeFile = %mySpellDataPath @ "/FST/models/fst_casting_beam.dts";
  sequence = "beam";
  sequenceRate = 1.0/1.2;
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  useVertexAlpha = true;
};
//
datablock afxEffectWrapperData(FST_CastingBeamA_EW)
{
  effect = FST_CastingBeamA_CE;
  constraint = "zodeAnchor";
  scaleFactor = 1.52*$FST_CastingScale;
  delay = $FST_Cue_Start + 0.5;
  lifetime = 1.9;
  fadeinTime = 0.2;
  fadeOutTime = 0.5;
};


datablock afxXM_SpinData(FST_PentagramBeamA_spin_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 90-3 +  $FST_Pentagram_startAngle;
  spinRate = -60;
  lifetime = $FST_Cue_StaffPlant - $FST_Cue_RevealSymbol;
  fadeOutTime = 0;
};

datablock afxModelData(FST_PentagramBeamA_CE)
{
  shapeFile = %mySpellDataPath @ "/FST/models/fst_pent_beam.dts";
  sequence = "beam";
  sequenceRate = 1.0;
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  useVertexAlpha = true;
};
//
datablock afxEffectWrapperData(FST_PentagramBeamA_EW)
{
  effectEnabled = "$$ %%._beam";
  effect = FST_PentagramBeamA_CE;
  posConstraint = "strikeLoc";
  scaleFactor = 2.6*$FST_CastingScale;
  delay = $FST_Cue_RevealSymbol;
  lifetime = 9.2;
  fadeinTime = 0.2;
  fadeOutTime = 0.8;
  xfmModifiers[0] = FST_PentagramBeamA_spin_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// STAFF

datablock afxModelData(FST_Staff_CE)
{
  shapeFile = %mySpellDataPath @ "/FST/models/fst_staff.dts";
};
//
datablock afxEffectWrapperData(FST_Staff_EW)
{
  effect = FST_Staff_CE;
  constraint = "caster.Mount0";
  effectName = "FireStaff";
  isConstraintSrc = true;
  delay = $FST_Cue_StaffAppearance;
  lifetime = $FST_Cue_StaffPlant - $FST_Cue_StaffAppearance;
  fadeinTime = 0.7;
  fadeOutTime = 0.0;
};

datablock afxEffectWrapperData(FST_StaffStick_EW)
{
  effect = FST_Staff_CE;
  constraint = "stuckStaffLoc";
  effectName = "FireStaffStick";
  isConstraintSrc = true;
  delay = $FST_Cue_StaffPlant;
  lifetime = $FST_StuckStaff_Life - 0.5;
  fadeinTime = 0.0;
  fadeOutTime = 0.5;
};

datablock ParticleData(FST_StaffFire_Big_P)
{
  textureName          = %mySpellDataPath @ "/Shared/particles/sp2_flame_C128";
  dragCoeffiecient     = 0.5;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 600;
  lifetimeVarianceMS   = 100;
  spinRandomMin        = -500;
  spinRandomMax        = 500;
  colors[0]            = "1.0 1.0 1.0 0.17";
    colors[1]            = "1.0 0.8 0.5 0.17";
    colors[2]            = "1.0 0.3 0.1 0.17";
    colors[3]            = "0.0 0.0 0.0 0.0";
  sizeBias             = 0.8;
  sizes[0]             = 1.0;
    sizes[1]             = 3.5; 
    sizes[2]             = 2.0;
    sizes[3]             = 1.0;
  times[0]             = 0.0;
    times[1]             = 0.2;
    times[2]             = 0.4;
    times[3]             = 1.0;
};
datablock ParticleData(FST_StaffFire_P : FST_StaffFire_Big_P)
{
  sizeBias = 0.4;
};

datablock ParticleEmitterData(FST_StaffFire_E)
{
  ejectionPeriodMS      = 15;
  periodVarianceMS      = 5;
  ejectionVelocity      = 0;
  velocityVariance      = 0;
  thetaMin              = 0.0;
  thetaMax              = 0.0;
  particles             = FST_StaffFire_P;
  fadeSize              = true;
  blendStyle            = "PREMULTALPHA";
};
datablock ParticleEmitterData(FST_StaffFireBig_E : FST_StaffFire_E)
{
  ejectionPeriodMS      = 30;
  periodVarianceMS      = 10;
  particles             = FST_StaffFire_Big_P;
  blendStyle            = "PREMULTALPHA";
};

datablock afxXM_LocalOffsetData(FST_StaffFire_Top_offset_XM)
{
  localOffset = "0 0" SPC (-1.159*2.5);
  delay = 0;
  lifetime = 0;
  fadeInTime = 0;
  fadeOutTime = 0.4;
};
datablock afxXM_LocalOffsetData(FST_StaffFire_Bot_offset_XM : FST_StaffFire_Top_offset_XM)
{
  localOffset = "0 0" SPC (0.507*2.5);
};

datablock afxEffectWrapperData(FST_StaffFire_Top_EW)
{
  effect = FST_StaffFire_E;
  constraint = "#effect.FireStaff.MountTop";
  delay = $FST_Cue_StaffAppearance;
  lifetime = 4.77;
  xfmModifiers[0] = FST_StaffFire_Top_offset_XM;
};
datablock afxEffectWrapperData(FST_StaffFire_Bot_EW : FST_StaffFire_Top_EW)
{
  constraint = "#effect.FireStaff.MountBottom";
  xfmModifiers[0] = FST_StaffFire_Bot_offset_XM;
};

datablock afxEffectWrapperData(FST_StaffFireBig_Top_EW : FST_StaffFire_Top_EW)
{
  effect = FST_StaffFireBig_E;
  lifetime = 0.4;
};
datablock afxEffectWrapperData(FST_StaffFireBig_Bot_EW : FST_StaffFire_Bot_EW)
{
  effect = FST_StaffFireBig_E;
  lifetime = 0.4;
};


//*********************************************************8
datablock afxPathData(FST_StaffStickFire_Path)
{
  points = "0 0 0" SPC "0 0 -3.8";
  delay = $FST_StuckStaff_Life-0.5;
  lifetime = 0.5;
};
datablock afxXM_PathConformData(FST_StaffStickFire_path_XM)
{
  paths = "FST_StaffStickFire_Path";
};
datablock afxEffectWrapperData(FST_StaffStickFire_Top_EW)
{
  effect = FST_StaffFireBig_E;
  constraint = "#effect.FireStaffStick.MountTop";
  delay = $FST_Cue_StaffPlant;
  lifetime = $FST_StuckStaff_Life;
  fadeinTime = 0.0;
  fadeOutTime = 0.0;
  xfmModifiers[0] = FST_StaffStickFire_path_XM;
};

datablock afxModelData(FST_StaffStikeGlow_CE)
{
  shapeFile = %mySpellDataPath @ "/FST/models/fst_strike_glow.dts";
  sequence = "flash";
  sequenceRate = 1.1;
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  useVertexAlpha = true;
};
//
datablock afxEffectWrapperData(FST_StaffStrikeGlow_EW)
{
  effect = FST_StaffStikeGlow_CE;
  posConstraint = "strikeLoc";
  delay = $FST_Cue_StaffPlant - (0.067);
  lifetime = 0.515;
  fadeinTime = 0.0;
  fadeOutTime = 0.667;
  scaleFactor = 8.0;
};

// 
datablock afxZodiacData(FST_StaffStrikeZodiac_CE)
{  
  texture = %mySpellDataPath @ "/FST/zodiacs/fst_staff_strike";
  radius = 1.0*$FST_CastingScale;
  startAngle = ($FST_Cue_StaffPlant-3.25) * -60.0;
  rotationRate = 0.0;  
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
  growthRate = 35;
  showOnInteriors = false;
};
//
datablock afxEffectWrapperData(FST_StaffStrikeZodiacA_EW)
{
  effect = FST_StaffStrikeZodiac_CE;
  posConstraint = "strikeLoc";
  delay = $FST_Cue_StaffPlant;
  lifetime = 0.267;
  fadeInTime = 0;
  fadeOutTime = 0.333;
};
datablock afxEffectWrapperData(FST_StaffStrikeZodiacB_EW)
{
  effect = FST_StaffStrikeZodiac_CE;
  posConstraint = "strikeLoc";
  delay = $FST_Cue_StaffPlant+(18/30);
  lifetime = ((50-18)/30)-(15/30);
  fadeInTime = 0;
  fadeOutTime = (15/30);
};

datablock afxEffectWrapperData(FST_StaffStrikeZodiacC_EW)
{
  effect = FST_StaffStrikeZodiac_CE;
  posConstraint = "strikeLoc";
  delay = $FST_Cue_StaffVanish-0.1;
  lifetime = 0.4;
  fadeInTime = 0;
  fadeOutTime = 0.4;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock ParticleData(FST_SparkA_P)
{
  textureName          = %mySpellDataPath @ "/Shared/particles/sp2_spark";
  dragCoeffiecient     = 0.5;
  gravityCoefficient   = 1.0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 700;
  lifetimeVarianceMS   = 50;
  useInvAlpha          = false;
  colors[0]            = "1.0 1.0 1.0 1.0";
    colors[1]            = "1.0 0.9 0.5 1.0";
    colors[2]            = "1.0 0.0 0.0 0.0";
  sizeBias             = 0.8;
  sizes[0]             = 0.3;
    sizes[1]             = 0.1;
    sizes[2]             = 0.0;
  times[0]             = 0.0;
    times[1]             = 0.5;
    times[2]             = 1.0;
};
datablock ParticleData(FST_SparkB_P : FST_SparkA_P)
{
  sizeBias             = 1.3;
};
datablock ParticleEmitterData(FST_StaffSparks_E)
{
  ejectionPeriodMS      = 9;
  periodVarianceMS      = 3;
  ejectionVelocity      = 7.0;
  velocityVariance      = 3.5;
  thetaMin              = 0.0;
  thetaMax              = 180.0;
  particles             = "FST_SparkA_P FST_SparkB_P";
};
datablock afxPathData(FST_StaffStickSpark_Path)
{
  points = "0 0 0" SPC "0 0 -3.8";
};
datablock afxXM_PathConformData(FST_StaffStickSpark_path_XM)
{
  paths = "FST_StaffStickSpark_Path";
};
datablock afxEffectWrapperData(FST_StaffSparks_EW)
{
  effect = FST_StaffSparks_E;
  constraint = "#effect.FireStaffStick.MountTop";
  delay = $FST_Cue_StaffVanish - 0.5;
  lifetime = 0.5;
  fadeinTime = 0.0;
  fadeOutTime = 0.0;
  xfmModifiers[0] = FST_StaffStickSpark_path_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SOUNDS

/////

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

// This script-event calls the script that will save the caster's transform
datablock afxScriptEventData(FST_GrabCasterXFM_CE)
{
  methodName = "GrabCasterXFM";
};
//
datablock afxEffectWrapperData(FST_GrabCasterXFM_EW)
{
  effect = FST_GrabCasterXFM_CE;
  constraint = "caster";
  delay = $FST_Cue_Start;
};

// This script-event calls the script that resets the caster's transform
// to the value saved by the GrabCasterXFM script.
datablock afxScriptEventData(FST_SnapCasterXFM_CE)
{
  methodName = "SnapCasterXFM";
};
//
datablock afxEffectWrapperData(FST_SnapCasterXFM_EW)
{
  effect = FST_SnapCasterXFM_CE;
  constraint = "caster";
  delay = 7.1;
};
//
datablock afxEffectWrapperData(FST_SnapCasterXFM2_EW)
{
  effect = FST_SnapCasterXFM_CE;
  constraint = "caster";
  delay = 9.0;
};

datablock afxScriptEventData(FST_onConjure_CE)
{
  methodName = "onConjure";
};
//
datablock afxEffectWrapperData(FST_onConjure_EW)
{
  effect = FST_onConjure_CE;
  constraint = "strikeLoc";
  delay = $FST_Cue_StaffVanish;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/fst_lighting_tge_sub.cs");
    exec("./audio/fst_audio_sub.cs");
  case "TGEA":
    exec("./lighting/fst_lighting_tgea_sub.cs");
    exec("./audio/fst_audio_sub.cs");
 case "T3D":
    exec("./lighting/fst_lighting_t3d_sub.cs");
    exec("./audio/fst_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// FLAMING STICK TRICK SPELL
//

datablock afxMagicSpellData(FlamingStickTrickSpell)
{
  echoPacketUsage = 20;
  castingDur = "$$ %%._castDur";
  staffPlantTime = $FST_Cue_StaffPlant; // Fire in the Sky uses this value

  // SUMMONING ////////

    // spellcaster animation //
  addCastingEffect = FST_Casting_Clip_EW;
  addCastingEffect = FST_AnimLock_EW;
    // caster orientation //
  addCastingEffect = FST_GrabCasterXFM_EW;
  addCastingEffect = FST_SnapCasterXFM_EW;
  addCastingEffect = FST_SnapCasterXFM2_EW;
  addCastingEffect = FST_onConjure_EW;
    // casting zodiacs //
  addCastingEffect = FST_CastingZode_Reveal_EW;
  addCastingEffect = FST_CastingZode_Main_EW;
  addCastingEffect = FST_CastingZode_Skull_EW;            // %%._skull enables
  addCastingEffect = FST_CastingZode_Main_underglow_EW;
  addCastingEffect = FST_CastingZode_Skull_underglow_EW;  // %%._skull enables
    // casting zodiac glows //
  addCastingEffect = FST_CastingZode_GlowFire_VIS_EW;
  addCastingEffect = FST_CastingZode_GlowSkull_VIS_EW;
    // casting zodiac pentagrams (spinning) //
  addCastingEffect = FST_CastingZode_Pentagram_Reveal_EW;
  addCastingEffect = FST_CastingZode_Pentagram_Under_EW;
  addCastingEffect = FST_CastingZode_Pentagram_EW;    
    // casting beams //
  addCastingEffect = FST_CastingBeamA_EW;
  addCastingEffect = FST_PentagramBeamA_EW;

  // STAFF-PRE-STRIKE ////////

    // staff //
  addCastingEffect = FST_Staff_EW;
    // staff flames //
  addCastingEffect = FST_StaffFireBig_Top_EW;
  addCastingEffect = FST_StaffFireBig_Bot_EW;
  addCastingEffect = FST_StaffFire_Top_EW;
  addCastingEffect = FST_StaffFire_Bot_EW;
  addCastingEffect = FST_StaffStrikeGlow_EW;
  addCastingEffect = FST_StaffStrikeZodiacA_EW;
  addCastingEffect = FST_StaffStrikeZodiacB_EW;
  addCastingEffect = FST_StaffStrikeZodiacC_EW;

  // STAFF-STRIKE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    // stuck staff //
  addCastingEffect = FST_StaffStick_EW;
  addCastingEffect = FST_StaffStickFire_Top_EW;
  addCastingEffect = FST_StaffSparks_EW;
}; // END OF SPELL

// sounds and lights added via sub-script functions //
FST_add_Lighting_FX(FlamingStickTrickSpell);
FST_add_Audio_FX(FlamingStickTrickSpell);

datablock afxRPGMagicSpellData(FlamingStickTrickSpell_RPG)
{
  spellName = "Flaming Stick Trick";
  desc = "A fiery staff ritual of summoning... " @
         "it's a little bit like <font:Arial Italic:16>Me 'Ol Bam-Boo<font:Arial:16> but here, " @
         "the staff is on fire.\n" @
         "\n" @
         "<font:Arial Italic:14>adaptation from Fire in the Sky: <font:Arial:14>Jeff Faust\n" @
         "<font:Arial Italic:14>sound design: <font:Arial:14>Matt Pacyga";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/FST/icons/fst";
  manaCost = 10;
  castingDur = 0.5;

  _castDur = 0.5;
  _conjure = 1;
  _skull = 0;
  _beam = 0;
  _sym = "swirl";
  _cine = 1;
};

datablock afxRPGMagicSpellData(FlamingStickTrickSpell2_RPG : FlamingStickTrickSpell_RPG)
{
  spellName = "Flaming Stick Trick (FT)";
  castingDur = 0;

  _castDur = 0;
  _conjure = 0; 
  _skull = 1;
  _beam = 1;
  _sym = "pent";
  _cine = 0;
};

// set a level of detail
function FlamingStickTrickSpell::onActivate(%this, %spell, %caster, %target)
{
  Parent::onActivate(%this, %spell, %caster, %target);
  if (MissionInfo.hasBrightLighting)
    %spell.setExecConditions($BrightLighting_mask);

  %caster_xfm = %caster.getTransform();

  %strike_loc = moveTransformAbs(%caster_xfm, %this.calcStrikeLoc(%caster)); 

  // Setup some fixed constraints relative to the caster.
  %spell.addConstraint(%strike_loc, "strikeLoc");
  %spell.addConstraint(%this.calcStuckStaffLoc(%caster), "stuckStaffLoc");
  %spell.addConstraint(%this.calcZodeAnchor(%caster), "zodeAnchor");
  %spell.addConstraint(%this.calcSkullAnchor(%caster), "skullAnchor");
  %spell.addConstraint(%this.calcSkullGlowAnchor(%caster), "skullGlowAnchor");

  if (%spell._cine && isObject(%caster.client))
  {
    %effe = new afxEffectron() 
    {
      datablock = FST_CamShot_Effe;
      _camAnchor = %strike_loc;
      _camCoiOffset = VectorSub(VectorAdd(%caster.getPosition(), "0 0 2"), MatrixCreate(%strike_loc,"0 0 1 0"));
    };
    %effe.addConstraint(%strike_loc, "camAnchor");
    %effe.addExplicitClient(%caster.client);
  }
}

function FlamingStickTrickSpell::calcOffsetAnchor(%this, %caster, %loc_offset)
{
  %caster_xfm = %caster.getTransform();
  %world_offset = MatrixMulVector(%caster_xfm, %loc_offset);
  %world_pos = VectorAdd(%caster.getPosition(), %world_offset); 
  return %world_pos SPC getWords(%caster_xfm, 3, 6);
}

function FlamingStickTrickSpell::calcZodeAnchor(%this, %caster)
{
  return %this.calcOffsetAnchor(%caster, "0" SPC $FST_CastingOffset_Y SPC "0");
}

function FlamingStickTrickSpell::calcSkullAnchor(%this, %caster)
{
  %skull_offset = "0" SPC ($FST_CastingOffset_Y-(5.35*$FST_CastingScale)) SPC "0"; 
  return %this.calcOffsetAnchor(%caster, %skull_offset);
}

function FlamingStickTrickSpell::calcSkullGlowAnchor(%this, %caster)
{
  %glow_offset = "0" SPC ($FST_CastingOffset_Y-(2.2*$FST_CastingScale)) SPC "0"; 
  return %this.calcOffsetAnchor(%caster, %glow_offset);
}

function FlamingStickTrickSpell::calcStrikeLoc(%this, %caster)
{
  %strike_offset = "0" SPC ($FST_CastingOffset_Y-(0.65*$FST_CastingScale)) SPC "0"; 
  return %this.calcOffsetAnchor(%caster, %strike_offset);
}

function FlamingStickTrickSpell::calcStuckStaffLoc(%this, %caster)
{
  return %this.calcOffsetAnchor(%caster, "-0.222 3.177 1.350");
}


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SCRIPT EVENT METHODS

// For this spell the caster requires a specific orientation otherwise he won't plant the
// staff in the right location. Instead of totally locking up the caster's orientation we
// grab the transform while it's good and then snap the caster back to it for the staff
// planting action.
//
// A possible better solution would be to lock the caster's orientation separate from the 
// camera's. The while the caster is locked, the camera could still look around.
//
function FlamingStickTrickSpell::GrabCasterXFM(%this, %spell, %caster, %consObj, %xfm, %data)
{
  if (isObject(%caster))
    %caster.ft_save_xfm = %xfm;
}
function FlamingStickTrickSpell::SnapCasterXFM(%this, %spell, %caster, %consObj, %xfm, %data)
{
  if (isObject(%caster) && %caster.ft_save_xfm !$= "")
    %caster.setTransform(%caster.ft_save_xfm);
}

// This method is called at a the time the final bit of sparkle-magic zips down the staff
// into the ground. It's a good time to make a summoned object appear.
//
function FlamingStickTrickSpell::onConjure(%this, %spell, %caster, %consObj, %xfm, %data)
{
  if (%spell._conjure)
  {
     %item = ItemData::create(HealthKitPotion);
     %item.sourceObject = %caster;
     %item.static = false;
     MissionCleanup.add(%item);

     %item.setTransform(%xfm);
     %item.setCollisionTimeout(%caster);
     %item.schedulePop();
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
  FlamingStickTrickSpell.scriptFile = $afxAutoloadScriptFile;
  FlamingStickTrickSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
  {
    addDemoSpellbookSpell(FlamingStickTrickSpell, FlamingStickTrickSpell_RPG);
    // for testing, uncomment below to add the variation used by Fire in the Sky  
    //addDemoSpellbookSpell(FlamingStickTrickSpell, FlamingStickTrickSpell2_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
