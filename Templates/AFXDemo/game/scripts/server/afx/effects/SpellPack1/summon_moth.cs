
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SUMMON FECKLESS MOTH SPELL
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

//UAISK+AFX Interop Changes: Start
if ($UAISK_Is_Available)
  exec("./summon_moth_sub.cs");
//UAISK+AFX Interop Changes: End

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

//
// Here we test if the script is being reloaded or if this is the
// first time the script has executed this mission.
//
$spell_reload = isObject(SummonFecklessMothSpell);
if ($spell_reload)
{
  GiantMothProp::kill();
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = SummonFecklessMothSpell.spellDataPath;
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
// The spellcaster begins with a short bow and strikes a pose
// of extreme concentration (or he finds the caterpillar crawl
// quite ticklish). Eventually, the summoning explosion throws
// him on his back just as the giant moth appears.
//

datablock afxAnimClipData(SFM_Casting_Clip_CE)
{
  clipName = "sfm";
  ignoreCorpse = true;
  rate = 1.0;
};
//
datablock afxEffectWrapperData(SFM_Casting_Clip_EW)
{
  effect = SFM_Casting_Clip_CE;
  constraint = caster;
  lifetime = 14.7;
  delay = 0.0;
};

datablock afxAnimLockData(SFM_AnimLock_CE)
{
  priority = 0;
};
//
datablock afxEffectWrapperData(SFM_AnimLock_EW)
{
  effect = SFM_AnimLock_CE;
  constraint = caster;
  lifetime = 5.0;
  delay = 9.7;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING ZODIAC

// this mooring anchors the casting zodiacs and measures
// their altitudes

datablock afxMooringData(SFM_CastingZodeMooring_CE)
{
  networking = $AFX::CLIENT_ONLY;
  displayAxisMarker = false;
};
datablock afxEffectWrapperData(SFM_CastingZodeMooring_EW)
{
  effect = SFM_CastingZodeMooring_CE;
  posConstraint = caster;
  effectName = "CastingZodeMooring";
  isConstraintSrc = true;
  lifetime = 13.0;
  xfmModifiers[0] = SHARED_freeze_AltitudeConform_XM;
};

//
// The main casting zodiac is formed by three zodiacs plus a
// white glow zodiac used to reveal the zodiac when the casting
// first starts. 
//
// The primary zodiac features nature colors and imagery and is
// combined with a secondary green zodiac with runes and skulls,
// These zodiacs use additive blending to suggest projected light.
//
// The center is filled with a cloudy portal zodiac at the early
// stages of the casting as it represents a magical doorway from
// which the caterpillar emerges.
//

// reveal zodiac
datablock afxZodiacData(SFM_ZodeReveal_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/SFM/zodiacs/SFM_caster_reveal";
  radius = 3.0;
  startAngle = 7.5; //0.0+7.5
  rotationRate = -30.0;  
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(SFM_ZodeReveal_EW)
{
  effect = SFM_ZodeReveal_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = 0.0; //0.01;
  lifetime = 0.75;
  fadeInTime = 0.5;
  fadeOutTime = 0.25;
  xfmModifiers[0] = SHARED_freeze_XM;
};

// main nature zodiac
datablock afxZodiacData(SFM_Zode1_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/SFM/zodiacs/SFM_caster";
  radius = 3.0;
  startAngle = 0.0;
  rotationRate = -30.0;
  color = "1.0 1.0 1.0 0.9";
  blend = additive;
};
//
datablock afxEffectWrapperData(SFM_Zode1_EW)
{
  effect = SFM_Zode1_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = 0.25;
  lifetime = 11.0;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  xfmModifiers[0] = SHARED_freeze_XM;
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
datablock afxZodiacData(SFM_Zode1_underglow_CE : SFM_Zode1_CE)
{
  color = "0.7 0.7 0.7 0.7";
  blend = normal;
};
//
datablock afxEffectWrapperData(SFM_Zode1_underglow_EW : SFM_Zode1_EW)
{
  effect = SFM_Zode1_underglow_CE;
  execConditions = $BrightLighting_mask;
};

// runes and skulls
datablock afxZodiacData(SFM_Zode2_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/SFM/zodiacs/zode_text";
  radius = 3.0;
  startAngle = 0.0;
  rotationRate = 20.0; //60
  color = "0.0 1.0 0.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(SFM_Zode2_EW)
{
  effect = SFM_Zode2_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = 0.25;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 11.0;
  xfmModifiers[0] = SHARED_freeze_XM;
};

// Runes & Skulls Zode Underglow
//  Here the zode is made black but only slightly opaque to subtly
//  darken the ground, making the additive glow zode appear more
//  saturated.
datablock afxZodiacData(SFM_Zode2_underglow_CE : SFM_Zode2_CE)
{
  color = "0 0 0 0.15";
  blend = normal;
};
//
datablock afxEffectWrapperData(SFM_Zode2_underglow_EW : SFM_Zode2_EW)
{
  effect = SFM_Zode2_underglow_CE;
  execConditions = $BrightLighting_mask;
};

// portal zodiac (caterpillar gate)
datablock afxZodiacData(SFM_ZodePortal_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/SFM/zodiacs/ALL_portal";
  radius = 0.80;
  startAngle = 0.0;
  rotationRate = 240.0;
  color = "1.0 1.0 1.0 1.0";
  growInTime = 1.0;
};
//
datablock afxEffectWrapperData(SFM_ZodePortal_EW)
{
  effect = SFM_ZodePortal_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = 0.5; //0.75;
  lifetime = 3.0;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  xfmModifiers[0] = SHARED_freeze_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GIANT CATERPILLAR

//
// A giant caterpillar emerges from a portal in the ground and
// crawls up the spellcaster's body to a point over his head.
//
// The caterpiller is built witha three piece model -- head,
// segment, and tail. The body is formed by repeating the
// segment a number of times. The caterpiller animates by
// following a long path that snakes around the spellcaster's
// body. 
//

datablock afxPathData(SFM_Caterpillar_Path)
{
  points = "0.04400121966 0 0.002611434273 " @
           "-0.1785129427 0.4854599403 0.2496931696 " @
           "-0.6449708483 0.3080040441 0.1718326439 " @
           "-0.584534702 -0.1697961229 0.08117842448 " @
           "-0.1191763754 -0.1151614031 0.01469866354 " @
           "0.3884872536 0.1714152299 0.250681406 " @
           "0.5721833461 -0.2031300417 0.3591846975 " @
           "0.327619713 -0.3186805507 0.5247929833 " @
           "0.1675691421 -0.06898134045 0.7569980428 " @
           "0.3461700888 -0.2201275967 1.067687261 " @
           "0.02587037576 -0.3535790675 1.329282996 " @
           "-0.4092698777 -0.2571728316 1.139521931 " @
           "-0.3936801662 0.2249023391 1.305598009 " @
           "0.001695917239 0.307402598 1.347756576 " @
           "0.2548000837 0.2552687467 1.399452839 " @
           "0.3182369883 -0.3300390409 1.49813798 " @
           "-0.08289881665 -0.3967959407 1.66202125 " @
           "-0.2793554499 -0.3837231529 1.990445094 " @
           "0.1579877861 -0.2583618389 2.245296097 " @
           "0.1709171269 0.0686730673 2.347533911 " @
           "-0.0647838437 -0.1395602345 2.618777644 " @
		       "0 0 3.75";

  roll = "0 -45 -90 -90 -70 " @
         "0 90 90 -45 -180 " @ 
         "-360 -290 -270 -270 -270 " @
         "-270 -270 -360 -360 -360 " @
         "-390 -360";

  //lifetime = 16.0;
  //delay = 0.5;
};

//
// caterpillar body parts -- head, repeated segment, tail
//

datablock afxModelData(SFM_CaterpillarHead_CE)
{
  shapeFile = %mySpellDataPath @ "/SFM/models/SFM_caterpillar_head.dts";
};

datablock afxModelData(SFM_CaterpillarSegment_CE)
{
  shapeFile = %mySpellDataPath @ "/SFM/models/SFM_caterpillar_segment.dts";

  //forceOffMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  forceOffMaterialFlags = $MaterialFlags::Translucent;
};

datablock afxModelData(SFM_CaterpillarTail_CE)
{
  shapeFile = %mySpellDataPath @ "/SFM/models/SFM_caterpillar_tail.dts";
};

// this modifier conforms the caterpillar parts to a path
datablock afxXM_PathConformData(SFM_Caterpillar_path_XM)
{
  paths = "SFM_Caterpillar_Path";
  orientToPath = true;
};
//
datablock afxEffectWrapperData(SFM_CaterpillarHead_EW)
{
  effect = SFM_CaterpillarHead_CE;
  constraint = caster;
  delay = 1.5;
  fadeInTime  = 0.4;
  fadeOutTime = 1.0;
  lifetime = 7.0;
  scaleFactor = 0.75; //0.70;
  xfmModifiers[0] = SFM_Caterpillar_path_XM;
};

datablock afxEffectWrapperData(SFM_CaterpillarSegment1_EW)
{
  effect = SFM_CaterpillarSegment_CE;
  constraint = caster;
  delay = 1.60; //1.5+0.10    //0.08
  fadeInTime  = 0.4;
  fadeOutTime = 1.0;
  lifetime = 7.0;
  scaleFactor = 0.75;
  xfmModifiers[0] = SFM_Caterpillar_path_XM;
};

datablock afxEffectWrapperData(SFM_CaterpillarSegment2_EW)
{
  effect = SFM_CaterpillarSegment_CE;
  constraint = caster;
  delay = 1.69; //1.60+0.09
  fadeInTime  = 0.4;
  fadeOutTime = 1.0;
  lifetime = 7.0;
  scaleFactor = 0.90; //1.0
  xfmModifiers[0] = SFM_Caterpillar_path_XM;
};

datablock afxEffectWrapperData(SFM_CaterpillarSegment3_EW)
{
  effect = SFM_CaterpillarSegment_CE;
  constraint = caster;
  delay = 1.80; //1.69+0.11
  fadeInTime  = 0.4;
  fadeOutTime = 1.0;
  lifetime = 7.0;
  scaleFactor = 1.00; //1.15;
  xfmModifiers[0] = SFM_Caterpillar_path_XM;
};

datablock afxEffectWrapperData(SFM_CaterpillarSegment4_EW)
{
  effect = SFM_CaterpillarSegment_CE;
  constraint = caster;
  delay = 1.93; //1.80+0.13
  fadeInTime  = 0.4;
  fadeOutTime = 1.0;
  lifetime = 7.0;
  scaleFactor = 1.10; //1.2;
  xfmModifiers[0] = SFM_Caterpillar_path_XM;
};

datablock afxEffectWrapperData(SFM_CaterpillarSegment5_EW)
{
  effect = SFM_CaterpillarSegment_CE;
  constraint = caster;
  delay = 2.07; //1.93+0.14
  fadeInTime  = 0.4;
  fadeOutTime = 1.0;
  lifetime = 7.0;
  scaleFactor = 1.15; //1.25;
  xfmModifiers[0] = SFM_Caterpillar_path_XM;
};

datablock afxEffectWrapperData(SFM_CaterpillarSegment6_EW)
{
  effect = SFM_CaterpillarSegment_CE;
  constraint = caster;
  delay = 2.21; //2.07+0.14
  fadeInTime  = 0.4;
  fadeOutTime = 1.0;
  lifetime = 7.0;
  scaleFactor = 1.15; //1.25;
  xfmModifiers[0] = SFM_Caterpillar_path_XM;
};

datablock afxEffectWrapperData(SFM_CaterpillarSegment7_EW)
{
  effect = SFM_CaterpillarSegment_CE;
  constraint = caster;
  delay = 2.34; //2.21+0.13
  fadeInTime  = 0.4;
  fadeOutTime = 1.0;
  lifetime = 7.0;
  scaleFactor = 1.10; //1.2;
  xfmModifiers[0] = SFM_Caterpillar_path_XM;
};

datablock afxEffectWrapperData(SFM_CaterpillarSegment8_EW)
{
  effect = SFM_CaterpillarSegment_CE;
  constraint = caster;
  delay = 2.45; //2.34+0.11
  fadeInTime  = 0.4;
  fadeOutTime = 1.0;
  lifetime = 7.0;
  scaleFactor = 1.00; //1.15;
  xfmModifiers[0] = SFM_Caterpillar_path_XM;
};

datablock afxEffectWrapperData(SFM_CaterpillarSegment9_EW)
{
  effect = SFM_CaterpillarSegment_CE;
  constraint = caster;
  delay = 2.54; //2.45+0.09
  fadeInTime  = 0.4;
  fadeOutTime = 1.0;
  lifetime = 7.0;
  scaleFactor = 0.90; //1.0
  xfmModifiers[0] = SFM_Caterpillar_path_XM;
};

datablock afxEffectWrapperData(SFM_CaterpillarSegment10_EW)
{
  effect = SFM_CaterpillarTail_CE;
  constraint = caster;
  delay = 2.62; //2.54+0.08
  fadeInTime  = 0.4;
  fadeOutTime = 1.0;
  lifetime = 7.0;
  scaleFactor = 0.75;
  xfmModifiers[0] = SFM_Caterpillar_path_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// COCOON

//
// A special point is established above the spellcaster's head
// by a pulsing pink glow. This is the point of attraction for
// the caterpillar and the swarming moths. A giant cocoon 
// eventually appears at this location.
// 

// this offset establishes the cocoon location, the central
// attraction point for this spell. It's used by a number of
// effects.
datablock afxXM_WorldOffsetData(SFM_Cocoon_offset_XM)
{
  worldOffset = "0 0 3.75";
};

// the cocoon
datablock afxModelData(SFM_Cocoon_CE)
{
  shapeFile = %mySpellDataPath @ "/SFM/models/SFM_cocoon.dts";
  //forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  forceOnMaterialFlags = $MaterialFlags::SelfIlluminating;
  sequence = "scaleUp";
};
//
datablock afxEffectWrapperData(SFM_Cocoon_EW)
{
  effect = SFM_Cocoon_CE;
  posConstraint = caster;
  delay = 3.0;
  fadeInTime  = 5.0; //6.0;
  fadeOutTime = 1.0;
  lifetime = 8.0;
  xfmModifiers[0] = SFM_Cocoon_offset_XM;
};

// a pulsing pink larva glow
datablock afxModelData(SFM_LarvaPulse_CE)
{
   shapeFile = %mySpellDataPath @ "/SFM/models/SFM_larva.dts";
   alphaMult = 0.45;
   sequence = "pulsate";
   useVertexAlpha = true;
   forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
};
//
datablock afxEffectWrapperData(SFM_LarvaPulse_EW)
{
  effect = SFM_LarvaPulse_CE;
  posConstraint = caster;
  delay = 1.0;
  fadeInTime  = 7.0;
  fadeOutTime = 0.5; //1.0;
  lifetime = 10.5;
  xfmModifiers[0] = SFM_Cocoon_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// MOTH SWARM

//
// Wave after wave of moths swarm into the pink glow. The swarms
// build up until the caterpillar also arrives and the magic
// explodes to produce the moth.
//

datablock afxModelData(SFM_MothSwarmA_CE)
{
  shapeFile = %mySpellDataPath @ "/SFM/models/SFM_mothSwarm_newA.dts";
  sequence = "swarm";
  //forceOnMaterialFlags = $MaterialFlags::Additive;
};
//
datablock afxEffectWrapperData(SFM_MothSwarm1_EW)
{
  effect = SFM_MothSwarmA_CE;
  posConstraint = caster;
  delay = 2.5; //3.0;
  lifetime = 2.95;  //3.0
  xfmModifiers[0] = SFM_Cocoon_offset_XM;
};

datablock afxModelData(SFM_MothSwarmB_CE)
{
  shapeFile = %mySpellDataPath @ "/SFM/models/SFM_mothSwarm_newB.dts";
  //forceOnMaterialFlags = $MaterialFlags::Additive;
  sequence = "swarm";
};
//
datablock afxEffectWrapperData(SFM_MothSwarm2_EW)
{
  effect = SFM_MothSwarmB_CE;
  posConstraint = caster;
  delay = 4.5; //5.0; //5.8;
  lifetime = 2.60; //2.67
  xfmModifiers[0] = SFM_Cocoon_offset_XM;
};

datablock afxModelData(SFM_MothSwarmC_CE)
{
  shapeFile = %mySpellDataPath @ "/SFM/models/SFM_mothSwarm_newC.dts";
  sequence = "swarm";
  //forceOnMaterialFlags = $MaterialFlags::Additive;
};
//
datablock afxEffectWrapperData(SFM_MothSwarm3_EW)
{
  effect = SFM_MothSwarmC_CE;
  posConstraint = caster;
  delay = 5.5; //6.0; //8.0;
  lifetime = 2.25; //2.33
  xfmModifiers[0] = SFM_Cocoon_offset_XM;
};

datablock afxModelData(SFM_MothSwarmD_CE)
{
  shapeFile = %mySpellDataPath @ "/SFM/models/SFM_mothSwarm_newD.dts";
  sequence = "swarm";
  //forceOnMaterialFlags = $MaterialFlags::Additive;
};
//
datablock afxEffectWrapperData(SFM_MothSwarm4_EW)
{
  effect = SFM_MothSwarmD_CE;
  posConstraint = caster;
  delay = 6.5; //7.0; //10.0;
  lifetime = 1.95; //2.0
  xfmModifiers[0] = SFM_Cocoon_offset_XM;
};

datablock afxModelData(SFM_MothSwarmE_CE)
{
  shapeFile = %mySpellDataPath @ "/SFM/models/SFM_mothSwarm_newE.dts";
  sequence = "swarm";
  //forceOnMaterialFlags = $MaterialFlags::Additive;
};
//
datablock afxEffectWrapperData(SFM_MothSwarm5_EW)
{
  effect = SFM_MothSwarmE_CE;
  posConstraint = caster;
  delay = 7.5; //8.0; //11.2;
  lifetime = 1.60; //1.67
  xfmModifiers[0] = SFM_Cocoon_offset_XM;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SUMMONING EXPLOSION

//
// Energy builds up around the pulsing glow and the cocoon. 
// Glowing cracks form on the top and bottom of the cocoon
// and it all explodes in burst of magic.
//

// these paths form a group of cracks on top 
// of the cocoon
datablock afxPathData(SFM_ExplosionPath_Top1)
{
  points = "-0.0210025065 -0.0001894962961 1.020201356 " @
           " 0.04195162114 0.03093824879 0.9881282809 " @
           " 0.06800323146 0.07873524857 0.9413694484 " @
           " 0.0749999517 0.122119117 0.8875394006 " @
           " 0.06622530304 0.1798887163 0.8426712134 " @
           " 0.07632728178 0.2159861288 0.8106526524 " @
           " 0.127523389 0.26845101 0.7739869211 " @
           " 0.1593798986 0.3103696834 0.6991801007 " @
           " 0.1682844463 0.3352296477 0.5960051323 " @
           " 0.2558547329 0.3048129884 0.5608739158 " @
           " 0.2635477226 0.3117933631 0.5458752777 " @
           " 0.292375821 0.3974142328 0.4234813333 " @
           " 0.3155048211 0.3980789464 0.3713372193 " @
           " 0.3174868935 0.403539695 0.3542338362 " @
           " 0.288532405 0.4255195386 0.3050805818 " @
           " 0.2986628293 0.4408478872 0.2825870176 " @
           " 0.2856630327 0.460736629 0.2253413103 " @
           " 0.2511056692 0.4825468766 0.2021554474 ";
};
datablock afxPathData(SFM_ExplosionPath_Top2)
{
  points = "-0.007250445217 0.0002212822367 1.016990349 " @
           "0.005425819932 0.02044207212 1.004272601 " @
           "-0.0194116626 0.04520268045 0.9925975361 " @
           "-0.04757240578 0.09818536212 0.9566318307 " @
           "-0.06712670457 0.1615397116 0.9092120866 " @
           "-0.07178140945 0.1716840747 0.902065775 " @
           "-0.1241378702 0.1888516331 0.8627844521 " @
           "-0.1541806371 0.2252338775 0.8089472731 " @
           "-0.1676525139 0.2612447377 0.7633126144 " @
           "-0.1861119654 0.2671392727 0.7388711943 " @
           "-0.2393231025 0.2883735366 0.6579619821 " @
           "-0.2376569141 0.2995492056 0.6455951117 " @
           "-0.240127371 0.3576101686 0.553856916 " @
           "-0.2440347657 0.4048264851 0.4543887224 " @
           "-0.2503737578 0.4250806611 0.4140737638 " @
           "-0.2595459619 0.4318144007 0.3832126552 " @
           "-0.3062992085 0.4354013795 0.2852844704 " @
           "-0.3172830848 0.4010207938 0.2009135871 ";
};
datablock afxPathData(SFM_ExplosionPath_Top3)
{
  points = "-0.02196109949 0.0003130643377 1.019809567 " @
           "-0.08629526188 0.03391410114 0.9813056828 " @
           "-0.1624226913 0.03400227687 0.9832106755 " @
           "-0.2175972572 0.02086623102 0.8678250855 " @
           "-0.2420110838 0.003789901807 0.8544625003 " @
           "-0.3014247795 -0.06030579137 0.7931743056 " @
           "-0.3108113532 -0.07682287799 0.7552956354 " @
           "-0.4087829158 -0.08779178918 0.6113013698 " @
           "-0.5370877109 -0.07481451968 0.4205772973 " @
           "-0.5786269418 -0.07267801534 0.3192005081 " @
           "-0.6065575485 -0.07986064238 0.1732434686 ";
};
datablock afxPathData(SFM_ExplosionPath_Top4)
{
  points = "-0.01566207471 0.00526515465 1.018940751 " @
           "-0.1012565401 -0.05030326289 0.9825894154 " @
           "-0.1185344079 -0.07280964559 0.9773881834 " @
           "-0.1529239676 -0.1507244343 0.8729285666 " @
           "-0.1819316578 -0.2398543199 0.7400076192 " @
           "-0.235234826 -0.2903755759 0.637150084 " @
           "-0.2885885538 -0.3100577139 0.5690419311 " @
           "-0.2922596678 -0.320003548 0.5520003865 " @
           "-0.2963231449 -0.3831062081 0.4714068305 " @
           "-0.28546988 -0.4301994006 0.4276079269 " @
           "-0.2846808163 -0.4360847413 0.4099985223 " @
           "-0.3130823944 -0.4388176088 0.2737232969 ";
};
datablock afxPathData(SFM_ExplosionPath_Top5)
{
  points = "-0.004635959449 -0.0003404037825 1.015809818 " @
           "0.003319684734 -0.02468865656 0.9993594003 " @
           "-0.00833963682 -0.05295829294 0.9806045062 " @
           "-0.01626868979 -0.1045484827 0.954907809 " @
           "0.0110571267 -0.1604241703 0.9047077746 " @
           "0.0371945185 -0.2045732517 0.8471495622 " @
           "0.04037297057 -0.2736150457 0.7865135239 " @
           "-0.0007578932155 -0.3337441442 0.6778463893 " @
           "-0.008996564928 -0.3617226919 0.6404062363 " @
           "-0.01312314526 -0.4129863081 0.5781992556 " @
           "-0.001363477865 -0.4337817599 0.559720888 " @
           "-0.007726771966 -0.4453336572 0.5427794257 " @
           "-0.05351290446 -0.4963574719 0.455416963 " @
           "-0.06985651179 -0.5183683282 0.4129529614 " @
           "-0.07106144607 -0.543649366 0.3353514439 " @
           "-0.07262861767 -0.5607858094 0.2823880907 " @
           "-0.05771474693 -0.5581815668 0.2645686097 " @
           "-0.04698672022 -0.5602411367 0.2113291702 ";
};
datablock afxPathData(SFM_ExplosionPath_Top6)
{
  points = "-0.002606088207 0.002076667439 1.015028107 " @
           "0.06182872722 -0.0518594915 0.9697145314 " @
           "0.09362062682 -0.101279177 0.9068725695 " @
           "0.1397014957 -0.160767538 0.8361099662 " @
           "0.1597754919 -0.1813349564 0.8212019115 " @
           "0.1823171208 -0.1875713928 0.8157756023 " @
           "0.2546723497 -0.2003586329 0.7393779598 " @
           "0.3132566785 -0.2109541682 0.6517221231 " @
           "0.3487838536 -0.2329181222 0.5418724616 " @
           "0.3786929436 -0.2466404412 0.4603752665 " @
           "0.3907843277 -0.2640537939 0.4430613665 " @
           "0.3993012793 -0.2924509819 0.4252997538 " @
           "0.4049686606 -0.2947434566 0.4114167733 " @
           "0.3939402234 -0.3177453315 0.360541613 " @
           "0.3932675331 -0.3252357276 0.3389782411 " @
           "0.4098610956 -0.3209978923 0.3234241523 " @
           "0.4501263376 -0.3142084257 0.245068162 ";
};
datablock afxPathData(SFM_ExplosionPath_Top7)
{
  points = "-0.0009578091729 0.007681451162 1.013480626 " @
           "0.03652648079 0.01961572621 0.9954493949 " @
           "0.0760367508 0.0128616318 0.9799018978 " @
           "0.09640790041 0.004630383081 0.9715663757 " @
           "0.1171535645 0.02082083449 0.9603739478 " @
           "0.144558548 0.03550792467 0.9230320403 " @
           "0.2042503071 0.07384318885 0.8672796016 " @
           "0.2409259355 0.09271480686 0.8292761958 " @
           "0.3130602232 0.08376869336 0.7746500135 " @
           "0.3541451381 0.09661372968 0.6778237082 " @
           "0.3616009777 0.1073976051 0.6659320854 " @
           "0.3897057031 0.1516119073 0.6060455471 " @
           "0.3982326878 0.1546918061 0.5843923832 " @
           "0.4382793466 0.135751722 0.4664610408 " @
           "0.4524629965 0.1271616237 0.4214095217 " @
           "0.4997076697 0.1379798652 0.3166416179 " @
           "0.5325555742 0.1456506743 0.2448222543 ";
};

// these paths form a group of cracks on the bottom 
// of the cocoon
datablock afxPathData(SFM_ExplosionPath_Bot1)
{
  points = "-0.004096532394 -0.0187888988 -1.087276698 " @
           "0.02963152398 -0.159058925 -0.9925287607 " @
           "0.1278532939 -0.2136218339 -0.8690234423 " @
           "0.1785593897 -0.2429322006 -0.782616466 " @
           "0.2150397348 -0.2832316217 -0.6862999405 " @
           "0.1965804992 -0.3358849602 -0.61162034 " @
           "0.2388675691 -0.3596992445 -0.4989644628 " @
           "0.3393118102 -0.3166591096 -0.3941163828 " @
           "0.4428932047 -0.2028016811 -0.3058348685 " @
           "0.488291067 -0.1241404723 -0.2346628243 " @
           "0.5028670077 -0.1154229432 -0.1204119377 " @
           "0.4523908407 -0.2164439896 -0.08738871697 " @
           "0.3375410153 -0.3682226477 -0.08897174545 ";
};
datablock afxPathData(SFM_ExplosionPath_Bot2)
{
  points = "0.007056853295 -0.01343400474 -1.085414671 " @
           "0.08963253487 -0.008813593312 -1.045413791 " @
           "0.1276385632 0.02753395135 -1.004251336 " @
           "0.1540497298 0.07755549982 -0.953414244 " @
           "0.2062825006 0.125114932 -0.8566974951 " @
           "0.2621205768 0.1198210697 -0.7855888536 " @
           "0.3275936227 0.03715425258 -0.7200902115 " @
           "0.3524442444 -0.07003060906 -0.6664585554 " @
           "0.3541307832 -0.1357968968 -0.6255117683 " @
           "0.3883744772 -0.1553151757 -0.5284630417 " @
           "0.4427655405 -0.08500115079 -0.429077766 " @
           "0.4767039684 0.005222827311 -0.3446411975 " @
           "0.4673730233 0.1486367014 -0.2839641191 ";
};
datablock afxPathData(SFM_ExplosionPath_Bot3)
{
  points = "0.00767552515 0.03489470961 -1.075041952 " @
           "0.02913045035 0.1109905258 -1.016430927 " @
           "0.01762909872 0.1854666064 -0.9335235266 " @
           "0.01751748393 0.2553525722 -0.8338759523 " @
           "0.07038972085 0.3220089208 -0.7151667253 " @
           "0.1735248736 0.3338716402 -0.6205329177 " @
           "0.270358367 0.304731163 -0.5461439124 " @
           "0.3325814139 0.2871766608 -0.4590637601 " @
           "0.3560819114 0.3195081533 -0.3350791995 " @
           "0.2843555637 0.3983906191 -0.2872203697 " @
           "0.2003561975 0.4674773935 -0.2046534339 " @
           "0.1243667681 0.4600344518 -0.08799046253 ";
};
datablock afxPathData(SFM_ExplosionPath_Bot4)
{
  points = "-0.05844360411 0.007340403728 -1.073217076 " @
           "-0.1051419744 0.0392187401 -1.033339985 " @
           "-0.1247942109 0.1096534192 -0.9702801051 " @
           "-0.1774348175 0.1328886984 -0.8979052028 " @
           "-0.2652536513 0.1200897967 -0.795148167 " @
           "-0.3403241164 0.1105309139 -0.678364524 " @
           "-0.3890906852 0.1287667042 -0.5556208773 " @
           "-0.3885763075 0.1754980085 -0.5074459405 " @
           "-0.3524716366 0.2499362344 -0.4895876835 " @
           "-0.2345301992 0.3493692509 -0.5160166361 ";
};
datablock afxPathData(SFM_ExplosionPath_Bot5)
{
  points = "-0.03700647556 -0.01813851869 -1.082686995 " @
           "-0.1179792601 -0.02975373597 -1.032629014 " @
           "-0.1652759827 -0.007040730385 -0.9835593654 " @
           "-0.2293318519 0.01257575582 -0.896738179 " @
           "-0.2942184563 -0.03918403569 -0.7944819713 " @
           "-0.3284184668 -0.08606331633 -0.7213443587 " @
           "-0.3687835212 -0.1040058662 -0.6290575823 " @
           "-0.4061352478 -0.06304152626 -0.5594190564 " @
           "-0.4327914159 0.02388576016 -0.492412988 " @
           "-0.4588718234 0.05344789172 -0.4042884553 " @
           "-0.4707641533 0.1178983749 -0.3210052873 " @
           "-0.4962616573 0.09013401819 -0.2341664285 ";
};
datablock afxPathData(SFM_ExplosionPath_Bot6)
{
  points = "-0.02322363444 -0.03433341913 -1.083508092 " @
           "-0.06474781052 -0.1219213229 -1.022545506 " @
           "-0.1059433777 -0.1715855239 -0.9472000713 " @
           "-0.1817146598 -0.1996196922 -0.8442204021 " @
           "-0.2410412177 -0.2034772793 -0.7670581291 " @
           "-0.2777107907 -0.2270505093 -0.6884025317 " @
           "-0.2812398702 -0.2810619479 -0.5988824547 " @
           "-0.2461513606 -0.3516643724 -0.5139595849 " @
           "-0.2358820755 -0.3898532666 -0.4313883918 " @
           "-0.2610327609 -0.4087913013 -0.3301340066 " @
           "-0.2975335258 -0.4135636875 -0.2193738695 " @
           "-0.2801766192 -0.4216563326 -0.124070514 " @
           "-0.195996921 -0.4779610617 -0.05765178893 " @
           "-0.06403610651 -0.5087481799 -0.01465788615 ";
};

// these particles form along the top and bottom cracks of
// the cocoon.
datablock ParticleData(SFM_ExplosionMagic_P)
{
   textureName          = %mySpellDataPath @ "/SFM/particles/SFM_explosionMagic";
   dragCoeffiecient     = 0.0;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 1000;
   lifetimeVarianceMS   = 0;
   useInvAlpha          = false;
   colors[0]            = "0.4 0.4 0.4 0.4";
   colors[1]            = "0.2 0.2 0.2 0.2";
   colors[2]            = "0.0 0.0 0.0 0.0";
   sizes[0]             = 0.5; //0.4;
   sizes[1]             = 0.8; //0.6;
   sizes[2]             = 1.1; //0.8;
   times[0]             = 0.0;
   times[1]             = 0.3;
   times[2]             = 1.0;
};

// the top emitter
datablock afxParticleEmitterPathData(SFM_ExplosionMagic_Top_E) // TGEA
{
  ejectionPeriodMS = 3;
  periodVarianceMS = 1;
  ejectionVelocity = 0.4;
  velocityVariance = 0.0;
  particles        = SFM_ExplosionMagic_P;

  // TGE emitterType = "path";
  pathOrigin  = "origin"; // origin point vector tangent
  paths = "SFM_ExplosionPath_Top1 SFM_ExplosionPath_Top2 " @
          "SFM_ExplosionPath_Top3 SFM_ExplosionPath_Top4 " @
		      "SFM_ExplosionPath_Top5 SFM_ExplosionPath_Top6 " @
		      "SFM_ExplosionPath_Top7";
  fadeSize = true;
};

// the bottom emitter
datablock afxParticleEmitterPathData(SFM_ExplosionMagic_Bot_E : SFM_ExplosionMagic_Top_E) // TGEA
{
  paths = "SFM_ExplosionPath_Bot1 SFM_ExplosionPath_Bot2 " @
          "SFM_ExplosionPath_Bot3 SFM_ExplosionPath_Bot4 " @
		      "SFM_ExplosionPath_Bot5 SFM_ExplosionPath_Bot6";
};

datablock afxEffectWrapperData(SFM_ExplosionMagic_Top_EW)
{
  effect = SFM_ExplosionMagic_Top_E;
  posConstraint = caster;
  lifetime    = 1.5; //2.5;
  delay       = 9.3;
  fadeInTime  = 3.0;
  xfmModifiers[0] = SFM_Cocoon_offset_XM;
};

datablock afxEffectWrapperData(SFM_ExplosionMagic_Bot_EW : SFM_ExplosionMagic_Top_EW)
{
  effect = SFM_ExplosionMagic_Bot_E;
};

// pink glow explosion
datablock afxModelData(SFM_LarvaExplosion_CE)
{
   shapeFile = %mySpellDataPath @ "/SFM/models/SFM_larvaExplosion.dts";
   alphaMult = 0.30;
   sequence = "explode";
   sequenceRate = 1.5;
   useVertexAlpha = true;
   forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
};
datablock afxEffectWrapperData(SFM_LarvaExplosion_EW)
{
  effect = SFM_LarvaExplosion_CE;
  posConstraint = caster;
  delay = 10.5; //11.5;
  fadeInTime  = 0.0;
  fadeOutTime = 0.67;
  lifetime = 0.66;
  offset = "0 0 3.75"; // ??????????????????????????????????????????????????????
  xfmModifiers[0] = SFM_Cocoon_offset_XM;
};

// a ring of dust that expands outward just above the ground
datablock ParticleData(SFM_ExplosionDust1_P)
{
   textureName          = %mySpellDataPath @ "/SFM/particles/smoke";
   dragCoeffiecient     = 0.5;
   gravityCoefficient   = 0.2;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 2000;
   lifetimeVarianceMS   = 500;
   useInvAlpha          = true;
   spinRandomMin        = -60.0;
   spinRandomMax        = 60.0;
   colors[0]            = "0.4 0.33 0.2 0.0";  
   colors[1]            = "0.4 0.33 0.2 0.25";
   colors[2]            = "0.4 0.33 0.2 0.15";
   colors[3]            = "0.4 0.33 0.2 0.0";
   sizes[0]             = 0.8;
   sizes[1]             = 2.0;
   sizes[2]             = 3.5;
   sizes[3]             = 5.0;
   times[0]             = 0.0;
   times[1]             = 0.2;
   times[2]             = 0.7;
   times[3]             = 1.0;   
};
datablock ParticleData(SFM_ExplosionDust2_P : SFM_ExplosionDust1_P)
{
   colors[0]            = "0.66 0.55 0.33 0.0";   
   colors[1]            = "0.66 0.55 0.33 0.25";
   colors[2]            = "0.66 0.55 0.33 0.15";
   colors[3]            = "0.66 0.55 0.33 0.0";
};

datablock afxParticleEmitterConeData(SFM_ExplosionDust_E) // TGEA
{
  ejectionOffset    = 13.0;
  ejectionPeriodMS  = 8;
  periodVarianceMS  = 2;
  ejectionVelocity  = 6.0; //8.0;
  velocityVariance  = 1.5;  
  particles         = "SFM_ExplosionDust1_P SFM_ExplosionDust2_P";

  // TGE emitterType = "cone";
  vector = "0.0 0.0 1.0";
  spreadMin = 179.0;
  spreadMax = 179.0;
  fadeOffset = true;
};

datablock afxXM_WorldOffsetData(SFM_ExplosionDust_offset_XM)
{
  worldOffset = "0 0 0.5";
};
//
datablock afxEffectWrapperData(SFM_ExplosionDust_EW)
{
  effect = SFM_ExplosionDust_E;
  posConstraint = caster;
  lifetime = 1.5;
  delay    = 11.0; //12.0;
  fadeInTime  = 1.5; 
  xfmModifiers[0] = SFM_ExplosionDust_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GIANT MOTH

//
// This is the summoned moth. Still need to insert it into the 
// scene as a targetable object and have it fly off on its own.
//

datablock afxModelData(SFM_Moth_CE)
{
   shapeFile = %mySpellDataPath @ "/SFM/models/SFM_moth.dts";
   sequence = "flap";
   sequenceRate = 7.0;
   receiveLMLighting = false; // Test on TGE
};

datablock afxEffectWrapperData(SFM_Moth_EW)
{
  effect = SFM_Moth_CE;
  //posConstraint = caster;
  constraint = caster;
  delay = 11.25; //12.25;
  fadeInTime  = 0.0;
  fadeOutTime = 1.0;
  lifetime = 4.0;
  xfmModifiers[0] = SFM_Cocoon_offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/sfm_lighting_tge_sub.cs");
    exec("./audio/sfm_audio_sub.cs");
  case "TGEA":
    exec("./lighting/sfm_lighting_tgea_sub.cs");
    exec("./audio/sfm_audio_sub.cs");
  case "T3D":
    exec("./lighting/sfm_lighting_t3d_sub.cs");
    exec("./audio/sfm_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SUMMON FECKLESS MOTH SPELL
//

datablock afxMagicSpellData(SummonFecklessMothSpell)
{
    // warmup //
  castingDur = 10;

    // spellcaster animation //
  addCastingEffect = SFM_Casting_Clip_EW;
  addCastingEffect = SFM_AnimLock_EW;
    // casting zodiac //
  addCastingEffect = SFM_CastingZodeMooring_EW;
  addCastingEffect = SFM_ZodeReveal_EW;
  addCastingEffect = SFM_Zode1_underglow_EW;
  addCastingEffect = SFM_Zode1_EW;
  addCastingEffect = SFM_Zode2_underglow_EW;
  addCastingEffect = SFM_Zode2_EW;
  addCastingEffect = SFM_ZodePortal_EW;
    // giant caterpillar //
  addCastingEffect = SFM_CaterpillarHead_EW;
  addCastingEffect = SFM_CaterpillarSegment1_EW; 
  addCastingEffect = SFM_CaterpillarSegment2_EW;
  addCastingEffect = SFM_CaterpillarSegment3_EW;
  addCastingEffect = SFM_CaterpillarSegment4_EW;
  addCastingEffect = SFM_CaterpillarSegment5_EW;
  addCastingEffect = SFM_CaterpillarSegment6_EW;
  addCastingEffect = SFM_CaterpillarSegment7_EW;
  addCastingEffect = SFM_CaterpillarSegment8_EW;
  addCastingEffect = SFM_CaterpillarSegment9_EW;
  addCastingEffect = SFM_CaterpillarSegment10_EW;
    // cocoon //
  addCastingEffect = SFM_Cocoon_EW;
  addCastingEffect = SFM_LarvaPulse_EW;
    // moth swarm //
  addCastingEffect = SFM_MothSwarm1_EW;
  addCastingEffect = SFM_MothSwarm2_EW;
  addCastingEffect = SFM_MothSwarm3_EW;
  addCastingEffect = SFM_MothSwarm4_EW;
  addCastingEffect = SFM_MothSwarm5_EW;
    // summoning explosion //
  addCastingEffect = SFM_ExplosionMagic_Top_EW;
  addCastingEffect = SFM_ExplosionMagic_Bot_EW;
  addCastingEffect = SFM_LarvaExplosion_EW;
  addCastingEffect = SFM_ExplosionDust_EW;
    // giant moth //
  addCastingEffect = SFM_Moth_EW;
};

// sounds and lights added via sub-script functions //
SFM_add_Lighting_FX(SummonFecklessMothSpell);
SFM_add_Audio_FX(SummonFecklessMothSpell);

datablock afxRPGMagicSpellData(SummonFecklessMothSpell_RPG)
{
  spellName = "Summon Feckless Moth";
  desc = "Endure a disgusting giant CATERPILLAR " @ 
         "crawling up your body and you'll be " @ 
         "rewarded with an equally disgusting giant " @ 
         "MOTH pet. EEEW!!!" @ 
         "\n" @
         "\nspell design: Matthew Durante" @
         "\nsound effects: Dave Schroeder" @ 
         "\nspell concept: Jeff Faust";
  sourcePack = "Spell Pack 1";

  iconBitmap = %mySpellDataPath @ "/SFM/icons/sfm";
  target = "nothing";
  manaCost = 10;
  castingDur = SummonFecklessMothSpell.castingDur;
  allowDamageInterrupts = true;
  minDamageToInterrupt = 10;
};

// set a level of detail
function SummonFecklessMothSpell::onActivate(%this, %spell, %caster, %target)
{
  Parent::onActivate(%this, %spell, %caster, %target);
  if (MissionInfo.hasBrightLighting)
    %spell.setExecConditions($BrightLighting_mask);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectWrapperData(SFM_Solo_Moth_EW)
{
  effect = SFM_Moth_CE;
  constraint = anchor;
  fadeInTime  = 2.0;
  fadeOutTime = 2.0;
};

datablock afxEffectronData(GiantMothProp)
{
  duration = $AFX::INFINITE_TIME;
  execOnNewClients = true;
  addEffect = SFM_Solo_Moth_EW;
};

function GiantMothProp::spawn()
{
  if (!isObject(GiantMothProp.one_and_only))
    GiantMothProp.one_and_only = startEffectron(GiantMothProp, "360.173 309.959 220", "anchor");
}

function GiantMothProp::kill()
{
  if (isObject(GiantMothProp.one_and_only))
    GiantMothProp.one_and_only.interrupt();
}

function GiantMothProp::onDeactivate(%this, %prop)
{
  GiantMothProp.one_and_only = "";
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
  SummonFecklessMothSpell.scriptFile = $afxAutoloadScriptFile;
  SummonFecklessMothSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(SummonFecklessMothSpell, SummonFecklessMothSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//