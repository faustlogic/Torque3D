
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// GREAT BALL OF FIRE SPELL
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
$spell_reload = isObject(GreatBallSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = GreatBallSpell.spellDataPath;
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

// this mooring anchors the zodiacs and measures
// their altitudes

datablock afxMooringData(GBoF_FixedZodeMooring_CE)
{
  networking = $AFX::CLIENT_ONLY;
  displayAxisMarker = false;
};
datablock afxEffectWrapperData(GBoF_FixedZodeMooring_EW)
{
  effect = GBoF_FixedZodeMooring_CE;
  posConstraint = caster;
  effectName = "FixedZodeMooring";
  isConstraintSrc = true;
  lifetime = 7.0;
  xfmModifiers[0] = SHARED_AltitudeConform_XM;
  xfmModifiers[1] = SHARED_freeze_XM;
};

//
// The main casting zodiac is formed by three zodiacs plus a white
// reveal glow when the casting first starts. Holes are left for 
// the five small portal zodiacs described below.
//

// this is the white reveal glow
datablock afxZodiacData(GBoF_ZodeReveal_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/GBoF/zodiacs/GBofF_reveal";
  radius = 3.0;
  startAngle = 0.0;
  rotationRate = -30.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(GBoF_Zode_Reveal_EW)
{
  effect = GBoF_ZodeReveal_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  lifetime = 0.75;
  fadeInTime = 0.5;
  fadeOutTime = 0.25;
  propagateTimeFactor = true;
};

// this is the main pattern of the casting zode
datablock afxZodiacData(GBoF_Zode1_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/GBoF/zodiacs/GBofF_rings";
  radius = 3.0;
  startAngle = 0.0;
  rotationRate = -30.0;
  color = "1.0 1.0 1.0 0.8";
  blend = additive;
};
//
datablock afxEffectWrapperData(GBoF_Zode1_EW)
{
  effect = GBoF_Zode1_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  delay = 0.25;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 5.0;
  propagateTimeFactor = true;
};

// Main Zode Underglow
//  Glowing zodiacs blended additively tend to be washed-out atop
//  light groundplanes. To make them visually "pop" the ground must
//  be darkened, and this is done here.  The "underglow" zodiac is
//  a copy of the glow zodiac that is blended normally.  Because the
//  glow zodiacs have halos extending beyond their opaque regions
//  that blend with black, the ground is subtly darkened.  As the
//  glow is layered atop it -- it pops!  Increasing the color value
//  increases the effect.
datablock afxZodiacData(GBoF_Zode1_underglow_CE : GBoF_Zode1_CE)
{
  texture = %mySpellDataPath @ "/GBoF/zodiacs/GBofF_rings-underglow256";
  color = "0.45 0.45 0.45 0.45";
  blend = normal;
};
//
datablock afxEffectWrapperData(GBoF_Zode1_underglow_EW : GBoF_Zode1_EW)
{
  effect = GBoF_Zode1_underglow_CE;
  execConditions = $BrightLighting_mask;
};

// this is the rune ring portion of the casting zode
datablock afxZodiacData(GBoF_Zode2_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/GBoF/zodiacs/zode_text";
  radius = 3.0;
  startAngle = 0.0;
  rotationRate = 20.0; //60
  color = "1.0 0.0 0.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(GBoF_Zode2_EW)
{
  effect = GBoF_Zode2_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  delay = 0.25;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 5.0;
  propagateTimeFactor = true;
};

// Runes & Skulls Zode Underglow
//  Here the zode is made black but only slightly opaque to subtly
//  darken the ground, making the additive glow zode appear more
//  saturated.
datablock afxZodiacData(GBoF_Zode2_underglow_CE : GBoF_Zode2_CE)
{
  color = "0 0 0 0.25";
  blend = normal;
};
//
datablock afxEffectWrapperData(GBoF_Zode2_underglow_EW : GBoF_Zode2_EW)
{
  effect = GBoF_Zode2_underglow_CE;
  execConditions = $BrightLighting_mask;
};

// this layer adds sketchy white symbols to the casting zode
datablock afxZodiacData(GBoF_Zode3_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/GBoF/zodiacs/zode_symbols";
  radius = 3.0;
  startAngle = 0.0;
  rotationRate = -30.0;
  color = "1.0 1.0 1.0 0.5";
  blend = additive;
};
//
datablock afxEffectWrapperData(GBoF_Zode3_EW)
{
  effect = GBoF_Zode3_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  delay = 0.25;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 5.0;
  propagateTimeFactor = true;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// PORTAL ZODIACS

//
// These five smaller zodiacs match holes in the main zodiac and
// define the portal locations where the portal-beams materialize.
//

datablock afxZodiacData(GBoF_ZodePortal_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/GBoF/zodiacs/ALL_portal";
  radius = 0.45;
  startAngle = 0.0;
  rotationRate = 240.0;
  color = "1.0 1.0 1.0 1.0";
  growInTime = 1.0;
};

// this offset defines the radius of the portal-zodes
datablock afxXM_LocalOffsetData(GBoF_ZodePortal_Offset_XM)
{
  localOffset = "0 1.5 0";
};

// this and the other spin modifiers set the starting
// angle and rotation rate of the portal-beams.
datablock afxXM_SpinData(GBoF_ZodePortal_Spin1_XM)
{
  spinAxis = "0 0 1";
  spinAngle = -15; // 0-15
  spinRate = -30;
};
//
datablock afxEffectWrapperData(GBoF_ZodePortal1_EW)
{
  effect = GBoF_ZodePortal_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  delay = 0.75;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 3.0;
  xfmModifiers[0] = "GBoF_ZodePortal_Spin1_XM";
  xfmModifiers[1] = "GBoF_ZodePortal_Offset_XM";
};

datablock afxXM_SpinData(GBoF_ZodePortal_Spin2_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 57; // 72-15
  spinRate = -30;
};
//
datablock afxEffectWrapperData(GBoF_ZodePortal2_EW)
{
  effect = GBoF_ZodePortal_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  delay = 0.75;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 3.0;
  xfmModifiers[0] = "GBoF_ZodePortal_Spin2_XM";
  xfmModifiers[1] = "GBoF_ZodePortal_Offset_XM";
};

datablock afxXM_SpinData(GBoF_ZodePortal_Spin3_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 129; // 144-15
  spinRate = -30;
};
//
datablock afxEffectWrapperData(GBoF_ZodePortal3_EW)
{
  effect = GBoF_ZodePortal_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  delay = 0.75;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 3.0;
  xfmModifiers[0] = "GBoF_ZodePortal_Spin3_XM";
  xfmModifiers[1] = "GBoF_ZodePortal_Offset_XM";
};

datablock afxXM_SpinData(GBoF_ZodePortal_Spin4_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 201; // 216-15
  spinRate = -30;
};
//
datablock afxEffectWrapperData(GBoF_ZodePortal4_EW)
{
  effect = GBoF_ZodePortal_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  delay = 0.75;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 3.0;
  xfmModifiers[0] = "GBoF_ZodePortal_Spin4_XM";
  xfmModifiers[1] = "GBoF_ZodePortal_Offset_XM";
};

datablock afxXM_SpinData(GBoF_ZodePortal_Spin5_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 273; // 288-15
  spinRate = -30;
};
//
datablock afxEffectWrapperData(GBoF_ZodePortal5_EW)
{
  effect = GBoF_ZodePortal_CE;
  posConstraint = "#effect.FixedZodeMooring";
  borrowAltitudes = true;
  delay = 0.75;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 3.0;
  xfmModifiers[0] = "GBoF_ZodePortal_Spin5_XM";
  xfmModifiers[1] = "GBoF_ZodePortal_Offset_XM";
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// PORTAL BEAMS
//

//
// These five glowing columns of light materialize from the
// portal zodiacs.
//

switch$ (afxGetEngine())
{
  case "TGE":
    $GBoF_portalBeam_alphaMult = 0.65;
  case "TGEA":
    $GBoF_portalBeam_alphaMult = 0.5;
  case "T3D":
    $GBoF_portalBeam_alphaMult = 0.65; // JTF Note: test this
}

// this glowing cylinder is shared by the five portal-beam effects
datablock afxModelData(GBoF_portalBeam_model_CE)
{
   shapeFile = %mySpellDataPath @ "/GBoF/models/GBofF_portalBeam.dts"; 
   sequence = "arise";
   alphaMult = $GBoF_portalBeam_alphaMult;
   forceOnMaterialFlags = $MaterialFlags::Translucent; // TGE (ignored by TGEA)
   remapTextureTags = "portalbeamcolor2.png:GBoF_beam"; // TGEA (ignored by TGE)
};

// this offset defines the radius of the portal-beams
datablock afxXM_LocalOffsetData(GBoF_portalBeam_Offset)
{
  localOffset = "0 1.1 0";
};

// this modifier keeps the beams at ground level
datablock afxXM_GroundConformData(GBoF_portalBeam_Ground)
{
  height = 0.0;
};

// this and the other spin modifiers set the starting
// angle and rotation rate of the portal-beams.
datablock afxXM_SpinData(GBoF_portalBeam_Spin1_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 0;
  spinRate = -30;
};
//
datablock afxEffectWrapperData(GBoF_portalBeam1_EW)
{
  effect     = GBoF_portalBeam_model_CE;
  posConstraint = caster;
  delay = 0.25;
  fadeInTime = 0.1;
  fadeOutTime = 0.75;
  lifetime = 2.25;
  xfmModifiers[0] = "GBoF_portalBeam_Spin1_XM";
  xfmModifiers[1] = "GBoF_portalBeam_Offset";
  xfmModifiers[2] = "GBoF_portalBeam_Ground";
  propagateTimeFactor = true;
};

datablock afxXM_SpinData(GBoF_portalBeam_Spin2_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 72;
  spinRate = -30;
};
//
datablock afxEffectWrapperData(GBoF_portalBeam2_EW)
{
  effect     = GBoF_portalBeam_model_CE;
  posConstraint = caster;
  delay = 0.25;
  fadeInTime = 0.1;
  fadeOutTime = 0.75;
  lifetime = 2.25;
  xfmModifiers[0] = "GBoF_portalBeam_Spin2_XM";
  xfmModifiers[1] = "GBoF_portalBeam_Offset";
  xfmModifiers[2] = "GBoF_portalBeam_Ground";
  propagateTimeFactor = true;
};

datablock afxXM_SpinData(GBoF_portalBeam_Spin3_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 144;
  spinRate = -30;
};
//
datablock afxEffectWrapperData(GBoF_portalBeam3_EW)
{
  effect     = GBoF_portalBeam_model_CE;
  posConstraint = caster;
  delay = 0.25;
  fadeInTime = 0.1;
  fadeOutTime = 0.75;
  lifetime = 2.25;
  xfmModifiers[0] = "GBoF_portalBeam_Spin3_XM";
  xfmModifiers[1] = "GBoF_portalBeam_Offset";
  xfmModifiers[2] = "GBoF_portalBeam_Ground";
  propagateTimeFactor = true;
};

datablock afxXM_SpinData(GBoF_portalBeam_Spin4_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 216;
  spinRate = -30;
};
//
datablock afxEffectWrapperData(GBoF_portalBeam4_EW)
{
  effect     = GBoF_portalBeam_model_CE;
  posConstraint = caster;
  delay = 0.25;
  fadeInTime = 0.1;
  fadeOutTime = 0.75;
  lifetime = 2.25;
  xfmModifiers[0] = "GBoF_portalBeam_Spin4_XM";
  xfmModifiers[1] = "GBoF_portalBeam_Offset";
  xfmModifiers[2] = "GBoF_portalBeam_Ground";
  propagateTimeFactor = true;
};

datablock afxXM_SpinData(GBoF_portalBeam_Spin5_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 288;
  spinRate = -30;
};
//
datablock afxEffectWrapperData(GBoF_portalBeam5_EW)
{
  effect     = GBoF_portalBeam_model_CE;
  posConstraint = caster;
  delay = 0.25;
  fadeInTime = 0.1;
  fadeOutTime = 0.75;
  lifetime = 2.25;
  xfmModifiers[0] = "GBoF_portalBeam_Spin5_XM";
  xfmModifiers[1] = "GBoF_portalBeam_Offset";
  xfmModifiers[2] = "GBoF_portalBeam_Ground";
  propagateTimeFactor = true;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// PORTAL FLAMES

//
// portal flame paths
//
// These paths are used for the particle flames and 
// point lights of the fire-portals. 
//
// Objects following these paths start with a small hovering
// movement then zip up to a point above the caster's head
// where the big fireball appears.
//

datablock afxPathData(GBoF_FirePortalPath1_U)
{
  points = "0 0 0	0 0 .7";  
  lifetime = 1.0;
};

datablock afxPathData(GBoF_FirePortalPath2_U)
{
  points = "0 0 0	0 0.4 1		0 -1.4 2.65";  
  delay  = 2.0;
  lifetime = 0.75;
};

//
// portal flames particles
//
// The following particles and emitter are used to create the 
// five small portal flames that appear to coalesce, forming
// the huge fireball.
//

datablock ParticleData(GBoF_FirePortal_P)
{
   // TGE textureName          = %mySpellDataPath @ "/GBoF/particles/starfire";
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
   sizes[0]             = 0.1;
   sizes[1]             = 0.3;
   sizes[2]             = 0.1;
   sizes[3]             = 0.07;
   times[0]             = 0.0;
   times[1]             = 0.2;
   times[2]             = 0.55;
   times[3]             = 1.0;

   textureName          = %mySpellDataPath @ "/GBoF/particles/gbof_tiled_parts"; // starfire
   textureCoords[0]     = "0.25 0.75";
   textureCoords[1]     = "0.25 1.0";
   textureCoords[2]     = "0.5  1.0";
   textureCoords[3]     = "0.5  0.75";
};
//
datablock ParticleEmitterData(GBoF_FirePortal_E)
{
  ejectionOffset        = 0.02;
  ejectionPeriodMS      = 10;
  periodVarianceMS      = 0;
  ejectionVelocity      = 0.4; //0.8;
  velocityVariance      = 0.1; //0.00;
  thetaMin              = 0.0;
  thetaMax              = 0.0;
  //phiReferenceVel       = 90;
  //phiVariance           = 180;
  particles             = GBoF_FirePortal_P;
};

// the fire-portal modifiers are shared with the 
// fire-portal lights.

// this offset defines the radius of the portal-flames
datablock afxXM_LocalOffsetData(GBoF_FirePortal_Offset_XM)
{
  localOffset = "0 1.5 0";
};

// this modifier conforms the portal flames to a path
datablock afxXM_PathConformData(GBoF_FirePortal_Path_XM)
{
  paths = "GBoF_FirePortalPath1_U GBoF_FirePortalPath2_U";
};

//
// portal flames 
//
// The following effects create the five small portal
// flames.
//

// this and the other spin modifiers set the starting
// angle and rotation rate of the portal-flames.
datablock afxXM_SpinData(GBoF_FirePortal_Spin1_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 15; //0+15
  spinRate = -30;
};
//
datablock afxEffectWrapperData(GBoF_FirePortal1_EW)
{
  effectName = "PortalFlame1";
  isConstraintSrc = true;
  effect = GBoF_FirePortal_E;
  posConstraint = caster;
  delay = 2.0;
  lifetime = 3.75;
  xfmModifiers[0] = GBoF_FirePortal_Spin1_XM;
  xfmModifiers[1] = GBoF_FirePortal_Offset_XM;
  xfmModifiers[2] = GBoF_FirePortal_Path_XM;
};

datablock afxXM_SpinData(GBoF_FirePortal_Spin2_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 87; // 72+15
  spinRate = -30;
};
//
datablock afxEffectWrapperData(GBoF_FirePortal2_EW)
{
  effectName = "PortalFlame2";
  isConstraintSrc = true;
  effect = GBoF_FirePortal_E;
  posConstraint = caster;
  delay = 2.0;
  lifetime = 3.75;
  xfmModifiers[0] = GBoF_FirePortal_Spin2_XM;
  xfmModifiers[1] = GBoF_FirePortal_Offset_XM;
  xfmModifiers[2] = GBoF_FirePortal_Path_XM;
};

datablock afxXM_SpinData(GBoF_FirePortal_Spin3_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 159; //144+15
  spinRate = -30;
};
//
datablock afxEffectWrapperData(GBoF_FirePortal3_EW)
{
  effectName = "PortalFlame3";
  isConstraintSrc = true;
  effect = GBoF_FirePortal_E;
  posConstraint = caster;
  delay = 2.0;
  lifetime = 3.75;
  xfmModifiers[0] = GBoF_FirePortal_Spin3_XM;
  xfmModifiers[1] = GBoF_FirePortal_Offset_XM;
  xfmModifiers[2] = GBoF_FirePortal_Path_XM;
};

datablock afxXM_SpinData(GBoF_FirePortal_Spin4_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 231; //216+15
  spinRate = -30;
};
//
datablock afxEffectWrapperData(GBoF_FirePortal4_EW)
{
  effectName = "PortalFlame4";
  isConstraintSrc = true;
  effect = GBoF_FirePortal_E;
  posConstraint = caster;
  delay = 2.0;
  lifetime = 3.75;
  xfmModifiers[0] = GBoF_FirePortal_Spin4_XM;
  xfmModifiers[1] = GBoF_FirePortal_Offset_XM;
  xfmModifiers[2] = GBoF_FirePortal_Path_XM;
};

datablock afxXM_SpinData(GBoF_FirePortal_Spin5_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 303; // 288+15
  spinRate = -30;
};
//
datablock afxEffectWrapperData(GBoF_FirePortal5_EW)
{
  effectName = "PortalFlame5";
  isConstraintSrc = true;
  effect = GBoF_FirePortal_E;
  posConstraint = caster;
  delay = 2.0;
  lifetime = 3.75;
  xfmModifiers[0] = GBoF_FirePortal_Spin5_XM;
  xfmModifiers[1] = GBoF_FirePortal_Offset_XM;
  xfmModifiers[2] = GBoF_FirePortal_Path_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// HUGE FIREBALL

//
// fireball particles
//
// The following particles and emitter are used to create the 
// huge fireball. They are used both by the missile and effects
// that statically form the fireball over the spellcaster's head.
//

datablock ParticleData(GBoF_HugeFireBall_P)
{
   // TGE textureName          = %mySpellDataPath @ "/GBoF/particles/smokeParticle";
   dragCoeffiecient     = 0.0;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 500;
   lifetimeVarianceMS   = 100;
   useInvAlpha          = false;
   spinRandomMin        = -900.0;
   spinRandomMax        = 900.0;
   colors[0]            = "1.0 1.0 1.0 1.0";
   colors[1]            = "1.0 0.8 0.0 1.0";
   colors[2]            = "1.0 0.0 0.0 1.0";
   sizes[0]             = 1.75;
   sizes[1]             = 0.85;
   sizes[2]             = 0.1;
   times[0]             = 0.0;
   times[1]             = 0.3;
   times[2]             = 1.0;

   textureName          = %mySpellDataPath @ "/GBoF/particles/gbof_tiled_parts"; // smokeParticle
   textureCoords[0]     = "0.75 0.75";
   textureCoords[1]     = "0.75 1.0";
   textureCoords[2]     = "1.0  1.0";
   textureCoords[3]     = "1.0  0.75";
};
//
datablock ParticleData(GBoF_HugeFireBall_P2)
{
   // TGE textureName          = %mySpellDataPath @ "/GBoF/particles/GBofF_fireBall"; //firetest3";
   dragCoeffiecient     = 0.0;
   gravityCoefficient   = 0.0;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 500;
   lifetimeVarianceMS   = 100;
   useInvAlpha          = false;
   spinRandomMin        = -900.0;
   spinRandomMax        = 900.0;
   colors[0]            = "1.0 1.0 0.0 1.0";
   colors[1]            = "1.0 0.0 0.0 1.0";
   colors[2]            = "1.0 0.0 0.0 1.0";
   sizes[0]             = 3.0;
   sizes[1]             = 1.2;
   sizes[2]             = 0.2;
   times[0]             = 0.0;
   times[1]             = 0.3;
   times[2]             = 1.0;

   textureName          = %mySpellDataPath @ "/GBoF/particles/gbof_tiled_parts"; // fireBall
   textureCoords[0]     = "0.0 0.0";
   textureCoords[1]     = "0.0 0.5";
   textureCoords[2]     = "0.5 0.5";
   textureCoords[3]     = "0.5 0.0";
};
//
datablock ParticleEmitterData(GBoF_HugeFireBall_E)
{
  ejectionPeriodMS      = 2;
  periodVarianceMS      = 1;
  ejectionVelocity      = 0.25;
  velocityVariance      = 0.10;
  thetaMin              = 0;
  thetaMax              = 180;
  phiReferenceVel       = 90;
  phiVariance           = 180;
  particles             = "GBoF_HugeFireBall_P GBoF_HugeFireBall_P2"; 
};

//
// fireball location
//
// This offset defines the starting point of the huge fireball
//
datablock afxXM_WorldOffsetData(GBoF_FireBall_Offset_XM)
{
  worldOffset = "0 0 3.4";
};

//
// static fireball
//
// This effect places introduces the huge fireball above
// the spellcaster's head.
// 
datablock afxEffectWrapperData(GBoF_FireBall_Static_EW)
{
  effect = GBoF_HugeFireBall_E;
  constraint = caster;
  delay = 4.6; //3.6; //3.75;
  //lifetime = 1.4; //2.4;
  lifetime = 1.2; //2.4;
  xfmModifiers[0] = GBoF_FireBall_Offset_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPELLCASTER ANIMATION

//
// This clip selects the main animation sequence for casting this
// spell. The spellcaster moves something like a weight lifter to
// concentrate the magic required to summon a very heavy fireball.
//

datablock afxAnimClipData(GBoF_FlameCast_Clip_CE)
{
  clipName = "gbof";
  rate = 1.0;
};
//
datablock afxEffectWrapperData(GBoF_FlameCast_Clip_EW)
{
  effect = GBoF_FlameCast_Clip_CE;
  constraint = "caster";
  lifetime = 7;
  propagateTimeFactor = true;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// explosions

datablock ParticleData(GBoF_ExplosionFire_P)
{
   // TGE textureName          = %mySpellDataPath @ "/GBoF/particles/fireExplosion";
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

   textureName          = %mySpellDataPath @ "/GBoF/particles/gbof_tiled_parts"; // fireExplosion
   textureCoords[0]     = "0.5 0.0";
   textureCoords[1]     = "0.5 0.5";
   textureCoords[2]     = "1.0 0.5";
   textureCoords[3]     = "1.0 0.0";
};
//
datablock ParticleEmitterData(GBoF_ExplosionFire2_E)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 0.8;
   velocityVariance = 0.5;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   lifetimeMS       = 250;
   particles = "GBoF_ExplosionFire_P";
};

datablock ParticleData(GBoF_ExplosionSmoke_P)
{
   // TGE textureName          = %mySpellDataPath @ "/GBoF/particles/smoke";
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

   textureName          = %mySpellDataPath @ "/GBoF/particles/gbof_tiled_parts"; // smoke
   textureCoords[0]     = "0.0  0.75";
   textureCoords[1]     = "0.0  1.0";
   textureCoords[2]     = "0.25 1.0";
   textureCoords[3]     = "0.25 0.75";
};
//
datablock ParticleEmitterData(GBoF_ExplosionSmoke_E)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 0;
   ejectionVelocity = 10;
   velocityVariance = 1.5;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   lifetimeMS       = 250;
   particles        = "GBoF_ExplosionSmoke_P";
};

datablock ParticleData(GBoF_ExplosionBigSmoke_P)
{
   // TGE textureName          = %mySpellDataPath @ "/GBoF/particles/smoke";
   dragCoeffiecient     = 100.0;
   gravityCoefficient   = 0;
   inheritedVelFactor   = 0.25;
   constantAcceleration = -0.30;
   lifetimeMS           = 3000;
   lifetimeVarianceMS   = 500;
   useInvAlpha =  true;
   spinRandomMin = -80.0;
   spinRandomMax =  80.0;

   colors[0]     = "1.0 1.0 1.0 1.0";
   colors[1]     = "1.0 0.5 0.0 0.9";
   colors[2]     = "0.4 0.4 0.4 0.6";
   colors[3]     = "0.0 0.0 0.0 0.0";

   sizes[0]      = 4.5;
   sizes[1]      = 7.0;
   sizes[2]      = 9.0;
   sizes[3]      = 12.0;

   times[0]      = 0.0;
   times[1]      = 0.33;
   times[2]      = 0.66;
   times[3]      = 1.0;

   textureName          = %mySpellDataPath @ "/GBoF/particles/gbof_tiled_parts"; // smoke
   textureCoords[0]     = "0.0  0.75";
   textureCoords[1]     = "0.0  1.0";
   textureCoords[2]     = "0.25 1.0";
   textureCoords[3]     = "0.25 0.75";
};
//
datablock ParticleEmitterData(GBoF_ExplosionBigSmoke_E)
{
   ejectionPeriodMS = 4;
   periodVarianceMS = 0;
   ejectionVelocity = 13;
   velocityVariance = 2.0;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   lifetimeMS       = 250;
   particles = "GBoF_ExplosionBigSmoke_P";
};

//----

datablock ParticleEmitterData(GBoF_ExplosionFire_E)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 0.8;
   velocityVariance = 0.5;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   lifetimeMS       = 250;
   particles        = "GBoF_ExplosionFire_P";
};
//
datablock ExplosionData(GBoF_Explosion_CE)
{
   lifeTimeMS = 1200;

   // Volume particles
   particleEmitter = GBoF_ExplosionFire_E;
   particleDensity = 20; //50;
   particleRadius = 3;

   // Point emission
   emitter[0] = GBoF_ExplosionSmoke_E;
   emitter[1] = GBoF_ExplosionSmoke_E;

   // Impulse
   impulseRadius = 10;
   impulseForce = 15;
};

datablock ExplosionData(GBoF_Explosion2_CE)
{
   lifeTimeMS = 1200;

   // Volume particles
   particleEmitter = GBoF_ExplosionFire2_E;
   particleDensity = 20; //50;
   particleRadius = 3;

   // Point emission
   emitter[0] = GBoF_ExplosionSmoke_E;
   emitter[1] = GBoF_ExplosionSmoke_E;

   // Impulse
   impulseRadius = 10;
   impulseForce = 15;
};

datablock ExplosionData(GBoF_ExplosionBig_CE)
{
   lifeTimeMS = 1200;

   // Volume particles
   particleEmitter = GBoF_ExplosionFire2_E;
   particleDensity = 80;
   particleRadius = 7;

   // Point emission
   emitter[0] = GBoF_ExplosionBigSmoke_E;
   emitter[1] = GBoF_ExplosionBigSmoke_E;
   emitter[2] = GBoF_ExplosionBigSmoke_E;

  
   // Impulse
   impulseRadius = 10;
   impulseForce = 15;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// ON TARGET IMPACT

//
// When the intended target is hit, a three stage explosion occurs,
// First a couple quick explosions, followed by a larger explosion.
//

// explosions

//
// A three-stage explosion... boom, boom, ba-boom!
//

datablock afxEffectWrapperData(GBoF_Explosion1_EW)
{
  effect = GBoF_Explosion_CE;
  constraint = "impactPoint";
  execConditions[0] = $AFX::IMPACTED_SOMETHING;
};

datablock afxEffectWrapperData(GBoF_Explosion2_EW)
{
  effect = GBoF_Explosion2_CE;
  delay = 0.4;
  constraint = "impactedObject.Eye";
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
};

datablock afxEffectWrapperData(GBoF_Explosion3_EW)
{
  effect = GBoF_ExplosionBig_CE;
  delay = 1.2;
  constraint = "impactedObject.Bip01 Spine1";
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
};

// shockwaves

//
// Three fast moving shockwaves that follow the three
// explosions. All very similar, but the third has a 
// little more density, since the third explosion is
// biggest.
//

datablock afxZodiacData(GBoF_ImpactZodeFast_CE)
{  
  texture = %mySpellDataPath @ "/GBoF/zodiacs/zode_impactA";
  radius = 1.0;
  startAngle = 0.0;
  rotationRate = 0.0;  
  color = "1.0 1.0 1.0 0.35";
  blend = additive;
  growthRate = 300.0;
  showOnInteriors = false;
};
//
datablock afxEffectWrapperData(GBoF_ImpactZodeFast1_EW)
{
  effect = GBoF_ImpactZodeFast_CE;
  posConstraint = "impactedObject";
  delay = 0.05;
  fadeInTime = 0.0;
  fadeOutTime = 1.0;
  lifetime = 0.5;
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
};
//
datablock afxEffectWrapperData(GBoF_ImpactZodeFast2_EW)
{
  effect = GBoF_ImpactZodeFast_CE;
  posConstraint = "impactedObject";
  delay = 0.45;
  fadeInTime = 0.0;
  fadeOutTime = 1.0;
  lifetime = 0.5;
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
};

datablock afxZodiacData(GBoF_ImpactZodeFast3_CE)
{  
  texture = %mySpellDataPath @ "/GBoF/zodiacs/zode_impactA";
  radius = 1.0;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 0.6";
  blend = additive;
  growthRate = 300.0;
  showOnInteriors = false;
};
//
datablock afxEffectWrapperData(GBoF_ImpactZodeFast3_EW)
{
  effect = GBoF_ImpactZodeFast3_CE;
  posConstraint = "impactedObject";
  delay = 1.25;
  fadeInTime = 0.0;
  fadeOutTime = 1.0;
  lifetime = 0.5;
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
};

//
// A slow moving white cloud shockwave formed from two
// similar zodiacs that rotate in opposite directions.
//

datablock afxZodiacData(GBoF_EtherealImpactZode1_CE)
{  
  texture = %mySpellDataPath @ "/GBoF/zodiacs/GBofF_impactB-1";
  radius = 1.0;
  startAngle = 0.0;
  rotationRate = 40.0;
  color = "1.0 1.0 1.0 0.5";
  blend = additive;
  growthRate = 30.0;
  showOnInteriors = false;
};
//
datablock afxEffectWrapperData(GBoF_EtherealImpactZode1_EW)
{
  effect = GBoF_EtherealImpactZode1_CE;
  posConstraint = "impactPoint";
  delay = 1.0;
  fadeInTime = 0.25;
  fadeOutTime = 3.25;
  lifetime = 1.75;
  execConditions[0] = $AFX::IMPACTED_TARGET;
};

datablock afxZodiacData(GBoF_EtherealImpactZode2_CE)
{  
  texture = %mySpellDataPath @ "/GBoF/zodiacs/GBofF_impactB-2";
  radius = 1.0;
  startAngle = 0.0;
  rotationRate = -40.0;
  color = "1.0 1.0 1.0 0.5";
  blend = additive;
  growthRate = 30.0;
  showOnInteriors = false;
};
//
datablock afxEffectWrapperData(GBoF_EtherealImpactZode2_EW)
{
  effect = GBoF_EtherealImpactZode2_CE;
  posConstraint = "impactPoint";
  delay = 1.0;
  fadeInTime = 0.25;
  fadeOutTime = 3.25;
  lifetime = 1.75;
  execConditions[0] = $AFX::IMPACTED_TARGET;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SCORCHED EARTH RESIDUE

//
// The scorched earth texture will stick around for 30 seconds
// and slowly fade off.
//

datablock afxZodiacData(GBoF_ScorchedEarth_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/GBoF/zodiacs/GBoF_blastimpact";
  radius = 40.0;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 1.0";
};
//
datablock afxEffectWrapperData(GBoF_ScorchedEarth_EW)
{
  effect = GBoF_ScorchedEarth_CE;
  constraint = "impactPoint";
  delay = 1.25;
  fadeInTime = 0.5;
  lifetime = 0.5;
  residueLifetime = 20;
  fadeOutTime = 5;
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
  xfmModifiers[0] = SHARED_AltitudeConform_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/gbof_lighting_tge_sub.cs");
    exec("./audio/gbof_audio_sub.cs");
  case "TGEA":
    exec("./lighting/gbof_lighting_tgea_sub.cs");
    exec("./audio/gbof_audio_sub.cs");
 case "T3D":
    exec("./lighting/gbof_lighting_t3d_sub.cs");
    exec("./audio/gbof_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// HUGE FIREBALL MISSILE

datablock afxMagicMissileData(GBoF_Fireball)
{
  particleEmitter       = GBoF_HugeFireBall_E;
  muzzleVelocity        = 30; //12;
  velInheritFactor      = 0;
  lifetime              = 20000;
  isBallistic           = true;
  ballisticCoefficient  = 0.95;
  gravityMod            = 0.05;
  isGuided              = true;
  precision             = 30;
  trackDelay            = 7;

  hasLight    = false;

  sound = GBoF_FireBallSnd_CE;

  launchOffset = "0.0 0.0 3.4";
  echoLaunchOffset = false;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GREAT BALL OF FIRE SPELL
//

datablock afxMagicSpellData(GreatBallSpell)
{
    // warmup //
  //castingDur = 5.85; //6.0; //1.2;
  castingDur = 5.7; //6.0; //1.2;

    // lingering //
  lingerDur = 6.0;

    // magic missile //
  missile = GBoF_Fireball;

    // casting zodiac //
  addCastingEffect = GBoF_FixedZodeMooring_EW;
  addCastingEffect = GBoF_Zode_Reveal_EW;

  addCastingEffect = GBoF_Zode1_underglow_EW;
  addCastingEffect = GBoF_Zode1_EW;
  addCastingEffect = GBoF_Zode2_underglow_EW;
  addCastingEffect = GBoF_Zode2_EW;
  addCastingEffect = GBoF_Zode3_EW;
  
    // portal beams //
  addCastingEffect = GBoF_portalBeam1_EW;
  addCastingEffect = GBoF_portalBeam2_EW;
  addCastingEffect = GBoF_portalBeam3_EW;
  addCastingEffect = GBoF_portalBeam4_EW;
  addCastingEffect = GBoF_portalBeam5_EW;

    // portal flames //
  addCastingEffect = GBoF_FirePortal1_EW;
  addCastingEffect = GBoF_FirePortal2_EW;
  addCastingEffect = GBoF_FirePortal3_EW;
  addCastingEffect = GBoF_FirePortal4_EW;
  addCastingEffect = GBoF_FirePortal5_EW;

  
    // portal zodes //
  addCastingEffect = GBoF_ZodePortal1_EW;
  addCastingEffect = GBoF_ZodePortal2_EW;
  addCastingEffect = GBoF_ZodePortal3_EW;
  addCastingEffect = GBoF_ZodePortal4_EW;
  addCastingEffect = GBoF_ZodePortal5_EW;
    // huge fireball //
  addCastingEffect = GBoF_FireBall_Static_EW;  
    // spellcaster animation //
  addCastingEffect = GBoF_FlameCast_Clip_EW;

    // on-target explosions //
  addImpactEffect = GBoF_Explosion1_EW;
  addImpactEffect = GBoF_Explosion2_EW;
  addImpactEffect = GBoF_Explosion3_EW;  
    // shockwaves //
  addImpactEffect = GBoF_EtherealImpactZode1_EW;
  addImpactEffect = GBoF_EtherealImpactZode2_EW;
  addImpactEffect = GBoF_ImpactZodeFast1_EW;
  addImpactEffect = GBoF_ImpactZodeFast2_EW;
  addImpactEffect = GBoF_ImpactZodeFast3_EW;
    // residue //
  addImpactEffect = GBoF_ScorchedEarth_EW;
};

// sounds and lights added via sub-script functions //
GBoF_add_Lighting_FX(GreatBallSpell);
GBoF_add_Audio_FX(GreatBallSpell);

datablock afxRPGMagicSpellData(GreatBallSpell_RPG)
{
  spellName = "Great Ball of Fire";
  desc = "Hurls a ball of FLAMING DEATH that only the most intrepid caster " @
         "dare summon! Goodness Gracious! " @
         "Does 50 damage plus 20 radius damage." @ 
         "\n" @
         "\nspell design: Matthew Durante" @
         "\nsound effects: Dave Schroeder" @ 
         "\nspell concept: Matthew Durante";
  sourcePack = "Core Tech";

  iconBitmap = %mySpellDataPath @ "/GBoF/icons/gbof";
  target = "enemy";
  range = 80;
  manaCost = 10;
  directDamage = 50.0;
  areaDamage = 20;
  areaDamageRadius = 25;
  areaDamageImpulse = 1000;
  castingDur = GreatBallSpell.castingDur;
};

// set a level of detail
function GreatBallSpell::onActivate(%this, %spell, %caster, %target)
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
  GreatBallSpell.scriptFile = $afxAutoloadScriptFile;
  GreatBallSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(GreatBallSpell, GreatBallSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
