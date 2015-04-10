
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// FLAME BROIL SPELL
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
$spell_reload = isObject(FlameBroilSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = FlameBroilSpell.spellDataPath;
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

datablock afxMooringData(FB_ZodeMooring_CE)
{
  networking = $AFX::CLIENT_ONLY;
  displayAxisMarker = false;
};
datablock afxEffectWrapperData(FB_ZodeMooring_EW)
{
  effect = FB_ZodeMooring_CE;
  posConstraint = caster;
  effectName = "ZodeMooring";
  isConstraintSrc = true;
  lifetime = 5.0;
  xfmModifiers[0] = SHARED_freeze_AltitudeConform_XM;
};

//
// The casting zodiac is created with three primary
// layers and two glows. Using additive blends helps
// to suggest that the patterns are projected light.
// The SHARED_freeze_XM transform modifier (defined in
// another file) is used to lock some of the zodiacs
// to their initial constraint positions.
// 

// A white glowing zodiac that fades in and
// out at start of casting-time
datablock afxZodiacData(FB_ZodeReveal_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/FB/zodiacs/FB_caster_reveal";
  radius = 3.0;
  startAngle = 187.5; 
  rotationRate = -30.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(FB_ZodeReveal_EW)
{
  effect = FB_ZodeReveal_CE;
  posConstraint = "#effect.ZodeMooring";
  borrowAltitudes = true;
  lifetime = 0.75;
  fadeInTime = 0.5;
  fadeOutTime = 0.25;
};

// An orange/red zodiac.
datablock afxZodiacData(FB_Zode1_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/FB/zodiacs/FB_caster";
  radius = 3.0;
  startAngle = 180.0;
  rotationRate = -30.0;
  color = "1.0 1.0 1.0 0.8";
  blend = additive;
};
//
datablock afxEffectWrapperData(FB_Zode1_EW)
{
  effect = FB_Zode1_CE;
  posConstraint = "#effect.ZodeMooring";
  borrowAltitudes = true;
  delay = 0.25;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 3.0;
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
datablock afxZodiacData(FB_Zode1_underglow_CE : FB_Zode1_CE)
{
  texture = %mySpellDataPath @ "/FB/zodiacs/FB_caster-underglow256";
  color = "0.5 0.5 0.5 0.5";
  blend = normal;
};
//
datablock afxEffectWrapperData(FB_Zode1_underglow_EW : FB_Zode1_EW)
{
  effect = FB_Zode1_underglow_CE;
  execConditions = $BrightLighting_mask;
};

// A sparse orange zodiac with inner rune ring
// and outer skulls.
datablock afxZodiacData(FB_Zode2_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/FB/zodiacs/zode_text";
  radius = 3.0;
  startAngle = 180.0;
  rotationRate = 20.0;
  color = "1.0 0.0 0.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(FB_Zode2_EW)
{
  effect = FB_Zode2_CE;
  posConstraint = "#effect.ZodeMooring";
  borrowAltitudes = true;
  delay = 0.25;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 3.0;
};

// Runes & Skulls Zode Underglow
//  Here the zode is made black but only slightly opaque to subtly
//  darken the ground, making the additive glow zode appear more
//  saturated.
datablock afxZodiacData(FB_Zode2_underglow_CE : FB_Zode2_CE)
{
  color = "0 0 0 0.25";
  blend = normal;
};
//
datablock afxEffectWrapperData(FB_Zode2_underglow_EW : FB_Zode2_EW)
{
  effect = FB_Zode2_underglow_CE;
  execConditions = $BrightLighting_mask;
};

// A white zodiac with sketchy runes. 
datablock afxZodiacData(FB_Zode3_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/FB/zodiacs/FB_caster-symbols";
  radius = 3.0;
  startAngle = 180.0;
  rotationRate = -30.0;
  color = "1.0 1.0 1.0 0.5";
  blend = additive;
};
//
datablock afxEffectWrapperData(FB_Zode3_EW)
{
  effect = FB_Zode3_CE;
  posConstraint = "#effect.ZodeMooring";
  borrowAltitudes = true;
  delay = 0.25;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 3.0;
};

// A white glowing zodiac that fades in and out at middle
// of casting-time. It establishes the circle where the 
// ring-of-fire emerges.
datablock afxZodiacData(FB_ZodeGlowRing_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/FB/zodiacs/FB_glowring";
  radius = 3.0;
  startAngle = 165.0;
  rotationRate = -30.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(FB_ZodeGlowRing_EW)
{
  effect = FB_ZodeGlowRing_CE;
  posConstraint = "#effect.ZodeMooring";
  borrowAltitudes = true;
  delay = 0.6;
  lifetime = 1.25;
  fadeInTime = 0.6;
  fadeOutTime = 0.25;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// RING OF FIRE

//
// The ring-of-fire is created by layering ten emitters that 
// orbit the caster at ground level. To fill out the ring, 
// the emitters start out at different angles distributed 
// around the circle, and also move at varying rates in both
// clockwise and counter-clockwise directions.  
//

//
// This particle system is the main component used in the
// ring-of-fire and also by the fireball missile.
//
datablock ParticleData(FB_Flames_P)
{
   textureName          = %mySpellDataPath @ "/FB/particles/FB_fireZodiac";
   dragCoeffiecient     = 0.5;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 900;
   lifetimeVarianceMS   = 200;
   useInvAlpha          = false;
   spinRandomMin        = -720.0;
   spinRandomMax        = 720.0;
   colors[0]            = "1.0 1.0 1.0 1.0";
   colors[1]            = "1.0 1.0 0.0 1.0";
   colors[2]            = "1.0 0.0 0.0 1.0";
   colors[3]            = "1.0 0.0 0.0 0.0";
   sizes[0]             = 0.6;
   sizes[1]             = 1.05;
   sizes[2]             = 0.5;
   sizes[3]             = 0.15;
   times[0]             = 0.0;
   times[1]             = 0.2;
   times[2]             = 0.55;
   times[3]             = 1.0;
};
//
datablock ParticleEmitterData(FB_Flames_E)
{
  ejectionOffset        = 0.02;
  ejectionPeriodMS      = 10;
  periodVarianceMS      = 0;
  ejectionVelocity      = 1.2;
  velocityVariance      = 0.9;
  thetaMin              = 0.0;
  thetaMax              = 0.0;
  particles             = FB_Flames_P;
};

// this offset defines the radius of ring-of-fire
datablock afxXM_LocalOffsetData(FB_FireRing_offset_XM)
{
  localOffset = "0 1.75 0";
};

datablock afxXM_GroundConformData(FB_FireRing_ground_XM)
{
  height = 0.01;
};

// this and the other spin modifiers set the starting
// angle and rotation rate of the ring-of-fire emitters.
datablock afxXM_SpinData(FB_FireRing_spin1_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 0;
  spinRate = 900;
};
//
datablock afxEffectWrapperData(FB_FireRing1_EW)
{
  effect = FB_Flames_E;
  posConstraint = caster;
  delay = 1.35; //1.5-.25
  lifetime = 1.6; //1.25
  xfmModifiers[0] = "FB_FireRing_spin1_XM";
  xfmModifiers[1] = "FB_FireRing_offset_XM";
  xfmModifiers[2] = "FB_FireRing_ground_XM";
};

datablock afxXM_SpinData(FB_FireRing_spin2_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 60;
  spinRate = 500;
};
//
datablock afxEffectWrapperData(FB_FireRing2_EW : FB_FireRing1_EW)
{
  xfmModifiers[0] = "FB_FireRing_spin2_XM";
  xfmModifiers[1] = "FB_FireRing_offset_XM";
  xfmModifiers[2] = "FB_FireRing_ground_XM";
};

datablock afxXM_SpinData(FB_FireRing_spin3_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 120;
  spinRate = -850;
};
//
datablock afxEffectWrapperData(FB_FireRing3_EW : FB_FireRing1_EW)
{
  xfmModifiers[0] = "FB_FireRing_spin3_XM";
  xfmModifiers[1] = "FB_FireRing_offset_XM";
  xfmModifiers[2] = "FB_FireRing_ground_XM";
};

datablock afxXM_SpinData(FB_FireRing_spin4_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 180;
  spinRate = 600;
};
//
datablock afxEffectWrapperData(FB_FireRing4_EW : FB_FireRing1_EW)
{
  xfmModifiers[0] = "FB_FireRing_spin4_XM";
  xfmModifiers[1] = "FB_FireRing_offset_XM";
  xfmModifiers[2] = "FB_FireRing_ground_XM";
};

datablock afxXM_SpinData(FB_FireRing_spin5_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 240;
  spinRate = -560;
};
//
datablock afxEffectWrapperData(FB_FireRing5_EW : FB_FireRing1_EW)
{
  xfmModifiers[0] = "FB_FireRing_spin5_XM";
  xfmModifiers[1] = "FB_FireRing_offset_XM";
  xfmModifiers[2] = "FB_FireRing_ground_XM";
};

datablock afxXM_SpinData(FB_FireRing_spin6_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 300;
  spinRate = -360;
};
//
datablock afxEffectWrapperData(FB_FireRing6_EW : FB_FireRing1_EW)
{
  xfmModifiers[0] = "FB_FireRing_spin6_XM";
  xfmModifiers[1] = "FB_FireRing_offset_XM";
  xfmModifiers[2] = "FB_FireRing_ground_XM";
};

datablock afxXM_SpinData(FB_FireRing_spin7_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 30;
  spinRate = -780;
};
//
datablock afxEffectWrapperData(FB_FireRing7_EW : FB_FireRing1_EW)
{
  xfmModifiers[0] = "FB_FireRing_spin7_XM";
  xfmModifiers[1] = "FB_FireRing_offset_XM";
  xfmModifiers[2] = "FB_FireRing_ground_XM";
};

datablock afxXM_SpinData(FB_FireRing_spin8_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 90;
  spinRate = 450;
};
//
datablock afxEffectWrapperData(FB_FireRing8_EW : FB_FireRing1_EW)
{
  xfmModifiers[0] = "FB_FireRing_spin8_XM";
  xfmModifiers[1] = "FB_FireRing_offset_XM";
  xfmModifiers[2] = "FB_FireRing_ground_XM";
};

datablock afxXM_SpinData(FB_FireRing_spin9_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 150;
  spinRate = -700;
};
//
datablock afxEffectWrapperData(FB_FireRing9_EW : FB_FireRing1_EW)
{
  xfmModifiers[0] = "FB_FireRing_spin9_XM";
  xfmModifiers[1] = "FB_FireRing_offset_XM";
  xfmModifiers[2] = "FB_FireRing_ground_XM";
};

datablock afxXM_SpinData(FB_FireRing_spin10_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 210;
  spinRate = -340;
};
//
datablock afxEffectWrapperData(FB_FireRing10_EW : FB_FireRing1_EW)
{
  xfmModifiers[0] = "FB_FireRing_spin10_XM";
  xfmModifiers[1] = "FB_FireRing_offset_XM";
  xfmModifiers[2] = "FB_FireRing_ground_XM";
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// RING-OF-FIRE LIGHTS

//
// Light from the ring of fire is simultated using two point lights
// positioned below the caster.  Paths are used to simulate a subtle
// flicker.
//

// this offset defines the lights distance beneath the terrain
datablock afxXM_LocalOffsetData(FB_FireRingLight_offset_XM)
{
  localOffset = "0 0 -4";
};

// flicker path 1
datablock afxPathData(FB_FireRingLight_1_Path)
{
  points = " 0    0    0"   SPC
           " 0.1 -0.3  0.8" SPC
           "-0.3  0.2 -0.6" SPC
           " 0.0 -0.6  0.4" SPC
           " -0.7 0.4 -0.8" SPC
           " 0    0    0";
  lifetime = 0.25;
  loop = cycle;
  mult = 0.06;
};
//
datablock afxXM_PathConformData(FB_FireRingLight_1_path_XM)
{
  paths = "FB_FireRingLight_1_Path";
};

// flicker path 2
datablock afxPathData(FB_FireRingLight_2_Path)
{
  points = " 0    0    0"   SPC
           " 0.4  0.7 -0.3" SPC
           "-0.3  0.0  0.4" SPC
           " 0.2  0.4 -0.8" SPC
           "-0.4 -0.8  0.5" SPC
           " 0    0    0";
  lifetime = 0.20;
  loop = cycle;
  mult = 0.05;
};
//
datablock afxXM_PathConformData(FB_FireRingLight_2_path_XM)
{
  paths = "FB_FireRingLight_2_Path";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPELLCASTER ANIMATION
//

// caster performs a summoning animation 
datablock afxAnimClipData(FB_SummonFire_Clip_CE)
{
  clipName = "summon";
  rate = 0.45;
};
datablock afxEffectWrapperData(FB_SummonFire_Clip_EW)
{
  effect = FB_SummonFire_Clip_CE;
  constraint = "caster";
  lifetime = 1.0;
  delay = 0.5;
};

// caster performs a casting animation 
datablock afxAnimClipData(FB_Casting1_Clip_CE)
{
  clipName = "fb";
  rate = 1.8;
};
datablock afxEffectWrapperData(FB_Casting1_Clip_EW)
{
  effect = FB_Casting1_Clip_CE;
  constraint = "caster";
  delay = 1.8;
};

// caster performs a throwing animation 
datablock afxAnimClipData(FB_Casting2_Clip_CE)
{
  clipName = "throw";
  rate = 0.7;
};
datablock afxEffectWrapperData(FB_Casting2_Clip_EW)
{
  effect = FB_Casting2_Clip_CE;
  constraint = "caster";
  delay = 2.8;
  lifetime = 2.0; 
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// FIREBALL

//
// here the fire particles are used to make a fireball
// appear in the caster's right hand.
//
datablock afxEffectWrapperData(FB_FireballEmerge_EW)
{
  effect = FB_Flames_E;
  posConstraint = "caster.Bip01 R Hand";  
  delay = 2.0;
  fadeInTime  = 0.1;
  fadeOutTime = 0.3;
  lifetime = 0.7;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// IMPACT EXPLOSION
//

datablock ParticleData(FB_ExplosionSmoke_P)
{
   textureName          = %mySpellDataPath @ "/FB/particles/smoke";
   dragCoeffiecient     = 100.0;
   gravityCoefficient   = 0;
   inheritedVelFactor   = 0.25;
   constantAcceleration = -0.30;
   lifetimeMS           = 1200;
   lifetimeVarianceMS   = 300;
   useInvAlpha =  true;
   spinRandomMin = -80.0;
   spinRandomMax =  80.0;

   colors[0]     = "1.0 1.0 1.0 1.0";
   colors[1]     = "1.0 0.5 0.0 1.0";
   colors[2]     = "0.4 0.4 0.4 1.0";
   colors[3]     = "0.0 0.0 0.0 0.0";

   sizes[0]      = 4.0;
   sizes[1]      = 6.0;
   sizes[2]      = 8.0;
   sizes[3]      = 10.0;

   times[0]      = 0.0;
   times[1]      = 0.33;
   times[2]      = 0.66;
   times[3]      = 1.0;
};
//
datablock ParticleData(FB_ExplosionFire_P)
{
   textureName          = %mySpellDataPath @ "/FB/particles/fireExplosion";
   dragCoeffiecient     = 100.0;
   gravityCoefficient   = 0;
   inheritedVelFactor   = 0.25;
   constantAcceleration = 0.1;
   lifetimeMS           = 600; //1200;
   lifetimeVarianceMS   = 150; //300;
   useInvAlpha =  false;
   spinRandomMin        = -900.0;
   spinRandomMax        = 900.0;
   colors[0]            = "1.0 1.0 1.0 1.0";
   colors[1]            = "1.0 1.0 0.0 1.0";
   colors[2]            = "1.0 0.0 0.0 1.0";
   colors[3]            = "1.0 0.0 0.0 0.0";
   sizes[0]             = 3.0;
   sizes[1]             = 5.0;
   sizes[2]             = 7.0;//2.2;
   sizes[3]             = 3.0; //1.2;
   times[0]             = 0.0;
   times[1]             = 0.2;
   times[2]             = 0.7;
   times[3]             = 1.0;
};
//
datablock ParticleEmitterData(FB_ExplosionFire_E)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 0.8;
   velocityVariance = 0.5;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   lifetimeMS       = 250;
   particles        = "FB_ExplosionFire_P";
};
//
datablock ParticleEmitterData(FB_ExplosionSmoke_E)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 0;
   ejectionVelocity = 10;
   velocityVariance = 1.5;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   lifetimeMS       = 250;
   particles        = "FB_ExplosionSmoke_P";
};
//
datablock ExplosionData(FB_Explosion_CE)
{
   lifeTimeMS = 1200;

   // Volume particles
   particleEmitter = FB_ExplosionFire_E;
   particleDensity = 20; //50;
   particleRadius = 3;

   // Point emission
   emitter[0] = FB_ExplosionSmoke_E;
   emitter[1] = FB_ExplosionSmoke_E;

   // Impulse
   impulseRadius = 10;
   impulseForce = 15;
};
//
datablock afxEffectWrapperData(FB_Explosion1_EW)
{
  effect = FB_Explosion_CE;
  constraint = "impactPoint";
  execConditions[0] = $AFX::IMPACTED_SOMETHING;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// IMPACT SHOCKWAVE
//

datablock afxZodiacData(FB_ImpactZodeFast_CE)
{  
  texture = %mySpellDataPath @ "/FB/zodiacs/zode_impactA";
  radius = 1.0;
  startAngle = 0.0;
  rotationRate = 0.0;  
  color = "1.0 1.0 1.0 0.10";
  blend = additive;
  growthRate = 110.0;
  showOnInteriors = false;
};

// A fast growing zodiac used as an impact shockwave.
datablock afxEffectWrapperData(FB_ImpactZodeFast_EW)
{
  effect = FB_ImpactZodeFast_CE;
  posConstraint = "impactPoint";
  delay = 0.05;
  fadeInTime = 0.0;
  fadeOutTime = 0.5;
  lifetime = 0.5;
  xfmModifiers[0] = SHARED_freeze_XM;
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// DAMAGE

datablock afxDamageData(FB_dot_damage_CE)
{
  label = "fb_dot";
  flavor = "fire";
  directDamage = 3;
  directDamageRepeats = 6;
};

datablock afxEffectWrapperData(FB_dot_damage_EW)
{
  effect = FB_dot_damage_CE;
  posConstraint = "impactPoint";
  posConstraint2 = "impactedObject"; 
  lifetime = 6;
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/fb_lighting_tge_sub.cs");
    exec("./audio/fb_audio_sub.cs");
  case "TGEA":
    exec("./lighting/fb_lighting_tgea_sub.cs");
    exec("./audio/fb_audio_sub.cs");
  case "T3D":
    exec("./lighting/fb_lighting_t3d_sub.cs");
    exec("./audio/fb_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// FIREBALL MISSILE

datablock afxMagicMissileData(FB_Fireball)
{
  particleEmitter = FB_Flames_E;
  muzzleVelocity = 30;
  velInheritFactor = 0;
  lifetime = 20000;
  isBallistic = true;
  ballisticCoefficient = 0.95;
  gravityMod = 0.05;
  isGuided  = true;
  precision = 50;
  trackDelay  = 7;
  sound = FB_FireBallSnd_CE;

  // NOTE - Replace launchNode with best node given the character's skeleton
  // and casting animation.
  launchNode = "Bip01 R Hand";

  // NOTE - The character is not animating on the server, so a fixed launch point
  // approximating the correct location is set. (launchOffsetServer overrides
  // launchNode on the server.) By setting echoLaunchOffset to true, you can see
  // in the console what offset is used on the client and use that for the
  // launchOffsetServer value.
  launchOffsetServer = "0.49 1.20 1.88";
  echoLaunchOffset = false;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// FLAME BROIL SPELL
//

datablock afxMagicSpellData(FlameBroilSpell)
{
  castingDur = 3.0;
  lingerDur = 6.0;

    // magic missile //
  missile = FB_Fireball;

    // casting zodiac //
  addCastingEffect = FB_ZodeMooring_EW;
  addCastingEffect = FB_ZodeReveal_EW;
  addCastingEffect = FB_Zode1_underglow_EW;
  addCastingEffect = FB_Zode1_EW;
  addCastingEffect = FB_Zode2_underglow_EW;
  addCastingEffect = FB_Zode2_EW;
  addCastingEffect = FB_Zode3_EW;
  addCastingEffect = FB_ZodeGlowRing_EW;
    // ring-of-fire //
  addCastingEffect = FB_FireRing1_EW;
  addCastingEffect = FB_FireRing2_EW;
  addCastingEffect = FB_FireRing3_EW;
  addCastingEffect = FB_FireRing4_EW;
  addCastingEffect = FB_FireRing5_EW;
  addCastingEffect = FB_FireRing6_EW;
  addCastingEffect = FB_FireRing7_EW;
  addCastingEffect = FB_FireRing8_EW;
  addCastingEffect = FB_FireRing9_EW;
  addCastingEffect = FB_FireRing10_EW;
   // spellcaster animation //
  addCastingEffect = FB_SummonFire_Clip_EW;
  addCastingEffect = FB_Casting1_Clip_EW;
  addCastingEffect = FB_Casting2_Clip_EW;
    // fireball //
  addCastingEffect = FB_FireballEmerge_EW;

    // impact effects //
  addImpactEffect = FB_Explosion1_EW;
  addImpactEffect = FB_ImpactZodeFast_EW;
  addImpactEffect = FB_dot_damage_EW;
};

// sounds and lights added via sub-script functions //
FB_add_Lighting_FX(FlameBroilSpell);
FB_add_Audio_FX(FlameBroilSpell);

datablock afxRPGMagicSpellData(FlameBroilSpell_RPG)
{
  spellName = "Flame Broil";
  desc = "Conjure up a broiling ring of fire and fling a fiery charcoal briquette " @
         "to inflict " @ 35 @ " damage plus an additional " @ 18 @ 
         " over " @ 6 @ " seconds." @
         "\n" @
         "\nspell design: Matthew Durante" @
         "\nsound effects: Dave Schroeder" @ 
         "\nspell concept: Jeff Faust";
  sourcePack = "Spell Pack 1";
  iconBitmap = %mySpellDataPath @ "/FB/icons/fb";

  target = "enemy";
  range = 75;
  manaCost = 10;

  directDamage = 35.0;

  castingDur = FlameBroilSpell.castingDur;
};

// set a level of detail
function FlameBroilSpell::onActivate(%this, %spell, %caster, %target)
{
  Parent::onActivate(%this, %spell, %caster, %target);
  if (MissionInfo.hasBrightLighting)
    %spell.setExecConditions($BrightLighting_mask);
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
  FlameBroilSpell.scriptFile = $afxAutoloadScriptFile;
  FlameBroilSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(FlameBroilSpell, FlameBroilSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
