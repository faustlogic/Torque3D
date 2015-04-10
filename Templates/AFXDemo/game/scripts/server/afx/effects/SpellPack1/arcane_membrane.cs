
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// ARCANE IN THE MEMBRANE SPELL
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
$spell_reload = isObject(ArcaneMembraneSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = ArcaneMembraneSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

// ParticleEmitterData::useEmitterTransform is an alternative to 
// ParticleData::constrainPos but it currently does not work in TGEA.
$UseEmitterTransform = false;  // should always be false on TGEA

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPELLCASTER ANIMATION

//
// The spellcaster quickly covers his face to hide from the craziness
// he is casting, but once the mask has passed he raises up, summoning
// symbols from his hands that send the craziness away.
//

datablock afxAnimClipData(AitM_Casting_Clip_CE)
{
  clipName = "aitm";
  ignoreCorpse = true;
  rate = 1.0;
};
//
datablock afxEffectWrapperData(AitM_Casting_Clip_EW)
{
  effect = AitM_Casting_Clip_CE;
  constraint = "caster";
  lifetime = 180/30;
  delay = 0.0;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING CRAZY FACE 

//
// A giant mask, like some crazy clown from some crazy world,
// introduces the casting.  It has searching eyes that find you,
// and something else, did you see the teeth???
//
// The mask is animated entirely with transform modifiers: a local
// transform moves it, a path moves it closer to the caster while
// scale modifiers enlarge and stretch it out.
//
// The eyes reuse all of the mask's modifiers, then additionally are
// animated with three paths that make them look left, right, and 
// down at the caster.  Three individual paths are used to get nice
// crisp motion; a single path would never work given how it 
// interpolates...
//
// The teeth work in a similar way, reusing the mask's modifiers for
// correct placement.  However, because I don't want them to appear
// until late in the effect, I had to do the visibility animation in
// the dts file.  If I had tried to change the delay, the teeth would
// no longer have lined-up with the mask.
//

// crazy mask path, moving the mask closer to the caster
datablock afxPathData(AitM_CasterCrazyHead_Path)
{
  points = "0 -2.0 0" SPC
           "0 -0.3 0";
};
//
datablock afxXM_PathConformData(AitM_CasterCrazyHead_path_XM)
{
  paths = "AitM_CasterCrazyHead_Path";
};

// crazy mask aim constraint
datablock afxXM_AimData(AitM_CasterCrazyHead_aim_XM)
{
  aimZOnly = true;
};

// crazy mask scale 1
datablock afxXM_ScaleData(AitM_CasterCrazyHead_scale_XM)
{
  scale = "6 6 3";

  delay = 0.0;
  fadeInTime = 3.0;
};

// crazy mask scale 2, stretching it out (elongating)
datablock afxXM_ScaleData(AitM_CasterCrazyHead_stretch_scale_XM)
{
  scale = "0 0 6";

  delay = 1.0;
  fadeInTime = 2.0;
};

datablock afxXM_LocalOffsetData(AitM_CasterCrazyHead_offset_XM)
{
  localOffset = "0 0 2.5";

  delay = 0.0;
  fadeInTime = 3.0;
};

// crazy mask
datablock afxModelData(AitM_CasterCrazyHead_CE)
{
  shapeFile = %mySpellDataPath @ "/AitM/models/AitM_casterCrazyHead.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  useVertexAlpha = true; // TGE (ignored by TGEA)
  alphaMult = 0.25;
};
//
datablock afxEffectWrapperData(AitM_CasterCrazyHead_EW)
{
  effect = AitM_CasterCrazyHead_CE;
  posConstraint = "caster.Bip01 Head";
  posConstraint2 = "camera"; // aim

  scaleFactor = 3.0;

  delay       = 0;
  fadeInTime  = 1.0;
  fadeOutTime = 2.0;
  lifetime    = 1.0;

  xfmModifiers[0] = AitM_CasterCrazyHead_scale_XM;
  xfmModifiers[1] = AitM_CasterCrazyHead_stretch_scale_XM;
  xfmModifiers[2] = AitM_CasterCrazyHead_offset_XM;
  xfmModifiers[3] = AitM_CasterCrazyHead_aim_XM;
  xfmModifiers[4] = AitM_CasterCrazyHead_path_XM;
};

// eye paths
//  look camera left
datablock afxPathData(AitM_CasterCrazyEye_1_Path)
{
  points = "0 0  0" SPC
           "0.2 0 0";
  lifetime = 0.3;
  delay = 0.5;
};
//  look camera right
datablock afxPathData(AitM_CasterCrazyEye_2_Path)
{
  points = "0 0  0" SPC
           "-0.4 0 0";
  lifetime = 0.35;
  delay = 1.0;
};
//  look center and down at the caster
datablock afxPathData(AitM_CasterCrazyEye_3_Path)
{
  points = "0 0  0" SPC
           "0.2 0 -0.6";
  lifetime = 0.75;
  delay = 1.75;
};
//
datablock afxXM_PathConformData(AitM_CasterCrazyEye_path_XM)
{
  paths = "AitM_CasterCrazyEye_1_Path AitM_CasterCrazyEye_2_Path AitM_CasterCrazyEye_3_Path";
};

// caster eyes, RT and LF
datablock afxModelData(AitM_CasterCrazyEye_RT_CE)
{
  shapeFile = %mySpellDataPath @ "/AitM/models/AitM_casterCrazyEye_RT.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  useVertexAlpha = true; // TGE (ignored by TGEA)
  alphaMult = 0.75; //0.65;
};
//
datablock afxModelData(AitM_CasterCrazyEye_LF_CE : AitM_CasterCrazyEye_RT_CE)
{
  shapeFile = %mySpellDataPath @ "/AitM/models/AitM_casterCrazyEye_LF.dts";
};
// (both eyes reuse all the masks transforms and timing parameters to
//  stay properly aligned)
datablock afxEffectWrapperData(AitM_CasterCrazyEye_RT_EW : AitM_CasterCrazyHead_EW)
{
  effect = AitM_CasterCrazyEye_RT_CE;
  xfmModifiers[5] = AitM_CasterCrazyEye_path_XM;
};
//
datablock afxEffectWrapperData(AitM_CasterCrazyEye_LF_EW : AitM_CasterCrazyEye_RT_EW)
{
  effect = AitM_CasterCrazyEye_LF_CE;
};

// teeth
datablock afxModelData(AitM_CasterCrazyTeeth_CE)
{
  shapeFile = %mySpellDataPath @ "/AitM/models/AitM_casterCrazyTeeth.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  sequence = "fade";
  useVertexAlpha = true; // TGE (ignored by TGEA)
  alphaMult = 0.4;
};
//
datablock afxEffectWrapperData(AitM_CasterCrazyTeeth_EW : AitM_CasterCrazyHead_EW)
{
  effect = AitM_CasterCrazyTeeth_CE;
  
  delay       = 0;
  fadeInTime  = 0;
  fadeOutTime = 0;
  lifetime    = 3.0;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING SPARKLES

//
// During casting, twenty sparkles appear around the caster, gently
// moving upward and twisting then fading away.
//
// These particles are emitted in the local-space of the emitter,
// rather than into world-space as normal.  Doing this, and emitting
// them without velocity, the sparkles remain clumped together even
// as the emitters move.  This mode is set using the "constrainPos"
// attribute of ParticleData.  To see the difference, just comment
// out the line and notice what happens when the sparkles are
// emitted into world-space.
//
// Each emitter has its own local offset from the caster, about which
// it spins.  The direction of spin is alternated from positive to
// negative for each emitter.  They all share the same upward path.
//

// large sparkles, positive spin
datablock ParticleData(AitM_Sparkle_Big_pos_P)
{
  textureName          = %mySpellDataPath @ "/AitM/particles/AitM_sparkle";
  dragCoeffiecient     = 0;
  gravityCoefficient   = 0;
  windCoefficient      = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 1000;
  lifetimeVarianceMS   = 0;
  useInvAlpha          = false;
  spinRandomMin        = 60.0;
  spinRandomMax        = 60.0;
  colors[0]            = 255/255 SPC 158/255 SPC 51/255 SPC "0.4";
  colors[1]            = "1 1 1 0";
  sizes[0]             = 0.7; 
  sizes[1]             = 0.4;
  times[0]             = 0.9;
  times[1]             = 1.0;

  constrainPos         = !$UseEmitterTransform;
};
// large sparkles, negative spin
datablock ParticleData(AitM_Sparkle_Big_neg_P : AitM_Sparkle_Big_pos_P)
{
  spinRandomMin        = -60.0;
  spinRandomMax        = -60.0;
};
// medium sparkles, positive spin
datablock ParticleData(AitM_Sparkle_Medium_pos_P : AitM_Sparkle_Big_pos_P)
{
  colors[0]            = 255/255 SPC 255/255 SPC 51/255 SPC "0.4";
  sizes[0]             = 0.7*0.6; 
  sizes[1]             = 0.4*0.6;
};
// medium sparkles, negative spin
datablock ParticleData(AitM_Sparkle_Medium_neg_P : AitM_Sparkle_Medium_pos_P)
{
  spinRandomMin        = -60.0;
  spinRandomMax        = -60.0;
};
// small sparkles, positive spin
datablock ParticleData(AitM_Sparkle_Small_pos_P : AitM_Sparkle_Big_pos_P)
{
  colors[0]            = 255/255 SPC 255/255 SPC 138/255 SPC "0.4";
  sizes[0]             = 0.7*0.4; 
  sizes[1]             = 0.4*0.4;
};
// small sparkles, negative spin
datablock ParticleData(AitM_Sparkle_Small_neg_P : AitM_Sparkle_Small_pos_P)
{
  spinRandomMin        = -60.0;
  spinRandomMax        = -60.0;
};

// sparkle emitter, big sparkles
datablock ParticleEmitterData(AitM_Sparkle_Big_E)
{
  ejectionPeriodMS      = 100;
  periodVarianceMS      = 0;
  ejectionVelocity      = 0;
  velocityVariance      = 0;
  thetaMin              = 0.0;
  thetaMax              = 180.0;
  particles             = "AitM_Sparkle_Big_pos_P AitM_Sparkle_Big_neg_P";
  fadeColor             = true;
  useEmitterTransform = $UseEmitterTransform;
};
// sparkle emitter, medium sparkles
datablock ParticleEmitterData(AitM_Sparkle_Medium_E : AitM_Sparkle_Big_E)
{
  particles             = "AitM_Sparkle_Medium_pos_P AitM_Sparkle_Medium_neg_P";
};
// sparkle emitter, small sparkles
datablock ParticleEmitterData(AitM_Sparkle_Small_E : AitM_Sparkle_Big_E)
{
  particles             = "AitM_Sparkle_Small_pos_P AitM_Sparkle_Small_neg_P";
};

// sparkle offsets
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_1_XM)
{
  localOffset = "-1.5 -2.0 1.0";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_2_XM)
{
  localOffset = "-0.5 2.2 4.0";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_3_XM)
{
  localOffset = "1.53 -1.0 3.2";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_4_XM)
{
  localOffset = "2.03 -1.7 1.54";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_5_XM)
{
  localOffset = "1.45 1.3 0.7";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_6_XM)
{
  localOffset = "-1.12 2.78 3.6";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_7_XM)
{
  localOffset = "1.56 -0.7 4.4";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_8_XM)
{
  localOffset = "-2.1 1.0 1.9";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_9_XM)
{
  localOffset = "1.0 1.05 2.7";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_10_XM)
{
  localOffset = "0.5 -1.8 3.85";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_11_XM)
{
  localOffset = "-1.3 2.0 1.85";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_12_XM)
{
  localOffset = "1.05 -0.85 0.65";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_13_XM)
{
  localOffset = "-0.7 -1.25 3.15";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_14_XM)
{
  localOffset = "-2.7 2.25 2.1";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_15_XM)
{
  localOffset = "1.17 0.5 2.67";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_16_XM)
{
  localOffset = "-1.25 -1.5 3.05";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_17_XM)
{
  localOffset = "2.1 -0.5 1.5";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_18_XM)
{
  localOffset = "1.6 -2.03 0.9";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_19_XM)
{
  localOffset = "-0.43 1.98 2.6";
};
datablock afxXM_LocalOffsetData(AitM_Sparkle_Offset_20_XM)
{
  localOffset = "-1.63 -1.5 3.1";
};

// sparkle path, moving up
datablock afxPathData(AitM_Sparkle_Path)
{
  points = "0 0 -0.2" SPC
           "0 0  0.4";
};
//
datablock afxXM_PathConformData(AitM_Sparkle_path_XM)
{
  paths = "AitM_Sparkle_Path";
};

// sparkle spin, positive
datablock afxXM_SpinData(AitM_Sparkle_spin1_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 0.0;
  spinRate  = 20;
};
// sparkle spin, negative
datablock afxXM_SpinData(AitM_Sparkle_spin2_XM : AitM_Sparkle_spin1_XM)
{
  spinRate  = -20;
};

// sparkles, appearing sequentially
$AitM_Sparkle_delay = 0.1;
datablock afxEffectWrapperData(AitM_Sparkle_1_EW)
{
  effect = AitM_Sparkle_Big_E;
  posConstraint = caster;

  delay = $AitM_Sparkle_delay * 1;
  fadeInTime  = 0.2;
  fadeOutTime = 0.2;
  lifetime = 1.0;

  xfmModifiers[0] = AitM_Sparkle_spin1_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_1_XM;
  xfmModifiers[2] = AitM_Sparkle_path_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_2_EW : AitM_Sparkle_1_EW)
{
  effect = AitM_Sparkle_Big_E;
  delay  = $AitM_Sparkle_delay * 2;
  xfmModifiers[0] = AitM_Sparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_2_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_3_EW : AitM_Sparkle_1_EW)
{
  effect = AitM_Sparkle_Small_E;
  delay  = $AitM_Sparkle_delay * 3;
  xfmModifiers[1] = AitM_Sparkle_Offset_3_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_4_EW : AitM_Sparkle_1_EW)
{
  effect = AitM_Sparkle_Medium_E;
  delay  = $AitM_Sparkle_delay * 4;
  xfmModifiers[0] = AitM_Sparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_4_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_5_EW : AitM_Sparkle_1_EW)
{
  effect = AitM_Sparkle_Small_E;
  delay  = $AitM_Sparkle_delay * 5;
  xfmModifiers[1] = AitM_Sparkle_Offset_5_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_6_EW : AitM_Sparkle_1_EW)
{
  effect = AitM_Sparkle_Big_E;
  delay  = $AitM_Sparkle_delay * 6;
  xfmModifiers[0] = AitM_Sparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_6_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_7_EW : AitM_Sparkle_1_EW)
{ 
  effect = AitM_Sparkle_Medium_E;
  delay  = $AitM_Sparkle_delay * 7;
  xfmModifiers[1] = AitM_Sparkle_Offset_7_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_8_EW : AitM_Sparkle_1_EW)
{
  effect = AitM_Sparkle_Medium_E;
  delay  = $AitM_Sparkle_delay * 8;
  xfmModifiers[0] = AitM_Sparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_8_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_9_EW : AitM_Sparkle_1_EW)
{
  effect = AitM_Sparkle_Big_E;
  delay  = $AitM_Sparkle_delay * 9;
  xfmModifiers[1] = AitM_Sparkle_Offset_9_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_10_EW : AitM_Sparkle_1_EW)
{
  effect = AitM_Sparkle_Small_E;
  delay  = $AitM_Sparkle_delay * 10;
  xfmModifiers[0] = AitM_Sparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_10_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_11_EW : AitM_Sparkle_1_EW)
{
  effect = AitM_Sparkle_Big_E;
  delay  = $AitM_Sparkle_delay * 11;
  xfmModifiers[1] = AitM_Sparkle_Offset_11_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_12_EW : AitM_Sparkle_1_EW)
{
  effect = AitM_Sparkle_Big_E;
  delay  = $AitM_Sparkle_delay * 12;
  xfmModifiers[0] = AitM_Sparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_12_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_13_EW : AitM_Sparkle_1_EW)
{
  effect = AitM_Sparkle_Small_E;
  delay  = $AitM_Sparkle_delay * 13;
  xfmModifiers[1] = AitM_Sparkle_Offset_13_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_14_EW : AitM_Sparkle_1_EW)
{
  effect = AitM_Sparkle_Medium_E;
  delay  = $AitM_Sparkle_delay * 14;
  xfmModifiers[0] = AitM_Sparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_14_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_15_EW : AitM_Sparkle_1_EW)
{
  effect = AitM_Sparkle_Small_E;
  delay  = $AitM_Sparkle_delay * 15;
  xfmModifiers[1] = AitM_Sparkle_Offset_15_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_16_EW : AitM_Sparkle_1_EW)
{
  effect = AitM_Sparkle_Big_E;
  delay  = $AitM_Sparkle_delay * 16;
  xfmModifiers[0] = AitM_Sparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_16_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_17_EW : AitM_Sparkle_1_EW)
{ 
  effect = AitM_Sparkle_Medium_E;
  delay  = $AitM_Sparkle_delay * 17;
  xfmModifiers[1] = AitM_Sparkle_Offset_17_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_18_EW : AitM_Sparkle_1_EW)
{
  effect = AitM_Sparkle_Medium_E;
  delay  = $AitM_Sparkle_delay * 18;
  xfmModifiers[0] = AitM_Sparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_18_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_19_EW : AitM_Sparkle_1_EW)
{
  effect = AitM_Sparkle_Big_E;
  delay  = $AitM_Sparkle_delay * 19;
  xfmModifiers[1] = AitM_Sparkle_Offset_19_XM;
};
datablock afxEffectWrapperData(AitM_Sparkle_20_EW : AitM_Sparkle_1_EW)
{
  effect = AitM_Sparkle_Small_E;
  delay  = $AitM_Sparkle_delay * 20;
  xfmModifiers[0] = AitM_Sparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_20_XM;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING HAND SYMBOLS & SPARKLES

//
// Casting ends with these particles shooting from the hands of the
// caster.  Five arcane/bizarre symbols are used as textures, A-E.
// The particles are slightly unusual in that they increase in size
// during the majority of their lifetime, but at the end suddenly
// decrease (bizarre!).
//
// Along with the symbols are a whole lot of sparkles... for that
// magic feel.
//

// casting symbol A particle
datablock ParticleData(AitM_Symbol_A_P)
{
  // TGE textureName          = %mySpellDataPath @ "/AitM/particles/AitM_symbol_A";
  dragCoeffiecient     = 0;
  gravityCoefficient   = 0;
  windCoefficient      = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 1000;
  lifetimeVarianceMS   = 200;
  useInvAlpha          = false;
  spinRandomMin        = 0.0;
  spinRandomMax        = 0.0;
  colors[0]            = "0 0 0 0";
  colors[1]            = 255/255 SPC 158/255 SPC 51/255 SPC "1.0";
  colors[2]            = "1 1 1 1.0";
  colors[3]            = "0 0 0 0";
  sizes[0]             = 0.5; 
  sizes[1]             = 1.0;
  sizes[2]             = 2.0;
  sizes[3]             = 0.0;
  times[0]             = 0.0;
  times[1]             = 0.3;
  times[2]             = 0.8;
  times[3]             = 1.0;

  // TGEA
  textureName          = %mySpellDataPath @ "/AitM/particles/aitm_tiled_symbols"; // symbol_A
  textureCoords[0]     = "0.0 0.0";
  textureCoords[1]     = "0.0 0.5";
  textureCoords[2]     = "0.5 0.5";
  textureCoords[3]     = "0.5 0.0";
};
// casting symbol B particle
datablock ParticleData(AitM_Symbol_B_P : AitM_Symbol_A_P)
{
  // TGE textureName          = %mySpellDataPath @ "/AitM/particles/AitM_symbol_B";
  // TGEA -- symbol_B
  textureCoords[0]     = "0.322 0.5";
  textureCoords[1]     = "0.322 1.0";
  textureCoords[2]     = "0.697 1.0";
  textureCoords[3]     = "0.697 0.5";
};
// casting symbol C particle
datablock ParticleData(AitM_Symbol_C_P : AitM_Symbol_A_P)
{
  // TGE textureName          = %mySpellDataPath @ "/AitM/particles/AitM_symbol_C";
  // TGEA -- symbol_C
  textureCoords[0]     = "0.5 0.0";
  textureCoords[1]     = "0.5 0.5";
  textureCoords[2]     = "1.0 0.5";
  textureCoords[3]     = "1.0 0.0";
};
// casting symbol D particle
datablock ParticleData(AitM_Symbol_D_P : AitM_Symbol_A_P)
{
  // TGE textureName          = %mySpellDataPath @ "/AitM/particles/AitM_symbol_D";
  // TGEA -- symbol_D
  textureCoords[0]     = "0.623 0.5";
  textureCoords[1]     = "0.623 1.0";
  textureCoords[2]     = "1.0   1.0";
  textureCoords[3]     = "1.0   0.5";
};
// casting symbol E particle
datablock ParticleData(AitM_Symbol_E_P : AitM_Symbol_A_P)
{
  // TGE textureName          = %mySpellDataPath @ "/AitM/particles/AitM_symbol_E";
  // TGEA -- symbol_E
  textureCoords[0]     = "0.0   0.5";
  textureCoords[1]     = "0.0   1.0";
  textureCoords[2]     = "0.373 1.0";
  textureCoords[3]     = "0.373 0.5";
};

// casting symbol emitter (standard "sprinkler" type)
datablock ParticleEmitterData(AitM_CasterHand_Symbols_E)
{
  ejectionPeriodMS      = 140;
  periodVarianceMS      = 14;
  ejectionVelocity      = 2.0;
  velocityVariance      = 0.2;
  thetaMin              = 0.0;
  thetaMax              = 180.0;
  particles             = "AitM_Symbol_A_P AitM_Symbol_B_P AitM_Symbol_C_P AitM_Symbol_D_P AitM_Symbol_E_P";
  fadeColor             = true;
  ejectionOffset        = 1.0;
};

// casting symbols, left hand
datablock afxEffectWrapperData(AitM_CasterSymbols_lf_hand_EW)
{
  effect = AitM_CasterHand_Symbols_E;
  constraint = "caster.Bip01 L Hand";
  lifetime = 2.3;
  delay = 3.0;
  fadeInTime = 0;
  fadeOutTime = 0.3;
};
// casting symbols, right hand
datablock afxEffectWrapperData(AitM_CasterSymbols_rt_hand_EW : AitM_CasterSymbols_lf_hand_EW)
{
  constraint = "caster.Bip01 R Hand";
};

// casting sparkle particle
datablock ParticleData(AitM_CastingSparkle_P)
{
   textureName          = %mySpellDataPath @ "/AitM/particles/AitM_sparkle";
   dragCoeffiecient     = 0;
   gravityCoefficient   = 0;
   windCoefficient      = 0;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 1000;
   lifetimeVarianceMS   = 200;
   useInvAlpha          = false;
   spinRandomMin        = 0.0;
   spinRandomMax        = 0.0;
   colors[0]            = "1 1 1 0.7";
   colors[1]            = 255/255 SPC 158/255 SPC 51/255 SPC "1.0"; //255/255 SPC 255/255 SPC 60/255 SPC "1.0";
   colors[2]            = "0 0 0 0";
   sizes[0]             = 3.0; 
   sizes[1]             = 0.7;
   sizes[2]             = 0.4;
   times[0]             = 0.2;
   times[1]             = 0.8;
   times[2]             = 1.0;
};

// casting sparkle emitter (standard "sprinkler" type)
datablock ParticleEmitterData(AitM_CasterHand_Sparkle_E)
{
  ejectionPeriodMS      = 20;
  periodVarianceMS      = 3;
  ejectionVelocity      = 1.4;
  velocityVariance      = 0.2;
  thetaMin              = 0.0;
  thetaMax              = 180.0;
  particles             = "AitM_CastingSparkle_P";
  fadeColor             = true;
};

// casting sparkles, left hand
datablock afxEffectWrapperData(AitM_CasterSparkle_lf_hand_EW)
{
  effect = AitM_CasterHand_Sparkle_E;
  constraint = "caster.Bip01 L Hand";
  lifetime = 2.3;
  delay = 3.0;
  fadeInTime = 0;
  fadeOutTime = 0.3;
};
// casting sparkles, right hand
datablock afxEffectWrapperData(AitM_CasterSparkle_rt_hand_EW : AitM_CasterSparkle_lf_hand_EW)
{
  constraint = "caster.Bip01 R Hand";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// TARGET SYMBOLS

//
// Symbols like those emitted from the caster's hands begin the 
// impact phase by shooting into the target's head.  This is done
// using an emitter constrained to the head with "ejectionInvert"
// set.  In order to track a moving target effectively, "constrainPos"
// must be used (otherwise the particles will strike the position 
// where the target *was* some time earlier).
//

// target symbols particles, A-E:
datablock ParticleData(AitM_TargetSymbol_A_P)
{
  // TGE textureName          = %mySpellDataPath @ "/AitM/particles/AitM_symbol_A";
  dragCoeffiecient     = 0;
  gravityCoefficient   = 0;
  windCoefficient      = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 400;
  lifetimeVarianceMS   = 40;
  useInvAlpha          = false;
  spinRandomMin        = 0.0;
  spinRandomMax        = 0.0;
  colors[0]            = "0 0 0 0";
  colors[1]            = "1 1 1 1.0";
  colors[2]            = "0.2 0 1 1";
  colors[3]            = "0 0 0 0";
  sizes[0]             = 0.5; 
  sizes[1]             = 2.5;
  sizes[2]             = 1.5;
  sizes[3]             = 0.0;
  times[0]             = 0.0;
  times[1]             = 0.3;
  times[2]             = 0.8;
  times[3]             = 1.0;

  constrainPos         = !$UseEmitterTransform;

  // TGEA
  textureName          = %mySpellDataPath @ "/AitM/particles/aitm_tiled_symbols"; // symbol_A
  textureCoords[0]     = "0.0 0.0";
  textureCoords[1]     = "0.0 0.5";
  textureCoords[2]     = "0.5 0.5";
  textureCoords[3]     = "0.5 0.0";
};
datablock ParticleData(AitM_TargetSymbol_B_P : AitM_TargetSymbol_A_P)
{
  // TGE textureName          = %mySpellDataPath @ "/AitM/particles/AitM_symbol_B";
  colors[2]            = "0.5 0 1 1";

  // TGEA -- symbol_B
  textureCoords[0]     = "0.322 0.5";
  textureCoords[1]     = "0.322 1.0";
  textureCoords[2]     = "0.697 1.0";
  textureCoords[3]     = "0.697 0.5";
};
datablock ParticleData(AitM_TargetSymbol_C_P : AitM_TargetSymbol_A_P)
{
  // TGE textureName          = %mySpellDataPath @ "/AitM/particles/AitM_symbol_C";
  colors[2]            = "0.8 0 1 1";

  // TGEA -- symbol_C
  textureCoords[0]     = "0.5 0.0";
  textureCoords[1]     = "0.5 0.5";
  textureCoords[2]     = "1.0 0.5";
  textureCoords[3]     = "1.0 0.0";
};
datablock ParticleData(AitM_TargetSymbol_D_P : AitM_TargetSymbol_A_P)
{
  // TGE textureName          = %mySpellDataPath @ "/AitM/particles/AitM_symbol_D";
  // TGEA -- symbol_D
  textureCoords[0]     = "0.623 0.5";
  textureCoords[1]     = "0.623 1.0";
  textureCoords[2]     = "1.0   1.0";
  textureCoords[3]     = "1.0   0.5";
};
datablock ParticleData(AitM_TargetSymbol_E_P : AitM_TargetSymbol_A_P)
{
  // TGE textureName          = %mySpellDataPath @ "/AitM/particles/AitM_symbol_E";
  colors[2]            = "0.5 0 1 1";

  // TGEA -- symbol_E
  textureCoords[0]     = "0.0   0.5";
  textureCoords[1]     = "0.0   1.0";
  textureCoords[2]     = "0.373 1.0";
  textureCoords[3]     = "0.373 0.5";
};

// target head symbol emitter
//  ejectionInvert and a positive ejectionOffset cause particles to
//  shoot inward
datablock ParticleEmitterData(AitM_TargetHead_Symbols_E)
{
  ejectionPeriodMS      = 100;
  periodVarianceMS      = 10;
  ejectionVelocity      = 7.0; 
  velocityVariance      = 1.0;
  thetaMin              = 0.0;
  thetaMax              = 180.0;
  particles             = "AitM_TargetSymbol_A_P AitM_TargetSymbol_B_P" SPC
                          "AitM_TargetSymbol_C_P AitM_TargetSymbol_D_P" SPC
                          "AitM_TargetSymbol_E_P";
  fadeColor             = true;

  ejectionOffset = 3.0;
  ejectionInvert = true;
  useEmitterTransform = $UseEmitterTransform;
};

// target head symbols
datablock afxEffectWrapperData(AitM_TargetSymbols_EW)
{
  effect = AitM_TargetHead_Symbols_E;
  constraint = "impactedObject.Bip01 Head";
  lifetime = 2.5;//3.0;
  delay = 0.5;
  fadeInTime = 0;
  fadeOutTime = 0.0;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// TARGET ZODIACS

//
// Three zodiacs appear around the target, growing to large radius
// that anticipates the confusion cloud vortex formation that
// immediately follows.
//

// target zodiac A, small radius
datablock afxZodiacData(AitM_TargetZodeA_CE)
{  
  texture = %mySpellDataPath @ "/AitM/zodiacs/AitM_targetZode";
  radius = 3.0;
  startAngle = 0.0;
  rotationRate = 90.0;  
  color = "0.85 0.85 0.85 0.8";
  blend = additive;
  showOnInteriors = false;
  growthRate = 13.0;
};
datablock afxEffectWrapperData(AitM_TargetZodeA_EW)
{
  effect = AitM_TargetZodeA_CE;
  posConstraint = "impactedObject";
  delay = 0.5+2.0;
  fadeInTime = 0.1;
  fadeOutTime = 0.3;
  lifetime = 0.4;
  xfmModifiers[0] = SHARED_freeze_XM;
};

// target zodiac B, medium radius
datablock afxZodiacData(AitM_TargetZodeB_CE : AitM_TargetZodeA_CE)
{  
  radius = 7.5;
  startAngle = 60.0;
};
datablock afxEffectWrapperData(AitM_TargetZodeB_EW : AitM_TargetZodeA_EW)
{
  effect = AitM_TargetZodeB_CE;
  delay = 0.75+2.0;
};

// target zodiac C, large radius
datablock afxZodiacData(AitM_TargetZodeC_CE : AitM_TargetZodeA_CE)
{  
  radius = 15.0;
  startAngle = 120.0;
};
datablock afxEffectWrapperData(AitM_TargetZodeC_EW : AitM_TargetZodeA_EW)
{
  effect = AitM_TargetZodeC_CE;
  delay = 1.0+2.0;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// TARGET CONFUSION CLOUD VORTEX

//
// The confusion cloud is formed by a vortex of cloud particles that
// converges rapidly on the target.  This is done using twelve
// emitters, each spinning about the target, and a local offset
// transform modifier that is animated to fade-out (functioning as
// the shrinking radius of the vortex).
//

// confusion smoke particle
//  (Note: setting "useInvAlpha" changes the openGL blend mode used
//         to render the particles, and is necessary to get a non-
//         glowy particle.  Hopefully in the future the blend mode
//         can be specified explicity.)
datablock ParticleData(AitM_ConfusionSmoke_P)
{
  textureName          = %mySpellDataPath @ "/AitM/particles/AitM_darkSmoke";

  dragCoeffiecient     = 0;
  gravityCoefficient   = -0.1;

  inheritedVelFactor   = 0.00;
  lifetimeMS           = 1000;
  lifetimeVarianceMS   = 200;
  useInvAlpha          = true;
  spinRandomMin        = -150.0;
  spinRandomMax        = -100.0;

  colors[0]            = "0.0 0.0 0.0 0.0";
  colors[1]            = "0.5 0.5 0.5 0.8";
  colors[2]            = "0.5 0.5 0.5 0.8";
  colors[3]            = "0.0 0.0 0.0 0.0";

  sizes[0]             = 4.0;
  sizes[1]             = 3.0;
  sizes[2]             = 2.0;
  sizes[3]             = 1.5;

  times[0]             = 0.0;
  times[1]             = 0.2;
  times[2]             = 0.6;
  times[3]             = 1.0;

  constrainPos = !$UseEmitterTransform;
};

// confusion cloud vortex emitter
datablock ParticleEmitterData(AitM_ConfusionCloudVortex_E)
{
  ejectionOffset        = 0;
  ejectionPeriodMS      = 50;
  periodVarianceMS      = 10;
  ejectionVelocity      = 4.0;
  velocityVariance      = 1.0;  
  particles             = "AitM_ConfusionSmoke_P";
  fadeColor = true;
  fadeSize  = true;
  useEmitterTransform = $UseEmitterTransform;
};

// confusion cloud vortex offset
//  -- this defines the initial radius of the vortex
//  -- timing parameters fade the radius to 0, shrinking the vortex
datablock afxXM_LocalOffsetData(AitM_ConfusionCloudVortex_offset_XM)
{
  localOffset = "45 0 0";

  delay = 0.0;
  lifetime = 0;
  fadeOutTime = 2.0;
};

// confusion cloud vortex spins
datablock afxXM_SpinData(AitM_ConfusionCloudVortex_spin_1_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 0;
  spinAngleVariance = 10;
  spinRate  = -180;
};
datablock afxXM_SpinData(AitM_ConfusionCloudVortex_spin_2_XM : AitM_ConfusionCloudVortex_spin_1_XM)
{
  spinAngle = 30;
};
datablock afxXM_SpinData(AitM_ConfusionCloudVortex_spin_3_XM : AitM_ConfusionCloudVortex_spin_1_XM)
{
  spinAngle = 60;
};
datablock afxXM_SpinData(AitM_ConfusionCloudVortex_spin_4_XM : AitM_ConfusionCloudVortex_spin_1_XM)
{
  spinAngle = 90;
};
datablock afxXM_SpinData(AitM_ConfusionCloudVortex_spin_5_XM : AitM_ConfusionCloudVortex_spin_1_XM)
{
  spinAngle = 120;
};
datablock afxXM_SpinData(AitM_ConfusionCloudVortex_spin_6_XM : AitM_ConfusionCloudVortex_spin_1_XM)
{
  spinAngle = 150;
};
datablock afxXM_SpinData(AitM_ConfusionCloudVortex_spin_7_XM : AitM_ConfusionCloudVortex_spin_1_XM)
{
  spinAngle = 180;
};
datablock afxXM_SpinData(AitM_ConfusionCloudVortex_spin_8_XM : AitM_ConfusionCloudVortex_spin_1_XM)
{
  spinAngle = 210;
};
datablock afxXM_SpinData(AitM_ConfusionCloudVortex_spin_9_XM : AitM_ConfusionCloudVortex_spin_1_XM)
{
  spinAngle = 240;
};
datablock afxXM_SpinData(AitM_ConfusionCloudVortex_spin_10_XM : AitM_ConfusionCloudVortex_spin_1_XM)
{
  spinAngle = 270;
};
datablock afxXM_SpinData(AitM_ConfusionCloudVortex_spin_11_XM : AitM_ConfusionCloudVortex_spin_1_XM)
{
  spinAngle = 300;
};
datablock afxXM_SpinData(AitM_ConfusionCloudVortex_spin_12_XM : AitM_ConfusionCloudVortex_spin_1_XM)
{
  spinAngle = 330;
};

// confusion cloud vortex ground transform
datablock afxXM_GroundConformData(AitM_ConfusionCloudVortex_ground_XM)
{
  height = 0.01;
};

// confusion cloud vortexes
datablock afxEffectWrapperData(AitM_ConfusionCloudVortex_1_EW)
{
  effect = AitM_ConfusionCloudVortex_E;
  posConstraint = "impactedObject";
  lifetime = 2.0;
  delay = 2.5;
  fadeInTime = 1.8;
  fadeOutTime = 0.0;

  xfmModifiers[0] = AitM_ConfusionCloudVortex_spin_1_XM;
  xfmModifiers[1] = AitM_ConfusionCloudVortex_offset_XM;
  xfmModifiers[2] = AitM_ConfusionCloudVortex_ground_XM;
};
datablock afxEffectWrapperData(AitM_ConfusionCloudVortex_2_EW : AitM_ConfusionCloudVortex_1_EW)
{
  xfmModifiers[0] = AitM_ConfusionCloudVortex_spin_2_XM;
};
datablock afxEffectWrapperData(AitM_ConfusionCloudVortex_3_EW : AitM_ConfusionCloudVortex_1_EW)
{
  xfmModifiers[0] = AitM_ConfusionCloudVortex_spin_3_XM;
};
datablock afxEffectWrapperData(AitM_ConfusionCloudVortex_4_EW : AitM_ConfusionCloudVortex_1_EW)
{
  xfmModifiers[0] = AitM_ConfusionCloudVortex_spin_4_XM;
};
datablock afxEffectWrapperData(AitM_ConfusionCloudVortex_5_EW : AitM_ConfusionCloudVortex_1_EW)
{
  xfmModifiers[0] = AitM_ConfusionCloudVortex_spin_5_XM;
};
datablock afxEffectWrapperData(AitM_ConfusionCloudVortex_6_EW : AitM_ConfusionCloudVortex_1_EW)
{
  xfmModifiers[0] = AitM_ConfusionCloudVortex_spin_6_XM;
};
datablock afxEffectWrapperData(AitM_ConfusionCloudVortex_7_EW : AitM_ConfusionCloudVortex_1_EW)
{
  xfmModifiers[0] = AitM_ConfusionCloudVortex_spin_7_XM;
};
datablock afxEffectWrapperData(AitM_ConfusionCloudVortex_8_EW : AitM_ConfusionCloudVortex_1_EW)
{
  xfmModifiers[0] = AitM_ConfusionCloudVortex_spin_8_XM;
};
datablock afxEffectWrapperData(AitM_ConfusionCloudVortex_9_EW : AitM_ConfusionCloudVortex_1_EW)
{
  xfmModifiers[0] = AitM_ConfusionCloudVortex_spin_9_XM;
};
datablock afxEffectWrapperData(AitM_ConfusionCloudVortex_10_EW : AitM_ConfusionCloudVortex_1_EW)
{
  xfmModifiers[0] = AitM_ConfusionCloudVortex_spin_10_XM;
};
datablock afxEffectWrapperData(AitM_ConfusionCloudVortex_11_EW : AitM_ConfusionCloudVortex_1_EW)
{
  xfmModifiers[0] = AitM_ConfusionCloudVortex_spin_11_XM;
};
datablock afxEffectWrapperData(AitM_ConfusionCloudVortex_12_EW : AitM_ConfusionCloudVortex_1_EW)
{
  xfmModifiers[0] = AitM_ConfusionCloudVortex_spin_12_XM;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// TARGET CONFUSION CLOUD

//
// The confusion cloud vortex creates a new dark, smoky cloud that
// is constrained to the caster.  A light flash coincides with its
// creation.
//

// confusion cloud emitter (disc)
datablock afxParticleEmitterDiscData(AitM_ConfusionCloud_E) // TGEA
{
  ejectionPeriodMS      = 10;
  periodVarianceMS      = 3;
  ejectionVelocity      = 2.0;
  velocityVariance      = 0.5;
  particles             = "AitM_ConfusionSmoke_P";
  fadeColor             = true;

  // TGE emitterType = "disc";
  vector = "0 0 1";
  radiusMin = 1.5;
  radiusMax = 2.0;

  useEmitterTransform = $UseEmitterTransform;
};

// confusion cloud
datablock afxEffectWrapperData(AitM_ConfusionCloud_EW)
{
  effect = AitM_ConfusionCloud_E;
  posConstraint = "impactedObject";
  lifetime = 12.0;
  delay = 4.0;
  fadeInTime = 1.0;
  fadeOutTime = 3.0;
  forcedBBox = "-1.0 -1.0 0.0 1.0 1.0 1.5";
  updateForcedBBox = true;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// TARGET CRAZY MASKS, GHOSTS & EYES

//
// During the impact phase two mask models swirl about the target,
// followed by two ghostly copies.  At terrifying moments the masks
// suddenly turn to the camera, and their eyes appear!  These shapes
// are modeled after the face used during casting.
//
// The masks are animated entirely with transform modifiers.  Each is
// moved beside the target, spun around it, and made to hover up and 
// down with a cycling path.  Three aim constriants with unique
// timing parameters are faded in and out, causing the masks to
// suddenly look at the camera three times.  In conjuction three
// scale constraints enlarge the masks during the aims.
//
// In order for the masks to work properly with the aim constraints,
// they were exported facing down the positive Y axis.  However, in
// order to face the target when the aim constraints are inactive,
// a rotation of -90 along the Z axis is necessary.  This is why
// there are two spins for each mask: one to orient the mask to face
// the target, another to rotate it about the target.
//
// Only a subset of the masks modifiers are used to animate the ghosts,
// as these do not aim or scale.
//
// The eyes, however, reuse all the modifiers so that they will be
// placed correctly.  Since there is no way a modifier can modify
// material attributes through animated fading, the eye models had to
// have animated visibility.  They could probably have been included
// in the mask files...
//

// crazy mask A
//  -- the mask's texture has transparency, so "useVertexAlpha" must be set
datablock afxModelData(AitM_TargetCrazyMaskA_CE)
{
  shapeFile = %mySpellDataPath @ "/AitM/models/AitM_crazyMaskA.dts";
  forceOnMaterialFlags = $MaterialFlags::SelfIlluminating;
  useVertexAlpha = true; // TGE (ignored by TGEA)
  alphaMult = 0.6;
};
// crazy mask B
datablock afxModelData(AitM_TargetCrazyMaskB_CE : AitM_TargetCrazyMaskA_CE)
{
  shapeFile = %mySpellDataPath @ "/AitM/models/AitM_crazyMaskB.dts";
};

// crazy mask ghosts A
datablock afxModelData(AitM_TargetCrazyMaskA_ghost1_CE : AitM_TargetCrazyMaskA_CE)
{
  alphaMult = 0.3;
};
datablock afxModelData(AitM_TargetCrazyMaskA_ghost2_CE : AitM_TargetCrazyMaskA_CE)
{
  alphaMult = 0.15;
};

// crazy mask ghosts B
datablock afxModelData(AitM_TargetCrazyMaskB_ghost1_CE : AitM_TargetCrazyMaskB_CE)
{
  alphaMult = 0.3;
};
datablock afxModelData(AitM_TargetCrazyMaskB_ghost2_CE : AitM_TargetCrazyMaskB_CE)
{
  alphaMult = 0.15;
};


// crazy mask offset, lifting the mask off the ground and beside the target
datablock afxXM_LocalOffsetData(AitM_TargetCrazyMask_offset_XM)
{
  localOffset = "3.5 0 3";
};

// crazy mask inital spin, reorienting the mask to face the target
datablock afxXM_SpinData(AitM_TargetCrazyMask_spin_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = -90.0;
  spinRate  = 0;
};

// crazy mask spin A, positive spin about the target
datablock afxXM_SpinData(AitM_TargetCrazyMaskA_spin_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 0.0;
  spinRate  = 90;
};
// crazy mask spin A, negative spin about the target
datablock afxXM_SpinData(AitM_TargetCrazyMaskB_spin_XM : AitM_TargetCrazyMaskA_spin_XM)
{
  spinRate  = -90;
};

// crazy mask hover path A
datablock afxPathData(AitM_TargetCrazyMaskA_Path)
{
  points = "0 0 -1.3" SPC
           "0 0  0.8" SPC
           "0 0 -0.6" SPC
           "0 0  0.4" SPC
           "0 0 -0.8" SPC
           "0 0 -1.3";
  lifetime = 3.0;
  loop = cycle;
};
//
datablock afxXM_PathConformData(AitM_TargetCrazyMaskA_path_XM)
{
  paths = "AitM_TargetCrazyMaskA_Path";
};

// crazy mask hover path B
datablock afxPathData(AitM_TargetCrazyMaskB_Path)
{
  points = "0 0  1.0" SPC
           "0 0 -0.4" SPC
           "0 0  0.7" SPC
           "0 0 -1.4" SPC
           "0 0  0.0" SPC
           "0 0  1.0";
  lifetime = 3.0;
  loop = cycle;
};
//
datablock afxXM_PathConformData(AitM_TargetCrazyMaskB_path_XM)
{
  paths = "AitM_TargetCrazyMaskB_Path";
};

// crazy mask animated aim constraints
datablock afxXM_AimData(AitM_TargetCrazyMask_aim1_XM)
{
  aimZOnly = false;
  delay = 2.0;
  lifetime = 2.0;
  fadeInTime = 0.5;
  fadeOutTime = 0.5;
};
//
datablock afxXM_AimData(AitM_TargetCrazyMask_aim2_XM : AitM_TargetCrazyMask_aim1_XM)
{
  delay = 7.0;
};
//
datablock afxXM_AimData(AitM_TargetCrazyMask_aim3_XM : AitM_TargetCrazyMask_aim1_XM)
{
  delay = 11.0;
};

// crazy mask animated scale constraints, animated to match the aims
datablock afxXM_ScaleData(AitM_TargetCrazyMask_scale1_XM)
{
  scale = "2 2 2";

  delay = 2.0;
  lifetime = 2.0;
  fadeInTime = 0.5;
  fadeOutTime = 0.5;
};
//
datablock afxXM_ScaleData(AitM_TargetCrazyMask_scale2_XM : AitM_TargetCrazyMask_scale1_XM)
{
  delay = 7.0;
};
//
datablock afxXM_ScaleData(AitM_TargetCrazyMask_scale3_XM : AitM_TargetCrazyMask_scale1_XM)
{
  delay = 11.0;
};

// crazy mask A
%AitM_TargetMaskScale = 2.0;
datablock afxEffectWrapperData(AitM_TargetCrazyMaskA_EW)
{
  effect = AitM_TargetCrazyMaskA_CE;
  effectName = "TargetCrazyMaskA";
  isConstraintSrc = true;

  posConstraint = "impactedObject";
  lifetime = 11.0;
  delay = 6.0;
  fadeInTime = 0.5;
  fadeOutTime = 1.0;

  scaleFactor = %AitM_TargetMaskScale;

  posConstraint2 = "camera"; // aim

  xfmModifiers[0] = AitM_TargetCrazyMaskA_spin_XM;
  xfmModifiers[1] = AitM_TargetCrazyMask_offset_XM;
  xfmModifiers[2] = AitM_TargetCrazyMask_spin_XM;
  xfmModifiers[3] = AitM_TargetCrazyMaskA_path_XM;
  xfmModifiers[4] = AitM_TargetCrazyMask_aim1_XM;
  xfmModifiers[5] = AitM_TargetCrazyMask_aim2_XM;
  xfmModifiers[6] = AitM_TargetCrazyMask_aim3_XM;
  xfmModifiers[7] = AitM_TargetCrazyMask_scale1_XM;
  xfmModifiers[8] = AitM_TargetCrazyMask_scale2_XM;
  xfmModifiers[9] = AitM_TargetCrazyMask_scale3_XM;
};
// crazy mask B
//datablock afxEffectWrapperData(AitM_TargetCrazyMaskB_EW : AitM_TargetCrazyMaskA_EW)
//
//  effect = AitM_TargetCrazyMaskB_CE;
//  effectName = "TargetCrazyMaskB";
//  isConstraintSrc = true;
//  delay = 6.85;
//  xfmModifiers[0] = AitM_TargetCrazyMaskB_spin_XM;
//  xfmModifiers[3] = AitM_TargetCrazyMaskB_path_XM;
//};
datablock afxEffectWrapperData(AitM_TargetCrazyMaskB_EW)
{
  effect = AitM_TargetCrazyMaskB_CE;
  effectName = "TargetCrazyMaskB";
  isConstraintSrc = true;

  posConstraint = "impactedObject";
  lifetime = 11.0;
  delay = 6.85;
  fadeInTime = 0.5;
  fadeOutTime = 1.0;

  scaleFactor = %AitM_TargetMaskScale;

  posConstraint2 = "camera"; // aim

  xfmModifiers[0] = AitM_TargetCrazyMaskB_spin_XM;
  xfmModifiers[1] = AitM_TargetCrazyMask_offset_XM;
  xfmModifiers[2] = AitM_TargetCrazyMask_spin_XM;
  xfmModifiers[3] = AitM_TargetCrazyMaskB_path_XM;
  xfmModifiers[4] = AitM_TargetCrazyMask_aim1_XM;
  xfmModifiers[5] = AitM_TargetCrazyMask_aim2_XM;
  xfmModifiers[6] = AitM_TargetCrazyMask_aim3_XM;
  xfmModifiers[7] = AitM_TargetCrazyMask_scale1_XM;
  xfmModifiers[8] = AitM_TargetCrazyMask_scale2_XM;
  xfmModifiers[9] = AitM_TargetCrazyMask_scale3_XM;
};

// crazy mask ghost A 1
datablock afxEffectWrapperData(AitM_TargetCrazyMaskA_ghost1_EW)
{
  effect = AitM_TargetCrazyMaskA_ghost1_CE;
  posConstraint = "impactedObject";
  lifetime = 11.0;
  delay = 6.0+0.2;
  fadeInTime = 0.5;
  fadeOutTime = 1.0;

  scaleFactor = %AitM_TargetMaskScale;

  xfmModifiers[0] = AitM_TargetCrazyMaskA_spin_XM;
  xfmModifiers[1] = AitM_TargetCrazyMask_offset_XM;
  xfmModifiers[2] = AitM_TargetCrazyMask_spin_XM;
  xfmModifiers[3] = AitM_TargetCrazyMaskA_path_XM;
};
// crazy mask ghost A 2
datablock afxEffectWrapperData(AitM_TargetCrazyMaskA_ghost2_EW : AitM_TargetCrazyMaskA_ghost1_EW)
{
  effect = AitM_TargetCrazyMaskA_ghost2_CE;
  delay = 6.0+0.5;
};

// crazy mask ghost B 1
datablock afxEffectWrapperData(AitM_TargetCrazyMaskB_ghost1_EW)
{
  effect = AitM_TargetCrazyMaskB_ghost1_CE;
  posConstraint = "impactedObject";
  lifetime = 11.0;
  delay = 6.85+0.2;
  fadeInTime = 0.5;
  fadeOutTime = 1.0;

  scaleFactor = %AitM_TargetMaskScale;

  xfmModifiers[0] = AitM_TargetCrazyMaskB_spin_XM;
  xfmModifiers[1] = AitM_TargetCrazyMask_offset_XM;
  xfmModifiers[2] = AitM_TargetCrazyMask_spin_XM;
  xfmModifiers[3] = AitM_TargetCrazyMaskB_path_XM;
};
// crazy mask ghost B 2
datablock afxEffectWrapperData(AitM_TargetCrazyMaskB_ghost2_EW : AitM_TargetCrazyMaskB_ghost1_EW)
{
  effect = AitM_TargetCrazyMaskB_ghost2_CE;
  delay = 6.85+0.5;
};

// crazy mask eyes
//  -- visibility is animated in the "stare" sequence
datablock afxModelData(AitM_TargetCrazyMaskEyes_CE)
{
  shapeFile = %mySpellDataPath @ "/AitM/models/AitM_crazyMaskEyes.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  sequence = "stare";
  useVertexAlpha = true; // TGE (ignored by TGEA)
  remapTextureTags = "AitM_casterEye.png:AitM_MaskEyes"; // TGEA (ignored by TGE)
};

// crazy mask eyes A
datablock afxEffectWrapperData(AitM_TargetCrazyMaskEyesA_EW : AitM_TargetCrazyMaskA_EW)
{
  effect = AitM_TargetCrazyMaskEyes_CE;
  effectName = "";
  isConstraintSrc = false;
};
// crazy mask eyes B
datablock afxEffectWrapperData(AitM_TargetCrazyMaskEyesB_EW : AitM_TargetCrazyMaskB_EW)
{
  effect = AitM_TargetCrazyMaskEyes_CE;
  effectName = "";
  isConstraintSrc = false;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// TARGET CRAZY MASK SPARKLES

//
// From the tip of each hair of the crazy masks, particle sparkles
// are emitted.  This is done by reusing the essential tranform
// modifiers from the mask wrappers, and adding a local transform
// for correct placement.
//
// Note however that it is impossible for the emitters to remain
// properly placed when the mask's aim and scale constraints become
// active.  Basically this is because each emitter is its own origin,
// and therefore rotations and scales don't really have much meaning.
// For this to work properly, the emitters would need to be
// constrained to mount nodes placed at the hair tips in each mask
// geometry.  Perhaps this feature will be included in future
// revisions...???  ;)
//

// crazy mask sparkle particles A-C
datablock ParticleData(AitM_MaskSparkle_A_P)
{
   textureName          = %mySpellDataPath @ "/AitM/particles/AitM_sparkle";
   dragCoeffiecient     = 0;
   gravityCoefficient   = 0;
   windCoefficient      = 0;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 1000;
   lifetimeVarianceMS   = 200;
   useInvAlpha          = false;
   spinRandomMin        = 0.0;
   spinRandomMax        = 0.0;
   colors[0]            = "1 1 1 1";
   colors[1]            = "0.2 0 1 1";
   colors[2]            = "0 0 0 0";
   sizes[0]             = 0.6; 
   sizes[1]             = 0.4;
   sizes[2]             = 0.2;
   times[0]             = 0.0;
   times[1]             = 0.5;
   times[2]             = 1.0;
};
//
datablock ParticleData(AitM_MaskSparkle_B_P : AitM_MaskSparkle_A_P)
{
  colors[1]            = "0.5 0 1 1";
};
//
datablock ParticleData(AitM_MaskSparkle_C_P : AitM_MaskSparkle_A_P)
{
  colors[1]            = "0.8 0 1 1";
};

// crazy mask sparkle emitter
datablock ParticleEmitterData(AitM_MaskSparkle_E)
{
  ejectionPeriodMS      = 50;
  periodVarianceMS      = 15;
  ejectionVelocity      = 0.5;
  velocityVariance      = 0.2;
  thetaMin              = 0.0;
  thetaMax              = 180.0;
  particles             = "AitM_MaskSparkle_A_P AitM_MaskSparkle_B_P AitM_MaskSparkle_C_P";
  fadeColor             = true;
};


// crazy mask sparkle offsets
//  -- each corresponds to the tip of one of the mask's hairs (six total)
//datablock afxXM_LocalOffsetData(AitM_MaskSparkle_Offset_1_XM)
//{
//  localOffset = -0.357*%AitM_TargetMaskScale SPC -0.113*%AitM_TargetMaskScale SPC -0.043*%AitM_TargetMaskScale;
//};
//datablock afxXM_LocalOffsetData(AitM_MaskSparkle_Offset_2_XM)
//{
//  localOffset = -0.317*%AitM_TargetMaskScale SPC -0.119*%AitM_TargetMaskScale SPC 0.214*%AitM_TargetMaskScale;
//};
//datablock afxXM_LocalOffsetData(AitM_MaskSparkle_Offset_3_XM)
//{
//  localOffset = -0.105*%AitM_TargetMaskScale SPC -0.115*%AitM_TargetMaskScale SPC 0.375*%AitM_TargetMaskScale;
//};
//datablock afxXM_LocalOffsetData(AitM_MaskSparkle_Offset_4_XM)
//{
//  localOffset = 0.105*%AitM_TargetMaskScale SPC -0.115*%AitM_TargetMaskScale SPC 0.375*%AitM_TargetMaskScale;
//};
//datablock afxXM_LocalOffsetData(AitM_MaskSparkle_Offset_5_XM)
//{
//  localOffset = 0.317*%AitM_TargetMaskScale SPC -0.119*%AitM_TargetMaskScale SPC 0.214*%AitM_TargetMaskScale;
//};
//datablock afxXM_LocalOffsetData(AitM_MaskSparkle_Offset_6_XM)
//{
//  localOffset = 0.357*%AitM_TargetMaskScale SPC -0.113*%AitM_TargetMaskScale SPC -0.043*%AitM_TargetMaskScale;
//};

// crazy mask sparkles A 1
datablock afxEffectWrapperData(AitM_TargetMaskSparkleA_1_EW)
{
  effect = AitM_MaskSparkle_E;
  //posConstraint = "impactedObject";    
  posConstraint = "#effect.TargetCrazyMaskA.Mount_hair1";
  lifetime = 11.0;
  delay = 6.0;
  fadeInTime = 0.5;
  fadeOutTime = 1.0;

  //scaleFactor = %AitM_TargetMaskScale;

  //xfmModifiers[0] = AitM_TargetCrazyMaskA_spin_XM;
  //xfmModifiers[1] = AitM_TargetCrazyMask_offset_XM;
  //xfmModifiers[2] = AitM_TargetCrazyMask_spin_XM;
  //xfmModifiers[3] = AitM_TargetCrazyMaskA_path_XM;
  //xfmModifiers[4] = AitM_MaskSparkle_Offset_1_XM;
};
// crazy mask sparkles A 2
datablock afxEffectWrapperData(AitM_TargetMaskSparkleA_2_EW : AitM_TargetMaskSparkleA_1_EW)
{
  posConstraint = "#effect.TargetCrazyMaskA.Mount_hair2";
};
// crazy mask sparkles A 3
datablock afxEffectWrapperData(AitM_TargetMaskSparkleA_3_EW : AitM_TargetMaskSparkleA_1_EW)
{
  posConstraint = "#effect.TargetCrazyMaskA.Mount_hair3";
};
// crazy mask sparkles A 4
datablock afxEffectWrapperData(AitM_TargetMaskSparkleA_4_EW : AitM_TargetMaskSparkleA_1_EW)
{
  posConstraint = "#effect.TargetCrazyMaskA.Mount_hair4";
};
// crazy mask sparkles A 5
datablock afxEffectWrapperData(AitM_TargetMaskSparkleA_5_EW : AitM_TargetMaskSparkleA_1_EW)
{
  posConstraint = "#effect.TargetCrazyMaskA.Mount_hair5";
};
// crazy mask sparkles A 6
datablock afxEffectWrapperData(AitM_TargetMaskSparkleA_6_EW : AitM_TargetMaskSparkleA_1_EW)
{
  posConstraint = "#effect.TargetCrazyMaskA.Mount_hair6";
};

// crazy mask sparkles B 1
datablock afxEffectWrapperData(AitM_TargetMaskSparkleB_1_EW )// : AitM_TargetMaskSparkleA_1_EW)
{
  posConstraint = "#effect.TargetCrazyMaskB.Mount_hair1";
  delay = 6.85;
  //xfmModifiers[0] = AitM_TargetCrazyMaskB_spin_XM;
  //xfmModifiers[3] = AitM_TargetCrazyMaskB_path_XM;

  effect = AitM_MaskSparkle_E;
  lifetime = 11.0;
  fadeInTime = 0.5;
  fadeOutTime = 1.0;
};
// crazy mask sparkles B 2
datablock afxEffectWrapperData(AitM_TargetMaskSparkleB_2_EW : AitM_TargetMaskSparkleB_1_EW)
{
  posConstraint = "#effect.TargetCrazyMaskB.Mount_hair2";
};
// crazy mask sparkles B 3
datablock afxEffectWrapperData(AitM_TargetMaskSparkleB_3_EW : AitM_TargetMaskSparkleB_1_EW)
{
  posConstraint = "#effect.TargetCrazyMaskB.Mount_hair3";
};
// crazy mask sparkles B 4
datablock afxEffectWrapperData(AitM_TargetMaskSparkleB_4_EW : AitM_TargetMaskSparkleB_1_EW)
{
  posConstraint = "#effect.TargetCrazyMaskB.Mount_hair4";
};
// crazy mask sparkles B 5
datablock afxEffectWrapperData(AitM_TargetMaskSparkleB_5_EW : AitM_TargetMaskSparkleB_1_EW)
{
  posConstraint = "#effect.TargetCrazyMaskB.Mount_hair5";
};
// crazy mask sparkles B 6
datablock afxEffectWrapperData(AitM_TargetMaskSparkleB_6_EW : AitM_TargetMaskSparkleB_1_EW)
{
  posConstraint = "#effect.TargetCrazyMaskB.Mount_hair6";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// TARGET SPARKLES

//
// The casting sparkles are reused here during the impact phase, but
// with new spins and paths to create a more hectic appearance.
//

// target sparkle spin, positive
datablock afxXM_SpinData(AitM_TargetSparkle_spin1_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 0.0;
  spinRate  = 360;
};
// target sparkle spin, negative
datablock afxXM_SpinData(AitM_TargetSparkle_spin2_XM : AitM_TargetSparkle_spin1_XM)
{
  spinRate  = -360;
};

// target sparkle path, cycling
datablock afxPathData(AitM_TargetSparkle_Path)
{
  points = "0 0 -0.7" SPC
           "0 0  1.5" SPC
           "0 0 -0.7";
  lifetime = 2.3;
  loop = cycle;
};
//
datablock afxXM_PathConformData(AitM_TargetSparkle_path_XM)
{
  paths = "AitM_TargetSparkle_Path";
};

// target sparkles, 1-20
//  -- variables are used to control the delay of these sparkles, and
//      the amount of time between their appearances
$AitM_TargetSparkle_delay = 4.0;
$AitM_TargetSparkle_delay_delta = 0.2;
datablock afxEffectWrapperData(AitM_TargetSparkle_1_EW)
{
  effect = AitM_Sparkle_Big_E;
  posConstraint = "impactedObject";

  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(1-1));
  fadeInTime  = 0.2;
  fadeOutTime = 0.2;
  lifetime = 10.0;

  xfmModifiers[0] = AitM_TargetSparkle_spin1_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_1_XM;
  xfmModifiers[2] = AitM_TargetSparkle_path_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_2_EW : AitM_TargetSparkle_1_EW)
{
  effect = AitM_Sparkle_Big_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(2-1));
  xfmModifiers[0] = AitM_TargetSparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_2_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_3_EW : AitM_TargetSparkle_1_EW)
{
  effect = AitM_Sparkle_Small_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(3-1));
  xfmModifiers[1] = AitM_Sparkle_Offset_3_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_4_EW : AitM_TargetSparkle_1_EW)
{
  effect = AitM_Sparkle_Medium_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(4-1));
  xfmModifiers[0] = AitM_TargetSparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_4_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_5_EW : AitM_TargetSparkle_1_EW)
{
  effect = AitM_Sparkle_Small_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(5-1));
  xfmModifiers[1] = AitM_Sparkle_Offset_5_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_6_EW : AitM_TargetSparkle_1_EW)
{
  effect = AitM_Sparkle_Big_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(6-1));
  xfmModifiers[0] = AitM_TargetSparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_6_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_7_EW : AitM_TargetSparkle_1_EW)
{ 
  effect = AitM_Sparkle_Medium_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(7-1));
  xfmModifiers[1] = AitM_Sparkle_Offset_7_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_8_EW : AitM_TargetSparkle_1_EW)
{
  effect = AitM_Sparkle_Medium_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(8-1));
  xfmModifiers[0] = AitM_TargetSparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_8_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_9_EW : AitM_TargetSparkle_1_EW)
{
  effect = AitM_Sparkle_Big_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(9-1));
  xfmModifiers[1] = AitM_Sparkle_Offset_9_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_10_EW : AitM_TargetSparkle_1_EW)
{
  effect = AitM_Sparkle_Small_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(10-1));
  xfmModifiers[0] = AitM_TargetSparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_10_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_11_EW : AitM_TargetSparkle_1_EW)
{
  effect = AitM_Sparkle_Big_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(11-1));
  xfmModifiers[1] = AitM_Sparkle_Offset_11_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_12_EW : AitM_TargetSparkle_1_EW)
{
  effect = AitM_Sparkle_Big_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(12-1));
  xfmModifiers[0] = AitM_TargetSparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_12_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_13_EW : AitM_TargetSparkle_1_EW)
{
  effect = AitM_Sparkle_Small_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(13-1));
  xfmModifiers[1] = AitM_Sparkle_Offset_13_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_14_EW : AitM_TargetSparkle_1_EW)
{
  effect = AitM_Sparkle_Medium_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(14-1));
  xfmModifiers[0] = AitM_TargetSparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_14_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_15_EW : AitM_TargetSparkle_1_EW)
{
  effect = AitM_Sparkle_Small_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(15-1));
  xfmModifiers[1] = AitM_Sparkle_Offset_15_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_16_EW : AitM_TargetSparkle_1_EW)
{
  effect = AitM_Sparkle_Big_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(16-1));
  xfmModifiers[0] = AitM_TargetSparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_16_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_17_EW : AitM_TargetSparkle_1_EW)
{ 
  effect = AitM_Sparkle_Medium_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(17-1));
  xfmModifiers[1] = AitM_Sparkle_Offset_17_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_18_EW : AitM_TargetSparkle_1_EW)
{
  effect = AitM_Sparkle_Medium_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(18-1));
  xfmModifiers[0] = AitM_TargetSparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_18_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_19_EW : AitM_TargetSparkle_1_EW)
{
  effect = AitM_Sparkle_Big_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(19-1));
  xfmModifiers[1] = AitM_Sparkle_Offset_19_XM;
};
datablock afxEffectWrapperData(AitM_TargetSparkle_20_EW : AitM_TargetSparkle_1_EW)
{
  effect = AitM_Sparkle_Small_E;
  delay = $AitM_TargetSparkle_delay + ($AitM_TargetSparkle_delay_delta*(20-1));
  xfmModifiers[0] = AitM_TargetSparkle_spin2_XM;
  xfmModifiers[1] = AitM_Sparkle_Offset_20_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/aitm_lighting_tge_sub.cs");
    exec("./audio/aitm_audio_sub.cs");
  case "TGEA":
    exec("./lighting/aitm_lighting_tgea_sub.cs");
    exec("./audio/aitm_audio_sub.cs");
  case "T3D":
    exec("./lighting/aitm_lighting_t3d_sub.cs");
    exec("./audio/aitm_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// ARCANE IN THE MEMBRANE SPELL
//

datablock afxMagicSpellData(ArcaneMembraneSpell)
{
  castingDur = 4.0;
  //lingerDur = 5.0;

    // spellcaster animation //
  addCastingEffect = AitM_Casting_Clip_EW;
    // casting crazy face //
  addCastingEffect = AitM_CasterCrazyHead_EW;
  addCastingEffect = AitM_CasterCrazyEye_RT_EW;
  addCastingEffect = AitM_CasterCrazyEye_LF_EW;
  addCastingEffect = AitM_CasterCrazyTeeth_EW;
    // casting sparkles //
  addCastingEffect = AitM_Sparkle_1_EW;
  addCastingEffect = AitM_Sparkle_2_EW;
  addCastingEffect = AitM_Sparkle_3_EW;
  addCastingEffect = AitM_Sparkle_4_EW;
  addCastingEffect = AitM_Sparkle_5_EW;
  addCastingEffect = AitM_Sparkle_6_EW;
  addCastingEffect = AitM_Sparkle_7_EW;
  addCastingEffect = AitM_Sparkle_8_EW;
  addCastingEffect = AitM_Sparkle_9_EW;
  addCastingEffect = AitM_Sparkle_10_EW;
  addCastingEffect = AitM_Sparkle_11_EW;
  addCastingEffect = AitM_Sparkle_12_EW;
  addCastingEffect = AitM_Sparkle_13_EW;
  addCastingEffect = AitM_Sparkle_14_EW;
  addCastingEffect = AitM_Sparkle_15_EW;
  addCastingEffect = AitM_Sparkle_16_EW;
  addCastingEffect = AitM_Sparkle_17_EW;
  addCastingEffect = AitM_Sparkle_18_EW;
  addCastingEffect = AitM_Sparkle_19_EW;
  addCastingEffect = AitM_Sparkle_20_EW;
    // casting hand symbols & sparkles //
  addCastingEffect = AitM_CasterSymbols_lf_hand_EW;
  addCastingEffect = AitM_CasterSymbols_rt_hand_EW;
  addCastingEffect = AitM_CasterSparkle_lf_hand_EW;
  addCastingEffect = AitM_CasterSparkle_rt_hand_EW;

    // target symbols //
  addImpactEffect = AitM_TargetSymbols_EW;
    // target zodiacs //
  addImpactEffect = AitM_TargetZodeA_EW;
  addImpactEffect = AitM_TargetZodeB_EW;
  addImpactEffect = AitM_TargetZodeC_EW;
    // target confusion cloud vortex //
  addImpactEffect = AitM_ConfusionCloudVortex_1_EW;
  addImpactEffect = AitM_ConfusionCloudVortex_2_EW;
  addImpactEffect = AitM_ConfusionCloudVortex_3_EW;
  addImpactEffect = AitM_ConfusionCloudVortex_4_EW;
  addImpactEffect = AitM_ConfusionCloudVortex_5_EW;
  addImpactEffect = AitM_ConfusionCloudVortex_6_EW;
  addImpactEffect = AitM_ConfusionCloudVortex_7_EW;
  addImpactEffect = AitM_ConfusionCloudVortex_8_EW;
  addImpactEffect = AitM_ConfusionCloudVortex_9_EW;
  addImpactEffect = AitM_ConfusionCloudVortex_10_EW;
  addImpactEffect = AitM_ConfusionCloudVortex_11_EW;
  addImpactEffect = AitM_ConfusionCloudVortex_12_EW;
    // target confusion cloud //
  addImpactEffect = AitM_ConfusionCloud_EW;
    // target crazy masks, ghosts & eyes //
  addImpactEffect = AitM_TargetCrazyMaskA_EW;
  addImpactEffect = AitM_TargetCrazyMaskB_EW;
  addImpactEffect = AitM_TargetCrazyMaskEyesA_EW;
  addImpactEffect = AitM_TargetCrazyMaskEyesB_EW;
  addImpactEffect = AitM_TargetCrazyMaskA_ghost1_EW;
  addImpactEffect = AitM_TargetCrazyMaskA_ghost2_EW;
  addImpactEffect = AitM_TargetCrazyMaskB_ghost1_EW;
  addImpactEffect = AitM_TargetCrazyMaskB_ghost2_EW;
    // target crazy mask sparkles //
  addImpactEffect = AitM_TargetMaskSparkleA_1_EW;
  addImpactEffect = AitM_TargetMaskSparkleA_2_EW;
  addImpactEffect = AitM_TargetMaskSparkleA_3_EW;
  addImpactEffect = AitM_TargetMaskSparkleA_4_EW;
  addImpactEffect = AitM_TargetMaskSparkleA_5_EW;
  addImpactEffect = AitM_TargetMaskSparkleA_6_EW;
  addImpactEffect = AitM_TargetMaskSparkleB_1_EW;
  addImpactEffect = AitM_TargetMaskSparkleB_2_EW;
  addImpactEffect = AitM_TargetMaskSparkleB_3_EW;
  addImpactEffect = AitM_TargetMaskSparkleB_4_EW;
  addImpactEffect = AitM_TargetMaskSparkleB_5_EW;
  addImpactEffect = AitM_TargetMaskSparkleB_6_EW;

    // target sparkles //
  addImpactEffect = AitM_TargetSparkle_1_EW;
  addImpactEffect = AitM_TargetSparkle_2_EW;
  addImpactEffect = AitM_TargetSparkle_3_EW;
  addImpactEffect = AitM_TargetSparkle_4_EW;
  addImpactEffect = AitM_TargetSparkle_5_EW;
  addImpactEffect = AitM_TargetSparkle_6_EW;
  addImpactEffect = AitM_TargetSparkle_7_EW;
  addImpactEffect = AitM_TargetSparkle_8_EW;
  addImpactEffect = AitM_TargetSparkle_9_EW;
  addImpactEffect = AitM_TargetSparkle_10_EW;
  addImpactEffect = AitM_TargetSparkle_11_EW;
  addImpactEffect = AitM_TargetSparkle_12_EW;
  addImpactEffect = AitM_TargetSparkle_13_EW;
  addImpactEffect = AitM_TargetSparkle_14_EW;
  addImpactEffect = AitM_TargetSparkle_15_EW;
  addImpactEffect = AitM_TargetSparkle_16_EW;
  addImpactEffect = AitM_TargetSparkle_17_EW;
  addImpactEffect = AitM_TargetSparkle_18_EW;
  addImpactEffect = AitM_TargetSparkle_19_EW;
  addImpactEffect = AitM_TargetSparkle_20_EW;
};

// sounds and lights added via sub-script functions //
AitM_add_Lighting_FX(ArcaneMembraneSpell);
AitM_add_Audio_FX(ArcaneMembraneSpell);

datablock afxRPGMagicSpellData(ArcaneMembraneSpell_RPG)
{
  spellName = "Arcane in the Membrane";
  desc = "This circus magic penetrates your skull and sticks in your brain " @ 
         "like bad orcish OPERA causing 15 seconds of confusion." @ 
         "\n" @
         "\nspell design: Matthew Durante" @
         "\nsound effects: Dave Schroeder" @ 
         "\nspell concept: Jeff Faust";
  sourcePack = "Spell Pack 1";   
  iconBitmap = %mySpellDataPath @ "/AitM/icons/aitm";
  target = "enemy";
  range = 75;
  manaCost = 10;
  castingDur = ArcaneMembraneSpell.castingDur;
  canTargetSelf = true;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

if ($spell_reload)
{
  // Removes then adds all reloaded datablocks
  touchDataBlocks();
}
else
{
  // save script filename and data path for reloads
  ArcaneMembraneSpell.scriptFile = $afxAutoloadScriptFile;
  ArcaneMembraneSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(ArcaneMembraneSpell, ArcaneMembraneSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

//UAISK+AFX Interop Changes: Start
function ArcaneMembraneSpell::onImpact(%this, %spell, %caster, %impObj, %impPos, %impNorm)
{
  Parent::onImpact(%this, %spell, %caster, %impObj, %impPos, %impNorm);

  if ($UAISK_Is_Available)
  {
    // cloud gathers around target about 4.3sec after impact and lasts about 14sec
    %delay = 4.3;
    %lifetime = 14;
    %tgt = %spell.target;
    if (%tgt.isBot)
       schedule(%delay*1000, 0, "confuseBot", %tgt, $Sim::Time + %delay + %lifetime, %tgt.detDis);
  }
}
//UAISK+AFX Interop Changes: End
