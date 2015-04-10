
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// REAPER MADNESS SPELL
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
$spell_reload = isObject(ReaperMadnessSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = ReaperMadnessSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

$BrightLighting_mask = 0x800000; // BIT(23); 

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPELLCASTER ANIMATION

//
// This clip selects the main animation sequence for casting this
// spell. The spellcaster bows low to the ground in order to plead
// with the gods of the underworld to restore the living spirit to
// the targeted corpse.
//

datablock afxAnimClipData(RM_Prostrate_Clip_CE)
{
  clipName = "rm";
  rate = 1.0;
};

datablock afxEffectWrapperData(RM_Prostrate_Clip_EW)
{
  effect = RM_Prostrate_Clip_CE;
  constraint = caster;
  lifetime = 4.0;
  delay = 1.0;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING ZODIAC

// this mooring anchors the casting zodiacs and measures
// their altitudes

datablock afxMooringData(RM_CastingZodeMooring_CE)
{
  networking = $AFX::CLIENT_ONLY;
  displayAxisMarker = false;
};
datablock afxEffectWrapperData(RM_CastingZodeMooring_EW)
{
  effect = RM_CastingZodeMooring_CE;
  posConstraint = caster;
  effectName = "CastingZodeMooring";
  isConstraintSrc = true;
  lifetime = 5.0;
  xfmModifiers[0] = SHARED_freeze_AltitudeConform_XM;
};

//
// The main casting zodiac is formed by two zodiacs plus a white
// reveal glow when the casting first starts. 
//

// this is the white reveal glow
datablock afxZodiacData(RM_ZodeReveal_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/RM/zodiacs/RM_caster_reveal";
  radius = 3.0;
  startAngle = 7.5; //0.0+7.5
  rotationRate = -30.0;  
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(RM_ZodeReveal_EW)
{
  effect = RM_ZodeReveal_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = 0.0; //0.01;
  lifetime = 0.75;
  fadeInTime = 0.5;
  fadeOutTime = 0.25;
};

// this is the main casting zodiac. yellow, pink, and orange,
// it features vague images of a shadowy horned figure.
datablock afxZodiacData(RM_Zode1_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/RM/zodiacs/RM_caster";
  radius = 3.0;
  startAngle = 0.0;
  rotationRate = -30.0;
  color = "1.0 1.0 1.0 0.9";
  blend = additive;
};
//
datablock afxEffectWrapperData(RM_Zode1_EW)
{
  effect = RM_Zode1_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = 0.25;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 3.75;
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
datablock afxZodiacData(RM_Zode1_underglow_CE : RM_Zode1_CE)
{
  texture = %mySpellDataPath @ "/RM/zodiacs/RM_caster-underglow256";
  color = "0.7 0.7 0.7 0.7";
  blend = normal;
};
//
datablock afxEffectWrapperData(RM_Zode1_underglow_EW : RM_Zode1_EW)
{
  effect = RM_Zode1_underglow_CE;
  execConditions = $BrightLighting_mask;
};

// blue runes and skulls, this zodiac is a common
// element in the casting zodiac of several spells.
datablock afxZodiacData(RM_Zode2_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/RM/zodiacs/zode_text";
  radius = 3.0;
  startAngle = 0.0;
  rotationRate = 20.0; //60
  color = "0.0 0.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(RM_Zode2_EW)
{
  effect = RM_Zode2_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = 0.25;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 3.75;
};

// Runes & Skulls Zode Underglow
//  Here the zode is made black but only slightly opaque to subtly
//  darken the ground, making the additive glow zode appear more
//  saturated.
datablock afxZodiacData(RM_Zode2_underglow_CE : RM_Zode2_CE)
{
  color = "0 0 0 0.4";
  blend = normal;
};
//
datablock afxEffectWrapperData(RM_Zode2_underglow_EW : RM_Zode2_EW)
{
  effect = RM_Zode2_underglow_CE;
  execConditions = $BrightLighting_mask;
};




//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// TRIANGLE FIRES

// the corners of a triangle outside main zodiac
datablock afxZodiacData(RM_TriangleZode_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/RM/zodiacs/RM_caster_triangle";
  radius = 3.0;
  startAngle = 169.0; //161.0; //180-19
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(RM_TriangleZode_EW)
{
  effect = RM_TriangleZode_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = 1.0;
  fadeInTime = 1.0;
  fadeOutTime = 1.0;
  lifetime = 2.5;
};

// the fire particles
datablock ParticleData(RM_Fire_P)
{
   // TGE textureName          = %mySpellDataPath @ "/RM/particles/starfire";
   dragCoeffiecient     = 0.5;
   //gravityCoefficient   = 0.2;
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
   sizes[0]             = 0.2;
   sizes[1]             = 0.6;
   sizes[2]             = 0.2;
   sizes[3]             = 0.1;
   times[0]             = 0.0;
   times[1]             = 0.2;
   times[2]             = 0.55;
   times[3]             = 1.0;

   textureName          = %mySpellDataPath @ "/RM/particles/rm_tiled_parts"; // starfire
   textureCoords[0]     = "0.5 0.0";
   textureCoords[1]     = "0.5 0.5";
   textureCoords[2]     = "1.0 0.5";
   textureCoords[3]     = "1.0 0.0";
};
datablock ParticleEmitterData(RM_Fire_E)
{
  ejectionOffset        = 0.02;
  ejectionPeriodMS      = 10;
  periodVarianceMS      = 0;
  ejectionVelocity      = 0.6; //0.8;
  velocityVariance      = 0.15; //0.00;
  thetaMin              = 0.0;
  thetaMax              = 0.0;
  particles             = RM_Fire_P;
};

// three small fires at triangle corners

// flicker path 1
datablock afxPathData(RM_FireFlicker1_Path)
{
  points = " 0    0    0"   SPC
           " 0.1 -0.3  0.8" SPC
           "-0.3  0.2 -0.6" SPC
           " 0.0 -0.6  0.4" SPC
           " -0.7 0.4 -0.8" SPC
           " 0    0    0";
  lifetime = 0.35;
  loop = cycle;
  mult = 0.16*0.75;
};
//
datablock afxXM_PathConformData(RM_FireFlicker1_path_XM)
{
  paths = "RM_FireFlicker1_Path";
};

// flicker path 2
datablock afxPathData(RM_FireFlicker2_Path)
{
  points = " 0    0    0"   SPC
           " 0.4  0.7 -0.3" SPC
           "-0.3  0.0  0.4" SPC
           " 0.2  0.4 -0.8" SPC
           "-0.4 -0.8  0.5" SPC
           " 0    0    0";
  lifetime = 0.50;
  loop = cycle;
  mult = 0.15*0.75;
};
//
datablock afxXM_PathConformData(RM_FireFlicker2_path_XM)
{
  paths = "RM_FireFlicker2_Path";
};

// flicker path 3
datablock afxPathData(RM_FireFlicker3_Path)
{
  points = " 0    0    0"   SPC
           " 0.2 -0.4 -0.3" SPC
           "-0.5  0.7  0.1" SPC
           " 0.5 -0.3 -0.6" SPC
           "-0.7  0.4  0.4" SPC
           " 0    0    0";
  lifetime = 0.40;
  loop = cycle;
  mult = 0.15*0.75;
};
//
datablock afxXM_PathConformData(RM_FireFlicker3_path_XM)
{
  paths = "RM_FireFlicker3_Path";
};

datablock afxXM_WorldOffsetData(RM_Fire1_offset_XM)
{
  worldOffset = "-2.75 0.0 0.25";
};
datablock afxEffectWrapperData(RM_Fire1_EW)
{
  effect = RM_Fire_E;
  posConstraint = caster;
  delay = 1.8;
  lifetime = 1.7;
  xfmModifiers[0] = RM_Fire1_offset_XM;
  xfmModifiers[1] = RM_FireFlicker1_path_XM;
};

datablock afxXM_WorldOffsetData(RM_Fire2_offset_XM)
{
  worldOffset = "1.5 2.3 0.25";
};
datablock afxEffectWrapperData(RM_Fire2_EW : RM_Fire1_EW)
{
  xfmModifiers[0] = RM_Fire2_offset_XM;
  xfmModifiers[1] = RM_FireFlicker2_path_XM;
};

datablock afxXM_WorldOffsetData(RM_Fire3_offset_XM)
{
  worldOffset = "1.5 -2.3 0.25";
};
datablock afxEffectWrapperData(RM_Fire3_EW : RM_Fire1_EW)
{
  xfmModifiers[0] = RM_Fire3_offset_XM;
  xfmModifiers[1] = RM_FireFlicker3_path_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CORPSE ZODIAC

// this mooring anchors the corpse zodiacs and measures
// their altitudes

datablock afxMooringData(RM_CorpseZodeMooring_CE)
{
  networking = $AFX::CLIENT_ONLY;
  displayAxisMarker = false;
};
datablock afxEffectWrapperData(RM_CorpseZodeMooring_EW)
{
  effect = RM_CorpseZodeMooring_CE;
  posConstraint = target;
  effectName = "CorpseZodeMooring";
  isConstraintSrc = true;
  lifetime = 6.0;
  xfmModifiers[0] = SHARED_freeze_AltitudeConform_XM;
};

//
// This zodiac appears below the target corpse. 
//

// this is the white reveal glow for corpse zodiac
datablock afxZodiacData(RM_CorpseDevilReveal_CE : SHARED_ZodiacBase_CE)
{
  texture = %mySpellDataPath @ "/RM/zodiacs/RM_devil_reveal";
  radius = 6.0;
  startAngle = -37.5; //-45 + 7.5
  rotationRate = -30.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(RM_CorpseDevilReveal_EW)
{
  effect = RM_CorpseDevilReveal_CE;
  posConstraint = "#effect.CorpseZodeMooring";
  borrowAltitudes = true;
  delay = 2.25;//1.25;
  lifetime = 0.75;
  fadeInTime = 0.5;
  fadeOutTime = 0.25;
};

// this is the main target zodiac. orange and purple
// it features a devilish horned figure cradling the
// corpse in his claws.
datablock afxZodiacData(RM_CorpseDevil_CE : SHARED_ZodiacBase_CE)
{
  texture = %mySpellDataPath @ "/RM/zodiacs/RM_devil";
  radius = 6.0; //7.5;
  startAngle = -45.0;
  rotationRate = -30.0; //-40.0;
  color = "1.0 1.0 1.0 0.6";
  blend = additive;
};
//
datablock afxEffectWrapperData(RM_CorpseDevil_EW)
{
  effect = RM_CorpseDevil_CE;
  posConstraint = "#effect.CorpseZodeMooring";
  borrowAltitudes = true;
  delay = 2.5;//1.5;
  fadeInTime  = 1.0;
  fadeOutTime = 0.2;
  lifetime = 3.45;
};

// Devil Zode Underglow
datablock afxZodiacData(RM_CorpseDevil_underglow_CE : RM_CorpseDevil_CE)
{
  texture = %mySpellDataPath @ "/RM/zodiacs/RM_devil-underglow256";
  color = "0.6 0.6 0.6 0.6";
  blend = normal;
};
//
datablock afxEffectWrapperData(RM_CorpseDevil_underglow_EW : RM_CorpseDevil_EW)
{
  effect = RM_CorpseDevil_underglow_CE;
  execConditions = $BrightLighting_mask;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CORPSE INTAKE FLARES

// first flare
datablock afxZodiacData(RM_CorpseFlare1A_CE : SHARED_ZodiacBase_CE)
{
  texture = %mySpellDataPath @ "/RM/zodiacs/RM_corpse_flareA";
  radius = 10.0;
  startAngle = 0.0;
  rotationRate = 30;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
  growthRate = -13.0;
};
//
datablock afxEffectWrapperData(RM_CorpseFlare1A_EW)
{
  effect = RM_CorpseFlare1A_CE;
  posConstraint = "#effect.CorpseZodeMooring";
  borrowAltitudes = true;
  delay = 4.0;//3.0;
  fadeInTime  = 0.4;
  lifetime = 2.0;
};

datablock afxZodiacData(RM_CorpseFlare1B_CE : RM_CorpseFlare1A_CE)
{
  startAngle = 73.0;
  rotationRate = -30; //-400.0;
};
//
datablock afxEffectWrapperData(RM_CorpseFlare1B_EW : RM_CorpseFlare1A_EW)
{
  effect = RM_CorpseFlare1B_CE;
};

// second flare
datablock afxZodiacData(RM_CorpseFlare2A_CE : SHARED_ZodiacBase_CE)
{
  texture = %mySpellDataPath @ "/RM/zodiacs/RM_corpse_flareB";
  radius = 11.0;
  startAngle = 150.0;
  rotationRate = 45.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
  growthRate = -15.0;
};
//
datablock afxEffectWrapperData(RM_CorpseFlare2A_EW)
{
  effect = RM_CorpseFlare2A_CE;
  posConstraint = "#effect.CorpseZodeMooring";
  borrowAltitudes = true;
  delay = 4.5;//3.5;
  fadeInTime  = 0.4;
  lifetime = 2.0;
};

datablock afxZodiacData(RM_CorpseFlare2B_CE : RM_CorpseFlare2A_CE)
{
  startAngle = -15.0;
  rotationRate = -45.0;
};
//
datablock afxEffectWrapperData(RM_CorpseFlare2B_EW : RM_CorpseFlare2A_EW)
{
  effect = RM_CorpseFlare2B_CE;
};

// third flare
datablock afxZodiacData(RM_CorpseFlare3A_CE : SHARED_ZodiacBase_CE)
{
  texture = %mySpellDataPath @ "/RM/zodiacs/RM_corpse_flareC";
  radius = 13.0;
  startAngle = 75.0;
  rotationRate = 60.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
  growthRate = -21.0;
};
//
datablock afxEffectWrapperData(RM_CorpseFlare3A_EW)
{
  effect = RM_CorpseFlare3A_CE;
  posConstraint = "#effect.CorpseZodeMooring";
  borrowAltitudes = true;
  delay = 5.0;//4.0;
  fadeInTime  = 0.4;
  lifetime = 2.0;
};

datablock afxZodiacData(RM_CorpseFlare3B_CE : RM_CorpseFlare3A_CE)
{
  texture = %mySpellDataPath @ "/RM/zodiacs/RM_corpse_flareC";
  startAngle = 5.0;
  rotationRate = -60.0;
};
//
datablock afxEffectWrapperData(RM_CorpseFlare3B_EW :RM_CorpseFlare3A_EW )
{
  effect = RM_CorpseFlare3B_CE;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CORPSE SUNBURSTS

// first sunburst
datablock afxZodiacData(RM_CorpseSunburst1A_CE : SHARED_ZodiacBase_CE)
{
  texture = %mySpellDataPath @ "/RM/zodiacs/RM_corpse_glow";
  radius = 1.3;
  startAngle = 75.0;
  rotationRate = 60.0;
  color = "1.0 1.0 1.0 0.5";
  blend = additive;
  growthRate = 0.0;
};
//
datablock afxEffectWrapperData(RM_CorpseSunburst1A_EW)
{
  effect = RM_CorpseSunburst1A_CE;
  posConstraint = "#effect.CorpseZodeMooring";
  borrowAltitudes = true;
  delay = 4.5;//3.5;
  fadeInTime  = 0.1;
  fadeOutTime = 0.25;
  lifetime = 0.15;
};

datablock afxZodiacData(RM_CorpseSunburst1B_CE : RM_CorpseSunburst1A_CE)
{
  startAngle = -24.0;
  rotationRate = -60.0;
};
//
datablock afxEffectWrapperData(RM_CorpseSunburst1B_EW : RM_CorpseSunburst1A_EW)
{
  effect = RM_CorpseSunburst1B_CE;
};

// second sunburst
datablock afxZodiacData(RM_CorpseSunburst2A_CE : SHARED_ZodiacBase_CE)
{
  texture = %mySpellDataPath @ "/RM/zodiacs/RM_corpse_glow";
  radius = 1.6;
  startAngle = 222.0;
  rotationRate = 90.0;
  color = "1.0 1.0 1.0 0.5";
  blend = additive;
  growthRate = 0.0;
};
//
datablock afxEffectWrapperData(RM_CorpseSunburst2A_EW)
{
  effect = RM_CorpseSunburst2A_CE;
  posConstraint = "#effect.CorpseZodeMooring";
  borrowAltitudes = true;
  delay = 5.0;//4.0;
  fadeInTime  = 0.1;
  fadeOutTime = 0.35;
  lifetime = 0.15;
};

datablock afxZodiacData(RM_CorpseSunburst2B_CE : RM_CorpseSunburst2A_CE)
{
  startAngle = 11.0;
  rotationRate = -90.0;
};
//
datablock afxEffectWrapperData(RM_CorpseSunburst2B_EW : RM_CorpseSunburst2A_EW)
{
  effect = RM_CorpseSunburst2B_CE;
};

// third sunburst (long)
datablock afxZodiacData(RM_CorpseSunburst3A_CE : SHARED_ZodiacBase_CE)
{
  texture = %mySpellDataPath @ "/RM/zodiacs/RM_corpse_glow";
  radius = 1.85;
  startAngle = -60.0;
  rotationRate = 240.0;
  color = "1.0 1.0 1.0 0.5";
  blend = additive;
  growthRate = 0.45;
};
//
datablock afxEffectWrapperData(RM_CorpseSunburst3A_EW)
{
  effect = RM_CorpseSunburst3A_CE;
  posConstraint = "#effect.CorpseZodeMooring";
  borrowAltitudes = true;
  delay = 5.7;//4.7;//4.52;
  fadeInTime  = 0.1;
  fadeOutTime = 0.6;
  lifetime = 3.8;
};

datablock afxZodiacData(RM_CorpseSunburst3B_CE : RM_CorpseSunburst3A_CE)
{
  startAngle = 123.0;
  rotationRate = -240.0;
};
//
datablock afxEffectWrapperData(RM_CorpseSunburst3B_EW : RM_CorpseSunburst3A_EW)
{
  effect = RM_CorpseSunburst3B_CE;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CORPSE SHOCKWAVE

datablock afxZodiacData(RM_CorpseShockwaveA_CE)
{
  texture = %mySpellDataPath @ "/RM/zodiacs/RM_corpse_flareD";
  radius = 1.0;
  startAngle = 0.0;
  rotationRate = 200.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
  growthRate = 40.0;
  showOnInteriors = false;
};
//
datablock afxEffectWrapperData(RM_CorpseShockwaveA_EW)
{
  effect = RM_CorpseShockwaveA_CE;
  posConstraint = "target";
  delay = 5.75;//4.75; //4.5;
  fadeInTime  = 0.05;
  fadeOutTime = 0.4;
  lifetime = 0.1;
};

datablock afxZodiacData(RM_CorpseShockwaveB_CE : RM_CorpseShockwaveA_CE)
{
  startAngle = 66.0;
  rotationRate = -200.0;
  showOnInteriors = false;
};
//
datablock afxEffectWrapperData(RM_CorpseShockwaveB_EW : RM_CorpseShockwaveA_EW)
{
  effect = RM_CorpseShockwaveB_CE;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CORPSE PARTICLE EFFECTS

// yellow sparkle particles
datablock ParticleData(RM_Sparkle_P)
{
   // TGE textureName          = %mySpellDataPath @ "/RM/particles/sparkle";
   dragCoeffiecient     = 0.5;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 1000;
   lifetimeVarianceMS   = 300;
   useInvAlpha          = false;
   spinRandomMin        = 0.0;
   spinRandomMax        = 0.0;
   colors[0]            = "1.0 1.0 1.0 1.0";
   colors[1]            = "1.0 0.5 0.0 1.0";
   colors[2]            = "1.0 0.2 0.0 1.0";
   sizes[0]             = 0.8;
   sizes[1]             = 0.6;
   sizes[2]             = 0.1;
   times[0]             = 0.0;
   times[1]             = 0.3;
   times[2]             = 1.0;

   textureName          = %mySpellDataPath @ "/RM/particles/rm_tiled_parts"; // sparkle
   textureCoords[0]     = "0.0 0.0";
   textureCoords[1]     = "0.0 0.5";
   textureCoords[2]     = "0.5 0.5";
   textureCoords[3]     = "0.5 0.0";
};
datablock ParticleEmitterData(RM_Sparkle_E)
{
  ejectionPeriodMS      = 2;
  periodVarianceMS      = 1;
  ejectionVelocity      = 5.0;
  velocityVariance      = 1.6;
  thetaMin              = 0;
  thetaMax              = 0;
  particles             = "RM_Sparkle_P";  
};

datablock afxXM_SpinData(RM_Sparkle_spin_XM)
{
  spinAxis = "0 0 1";
  spinRate = 900;
};
datablock afxXM_LocalOffsetData(RM_Sparkle_offset_XM)
{
  localOffset = "0.0 1.6 0.0";
};
datablock afxEffectWrapperData(RM_Sparkle_EW)
{
  effect = RM_Sparkle_E;
  posConstraint = "target";
  delay = 5.8; //4.8;//4.4; 
  lifetime = 4.0;
  xfmModifiers[0] = RM_Sparkle_spin_XM;
  xfmModifiers[1] = RM_Sparkle_offset_XM;
};

// bubbly smoke particles
datablock ParticleData(RM_Smoke_P)
{
  // TGE textureName          = %mySpellDataPath @ "/RM/particles/smokeParticle";
  dragCoeffiecient     = 0.5;
  gravityCoefficient   = 0.0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 700;
  lifetimeVarianceMS   = 200;
  useInvAlpha          = false;
  spinRandomMin        = 0.0;
  spinRandomMax        = 0.0;
  colors[0]            = "0.0 0.0 1.0 1.0";
  colors[1]            = "0.5 0.5 1.0 1.0";
  colors[2]            = "1.0 1.0 1.0 1.0";
  sizes[0]             = 2.5;
  sizes[1]             = 0.9;
  sizes[2]             = 0.1;
  times[0]             = 0.0;
  times[1]             = 0.3;
  times[2]             = 1.0;

  // TGEA
  textureName          = %mySpellDataPath @ "/RM/particles/rm_tiled_parts"; // smokeParticle
  textureCoords[0]     = "0.5 0.5";
  textureCoords[1]     = "0.5 1.0";
  textureCoords[2]     = "1.0 1.0";
  textureCoords[3]     = "1.0 0.5";
};
datablock ParticleEmitterData(RM_Smoke_E)
{
  ejectionPeriodMS      = 6;
  periodVarianceMS      = 1;
  ejectionVelocity      = 4.0;
  velocityVariance      = 0.9;
  thetaMin              = 0;
  thetaMax              = 0;
  particles             = "RM_Smoke_P";  
};

datablock afxXM_SpinData(RM_Smoke1_spin_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 0;
  spinRate = 9000;
};
datablock afxXM_LocalOffsetData(RM_Smoke1_offset_XM)
{
  localOffset = "0.0 1.4 0.0";
};
datablock afxEffectWrapperData(RM_Smoke1_EW)
{
  effect = RM_Smoke_E;
  posConstraint = "target";
  delay = 6.2; //5.2;//4.8; 
  lifetime = 3.8;
  xfmModifiers[0] = RM_Smoke1_spin_XM;
  xfmModifiers[1] = RM_Smoke1_offset_XM;
};

datablock afxXM_SpinData(RM_Smoke2_spin_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 180;
  spinRate = 7000;
};
datablock afxXM_LocalOffsetData(RM_Smoke2_offset_XM)
{
  localOffset = "0.0 1.4 0.0";
};
datablock afxEffectWrapperData(RM_Smoke2_EW)
{
  effect = RM_Smoke_E;
  posConstraint = "target";
  delay = 6.6; //5.6;//5.2; 
  lifetime = 3.4;
  xfmModifiers[0] = RM_Smoke2_spin_XM;
  xfmModifiers[1] = RM_Smoke2_offset_XM;
};

datablock afxXM_SpinData(RM_Smoke3_spin_XM)
{
  spinAxis = "0 0 1";
  spinAngle = -76;
  spinRate = -5000;
};
datablock afxXM_LocalOffsetData(RM_Smoke3_offset_XM)
{
  localOffset = "0.0 1.4 0.0";
};
datablock afxEffectWrapperData(RM_Smoke3_EW)
{
  effect = RM_Smoke_E;
  posConstraint = "target";
  delay = 7.0;//6.0;//5.6; 
  lifetime = 3.0;
  xfmModifiers[0] = RM_Smoke3_spin_XM;
  xfmModifiers[1] = RM_Smoke3_offset_XM;
};

datablock afxXM_SpinData(RM_Smoke4_spin_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 23;
  spinRate = 5687;
};
datablock afxXM_LocalOffsetData(RM_Smoke4_offset_XM)
{
  localOffset = "0.0 1.8 0.0";
};
datablock afxEffectWrapperData(RM_Smoke4_EW)
{
  effect = RM_Smoke_E;
  posConstraint = "target";
  delay = 7.4;//6.4;//5.6; 
  lifetime = 2.6;
  xfmModifiers[0] = RM_Smoke4_spin_XM;
  xfmModifiers[1] = RM_Smoke4_offset_XM;
};

datablock afxXM_SpinData(RM_Smoke5_spin_XM)
{
  spinAxis = "0 0 1";
  spinAngle = -188;
  spinRate = -7556;
};
datablock afxXM_LocalOffsetData(RM_Smoke5_offset_XM)
{
  localOffset = "0.0 2.2 0.0";
};
datablock afxEffectWrapperData(RM_Smoke5_EW)
{
  effect = RM_Smoke_E;
  posConstraint = "target";
  delay = 7.8;//6.8;//5.6; 
  lifetime = 2.2;
  xfmModifiers[0] = RM_Smoke5_spin_XM;
  xfmModifiers[1] = RM_Smoke5_offset_XM;
};

datablock afxXM_SpinData(RM_Smoke6_spin_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 228;
  spinRate = 6000;
};
datablock afxXM_LocalOffsetData(RM_Smoke6_offset_XM)
{
  localOffset = "0.0 2.2 0.0";
};
datablock afxEffectWrapperData(RM_Smoke6_EW)
{
  effect = RM_Smoke_E;
  posConstraint = "target";
  delay = 7.8;//6.8;//5.6; 
  lifetime = 2.2;
  xfmModifiers[0] = RM_Smoke6_spin_XM;
  xfmModifiers[1] = RM_Smoke6_offset_XM;
};

datablock ParticleData(RM_Eye_P)
{
   // TGE textureName          = %mySpellDataPath @ "/RM/particles/RM_diamondeye";
   dragCoeffiecient     = 0.0;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 1000;
   lifetimeVarianceMS   = 300;
   useInvAlpha          = false;
   spinRandomMin        = 0.0;
   spinRandomMax        = 0.0;
   colors[0]            = "0.2 0.2 1.0 1.0";
   colors[1]            = "0.2 0.2 1.0 0.0";
   sizes[0]             = 0.55;
   sizes[1]             = 0.55;
   times[0]             = 0.8;
   times[1]             = 1.0;

   textureName          = %mySpellDataPath @ "/RM/particles/rm_tiled_parts"; // diamondEye
   textureCoords[0]     = "0.0 0.5";
   textureCoords[1]     = "0.0 1.0";
   textureCoords[2]     = "0.5 1.0";
   textureCoords[3]     = "0.5 0.5";
};
datablock ParticleEmitterData(RM_Eye_E)
{
  ejectionPeriodMS      = 20;
  periodVarianceMS      = 4;
  ejectionVelocity      = 6.0;
  velocityVariance      = 2.2;
  thetaMin              = 0;
  thetaMax              = 0;
  particles             = "RM_Eye_P";  
};

// diamond eye particles
datablock afxXM_SpinData(RM_DiamondEye_spin_XM)
{
  spinAxis = "0 0 1";
  spinRate = -5688;
};
datablock afxXM_LocalOffsetData(RM_DiamondEye_offset_XM)
{
  localOffset = "0.0 1.0 0.0";
};
datablock afxEffectWrapperData(RM_DiamondEye_EW)
{
  effect = RM_Eye_E;
  posConstraint = "target";
  delay = 5.8;//4.8;//4.4; 
  lifetime = 4.0;
  xfmModifiers[0] = RM_DiamondEye_spin_XM;
  xfmModifiers[1] = RM_DiamondEye_offset_XM;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Resurrection Script

//
// This script effect triggers the scripting that does the
// actual respawning of the resurrected character. 
//

datablock afxScriptEventData(RM_ResurrectScript_CE)
{
  methodName = "ResurrectEvent";   // name of method in afxMagicSpellData subclass
};
datablock afxEffectWrapperData(RM_ResurrectScript_EW)
{
  effect = RM_ResurrectScript_CE;
  constraint = "impactedObject";
  delay = 5.5;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/rm_lighting_tge_sub.cs");
    exec("./audio/rm_audio_sub.cs");
  case "TGEA":
    exec("./lighting/rm_lighting_tgea_sub.cs");
    exec("./audio/rm_audio_sub.cs");
 case "T3D":
    exec("./lighting/rm_lighting_t3d_sub.cs");
    exec("./audio/rm_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// REAPER MADNESS SPELL
//

datablock afxMagicSpellData(ReaperMadnessSpell)
{
    // warmup //
  castingDur = 4.0; //8.5;

    // spellcaster animation //
  addCastingEffect = RM_Prostrate_Clip_EW;
    // casting zodiac //
  addCastingEffect = RM_CastingZodeMooring_EW;
  addCastingEffect = RM_ZodeReveal_EW;
  addCastingEffect = RM_Zode1_underglow_EW;
  addCastingEffect = RM_Zode1_EW;
  addCastingEffect = RM_Zode2_underglow_EW;
  addCastingEffect = RM_Zode2_EW;
    // triangle fires //    
  addCastingEffect = RM_TriangleZode_EW;
  addCastingEffect = RM_Fire1_EW;
  addCastingEffect = RM_Fire2_EW;
  addCastingEffect = RM_Fire3_EW;

    // target corpse zodiac //
  addCastingEffect = RM_CorpseZodeMooring_EW;
  addCastingEffect = RM_CorpseDevilReveal_EW;
  addCastingEffect = RM_CorpseDevil_underglow_EW;
  addCastingEffect = RM_CorpseDevil_EW;
    // intake flares //    
  addCastingEffect = RM_CorpseFlare1A_EW;
  addCastingEffect = RM_CorpseFlare1B_EW;
  addCastingEffect = RM_CorpseFlare2A_EW;
  addCastingEffect = RM_CorpseFlare2B_EW;
  addCastingEffect = RM_CorpseFlare3A_EW;
  addCastingEffect = RM_CorpseFlare3B_EW;
    // sunbursts //
  addCastingEffect = RM_CorpseSunburst1A_EW;
  addCastingEffect = RM_CorpseSunburst1B_EW;
  addCastingEffect = RM_CorpseSunburst2A_EW;
  addCastingEffect = RM_CorpseSunburst2B_EW;
  addCastingEffect = RM_CorpseSunburst3A_EW;
  addCastingEffect = RM_CorpseSunburst3B_EW;
  addCastingEffect = $RM_CorpseSunburstSpotLight1;
  addCastingEffect = $RM_CorpseSunburstSpotLight2;
    // shockwave //
  addCastingEffect = RM_CorpseShockwaveA_EW;
  addCastingEffect = RM_CorpseShockwaveB_EW;
    // corpse particles //
  addCastingEffect = RM_Sparkle_EW;
  addCastingEffect = RM_Smoke1_EW;
  addCastingEffect = RM_Smoke2_EW;
  addCastingEffect = RM_Smoke3_EW;
  addCastingEffect = RM_Smoke4_EW;
  addCastingEffect = RM_Smoke5_EW;
  addCastingEffect = RM_Smoke6_EW;
  addCastingEffect = RM_DiamondEye_EW;

  lingerDur = 5.5;
  addLingerEffect = RM_ResurrectScript_EW;
};

// sounds and lights added via sub-script functions //
RM_add_Lighting_FX(ReaperMadnessSpell);
RM_add_Audio_FX(ReaperMadnessSpell);

datablock afxRPGMagicSpellData(ReaperMadnessSpell_RPG)
{
  spellName = "Reaper Madness";
  desc = "You play a game of chess against DEATH... and win! " @
         "Resurrect a corpse of your choosing." @
         "\n" @
         "\nspell design: Matthew Durante" @
         "\nsound effects: Dave Schroeder" @ 
         "\nspell concept: Jeff Faust";
  sourcePack = "Core Tech";

  iconBitmap = %mySpellDataPath @ "/RM/icons/rm";
  target = "corpse";
  range = 20;
  manaCost = 10;
  castingDur = ReaperMadnessSpell.castingDur;
};

// script methods
function ReaperMadnessSpell::ResurrectEvent(%this, %spell, %caster, %corpse, %pos, %data)
{
  afxResurrectCorpse(%corpse);
}

// set a level of detail
function ReaperMadnessSpell::onActivate(%this, %spell, %caster, %target)
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
  ReaperMadnessSpell.scriptFile = $afxAutoloadScriptFile;
  ReaperMadnessSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(ReaperMadnessSpell, ReaperMadnessSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//