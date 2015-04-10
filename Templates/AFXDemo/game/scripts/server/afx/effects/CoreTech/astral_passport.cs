
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// ASTRAL PASSPORT SPELL
//
//    Note: Both the disappearance and reappearance effects are in
//          this file. 
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
$spell_reload = isObject(AstralPassportSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = AstralPassportSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GLOBALS

//
// This spell demonstrates a useful way to time and coordinate 
// multiple effects: global variables.  Instead of manually tweaking
// attribute values across many pages of datablocks, one can group
// these values in one area as demonstrated.  In a way, this mimics
// a mini-spreadsheet.
//
// Note also the use of computations instead of constants to arrive
// at a value for some of these variables.
//

// .. Disappearance Effects ......................................//

// time delays for the 10 QuickWave disappearance zodiacs; see 
//  "QUICKWAVE ZODES" section
%AP_QuickWave_disappear_1_delay  = 1.000;
%AP_QuickWave_disappear_2_delay  = 1.156;
%AP_QuickWave_disappear_3_delay  = 1.291;
%AP_QuickWave_disappear_4_delay  = 1.405;
%AP_QuickWave_disappear_5_delay  = 1.498;
%AP_QuickWave_disappear_6_delay  = 1.573;
%AP_QuickWave_disappear_7_delay  = 1.630;
%AP_QuickWave_disappear_8_delay  = 1.669;
%AP_QuickWave_disappear_9_delay  = 1.693;
%AP_QuickWave_disappear_10_delay = 1.700;

// radius values for the 10 QuickWave disappearance zodiacs; see
//  "QUICKWAVE ZODES" section
%AP_QuickWave_disappear_1_radius  = 10.0;
%AP_QuickWave_disappear_2_radius  = 9.007;
%AP_QuickWave_disappear_3_radius  = 8.108;
%AP_QuickWave_disappear_4_radius  = 7.31;
%AP_QuickWave_disappear_5_radius  = 6.619;
%AP_QuickWave_disappear_6_radius  = 6.042;
%AP_QuickWave_disappear_7_radius  = 5.585;
%AP_QuickWave_disappear_8_radius  = 5.254;
%AP_QuickWave_disappear_9_radius  = 5.057;
%AP_QuickWave_disappear_10_radius = 5.0;

// lifetime values for the 10 QuickWave disappearance zodiacs; see
//  "QUICKWAVE ZODES" section
// computation explanation: all these zodiacs must disappear when
//  their radius shrinks to the same size, 3.0.  The equation is:
//     start_radius+(growth_rate*lifetime) = end_radius
//  solving for lifetime:
//     lifetime = (end_radius-start_radius)/growth_rate
//  contants:
//     growth_rate = -25.0
//     end_radius  = 3.0

// 10.0+(-25.0*lifetime) = 3.0
// lifetime = (3.0-10.0)/-25.0

%AP_QuickWave_disappear_1_lifetime  = 0.28; // (3.0-10.0)/-25.0
%AP_QuickWave_disappear_2_lifetime  = 0.24;// (3.0-9.007)/-25.0
%AP_QuickWave_disappear_3_lifetime  = 0.204;// (3.0-8.108)/-25.0
%AP_QuickWave_disappear_4_lifetime  = 0.172;// (3.0-7.31)/-25.0
%AP_QuickWave_disappear_5_lifetime  = 0.145;// (3.0-6.619)/-25.0
%AP_QuickWave_disappear_6_lifetime  = 0.122;// (3.0-6.042)/-25.0
%AP_QuickWave_disappear_7_lifetime  = 0.103;// (3.0-5.585)/-25.0
%AP_QuickWave_disappear_8_lifetime  = 0.090;// (3.0-5.254)/-25.0
%AP_QuickWave_disappear_9_lifetime  = 0.082;// (3.0-5.057)/-25.0
%AP_QuickWave_disappear_10_lifetime = 0.08;// (3.0-5.0)/-25.0

// .. Reappearance Effects .......................................//

// time offset between disappearance and reappearance
////%AP_ReappearTimeOffset = 5.0;
%AP_ReappearTimeOffset = 0.0;

// time delays for the 10 QuickWave reappearance zodiacs; see 
//  "QUICKWAVE ZODES" section
%AP_QuickWave_reappear_1_delay  = 1.000+%AP_ReappearTimeOffset;
%AP_QuickWave_reappear_2_delay  = 1.008+%AP_ReappearTimeOffset;
%AP_QuickWave_reappear_3_delay  = 1.031+%AP_ReappearTimeOffset;
%AP_QuickWave_reappear_4_delay  = 1.071+%AP_ReappearTimeOffset;
%AP_QuickWave_reappear_5_delay  = 1.129+%AP_ReappearTimeOffset;
%AP_QuickWave_reappear_6_delay  = 1.204+%AP_ReappearTimeOffset;
%AP_QuickWave_reappear_7_delay  = 1.298+%AP_ReappearTimeOffset;
%AP_QuickWave_reappear_8_delay  = 1.412+%AP_ReappearTimeOffset;
%AP_QuickWave_reappear_9_delay  = 1.545+%AP_ReappearTimeOffset;
%AP_QuickWave_reappear_10_delay = 1.700+%AP_ReappearTimeOffset;

// lifetime values for the 10 QuickWave reappearance zodiacs; see
//  "QUICKWAVE ZODES" section
%AP_QuickWave_reappear_1_lifetime  = 0.150*0.5;
%AP_QuickWave_reappear_2_lifetime  = 0.151*0.5;
%AP_QuickWave_reappear_3_lifetime  = 0.154*0.5;
%AP_QuickWave_reappear_4_lifetime  = 0.159*0.5;
%AP_QuickWave_reappear_5_lifetime  = 0.166*0.5;
%AP_QuickWave_reappear_6_lifetime  = 0.176*0.5;
%AP_QuickWave_reappear_7_lifetime  = 0.190*0.5;
%AP_QuickWave_reappear_8_lifetime  = 0.206*0.5;
%AP_QuickWave_reappear_9_lifetime  = 0.226*0.5;
%AP_QuickWave_reappear_10_lifetime = 0.250*0.5;


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPELLCASTER ANIMATION

//
// The spellcaster lifts his arms then is raised in a twirling tear
// of space-time as he is stretched and sucked into the beyond below
// him.  At least that's what it looks like.  Upon reappearance,
// the same animation is used but played-back in reverse.  This was
// done in Maya with scaling enabled on the sequence node.
//

// .. Disappearance Effects ......................................//

datablock afxAnimClipData(AP_Casting_Clip_disappear_CE)
{
  clipName = "ap";
  ignoreCorpse = true;
  rate = 1.0;
};
//
datablock afxEffectWrapperData(AP_Casting_Clip_disappear_EW)
{
  effect = AP_Casting_Clip_disappear_CE;
  constraint = "caster";
  lifetime = 4.0;
  delay = 0.0;
  propagateTimeFactor = true;
};

datablock afxAnimLockData(AP_AnimLock_disappear_CE)
{
  priority = 0;
};
//
datablock afxEffectWrapperData(AP_AnimLock_disappear_EW)
{
  effect = AP_AnimLock_disappear_CE;
  delay = 1.0;
  constraint = "caster";
  lifetime = 3.5;
};

// .. Reappearance Effects .......................................//

datablock afxAnimClipData(AP_Casting_Clip_reappear_CE : AP_Casting_Clip_disappear_CE)
{
  rate = -1.0;
};
//
datablock afxEffectWrapperData(AP_Casting_Clip_reappear_EW : AP_Casting_Clip_disappear_EW)
{
  effect = AP_Casting_Clip_reappear_CE;
  delay = %AP_ReappearTimeOffset;
  propagateTimeFactor = true;
};

datablock afxAnimLockData(AP_AnimLock_reappear_CE)
{
  priority = 0;
};
//
datablock afxEffectWrapperData(AP_AnimLock_reappear_EW)
{
  effect = AP_AnimLock_reappear_CE;
  delay = %AP_ReappearTimeOffset;
  constraint = "caster";
  lifetime = 3.5;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// TELEPORT ZODIAC

// this mooring anchors the zodiacs and measures
// their altitudes

datablock afxMooringData(AP_FixedZodeMooring_CE)
{
  networking = $AFX::CLIENT_ONLY;
  displayAxisMarker = false;
};
datablock afxEffectWrapperData(AP_FixedZodeMooring_EW)
{
  effect = AP_FixedZodeMooring_CE;
  posConstraint = caster;
  effectName = "FixedZodeMooring";
  isConstraintSrc = true;
  lifetime = 5.0;
  xfmModifiers[0] = SHARED_AltitudeConform_XM;
};

//
// The main teleport zodiac is first revealed by a white glow zodiac
// when the casting begins.
//

// .. Disappearance Effects ......................................//

// white reveal glow (disappearance)
datablock afxZodiacData(AP_TeleportZodeReveal_disappear_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/AP/zodiacs/AP_teleportZode_reveal.png";
  radius = 1.5;
  startAngle = 45.0; //180*.25
  rotationRate = -180.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(AP_TeleportZodeReveal_disappear_EW)
{
  effect = AP_TeleportZodeReveal_disappear_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  lifetime = 0.75;
  delay = 0.0;
  fadeInTime = 0.5;
  fadeOutTime = 0.25;
};

// teleport zodiac (disappearance)
datablock afxZodiacData(AP_TeleportZode_disappear_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/AP/zodiacs/AP_teleportZode.png";
  radius = 1.5;
  startAngle = 0.0;
  rotationRate = -180.0;
  color = "1.0 1.0 1.0 0.9";
  blend = additive;
};
//
datablock afxEffectWrapperData(AP_TeleportZode_disappear_EW)
{
  effect = AP_TeleportZode_disappear_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  lifetime = 3.3; //2.8;
  delay    = 0.25;  
  fadeInTime  = 0.75;
  fadeOutTime = 0.10;
};

// teleport zodiac (reappearance)
datablock afxEffectWrapperData(AP_TeleportZode_reappear_EW : AP_TeleportZode_disappear_EW)
{
  delay    = 0.25+%AP_ReappearTimeOffset;  
};

// .. Reappearance Effects .......................................//

// white reveal glow (reappearance)
datablock afxEffectWrapperData(AP_TeleportZodeReveal_reappear_EW : AP_TeleportZodeReveal_disappear_EW)
{
  delay = 0.0+%AP_ReappearTimeOffset;
};





//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// QUICKWAVE ZODES

//
// At the beginning of disappearance, 10 quick zodiacs converge on
// the teleport zodiac, causing it to glow (see "TELEPORT ZODIAC
// FLASHES").  Similarly, during reappearance 10 quick zodiacs are
// emitted from the glowing teleport zodiac.  Note that to
// coordinate these many datablocks, global variables have been used
// extensively (see "GLOBALS").
//
// During disappearance, the starting radiuses of the zodiacs 
// progressively get smaller.  Each shrinks with a growthRate of
// -25.0.  Each must disappear when its radius is 3.0, matching the
// size of the teleport zodiac's ring, but the only way to control
// that is through lifetime; the equations used to determine this 
// are explained in "GLOBALS".  These zodiacs don't rotate, but 
// each is given a unique starting rotation to add randomness.
//
// During reappearance, the starting radius is constant at 1.5 (not
// 3.0, because a different zodiac texture is used here requiring
// a different value to match the teleport zodiac ring).  All grow
// at a rate of 25.0.  Their lifetimes grow progressively larger,
// causing each zodiac to grow larger than the previous.  Each is
// given a unique starting rotation to add randomness.
//


// .. Disappearance Effects ......................................//

// quick wave disappear 1
datablock afxZodiacData(AP_QuickWaveZode_disappear_1_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/AP/zodiacs/AP_quickWaveA.png";
  radius = %AP_QuickWave_disappear_1_radius;
  startAngle = 0.0;
  color = "1.0 1.0 1.0 0.5";
  blend = additive;
  growthRate = -25.0;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_disappear_1_EW)
{
  effect = AP_QuickWaveZode_disappear_1_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  lifetime = %AP_QuickWave_disappear_1_lifetime;
  delay    = %AP_QuickWave_disappear_1_delay;  
  fadeInTime  = 0.1;
  fadeOutTime = 0.0;
};

// quick wave disappear 2
datablock afxZodiacData(AP_QuickWaveZode_disappear_2_CE : AP_QuickWaveZode_disappear_1_CE)
{  
  radius = %AP_QuickWave_disappear_2_radius;
  startAngle = -96;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_disappear_2_EW : AP_QuickWaveZode_disappear_1_EW)
{
  effect = AP_QuickWaveZode_disappear_2_CE;
  lifetime = %AP_QuickWave_disappear_2_lifetime;
  delay  = %AP_QuickWave_disappear_2_delay;
};

// quick wave disappear 3
datablock afxZodiacData(AP_QuickWaveZode_disappear_3_CE : AP_QuickWaveZode_disappear_1_CE)
{  
  radius = %AP_QuickWave_disappear_3_radius;
  startAngle = 123;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_disappear_3_EW : AP_QuickWaveZode_disappear_1_EW)
{
  effect = AP_QuickWaveZode_disappear_3_CE;
  lifetime = %AP_QuickWave_disappear_3_lifetime;
  delay  = %AP_QuickWave_disappear_3_delay;
};

// quick wave disappear 4
datablock afxZodiacData(AP_QuickWaveZode_disappear_4_CE : AP_QuickWaveZode_disappear_1_CE)
{  
  radius = %AP_QuickWave_disappear_4_radius;
  startAngle = -11;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_disappear_4_EW : AP_QuickWaveZode_disappear_1_EW)
{
  effect = AP_QuickWaveZode_disappear_4_CE;
  lifetime = %AP_QuickWave_disappear_4_lifetime;
  delay  = %AP_QuickWave_disappear_4_delay;
};

// quick wave disappear 5
datablock afxZodiacData(AP_QuickWaveZode_disappear_5_CE : AP_QuickWaveZode_disappear_1_CE)
{  
  radius = %AP_QuickWave_disappear_5_radius;
  startAngle = 213;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_disappear_5_EW : AP_QuickWaveZode_disappear_1_EW)
{
  effect = AP_QuickWaveZode_disappear_5_CE;
  lifetime = %AP_QuickWave_disappear_5_lifetime;
  delay  = %AP_QuickWave_disappear_5_delay;
};

// quick wave disappear 6
datablock afxZodiacData(AP_QuickWaveZode_disappear_6_CE : AP_QuickWaveZode_disappear_1_CE)
{  
  radius = %AP_QuickWave_disappear_6_radius;
  startAngle = -111;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_disappear_6_EW : AP_QuickWaveZode_disappear_1_EW)
{
  effect = AP_QuickWaveZode_disappear_6_CE;
  lifetime = %AP_QuickWave_disappear_6_lifetime;
  delay  = %AP_QuickWave_disappear_6_delay;
};

// quick wave disappear 7
datablock afxZodiacData(AP_QuickWaveZode_disappear_7_CE : AP_QuickWaveZode_disappear_1_CE)
{  
  radius = %AP_QuickWave_disappear_7_radius;
  startAngle = 23;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_disappear_7_EW : AP_QuickWaveZode_disappear_1_EW)
{
  effect = AP_QuickWaveZode_disappear_7_CE;
  lifetime = %AP_QuickWave_disappear_7_lifetime;
  delay  = %AP_QuickWave_disappear_7_delay;
};

// quick wave disappear 8
datablock afxZodiacData(AP_QuickWaveZode_disappear_8_CE : AP_QuickWaveZode_disappear_1_CE)
{  
  radius = %AP_QuickWave_disappear_8_radius;
  startAngle = 277;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_disappear_8_EW : AP_QuickWaveZode_disappear_1_EW)
{
  effect = AP_QuickWaveZode_disappear_8_CE;
  lifetime = %AP_QuickWave_disappear_8_lifetime;
  delay  = %AP_QuickWave_disappear_8_delay;
};

// quick wave disappear 9
datablock afxZodiacData(AP_QuickWaveZode_disappear_9_CE : AP_QuickWaveZode_disappear_1_CE)
{  
  radius = %AP_QuickWave_disappear_9_radius;
  startAngle = -47;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_disappear_9_EW : AP_QuickWaveZode_disappear_1_EW)
{
  effect = AP_QuickWaveZode_disappear_9_CE;
  lifetime = %AP_QuickWave_disappear_9_lifetime;
  delay  = %AP_QuickWave_disappear_9_delay;
};

// quick wave disappear 10
datablock afxZodiacData(AP_QuickWaveZode_disappear_10_CE : AP_QuickWaveZode_disappear_1_CE)
{  
  radius = %AP_QuickWave_disappear_10_radius;
  startAngle = 136.0;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_disappear_10_EW : AP_QuickWaveZode_disappear_1_EW)
{
  effect = AP_QuickWaveZode_disappear_10_CE;
  lifetime = %AP_QuickWave_disappear_10_lifetime;
  delay  = %AP_QuickWave_disappear_10_delay;
};

// .. Reappearance Effects .......................................//

// quick wave reappear 1
datablock afxZodiacData(AP_QuickWaveZode_reappear_1_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/AP/zodiacs/AP_quickWaveB.png";
  radius = 1.5;
  startAngle = 0.0;
  color = "1.0 1.0 1.0 0.5";
  blend = additive;
  growthRate = 25.0;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_reappear_1_EW)
{
  effect = AP_QuickWaveZode_reappear_1_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  lifetime = %AP_QuickWave_reappear_1_lifetime;
  delay    = %AP_QuickWave_reappear_1_delay;  
  fadeInTime  = 0.05;
  fadeOutTime = 0.10;
};

// quick wave reappear 2
datablock afxZodiacData(AP_QuickWaveZode_reappear_2_CE : AP_QuickWaveZode_reappear_1_CE)
{  
  startAngle = -96;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_reappear_2_EW : AP_QuickWaveZode_reappear_1_EW)
{
  effect   = AP_QuickWaveZode_reappear_2_CE;
  lifetime = %AP_QuickWave_reappear_2_lifetime;
  delay    = %AP_QuickWave_reappear_2_delay;
};

// quick wave reappear 3
datablock afxZodiacData(AP_QuickWaveZode_reappear_3_CE : AP_QuickWaveZode_reappear_1_CE)
{  
  startAngle = -211;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_reappear_3_EW : AP_QuickWaveZode_reappear_1_EW)
{
  effect   = AP_QuickWaveZode_reappear_3_CE;
  lifetime = %AP_QuickWave_reappear_3_lifetime;
  delay    = %AP_QuickWave_reappear_3_delay;
};

// quick wave reappear 4
datablock afxZodiacData(AP_QuickWaveZode_reappear_4_CE : AP_QuickWaveZode_reappear_1_CE)
{  
  startAngle = 89;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_reappear_4_EW : AP_QuickWaveZode_reappear_1_EW)
{
  effect   = AP_QuickWaveZode_reappear_4_CE;
  lifetime = %AP_QuickWave_reappear_4_lifetime;
  delay    = %AP_QuickWave_reappear_4_delay;
};

// quick wave reappear 5
datablock afxZodiacData(AP_QuickWaveZode_reappear_5_CE : AP_QuickWaveZode_reappear_1_CE)
{  
  startAngle = -13;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_reappear_5_EW : AP_QuickWaveZode_reappear_1_EW)
{
  effect   = AP_QuickWaveZode_reappear_5_CE;
  lifetime = %AP_QuickWave_reappear_5_lifetime;
  delay    = %AP_QuickWave_reappear_5_delay;
};

// quick wave reappear 6
datablock afxZodiacData(AP_QuickWaveZode_reappear_6_CE : AP_QuickWaveZode_reappear_1_CE)
{  
  startAngle = 244;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_reappear_6_EW : AP_QuickWaveZode_reappear_1_EW)
{
  effect   = AP_QuickWaveZode_reappear_6_CE;
  lifetime = %AP_QuickWave_reappear_6_lifetime;
  delay    = %AP_QuickWave_reappear_6_delay;
};

// quick wave reappear 7
datablock afxZodiacData(AP_QuickWaveZode_reappear_7_CE : AP_QuickWaveZode_reappear_1_CE)
{  
  startAngle = 96;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_reappear_7_EW : AP_QuickWaveZode_reappear_1_EW)
{
  effect   = AP_QuickWaveZode_reappear_7_CE;
  lifetime = %AP_QuickWave_reappear_7_lifetime;
  delay    = %AP_QuickWave_reappear_7_delay;
};

// quick wave reappear 8
datablock afxZodiacData(AP_QuickWaveZode_reappear_8_CE : AP_QuickWaveZode_reappear_1_CE)
{  
  startAngle = -135;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_reappear_8_EW : AP_QuickWaveZode_reappear_1_EW)
{
  effect   = AP_QuickWaveZode_reappear_8_CE;
  lifetime = %AP_QuickWave_reappear_8_lifetime;
  delay    = %AP_QuickWave_reappear_8_delay;
};

// quick wave reappear 9
datablock afxZodiacData(AP_QuickWaveZode_reappear_9_CE : AP_QuickWaveZode_reappear_1_CE)
{  
  startAngle = 45;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_reappear_9_EW : AP_QuickWaveZode_reappear_1_EW)
{
  effect   = AP_QuickWaveZode_reappear_9_CE;
  lifetime = %AP_QuickWave_reappear_9_lifetime;
  delay    = %AP_QuickWave_reappear_9_delay;
};

// quick wave reappear 10
datablock afxZodiacData(AP_QuickWaveZode_reappear_10_CE : AP_QuickWaveZode_reappear_1_CE)
{  
  startAngle = -200;
};
//
datablock afxEffectWrapperData(AP_QuickWaveZode_reappear_10_EW : AP_QuickWaveZode_reappear_1_EW)
{
  effect   = AP_QuickWaveZode_reappear_10_CE;
  lifetime = %AP_QuickWave_reappear_10_lifetime;
  delay    = %AP_QuickWave_reappear_10_delay;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// QUICKWAVE FINAL ZODES

//
// After the last quick-wave zodiacs, during both disappearance and
// reappearance, a final quick zodiac shoots out from the teleport
// zodiac.  This last quick-wave is actually made up of three
// zodiacs, all the same except for different startAngles and 
// rotationRates.
//

// .. Disappearance Effects ......................................//

// quick wave final disappear 1
datablock afxZodiacData(AP_QuickWaveFinalZode_disappear_1_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/AP/zodiacs/AP_quickWaveC.png";
  radius = 1.5;
  startAngle = 0.0;
  rotationRate = -95.0;
  color = "1.0 1.0 1.0 0.35";
  blend = additive;
  growthRate = 15.0;
};
//
datablock afxEffectWrapperData(AP_QuickWaveFinalZode_disappear_1_EW)
{
  effect = AP_QuickWaveFinalZode_disappear_1_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  lifetime = 0.20;
  delay    = 2.85;  
  fadeInTime  = 0.025;
  fadeOutTime = 0.6;
};

// quick wave final disappear 2
datablock afxZodiacData(AP_QuickWaveFinalZode_disappear_2_CE : AP_QuickWaveFinalZode_disappear_1_CE)
{  
  startAngle = 123.0;
  rotationRate = 115.0;
};
//
datablock afxEffectWrapperData(AP_QuickWaveFinalZode_disappear_2_EW : AP_QuickWaveFinalZode_disappear_1_EW)
{
  effect = AP_QuickWaveFinalZode_disappear_2_CE;
};

// quick wave final disappear 3
datablock afxZodiacData(AP_QuickWaveFinalZode_disappear_3_CE : AP_QuickWaveFinalZode_disappear_1_CE)
{  
  startAngle = -98.0;
  rotationRate = -166.0;
};
//
datablock afxEffectWrapperData(AP_QuickWaveFinalZode_disappear_3_EW : AP_QuickWaveFinalZode_disappear_1_EW)
{
  effect = AP_QuickWaveFinalZode_disappear_3_CE;
};

// .. Reappearance Effects .......................................//

// quick wave final reappear 1
datablock afxEffectWrapperData(AP_QuickWaveFinalZode_reappear_1_EW : AP_QuickWaveFinalZode_disappear_1_EW)
{
  delay = 2.85+%AP_ReappearTimeOffset;  
};

// quick wave final reappear 2
datablock afxEffectWrapperData(AP_QuickWaveFinalZode_reappear_2_EW : AP_QuickWaveFinalZode_disappear_2_EW)
{
  delay = 2.85+%AP_ReappearTimeOffset;  
};

// quick wave final reappear 3
datablock afxEffectWrapperData(AP_QuickWaveFinalZode_reappear_3_EW : AP_QuickWaveFinalZode_disappear_3_EW)
{
  delay = 2.85+%AP_ReappearTimeOffset;  
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// TELEPORT ZODIAC FLASHES

//
// The zodiac texture previously used to reveal the teleport zodiac
// is reused here, to make it "flash" just prior to the appearance
// of the ring and when that ring re-enters the ground.  This occurs
// during both disappearance and reappearance. 
//

// .. Disappearance Effects ......................................//

// teleport flash 1 (disappearance) 
datablock afxZodiacData(AP_TeleportZodeFlash1_disappear_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/AP/zodiacs/AP_teleportZode_reveal.png";
  radius = 1.5;
  startAngle = 45.0; //180*.25
  rotationRate = -180.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(AP_TeleportZodeFlash1_disappear_EW)
{
  effect = AP_TeleportZodeFlash1_disappear_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  delay = 1.28;
  lifetime = 0.5;
  fadeInTime = 0.05;
  fadeOutTime = 0.15;
};

// teleport flash 2 (disappearance)
datablock afxZodiacData(AP_TeleportZodeFlash2_disappear_CE : AP_TeleportZodeFlash1_disappear_CE)
{  
  startAngle = -468.0; // (2.85-.25)*-180
};
//
datablock afxEffectWrapperData(AP_TeleportZodeFlash2_disappear_EW)
{
  effect = AP_TeleportZodeFlash2_disappear_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  delay = 2.80;
  lifetime = 0.50;
  fadeInTime = 0.05;
  fadeOutTime = 0.30;
};

// .. Reappearance Effects .......................................//

// teleport flash 1 (reappearance)
datablock afxEffectWrapperData(AP_TeleportZodeFlash1_reappear_EW : AP_TeleportZodeFlash1_disappear_EW)
{
  delay = 0.65+%AP_ReappearTimeOffset;
  lifetime = 0.8;
};

// teleport flash 2 (reappearance)
datablock afxEffectWrapperData(AP_TeleportZodeFlash2_reappear_EW : AP_TeleportZodeFlash2_disappear_EW)
{
  delay = 2.80+%AP_ReappearTimeOffset;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// PORTAL

//
// A portal zodiac appears beneath the spellcaster, into which he
// disappears and from which he reappears.  The entrance to the 
// other world...!
//

// .. Disappearance Effects ......................................//

// portal zodiac (disappearance)
datablock afxZodiacData(AP_ZodePortal_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/AP/zodiacs/ALL_portal";
  radius = 1.6;
  startAngle = 0.0;
  rotationRate = 240.0;
  color = "1.0 1.0 1.0 1.0";
  growInTime = 1.0;
};
//
datablock afxEffectWrapperData(AP_ZodePortal_disappear_EW)
{
  effect = AP_ZodePortal_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  delay = 0.2;
  lifetime = 3.2;
  fadeInTime = 0.75;
  fadeOutTime = 0.4;
};

// .. Reappearance Effects .......................................//

// portal zodiac (reappearance)
datablock afxEffectWrapperData(AP_ZodePortal_reappear_EW : AP_ZodePortal_disappear_EW)
{
  delay = 0.2+%AP_ReappearTimeOffset;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// PORTAL PARTICLES

//
// Four disc-type particle emitters are used to emit particles up
// from the portal.  The first three emit diamond-shaped particles,
// and they begin emitting in delayed sequence.  The final emitter
// emits linear sparkles just along the edges of its disc, so that
// it looks as if they make-up the edge of this virtual 
// "teleportation cylinder" full of particles.
//

// sparkle particle, linear -- blue
datablock ParticleData(AP_Sparkle_P)
{
   // TGE textureName          = %mySpellDataPath @ "/AP/particles/AP_linearSparkle";
   dragCoeffiecient     = 0.5;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 400;
   lifetimeVarianceMS   = 100;
   useInvAlpha          = false;
   spinRandomMin        = 0.0;
   spinRandomMax        = 0.0;
   colors[0]            = "0.2 0.03 0.93 1.0";
   colors[1]            = "0.2 0.03 0.93 1.0";
   colors[2]            = "0.2 0.03 0.93 0.0";
   sizes[0]             = 0.5;
   sizes[1]             = 0.5;
   sizes[2]             = 0.5;
   times[0]             = 0.0;
   times[1]             = 0.9;
   times[2]             = 1.0;

   textureName          = %mySpellDataPath @ "/AP/particles/ap_tiled_parts"; // linearSparkle
   textureCoords[0]     = "0.75 0.0";
   textureCoords[1]     = "0.75 0.25";
   textureCoords[2]     = "1.0  0.25";
   textureCoords[3]     = "1.0  0.0";
};
// sparkle particle, diamond -- pink
datablock ParticleData(AP_SparkleDiamondA_P)
{
   // TGE textureName          = %mySpellDataPath @ "/AP/particles/AP_diamondSparkle";
   dragCoeffiecient     = 0.5;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 500;
   lifetimeVarianceMS   = 150;
   useInvAlpha          = false;
   spinRandomMin        = 0.0;
   spinRandomMax        = 0.0;
   colors[0]            = "1.0 0.56 1.0 1.0";
   colors[1]            = "1.0 0.56 1.0 1.0";
   colors[2]            = "1.0 0.56 1.0 0.0";
   sizes[0]             = 0.15;
   sizes[1]             = 0.15;
   sizes[2]             = 0.15;
   times[0]             = 0.0;
   times[1]             = 0.6;
   times[2]             = 1.0;

   textureName          = %mySpellDataPath @ "/AP/particles/ap_tiled_parts"; // diamondSparkle
   textureCoords[0]     = "0.5  0.25";
   textureCoords[1]     = "0.5  0.5";
   textureCoords[2]     = "0.75 0.5";
   textureCoords[3]     = "0.75 0.25";
};
// sparkle particle, diamond -- light pink
datablock ParticleData(AP_SparkleDiamondB_P)
{
   // TGE textureName          = %mySpellDataPath @ "/AP/particles/AP_diamondSparkle";
   dragCoeffiecient     = 0.5;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 500;
   lifetimeVarianceMS   = 150;
   useInvAlpha          = false;
   spinRandomMin        = 0.0;
   spinRandomMax        = 0.0;
   colors[0]            = "1.0 0.94 1.0 1.0";
   colors[1]            = "1.0 0.94 1.0 1.0";
   colors[2]            = "1.0 0.94 1.0 0.0";
   sizes[0]             = 0.15;
   sizes[1]             = 0.15;
   sizes[2]             = 0.15;
   times[0]             = 0.0;
   times[1]             = 0.6;
   times[2]             = 1.0;

   textureName          = %mySpellDataPath @ "/AP/particles/ap_tiled_parts"; // diamondSparkle 
   textureCoords[0]     = "0.5  0.25";
   textureCoords[1]     = "0.5  0.5";
   textureCoords[2]     = "0.75 0.5";
   textureCoords[3]     = "0.75 0.25";
};

// .. Disappearance Effects ......................................//

// diamond particles emitted up through small-radius disc (disappearance)
datablock afxParticleEmitterDiscData(AP_PortalSparkleA_disappear_E) // TGEA
{
  ejectionOffset        = 0.0;
  ejectionPeriodMS      = 2;
  periodVarianceMS      = 1;
  ejectionVelocity      = 10.0;
  velocityVariance      = 3.0;  
  particles             = "AP_SparkleDiamondA_P AP_SparkleDiamondB_P";

  // TGE emitterType = "disc";
  vector = "0 0 1";
  radiusMin = 0.0;
  radiusMax = 0.5;

  fadeColor = true;
  fadeSize = true;
};
//
datablock afxEffectWrapperData(AP_PortalSparkleA_disappear_EW)
{
  effect = AP_PortalSparkleA_disappear_E;
  constraint = caster;
  lifetime = 2.25;
  delay    = 0.5;
  fadeInTime  = 0.5;
  fadeOutTime = 0.5;
};

// diamond particles emitted up through medium-radius disc (disappearance)
datablock afxParticleEmitterDiscData(AP_PortalSparkleB_disappear_E : AP_PortalSparkleA_disappear_E) // TGEA
{
  radiusMax = 0.65;
};
//
datablock afxEffectWrapperData(AP_PortalSparkleB_disappear_EW : AP_PortalSparkleA_disappear_EW)
{
  effect = AP_PortalSparkleB_disappear_E;
  lifetime = 2.0;
  delay    = 0.75;
};

// diamond particles emitted up through large-radius disc (disappearance)
datablock afxParticleEmitterDiscData(AP_PortalSparkleC_disappear_E : AP_PortalSparkleA_disappear_E) // TGEA
{
  radiusMax = 0.8;
};
//
datablock afxEffectWrapperData(AP_PortalSparkleC_disappear_EW : AP_PortalSparkleA_disappear_EW)
{
  effect = AP_PortalSparkleC_disappear_E;
  lifetime = 1.75;
  delay    = 1.0;
};

// linear particles emitted up from the edge of a disc (disappearance)
datablock afxParticleEmitterDiscData(AP_PortalSparkleD_disappear_E) // TGEA
{
  ejectionOffset        = 0.0;
  ejectionPeriodMS      = 2;
  periodVarianceMS      = 1;
  ejectionVelocity      = 10.0;
  velocityVariance      = 3.0;  
  particles             = "AP_Sparkle_P";

  // TGE emitterType = "disc";
  vector = "0 0 1";
  radiusMin = 0.8;
  radiusMax = 1.0;

  fadeColor = true;
};
//
datablock afxEffectWrapperData(AP_PortalSparkleD_disappear_EW)
{
  effect = AP_PortalSparkleD_disappear_E;
  constraint = caster;
  lifetime = 1.4;
  delay    = 1.0;
  fadeInTime  = 0.0;
  fadeOutTime = 0.5;
};

// .. Reappearance Effects .......................................//

// diamond particles emitted up through small-radius disc (reappearance)
datablock afxEffectWrapperData(AP_PortalSparkleA_reappear_EW : AP_PortalSparkleA_disappear_EW)
{
  delay    = 0.5+%AP_ReappearTimeOffset;
};

// diamond particles emitted up through medium-radius disc (reappearance)
datablock afxEffectWrapperData(AP_PortalSparkleB_reappear_EW : AP_PortalSparkleB_disappear_EW)
{
  delay    = 0.75+%AP_ReappearTimeOffset;
};

// diamond particles emitted up through large-radius disc (reappearance)
datablock afxEffectWrapperData(AP_PortalSparkleC_reappear_EW : AP_PortalSparkleC_disappear_EW)
{
  delay    = 1.0+%AP_ReappearTimeOffset;
};

// linear particles emitted up from the edge of a disc (reappearance)
datablock afxEffectWrapperData(AP_PortalSparkleD_reappear_EW : AP_PortalSparkleD_disappear_EW)
{
  delay    = 1.0+%AP_ReappearTimeOffset;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// PORTAL BEAMS

//
// Three dts models are used to help fill the volume of particles
// emitted from the portal.  Each is a vertical cylinder, varying
// in height and width, and textured to look like a beam of light.
// They appear in delayed sequence.
//

// aim constraint for the cylinders, used for the camera
datablock afxXM_AimData(AP_PortalBeam_Aim_XM)
{
  aimZOnly = true;
};

// .. Disappearance Effects ......................................//

// beam A (disappearance)
datablock afxModelData(AP_PortalBeamA_model_CE)
{
   shapeFile = %mySpellDataPath @ "/AP/models/AP_beamA.dts"; 
   alphaMult = 0.40;
   forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;   
};
//
datablock afxEffectWrapperData(AP_PortalBeamA_disappear_EW)
{
  effect        = AP_PortalBeamA_model_CE;
  constraint    = caster;
  posConstraint2 = "camera"; // aim

  delay = 0.5;
  fadeInTime  = 0.3;
  fadeOutTime = 0.3;
  lifetime    = 2.0;

  xfmModifiers[0] = "AP_PortalBeam_Aim_XM";
  propagateTimeFactor = true;
};

// beam B (disappearance)
datablock afxModelData(AP_PortalBeamB_model_CE)
{
   shapeFile = %mySpellDataPath @ "/AP/models/AP_beamB.dts"; 
   alphaMult = 0.30;
   forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;   
};
//
datablock afxEffectWrapperData(AP_PortalBeamB_disappear_EW)
{
  effect        = AP_PortalBeamB_model_CE;
  constraint    = caster;
  posConstraint2 = "camera"; // aim

  delay = 0.75;
  fadeInTime  = 0.3;
  fadeOutTime = 0.3;
  lifetime    = 1.75;

  xfmModifiers[0] = "AP_PortalBeam_Aim_XM";
  propagateTimeFactor = true;
};

// beam C (disappearance)
datablock afxModelData(AP_PortalBeamC_model_CE)
{
   shapeFile = %mySpellDataPath @ "/AP/models/AP_beamC.dts"; 
   alphaMult = 0.20;
   forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;   
};
//
datablock afxEffectWrapperData(AP_PortalBeamC_disappear_EW)
{
  effect        = AP_PortalBeamC_model_CE;
  constraint    = caster;
  posConstraint2 = "camera"; // aim

  delay = 1.0;
  fadeInTime  = 0.3;
  fadeOutTime = 0.3;
  lifetime    = 1.5;

  xfmModifiers[0] = "AP_PortalBeam_Aim_XM";
  propagateTimeFactor = true;
};

// .. Reappearance Effects .......................................//

// beam A (reappearance)
datablock afxEffectWrapperData(AP_PortalBeamA_reappear_EW : AP_PortalBeamA_disappear_EW)
{
  delay = 0.5+%AP_ReappearTimeOffset;
};

// beam B (reappearance)
datablock afxEffectWrapperData(AP_PortalBeamB_reappear_EW : AP_PortalBeamB_disappear_EW)
{
  delay = 0.75+%AP_ReappearTimeOffset;
};

// beam C (reappearance)
datablock afxEffectWrapperData(AP_PortalBeamC_reappear_EW : AP_PortalBeamC_disappear_EW)
{
  delay = 1.0+%AP_ReappearTimeOffset;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// PARTICLE SYMBOLS

//
// In a bit of frivolity, the letters "A", "F" and "X" are emitted
// from the teleportation zodiac, using three vector emitters that
// spin with it.  They add little but are there because Jeff hasn't
// noticed them yet (I think).
//

// "A"!
datablock ParticleData(AP_Symbol_A_P)
{
   // TGE textureName          = %mySpellDataPath @ "/AP/particles/AP_symbol_A";
   dragCoeffiecient     = 0.5;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 1300;
   lifetimeVarianceMS   = 150;
   useInvAlpha          = false;
   spinRandomMin        = 0.0;
   spinRandomMax        = 0.0;
   colors[0]            = "0.8 1.0 0.8 1.0"; //"1.0 0.56 1.0 1.0";
   colors[1]            = "0.8 1.0 0.8 1.0"; //"1.0 0.56 1.0 1.0";
   colors[2]            = "0.8 1.0 0.8 0.0"; //"1.0 0.56 1.0 0.0";
   sizes[0]             = 0.10;
   sizes[1]             = 0.10;
   sizes[2]             = 0.10;
   times[0]             = 0.0;
   times[1]             = 0.6;
   times[2]             = 1.0;

   textureName          = %mySpellDataPath @ "/AP/particles/ap_tiled_parts"; // symbol_A
   textureCoords[0]     = "0.0  0.5";
   textureCoords[1]     = "0.0  0.75";
   textureCoords[2]     = "0.25 0.75";
   textureCoords[3]     = "0.25 0.5";
};
// "F"!
datablock ParticleData(AP_Symbol_F_P)
{
   // TGE textureName          = %mySpellDataPath @ "/AP/particles/AP_symbol_F";
   dragCoeffiecient     = 0.5;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 1300;
   lifetimeVarianceMS   = 150;
   useInvAlpha          = false;
   spinRandomMin        = 0.0;
   spinRandomMax        = 0.0;
   colors[0]            = "0.8 1.0 0.8 1.0"; //"1.0 0.56 1.0 1.0";
   colors[1]            = "0.8 1.0 0.8 1.0"; //"1.0 0.56 1.0 1.0";
   colors[2]            = "0.8 1.0 0.8 0.0"; //"1.0 0.56 1.0 0.0";
   sizes[0]             = 0.10;
   sizes[1]             = 0.10;
   sizes[2]             = 0.10;
   times[0]             = 0.0;
   times[1]             = 0.6;
   times[2]             = 1.0;

   textureName          = %mySpellDataPath @ "/AP/particles/ap_tiled_parts"; // symbol_F
   textureCoords[0]     = "0.25 0.5";
   textureCoords[1]     = "0.25 0.75";
   textureCoords[2]     = "0.5  0.75";
   textureCoords[3]     = "0.5  0.5";
};
// "X"!
datablock ParticleData(AP_Symbol_X_P)
{
   // TGE textureName          = %mySpellDataPath @ "/AP/particles/AP_symbol_X";
   dragCoeffiecient     = 0.5;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 1300;
   lifetimeVarianceMS   = 150;
   useInvAlpha          = false;
   spinRandomMin        = 0.0;
   spinRandomMax        = 0.0;
   colors[0]            = "0.8 1.0 0.8 1.0"; //"1.0 0.56 1.0 1.0";
   colors[1]            = "0.8 1.0 0.8 1.0"; //"1.0 0.56 1.0 1.0";
   colors[2]            = "0.8 1.0 0.8 0.0"; //"1.0 0.56 1.0 0.0";
   sizes[0]             = 0.10;
   sizes[1]             = 0.10;
   sizes[2]             = 0.10;
   times[0]             = 0.0;
   times[1]             = 0.6;
   times[2]             = 1.0;

   textureName          = %mySpellDataPath @ "/AP/particles/ap_tiled_parts"; // symbol_X
   textureCoords[0]     = "0.5  0.5";
   textureCoords[1]     = "0.5  0.75";
   textureCoords[2]     = "0.75 0.75";
   textureCoords[3]     = "0.75 0.5";
};

// .. Disappearance Effects ......................................//

// vector emitter that emits "A", "F" and "X" in a seemingly random
//  sequence
datablock afxParticleEmitterVectorData(AP_Symbols_disappear_E) // TGEA
{
  ejectionOffset        = 0.0;
  ejectionPeriodMS      = 70;
  periodVarianceMS      = 0;
  ejectionVelocity      = 3.5;
  velocityVariance      = 0.0;  
  particles             = "AP_Symbol_A_P AP_Symbol_F_P AP_Symbol_X_P";

  // TGE emitterType = "vector";
  vector = "0 0 1";
};

// this offset defines the radius of the symbol emitters
datablock afxXM_LocalOffsetData(AP_Symbol_Offset_XM)
{
  localOffset = "0 1.10 0";
};

// this and the other spin modifiers set the starting
//  angle and rotation rate of the symbol emitters
datablock afxXM_SpinData(AP_Symbol_Spin1_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 0;
  spinRate  = -180;
};
//
datablock afxEffectWrapperData(AP_Symbols1_disappear_EW)
{
  effect = AP_Symbols_disappear_E;
  constraint = caster;
  lifetime = 2.0;
  delay    = 0.5;
  fadeInTime  = 0.0;
  fadeOutTime = 0.0;
  xfmModifiers[0] = "AP_Symbol_Spin1_XM";
  xfmModifiers[1] = "AP_Symbol_Offset_XM";
};

datablock afxXM_SpinData(AP_Symbol_Spin2_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 120;
  spinRate  = -180;
};
//
datablock afxEffectWrapperData(AP_Symbols2_disappear_EW)
{
  effect = AP_Symbols_disappear_E;
  constraint = caster;
  lifetime = 2.0;
  delay    = 0.5;
  fadeInTime  = 0.0;
  fadeOutTime = 0.0;
  xfmModifiers[0] = "AP_Symbol_Spin2_XM";
  xfmModifiers[1] = "AP_Symbol_Offset_XM";
};

datablock afxXM_SpinData(AP_Symbol_Spin3_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 240;
  spinRate  = -180;
};
//
datablock afxEffectWrapperData(AP_Symbols3_disappear_EW)
{
  effect = AP_Symbols_disappear_E;
  constraint = caster;
  lifetime = 2.0;
  delay    = 0.5;
  fadeInTime  = 0.0;
  fadeOutTime = 0.0;
  xfmModifiers[0] = "AP_Symbol_Spin3_XM";
  xfmModifiers[1] = "AP_Symbol_Offset_XM";
};

// .. Reappearance Effects .......................................//

datablock afxEffectWrapperData(AP_Symbols1_reappear_EW : AP_Symbols1_disappear_EW)
{
  delay = 0.5+%AP_ReappearTimeOffset;
};

datablock afxEffectWrapperData(AP_Symbols2_reappear_EW : AP_Symbols2_disappear_EW)
{
  delay = 0.5+%AP_ReappearTimeOffset;
};

datablock afxEffectWrapperData(AP_Symbols3_reappear_EW : AP_Symbols3_disappear_EW)
{
  delay = 0.5+%AP_ReappearTimeOffset;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// RING

//
// A dts ring model moves up and down from the teleportation zodiac,
// modeled to look like a glowing 3D version of it.  The ring is
// animated using transform modifiers only, a spin and two paths.
//

// spin modifier to rotate ring
datablock afxXM_SpinData(AP_Ring_Spin_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 0;
  spinRate  = -180;
};

// path that moves the ring up
datablock afxPathData(AP_Ring_path1_U)
{
  points = "0 0 -0.4	0 0 4.0";  
  lifetime = 0.6;
};

// path the moves the ring down
datablock afxPathData(AP_Ring_path2_U)
{
  points = "0 0 0	0 0 -4.4";  
  delay  = 0.9;
  lifetime = 0.4;
};

// this modifier conforms the ring to the pathes
datablock afxXM_PathConformData(AP_Ring_Path_XM)
{
  paths = "AP_Ring_path1_U AP_Ring_path2_U";
};

// .. Disappearance Effects ......................................//

// ring model (disappearance)
datablock afxModelData(AP_Ring_model_CE)
{
   shapeFile = %mySpellDataPath @ "/AP/models/AP_ring.dts";    
   forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;   
};
//
datablock afxEffectWrapperData(AP_Ring_disappear_EW)
{
  effect        = AP_Ring_model_CE;
  posConstraint = caster;

  delay = 1.7;
  fadeOutTime = 0.25;
  lifetime    = 1.0;

  scaleFactor = 0.8;
  xfmModifiers[0] = "AP_Ring_Spin_XM";
  xfmModifiers[1] = "AP_Ring_Path_XM";
  propagateTimeFactor = true;
};

// .. Reappearance Effects .......................................//

// ring model (reappearance)
datablock afxEffectWrapperData(AP_Ring_reappear_EW : AP_Ring_disappear_EW)
{
  delay = 1.7+%AP_ReappearTimeOffset;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// RING PARTICLES

//
// As the ring moves up and down, it emits particles that fill the
// volume of teleportation with smoky, sparkly magic.  Sparkly is
// the essence of space-time.  These emitters are disc emitters
// that use the same paths defined in the "RING" section to mirror
// the movement of the ring.
//
// Note the trick used in AP_RingDust_E: the particles string 
// contains mostly instances of "AP_SparkleDust_P", with one 
// "AP_Sparkly_P", thus ensuring a much higher percentage of dust
// will be emitted versus sparkly.
//

// sparkle particle, diamond -- pink, + rotation
datablock ParticleData(AP_SparkleDiamondC_P)
{
   // TGE textureName          = %mySpellDataPath @ "/AP/particles/AP_diamondSparkle";
   dragCoeffiecient     = 0.5;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 2000;
   lifetimeVarianceMS   = 400;
   useInvAlpha          = false;
   spinRandomMin        = 360.0;
   spinRandomMax        = 720.0;
   colors[0]            = "1.0 0.60 1.0 1.0";
   colors[1]            = "1.0 0.60 1.0 1.0";
   colors[2]            = "1.0 0.60 1.0 0.0";
   sizes[0]             = 0.10;
   sizes[1]             = 0.10;
   sizes[2]             = 0.10;
   times[0]             = 0.0;
   times[1]             = 0.6;
   times[2]             = 1.0;

   textureName          = %mySpellDataPath @ "/AP/particles/ap_tiled_parts"; // diamondSparkle
   textureCoords[0]     = "0.5  0.25";
   textureCoords[1]     = "0.5  0.5";
   textureCoords[2]     = "0.75 0.5";
   textureCoords[3]     = "0.75 0.25";
};
// sparkle particle, diamond -- blue, + rotation
datablock ParticleData(AP_SparkleDiamondD_P)
{
   // TGE textureName          = %mySpellDataPath @ "/AP/particles/AP_diamondSparkle";
   dragCoeffiecient     = 0.5;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 2000;
   lifetimeVarianceMS   = 400;
   useInvAlpha          = false;
   spinRandomMin        = 360.0;
   spinRandomMax        = 720.0;
   colors[0]            = "0.2 0.03 0.93 1.0";
   colors[1]            = "0.2 0.03 0.93 1.0";
   colors[2]            = "0.2 0.03 0.93 0.0";
   sizes[0]             = 0.10;
   sizes[1]             = 0.10;
   sizes[2]             = 0.10;
   times[0]             = 0.0;
   times[1]             = 0.6;
   times[2]             = 1.0;

   textureName          = %mySpellDataPath @ "/AP/particles/ap_tiled_parts"; // diamondSparkle
   textureCoords[0]     = "0.5  0.25";
   textureCoords[1]     = "0.5  0.5";
   textureCoords[2]     = "0.75 0.5";
   textureCoords[3]     = "0.75 0.25";
};
// sparkle particle, diamond -- pink, - rotation
datablock ParticleData(AP_SparkleDiamondC2_P : AP_SparkleDiamondC_P)
{
   spinRandomMin        = -720.0;
   spinRandomMax        = -360.0;
};
// sparkle particle, diamond -- blue, - rotation
datablock ParticleData(AP_SparkleDiamondD2_P : AP_SparkleDiamondD_P)
{
   spinRandomMin        = -720.0;
   spinRandomMax        = -360.0;
};
// dust particle -- pink
datablock ParticleData(AP_SparkleDust_P)
{
   // TGE textureName          = %mySpellDataPath @ "/AP/particles/smoke";
   dragCoeffiecient     = 0.5;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 500;
   lifetimeVarianceMS   = 150;
   useInvAlpha          = false;
   spinRandomMin        = -60.0;
   spinRandomMax        = 60.0;
   colors[0]            = "1.0 0.90 1.0 0.25";
   colors[1]            = "1.0 0.15 1.0 0.25";
   colors[2]            = "1.0 0.60 1.0 0.0";
   sizes[0]             = 0.6;
   sizes[1]             = 1.5;
   sizes[2]             = 2.0;
   times[0]             = 0.0;
   times[1]             = 0.7;
   times[2]             = 1.0;   

   textureName          = %mySpellDataPath @ "/AP/particles/ap_tiled_parts"; // smoke
   textureCoords[0]     = "0.5 0.0";
   textureCoords[1]     = "0.5 0.25";
   textureCoords[2]     = "0.75 0.25";
   textureCoords[3]     = "0.75 0.0";
};
// sparkly particle -- white
datablock ParticleData(AP_Sparkly_P)
{
   // TGE textureName          = %mySpellDataPath @ "/AP/particles/AP_sparkly";
   dragCoeffiecient     = 0.5;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 500;
   lifetimeVarianceMS   = 150;
   useInvAlpha          = false;
   spinRandomMin        = 0;
   spinRandomMax        = 0;
   colors[0]            = "1.0 1.0 1.0 1.0";
   colors[1]            = "1.0 1.0 1.0 1.0";
   colors[2]            = "1.0 1.0 1.0 0.0";
   sizes[0]             = 1.3;
   sizes[1]             = 1.3;
   sizes[2]             = 1.3;
   times[0]             = 0.0;
   times[1]             = 0.7;
   times[2]             = 1.0;   

   textureName          = %mySpellDataPath @ "/AP/particles/ap_tiled_parts"; // sparkly
   textureCoords[0]     = "0.0 0.0";
   textureCoords[1]     = "0.0 0.5";
   textureCoords[2]     = "0.5 0.5";
   textureCoords[3]     = "0.5 0.0";
};

// .. Disappearance Effects ......................................//

// ring dust disc emitter, with some sparkly (disappearance)
datablock afxParticleEmitterDiscData(AP_RingDust_E) // TGEA
{
  ejectionOffset        = 1.0;
  ejectionPeriodMS      = 2;
  periodVarianceMS      = 1;
  ejectionVelocity      = 0.3;
  velocityVariance      = 0.1; 
  particles             = "AP_SparkleDust_P AP_SparkleDust_P AP_SparkleDust_P " @
                          "AP_SparkleDust_P AP_Sparkly_P";

  // TGE emitterType = "disc";
  vector = "0 0 1";
  radiusMin = 0.0;
  radiusMax = 0.85;
};
//
datablock afxEffectWrapperData(AP_RingDust_disappear_EW)
{
  effect = AP_RingDust_E;
  posConstraint = caster;
  delay = 1.7;
  lifetime    = 1.3;
  xfmModifiers[0] = "AP_Ring_Path_XM";
};

// ring diamond sparkle emitter 1, just along edge (disappearance)
datablock afxParticleEmitterDiscData(AP_RingSparkle1_E) // TGEA
{
  ejectionOffset        = 0.0;
  ejectionPeriodMS      = 2;
  periodVarianceMS      = 1;
  ejectionVelocity      = 0.6;
  velocityVariance      = 0.3;  
  particles             = "AP_SparkleDiamondC_P AP_SparkleDiamondD_P";

  // TGE emitterType = "disc";
  vector = "0 0 1";
  radiusMin = 1.0;
  radiusMax = 1.0;

  fadeColor = true;
};
//
datablock afxEffectWrapperData(AP_RingSparkle1_disappear_EW)
{
  effect = AP_RingSparkle1_E;
  posConstraint = caster;
  delay = 1.7;
  fadeInTime  = 0.0;
  fadeOutTime = 0.25;
  lifetime    = 1.3;
  xfmModifiers[0] = "AP_Ring_Path_XM";
};

// ring diamond sparkle emitter 2, just along edge (disappearance)
datablock afxParticleEmitterDiscData(AP_RingSparkle2_E) // TGEA
{
  ejectionOffset        = 0.0;
  ejectionPeriodMS      = 2;
  periodVarianceMS      = 1;
  ejectionVelocity      = 0.6;
  velocityVariance      = 0.3;  
  particles             = "AP_SparkleDiamondC2_P AP_SparkleDiamondD2_P";

  // TGE emitterType = "disc";
  vector = "0 0 1";
  radiusMin = 1.0;
  radiusMax = 1.0;

  fadeColor = true;
};
//
datablock afxEffectWrapperData(AP_RingSparkle2_disappear_EW)
{
  effect = AP_RingSparkle2_E;
  posConstraint = caster;
  delay = 1.7;
  fadeInTime  = 0.0;
  fadeOutTime = 0.25;
  lifetime    = 1.3;
  xfmModifiers[0] = "AP_Ring_Path_XM";
};

// .. Reappearance Effects .......................................//

// ring dust disc emitter, with some sparkly (reappearance)
datablock afxEffectWrapperData(AP_RingDust_reappear_EW : AP_RingDust_disappear_EW)
{
  delay = 1.7+%AP_ReappearTimeOffset;
};
// ring diamond sparkle emitter 1, just along edge (reappearance)
datablock afxEffectWrapperData(AP_RingSparkle1_reappear_EW : AP_RingSparkle1_disappear_EW)
{
  delay = 1.7+%AP_ReappearTimeOffset;
};
// ring diamond sparkle emitter 2, just along edge (reappearance)
datablock afxEffectWrapperData(AP_RingSparkle2_reappear_EW : AP_RingSparkle2_disappear_EW)
{
  delay = 1.7+%AP_ReappearTimeOffset;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SCRIPTS

datablock afxScriptEventData(AP_FadeOutScript_CE)
{
  methodName = "FadeOutCaster";   // name of method in afxMagicSpellData subclass
};
datablock afxEffectWrapperData(AP_FadeOutScript_EW)
{
  effect = AP_FadeOutScript_CE;
  constraint = "impactedObject";
  delay = 2.7;
};

datablock afxScriptEventData(AP_FadeInScript_CE)
{
  methodName = "FadeInCaster";   // name of method in afxMagicSpellData subclass
};
datablock afxEffectWrapperData(AP_FadeInScript_EW)
{
  effect = AP_FadeInScript_CE;
  constraint = "impactedObject";
  delay = %AP_ReappearTimeOffset + 0.95;
};

datablock afxScriptEventData(AP_TeleportScript_CE)
{
  methodName = "TeleportCaster";   // name of method in afxMagicSpellData subclass
};
datablock afxEffectWrapperData(AP_TeleportScript_EW)
{
  effect = AP_TeleportScript_CE;
  constraint = "impactedObject";
  //delay = %AP_ReappearTimeOffset - 0.5;
  delay = 5.0 - 0.5;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/ap_lighting_tge_sub.cs");
    exec("./audio/ap_audio_sub.cs");
  case "TGEA":
    exec("./lighting/ap_lighting_tgea_sub.cs");
    exec("./audio/ap_audio_sub.cs");
 case "T3D":
    exec("./lighting/ap_lighting_t3d_sub.cs");
    exec("./audio/ap_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// ASTRAL PASSPORT SPELL
//

datablock afxMagicSpellData(AstralPassportSpell)
{
  castingDur = 3.5;

  addCastingEffect = AP_Casting_Clip_disappear_EW;
  addCastingEffect = AP_AnimLock_disappear_EW;
  addCastingEffect = AP_FixedZodeMooring_EW;
    // portal (disappear) //
  addCastingEffect = AP_ZodePortal_disappear_EW;
    // teleport zodiac (disappear) //
	//  NOTE: these zodiacs must be added after the portal zodiac above
	//         in order for them to be layered above the portal
  addCastingEffect = AP_TeleportZodeReveal_disappear_EW;
  addCastingEffect = AP_TeleportZode_disappear_EW;
    // quickwave zodes (disappear) //
  addCastingEffect = AP_QuickWaveZode_disappear_1_EW;
  addCastingEffect = AP_QuickWaveZode_disappear_2_EW;
  addCastingEffect = AP_QuickWaveZode_disappear_3_EW;
  addCastingEffect = AP_QuickWaveZode_disappear_4_EW;
  addCastingEffect = AP_QuickWaveZode_disappear_5_EW;
  addCastingEffect = AP_QuickWaveZode_disappear_6_EW;
  addCastingEffect = AP_QuickWaveZode_disappear_7_EW;
  addCastingEffect = AP_QuickWaveZode_disappear_8_EW;
  addCastingEffect = AP_QuickWaveZode_disappear_9_EW;
  addCastingEffect = AP_QuickWaveZode_disappear_10_EW;
    // teleport zodiac flash 1 (disappear) //
  addCastingEffect = AP_TeleportZodeFlash1_disappear_EW;  
    // portal particles (disappear) //
  addCastingEffect = AP_PortalSparkleA_disappear_EW;
  addCastingEffect = AP_PortalSparkleB_disappear_EW;
  addCastingEffect = AP_PortalSparkleC_disappear_EW;
  addCastingEffect = AP_PortalSparkleD_disappear_EW;
    // portal beams (disappear) //
  addCastingEffect = AP_PortalBeamA_disappear_EW;
  addCastingEffect = AP_PortalBeamB_disappear_EW;
  addCastingEffect = AP_PortalBeamC_disappear_EW;
    // particle symbols (disappear)
  addCastingEffect = AP_Symbols1_disappear_EW;
  addCastingEffect = AP_Symbols2_disappear_EW;
  addCastingEffect = AP_Symbols3_disappear_EW;
    // ring (disappear) //
  addCastingEffect = AP_Ring_disappear_EW;
    // ring particles (disappear)
  addCastingEffect = AP_RingDust_disappear_EW;
  addCastingEffect = AP_RingSparkle1_disappear_EW;
  addCastingEffect = AP_RingSparkle2_disappear_EW;
    // teleport zodiac flash 2 (disappear) //
  addCastingEffect = AP_TeleportZodeFlash2_disappear_EW;
    // quickwave final zodes (disappear) //
  addCastingEffect = AP_QuickWaveFinalZode_disappear_1_EW;
  addCastingEffect = AP_QuickWaveFinalZode_disappear_2_EW;
  addCastingEffect = AP_QuickWaveFinalZode_disappear_3_EW;

  addCastingEffect = AP_FadeOutScript_EW;
  addCastingEffect = AP_TeleportScript_EW;
};

// sounds and lights added via sub-script functions //
AP_disappear_add_Audio_FX(AstralPassportSpell);
AP_disappear_add_Lighting_FX(AstralPassportSpell);

datablock afxMagicSpellData(AstralPassportReappearSpell)
{
  lingerDur = 3.5;

  allowMovementInterrupts = false;

  addLingerEffect = AP_Casting_Clip_reappear_EW;
  addLingerEffect = AP_AnimLock_reappear_EW;
  addLingerEffect = AP_FixedZodeMooring_EW;
    // portal (reappear) //
  addLingerEffect = AP_ZodePortal_reappear_EW;
  	//  NOTE: these zodiacs must be added after the portal zodiac above
	  //         in order for them to be layered above the portal
  addLingerEffect = AP_TeleportZodeReveal_reappear_EW;
  addLingerEffect = AP_TeleportZode_reappear_EW;
    // quickwave zodes (reappear) //
  addLingerEffect = AP_QuickWaveZode_reappear_1_EW;
  addLingerEffect = AP_QuickWaveZode_reappear_2_EW;
  addLingerEffect = AP_QuickWaveZode_reappear_3_EW;
  addLingerEffect = AP_QuickWaveZode_reappear_4_EW;
  addLingerEffect = AP_QuickWaveZode_reappear_5_EW;
  addLingerEffect = AP_QuickWaveZode_reappear_6_EW;
  addLingerEffect = AP_QuickWaveZode_reappear_7_EW;
  addLingerEffect = AP_QuickWaveZode_reappear_8_EW;
  addLingerEffect = AP_QuickWaveZode_reappear_9_EW;
  addLingerEffect = AP_QuickWaveZode_reappear_10_EW;
    // teleport zodiac flash 1 (reappear) //
  addLingerEffect = AP_TeleportZodeFlash1_reappear_EW;
    // portal particles (reappear) //
  addLingerEffect = AP_PortalSparkleA_reappear_EW;
  addLingerEffect = AP_PortalSparkleB_reappear_EW;
  addLingerEffect = AP_PortalSparkleC_reappear_EW;
  addLingerEffect = AP_PortalSparkleD_reappear_EW;
    // portal beams (reappear) //
  addLingerEffect = AP_PortalBeamA_reappear_EW;
  addLingerEffect = AP_PortalBeamB_reappear_EW;
  addLingerEffect = AP_PortalBeamC_reappear_EW;
    // particle symbols (reappear)
  addLingerEffect = AP_Symbols1_reappear_EW;
  addLingerEffect = AP_Symbols2_reappear_EW;
  addLingerEffect = AP_Symbols3_reappear_EW;
    // ring (reappear) //
  addLingerEffect = AP_Ring_reappear_EW;
    // ring particles (reappear)
  addLingerEffect = AP_RingDust_reappear_EW;
  addLingerEffect = AP_RingSparkle1_reappear_EW;
  addLingerEffect = AP_RingSparkle2_reappear_EW;
    // teleport zodiac flash 2 (reappear) //
  addLingerEffect = AP_TeleportZodeFlash2_reappear_EW;
    // quickwave final zodes (reappear) //
  addLingerEffect = AP_QuickWaveFinalZode_reappear_1_EW;
  addLingerEffect = AP_QuickWaveFinalZode_reappear_2_EW;
  addLingerEffect = AP_QuickWaveFinalZode_reappear_3_EW;

  addLingerEffect = AP_FadeInScript_EW;
};

// sounds and lights added via sub-script functions //
AP_reappear_add_Lighting_FX(AstralPassportReappearSpell);
AP_reappear_add_Audio_FX(AstralPassportReappearSpell);

datablock afxRPGMagicSpellData(AstralPassportSpell_RPG)
{
  spellName = "Astral Passport";
  desc = "Take a duty-free shortcut across the astral plane to a nearby location." @
         "\n" @
         "\nspell design: Matthew Durante" @
         "\nsound effects: Dave Schroeder" @ 
         "\nspell concept: Jeff Faust";
  sourcePack = "Core Tech";
  iconBitmap = %mySpellDataPath @ "/AP/icons/ap";
  target = "self";
  manaCost = 10;
  reagentCost = 0;
  castingDur = AstralPassportSpell.castingDur;
};

// script methods

function AstralPassportSpell::FadeOutCaster(%this, %spell, %caster, %constraint, %pos, %data)
{
  if (isObject(%caster))
    %caster.startFade(450, 0, true);
}

function AstralPassportReappearSpell::FadeInCaster(%this, %spell, %caster, %constraint, %pos, %data)
{
  if (isObject(%caster))
    %caster.startFade(500, 0, false);
}

function AstralPassportSpell::TeleportCaster(%this, %spell, %caster, %constraint, %pos, %data)
{
  if (isObject(%caster))
  {
    %tele_dest = afxPickTeleportDest(%caster.getTransform());
    %caster.setTransform(%tele_dest);
    %client = %caster.getControllingClient();
    if (isObject(%client) && isObject(%client.camera))
    {
      %client.camera.setThirdPersonSnap();
      if (isFunction(snapAtlasGeometryMorph))
        snapAtlasGeometryMorph();
    }

    castSpell(AstralPassportReappearSpell, %caster, %caster);
  }
}

function AstralPassportSpell::onInterrupt(%this, %spell, %caster)
{
  Parent::onInterrupt(%this, %spell, %caster);
  if (%caster)
    %caster.startFade(500, 0, false);
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
  AstralPassportSpell.scriptFile = $afxAutoloadScriptFile;
  AstralPassportSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
  {
    addDemoSpellbookSpell(AstralPassportSpell, AstralPassportSpell_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//