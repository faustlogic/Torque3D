
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// OCCAM'S LASER SCI-FI EFFECT
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
$effect_reload = isObject(SciFiDroneSpell);
if ($effect_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = SciFiDroneSpell.spellDataPath;
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

// <Occam's Laser 1> >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
//   The effects defined below are used by SciFiDroneSpell, the first
//    and fundamental variation of the spell.  Two other variations
//    are defined later and reuse much of this.
//

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// DRONE GLOBALS

//
// These variables define often-used data for the drone, permitting
// quick global adjustments to attributes like timing (rather than
// having to hunt-down duplicate constants in a group of datablocks).
//

%SCIFI_Satellite_orbitRadius  = 4.0;
%SCIFI_Satellite_orbitHeight  = 3.0;
%SCIFI_Satellite_spinRate     = -70;
%SCIFI_Satellite_delay        = 1.0;
%SCIFI_Satellite_lifetime     = 8; //9.0;
%SCIFI_Satellite_aim_delay    = 5.0;
%SCIFI_Satellite_aim_fade     = 0.5;
%SCIFI_Satellite_aim_lifetime = 2.0;
%SCIFI_Satellite_OUT_delay    = %SCIFI_Satellite_delay+%SCIFI_Satellite_lifetime-1.0;
%SCIFI_Satellite_spinAngle    = %SCIFI_Satellite_spinRate*%SCIFI_Satellite_delay;


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPACE DRONE

//
// The drone's motion, and also the motion of teleportation and other
// effects, is created with an afxMooring object that is animated
// with xfmModifiers.  The effect objects are then positionally
// constrained to the mooring with effect constraints.  The mooring
// is used because the drone model itself only exists for part of the
// overall effect, but the motion must be continuous through all 
// three "stages": teleportation-in, drone, and teleportation-out.
// It also demonstrates how a mooring can be a useful tool for
// designing an animation independent of specific effects, because
// once it exists it can serve as a common mount point.
//
// Besides the mooring, the drone effect consists of:
//  * drone body dts model (SF_Drone_Body_EW)
//  * drone engine flare dts model (SF_Drone_EngineFlares_EW),
//     seperate from the drone so that unique material properties
//     can be set
//  * drone dust emitter (SF_Drone_Dust_EW), with an
//     afxXM_GroundConform to keep the dust emitting atop the ground
//  * drone lights, with some TLK enhancement ($SF_Drone_PointLight,
//     $SF_Drone_PointLight2)
//

// DRONE MOTION

// This path defines the up and down movement of the drone
datablock afxPathData(SF_Drone_Hover_Path)
{
  points = "0 0  1.0" SPC
           "0 0 -0.8" SPC
           "0 0  0.7" SPC
           "0 0 -1.1" SPC
           "0 0  1.0";
  lifetime = 4.0;
  loop = cycle;
  timeOffset = 3.0;
};
//
datablock afxXM_PathConformData(SF_Drone_HoverHeight_XM)
{
  paths = "SF_Drone_Hover_Path";
};

// This spinner sets the rate at which the drone orbits the caster
datablock afxXM_SpinData(SF_Drone_OrbitRate_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 0;
  spinRate  = %SCIFI_Satellite_spinRate;
};

// This offset sets the radius distance of the drone from the caster and
// the height above the caster's feet at which it hovers.
datablock afxXM_LocalOffsetData(SF_Drone_OrbitOffset_XM)
{
  localOffset = %SCIFI_Satellite_orbitRadius SPC "0" SPC %SCIFI_Satellite_orbitHeight;
};

// This spinner adjusts the bearing of the drone so that
// points away from the caster as it orbits.
datablock afxXM_SpinData(SF_Drone_Bearing_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 90;
  spinRate  = 0;
};

// After a delay, this XM will aim the drone at the target
// and then fall back to the default rotation.
datablock afxXM_AimData(SF_Drone_TargetLock_XM)
{
  aimZOnly    = false;
  delay       = %SCIFI_Satellite_aim_delay;
  lifetime    = %SCIFI_Satellite_aim_lifetime;
  fadeInTime  = %SCIFI_Satellite_aim_fade;
  fadeOutTime = %SCIFI_Satellite_aim_fade;
};
datablock afxXM_AimData(SF_Drone_TargetLock0_XM)
{
  aimZOnly    = false;
  delay       = %SCIFI_Satellite_aim_delay + %SCIFI_Satellite_delay;
  lifetime    = %SCIFI_Satellite_aim_lifetime;
  fadeInTime  = %SCIFI_Satellite_aim_fade;
  fadeOutTime = %SCIFI_Satellite_aim_fade;
};

// DRONE MOORING

datablock afxMooringData(SF_Mooring_CE)
{
  networking = $AFX::CLIENT_ONLY;
  displayAxisMarker = false;
};
//
datablock afxEffectWrapperData(SF_Mooring_EW)
{
  effect = SF_Mooring_CE;
  effectName = "DroneMooring";
  isConstraintSrc = true;

  posConstraint = "caster.#history(0.5/60)"; //"caster.#history(0.5)";
  posConstraint2 = "target.#center";

  lifetime = %SCIFI_Satellite_delay + %SCIFI_Satellite_lifetime + 2.3;

  xfmModifiers[0] = SF_Drone_HoverHeight_XM;
  xfmModifiers[1] = SF_Drone_OrbitRate_XM;
  xfmModifiers[2] = SF_Drone_OrbitOffset_XM;
  xfmModifiers[3] = SF_Drone_Bearing_XM;
  xfmModifiers[4] = SF_Drone_TargetLock0_XM;
};

// DRONE BODY

datablock afxModelData(SF_Drone_Body_CE)
{
  shapeFile = %mySpellDataPath @ "/SF_OL/models/gun_satellite.dts";
  sequence = "roam";
  sequenceRate = 1.0;
  useVertexAlpha = true;
  shadowEnable = true;
};
//
datablock afxEffectWrapperData(SF_Drone_Body_EW)
{
  effect = SF_Drone_Body_CE;
  constraint = "#effect.DroneMooring";
  delay       = %SCIFI_Satellite_delay;
  fadeInTime  = 1.0;
  fadeOutTime = 1.0;
  lifetime    = %SCIFI_Satellite_lifetime;
  scaleFactor = 1.0;
};

// DRONE ENGINE FLARE

datablock afxModelData(SF_Drone_EngineFlares_CE)
{
  shapeFile = %mySpellDataPath @ "/SF_OL/models/gun_satellite_flare.dts";
  sequence = "roam";
  sequenceRate = 1.0;
  useVertexAlpha = true;
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  remapTextureTags = "satellite.png:satellite_flare1 satellite.png:satellite_flare2"; // TGEA (ignored by TGE)
};
//
datablock afxEffectWrapperData(SF_Drone_EngineFlares_EW)
{
  effect = SF_Drone_EngineFlares_CE;
  constraint = "#effect.DroneMooring";
  delay       = %SCIFI_Satellite_delay;
  fadeInTime  = 1.0;
  fadeOutTime = 1.0;
  lifetime    = %SCIFI_Satellite_lifetime;
};

// DRONE DUST

// dark dust
datablock ParticleData(SF_Drone_DarkDust_P)
{
  textureName          = %mySpellDataPath @ "/SF_OL/particles/smoke";
  dragCoeffiecient     = 0.5;
  gravityCoefficient   = 0.2;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 1200;
  lifetimeVarianceMS   = 600;
  useInvAlpha          = true;
  spinRandomMin        = -240.0;
  spinRandomMax        = -70.0;
  colors[0]            = "0.9 0.65 0.23 0.0";
  colors[1]            = "0.9 0.65 0.23 0.30";
  colors[2]            = "0.4 0.33 0.2 0.10";
  colors[3]            = "0.4 0.33 0.2 0.0";
  sizes[0]             = 0.7;
  sizes[1]             = 1.5;
  sizes[2]             = 3.0;
  sizes[3]             = 7.0;
  times[0]             = 0.0;
  times[1]             = 0.2;
  times[2]             = 0.7;
  times[3]             = 1.0;
};
// light dust
datablock ParticleData(SF_Drone_LightDust_P : SF_Drone_DarkDust_P)
{
  colors[0]            = "1.0 0.83 0.55 0.0";
  colors[1]            = "1.0 0.83 0.55 0.25";
  colors[2]            = "0.66 0.55 0.33 0.10";
  colors[3]            = "0.66 0.55 0.33 0.0";
};
//
datablock afxParticleEmitterConeData(SF_Drone_Dust_E) // TGEA
{
  ejectionPeriodMS  = 10;
  periodVarianceMS  = 3;
  ejectionVelocity  = 4.0;
  velocityVariance  = 1.4;
  particles         = "SF_Drone_DarkDust_P SF_Drone_LightDust_P";
  // TGE emitterType = "cone";
  vector = "0 0 1";
  spreadMin = 20.0;
  spreadMax = 179.0;
  fadeColor = true;
};
//
datablock afxXM_GroundConformData(SF_Dust_Ground_XM)
{
  height = 0.5;
  //conformOrientation = true;
};
//
datablock afxEffectWrapperData(SF_Drone_Dust_EW)
{
  effect = SF_Drone_Dust_E;
  posConstraint = "#effect.DroneMooring";
  delay       = %SCIFI_Satellite_delay;
  fadeInTime  = 1.0;
  fadeOutTime = 1.0;
  lifetime    = %SCIFI_Satellite_lifetime;
  xfmModifiers[0] = SF_Dust_Ground_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// DRONE TELEPORTATION -- IN

//
// The drone teleportation effect is mostly done with particles.
// Three particle types are used: a beam particle that forms the core
// of the teleportation volume, and horizontal and vertical dashes
// that add a Trecky touch.  There are also some lighting effects,
// but see "DRONE TELEPORTATION -- LIGHTING" for those.
//

// Horizontal Dash Particles (red and blue)
//  -- contrainPos is used to keep the particles in the emitter's
//      space, rather than trailing off
datablock ParticleData(SF_Tele_Dash_Red_P)
{
  textureName          = %mySpellDataPath @ "/SF_OL/particles/teleport_horizontalDash";
  dragCoeffiecient     = 0;
  gravityCoefficient   = 0;
  windCoefficient      = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 1200;
  lifetimeVarianceMS   = 0;
  useInvAlpha          = false;
  spinRandomMin        = 0.0;
  spinRandomMax        = 0.0;
  colors[0]            = 240/255 SPC 192/255 SPC 255/255 SPC "0.0";
  colors[1]            = 240/255 SPC 192/255 SPC 255/255 SPC "1.0";
  colors[2]            = 240/255 SPC 192/255 SPC 255/255 SPC "1.0";
  colors[3]            = 240/255 SPC 192/255 SPC 255/255 SPC "0.0";
  sizes[0]             = 0.3;
  sizes[1]             = 0.3;
  sizes[2]             = 0.3;
  sizes[3]             = 0.3;
  times[0]             = 0.0;
  times[1]             = 0.3;
  times[2]             = 0.9;
  times[3]             = 1.0;
  constrainPos         = !$UseEmitterTransform;
};
//
datablock ParticleData(SF_Tele_Dash_Blue_P : SF_Tele_Dash_Red_P)
{
  colors[0]            = 173/255 SPC 171/255 SPC 255/255 SPC "0.0";
  colors[1]            = 173/255 SPC 171/255 SPC 255/255 SPC "1.0";
  colors[2]            = 173/255 SPC 171/255 SPC 255/255 SPC "1.0";
  colors[3]            = 173/255 SPC 171/255 SPC 255/255 SPC "0.0";
};

// Horizontal Dash Emitters (disc emitters)
datablock afxParticleEmitterDiscData(SF_Tele_Dash_UP_E) // TGEA
{
  ejectionPeriodMS      = 30;
  periodVarianceMS      = 10;
  ejectionVelocity      = 2.0;
  velocityVariance      = 0.5;
  particles             = "SF_Tele_Dash_Red_P SF_Tele_Dash_Blue_P";

  // TGE emitterType = "disc";
  vector = "0 0 1";
  radiusMin = 1.20;
  radiusMax = 1.3;

  fadeColor = true;
  useEmitterTransform = $UseEmitterTransform;
};
//
datablock afxParticleEmitterDiscData(SF_Tele_Dash_DOWN_E : SF_Tele_Dash_UP_E) // TGEA
{
  vector = "0 0 -1";
};

// Vertical Bar Particles
//  -- contrainPos is used to keep the particles in the emitter's
//      space, rather than trailing off
datablock ParticleData(SF_Tele_Bars_P)
{
  textureName          = %mySpellDataPath @ "/SF_OL/particles/teleport_verticalDash";
  dragCoeffiecient     = 0;
  gravityCoefficient   = 0;
  windCoefficient      = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 1000;
  lifetimeVarianceMS   = 0;
  useInvAlpha          = false;
  spinRandomMin        = 0.0;
  spinRandomMax        = 0.0;
  colors[0]            = "1 1 1 0";
  colors[1]            = 255/255 SPC 255/255 SPC 255/255 SPC "1.0";
  colors[2]            = 255/255 SPC 255/255 SPC 255/255 SPC "1.0";
  colors[3]            = "1 1 1 0";
  sizes[0]             = 0.5;
  sizes[1]             = 1.0;
  sizes[2]             = 2.0;
  sizes[3]             = 0.5;
  times[0]             = 0.0;
  times[1]             = 0.3;
  times[2]             = 0.7;
  times[3]             = 1.0;
  constrainPos         = !$UseEmitterTransform;
};

// Vertical Bar Emitter
datablock ParticleEmitterData(SF_Tele_Bars_E)
{
  ejectionPeriodMS      = 300;
  periodVarianceMS      = 100;
  ejectionVelocity      = 1.0;
  velocityVariance      = 0.5;
  particles             = "SF_Tele_Bars_P";
  // TGE emitterType = "sprinkler";
  fadeColor   = true;
  useEmitterTransform = $UseEmitterTransform;
};

// Beam Particles
//  -- this particle type forms the main body of the effect
//  -- contrainPos is used to keep the particles in the emitter's
//      space, rather than trailing off
datablock ParticleData(SF_Tele_Beam_P)
{
  textureName          = %mySpellDataPath @ "/SF_OL/particles/teleport_beam";
  dragCoeffiecient     = 0;
  gravityCoefficient   = 0;
  windCoefficient      = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 1200;
  lifetimeVarianceMS   = 0;
  useInvAlpha          = false;
  spinRandomMin        = 0.0;
  spinRandomMax        = 0.0;
  colors[0]            = "1 1 1 0";
  colors[1]            = 255/255 SPC 255/255 SPC 255/255 SPC "1.0";
  colors[2]            = 255/255 SPC 255/255 SPC 255/255 SPC "1.0";
  colors[3]            = "1 1 1 0";
  sizes[0]             = 3.5;
  sizes[1]             = 3.5;
  sizes[2]             = 3.5;
  sizes[3]             = 3.5;
  times[0]             = 0.0;
  times[1]             = 0.2;
  times[2]             = 0.8;
  times[3]             = 1.0;
  constrainPos         = !$UseEmitterTransform;
};

// Beam Emitter (path emitter)
//  -- particles are emitted along the following vertical path
datablock afxPathData(SF_Tele_Beam_Path)
{
  points = "0 0  1.0" SPC
           "0 0  0.3" SPC
           "0 0 -0.3" SPC
           "0 0 -1.0";
};
//  -- note the beam emitter has no velocity, which keep the
//      particles in a "solid" shape
datablock afxParticleEmitterPathData(SF_Tele_Beam_E) // TGEA
{
  ejectionPeriodMS = 30;
  periodVarianceMS = 5;
  ejectionVelocity = 0.0;
  velocityVariance = 0.0;
  particles        = SF_Tele_Beam_P;

  // TGE emitterType = "path";
  pathOrigin  = "origin"; // origin point vector tangent
  paths = "SF_Tele_Beam_Path";

  fadeSize = true;
  fadeColor = true;
  useEmitterTransform = $UseEmitterTransform;
};

//  TELEPORTATION -- IN TRANSFORM MODIFIERS

// Dashes :
//
datablock afxPathData(SF_Tele_Dashes_UP_Path)
{
  points = "0 0 0" SPC
           "0 0 -2.0";
};
datablock afxXM_PathConformData(SF_Tele_Dashes_UP_Path_XM)
{
  paths = "SF_Tele_Dashes_UP_Path";
};
//
datablock afxPathData(SF_Tele_Dashes_DOWN_Path)
{
  points = "0 0 0" SPC
           "0 0 2.0";
};
datablock afxXM_PathConformData(SF_Tele_Dashes_DOWN_Path_XM)
{
  paths = "SF_Tele_Dashes_DOWN_Path";
};

// Bars :
//  Radius of spins
datablock afxXM_LocalOffsetData(SF_Tele_Bars_Offset_XM)
{
  localOffset = 1.3 SPC "0" SPC "0";
};
//  Various spins for variety and randomness
datablock afxXM_SpinData(SF_Tele_Bars_Spin1_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 0;
  spinRate  = 180;
};
datablock afxXM_SpinData(SF_Tele_Bars_Spin2_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 180;
  spinRate  = -180;
};
datablock afxXM_SpinData(SF_Tele_Bars_Spin3_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 89;
  spinRate  = 123;
};
datablock afxXM_SpinData(SF_Tele_Bars_Spin4_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = -66;
  spinRate  = -150;
};
datablock afxXM_SpinData(SF_Tele_Bars_Spin5_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 66;
  spinRate  = 75;
};
datablock afxXM_SpinData(SF_Tele_Bars_Spin6_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 288;
  spinRate  = -93;
};
//  Various paths for randomness
datablock afxPathData(SF_Tele_Bars_Path1)
{
  points = "0 0  0.0" SPC
           "0 0 -1.0" SPC
           "0 0 -0.4";
};
datablock afxPathData(SF_Tele_Bars_Path2)
{
  points = "0 0  0.4" SPC
           "0 0 -0.2" SPC
           "0 0  0.8";
};
datablock afxPathData(SF_Tele_Bars_Path3)
{
  points = "0 0  0.8" SPC
           "0 0 -1.0";
};
datablock afxPathData(SF_Tele_Bars_Path4)
{
  points = "0 0 -0.5" SPC
           "0 0  1.0" SPC
           "0 0  0.6";
};
datablock afxPathData(SF_Tele_Bars_Path5)
{
  points = "0 0  0.3" SPC
           "0 0  0.8" SPC
           "0 0 -0.8";
};
datablock afxPathData(SF_Tele_Bars_Path6)
{
  points = "0 0  0.5" SPC
           "0 0 -0.7" SPC
           "0 0 -0.1";
};
datablock afxXM_PathConformData(SF_Tele_Bars_Path1_XM)
{
  paths = "SF_Tele_Bars_Path1";
};
datablock afxXM_PathConformData(SF_Tele_Bars_Path2_XM)
{
  paths = "SF_Tele_Bars_Path2";
};
datablock afxXM_PathConformData(SF_Tele_Bars_Path3_XM)
{
  paths = "SF_Tele_Bars_Path3";
};
datablock afxXM_PathConformData(SF_Tele_Bars_Path4_XM)
{
  paths = "SF_Tele_Bars_Path4";
};
datablock afxXM_PathConformData(SF_Tele_Bars_Path5_XM)
{
  paths = "SF_Tele_Bars_Path5";
};
datablock afxXM_PathConformData(SF_Tele_Bars_Path6_XM)
{
  paths = "SF_Tele_Bars_Path6";
};

// Teleportation Dashes
//  Two effects are used: the first moves down while emitting particles
//   up, while the other does the opposite.  The reason for this is to
//   get the particles to overlap and become denser...
datablock afxEffectWrapperData(SF_Tele_IN_Dashes_UP_EW)
{
  effect = SF_Tele_Dash_UP_E;
  posConstraint = "#effect.DroneMooring";
  delay       = 0;
  fadeInTime  = 0.3;
  fadeOutTime = 0.3;
  lifetime    = 3.0;
  xfmModifiers[0] = SF_Tele_Dashes_UP_Path_XM;
};
datablock afxEffectWrapperData(SF_Tele_IN_Dashes_DOWN_EW : SF_Tele_IN_Dashes_UP_EW)
{
  effect = SF_Tele_Dash_DOWN_E;
  xfmModifiers[0] = SF_Tele_Dashes_DOWN_Path_XM;
};

// Teleportation Bars
//  The combination of random spinning at a offset (radius) and path
//   movement is an effort to get a random distribution of these bars
//   on the edge of the teleportation "volume".
datablock afxEffectWrapperData(SF_Tele_IN_Bars_1_EW)
{
  effect = SF_Tele_Bars_E;
  posConstraint = "#effect.DroneMooring";
  delay       = 0;
  fadeInTime  = 0.3;
  fadeOutTime = 0.3;
  lifetime    = 3.0;
  xfmModifiers[0] = SF_Tele_Bars_Path1_XM;
  xfmModifiers[1] = SF_Tele_Bars_Spin1_XM;
  xfmModifiers[2] = SF_Tele_Bars_Offset_XM;
};
datablock afxEffectWrapperData(SF_Tele_IN_Bars_2_EW : SF_Tele_IN_Bars_1_EW)
{
  xfmModifiers[0] = SF_Tele_Bars_Path2_XM;
  xfmModifiers[1] = SF_Tele_Bars_Spin2_XM;
};
datablock afxEffectWrapperData(SF_Tele_IN_Bars_3_EW : SF_Tele_IN_Bars_1_EW)
{
  xfmModifiers[0] = SF_Tele_Bars_Path3_XM;
  xfmModifiers[1] = SF_Tele_Bars_Spin3_XM;
};
datablock afxEffectWrapperData(SF_Tele_IN_Bars_4_EW : SF_Tele_IN_Bars_1_EW)
{
  xfmModifiers[0] = SF_Tele_Bars_Path4_XM;
  xfmModifiers[1] = SF_Tele_Bars_Spin4_XM;
};
datablock afxEffectWrapperData(SF_Tele_IN_Bars_5_EW : SF_Tele_IN_Bars_1_EW)
{
  xfmModifiers[0] = SF_Tele_Bars_Path5_XM;
  xfmModifiers[1] = SF_Tele_Bars_Spin5_XM;
};
datablock afxEffectWrapperData(SF_Tele_IN_Bars_6_EW : SF_Tele_IN_Bars_1_EW)
{
  xfmModifiers[0] = SF_Tele_Bars_Path6_XM;
  xfmModifiers[1] = SF_Tele_Bars_Spin6_XM;
};

// Teleportation Beam
datablock afxEffectWrapperData(SF_Tele_IN_Beam_EW)
{
  effect = SF_Tele_Beam_E;
  posConstraint = "#effect.DroneMooring";
  delay       = 0;
  fadeInTime  = 0.3;
  fadeOutTime = 0.3;
  lifetime    = 3.0;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// DRONE TELEPORTATION -- OUT

//
// For the end teleportion, all the effects defined in "DRONE
// TELEPORTATION -- IN" are reused, but with a long delay for proper
// timing.
//

datablock afxEffectWrapperData(SF_Tele_OUT_Dashes_UP_EW : SF_Tele_IN_Dashes_UP_EW)
{
  delay = %SCIFI_Satellite_OUT_delay;
};
datablock afxEffectWrapperData(SF_Tele_OUT_Dashes_DOWN_EW : SF_Tele_IN_Dashes_DOWN_EW)
{
  delay = %SCIFI_Satellite_OUT_delay;
};
datablock afxEffectWrapperData(SF_Tele_OUT_Bars_1_EW : SF_Tele_IN_Bars_1_EW)
{
  delay = %SCIFI_Satellite_OUT_delay;
};
datablock afxEffectWrapperData(SF_Tele_OUT_Bars_2_EW : SF_Tele_IN_Bars_2_EW)
{
  delay = %SCIFI_Satellite_OUT_delay;
};
datablock afxEffectWrapperData(SF_Tele_OUT_Bars_3_EW : SF_Tele_IN_Bars_3_EW)
{
  delay = %SCIFI_Satellite_OUT_delay;
};
datablock afxEffectWrapperData(SF_Tele_OUT_Bars_4_EW : SF_Tele_IN_Bars_4_EW)
{
  delay = %SCIFI_Satellite_OUT_delay;
};
datablock afxEffectWrapperData(SF_Tele_OUT_Bars_5_EW : SF_Tele_IN_Bars_5_EW)
{
  delay = %SCIFI_Satellite_OUT_delay;
};
datablock afxEffectWrapperData(SF_Tele_OUT_Bars_6_EW : SF_Tele_IN_Bars_6_EW)
{
  delay = %SCIFI_Satellite_OUT_delay;
};
datablock afxEffectWrapperData(SF_Tele_OUT_Beam_EW : SF_Tele_IN_Beam_EW)
{
  delay = %SCIFI_Satellite_OUT_delay;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// LASER BEAMS

//
// The main laser beam effect is something of a hack: tubes with
// animated scales and animated visibility are constrained to the
// mooring in just the same way as the drone itself, and as their
// sequence is played the timing is such that the lasers "hit" when
// the delivery phrase ends, at which time a hit automagically occurs
// and the impact phrase executes.  Clearly, no projectile is really
// being launched, no intervening object can block the laser, and no
// hit is really occuring; the effect is entirely visual.  However
// since the drone is target-locked to the target during the period
// of laser fire, the lasers do appear to strike the target.
//
// This approach was taken for lack of a true laser projectile class,
// which would require new coding.
//
// The laser beams are seperated into left and right due to a
// rendering bug that was occuring in TGE 1.5 when both were included
// in the same dts.
//
// If advanced lighting is used, a large red point light is included.
// To better mimic the laser's light an efficient cylindrical light
// source would probably be needed, which also would require new
// coding!  (And is there such a thing?)
//

// laser beams, left and right
datablock afxModelData(SF_Beam_Blast_LF_CE)
{
  shapeFile = %mySpellDataPath @ "/SF_OL/models/beam_blast_LF.dts";
  sequence = "blast";
  sequenceRate = 1.0;
  forceOnMaterialFlags = $MaterialFlags::SelfIlluminating;
};
datablock afxModelData(SF_Beam_Blast_RT_CE : SF_Beam_Blast_LF_CE)
{
  shapeFile = %mySpellDataPath @ "/SF_OL/models/beam_blast_RT.dts";
};
//
datablock afxEffectWrapperData(SF_Beam_Blast_LF_EW : SF_Drone_Body_EW)
{
  effect = SF_Beam_Blast_LF_CE;
  fadeInTime  = 0.0;
  fadeOutTime = 0.0;
};
datablock afxEffectWrapperData(SF_Beam_Blast_RT_EW : SF_Drone_Body_EW)
{
  effect = SF_Beam_Blast_RT_CE;
  fadeInTime  = 0.0;
  fadeOutTime = 0.0;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// IMPACT EFFECTS

//
// Upon impact, particles are used to simulate the outpouring energy,
// in conjunction with a pulsating light.  Then, as the ground is
// scorched, small lingering fires appear that send smoke curling up
// into the air.
//
// The most interesting technique demonstrated here is the use of spin
// modifiers on vector particle emitters, which creates the curving
// tendrils of smoke.  Since the vectors should not rotate past +-90
// degrees, timing parameters must be used and thus many modifiers,
// so the technique is unfortunately complex.  An oscillating
// transform modifier would solve this, something perhaps based on the
// sine function.
//

// Beam Impact Pulse
//  -- note how the size is set to "see-saw"; this is what gives the
//      particle its oscillating appearance
datablock ParticleData(SF_Beam_Pulse_P)
{
  textureName          = %mySpellDataPath @ "/SF_OL/particles/beam_pulse";
  dragCoeffiecient     = 0;
  gravityCoefficient   = 0;
  windCoefficient      = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 300;
  lifetimeVarianceMS   = 100;
  useInvAlpha          = false;
  spinRandomMin        = 0.0;
  spinRandomMax        = 0.0;
  colors[0]            = "1 1 1 1";
  colors[1]            = "1 1 1 1";
  colors[2]            = "1 1 1 1";
  colors[3]            = "1 1 1 0";
  sizes[0]             = 2.0;
  sizes[1]             = 0.7;
  sizes[2]             = 4.0;
  sizes[3]             = 1.5;
  times[0]             = 0.0;
  times[1]             = 0.3;
  times[2]             = 0.7;
  times[3]             = 1.0;
};
//
datablock ParticleEmitterData(SF_Beam_Pulse_E)
{
  ejectionPeriodMS = 30;
  periodVarianceMS = 5;
  ejectionVelocity = 1.0;
  velocityVariance = 0.5;
  particles        = SF_Beam_Pulse_P;
};
//
datablock afxEffectWrapperData(SF_Beam_Pulse_EW)
{
  effect = SF_Beam_Pulse_E;
  constraint = "impactedObject";

  delay       = 0;
  fadeInTime  = 0.0;
  fadeOutTime = 0.0;
  lifetime    = 0.7;
};

// Beam Fire/Smoke Positions
//  The following offsets define the relative positions of the fires
//   that are started by the laser, and from which smoke curls.
datablock afxXM_LocalOffsetData(SF_Beam_Impact_Fire_1_offset_XM)
{
  localOffset = "0.5 0.5 0";
};
datablock afxXM_LocalOffsetData(SF_Beam_Impact_Fire_2_offset_XM)
{
  localOffset = "0.1 0.75 0";
};
datablock afxXM_LocalOffsetData(SF_Beam_Impact_Fire_3_offset_XM)
{
  localOffset = "0.65 0.0 0";
};
datablock afxXM_LocalOffsetData(SF_Beam_Impact_Fire_4_offset_XM)
{
  localOffset = "-0.5 -0.25 0";
};
datablock afxXM_LocalOffsetData(SF_Beam_Impact_Fire_5_offset_XM)
{
  localOffset = "-.15 -0.5 0";
};

// Beam fire/smoke ground conform
datablock afxXM_GroundConformData(SF_Beam_Fire_Ground_XM)
{
  height = 0.3;
};

// Beam Impact Fires
datablock ParticleData(SF_Beam_Impact_Fire_P)
{
  textureName          = %mySpellDataPath @ "/SF_OL/particles/beam_smoke_fire";
  dragCoeffiecient     = 0;
  gravityCoefficient   = 0;
  windCoefficient      = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 300;
  lifetimeVarianceMS   = 100;
  useInvAlpha          = false;
  spinRandomMin        = 0.0;
  spinRandomMax        = 0.0;
  colors[0]            = "1 1 1 1";
  colors[1]            = "1 1 1 1";
  colors[2]            = "1 1 1 1";
  colors[3]            = "1 1 1 0";
  sizes[0]             = 0.2;
  sizes[1]             = 0.4;
  sizes[2]             = 0.24;
  sizes[3]             = 0.3;
  times[0]             = 0.0;
  times[1]             = 0.3;
  times[2]             = 0.7;
  times[3]             = 1.0;
};
datablock ParticleEmitterData(SF_Beam_Impact_Fire_E)
{
  ejectionPeriodMS = 30;
  periodVarianceMS = 5;
  ejectionVelocity = 0.0;
  velocityVariance = 0.0;
  particles        = SF_Beam_Impact_Fire_P;
  fadeSize = true;
};
//
datablock ParticleData(SF_Beam_Impact_SmallFire_P : SF_Beam_Impact_Fire_P)
{
  sizes[0]             = 0.1;
  sizes[1]             = 0.2;
  sizes[2]             = 0.12;
  sizes[3]             = 0.15;
};
datablock ParticleEmitterData(SF_Beam_Impact_SmallFire_E : SF_Beam_Impact_Fire_E)
{
  particles = SF_Beam_Impact_SmallFire_P;
};
//
datablock afxEffectWrapperData(SF_Beam_Impact_Fire_1_EW)
{
  effect = SF_Beam_Impact_Fire_E;
  constraint = "impactPoint";
  delay       = 0;
  fadeInTime  = 0.2;
  fadeOutTime = 0.5;
  lifetime    = 1.5;

  xfmModifiers[0] = SF_Beam_Fire_Ground_XM;
  xfmModifiers[1] = SF_Beam_Impact_Fire_1_offset_XM;
  xfmModifiers[2] = "";
};
datablock afxEffectWrapperData(SF_Beam_Impact_Fire_2_EW : SF_Beam_Impact_Fire_1_EW)
{
  effect = SF_Beam_Impact_SmallFire_E;
  delay  = 0.2;
  xfmModifiers[0] = SF_Beam_Fire_Ground_XM;
  xfmModifiers[1] = SF_Beam_Impact_Fire_2_offset_XM;
  xfmModifiers[2] = "";
};
datablock afxEffectWrapperData(SF_Beam_Impact_Fire_3_EW : SF_Beam_Impact_Fire_1_EW)
{
  effect = SF_Beam_Impact_SmallFire_E;
  delay  = 0.4;
  xfmModifiers[0] = SF_Beam_Fire_Ground_XM;
  xfmModifiers[1] = SF_Beam_Impact_Fire_3_offset_XM;
  xfmModifiers[2] = "";
};
datablock afxEffectWrapperData(SF_Beam_Impact_Fire_4_EW : SF_Beam_Impact_Fire_1_EW)
{
  effect = SF_Beam_Impact_Fire_E;
  delay  = 0.6;
  xfmModifiers[0] = SF_Beam_Fire_Ground_XM;
  xfmModifiers[1] = SF_Beam_Impact_Fire_4_offset_XM;
  xfmModifiers[2] = "";
};
datablock afxEffectWrapperData(SF_Beam_Impact_Fire_5_EW : SF_Beam_Impact_Fire_1_EW)
{
  effect = SF_Beam_Impact_SmallFire_E;
  delay  = 0.8;
  xfmModifiers[0] = SF_Beam_Fire_Ground_XM;
  xfmModifiers[1] = SF_Beam_Impact_Fire_5_offset_XM;
  xfmModifiers[2] = "";
};

// Beam Impact Smoke
datablock ParticleData(SF_Beam_Smoke_P)
{
  textureName          = %mySpellDataPath @ "/SF_OL/particles/beam_pulse";
  dragCoeffiecient     = 0.5;
  gravityCoefficient   = -0.4;
  windCoefficient      = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 2000;
  lifetimeVarianceMS   = 200;
  useInvAlpha          = false;
  spinRandomMin        = 0.0;
  spinRandomMax        = 0.0;
  colors[0]            = "0.3 0.3 0.3 0.3";
  colors[1]            = "0.2 0.2 0.2 0.2";
  colors[2]            = "0.1 0.1 0.1 0.1";
  colors[3]            = "0.0 0.0 0.0 0.0";
  sizes[0]             = 0.2;
  sizes[1]             = 0.7;
  sizes[2]             = 1.3;
  sizes[3]             = 2.0;
  times[0]             = 0.0;
  times[1]             = 0.3;
  times[2]             = 0.7;
  times[3]             = 1.0;
};
// vector emitter pointing straight up
datablock afxParticleEmitterVectorData(SF_Beam_Smoke_E) // TGEA
{
  ejectionPeriodMS = 20; //50;
  periodVarianceMS = 0;
  ejectionVelocity = 1.0;
  velocityVariance = 0.0;
  particles        = SF_Beam_Smoke_P;
  // TGE emitterType = "vector";
  vector = "0.0 0.0 1.0";
  fadeColor = true;
};
// smoke spin modifiers
//  -- a series of modifiers, fading-in and out sequentially with
//      time, are defined for the X axis and the Y axis
datablock afxXM_SpinData(SF_Beam_Smoke_spinX1_XM)
{
  spinAxis  = "1 0 0";
  spinAngle = -90;
  spinRate  = 200;

  delay       = 0;
  lifetime    = 0.3;
  fadeInTime  = 0.1;
  fadeOutTime = 0.1;
};
datablock afxXM_SpinData(SF_Beam_Smoke_spinX2_XM)
{
  spinAxis  = "1 0 0";
  spinAngle = 90;
  spinRate  = -225;

  delay       = 0.3;
  lifetime    = 0.3;
  fadeInTime  = 0.1;
  fadeOutTime = 0.1;
};
datablock afxXM_SpinData(SF_Beam_Smoke_spinX3_XM : SF_Beam_Smoke_spinX1_XM)
{
  delay       = 0.6;
};
datablock afxXM_SpinData(SF_Beam_Smoke_spinX4_XM : SF_Beam_Smoke_spinX2_XM)
{
  delay       = 0.9;
};
//
datablock afxXM_SpinData(SF_Beam_Smoke_spinY1_XM)
{
  spinAxis  = "0 1 0";
  spinAngle = -90;
  spinRate  = 180;

  delay       = 0;
  lifetime    = 0.4;
  fadeInTime  = 0.1;
  fadeOutTime = 0.1;
};
datablock afxXM_SpinData(SF_Beam_Smoke_spinY2_XM)
{
  spinAxis  = "0 1 0";
  spinAngle = 90;
  spinRate  = -240;

  delay       = 0.4;
  lifetime    = 0.4;
  fadeInTime  = 0.1;
  fadeOutTime = 0.1;
};
datablock afxXM_SpinData(SF_Beam_Smoke_spinY3_XM : SF_Beam_Smoke_spinY1_XM)
{
  delay       = 0.8;
};
// beam smoke
//  -- note how the ordering of spin modifiers is done a little
//      differently for each tendril, which makes each look somewhat
//      unique
datablock afxEffectWrapperData(SF_Beam_Smoke_1_EW : SF_Beam_Impact_Fire_1_EW)
{
  effect = SF_Beam_Smoke_E;
  delay  = 0;
  xfmModifiers[4] = SF_Beam_Fire_Ground_XM;
  xfmModifiers[5] = SF_Beam_Impact_Fire_1_offset_XM;
  xfmModifiers[0] = SF_Beam_Smoke_spinX4_XM;
  xfmModifiers[1] = SF_Beam_Smoke_spinX3_XM;
  xfmModifiers[2] = SF_Beam_Smoke_spinX2_XM;
  xfmModifiers[3] = SF_Beam_Smoke_spinX1_XM;
};
datablock afxEffectWrapperData(SF_Beam_Smoke_2_EW : SF_Beam_Smoke_1_EW)
{
  delay           = 0.2;
  xfmModifiers[0] = SF_Beam_Fire_Ground_XM;
  xfmModifiers[1] = SF_Beam_Impact_Fire_2_offset_XM;
  xfmModifiers[2] = SF_Beam_Smoke_spinY3_XM;
  xfmModifiers[3] = SF_Beam_Smoke_spinY2_XM;
  xfmModifiers[4] = SF_Beam_Smoke_spinY1_XM;
  xfmModifiers[5] = "";
};
datablock afxEffectWrapperData(SF_Beam_Smoke_3_EW : SF_Beam_Smoke_1_EW)
{
  delay           = 0.4;
  xfmModifiers[0] = SF_Beam_Fire_Ground_XM;
  xfmModifiers[1] = SF_Beam_Impact_Fire_3_offset_XM;
  xfmModifiers[2] = SF_Beam_Smoke_spinX4_XM;
  xfmModifiers[3] = SF_Beam_Smoke_spinY2_XM;
  xfmModifiers[4] = SF_Beam_Smoke_spinY1_XM;
  xfmModifiers[5] = SF_Beam_Smoke_spinX1_XM;
};
datablock afxEffectWrapperData(SF_Beam_Smoke_4_EW : SF_Beam_Smoke_1_EW)
{
  delay           = 0.6;
  xfmModifiers[0] = SF_Beam_Fire_Ground_XM;
  xfmModifiers[1] = SF_Beam_Impact_Fire_4_offset_XM;
  xfmModifiers[2] = SF_Beam_Smoke_spinY3_XM;
  xfmModifiers[3] = SF_Beam_Smoke_spinX3_XM;
  xfmModifiers[4] = SF_Beam_Smoke_spinX1_XM;
  xfmModifiers[5] = SF_Beam_Smoke_spinY1_XM;
};
datablock afxEffectWrapperData(SF_Beam_Smoke_5_EW : SF_Beam_Smoke_1_EW)
{
  delay           = 0.8;
  xfmModifiers[0] = SF_Beam_Fire_Ground_XM;
  xfmModifiers[1] = SF_Beam_Impact_Fire_5_offset_XM;
  xfmModifiers[2] = SF_Beam_Smoke_spinX4_XM;
  xfmModifiers[3] = SF_Beam_Smoke_spinY2_XM;
  xfmModifiers[4] = SF_Beam_Smoke_spinX1_XM;
  xfmModifiers[5] = "";
};

// Beam Impact Scorched Earth
datablock afxZodiacData(SF_ScorchedEarth_CE : SHARED_ZodiacBase_CE)
{
  texture = %mySpellDataPath @ "/SF_OL/zodiacs/OL_magicBurn";
  radius = 3.0;
  growInTime = 1.5;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 0.9";
  altitudeFalloff = 0.0;
  altitudeMax = 5.0;
};
//
datablock afxEffectWrapperData(SF_ScorchedEarth_EW)
{
  effect = SF_ScorchedEarth_CE;
  constraint = "impactPoint";
  delay = 0.0;
  fadeInTime = 1.0;
  lifetime = 1.0;
  residueLifetime = 30;
  fadeOutTime = 5;
  execConditions[0] = $AFX::IMPACTED_SOMETHING;
  xfmModifiers[0] = SHARED_freeze_AltitudeConform_XM;
};

//
// <Occam's Laser 1> <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


// <Occam's Laser 2> >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
//   The effects that follow are defined for the second variation of
//    Occam's Laser, SciFiDrone2Spell.  Here the laser beam is
//    replaced with an afxMachineGun.
//

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPACE DRONE (RELOADED - MACHINE GUN)

//
// The mooring that controlled the drone for the previous spell
// existed only on the client, but in this variation an afxMachineGun
// is being used, and this effect exists only on the server.
// Therefore, in order to constrain the new laser blasts to the drone,
// its mooring must also exist on the server.  Client objects then
// constrain to the ghost of the mooring.
//
// Most of the effects defined here are simply copies of the previous
// spell's datablocks with the new mooring constraint specified.
//

// Server-side Mooring (ghosted to clients)
datablock afxMooringData(SF_Mooring_MG_CE)
{
  networking = $AFX::GHOSTABLE;
  displayAxisMarker = false;
};
//
datablock afxEffectWrapperData(SF_Mooring_MG_EW : SF_Mooring_EW)
{
  effect = SF_Mooring_MG_CE;
  effectName = "DroneMooring_MG";
  isConstraintSrc = true;
  ghostIsConstraintSrc = true;
};

// Re-constrained Drone Effects
datablock afxEffectWrapperData(SF_Drone_Body_MG_EW : SF_Drone_Body_EW)
{
  constraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Drone_EngineFlares_MG_EW : SF_Drone_EngineFlares_EW)
{
  constraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Drone_Dust_MG_EW : SF_Drone_Dust_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// LASER BLASTS (RELOADED - MACHINE GUN)

//
// To use an afxMachineGun effect with the drone, two are need because
// the drone has two guns (and afxMachineGun does not provide a new
// constraint source per projectile launched).  Unfortunately the
// laser effect was designed to use two particle types, which meant
// *another* two machine guns -- four total!  Why not just include the
// second particle type in the first emitter?  The main laser effect
// must be emitted with ejectionPeriodMS = 1 and no variance in order
// to produce a beam without gaps, and adding a second particle type
// would cause those gaps as the second particle type was randomly
// chosen per ms of emission.  See!
//
// (Note: the laser shot emitter exposed a small bug in the TGE
//   particle emitter code related to the age at which particles are
//   initialized upon emission.  Because the age was not properly
//   interpolated with respect to emission dt, but was instead set to
//   0, the laser beam appeared discretized.)
//
// The afxMachineGun effect takes as input a ProjectileData datablock,
// which is not completely utilized in this effect (no unique water
// explosion, for example).  However a reddish copy of the normal
// Torque explosion is created, using copies of a number of Crossbow
// datablocks but with a new color.
//
// Each laser blast is accompanied by a flashing light and a flaring 
// dts model that pulses at the drone's gun tips.
//

// main circular laser shot particle, which forms the main laser beams
datablock ParticleData(LaserShotParticle)
{
   textureName        = %mySpellDataPath @ "/SF_OL/particles/beam_pulse";
   dragCoefficient    = 0;
   gravityCoefficient = 0;
   inheritedVelFactor = 0.0;
   lifetimeMS         = 300;
   lifetimeVarianceMS = 0;
   useInvAlpha        = false;
   spinRandomMin      = 0;
   spinRandomMax      = 0;

   colors[0]     = "1 1 1 1";
   colors[1]     = "1 1 1 1";
   colors[2]     = "1 1 1 1";
   colors[3]     = "1 1 1 1";

   sizes[0]      = 0.60*0.6; // change this multiplier to change the beam width
   sizes[1]      = 0.40*0.6; // change this multiplier to change the beam width
   sizes[2]      = 0.20*0.6; // change this multiplier to change the beam width
   sizes[3]      = 0.00;

   times[0]      = 0.0;
   times[1]      = 0.33;
   times[2]      = 0.66;
   times[3]      = 1.0;
};
// "sparkle" emitter meant to add interest to the head of each laser
//   beam, but necessitating a second set of afxMachineGuns!
datablock ParticleData(LaserShotSparkleParticle)
{
   textureName        = %mySpellDataPath @ "/SF_OL/particles/beam_sparkle";
   dragCoefficient    = 0;
   gravityCoefficient = 0;
   inheritedVelFactor = 0.0;
   lifetimeMS         = 100;
   lifetimeVarianceMS = 0;
   useInvAlpha        = false;
   spinRandomMin      = -1000;
   spinRandomMax      = 1000;

   colors[0]     = "1 1 1 1";
   colors[1]     = "0.95 0.7 0.7 1";
   colors[2]     = "0.83 0.13 0.13 1";
   colors[3]     = "0 0 0 0";

   sizes[0]      = 4*0.25;
   sizes[1]      = 3*0.25; 
   sizes[2]      = 2*0.25;
   sizes[3]      = 1*0.25;

   times[0]      = 0.0;
   times[1]      = 0.33;
   times[2]      = 0.66;
   times[3]      = 1.0;  
};

// main laser emitter
datablock ParticleEmitterData(SF_LaserShotEmitter)
{
  ejectionPeriodMS = 1; // necessary, or gaps between particles will be seen
  periodVarianceMS = 0;

  ejectionVelocity = 0.0;
  velocityVariance = 0.0;

  thetaMin         = 0.0;
  thetaMax         = 90.0;  

  particles = LaserShotParticle;
  // Doing something like this:
  //  particles = "LaserShotParticle LaserShotParticle LaserShotParticle" SPC
  //              "LaserShotParticle LaserShotParticle LaserShotParticle" SPC
  //              "LaserShotParticle LaserShotParticle LaserShotSparkleParticle";
  // will cause gaps as mentioned previously.
};
// sparkle "head" emitter
datablock ParticleEmitterData(SF_LaserShotSparkleEmitter)
{
  ejectionPeriodMS = 3;
  periodVarianceMS = 1;

  ejectionVelocity = 0.0;
  velocityVariance = 0.0;

  thetaMin         = 0.0;
  thetaMax         = 90.0;  

  particles = LaserShotSparkleParticle;
};

// Copies of Crossbow explosions, with reddish color
datablock ParticleData(SF_LaserShot_ExplosionFire : CrossbowExplosionFire)
{
  colors[0]     = "1.0 0.5 0.5 0.8";
  colors[1]     = "0.5 0.1 0.1 0.8";
  colors[2]     = "0.0 0.0 0.0 0.0";
};
datablock ParticleEmitterData(SF_LaserShot_ExplosionFireEmitter : CrossbowExplosionFireEmitter)
{
  particles = "SF_LaserShot_ExplosionFire";
};
//
datablock ParticleData(SF_LaserShot_DebrisTrail : CrossbowDebrisTrail)
{
  colors[0]     = "0.8 0.1 0.1 1.0";
  colors[1]     = "0.1 0.1 0.1 0.7";
  colors[2]     = "0.1 0.1 0.1 0.0";
};
datablock ParticleEmitterData(SF_LaserShot_DebrisTrailEmitter : CrossbowDebrisTrailEmitter)
{
  particles = "SF_LaserShot_DebrisTrail";
};
//
datablock ParticleData(SF_LaserShot_DebrisSpark : CrossbowDebrisSpark)
{
   colors[0]     = "0.8 0.1 0.1 1.0";
   colors[1]     = "0.8 0.5 0.5 1.0";
   colors[2]     = "0 0 0 0.0";
};
datablock ParticleEmitterData(SF_LaserShot_DebrisSparkEmitter : CrossbowDebrisSparkEmitter)
{
  particles = "SF_LaserShot_DebrisSpark";
};
//
datablock ExplosionData(SF_LaserShot_DebrisExplosion : CrossbowDebrisExplosion)
{
  emitter[0] = SF_LaserShot_DebrisSparkEmitter;
};
datablock DebrisData(SF_LaserShot_ExplosionDebris : CrossbowExplosionDebris)
{
  emitters = "SF_LaserShot_DebrisTrailEmitter";
  explosion = SF_LaserShot_DebrisExplosion;
};
datablock ExplosionData(SF_LaserShot_Explosion : CrossbowExplosion)
{
  particleEmitter = SF_LaserShot_ExplosionFireEmitter;

  debris = SF_LaserShot_ExplosionDebris;

  lightStartColor = "1.0 0.15 0.15";
  lightEndColor   = "0 0 0";
};

// new laser shot projectile
datablock ProjectileData(LaserShotProjectile)
{
   directDamage        = 15;

   explosion           = SF_LaserShot_Explosion;
   //waterExplosion      = CrossbowWaterExplosion;

   particleEmitter     = SF_LaserShotEmitter;
   particleWaterEmitter= SF_LaserShotEmitter;

   splash              = CrossbowSplash;

   muzzleVelocity      = 90;
   velInheritFactor    = 0.3;

   armingDelay         = 0;
   lifetime            = 5000;
   fadeDelay           = 5000;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod = 0.80;
};
function LaserShotProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
   if (%col.getType() & $TypeMasks::ShapeBaseObjectType)
      %col.damage(%obj,%pos,%this.directDamage,"LaserShot");
}

switch$ (afxGetEngine())
{
  case "TGE":
    $LaserShotHeadProjectile_lightRadius = 6;
  case "TGEA":
    $LaserShotHeadProjectile_lightRadius = 3.0;
 case "T3D":
    $LaserShotHeadProjectile_lightRadius = 3.0; // JTF Notes: ??
}

// seperate projectile for sparkling laser-head, with light
datablock ProjectileData(LaserShotHeadProjectile : LaserShotProjectile)
{
  particleEmitter     = SF_LaserShotSparkleEmitter;
  particleWaterEmitter= SF_LaserShotSparkleEmitter;

  directDamage        = 0;
  radiusDamage        = 0;
  damageRadius        = 0;
  areaImpulse         = 0;

  explosion           = "";
  splash              = "";

  hasLight    = true;
  lightRadius = $LaserShotHeadProjectile_lightRadius;
  lightColor  = "1 1 1";

  hasWaterLight     = true;
  waterLightColor   = "1 1 1";
};

// offsets to place machine guns at the tip of each of the drones guns
datablock afxXM_LocalOffsetData(SF_Drone_GunOffset_LF_XM)
{
  localOffset = "0.632 0.979 0";
};
//
datablock afxXM_LocalOffsetData(SF_Drone_GunOffset_RT_XM)
{
  localOffset = "-0.632 0.979 0";
};

// afxMachineGuns for normal beams and sparkle-heads
datablock afxMachineGunData(SF_LaserShots_CE)
{
  projectile = LaserShotProjectile;
  roundsPerMinute = 125;
};
//
datablock afxMachineGunData(SF_LaserShotsHead_CE : SF_LaserShots_CE)
{
  projectile = LaserShotHeadProjectile;
};

// Machine Guns, laser shots/beams
datablock afxEffectWrapperData(SF_LaserPulses_LF_EW)
{
  effect = SF_LaserShots_CE;
  constraint = "#effect.DroneMooring_MG";
  posConstraint2 = "target.#center";

  delay       = %SCIFI_Satellite_delay+%SCIFI_Satellite_aim_delay+%SCIFI_Satellite_aim_fade;
  lifetime    = %SCIFI_Satellite_aim_lifetime-%SCIFI_Satellite_aim_fade;

  xfmModifiers[0] = SF_Drone_GunOffset_LF_XM;
};
//
datablock afxEffectWrapperData(SF_LaserPulses_RT_EW : SF_LaserPulses_LF_EW)
{
  xfmModifiers[0] = SF_Drone_GunOffset_RT_XM;
};

// Machine Guns, sparkle heads
datablock afxEffectWrapperData(SF_LaserPulsesHead_LF_EW : SF_LaserPulses_LF_EW)
{
  effect = SF_LaserShotsHead_CE;
};
//
datablock afxEffectWrapperData(SF_LaserPulsesHead_RT_EW : SF_LaserPulses_RT_EW)
{
  effect = SF_LaserShotsHead_CE;
};

// Light Flash Model
//  dts, animated scales
datablock afxModelData(SF_LaserPulseFlashModel_CE)
{
  shapeFile = %mySpellDataPath @ "/SF_OL/models/beam_flare_head.dts";
  sequence = "flash";
  sequenceRate = 1.5;
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
};
//  offset (used in addition to those defined earlier) that nudges the
//   model out a bit from the guns
datablock afxXM_LocalOffsetData(SF_LaserPulseFlashModel_offset_XM)
{
  localOffset = "0 0.07 0";
};
//  six flashes, 3 on each side (gun)
datablock afxEffectWrapperData(SF_LaserPulseFlashModel_LF_1_EW)
{
  effect = SF_LaserPulseFlashModel_CE;
  constraint = "#ghost.DroneMooring_MG";
  
  scaleFactor = 0.10;

  delay       = %SF_LaserPulseFlash_1_delay;
  fadeInTime  = 0.05;
  fadeOutTime = 0.05;
  lifetime    = 0.3;

  xfmModifiers[0] = SF_Drone_GunOffset_LF_XM;
  xfmModifiers[1] = SF_LaserPulseFlashModel_offset_XM;
};
datablock afxEffectWrapperData(SF_LaserPulseFlashModel_RT_1_EW : SF_LaserPulseFlashModel_LF_1_EW)
{
  xfmModifiers[0] = SF_Drone_GunOffset_RT_XM;
};
datablock afxEffectWrapperData(SF_LaserPulseFlashModel_LF_2_EW : SF_LaserPulseFlashModel_LF_1_EW)
{
  delay       = %SF_LaserPulseFlash_2_delay;
};
datablock afxEffectWrapperData(SF_LaserPulseFlashModel_RT_2_EW : SF_LaserPulseFlashModel_RT_1_EW)
{
  delay       = %SF_LaserPulseFlash_2_delay;
};
datablock afxEffectWrapperData(SF_LaserPulseFlashModel_LF_3_EW : SF_LaserPulseFlashModel_LF_1_EW)
{
  delay       = %SF_LaserPulseFlash_3_delay;
};
datablock afxEffectWrapperData(SF_LaserPulseFlashModel_RT_3_EW : SF_LaserPulseFlashModel_RT_1_EW)
{
  delay       = %SF_LaserPulseFlash_3_delay;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// DRONE TELEPORTATION -- IN (RELOADED - MACHINE GUN)

//
// All of the effects defined here are simply copies of the previous
// spell's datablocks with the new mooring constraint specified.
//

datablock afxEffectWrapperData(SF_Tele_IN_Dashes_UP_MG_EW : SF_Tele_IN_Dashes_UP_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Tele_IN_Dashes_DOWN_MG_EW : SF_Tele_IN_Dashes_DOWN_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Tele_IN_Bars_1_MG_EW : SF_Tele_IN_Bars_1_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Tele_IN_Bars_2_MG_EW : SF_Tele_IN_Bars_2_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Tele_IN_Bars_3_MG_EW : SF_Tele_IN_Bars_3_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Tele_IN_Bars_4_MG_EW : SF_Tele_IN_Bars_4_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Tele_IN_Bars_5_MG_EW : SF_Tele_IN_Bars_5_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Tele_IN_Bars_6_MG_EW : SF_Tele_IN_Bars_6_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Tele_IN_Beam_MG_EW : SF_Tele_IN_Beam_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// DRONE TELEPORTATION -- OUT (RELOADED - MACHINE GUN)

//
// All of the effects defined here are simply copies of the previous
// spell's datablocks with the new mooring constraint specified.
//

datablock afxEffectWrapperData(SF_Tele_OUT_Dashes_UP_MG_EW : SF_Tele_OUT_Dashes_UP_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Tele_OUT_Dashes_DOWN_MG_EW : SF_Tele_OUT_Dashes_DOWN_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Tele_OUT_Bars_1_MG_EW : SF_Tele_OUT_Bars_1_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Tele_OUT_Bars_2_MG_EW : SF_Tele_OUT_Bars_2_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Tele_OUT_Bars_3_MG_EW : SF_Tele_OUT_Bars_3_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Tele_OUT_Bars_4_MG_EW : SF_Tele_OUT_Bars_4_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Tele_OUT_Bars_5_MG_EW : SF_Tele_OUT_Bars_5_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Tele_OUT_Bars_6_MG_EW : SF_Tele_OUT_Bars_6_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Tele_OUT_Beam_MG_EW : SF_Tele_OUT_Beam_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};

//
// <Occam's Laser 2> <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


// <Occam's Laser 3> >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
//   The effects that follow are defined for the third variation of
//    Occam's Laser, SciFiDrone3Spell.  Here the drone is a static
//    shape that exists only on the server.


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPACE DRONE (REDUX - STATIC SHAPE)

//
// The mooring that controlled the drone for the main spell existed
// only on the client, but in this variation, as with Reloaded, the
// mooring exists on the server and is ghosted to clients.  This is
// done because the drone will now exist as a server-side static
// shape, and therefore its motion must come from a source also on the
// server.  Client objects constrain to the ghost of the mooring.
//
// Most of the effects defined here are simply copies of the previous
// spell's datablocks with the new mooring constraint specified.
//

// Server-side Mooring (ghosted to clients)
datablock afxMooringData(SF_Mooring_S_CE)
{
  networking = $AFX::SCOPE_ALWAYS;
  displayAxisMarker = false;
};
//
datablock afxEffectWrapperData(SF_Mooring_S_EW : SF_Mooring_EW)
{
  effect = SF_Mooring_S_CE;
  effectName = "DroneMooring_S";
  isConstraintSrc = true;
  ghostIsConstraintSrc = true;
};

// Static Shape Drone
datablock StaticShapeData(SF_Drone_Body_S_CE)
{
  shapeFile = %mySpellDataPath @ "/SF_OL/models/gun_satellite.dts";
  sequence = "roam";
  shadowEnable = false;
};
//
datablock afxEffectWrapperData(SF_Drone_Body_S_EW : SF_Drone_Body_EW)
{
  effect = SF_Drone_Body_S_CE;
  constraint = "#effect.DroneMooring_S";
};

// Re-constrained Drone Effects
datablock afxEffectWrapperData(SF_Drone_EngineFlares_S_EW : SF_Drone_EngineFlares_EW)
{
  constraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Drone_Dust_S_EW : SF_Drone_Dust_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// DRONE TELEPORTATION -- IN (REDUX - STATIC SHAPE)

//
// All of the effects defined here are simply copies of the previous
// spell's datablocks with the new mooring constraint specified.
//

datablock afxEffectWrapperData(SF_Tele_IN_Dashes_UP_S_EW : SF_Tele_IN_Dashes_UP_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Tele_IN_Dashes_DOWN_S_EW : SF_Tele_IN_Dashes_DOWN_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Tele_IN_Bars_1_S_EW : SF_Tele_IN_Bars_1_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Tele_IN_Bars_2_S_EW : SF_Tele_IN_Bars_2_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Tele_IN_Bars_3_S_EW : SF_Tele_IN_Bars_3_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Tele_IN_Bars_4_S_EW : SF_Tele_IN_Bars_4_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Tele_IN_Bars_5_S_EW : SF_Tele_IN_Bars_5_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Tele_IN_Bars_6_S_EW : SF_Tele_IN_Bars_6_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Tele_IN_Beam_S_EW : SF_Tele_IN_Beam_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// DRONE TELEPORTATION -- OUT (REDUX - STATIC SHAPE)

//
// All of the effects defined here are simply copies of the previous
// spell's datablocks with the new mooring constraint specified.
//

datablock afxEffectWrapperData(SF_Tele_OUT_Dashes_UP_S_EW : SF_Tele_OUT_Dashes_UP_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Tele_OUT_Dashes_DOWN_S_EW : SF_Tele_OUT_Dashes_DOWN_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Tele_OUT_Bars_1_S_EW : SF_Tele_OUT_Bars_1_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Tele_OUT_Bars_2_S_EW : SF_Tele_OUT_Bars_2_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Tele_OUT_Bars_3_S_EW : SF_Tele_OUT_Bars_3_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Tele_OUT_Bars_4_S_EW : SF_Tele_OUT_Bars_4_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Tele_OUT_Bars_5_S_EW : SF_Tele_OUT_Bars_5_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Tele_OUT_Bars_6_S_EW : SF_Tele_OUT_Bars_6_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Tele_OUT_Beam_S_EW : SF_Tele_OUT_Beam_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// LASER BEAMS (REDUX - STATIC SHAPE)

//
// Most of the effects defined here are simply copies of the previous
// spell's datablocks with the new mooring constraint specified.
//

datablock afxEffectWrapperData(SF_Beam_Blast_LF_S_EW : SF_Beam_Blast_LF_EW)
{
  constraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Beam_Blast_RT_S_EW : SF_Beam_Blast_RT_EW)
{
  constraint = "#ghost.DroneMooring_S";
};

//
// <Occam's Laser 3> <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING (All 3 Effect Variations)

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/ol_lighting_tge_sub.cs");
    exec("./audio/ol_audio_sub.cs");
  case "TGEA":
    exec("./lighting/ol_lighting_tgea_sub.cs");
    exec("./audio/ol_audio_sub.cs");
 case "T3D":
    exec("./lighting/ol_lighting_t3d_sub.cs");
    exec("./audio/ol_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// OCCAM'S LASER (SCI-FI) SPELL
//

datablock afxMagicSpellData(SciFiDroneSpell)
{
  castingDur = 0;
  deliveryDur = %SCIFI_Satellite_delay+%SCIFI_Satellite_aim_delay+%SCIFI_Satellite_aim_fade+0.5;//+1.0;
  allowMovementInterrupts = false;

    // drone //
  addCastingEffect = SF_Mooring_EW;
  addCastingEffect = SF_Drone_Body_EW;
  addCastingEffect = SF_Drone_EngineFlares_EW;
  addCastingEffect = SF_Drone_Dust_EW;

    // drone teleportation -- in //
  addCastingEffect = SF_Tele_IN_Dashes_UP_EW;
  addCastingEffect = SF_Tele_IN_Dashes_DOWN_EW;
  addCastingEffect = SF_Tele_IN_Bars_1_EW;
  addCastingEffect = SF_Tele_IN_Bars_2_EW;
  addCastingEffect = SF_Tele_IN_Bars_3_EW;
  addCastingEffect = SF_Tele_IN_Bars_4_EW;
  addCastingEffect = SF_Tele_IN_Bars_5_EW;
  addCastingEffect = SF_Tele_IN_Bars_6_EW;
  addCastingEffect = SF_Tele_IN_Beam_EW;

    // drone teleportation -- out //
  addCastingEffect = SF_Tele_OUT_Dashes_UP_EW;
  addCastingEffect = SF_Tele_OUT_Dashes_DOWN_EW;
  addCastingEffect = SF_Tele_OUT_Bars_1_EW;
  addCastingEffect = SF_Tele_OUT_Bars_2_EW;
  addCastingEffect = SF_Tele_OUT_Bars_3_EW;
  addCastingEffect = SF_Tele_OUT_Bars_4_EW;
  addCastingEffect = SF_Tele_OUT_Bars_5_EW;
  addCastingEffect = SF_Tele_OUT_Bars_6_EW;
  addCastingEffect = SF_Tele_OUT_Beam_EW;
    // laser beams //
  addDeliveryEffect = SF_Beam_Blast_LF_EW;
  addDeliveryEffect = SF_Beam_Blast_RT_EW;

    // impact effects
  addImpactEffect = SF_Beam_Pulse_EW;
  addImpactEffect = SF_Beam_Impact_Fire_1_EW;
  addImpactEffect = SF_Beam_Impact_Fire_2_EW;
  addImpactEffect = SF_Beam_Impact_Fire_3_EW;
  addImpactEffect = SF_Beam_Impact_Fire_4_EW;
  addImpactEffect = SF_Beam_Impact_Fire_5_EW;
  addImpactEffect = SF_Beam_Smoke_1_EW;
  addImpactEffect = SF_Beam_Smoke_2_EW;
  addImpactEffect = SF_Beam_Smoke_3_EW;
  addImpactEffect = SF_Beam_Smoke_4_EW;
  addImpactEffect = SF_Beam_Smoke_5_EW;
  addImpactEffect = SF_ScorchedEarth_EW;
};

// sounds and lights added via sub-script functions //
OL_main_add_Lighting_FX(SciFiDroneSpell);
OL_main_add_Audio_FX(SciFiDroneSpell);

datablock afxRPGMagicSpellData(SciFiDroneSpell_RPG)
{
  spellName = "Occam's Laser (sci-fi)";
  desc = "This autonomous but faithful hover DRONE will watch your back and dutifully apply Occam's maxim: " @ 
         "\"An enemy in three pieces is a lessor enemy.\" Divide and Conquer!" @
         "\n" @
         "\neffect design: Matthew Durante" @
         "\neffect concept: Matthew Durante";
  sourcePack = "Core Tech";
  iconBitmap = %mySpellDataPath @ "/SF_OL/icons/sf_ol";
  target = "enemy";
  range = 100;
  manaCost = 10;

  directDamage = 100;

  castingDur = SciFiDroneSpell.castingDur;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// OCCAM'S LASER RELOADED (SCI-FI) SPELL
//
datablock afxMagicSpellData(SciFiDrone2Spell)
{
  castingDur = 0;
  deliveryDur = %SCIFI_Satellite_delay+%SCIFI_Satellite_aim_delay+%SCIFI_Satellite_aim_fade+0.5;//+1.0;
  allowMovementInterrupts = false;

    // drone //
  addCastingEffect = SF_Mooring_MG_EW;
  addCastingEffect = SF_Drone_Body_MG_EW;
  addCastingEffect = SF_Drone_EngineFlares_MG_EW;
  addCastingEffect = SF_Drone_Dust_MG_EW;

    // drone teleportation -- in //
  addCastingEffect = SF_Tele_IN_Dashes_UP_MG_EW;
  addCastingEffect = SF_Tele_IN_Dashes_DOWN_MG_EW;
  addCastingEffect = SF_Tele_IN_Bars_1_MG_EW;
  addCastingEffect = SF_Tele_IN_Bars_2_MG_EW;
  addCastingEffect = SF_Tele_IN_Bars_3_MG_EW;
  addCastingEffect = SF_Tele_IN_Bars_4_MG_EW;
  addCastingEffect = SF_Tele_IN_Bars_5_MG_EW;
  addCastingEffect = SF_Tele_IN_Bars_6_MG_EW;
  addCastingEffect = SF_Tele_IN_Beam_MG_EW;

    // drone teleportation -- out //
  addCastingEffect = SF_Tele_OUT_Dashes_UP_MG_EW;
  addCastingEffect = SF_Tele_OUT_Dashes_DOWN_MG_EW;
  addCastingEffect = SF_Tele_OUT_Bars_1_MG_EW;
  addCastingEffect = SF_Tele_OUT_Bars_2_MG_EW;
  addCastingEffect = SF_Tele_OUT_Bars_3_MG_EW;
  addCastingEffect = SF_Tele_OUT_Bars_4_MG_EW;
  addCastingEffect = SF_Tele_OUT_Bars_5_MG_EW;
  addCastingEffect = SF_Tele_OUT_Bars_6_MG_EW;
  addCastingEffect = SF_Tele_OUT_Beam_MG_EW;
  
    // laser blasts (Machine Gun) //
  addDeliveryEffect = SF_LaserPulses_LF_EW;
  addDeliveryEffect = SF_LaserPulses_RT_EW;
  addDeliveryEffect = SF_LaserPulsesHead_LF_EW;
  addDeliveryEffect = SF_LaserPulsesHead_RT_EW;
  addDeliveryEffect = SF_LaserPulseFlashModel_LF_1_EW;
  addDeliveryEffect = SF_LaserPulseFlashModel_RT_1_EW;
  addDeliveryEffect = SF_LaserPulseFlashModel_LF_2_EW;
  addDeliveryEffect = SF_LaserPulseFlashModel_RT_2_EW;
  addDeliveryEffect = SF_LaserPulseFlashModel_LF_3_EW;
  addDeliveryEffect = SF_LaserPulseFlashModel_RT_3_EW;
};

// sounds and lights added via sub-script functions //
OL_reloaded_add_Lighting_FX(SciFiDrone2Spell);
OL_reloaded_add_Audio_FX(SciFiDrone2Spell);

datablock afxRPGMagicSpellData(SciFiDrone2Spell_RPG)
{
  spellName = "Occam's Laser Reloaded (sci-fi)";
  desc = "This autonomous but faithful hover DRONE will watch your back and dutifully apply Occam's maxim: " @ 
         "\"An enemy in three pieces is a lessor enemy.\" Divide and Conquer!" @
         "\n\n" @
         "(In this variation, afxMachineGun is used in place of the laser beams.)";
  sourcePack = "Core Tech";
  iconBitmap = %mySpellDataPath @ "/SF_OL/icons/sf_ol";
  target = "enemy";
  range = 100;
  manaCost = 10;

  directDamage = 0;

  castingDur = SciFiDroneSpell.castingDur;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// OCCAM'S LASER REDUX (SCI-FI) SPELL
//
datablock afxMagicSpellData(SciFiDrone3Spell)
{
  castingDur = 0;
  deliveryDur = %SCIFI_Satellite_delay+%SCIFI_Satellite_aim_delay+%SCIFI_Satellite_aim_fade+0.5;//+1.0;
  allowMovementInterrupts = false;

    // drone //
  addCastingEffect = SF_Mooring_S_EW;
  addCastingEffect = SF_Drone_Body_S_EW;
  addCastingEffect = SF_Drone_EngineFlares_S_EW;
  addCastingEffect = SF_Drone_Dust_S_EW;

    // drone teleportation -- in //
  addCastingEffect = SF_Tele_IN_Dashes_UP_S_EW;
  addCastingEffect = SF_Tele_IN_Dashes_DOWN_S_EW;
  addCastingEffect = SF_Tele_IN_Bars_1_S_EW;
  addCastingEffect = SF_Tele_IN_Bars_2_S_EW;
  addCastingEffect = SF_Tele_IN_Bars_3_S_EW;
  addCastingEffect = SF_Tele_IN_Bars_4_S_EW;
  addCastingEffect = SF_Tele_IN_Bars_5_S_EW;
  addCastingEffect = SF_Tele_IN_Bars_6_S_EW;
  addCastingEffect = SF_Tele_IN_Beam_S_EW;

    // drone teleportation -- out //
  addCastingEffect = SF_Tele_OUT_Dashes_UP_S_EW;
  addCastingEffect = SF_Tele_OUT_Dashes_DOWN_S_EW;
  addCastingEffect = SF_Tele_OUT_Bars_1_S_EW;
  addCastingEffect = SF_Tele_OUT_Bars_2_S_EW;
  addCastingEffect = SF_Tele_OUT_Bars_3_S_EW;
  addCastingEffect = SF_Tele_OUT_Bars_4_S_EW;
  addCastingEffect = SF_Tele_OUT_Bars_5_S_EW;
  addCastingEffect = SF_Tele_OUT_Bars_6_S_EW;
  addCastingEffect = SF_Tele_OUT_Beam_S_EW;

    // laser beams //
  addDeliveryEffect = SF_Beam_Blast_LF_S_EW;
  addDeliveryEffect = SF_Beam_Blast_RT_S_EW;

    // impact effects
  addImpactEffect = SF_Beam_Pulse_EW;
  addImpactEffect = SF_Beam_Impact_Fire_1_EW;
  addImpactEffect = SF_Beam_Impact_Fire_2_EW;
  addImpactEffect = SF_Beam_Impact_Fire_3_EW;
  addImpactEffect = SF_Beam_Impact_Fire_4_EW;
  addImpactEffect = SF_Beam_Impact_Fire_5_EW;
  addImpactEffect = SF_Beam_Smoke_1_EW;
  addImpactEffect = SF_Beam_Smoke_2_EW;
  addImpactEffect = SF_Beam_Smoke_3_EW;
  addImpactEffect = SF_Beam_Smoke_4_EW;
  addImpactEffect = SF_Beam_Smoke_5_EW;
  addImpactEffect = SF_ScorchedEarth_EW;
};

// sounds and lights added via sub-script functions //
OL_redux_add_Lighting_FX(SciFiDrone3Spell);
OL_redux_add_Audio_FX(SciFiDrone3Spell);

datablock afxRPGMagicSpellData(SciFiDrone3Spell_RPG)
{
  spellName = "Occam's Laser Redux (sci-fi)";
  desc = "This autonomous but faithful hover DRONE will watch your back and dutifully apply Occam's maxim: " @ 
         "\"An enemy in three pieces is a lessor enemy.\" Divide and Conquer!" @
         "\n\n" @
         "(In this variation, drone is server-based StaticShape with client-based secondary effects.)";
  sourcePack = "Core Tech";
  iconBitmap = %mySpellDataPath @ "/SF_OL/icons/sf_ol";
  target = "enemy";
  range = 100;
  manaCost = 10;

  directDamage = 100;

  castingDur = SciFiDroneSpell.castingDur;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

if ($effect_reload)
{
  // Removes then adds all reloaded datablocks
  touchDataBlocks();
}
else
{
  // save script filename and data path for reloads
  SciFiDroneSpell.scriptFile = $afxAutoloadScriptFile;
  SciFiDrone2Spell.scriptFile = $afxAutoloadScriptFile;
  SciFiDrone3Spell.scriptFile = $afxAutoloadScriptFile;
  SciFiDroneSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
  {
    addDemoSpellbookSpell(SciFiDroneSpell, SciFiDroneSpell_RPG);
    addDemoSpellbookSpell(SciFiDrone2Spell, SciFiDrone2Spell_RPG);
    addDemoSpellbookSpell(SciFiDrone3Spell, SciFiDrone3Spell_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

