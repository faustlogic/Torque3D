//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// PRESTO CHANGE ORC
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

// Test engine requirements for this script
if (afxGetEngine() $= "TGE")
{
  error("AFX script " @ fileName($afxAutoloadScriptFile) @ " is not compatible with the TGE engine.");
  return;
}


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

//
// Here we test if the script is being reloaded or if this is the
// first time the script has executed this mission.
//
$spell_reload = isObject(PrestoChangeOrcSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = PrestoChangeOrcSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

// this offset helps to center the hat
datablock afxXM_LocalOffsetData(PCO_TopHat_Offset_XM)
{
  localOffset = "0 -0.5 0";
};

datablock afxModelData(PCO_TopHat_CE)
{
  shapeFile = %mySpellDataPath @ "/PCO/models/PCO_TopHat.dts";
  sequence = "tophatanim";
  textureFiltering = false;
};
//
datablock afxEffectWrapperData(PCO_TopHat_EW)
{
  effect     = PCO_TopHat_CE;
  constraint = caster;
  delay = 0;
  fadeInTime  = 0.2;
  fadeOutTime = 0.1;
  lifetime = 42/30;
  //propagateTimeFactor = true;

  xfmModifiers[0] = PCO_TopHat_Offset_XM;
};

datablock afxModelData(PCO_Wand_CE)
{
  shapeFile = %mySpellDataPath @ "/PCO/models/PCO_Wand.dts";
  sequence = "wandanim";
};
//
datablock afxEffectWrapperData(PCO_Wand_EW)
{
  effect     = PCO_Wand_CE;
  constraint = caster;
  delay         = 0;
  fadeInTime    = 0;
  fadeOutTime = 5/30;
  lifetime = (50/30)-(5/30);
  //propagateTimeFactor = true;

  xfmModifiers[0] = PCO_TopHat_Offset_XM;
  
  effectName = "Wand";
  isConstraintSrc = true;
};

datablock ParticleData(PCO_SparkleA_P)
{
   // TGE textureName          = %mySpellDataPath @ "/AP/particles/AP_linearSparkle";
   dragCoeffiecient     = 0.5;
   gravityCoefficient   = 1.0;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 400;
   lifetimeVarianceMS   = 100;
   useInvAlpha          = false;
   spinRandomMin        = 0.0;
   spinRandomMax        = 0.0;
   colors[0]            = "1 1 1 1";
   colors[1]            = "1 1 1 0.5";
   colors[2]            = "0 0 0 0";
   sizes[0]             = 1.0;
   sizes[1]             = 1.0;
   sizes[2]             = 1.0;
   times[0]             = 0.0;
   times[1]             = 0.5;
   times[2]             = 1.0;

   //textureName          = %mySpellDataPath @ "/PCO/particles/PCO_sparkleA";
   textureName          = %mySpellDataPath @ "/PCO/particles/PCO_tiled_parts";
   textureCoords[0]     = "0.0 0.0";
   textureCoords[1]     = "0.0 0.5";
   textureCoords[2]     = "0.5 0.5";
   textureCoords[3]     = "0.5 0.0";
};
datablock ParticleData(PCO_SparkleB_P : PCO_SparkleA_P)
{ 
  // TGE textureName          = %mySpellDataPath @ "/AP/particles/AP_linearSparkle";   
  sizes[0]             = 0.5;
  sizes[1]             = 0.5;
  sizes[2]             = 0.5;   

  //textureName          = %mySpellDataPath @ "/PCO/particles/PCO_sparkleB";
  textureName          = %mySpellDataPath @ "/PCO/particles/PCO_tiled_parts";
  textureCoords[0]     = "0.50 0.00";
  textureCoords[1]     = "0.50 0.25";
  textureCoords[2]     = "0.75 0.25";
  textureCoords[3]     = "0.75 0.00";
};

datablock ParticleData(PCO_SparkleA_pink_P : PCO_SparkleA_P)
{
  gravityCoefficient   = -3*1.25;
  lifetimeMS           = 400*1.5;
  lifetimeVarianceMS   = 100*1.5;
  
  textureCoords[0]     = "0.5 0.5";
  textureCoords[1]     = "0.5 1.0";
  textureCoords[2]     = "1.0 1.0";
  textureCoords[3]     = "1.0 0.5";
};
datablock ParticleData(PCO_SparkleB_pink_P : PCO_SparkleB_P)
{
  gravityCoefficient   = -3*1.6;
  lifetimeMS           = 400*2.5;
  lifetimeVarianceMS   = 100*2.5;

  textureCoords[0]     = "0.75 0.00";
  textureCoords[1]     = "0.75 0.25";
  textureCoords[2]     = "1.00 0.25";
  textureCoords[3]     = "1.00 0.00";
};

datablock ParticleEmitterData(PCO_WandSparkles_E)
{
  // TGE emitterType = "sprinkler";

  ejectionOffset        = 0.02;
  ejectionPeriodMS      = 5; //10;
  periodVarianceMS      = 2; //3;
  ejectionVelocity      = 2;
  velocityVariance      = 1;  
  particles             = "PCO_SparkleA_P PCO_SparkleB_P PCO_SparkleB_P";  
};

datablock afxEffectWrapperData(PCO_WandSparkles_EW)
{
  effect     = PCO_WandSparkles_E;
  posConstraint = "#effect.Wand.mountWand";
  
  delay         = 30/30;
  fadeInTime    = 0;
  fadeOutTime = 5/30;
  lifetime = 0.35;
  //propagateTimeFactor = true;
};

datablock ParticleData(PCO_ExplosionSmoke_P)
{
  //textureName          = %mySpellDataPath @ "/PCO/particles/smoke";
  textureName          = %mySpellDataPath @ "/PCO/particles/PCO_smoke";
  dragCoeffiecient     = 100.0;
  gravityCoefficient   = 0;
  inheritedVelFactor   = 0.25;
  constantAcceleration = -0.30;
  lifetimeMS           = 1200;
  lifetimeVarianceMS   = 300;
  useInvAlpha =  true;
  spinRandomMin = -80.0;
  spinRandomMax =  80.0;

  colors[0]     = "1.0 0.5 0.5 1.0";
  colors[1]     = "1.0 0.5 0.5 0.5"; //1.0";
  colors[2]     = "0.4 0.4 0.4 0.25; //1.0";
  colors[3]     = "0.0 0.0 0.0 0.0";

  sizes[0]      = 2; //4.0;
  sizes[1]      = 4; //6.0;
  sizes[2]      = 8.0;
  sizes[3]      = 15; //10.0;

  times[0]      = 0.0;
  times[1]      = 0.2; //0.33;
  times[2]      = 0.4; //0.66;
  times[3]      = 1.0;
};
//
datablock ParticleData(PCO_ExplosionFire_pRot_P)
{
  //textureName          = %mySpellDataPath @ "/PCO/particles/PCO_fireExplosion";
  textureName          = %mySpellDataPath @ "/PCO/particles/PCO_tiled_parts";
  textureCoords[0]     = "0.0 0.5";
  textureCoords[1]     = "0.0 1.0";
  textureCoords[2]     = "0.5 1.0";
  textureCoords[3]     = "0.5 0.5";

  dragCoeffiecient     = 100.0;
  gravityCoefficient   = -3; //0;
  inheritedVelFactor   = 0.25;
  constantAcceleration = 0.1;
  lifetimeMS           = 600; //1200;
  lifetimeVarianceMS   = 150; //300;
  useInvAlpha          =  false;
  spinRandomMin        = 700.0;
  spinRandomMax        = 900.0;
  colors[0]            = "1.0 1.0 1.0 1.0"; //"1.0 1.0 1.0 1.0";
  colors[1]            = "0.75 0.75 0.75 0.75"; //"1.0 1.0 0.0 1.0";
  colors[2]            = "0.5 0.5 0.5 0.5"; //"1.0 0.0 0.0 1.0";
  colors[3]            = "0.25 0.25 0.25 0.0"; //"1.0 0.0 0.0 0.0";
  sizes[0]             = 0.1; //3.0*0.5*0.5;
  sizes[1]             = 4.5; //5.0*0.5;
  sizes[2]             = 3.0; //7.0*0.5;
  sizes[3]             = 3.0*0.5;
  times[0]             = 0.0;
  times[1]             = 0.4;//0.2;
  times[2]             = 0.7;
  times[3]             = 1.0;   
};
datablock ParticleData(PCO_ExplosionFire_nRot_P : PCO_ExplosionFire_pRot_P)
{
  spinRandomMin        = -900.0;
  spinRandomMax        = -700.0;
};
//
datablock ParticleEmitterData(PCO_ExplosionFire_E)
{
   ejectionPeriodMS = 10;
   periodVarianceMS = 0;
   ejectionVelocity = 0.8;
   velocityVariance = 0.5;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   lifetimeMS       = 250;
   particles        = "PCO_ExplosionFire_pRot_P PCO_ExplosionFire_nRot_P" SPC
                      "PCO_SparkleA_pink_P PCO_SparkleA_pink_P" SPC
                      "PCO_SparkleB_pink_P PCO_SparkleB_pink_P";
};
//
datablock ParticleEmitterData(PCO_ExplosionSmoke_E)
{
   ejectionPeriodMS = 5;
   periodVarianceMS = 0;
   ejectionVelocity = 10;
   velocityVariance = 1.5;
   thetaMin         = 0.0;
   thetaMax         = 180.0;
   lifetimeMS       = 250;
   particles        = "PCO_ExplosionSmoke_P";
};
//
datablock ExplosionData(PCO_Explosion_CE)
{
   lifeTimeMS = 1200;

   // Volume particles
   particleEmitter = PCO_ExplosionFire_E;
   particleDensity = 50; //20;
   particleRadius = 3*0.6;

   // Point emission
   emitter[0] = PCO_ExplosionSmoke_E;
   emitter[1] = ""; //PCO_ExplosionSmoke_E;

   // Impulse
   impulseRadius = 10;
   impulseForce = 15;
};
//
datablock afxEffectWrapperData(PCO_Explosion_EW)
{
  effect = PCO_Explosion_CE;
  constraint = "caster";
  delay = (42/30);
};

datablock afxParticleEmitterDiscData(PCO_ExplosionSpirals_E) // TGEA
{
  ejectionOffset        = 0.0;
  ejectionPeriodMS      = 5; //3;
  periodVarianceMS      = 2; //1;
  ejectionVelocity      = 10.0;
  velocityVariance      = 3.0;  
  particles             = "PCO_ExplosionFire_pRot_P PCO_ExplosionFire_nRot_P" SPC
                          "PCO_SparkleA_pink_P PCO_SparkleA_pink_P" SPC
                          "PCO_SparkleB_pink_P PCO_SparkleB_pink_P";

  // TGE emitterType = "disc";
  vector = "0 0 1";
  radiusMin = 0.0;
  radiusMax = 3*0.6;

  //fadeColor = true;
  //fadeSize = true;
};
//
datablock afxEffectWrapperData(PCO_ExplosionSpirals_EW)
{
  effect = PCO_ExplosionSpirals_E;
  constraint = "caster";
  delay = (42/30);
};

datablock afxParticleEmitterConeData(PCO_ExplosionSmoke2_E) // TGEA
{
  ejectionOffset        = 2.0; //4.5;
  ejectionPeriodMS      = 2; //8;
  periodVarianceMS      = 1; //2;
  ejectionVelocity      = 6.0*3;
  velocityVariance      = 1.5*3;  
  particles             = "PCO_ExplosionSmoke_P";

  // TGE emitterType = "cone";
  vector = "0.0 0.0 1.0";
  spreadMin = 179.0;
  spreadMax = 179.0;

  //fadeOffset = true;
};
//
datablock afxEffectWrapperData(PCO_ExplosionSmoke_EW)
{
  effect = PCO_ExplosionSmoke2_E;
  constraint = "caster";
  delay = (42/30);
};

datablock afxZodiacData(PCO_ScorchedEarth_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/PCO/zodiacs/PCO_blastimpact";
  radius = 15.0; //25.0;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 1.0";
};
//
// constraint is being destroyed with orc swap!; use residue...
datablock afxEffectWrapperData(PCO_ScorchedEarth_EW)
{
  effect = PCO_ScorchedEarth_CE;
  posConstraint = impactPoint; //caster;
  delay = (45/30);
  fadeInTime = 1.0; //0.25;
  lifetime = 1.0; //0.25;
  residueLifetime = 5;
  fadeOutTime = 3;
  xfmModifiers[0] = SHARED_AltitudeConform_XM;
};


datablock afxScriptEventData(PCO_FadeOutScript_CE)
{
  methodName = "FadeOutCaster";   // name of method in afxMagicSpellData subclass
};
datablock afxEffectWrapperData(PCO_FadeOutScript_EW)
{
  effect = PCO_FadeOutScript_CE;
  constraint = "impactedObject";
  delay = (42/30);
};

datablock afxScriptEventData(PCO_OrcSwapScript_CE)
{
  methodName = "SwapOrcCaster";   // name of method in afxMagicSpellData subclass
};
datablock afxEffectWrapperData(PCO_OrcSwapScript_EW)
{
  effect = PCO_OrcSwapScript_CE;
  constraint = "impactedObject";  
  delay = (42/30);
};

datablock afxAnimClipData(PCO_HeadClip_CE)
{
  clipName = "head"; // this is a blend sequence
  posOffset = 0.5;
  rate = -1.0;
  ignoreFirstPerson = true;
};
//
datablock afxEffectWrapperData(PCO_HeadClip_EW)
{
  effect = PCO_HeadClip_CE;
  constraint = "caster";
  delay = 0.5;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  /* not available for TGE
  case "TGE":
    exec("./lighting/pco_lighting_tge_sub.cs");
    exec("./audio/pco_audio_sub.cs");
  */
  case "TGEA":
    exec("./lighting/pco_lighting_tgea_sub.cs");
    exec("./audio/pco_audio_sub.cs");
 case "T3D":
    exec("./lighting/pco_lighting_t3d_sub.cs");
    exec("./audio/pco_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

datablock afxMagicSpellData(PrestoChangeOrcSpell)
{
  castingDur = 1.5;

  addCastingEffect = PCO_HeadClip_EW;

  addCastingEffect = PCO_TopHat_EW;
  addCastingEffect = PCO_Wand_EW;
  addCastingEffect = PCO_WandSparkles_EW;
  
  //addCastingEffect = PCO_Explosion_EW;
  addCastingEffect = PCO_ExplosionSpirals_EW;
  addCastingEffect = PCO_ExplosionSmoke_EW;
  
  addCastingEffect = PCO_ScorchedEarth_EW;
  
  addCastingEffect = PCO_FadeOutScript_EW;
  addCastingEffect = PCO_OrcSwapScript_EW;
};

// sounds and lights added via sub-script functions //
PCO_add_Lighting_FX(PrestoChangeOrcSpell);
PCO_add_Audio_FX(PrestoChangeOrcSpell);

datablock afxRPGMagicSpellData(PrestoChangeOrcSpell_RPG)
{
  spellName = "Presto Change-Orc";
  desc = "Who says all the GOOD orcs are taken? " @
         "Magically TRADE your current body for a new one.\n" @
         "\nspell design: Matthew Durante" @
         "\nspell concept: Matthew Durante and Jeff Faust";
  sourcePack = "Core Tech [TGEA]";
  iconBitmap = %mySpellDataPath @ "/PCO/icons/pco";
  target = "self";
  manaCost = 10;
  reagentCost = 0;
  castingDur = PrestoChangeOrcSpell.castingDur;
};


// script methods

function PrestoChangeOrcSpell::FadeOutCaster(%this, %spell, %caster, %constraint, %pos, %data)
{
  if (isObject(%caster))
    %caster.startFade(500, 0, true);
}

function PrestoChangeOrcSpell::SwapOrcCaster(%this, %spell, %caster, %constraint, %pos, %data)
{
   if (isObject(%caster) && isObject(%caster.client))
   {
      if (%caster.dataBlock $= OrcMageData)
      {
        if (isObject(theLevelInfo))
          %night_mission = theLevelInfo.isNightMission;
        else if (isObject(MissionInfo))
          %night_mission = MissionInfo.isNightMission;

         if (%night_mission)
            %body_type = SpaceOrcMage_Night_Data;
         else
            %body_type = SpaceOrcMageData;
      }
      else
         %body_type = OrcMageData;

      %energy_level = %caster.getEnergyLevel();
      %damage_level = %caster.getDamageLevel();

      %caster.setDatablock(%body_type);
      %caster.startFade(500, 0, false);

      %caster.setEnergyLevel(%energy_level);
      %caster.setDamageLevel(%damage_level);
   }
}

function PrestoChangeOrcSpell::onInterrupt(%this, %spell, %caster)
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
  PrestoChangeOrcSpell.scriptFile = $afxAutoloadScriptFile;
  PrestoChangeOrcSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
  {    
    addDemoSpellbookSpell(PrestoChangeOrcSpell, PrestoChangeOrcSpell_RPG);
  }
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//