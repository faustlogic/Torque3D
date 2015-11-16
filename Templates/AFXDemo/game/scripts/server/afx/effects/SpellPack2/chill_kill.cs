
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// CHILL KILL SPELL
//
// Add snow, ice, gravel, and subzero temps proportioned to taste.
// Stir and throw, with authority! Also good for chilling large flagons of mead.
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
$spell_reload = isObject(ChillKillSpell);
if ($spell_reload)
{
  IceShardsProp::kill();

  // mark datablocks so we can detect which are reloaded in this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = ChillKillSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GLOBALS

// ParticleEmitterData::useEmitterTransform is an alternative to 
// ParticleData::constrainPos but it currently does not work in TGEA.
$UseEmitterTransform = false;  // should always be false on TGEA

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPELLCASTER GESTURING (animation)

//
// With a quick, magic twist of his torso the orc open up a portal to
//  some cold hell beneath him.  As he gestures rhythmically he wills
//  the rising snow and ice into an iceball.  Then he lunges forward,
//  launching it at his target.
//

datablock afxAnimClipData(CK_Casting_Clip_CE)
{
  clipName = "ck";
  ignoreCorpse = true;
  rate = 1.0;
};
//
datablock afxEffectWrapperData(CK_Casting_Clip_EW)
{
  effect = CK_Casting_Clip_CE;
  constraint = caster;
  lifetime = 220/30 - (10/30);  // for better blending into end pose...
  delay = 0.0;
};

datablock afxAnimLockData(CK_AnimLock_CE)
{
  priority = 0;
};
//
datablock afxEffectWrapperData(CK_AnimLock_EW)
{
  effect = CK_AnimLock_CE;
  constraint = caster;
  delay = 0.0;
  lifetime = 220/30;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING HAND MAGIC (particles)

//
// As the spellcaster twists to form the magic ring around him, a
//  magic snow falls from his hands.
//  
// Two basic particle types are used, one a general "magic" particle
//  suitable to snow, the other actual snowflakes that trail behind.
//  For this snow, two particle images are reused to create small,
//  medium and large variations, totaling six particle types.  These
//  snow particles are reused throughout the spell.
//

// Snow Magic particle
datablock ParticleData(CK_HandMagic_P)
{
  textureName          = %mySpellDataPath @ "/CK/particles/ck_snow_magic";
  dragCoeffiecient     = 0.5;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 600;
  lifetimeVarianceMS   = 100;
  spinRandomMin        = -200;
  spinRandomMax        = 200;
  colors[0]            = "1.0 1.0 1.0 1.0";
    colors[1]            = "0.8 0.8 0.8 1.0";
    colors[2]            = "0.0 0.0 0.0 0.0";
  sizes[0]             = 1.0;
    sizes[1]             = 0.5; 
    sizes[2]             = 0.05;
  times[0]             = 0.0;
    times[1]             = 0.5;
    times[2]             = 1.0;
};

// JTF Notes:
//  consider varying size using sizeBias and randomness

// Snow A, Large size
datablock ParticleData(CK_SnowA_L_P)
{
  textureName          = %mySpellDataPath @ "/CK/particles/ck_tiled_snow_blood";
  dragCoeffiecient     = 0.5;
  gravityCoefficient   = 0.1;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 1600;
  lifetimeVarianceMS   = 300;
  spinRandomMin        = -200;
  spinRandomMax        = 200;
  colors[0]            = "1.0 1.0 1.0 1.0";
    colors[1]            = "0.8 0.8 0.8 1.0";
    colors[2]            = "0.0 0.0 0.0 0.0";
  sizes[0]             = 0.2;
    sizes[1]             = 0.2; 
    sizes[2]             = 0.2;
  times[0]             = 0.0;
    times[1]             = 0.5;
    times[2]             = 1.0;
  textureCoords[0] = "0.0 0.0";
    textureCoords[1] = "0.0 0.5";
    textureCoords[2] = "0.5 0.5";
    textureCoords[3] = "0.5 0.0";
};
// Snow B, Large size
datablock ParticleData(CK_SnowB_L_P : CK_SnowA_L_P)
{
  textureName = %mySpellDataPath @ "/CK/particles/ck_tiled_snow_blood";
  textureCoords[0] = "0.5 0.0";
    textureCoords[1] = "0.5 0.5";
    textureCoords[2] = "1.0 0.5";
    textureCoords[3] = "1.0 0.0";
};
// Snow A, Medium size
datablock ParticleData(CK_SnowA_M_P : CK_SnowA_L_P)
{
  sizes[0] = 0.1;
    sizes[1] = 0.1; 
    sizes[2] = 0.1;
};
// Snow B, Medium size
datablock ParticleData(CK_SnowB_M_P : CK_SnowB_L_P)
{
  sizes[0] = 0.1;
    sizes[1] = 0.1; 
    sizes[2] = 0.1;
};
// Snow A, Small size
datablock ParticleData(CK_SnowA_S_P : CK_SnowA_L_P)
{
  sizes[0] = 0.5;
    sizes[1] = 0.5; 
    sizes[2] = 0.5;
};
// Snow B, Small size
datablock ParticleData(CK_SnowB_S_P : CK_SnowB_L_P)
{
  sizes[0] = 0.5;
    sizes[1] = 0.5; 
    sizes[2] = 0.5;
};

// Hand Magic emitter
datablock ParticleEmitterData(CK_HandMagic_E)
{
  ejectionPeriodMS      = 30;
  periodVarianceMS      = 5;
  ejectionVelocity      = 0;
  velocityVariance      = 0;
  thetaMin              = 0.0;
  thetaMax              = 0.0;
  particles             = CK_HandMagic_P;
  blendStyle            = "PREMULTALPHA";
};

// Hand Magic, left and right hands
datablock afxEffectWrapperData(CK_HandMagic_lfhand_EW)
{
  effect = CK_HandMagic_E;
  constraint = "caster.Bip01 L Hand";
  delay = 20/30;
  lifetime = (33-20)/30;
};
datablock afxEffectWrapperData(CK_HandMagic_rthand_EW : CK_HandMagic_lfhand_EW)
{
  constraint = "caster.Bip01 R Hand";
};

// Hand Snow emitter
datablock ParticleEmitterData(CK_HandSnow_E)
{
  ejectionPeriodMS      = 10;
  periodVarianceMS      = 5;
  ejectionVelocity      = 0.7; 
  velocityVariance      = 0.3; 
  thetaMin              = 0.0;
  thetaMax              = 180.0; 
  particles             = "CK_SnowA_L_P CK_SnowB_L_P" SPC
                          "CK_SnowA_L_P CK_SnowB_L_P" SPC
                          "CK_SnowA_M_P CK_SnowB_M_P" SPC
                          "CK_SnowA_M_P CK_SnowB_M_P" SPC
                          "CK_SnowA_S_P CK_SnowB_S_P" SPC
                          "CK_SnowA_S_P CK_SnowB_S_P";
  blendStyle = "PREMULTALPHA";
};

// Hand snow, left and right hands
datablock afxEffectWrapperData(CK_HandSnow_lfhand_EW)
{
  effect = CK_HandSnow_E;
  constraint = "caster.Bip01 L Hand";
  delay = 20/30;
  lifetime = (33-20)/30;
};
datablock afxEffectWrapperData(CK_HandSnow_rthand_EW : CK_HandSnow_lfhand_EW)
{
  constraint = "caster.Bip01 R Hand";
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING ZODIACS (zodiac)

// this mooring anchors the casting zodiacs and measures
// their altitudes

datablock afxMooringData(CK_CastingZodeMooring_CE)
{
  networking = $AFX::CLIENT_ONLY;
  displayAxisMarker = false;
};
datablock afxEffectWrapperData(CK_CastingZodeMooring_EW)
{
  effect = CK_CastingZodeMooring_CE;
  constraint = caster;
  effectName = "CastingZodeMooring";
  isConstraintSrc = true;
  lifetime = 6.0;
  xfmModifiers[0] = SHARED_AltitudeConform_XM;
};

//
// The casting zodiacs here create a complex multi-layered effect,
//  built upon the idea of a portal to a great snowy vortex from
//  which the snow and ice spews.  The design is naturally of a ring,
//  allowing the caster to remain on firm ground.
//
// The order outlined here is logical, but the actual ordering
//  specified in the spell (as effect's are added to the casting
//  phrase) is what determines the compositing order:
//    CK_CastingZode_Reveal_EW -- a comet-shaped reveal zodiac,
//      designed to mimic the motion of the spellcaster as he waves-
//      on the effect
//    CK_CastingZode_Snow_EW -- the base snow image, designed to
//      suggest the inside of a snowy vortex
//    CK_CastingZode_SnowA_EW
//    CK_CastingZode_SnowB_EW
//    CK_CastingZode_SnowC_EW
//    CK_CastingZode_SnowD_EW -- snow zodiacs, with different rotation
//      settings, to look like snow inside the vortex
//    CK_CastingZode_Rings_EW -- a static ring pattern that loosely
//      defines the border of the snow vortex ring
//    CK_CastingZode_OuterRing_EW
//    CK_CastingZode_OuterRingB_EW
//    CK_CastingZode_InnerRing_EW
//    CK_CastingZode_InnerRingB_EW -- rotating rings that enhance the
//      sense of the vortex twisting
//

// Comet-like Reveal Zodiac
datablock afxZodiacData(CK_CastingZode_Reveal_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/CK/zodiacs/ck_castingZode_reveal";
  radius = 3.5;
  startAngle = 30;
  rotationRate = -300.0;  
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(CK_CastingZode_Reveal_EW)
{
  effect = CK_CastingZode_Reveal_CE;
  constraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = (20/30)+(3/30);
  lifetime = 0.6;
  fadeInTime = 0.25; //0.5;
  fadeOutTime = 0.75;
};

// Base snow vortex zode
datablock afxZodiacData(CK_CastingZode_Snow_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/CK/zodiacs/ck_castingZode_snow";
  radius = 3.5;
  startAngle = 0;
  rotationRate = 180.0;  
  color = "0.5 0.5 0.5 1.0";
  blend = normal;
};
//
datablock afxEffectWrapperData(CK_CastingZode_Snow_EW)
{
  effect = CK_CastingZode_Snow_CE;
  constraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = (20/30)+0.5;
  lifetime = 4.0;
  fadeInTime = 0.5;
  fadeOutTime = 0.75;
};

// Snow zodiacs
//  - unique rotation values create randomness
datablock afxZodiacData(CK_CastingZode_SnowA_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/CK/zodiacs/ck_castingZode_snowA";
  radius = 3.5;
  startAngle = 0;
  rotationRate = 90.0;  
  color = "1.0 1.0 1.0 1.0";
  blend = normal;
};
datablock afxZodiacData(CK_CastingZode_SnowB_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/CK/zodiacs/ck_castingZode_snowB";
  radius = 3.5;
  startAngle = 0;
  rotationRate = -170.0;  
  color = "1.0 1.0 1.0 1.0";
  blend = normal;
};
datablock afxZodiacData(CK_CastingZode_SnowA2_CE : CK_CastingZode_SnowA_CE)
{
  startAngle = 77;
  rotationRate = -110.0;
};
datablock afxZodiacData(CK_CastingZode_SnowB2_CE : CK_CastingZode_SnowB_CE)
{ 
  startAngle = -93;
  rotationRate = 190.0;
};
//
datablock afxEffectWrapperData(CK_CastingZode_SnowA_EW)
{
  effect = CK_CastingZode_SnowA_CE;
  constraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = (20/30)+0.5;
  lifetime = 4.0;
  fadeInTime = 0.5;
  fadeOutTime = 0.75;
};
datablock afxEffectWrapperData(CK_CastingZode_SnowB_EW)
{
  effect = CK_CastingZode_SnowB_CE;
  constraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = (20/30)+0.5;
  lifetime = 4.0;
  fadeInTime = 0.5;
  fadeOutTime = 0.75;
};
datablock afxEffectWrapperData(CK_CastingZode_SnowC_EW : CK_CastingZode_SnowA_EW)
{
  effect = CK_CastingZode_SnowA2_CE;
};
datablock afxEffectWrapperData(CK_CastingZode_SnowD_EW : CK_CastingZode_SnowA_EW)
{
  effect = CK_CastingZode_SnowB2_CE;
};

// Static golden zodiac rings
datablock afxZodiacData(CK_CastingZode_Rings_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/CK/zodiacs/ck_castingZode_rings";
  radius = 3.5;
  startAngle = 0;
  rotationRate = 0.0;  
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(CK_CastingZode_Rings_EW)
{
  effect = CK_CastingZode_Rings_CE;
  constraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = (20/30)+0.5;
  lifetime = 4.0;
  fadeInTime = 0.5;
  fadeOutTime = 0.75;
};

// Animated golden outer zodiac, positive rotation
datablock afxZodiacData(CK_CastingZode_OuterRing_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/CK/zodiacs/ck_castingZode_outerRing";
  radius = 3.5;
  startAngle = 0;
  rotationRate = 25.0;  
  color = "0.65 0.65 0.65 0.65";
  blend = additive;
};
//
datablock afxEffectWrapperData(CK_CastingZode_OuterRing_EW)
{
  effect = CK_CastingZode_OuterRing_CE;
  constraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = (20/30)+0.5;
  lifetime = 4.0;
  fadeInTime = 0.5;
  fadeOutTime = 0.75;
};

// Animated golden outer zodiac, negative rotation
datablock afxZodiacData(CK_CastingZode_OuterRingB_CE : CK_CastingZode_OuterRing_CE)
{    
  rotationRate = -25.0;  
};
//
datablock afxEffectWrapperData(CK_CastingZode_OuterRingB_EW : CK_CastingZode_OuterRing_EW)
{
  effect = CK_CastingZode_OuterRingB_CE;
};

// Animated golden inner zodiac, negative rotation
datablock afxZodiacData(CK_CastingZode_InnerRing_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/CK/zodiacs/ck_castingZode_innerRing";
  radius = 3.5;
  startAngle = 0;
  rotationRate = -135.0;  
  color = "0.65 0.65 0.65 0.65";
  blend = additive;
};
//
datablock afxEffectWrapperData(CK_CastingZode_InnerRing_EW)
{
  effect = CK_CastingZode_InnerRing_CE;
  constraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = (20/30)+0.5;
  lifetime = 4.0;
  fadeInTime = 0.5;
  fadeOutTime = 0.75;
};

// Animated golden inner zodiac, positive rotation
datablock afxZodiacData(CK_CastingZode_InnerRingB_CE : CK_CastingZode_InnerRing_CE)
{   
  rotationRate = 135.0; 
};
//
datablock afxEffectWrapperData(CK_CastingZode_InnerRingB_EW : CK_CastingZode_InnerRing_EW)
{
  effect = CK_CastingZode_InnerRingB_CE;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING ZODIAC SNOW (particles)

//
// In addition to the zodiacs defined above, particles are used to
//  enhance the zodiac effect, creating snow around and above the
//  vortex.  The emitters spin with the reveal zodiac as it wipes-on
//  the effect.
//

// Zodiac Snow A, standard "sprinkler" emitter
datablock ParticleEmitterData(CK_CastingZodeSnowA_E)
{
  ejectionPeriodMS      = 4;
  periodVarianceMS      = 1;
  ejectionVelocity      = 1.0; 
  velocityVariance      = 0.5; 
  thetaMin              = 0.0;
  thetaMax              = 180.0;  
  particles             = "CK_SnowA_L_P CK_SnowB_L_P" SPC
                          "CK_SnowA_L_P CK_SnowB_L_P" SPC
                          "CK_SnowA_M_P CK_SnowB_M_P" SPC
                          "CK_SnowA_M_P CK_SnowB_M_P" SPC
                          "CK_SnowA_S_P CK_SnowB_S_P" SPC
                          "CK_SnowA_S_P CK_SnowB_S_P";  
  
  blendStyle = "PREMULTALPHA";
};

// Zodiac Snow B, disc emitter pointing up
datablock afxParticleEmitterDiscData(CK_CastingZodeSnowB_E)
{
  ejectionPeriodMS      = 4;
  periodVarianceMS      = 1;
  ejectionVelocity      = 1.0; 
  velocityVariance      = 0.5;   
  particles             = "CK_SnowA_L_P CK_SnowB_L_P" SPC
                          "CK_SnowA_L_P CK_SnowB_L_P" SPC
                          "CK_SnowA_M_P CK_SnowB_M_P" SPC
                          "CK_SnowA_M_P CK_SnowB_M_P" SPC
                          "CK_SnowA_S_P CK_SnowB_S_P" SPC
                          "CK_SnowA_S_P CK_SnowB_S_P";

  vector = "0 0 1";
  radiusMin = 0.0;
  radiusMax = 0.5;
  
  blendStyle = "PREMULTALPHA";
};

// Offset defining the radius of the vortex
datablock afxXM_LocalOffsetData(CK_CastingZodeSnow_offset_XM)
{
  localOffset = "0" SPC ((3.5*1.45)/2) SPC "0";
};
// Ground conform
datablock afxXM_GroundConformData(CK_CastingZodeSnow_ground_XM)
{
  height = 0.1;
};
// Spin to rotate the emitters to match the reveal zodiac
datablock afxXM_SpinData(CK_CastingZodeSnow_spin_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 30;
  spinRate = -300;
};

// Zodiac Snow, A & B
datablock afxEffectWrapperData(CK_CastingZodeSnowA_EW : CK_CastingZode_Reveal_EW)
{
  effect = CK_CastingZodeSnowA_E;
  xfmModifiers[1] = CK_CastingZodeSnow_offset_XM;
  xfmModifiers[0] = CK_CastingZodeSnow_spin_XM;
  xfmModifiers[2] = CK_CastingZodeSnow_ground_XM;
  fadeinTime = 0.0;
  fadeOutTime = 0.0;
  lifetime = 0.6+0.75;
};
datablock afxEffectWrapperData(CK_CastingZodeSnowB_EW : CK_CastingZodeSnowA_EW)
{
  effect = CK_CastingZodeSnowB_E;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// ICEBALL FORMATION SNOW

//
// As snow begins to spew from the magic vortex, a great volume of
//  it forms around the caster and heads for a point above his head.
//
// This effect is done by using a disc particle emitter with a
//  transform path.  Transform paths allow particles to move along a
//  path relative to their initial emission position.  When "concentric"
//  is flagged on the path, the path is essentially revolved around
//  the disc's axis.  The result is a revolved volume of particles.
//
// To enhance the feeling of snow, a second emitter is used to create
//  a snowy haze.  This haze occurs above the caster's head where all
//  the snow is accumulating.
//

// The concentric profile path for the disc emitter
//  -- x-values scale the width of the particle volume
//  -- z-values scales the height of the particle volume
//  -- y-values are ignored
datablock afxPathData(CK_SnowFormation_path)
{
  points = "-0.6 0 -5" SPC "0 0 0" SPC
           "0.2899779934 0"   SPC (0.4441380561*0.8) SPC
           "0.4902937452 0"   SPC (1.167500493 *0.8) SPC
           "0.3122352991 0"   SPC (2.024406765 *0.8) SPC
           "-0.03908829323 0" SPC (3.544858098 *0.8) SPC
           "-0.7203378381 0"  SPC (4.632338964 *0.8) SPC
           "-1.355311602 0"   SPC (5.144025803 *0.8);

  concentric = true;
};

// Snow Formation disc emitter, using a transform path
datablock afxParticleEmitterDiscData(CK_SnowFormation_E)
{
  ejectionPeriodMS      = 2;
  periodVarianceMS      = 1;
  ejectionVelocity      = 0.5; 
  velocityVariance      = 0.5; 
 
  particles             = "CK_SnowA_L_P CK_SnowB_L_P" SPC
                          "CK_SnowA_L_P CK_SnowB_L_P" SPC
                          "CK_SnowA_M_P CK_SnowB_M_P" SPC
                          "CK_SnowA_M_P CK_SnowB_M_P" SPC
                          "CK_SnowA_S_P CK_SnowB_S_P" SPC
                          "CK_SnowA_S_P CK_SnowB_S_P";

  vector = "0 0 1";
  radiusMin = 2.3;
  radiusMax = 2.6;

  pathsTransform = "CK_SnowFormation_path";
  overrideAdvance = true;
  
  blendStyle = "PREMULTALPHA";
};

// Snow Formation
datablock afxEffectWrapperData(CK_SnowFormation_EW)
{
  effect = CK_SnowFormation_E;  
  posConstraint = caster;

  delay = 1.5+0.3;
  lifetime = 1.8-0.3;
  fadeInTime = 0;
  fadeOutTime = 0;
};

// Haze particle
datablock ParticleData(CK_SnowHazeA_P)
{
  textureName          = %mySpellDataPath @ "/CK/particles/ck_snow_haze";
  dragCoeffiecient     = 0.5;
  gravityCoefficient   = 0.1;
  //windCoefficient      = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 1600;
  lifetimeVarianceMS   = 300;

  // TGE blendType            = normal;

  spinRandomMin        = -200;
  spinRandomMax        = 200;

  colors[0]            = "1.0 1.0 1.0 0.7";
  colors[1]            = "1.0 1.0 1.0 0.7";
  colors[2]            = "1.0 1.0 1.0 0.0";

  sizes[0]             = 2.0;
  sizes[1]             = 2.5; 
  sizes[2]             = 3.0;

  times[0]             = 0.0;
  times[1]             = 0.5;
  times[2]             = 1.0;
};

// Haze emitter, standard "sprinkler" type
datablock ParticleEmitterData(CK_SnowFormationHaze_E)
{
  ejectionPeriodMS      = 20;
  periodVarianceMS      = 10;
  ejectionVelocity      = 1.5; 
  velocityVariance      = 0.75; 
  thetaMin              = 0.0;
  thetaMax              = 180.0;
  particles             = "CK_SnowHazeA_P";

  fadeColor = false;
  fadeAlpha = true;

  blendStyle = "NORMAL";
};

// Haze offset, above the caster's head
datablock afxXM_LocalOffsetData(CK_SnowFormationHaze_offset_XM)
{
  localOffset = "0 0 3.8";
};

// Haze
datablock afxEffectWrapperData(CK_SnowFormationHaze_EW)
{
  effect = CK_SnowFormationHaze_E;
  constraint = caster;
  delay = 3.0;
  fadeinTime = 0.5;
  fadeOutTime = 1.0;
  lifetime = 2.0;
  xfmModifiers[0] = CK_SnowFormationHaze_offset_XM;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// ICEBALL

//
// The iceball model that forms above the caster is animated with
//  shards that propel outward.  Beyond this, however, all its
//  animation is done here with transform modifiers.  A second model
//  with shards already in place is also used.
//
// The difficulty with the iceball is that it is also used as a
//  missile.  However, the casting phrase of the spell, where no
//  missile constraint exists, and the delivery phrase where it does
//  represents a boundary that can only be crossed by doing a model
//  swap: just as the missile comes into existence, the casting
//  iceball must be hidden and replaced with a new iceball constrained
//  to the missile, and this swap must happen in the same place.  This
//  swap generally necessitates that the iceball be still when it
//  occurs.
//
// Now there are two models, one for casting and one for delivery, but
//  to further complicate things the casting iceball is broken down
//  into an animated and a static version.  Why escapes me, and a 
//  search of old emails has failed to reveal the answer.  I suspect
//  it was the simpler thing to do at the time.
//
// Here is the sequence of iceball creation:
//  CK_IceBall_EW(casting) > CK_IceBallStatic_EW(casting)
//    > CK_IceBallStatic_Missile_EW(delivery)
//
// To create interest between the transition of the iceball formation
//  and the missile launch, CK_IceBallStatic_EW is animated to drop
//  and twist.
//
// (CK_IceBallStatic_Missile_EW is defined in "ICEBALL MISSILE".)
//
// In addition to the iceballs themselves, glint models are used to
//  add interest.  These are dts models files full of tiny planes that
//  have been positioned on the iceball surface and oriented outward.
//  Then they are animated to scale-up randomly, creating a sparkle
//  effect.
//
// Finally, a particle emitters is timed to create a bursting snow
//  effect just as the shards pierce through the animated iceball.
//

// Iceball model, with projecting shard animation
datablock afxModelData(CK_IceBall_CE)
{
  shapeFile = %mySpellDataPath @ "/CK/models/ck_iceball.dts";
  sequence = "shards";  
};

// Iceball model, static (complete with shards)
datablock afxModelData(CK_IceBallStatic_CE)
{
  shapeFile = %mySpellDataPath @ "/CK/models/ck_iceball_static.dts";
};

// Height of iceball above caster
datablock afxXM_LocalOffsetData(CK_IceBall_offset_XM)
{
  localOffset = "0 0 3.0";
};

// Scale-up the iceball over time
datablock afxXM_ScaleData(CK_IceBall_scale_XM)
{
  scale = "2.0 2.0 2.0";

  delay = 0.0;
  fadeInTime = 1.5;
};

// Iceball rotation, gradually fading-out
datablock afxXM_SpinData(CK_IceBall_spin_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 0;
  spinRate = -360;

  delay = 0;
  lifetime = 0;
  fadeOutTime = 2.0;
};

// Wiggle paths used to slightly perturb the iceball
//  -- the paths are timed so that at the end of the iceball's
//      lifetime, the end point of "0 0 0" is reached and the
//      path's create no offset
datablock afxPathData(CK_IceBall_wiggleX_path)
{
  points = "0 0 0  -1 0 0  0 0 0  1 0 0  0 0 0";
  loop = cycle;
  lifetime = 1.0; 
  mult = 0.12;
};
datablock afxPathData(CK_IceBall_wiggleY_path)
{
  points = "0 0 0  0 -1 0  0 0 0  0 1 0  0 0 0";
  loop = cycle;
  lifetime = 2.0;
  mult = 0.12;
};
datablock afxXM_PathConformData(CK_IceBall_wiggle_paths_XM)
{
  paths = "CK_IceBall_wiggleX_path CK_IceBall_wiggleY_path";
};

// Height offset prior to falling (occurs in CK_IceBallStatic_EW)
datablock afxXM_LocalOffsetData(CK_IceBall_fall_offset_XM)
{
  localOffset = "0 0 0.75";
};

// Iceball, Animated
datablock afxEffectWrapperData(CK_IceBall_EW)
{
  effect = CK_IceBall_CE;
  constraint = caster;
  scaleFactor = 0.5;
  delay = 3.0;
  fadeinTime = 1.0;
  fadeOutTime = 0;
  lifetime = 2.0;
  xfmModifiers[0] = CK_IceBall_offset_XM;
  xfmModifiers[1] = CK_IceBall_scale_XM;
  xfmModifiers[2] = CK_IceBall_spin_XM;
  xfmModifiers[3] = CK_IceBall_wiggle_paths_XM;
  xfmModifiers[4] = CK_IceBall_fall_offset_XM;
};

%IceBallStatic_lifetime = ((165/30)-(2.0+3.0)) + (2/30);

// Height offset that fades-out to simulate falling
//  -- this moves CK_IceBallStatic_EW to the right height for the swap
datablock afxXM_LocalOffsetData(CK_IceBallStatic_fall_offset_XM)
{
  localOffset = "0 0 0.75";
  delay = 0;
  lifetime = 0.3;
  fadeOutTime = ((165/30)-(2.0+3.0))-0.3;
};

// Z Wiggle to add some interest
datablock afxPathData(CK_IceBallStatic_wiggleZ_path)
{
  points = "0 0 0  0 0 -1.5  0 0 0  0 0 0.5  0 0 0";  
  mult = 0.2;
};
datablock afxXM_PathConformData(CK_IceBallStatic_wiggle_paths_XM)
{
  paths = "CK_IceBallStatic_wiggleZ_path";
};

// X spin, causing the static iceball to rotate down before launch
datablock afxXM_SpinData(CK_IceBallStatic_spin_XM)
{
  spinAxis = "1 0 0";
  spinAngle = 0;
  spinRate = 360;

  delay = 0;
  lifetime    = %IceBallStatic_lifetime / 2.0;
  fadeInTime  = %IceBallStatic_lifetime / 2.0;
  fadeOutTime = %IceBallStatic_lifetime / 2.0;
};

// Iceball, Static
datablock afxEffectWrapperData(CK_IceBallStatic_EW)
{
  effect = CK_IceBallStatic_CE;
  constraint = caster;
  scaleFactor = 0.5+2.0;
  delay = 2.0+3.0;
  fadeinTime  = 0;
  fadeOutTime = 0;
  lifetime = ((165/30)-(2.0+3.0)) + (2/30);
  xfmModifiers[0] = CK_IceBall_offset_XM;
  xfmModifiers[1] = CK_IceBallStatic_spin_XM;
  xfmModifiers[2] = CK_IceBallStatic_fall_offset_XM;
  xfmModifiers[3] = CK_IceBallStatic_wiggle_paths_XM;
};

// Iceball Glints model
datablock afxModelData(CK_IceBallGlints_CE)
{
  shapeFile = %mySpellDataPath @ "/CK/models/ck_iceball_glints.dts";
  sequence = "glint";
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating; // TGE (ignored by TGEA)
  remapTextureTags = "CK_IceCrystalsA.png:CK_IceCrystalsGlint"; // TGEA (ignored by TGE)
};

// Iceball Glints for animated iceball
datablock afxEffectWrapperData(CK_IceBallGlints_EW : CK_IceBall_EW)
{
  effect = CK_IceBallGlints_CE;
};
// Iceball Glints for static iceball
datablock afxEffectWrapperData(CK_IceBallGlintsStatic_EW : CK_IceBallStatic_EW)
{
  effect = CK_IceBallGlints_CE;
};

// Iceball bursting snow emitter
datablock ParticleEmitterData(CK_IceBall_BurstSnow_E)
{
  ejectionPeriodMS      = 2;
  periodVarianceMS      = 1;
  ejectionVelocity      = 8.0; 
  velocityVariance      = 2.0; 
  thetaMin              = 0.0;
  thetaMax              = 180.0; 
  particles             = "CK_SnowA_L_P CK_SnowB_L_P" SPC
                          "CK_SnowA_L_P CK_SnowB_L_P" SPC
                          "CK_SnowA_M_P CK_SnowB_M_P" SPC
                          "CK_SnowA_M_P CK_SnowB_M_P" SPC
                          "CK_SnowA_S_P CK_SnowB_S_P" SPC
                          "CK_SnowA_S_P CK_SnowB_S_P";
                          
  blendStyle = "PREMULTALPHA";
};

// Iceball bursting snow
datablock afxEffectWrapperData(CK_IceBall_BurstSnow_EW)
{
  effect = CK_IceBall_BurstSnow_E;
  constraint = caster;
  delay = 5.0-0.23;
  lifetime = 0.1;
  fadeInTime = 0;
  fadeOutTime = 0;
  xfmModifiers[0] = CK_IceBall_offset_XM;
  xfmModifiers[1] = CK_IceBall_fall_offset_XM;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// ICEBALL MISSILE

//
// Here the missile for the iceball is defined, as well as the static
//  iceball that will be constrainted to the missile during delivery.
//  This missile also has glints.
//
// Additionally particle emitters are setup that emit snow from the
//  iceball missile in various configurations.
//

// Iceball Missile
datablock afxMagicMissileData(CK_IceBall_Missile)
{
  muzzleVelocity        = 30;
  velInheritFactor      = 0;
  lifetime              = 20000;
  isBallistic           = true;
  ballisticCoefficient  = 0.95;
  gravityMod            = 0.05;
  isGuided              = true;
  precision             = 30;
  trackDelay            = 7;

  launchOffset = "0.0 0.0 3.0";
  echoLaunchOffset = false;  
};

// Iceball X-spin
// -- the initial angle here is calculated to match the final angle of
//      CK_IceBallStatic_EW
datablock afxXM_SpinData(CK_IceBallMissile_spin_XM)
{
  spinAxis = "1 0 0"; 
  spinAngle = (360*(%IceBallStatic_lifetime/2.0));
  spinRate = -1200; //-1800;

  fadeInTime = 0.7;//1.0;
};

// Iceball, Static Missile
datablock afxEffectWrapperData(CK_IceBallStatic_Missile_EW)
{
  effect = CK_IceBallStatic_CE;
  constraint = missile;
  scaleFactor = 0.5+2.0;
  xfmModifiers[0] = CK_IceBallMissile_spin_XM;
};

// Iceball Glints for static iceball missile
datablock afxModelData(CK_IceBallGlints_Missile_CE : CK_IceBallGlints_CE)
{
  sequenceOffset = ((165/30)-(2.0+3.0)) + (2/30);
};
//
datablock afxEffectWrapperData(CK_IceBallMissile_Glints_EW : CK_IceBallStatic_Missile_EW)
{
  effect = CK_IceBallGlints_Missile_CE;
};

// New versions of the snow particles with shortened lifetimes
//  and existing in local space
datablock ParticleData(CK_SnowA2_L_P : CK_SnowA_L_P)
{
  lifetimeMS   = 800;
  constrainPos = !$UseEmitterTransform;
};
datablock ParticleData(CK_SnowB2_L_P : CK_SnowB_L_P)
{
  lifetimeMS   = 800;
  constrainPos = !$UseEmitterTransform;
};
datablock ParticleData(CK_SnowA2_M_P : CK_SnowA_M_P)
{
  lifetimeMS   = 800;
  constrainPos = !$UseEmitterTransform;
};
datablock ParticleData(CK_SnowB2_M_P : CK_SnowB_M_P)
{
  lifetimeMS   = 800;
  constrainPos = !$UseEmitterTransform;
};
datablock ParticleData(CK_SnowA2_S_P : CK_SnowA_S_P)
{
  lifetimeMS   = 800;
  constrainPos = !$UseEmitterTransform;
};
datablock ParticleData(CK_SnowB2_S_P : CK_SnowB_S_P)
{
  lifetimeMS   = 800;
  constrainPos = !$UseEmitterTransform;
};

// Iceball snow emitter A
//  -- emits particles in local space
datablock afxParticleEmitterConeData(CK_IceBallSnowA_E)
{
  ejectionPeriodMS      = 4;
  periodVarianceMS      = 1;
  ejectionVelocity      = 2.0;
  velocityVariance      = 0.5;  
  particles             = "CK_SnowA2_L_P CK_SnowB2_L_P" SPC
                          "CK_SnowA2_L_P CK_SnowB2_L_P" SPC
                          "CK_SnowA2_M_P CK_SnowB2_M_P" SPC
                          "CK_SnowA2_M_P CK_SnowB2_M_P" SPC
                          "CK_SnowA2_S_P CK_SnowB2_S_P" SPC
                          "CK_SnowA2_S_P CK_SnowB2_S_P";

  vector = "0 -1 0";
  spreadMin = 0.0;
  spreadMax = 120.0;
  useEmitterTransform = $UseEmitterTransform;
  
  blendStyle = "PREMULTALPHA";
};
//
datablock afxEffectWrapperData(CK_IceBallSnowA_EW)
{
  effect = CK_IceBallSnowA_E;
  constraint = missile;
  xfmModifiers[0] = CK_IceBallMissile_spin_XM;
};

// Iceball snow emitter B
datablock ParticleEmitterData(CK_IceBallSnowB_E)
{
  ejectionPeriodMS      = 8;
  periodVarianceMS      = 2;
  ejectionVelocity      = 8.0; 
  velocityVariance      = 2.0; 
  thetaMin              = 0.0;
  thetaMax              = 180.0;  
  particles             = "CK_SnowA_L_P CK_SnowB_L_P" SPC
                          "CK_SnowA_L_P CK_SnowB_L_P" SPC
                          "CK_SnowA_M_P CK_SnowB_M_P" SPC
                          "CK_SnowA_M_P CK_SnowB_M_P" SPC
                          "CK_SnowA_S_P CK_SnowB_S_P" SPC
                          "CK_SnowA_S_P CK_SnowB_S_P";
                          
  blendStyle = "PREMULTALPHA";                          
};
//
datablock afxEffectWrapperData(CK_IceBallSnowB_EW)
{
  effect = CK_IceBallSnowB_E;
  constraint = missile;
  delay = 1.0;
};

// Iceball snow emitter C
datablock afxParticleEmitterDiscData(CK_IceBallSnowC_E)
{
  ejectionPeriodMS      = 6;
  periodVarianceMS      = 2;
  ejectionVelocity      = 0.5; 
  velocityVariance      = 0.5; 
 
  particles             = "CK_SnowA_L_P CK_SnowB_L_P" SPC
                          "CK_SnowA_L_P CK_SnowB_L_P" SPC
                          "CK_SnowA_M_P CK_SnowB_M_P" SPC
                          "CK_SnowA_M_P CK_SnowB_M_P" SPC
                          "CK_SnowA_S_P CK_SnowB_S_P" SPC
                          "CK_SnowA_S_P CK_SnowB_S_P";

  vector = "0 -1 0";
  radiusMin = 0.1;
  radiusMax = 1.0;

  blendStyle = "PREMULTALPHA";
};
//
datablock afxEffectWrapperData(CK_IceBallSnowC_EW)
{
  effect = CK_IceBallSnowC_E;
  constraint = missile;
  delay = 0.5;
};

// Iceball Haze
datablock afxEffectWrapperData(CK_IceBallHaze_EW)
{
  effect = CK_SnowFormationHaze_E;
  constraint = missile;
  delay = 0.3;  
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// ICE IMPACTS

//
// Upon impact of the iceball, shards are sent flying from it, and a
//  giant ice crystal forms.
//
// The ice crystal geometry has two collision meshes, both boxes, but
//  with normals reversed.  In this way it possible for target
//  characters to become trapped inside the ice crystal.
//

// Shattering Ice Shards, animated
datablock afxModelData(CK_IceBallShatter_CE)
{
  shapeFile = %mySpellDataPath @ "/CK/models/ck_iceball_shatter.dts";
  sequence = "shatter";  
};
//
datablock afxEffectWrapperData(CK_IceBallShatter_EW)
{
  effect = CK_IceBallShatter_CE;
  constraint = impactPoint;
  scaleFactor = 0.5+2.0;
  delay = 0;
  lifetime = (15/30);
  fadeInTime = 0;
  fadeOutTime = 0;
};

// IceCrystal model
//  -- animated
//  -- static shape is used so that other players can be trapped within
//      its bounding box
datablock afxStaticShapeData(CK_IceCrystal_CE)
{
  shapeFile = %mySpellDataPath @ "/CK/models/ck_ice_crystal.dts";
  sequence = "icenslice";
  sequenceRate = 2.0;

  emap = true;
  shadowEnable = false;
};

// IceCrystal ground transform
datablock afxXM_GroundConformData(CK_IceCrystal_ground_XM)
{
  height = 0;
  //conformOrientation = true;
};

// IceCrystal
datablock afxEffectWrapperData(CK_IceCrystal_EW)
{
  effect = CK_IceCrystal_CE;
  constraint = impactPoint;
  scaleFactor = 6.0;
  delay = (4/30);
  lifetime = 9.0;
  fadeInTime = 0;
  fadeOutTime = 1.0;
  xfmModifiers[0] = CK_IceCrystal_ground_XM;
};

// IceCrystal snow zodiac
datablock afxZodiacData(CK_IceCrystalSnow_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/CK/zodiacs/ck_ice_crystal_snow";
  radius = 8.0;
  startAngle = 0;
  rotationRate = 0;  
  color = "1.0 1.0 1.0 1.0";
  blend = normal;
};
//
datablock afxEffectWrapperData(CK_IceCrystalSnow_EW)
{
  effect = CK_IceCrystalSnow_CE;
  constraint = impactPoint;
  delay = (4/30);
  lifetime = 9.0;
  fadeInTime = 0.5;
  fadeOutTime = 1.0;
  xfmModifiers[0] = SHARED_freeze_AltitudeConform_XM;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// BLOOD IMPACTS
//

// GUSH particles are each a single blood drop
datablock ParticleData(CK_BloodGush_P)
{
   textureName          = %mySpellDataPath @ "/CK/particles/ck_tiled_snow_blood";
   dragCoeffiecient     = 0.5;
   gravityCoefficient   = 1.0; //0.2;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 800;
   lifetimeVarianceMS   = 200;
   // TGE useInvAlpha          = true;
   //spinRandomMin        = -360.0;
   //spinRandomMax        = 360.0;
   colors[0]            = "1.0 1.0 1.0 1.0";   
   colors[1]            = "1.0 1.0 1.0 0.4";
   sizes[0]             = 0.05;   
   sizes[1]             = 0.15;
   times[0]             = 0.0;   
   times[1]             = 1.0;
   textureCoords[0] = "0.5 0.5";
   textureCoords[1] = "0.5 1.0";
   textureCoords[2] = "1.0 1.0";
   textureCoords[3] = "1.0 0.5";
};

// SPURT particles are a splatter of blood drops
datablock ParticleData(CK_BloodSpurt_P)
{
   textureName          = %mySpellDataPath @ "/CK/particles/ck_tiled_snow_blood";
   dragCoeffiecient     = 0.5;
   gravityCoefficient   = 1.0; //0.2;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 800;
   lifetimeVarianceMS   = 200;
   // TGE useInvAlpha          = true;
   spinRandomMin        = -360.0;
   spinRandomMax        = 360.0;
   colors[0]            = "1.0 1.0 1.0 1.0";   
   colors[1]            = "1.0 1.0 1.0 0.4";
   sizes[0]             = 0.9;   
   sizes[1]             = 0.95;
   times[0]             = 0.0;   
   times[1]             = 1.0;
   textureCoords[0] = "0.0 0.5";
   textureCoords[1] = "0.0 1.0";
   textureCoords[2] = "0.5 1.0";
   textureCoords[3] = "0.5 0.5";
};

//
// SPURTING blood -- short bursts of blood. These will
// occur on a corpse impact.
//

datablock ParticleEmitterData(CK_BloodSpurt_E)
{
  ejectionOffset        = 0.02;
  ejectionPeriodMS      = 10;
  periodVarianceMS      = 3;
  ejectionVelocity      = 5.0;
  velocityVariance      = 1.5;  
  particles             = "CK_BloodSpurt_P CK_BloodGush_P";
  blendStyle            = "NORMAL";
};

// blood spurts from body, then arm, then leg

datablock afxEffectWrapperData(CK_BloodSpurt_Body_EW)
{
  effect = CK_BloodSpurt_E;
  constraint = "impactedObject.Bip01 Spine1";
  delay    = 0.0;
  lifetime = 0.3; 
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
};

datablock afxEffectWrapperData(CK_BloodSpurt_LFArm_EW)
{
  effect = CK_BloodSpurt_E;
  constraint = "impactedObject.Bip01 L Clavicle";
  delay    = 0.5;
  lifetime = 0.3; 
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
};

datablock afxEffectWrapperData(CK_BloodSpurt_RTLeg_EW)
{
  effect = CK_BloodSpurt_E;
  constraint = "impactedObject.Bip01 R Calf";
  delay    = 1.0;
  lifetime = 0.3; 
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    //exec("./lighting/rof_lighting_tge_sub.cs");
    exec("./audio/ck_audio_sub.cs");
  case "TGEA":
    //exec("./lighting/rof_lighting_tgea_sub.cs");
    exec("./audio/ck_audio_sub.cs");
 case "T3D":
    //exec("./lighting/rof_lighting_t3d_sub.cs");
    exec("./audio/ck_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CHILL KILL SPELL
//

datablock afxMagicSpellData(ChillKillSpell)
{
  castingDur = 170/30 - (5/30);

    // magic missile //
  missile = CK_IceBall_Missile;

    // spellcaster animation //
  addCastingEffect = CK_Casting_Clip_EW;
    // casting hand magic //
  addCastingEffect = CK_HandMagic_lfhand_EW;
  addCastingEffect = CK_HandMagic_rthand_EW;
  addCastingEffect = CK_HandSnow_lfhand_EW;
  addCastingEffect = CK_HandSnow_rthand_EW;
    // casting zodiacs //
    //  NOTE: the order these zodiacs are added determines their layer ordering  
  addCastingEffect = CK_CastingZodeMooring_EW;
  addCastingEffect = CK_CastingZode_Snow_EW;
  addCastingEffect = CK_CastingZode_SnowA_EW;
  addCastingEffect = CK_CastingZode_SnowB_EW;
  addCastingEffect = CK_CastingZode_SnowC_EW;
  addCastingEffect = CK_CastingZode_SnowD_EW;
  addCastingEffect = CK_CastingZode_Rings_EW;
  addCastingEffect = CK_CastingZode_OuterRing_EW;
  addCastingEffect = CK_CastingZode_OuterRingB_EW;
  addCastingEffect = CK_CastingZode_InnerRing_EW;
  addCastingEffect = CK_CastingZode_InnerRingB_EW;
  addCastingEffect = CK_CastingZode_Reveal_EW;
    // casting zodiac snow //
  addCastingEffect = CK_CastingZodeSnowA_EW;
  addCastingEffect = CK_CastingZodeSnowB_EW;
    // iceball formation snow //
    //  NOTE: three copies of the snow formation particle effect are added
    //         to create a denser particle volume
  addCastingEffect = CK_SnowFormation_EW;
  addCastingEffect = CK_SnowFormation_EW;
  addCastingEffect = CK_SnowFormation_EW;
  addCastingEffect = CK_SnowFormationHaze_EW;
    // iceball //
  addCastingEffect = CK_IceBall_EW;
  addCastingEffect = CK_IceBallGlints_EW;
  addCastingEffect = CK_IceBall_BurstSnow_EW;
  addCastingEffect = CK_IceBallStatic_EW;
  addCastingEffect = CK_IceBallGlintsStatic_EW;
    // iceball missile //
  addDeliveryEffect = CK_IceBallStatic_Missile_EW;
  addDeliveryEffect = CK_IceBallMissile_Glints_EW;
  addDeliveryEffect = CK_IceBallSnowA_EW;
  addDeliveryEffect = CK_IceBallSnowB_EW;
  addDeliveryEffect = CK_IceBallSnowC_EW;
  addDeliveryEffect = CK_IceBallHaze_EW;
    // ice impacts //
  addImpactEffect = CK_IceBallShatter_EW;
  addImpactEffect = CK_IceCrystal_EW;
  addImpactEffect = CK_IceCrystalSnow_EW;
    // blood impacts //
  addImpactEffect = CK_BloodSpurt_Body_EW;
  addImpactEffect = CK_BloodSpurt_LFArm_EW;
  addImpactEffect = CK_BloodSpurt_RTLeg_EW;
};

// sounds and lights added via sub-script functions //
//CK_add_Lighting_FX(ChillKillSpell);
CK_add_Audio_FX(ChillKillSpell);

datablock afxRPGMagicSpellData(ChillKillSpell_RPG)
{
  spellName = "Chill Kill";
  desc = "Add snow, ice, gravel, and subzero temps " @
         "proportioned to taste. Stir and throw, with authority!\n" @
         "<font:Arial:6>\n" @
         "<just:center><font:Arial:14>Also good for chilling large flagons of mead.<just:left>\n" @
         "\n" @
         "<font:Arial Italic:14>spell design and concept: <font:Arial:14>Matthew Durante\n" @
         "<font:Arial Italic:14>sound design: <font:Arial:14>Matt Pacyga";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/CK/icons/ck";
  target = "enemy";
  range = 80;
  manaCost = 10;
  directDamage = 50.0;

  castingDur = ChillKillSpell.castingDur;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxStaticShapeData(CK_IceCrystal_Static_CE : CK_IceCrystal_CE)
{
  shapeFile = %mySpellDataPath @ "/CK/models/ck_ice_crystal_static.dts";
};

datablock afxEffectWrapperData(CK_IceShards_EW)
{
  effect = CK_IceCrystal_CE;
  constraint = anchor;
  scaleFactor = 6.0;
  fadeInTime  = 2.0;
  fadeOutTime = 2.0;
};

datablock afxXM_WorldOffsetData(CK_IceCrystal_offset_XM)
{
  worldOffset = "0 6 0";
};
datablock afxEffectWrapperData(CK_IceShards2_EW)
{
  effect = CK_IceCrystal_Static_CE;
  constraint = anchor;
  scaleFactor = 6.0;
  fadeInTime  = 2.0;
  fadeOutTime = 2.0;
  xfmModifiers[0] = CK_IceCrystal_offset_XM;
};

datablock afxEffectronData(IceShardsProp)
{
  //duration = $AFX::INFINITE_TIME;
  duration = 60;
  execOnNewClients = true;
  addEffect = CK_IceShards_EW;
  addEffect = CK_IceShards2_EW;
};

function IceShardsProp::spawn()
{
  if (!isObject(IceShardsProp.one_and_only))
    IceShardsProp.one_and_only = startEffectron(IceShardsProp, "366.173 309.959 217.8", "anchor");
}

function IceShardsProp::kill()
{
  if (isObject(IceShardsProp.one_and_only))
    IceShardsProp.one_and_only.interrupt();
}

function IceShardsProp::onDeactivate(%this, %prop)
{
  IceShardsProp.one_and_only = "";
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// ICE SHARDS PROP SPELL
//    a simple spell that allows placing of ice shard props
//    using a free-targeting selectron.
//

datablock afxMagicSpellData(IceShardsPropSpell)
{
  castingDur = 0;
};

datablock afxRPGMagicSpellData(IceShardsPropSpell_RPG)
{
  spellName = "Ice Shards Prop";
  desc = "Places a static cluster ice shards.\n" @
         "\n" @
         "[reusable element]";
  sourcePack = "Spell Pack 2";
  iconBitmap = %mySpellDataPath @ "/CK/icons/ck";
  manaCost = 10;
  castingDur = 0;
  target = "free";
};

function IceShardsPropSpell::onLaunch(%this, %spell, %caster, %target, %missile)
{
  Parent::onLaunch(%this, %spell, %caster, %target, %missile);

  // creates an anchor transform using target position + caster orientation
  %anchor = %spell.freeTarget SPC getWords(%caster.getTransform(), 3);

  startEffectron(IceShardsProp, %anchor, "anchor");
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
  ChillKillSpell.scriptFile = $afxAutoloadScriptFile;
  ChillKillSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
  {
    addDemoSpellbookSpell(ChillKillSpell, ChillKillSpell_RPG);
    addDemoSpellbookSpell(IceShardsPropSpell, IceShardsPropSpell_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//