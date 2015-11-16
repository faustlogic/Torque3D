
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX
//
// Shards of Vesuvius
//
// Copyright (C) Faust Logic, Inc.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

$SoV_Comet_UsePathParabolas = false;

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// PARTICLES AND EMITTERS

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Comet Fire Trail

datablock ParticleData(SoV_Comet_FireA_P)
{
  textureName          = %mySpellDataPath @ "/Shared/particles/sp2_flame_C128";
  dragCoeffiecient     = 6.0;
  gravityCoefficient   = 0.04;
  windCoefficient      = 0.0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 1000;
  lifetimeVarianceMS   = 200;
  blendType            = premultalpha;
  spinRandomMin        = -500;
  spinRandomMax        = 500;
  colors[0]            = "1.0 1.0 1.0 0.17";
    colors[1]            = "1.0 0.8 0.5 0.17";
    colors[2]            = "1.0 0.3 0.1 0.17";
    colors[3]            = "0.0 0.0 0.0 0.0";
  sizeBias             = 0.45;
  sizes[0]             = 1;
    sizes[1]             = 2;
    sizes[2]             = 3;
    sizes[3]             = 4;
  times[0]             = 0.0;
    times[1]             = 0.33;
    times[2]             = 0.66;
    times[3]             = 1.0;
};
datablock ParticleData(SoV_Comet_FireB_P : SoV_Comet_FireA_P)
{
  sizeBias = 0.75;
};
datablock ParticleData(SoV_Comet_FireC_P : SoV_Comet_FireA_P)
{
  sizeBias = 1.2;
};

datablock ParticleEmitterData(SoV_Comet_FireTrail_E)
{
  ejectionPeriodMS      = 8;
  periodVarianceMS      = 4;
  ejectionVelocity      = 0;
  velocityVariance      = 0; 
  fadeAlpha             = true; // fade alpha only so particles remain glowy as they fade
  fadeColor             = false;
  fadeSize              = true;
  blendStyle            = "PREMULTALPHA";
  particles             = "SoV_Comet_FireA_P SoV_Comet_FireA_P SoV_Comet_FireA_P" SPC
                          "SoV_Comet_FireB_P SoV_Comet_FireB_P" SPC
                          "SoV_Comet_FireC_P";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Comet Smoke Trail

datablock ParticleData(SoV_Comet_SmokeA_P)
{
  textureName          = "$$ \"" @ %mySpellDataPath @ "/Shared/particles/sp2_dust_2x2_\" @ " @ "%%._mood";
  textureExtName       = %mySpellDataPath @ "/Shared/particles/sp2_dust_2x2_white";
  textureCoords[0]     = "0.0 0.5";
    textureCoords[1]     = "0.0 1.0";
    textureCoords[2]     = "0.5 1.0";
    textureCoords[3]     = "0.5 0.5";

  dragCoeffiecient     = 6.0;
  gravityCoefficient   = 0.04;
  windCoefficient      = 0.0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 6000;
  lifetimeVarianceMS   = 2000;
  blendType            = normal;
  spinRandomMin        = -50;
  spinRandomMax        = 50;
  colors[0]            = "0.0 0.0 0.0 0.0";
  colors[1]            = "0.0 0.0 0.0 0.0";
  colors[2]            = "0.15 0.15 0.15 0.9";
  colors[3]            = "0.35 0.35 0.35 0.6";
  colors[4]            = "0.0 0.0 0.0 0.0";
  sizes[0]             = 1;
  sizes[1]             = 2;
  sizes[2]             = 3;
  sizes[3]             = 4*1.5;
  sizes[4]             = 5*4;
  sizeBias             = 1.2;
  times[0]             = 0.0;
  times[1]             = 0.15;
  times[2]             = 0.20;
  times[3]             = 0.5;
  times[4]             = 1.0;
};
datablock ParticleData(SoV_Comet_SmokeB_P : SoV_Comet_SmokeA_P)
{
  textureCoords[0]     = "0.5 0.0";
    textureCoords[1]     = "0.5 0.5";
    textureCoords[2]     = "1.0 0.5";
    textureCoords[3]     = "1.0 0.0";
  sizeBias             = 1.7;
};
datablock ParticleData(SoV_Comet_SmokeC_P : SoV_Comet_SmokeA_P)
{
  textureCoords[0]     = "0.0 0.0";
    textureCoords[1]     = "0.0 0.5";
    textureCoords[2]     = "0.5 0.5";
    textureCoords[3]     = "0.5 0.0";
  sizeBias             = 2.5;
};

datablock ParticleEmitterData(SoV_Comet_SmokeTrail_E)
{
  ejectionPeriodMS      = 40;
  periodVarianceMS      = 20;
  ejectionVelocity      = 0;
  velocityVariance      = 0; 
  particles             = "SoV_Comet_SmokeA_P SoV_Comet_SmokeA_P" SPC
                          "SoV_Comet_SmokeB_P SoV_Comet_SmokeB_P" SPC
                          "SoV_Comet_SmokeC_P";
  poolData              = SoV_SmokePool;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// FIREBALL COMET PARTS

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// COMET HEAD

//
//   Each mooring represents the head of a single flaming comet
//   as it traces a ballistic parabola. All other comet elements
//   are constrainted to the mooring.
//
//   Each mooring conforms to a randomized path that forms a 
//   parabolic trajectory in the Y-Z plane, anchored
//   at the Pentegram location, and rotated a random amount about
//   the Z axis. Their delay durations are also randomized.
//
//   The different moorings only vary in delay times, spin angles,
//   and trajectory paths.
//
//   These moorings could potentially be replaced by a ballistic 
//   projectile of some sort. 
//

if ($SoV_Comet_UsePathParabolas)
{
  datablock afxMooringData(SoV_Mooring_CE)
  {
    networking = $AFX::CLIENT_ONLY;
    displayAxisMarker = false;
  };

  datablock afxPathData(SoV_Comet_00_Path)
  {
    lifetime = "$$ SoV_calcCometLifetime(%%,##)";
    points = "$$ SoV_calcCometPath(%%,##)";
  };
  datablock afxXM_PathConformData(SoV_Comet_Path_00_XM)
  {
    paths = SoV_Comet_00_Path;
  };

  datablock afxXM_SpinData(SoV_Comet_Spin_00_XM)
  {
    spinAxis= "0 0 1";
    //spinAngle = "$$ (##-1)*(360/20)";
    spinAngle = "$$ (##-1)*(360/%%._numComets)";
    spinAngleVariance = 30;
    spinRate = 0;
  };

  datablock afxEffectWrapperData(SoV_Comet_Head_00_EW)
  {
    effect = SoV_Mooring_CE;
    constraint = "freeTarget";
    effectName = "$$ CometHeadMooring##";
    isConstraintSrc = true;
    delay = "$$ %%._cometDelay[##]";
    lifetime = 10.0;
    xfmModifiers[0] = SoV_Comet_Spin_00_XM;
    xfmModifiers[1] = SoV_Comet_Path_00_XM;
  };
}
else
{
  datablock afxProjectileData(SoV_Rocket_CE)
  {
    muzzleVelocity          = "$$ %%._cometSpeed[##]";
    velInheritFactor        = 0.0;
    lifetime                = 5000;
    bounceElasticity        = 0.999;
    bounceFriction          = 0.3;
    isBallistic             = true;
    ballisticCoefficient    = 1.0;
    gravityMod              = 1.0;

    networking              = $AFX::CLIENT_ONLY;
    ignoreSourceTimeout     = true;
    dynamicCollisionMask    = 0;
    staticCollisionMask     = 0;
    overrideCollisionMasks  = true;
    launchDirMethod         = "orientConstraint";
  };

  datablock afxXM_SpinData(SoV_RocketSpin_A_00_XM)
  {
    spinAxis= "0 0 1";
    //spinAngle = "$$ (##-1)*(360/20)";
    spinAngle = "$$ (##-1)*(360/%%._numComets)";
    spinAngleVariance = 30;
    spinRate = 0;
  };

  datablock afxXM_SpinData(SoV_RocketSpin_B_00_XM)
  {
    spinAxis= "1 0 0";
    spinAngle = "$$ 90 - %%._cometAng[##]";
    spinAngleVariance = 0;
    spinRate = 0;
  };

  datablock afxEffectWrapperData(SoV_Comet_Head_00_EW)
  {
    effect = SoV_Rocket_CE;
    constraint = "freeTarget";
    effectName = "$$ CometHeadMooring##";
    isConstraintSrc = true;
    delay = "$$ %%._cometDelay[##]";
    lifetime = 6;
    xfmModifiers[0] = SoV_RocketSpin_A_00_XM;
    xfmModifiers[1] = SoV_RocketSpin_B_00_XM;
  };
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// COMET FIRE-TRAIL EMITTER TEMPLATE

%SoV_Comet_FireTrail_BBox = "-30.0 -30.0 0 30.0 30.0 30.0";

datablock afxEffectWrapperData(SoV_Comet_FireTrail_00_EW)
{
  effect = SoV_Comet_FireTrail_E;
  constraint = "$$ \"#effect.CometHeadMooring##\"";
  delay = "$$ %%._cometDelay[##]";
  fadeinTime = 0.2;
  fadeOutTime = "$$ SoV_calcCometLifetime(%%,##)*0.1";
  lifetime = "$$ SoV_calcCometLifetime(%%,##)*0.6";
  sortPriority = 30;
  forcedBBox = %SoV_Comet_FireTrail_BBox;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// COMET SMOKE-TRAIL EMITTER TEMPLATE

%SoV_Comet_SmokeTrail_BBox = "-36.0 -36.0 0 36.0 36.0 36.0";

datablock afxEffectWrapperData(SoV_Comet_SmokeTrail_00_EW : SoV_Comet_FireTrail_00_EW)
{
  effect = SoV_Comet_SmokeTrail_E;
  fadeInTime  = 0.0;
  fadeOutTime = 0.0;
  forcedBBox = %SoV_Comet_SmokeTrail_BBox;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// COMET GLOW BILLBOARD TEMPLATE

datablock afxXM_AimData(SoV_Comet_Glow_aim_XM)
{
  aimZOnly = false;
};

datablock afxModelData(SoV_Comet_Glow_CE)
{
  shapeFile = %mySpellDataPath @ "/SoV/models/sov_fireBurstGlow.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  sequence = "pulsate";
  sequenceRate = 1.0;
};

datablock afxEffectWrapperData(SoV_Comet_Glow_00_EW : SoV_Comet_FireTrail_00_EW)
{
  effect = SoV_Comet_Glow_CE;
  posConstraint2 = camera;
  scaleFactor = "$$ getRandomF(3.0,7.0)";
  xfmModifiers[0] = SoV_Comet_Glow_aim_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// COMET AUDIO TEMPLATE

datablock afxEffectWrapperData(SoV_CometLaunch_SND_00_EW)
{
  effectEnabled = "$$ getRandom(1,5) == 1";
  effect = SoV_CometLaunch_SND;
  constraint = "$$ \"#effect.CometHeadMooring##\"";
  delay = "$$ %%._cometDelay[##]";
  lifetime = 3;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// COMET GROUP

datablock afxEffectGroupData(SoV_Comet_EG)
{
  assignIndices = true;
  indexOffset = 1;
  count = 20;
  count = "$$ %%._numComets";
  addEffect = SoV_Comet_Head_00_EW;
  addEffect = SoV_Comet_FireTrail_00_EW;
  addEffect = SoV_Comet_SmokeTrail_00_EW;
  addEffect = SoV_Comet_Glow_00_EW;
  addEffect = SoV_CometLaunch_SND_00_EW;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// HOOK FUNCTIONS

// add effects to mail spell datablock
function SoV_add_Comet_FX(%spell_data)
{
  %spell_data.addCastingEffect(SoV_Comet_EG);
}

// add comet parameters to main spell
function SoV_add_Comet_Params(%spell, %arise_time)
{
  //%ncom = getRandom(18,22); 
  %ncom = getRandom(12,16);

  %spell._numComets = %ncom;
  for (%i = 1; %i <= %ncom; %i++)
  {
    %spell._cometSpeed[%i] = getRandom(20, 40);
    %spell._cometAng[%i] = getRandom(30, 80);
    if (%i > ((16*%ncom)/20))
      %spell._cometDelay[%i] = mFloatLength(%arise_time + getRandomF(0.6, 2.7),1);
    else if (%i > ((10*%ncom)/20)) 
      %spell._cometDelay[%i] = mFloatLength(%arise_time + getRandomF(0.6, 1.7),1);
    else
      %spell._cometDelay[%i] = mFloatLength(%arise_time + getRandomF(0.6, 1.2),1);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//