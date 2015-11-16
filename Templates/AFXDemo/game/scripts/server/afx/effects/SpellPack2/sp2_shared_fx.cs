
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SPELLPACK 2 SHARED EFFECTS
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

// set data path from default plus containing folder name
%mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Fire Particles

datablock ParticleData(SP2_Fire_A_P)
{
  textureName          = %mySpellDataPath @ "/Shared/particles/sp2_flame_2x2";
  textureCoords[0] = "0.0 0.0";
    textureCoords[1] = "0.0 0.5";
    textureCoords[2] = "0.5 0.5";
    textureCoords[3] = "0.5 0.0";
  dragCoeffiecient     = 0.5;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 600;
  lifetimeVarianceMS   = 100;
  spinRandomMin        = -500;
  spinRandomMax        = 500;
  colors[0]            = "1.0 1.0 1.0 0.2";
    colors[1]            = "1.0 0.8 0.5 0.2";
    colors[2]            = "1.0 0.3 0.1 0.2";
    colors[3]            = "0.0 0.0 0.0 0.0";
  sizeBias             = 1.0;
  sizes[0]             = 1.0;
    sizes[1]             = 3.5;
    sizes[2]             = 2.0;
    sizes[3]             = 1.0;
  times[0]             = 0.0;
    times[1]             = 0.2;
    times[2]             = 0.4;
    times[3]             = 1.0;
};
datablock ParticleData(SP2_Fire_B_P : SP2_Fire_A_P)
{
  textureCoords[0] = "0.5 0.0";
    textureCoords[1] = "0.5 0.5";
    textureCoords[2] = "1.0 0.5";
    textureCoords[3] = "1.0 0.0";
};

datablock ParticleEmitterData(SP2_Fire_E)
{
  ejectionPeriodMS  = 15;
  periodVarianceMS  = 6;
  ejectionVelocity  = 0;
  velocityVariance  = 0;
  thetaMin          = 0.0;
  thetaMax          = 0.0;
  particles         = "SP2_Fire_A_P SP2_Fire_B_P";
  fadeSize          = true;
  blendStyle        = "PREMULTALPHA";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Ground Fire Particles

datablock ParticleData(SP2_GroundFire_LG_P)
{
  textureName          = %mySpellDataPath @ "/Shared/particles/sp2_groundFire";
  dragCoeffiecient     = 0;
  gravityCoefficient   = 0;
  windCoefficient      = 0;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 400;
  lifetimeVarianceMS   = 250;
  useInvAlpha          = false;
  spinRandomMin        = 0.0;
  spinRandomMax        = 0.0;
  colors[0]            = "1 1 0.9 0.2";
    colors[1]            = "1 1 0.9 0.4";
    colors[2]            = "1 1 0.9 0.8";
    colors[3]            = "1 1 0.9 0";
  sizeBias             = 0.9;
  sizes[0]             = 0.10;
    sizes[1]             = 0.50;
    sizes[2]             = 0.13;
    sizes[3]             = 0.20;
  times[0]             = 0.0;
    times[1]             = 0.3;
    times[2]             = 0.7;
    times[3]             = 1.0;
  blendType            = additive;
};
datablock ParticleData(SP2_GroundFire_MED_P : SP2_GroundFire_LG_P)
{
  sizeBias = 0.65;
};
datablock ParticleData(SP2_GroundFire_SM_P : SP2_GroundFire_LG_P)
{
  sizeBias = 0.4;
};
datablock ParticleData(SP2_GroundFire_RND_P : SP2_GroundFire_LG_P)
{
  sizeBias = "$$ getRandomF(0.4,0.9)";
};

datablock ParticleEmitterData(SP2_GroundFire_LG_E)
{
  ejectionPeriodMS = 60;
  periodVarianceMS = 5;
  ejectionVelocity = 0.0;
  velocityVariance = 0.0;
  particles        = SP2_GroundFire_LG_P;
  fadeSize         = true;
  blendStyle       = "additive";
};
datablock ParticleEmitterData(SP2_GroundFire_MED_E : SP2_GroundFire_LG_E)
{
  particles = SP2_GroundFire_MED_P;
};
datablock ParticleEmitterData(SP2_GroundFire_SM_E : SP2_GroundFire_LG_E)
{
  particles = SP2_GroundFire_SM_P;
};
datablock ParticleEmitterData(SP2_GroundFire_RND_E : SP2_GroundFire_LG_E)
{
  particles = SP2_GroundFire_RND_P;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// Tendril Smoke Particles

datablock ParticleData(SP2_TendrilSmokeA_P)
{
  textureName           = %mySpellDataPath @ "/Shared/particles/sp2_tendrilSmoke";
  dragCoeffiecient      = 0.5;
  gravityCoefficient    = -0.4;
  windCoefficient       = 0;
  inheritedVelFactor    = 0.00;
  lifetimeMS            = 2000;
  lifetimeVarianceMS    = 200;
  spinRandomMin         = -300.0;
  spinRandomMax         = 300.0;
  colors[0]             = "0.2 0.2 0.2 0.2";
    colors[1]             = "0.5 0.5 0.5 0.05";
    colors[2]             = "1.0 1.0 1.0 0.02"; //0.01"; -- too low
    colors[3]             = "0.0 0.0 0.0 0.0";
  sizes[0]              = 0.2;
    sizes[1]              = 0.7;
    sizes[2]              = 1.3;
    sizes[3]              = 2.0;
  times[0]              = 0.0;
    times[1]              = 0.3;
    times[2]              = 0.7;
    times[3]              = 1.0;
};
datablock ParticleData(SP2_TendrilSmokeB_P : SP2_TendrilSmokeA_P)
{
  lifetimeMS            = 2000*2;
  lifetimeVarianceMS    =  200*2;
};
datablock ParticleData(SP2_TendrilSmokeC_P : SP2_TendrilSmokeA_P)
{
  lifetimeMS            = 2000*0.6;
  lifetimeVarianceMS    =  200*0.6;
};
datablock ParticleData(SP2_TendrilSmokeRND_P : SP2_TendrilSmokeA_P)
{
  lifetimeMS            = "$$ 2000*getRandomF(0.6,2.0)";
  lifetimeVarianceMS    = "$$ 200*getRandomF(0.6,2.0)";
};

// vector emitter pointing straight up
datablock afxParticleEmitterVectorData(SP2_TendrilSmokeA_E)
{
  ejectionPeriodMS      = 20;
  periodVarianceMS      = 0;
  ejectionVelocity      = 1.0;
  velocityVariance      = 0.0;
  particles             = SP2_TendrilSmokeA_P;
  vector                = "0.0 0.0 1.0";
  fadeColor             = true;
  blendStyle            = "NORMAL";
};
datablock afxParticleEmitterVectorData(SP2_TendrilSmokeB_E : SP2_TendrilSmokeA_E)
{
  particles = SP2_TendrilSmokeB_P;
};
datablock afxParticleEmitterVectorData(SP2_TendrilSmokeC_E : SP2_TendrilSmokeA_E)
{
  particles = SP2_TendrilSmokeC_P;
};
datablock afxParticleEmitterVectorData(SP2_TendrilSmokeRND_E : SP2_TendrilSmokeA_E)
{
  particles = SP2_TendrilSmokeRND_P;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//