
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX
//
// FIRE TOWER FIREBALL (SUB-SCRIPT)
//
// Copyright (C) Faust Logic, Inc.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// HUGE FIREBALL (particles)

datablock ParticleData(FitS_HugeFireBallNew_P)
{
  textureName          = %mySpellDataPath @ "/FT/particles/FT_BigFireball";
  dragCoeffiecient     = 0.0;
  gravityCoefficient   = 0.0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 300;
  lifetimeVarianceMS   = 150;
  useInvAlpha          = false;
  angle                = 0;
  angleVariance        = 180;
  spinRandomMin        = 0;
  spinRandomMax        = 100;
  randomizeSpinDir     = true;
  colors[0]            = "1.0 1.0 1.0 0.05";
    colors[1]            = "1.0 1.0 1.0 0.02";
    colors[2]            = "0.0 0.0 0.0 0.0";
  sizes[0]             = 3;
    sizes[1]             = 5;
    sizes[2]             = 2;
  times[0]             = 0.0;
    times[1]             = 0.4;
    times[2]             = 1.0;
  constrainPos         = true;
};

datablock ParticleData(FitS_HugeFireBallCore_P)
{
  textureName          = %mySpellDataPath @ "/FT/particles/FT_fireball1";
  dragCoeffiecient     = 0.0;
  gravityCoefficient   = 0.0;
  inheritedVelFactor   = 0.00;
  useInvAlpha          = false;
  angle                = 0;
  angleVariance        = 180;
  lifetimeMS           = 600;
  lifetimeVarianceMS   = 100;  
  spinRandomMin        = -250; 
  spinRandomMax        = 250;
  colors[0]            = "1.0 1.0 1.0 0.0";
    colors[1]            = "1.0 1.0 1.0 0.3";
    colors[2]            = "1.0 1.0 1.0 0.6";
    colors[3]            = "1.0 1.0 1.0 0.6";
    colors[4]            = "0.0 0.0 0.0 0.0";
  sizeBias             = 1.5;
  sizes[0]             = 2;
    sizes[1]             = 5;
    sizes[2]             = 6;
    sizes[3]             = 8;
    sizes[4]             = 10;
  times[0]             = 0.0;
    times[1]             = 0.1;
    times[2]             = 0.4;
    times[3]             = 0.6;
    times[4]             = 1.0;
  constrainPos         = true;
};

datablock ParticleData(FitS_HugeFireBallCore2_P)
{
  textureName          = %mySpellDataPath @ "/FT/particles/FT_fireball2_parts";
  textureCoords[0]     = "0.5 0.0";
    textureCoords[1]     = "0.5 0.5";
    textureCoords[2]     = "1.0 0.5";
    textureCoords[3]     = "1.0 0.0";
  dragCoeffiecient     = 0.1;
  gravityCoefficient   = -0.5;
  inheritedVelFactor   = 0.00;
  useInvAlpha          = false;
  angle                = 0;
  angleVariance        = 180;
  lifetimeMS           = 2000;
  lifetimeVarianceMS   = 200;
  spinRandomMin        = -250; 
  spinRandomMax        = 250;
  colors[0]            = "1.0 1.0 1.0 0.0";
    colors[1]            = "1.0 1.0 1.0 0.3";
    colors[2]            = "0.75 0.75 0.75 0.75";
    colors[3]            = "0.0 0.0 0.0 0.0";
    colors[4]            = "0.0 0.0 0.0 0.0";
  sizeBias             = 3.0;
  sizes[0]             = 4;
    sizes[1]             = 3;
    sizes[2]             = 2;
    sizes[3]             = 1;
    sizes[4]             = 0;
  times[0]             = 0.0;
    times[1]             = 0.25;
    times[2]             = 0.5;
    times[3]             = 0.75;
    times[4]             = 1.0;
};

datablock ParticleData(FitS_FireballSmokeB_P)
{
  textureName          = %mySpellDataPath @ "/FT/particles/FT_fireball2_parts";
  textureCoords[0]     = "0.0 0.0";
    textureCoords[1]     = "0.0 0.5";
    textureCoords[2]     = "0.5 0.5";
    textureCoords[3]     = "0.5 0.0";
  dragCoeffiecient     = 0.1;
  gravityCoefficient   = -0.5;
  windCoefficient      = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 2000*1.5;
  lifetimeVarianceMS   = 200*1.5;
  spinRandomMin        = -300.0*0.5;
  spinRandomMax        = 300.0*0.5;
  colors[0]            = "0.0 0.0 0.0 0.0";
    colors[1]            = "0.0 0.0 0.0 0.0";
    colors[2]            = "0.0 0.0 0.0 1";
    colors[3]            = "0.0 0.0 0.0 0.0";
    colors[4]            = "0.0 0.0 0.0 0.0";
  sizeBias             = 3.0;
  sizes[0]             = 4;
    sizes[1]             = 3;
    sizes[2]             = 2;
    sizes[3]             = 10;
    sizes[4]             = 18;
  times[0]             = 0.0;
    times[1]             = 0.25;
    times[2]             = 0.45;
    times[3]             = 0.7;
    times[4]             = 1.0;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock ParticleEmitterData(FitS_HugeFireBallNew_E)
{
  ejectionPeriodMS     = 75;
  periodVarianceMS     = 29;
  ejectionVelocity     = 0.25;
  velocityVariance     = 0.10;
  thetaMin             = 0;
  thetaMax             = 180;
  phiReferenceVel      = 90;
  phiVariance          = 180;
  particles            = "FitS_HugeFireBallNew_P"; 
  blendStyle           = "PREMULTALPHA";
};

datablock ParticleEmitterData(FitS_HugeFireBallCore_E)
{
  ejectionPeriodMS     = 100;
  periodVarianceMS     = 50;
  ejectionVelocity     = 0.25;
  velocityVariance     = 0.10;
  thetaMin             = 0;
  thetaMax             = 180;
  phiReferenceVel      = 90;
  phiVariance          = 180;
  particles            = "FitS_HugeFireBallCore_P";
  blendStyle           = "PREMULTALPHA";
};

datablock ParticleEmitterData(FitS_HugeFireBallCore_fadein_E : FitS_HugeFireBallCore_E)
{
  ejectionPeriodMS     = 100;
  periodVarianceMS     = 50;
  particles            = "FitS_HugeFireBallCore_P";
  fadeSize             = true;
};

datablock afxParticleEmitterVectorData(FitS_HugeFireballSmoke_E)
{
  ejectionPeriodMS    = 4;
  periodVarianceMS    = 1;
  ejectionVelocity    = 10;
  velocityVariance    = 1.0;
  particles           = "FitS_FireballSmokeB_P FitS_HugeFireBallCore2_P";
  vector              = "0.0 -1.0 0.0";
  fadeColor           = true;
  fadeAlpha           = true;
  blendStyle          = "PREMULTALPHA";
  vectorIsWorld       = false;
  reverseOrder        = true;
};

//~~~~~~~~~~~~~~~~~~~~//

datablock afxEffectWrapperData(FitS_FireBall_Static_New_EW)
{
  effect = FitS_HugeFireBallNew_E;
  posConstraint = "caster.mountFireball";
};

datablock afxEffectWrapperData(FitS_FireBall_New_EW : FitS_FireBall_Static_New_EW)
{
  posConstraint = missile;
};

datablock afxXM_LocalOffsetData(FitS_FireBall_Static_Core_offset1_XM)
{
  localOffset = "0.0 0.0 0.0";
};
datablock afxXM_AimData(FitS_FireBall_Static_Core_aim_XM)
{
  aimZOnly = false;  
};
datablock afxXM_LocalOffsetData(FitS_FireBall_Static_Core_offset2_XM)
{
  localOffset = "0 -3.0 0";
};

datablock afxEffectWrapperData(FitS_FireBall_Static_Core_EW)
{
  effect = FitS_HugeFireBallCore_fadein_E;
  posConstraint = "caster.mountFireball";
  posConstraint2 = "camera"; // aim
  fadeInTime = 1.0; 
  xfmModifiers[0] = FitS_FireBall_Static_Core_offset1_XM;
  xfmModifiers[1] = FitS_FireBall_Static_Core_aim_XM;
  xfmModifiers[2] = FitS_FireBall_Static_Core_offset2_XM;
};

datablock afxEffectWrapperData(FitS_FireBall_Core_EW : FitS_FireBall_Static_Core_EW)
{
  effect = FitS_HugeFireBallCore_E;
  posConstraint = missile;
};

datablock afxXM_WaveScalarData(FitS_FireBall_Smoke_oscillate1_XM)
{
  a = -50.0;
  b = 50.0;
  aVariance = 30;
  bVariance = 30;
  parameter = "ori";
  op = "multiply";
  axis = "1 0 0";
  waveform = "sine";
  speed = 1.0;
  speedVariance = 2.0;
};

datablock afxXM_WaveScalarData(FitS_FireBall_Smoke_oscillate2_XM : FitS_FireBall_Smoke_oscillate1_XM)
{
  axis = "0 0 1";
};

datablock afxEffectWrapperData(FitS_FireBall_Smoke_EW)
{
  effect = FitS_HugeFireballSmoke_E;
  constraint = missile;
  xfmModifiers[0] = FitS_FireBall_Smoke_oscillate1_XM;
  xfmModifiers[1] = FitS_FireBall_Smoke_oscillate2_XM; 
};

//~~~~~~~~~~~~~~~~~~~~//

if (afxGetEngine() $= "T3D")
{
  $FitS_Fireball_Light = "";
  $FitS_Fireball_Appearance_Light_EW = "";
}
else
{
  %FitS_Fireball_intensity = 8.0;
  datablock afxLightData(FitS_FireballLight2_CE)
  {
    type = "Point";
    radius = 20;
    sgCastsShadows = true;
    sgDoubleSidedAmbient = true;

    sgLightingModelName = (afxGetEngine() $= "TGEA") ? "Original Advanced" : "Near Linear";
    color = (255/255)*%FitS_Fireball_intensity SPC
            (15/255)*%FitS_Fireball_intensity SPC 
            (0/255)*%FitS_Fireball_intensity;
    lightIlluminationMask = $AFX::ILLUM_DTS | $AFX::ILLUM_DIF; // TGEA (ignored by TGE)
  };

  datablock afxLightData(FitS_FireballLight_Appear_CE : FitS_FireballLight2_CE)
  {
    radius = 30;
  };

  //~~~~~~~~~~~~~~~~~~~~//

  datablock afxEffectWrapperData(FitS_Fireball_Light_EW)
  {
    effect = FitS_FireballLight2_CE;
    posConstraint = missile;
    fadeInTime = 0.5;
    fadeOutTime = 0.5;
  };

  datablock afxEffectWrapperData(FitS_Fireball_Appearance_Light_EW)
  {
    effect = FitS_FireballLight_Appear_CE;
    posConstraint = "caster.mountFireball";
    lifetime = 0.75;
    fadeInTime = 0.5;
    fadeOutTime = 0.5;
  };

  $FitS_Fireball_Light = FitS_Fireball_Light_EW;
  $FitS_Fireball_Appearance_Light_EW = FitS_Fireball_Appearance_Light_EW;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// HUGE FIREBALL MISSILE

datablock afxMagicMissileData(FitS_Fireball_MM)
{
  muzzleVelocity        = 50;
  velInheritFactor      = 0;
  lifetime              = 20000;
  isBallistic           = true;
  ballisticCoefficient  = 0.85;
  gravityMod            = 0.05;
  isGuided              = true;
  precision             = 30;
  trackDelay            = 7;
  hasLight              = false;
  launchOffset          = "0 0 43.7965";
  launchOnServerSignal  = true;
  launchAimPan          = "$$ %%._aimPan";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

function FitS_LightTheTorch(%tower)
{
  %tower.ammo = getRandom(10,20);
  schedule(0, 0, FitS_ShootFireBall, %tower);
}

function FitS_ShootFireBall(%tower)
{
  if (!isObject(%tower))
    return;

  if (%tower.ammo <= 0)
  {
    %tower.schedule(0, "startFade", 1000, 0, true);
    %tower.schedule(1000+1000, "delete");
    return;
  }

  %tower.ammo--;

  // shoot the fireball
  %target = FitS_FindTarget(%tower);
  if (isObject(%target))
  {
    FitS_FireBall_Shooter_RPG._aimPan = getRandom(0,1)*180;
    castSpell(FitS_FireBall_Shooter, %tower, %target, FitS_FireBall_Shooter_RPG);
  }

  // schedule next shot
  schedule(getRandom(2800,3200), 0, FitS_ShootFireBall, %tower);
}

function FitS_FindTarget(%tower)
{
  if (!isObject(%tower))
    return "";

  // collect all objects currently in the ring
  %n_inside = 0;
  %range = 160;
  %typemask = $TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType;
  InitContainerRadiusSearch(%tower.getPosition(), %range, %typemask);

  while ((%in_obj = containerSearchNext()) != 0) 
  {
    if (%in_obj != %tower)
    {
      %inside[%n_inside] = %in_obj;
      %n_inside++;
    }
  }

  return %inside[getRandom(0,%n_inside-1)];
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxMagicSpellData(FitS_FireBall_Shooter)
{
  castingDur = 1.0;
  missile = FitS_Fireball_MM;
  extraDeliveryTime = 1;
  
  addCastingEffect = FitS_FireBall_Static_New_EW;
  addCastingEffect = FitS_FireBall_Static_Core_EW;
  addCastingEffect = $FitS_Fireball_Appearance_Light;
  
  addDeliveryEffect = FitS_FireBall_New_EW;
  addDeliveryEffect = FitS_FireBall_Core_EW;
  addDeliveryEffect = FitS_FireBall_Smoke_EW;
  addDeliveryEffect = $FitS_Fireball_Light;
};

datablock afxRPGMagicSpellData(FitS_FireBall_Shooter_RPG)
{
  directDamage = 5.0;
  areaDamage = 5;
  areaDamageRadius = 10;
  areaDamageImpulse = 2000;
};

function FitS_FireBall_Shooter::onImpact(%this, %spell, %caster, %impObj, %impPos, %impNorm)
{
  Parent::onImpact(%this, %spell, %caster, %impObj, %impPos, %impNorm);

  if (isObject(%impObj) && %impObj.isMemberOfClass("Player"))
  {
    if (%impObj.uif_fire $= "")
    {
      %impObj.uif_fire = startEffectron(UpInFlamesEffectron, %impObj, "victim");
      %impObj.uif_fire._dur = getRandomF(9,11);
      %impObj.uif_fire.uif_victim = %impObj;
    }
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
