
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// OCCAMS'S LASER (lighting)
//    This script implements lighting for all 3 effect variations.
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

// DRONE POINT LIGHT

datablock afxXM_LocalOffsetData(SF_PointLight_offset_XM)
{
  localOffset = "0.0 0.0 -0.5";
};

datablock afxXM_LocalOffsetData(SF_PointLight_offset2_XM)
{
  localOffset = "0.0 0.0 -1.0";
};

// Two lights are used to simulate light from the drones engines.
//  The main light is warm and shadow-casting. A secondary small
//  white light attempts to create a hotspot on the ground as the
//  drone descends, however if the light is too intense it tends
//  to wash-out the underside of the drone, so it's somewhat subtle.

datablock afxT3DPointLightData(SF_Drone_PointLight_CE)
{
  radius = 4.5;
  color = "1.0 0.6 0.25";
  brightness = 0.75;
  castShadows = false; // T3D Off (temporarily)
  localRenderViz = false;
};

datablock afxT3DPointLightData(SF_Drone_PointLightWhite_CE)
{
  radius = 3;
  color = "1.0 1.0 1.0";
  brightness = 0.5;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(SF_Drone_PointLight_EW)
{
  effect = SF_Drone_PointLight_CE;
  posConstraint = "#effect.DroneMooring";
  delay       = %SCIFI_Satellite_delay;
  fadeInTime  = 1.0;
  fadeOutTime = 1.0;
  lifetime    = %SCIFI_Satellite_lifetime;
  xfmModifiers[0] = SF_PointLight_offset_XM;
};

datablock afxEffectWrapperData(SF_Drone_PointLightWhite_EW)
{
  effect = SF_Drone_PointLightWhite_CE;
  posConstraint = "#effect.DroneMooring";
  delay       = %SCIFI_Satellite_delay;
  fadeInTime  = 1.0;
  fadeOutTime = 1.0;
  lifetime    = %SCIFI_Satellite_lifetime;
  xfmModifiers[0] = SF_PointLight_offset2_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// DRONE TELEPORTATION -- LIGHTING

//
// Various lights are used during teleportation to enhance the effect.
// A simple white light is used throughout.  However, with advanced
// lighting on, a purplish light is created that reflects the color
// of the main teleportation particles.  Also three flares are used,
// timed to introduce the in-teleport and conclude the out.  The sound
// effects are roughly timed to accent these flares.
//

// Standard white light
datablock afxT3DPointLightData(SF_Tele_Light_CE)
{
  radius = 6;
  color = "1.0 1.0 1.0";
  brightness = 0.5;
  castShadows = false; // T3D Off (temporarily)
  localRenderViz = false;
};

datablock afxEffectWrapperData(SF_Tele_IN_Light_EW : SF_Tele_IN_Beam_EW)
{
  effect = SF_Tele_Light_CE;
};

datablock afxEffectWrapperData(SF_Tele_OUT_Light_EW : SF_Tele_OUT_Beam_EW)
{
  effect = SF_Tele_Light_CE;
};

// Purplish light
datablock afxT3DPointLightData(SF_Tele_ColoredLight_CE)
{
  radius = 7;
  color = "0.776 0.216 0.918";
  brightness = 0.5;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(SF_Tele_ColoredLight_IN_EW : SF_Tele_IN_Beam_EW)
{
  effect = SF_Tele_ColoredLight_CE;    
};

datablock afxEffectWrapperData(SF_Tele_ColoredLight_OUT_EW : SF_Tele_OUT_Beam_EW)
{
  effect = SF_Tele_ColoredLight_CE;
};

/*
// Light flares
//  There are three flares that introduce the in-teleportation, and
//   three flares that conclude the out.  Each is offset in space
//   to create a rising or falling sequence of flashes.
%SF_Tele_Flare_intensity = 5.0;
datablock sgLightObjectData(SF_Tele_Flare_CE)
{
  CastsShadows = false;
  Radius = 1.3;
  Brightness = %SF_Tele_Flare_intensity;
  Colour = "1 1 1";
  LightingModelName = "Original Advanced";

  FlareOn = true;
  LinkFlare = true;
  FlareBitmap = "common/lighting/corona";
  NearSize = 3;
  FarSize  = 2;
  NearDistance = 2;
  FarDistance  = 50;
};
// flare offsets
//  (middle flare of each sequence is at "0 0 0", so it requires
//   no offset)
datablock afxXM_LocalOffsetData(SF_Tele_Flare_offset1_XM)
{
  localOffset = "0 0 1.0";
};
datablock afxXM_LocalOffsetData(SF_Tele_Flare_offset2_XM)
{
  localOffset = "0 0 -1.0";
};
// IN-teleport flares
datablock afxEffectWrapperData(SF_Tele_Flare1_IN_EW)
{
  effect = SF_Tele_Flare_CE;
  posConstraint = "#effect.DroneMooring";
  delay       = 0;
  fadeInTime  = 0.25;
  fadeOutTime = 0.50;
  lifetime    = 0.25;
  xfmModifiers[0] = SF_Tele_Flare_offset1_XM;
};
datablock afxEffectWrapperData(SF_Tele_Flare2_IN_EW : SF_Tele_Flare1_IN_EW)
{
  delay       = 0.08;
  xfmModifiers[0] = "";
};
datablock afxEffectWrapperData(SF_Tele_Flare3_IN_EW : SF_Tele_Flare1_IN_EW)
{
  delay       = 0.16;
  xfmModifiers[0] = SF_Tele_Flare_offset2_XM;
};
// OUT-teleport flares
datablock afxEffectWrapperData(SF_Tele_Flare1_OUT_EW : SF_Tele_Flare1_IN_EW)
{
  delay = %SCIFI_Satellite_OUT_delay+2.5;
  xfmModifiers[0] = SF_Tele_Flare_offset2_XM;
};
datablock afxEffectWrapperData(SF_Tele_Flare2_OUT_EW : SF_Tele_Flare1_OUT_EW)
{
  delay = %SCIFI_Satellite_OUT_delay+2.5+0.08;
  xfmModifiers[0] = "";
};
datablock afxEffectWrapperData(SF_Tele_Flare3_OUT_EW : SF_Tele_Flare1_OUT_EW)
{
  delay = %SCIFI_Satellite_OUT_delay+2.5+0.16;
  xfmModifiers[0] = SF_Tele_Flare_offset1_XM;
};
*/

datablock afxT3DPointLightData(SF_Beam_Blast_Light_CE)
{
  radius = 20;
  color = "1.0 0.3 0.3";
  brightness = 2.0;
  castShadows = false; // T3D Off (temporarily)
  localRenderViz = false;
};

// light is moved out in front of the drone a bit
datablock afxXM_LocalOffsetData(SF_Drone_GunOffset_XM)
{
  localOffset = "0 3.0 0";
};
//
datablock afxEffectWrapperData(SF_Beam_Blast_Light_EW : SF_Drone_Body_EW)
{
  effect = SF_Beam_Blast_Light_CE;
  delay = %SCIFI_Satellite_delay+6.0;
  lifetime = 1.0-0.3;
  fadeInTime = 0.3;
  fadeOutTime = 0.3;
  xfmModifiers[0] = SF_Drone_GunOffset_XM;
};

// Beam impact light
//  -- the following vertical jumping path creates something of a
//      flickering appearance...
datablock afxPathData(SF_Beam_Impact_Light_Path)
{
  points = "0 0 1.2"  SPC
           "0 0 0.7"  SPC
           "0 0 1.74" SPC
           "0 0 1.1"  SPC
           "0 0 0.5"  SPC
           "0 0 0.8"  SPC
           "0 0 0.1"  SPC
           "0 0 1.0"  SPC
           "0 0 1.12" SPC
           "0 0 0.4"  SPC
           "0 0 1.3"  SPC
           "0 0 0.0"  SPC
           "0 0 1.3";
};

datablock afxXM_PathConformData(SF_Beam_Impact_Light_Path_XM)
{
  paths = SF_Beam_Impact_Light_Path;
};

/*
datablock afxLightData(SF_Beam_Impact_Light_CE)
{
  type = "Point";
  color = 1.0*1.5 SPC 0.15*1.5 SPC 0.15*1.5;
  radius = 4.0;
  sgLightingModelName = "Original Stock";
};
*/

datablock afxT3DPointLightData(SF_Beam_Impact_Light_CE)
{
  radius = 4;
  color = "1.0 0.15 0.15";
  brightness = 1.5;
  castShadows = false;
  localRenderViz = false;
};

datablock afxEffectWrapperData(SF_Beam_Impact_Light_EW)
{
  effect = SF_Beam_Impact_Light_CE;
  constraint = "impactPoint";
  delay       = 0;
  fadeInTime  = 0.2;
  fadeOutTime = 0.2;
  lifetime    = 0.7;
  xfmModifiers[0] = SF_Beam_Impact_Light_Path_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// DRONE TELEPORTATION -- LIGHTING (RELOADED - MACHINE GUN)

//
// Most of the effects defined here are simply copies of the previous
// spell's datablocks with the new mooring constraint specified.
//

// Re-constrained Drone Light Effects
datablock afxEffectWrapperData(SF_Drone_PointLight_MG_EW : SF_Drone_PointLight_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};

datablock afxEffectWrapperData(SF_Drone_PointLightWhite_MG_EW : SF_Drone_PointLight_MG_EW)
{
  effect = SF_Drone_PointLightWhite_CE;
  xfmModifiers[0] = SF_PointLight_offset2_XM;
};

datablock afxEffectWrapperData(SF_Tele_IN_Light_MG_EW : SF_Tele_IN_Light_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Tele_OUT_Light_MG_EW : SF_Tele_OUT_Light_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};

datablock afxEffectWrapperData(SF_Tele_ColoredLight_IN_MG_EW : SF_Tele_IN_Beam_MG_EW)
{
  effect = SF_Tele_ColoredLight_CE;
};
datablock afxEffectWrapperData(SF_Tele_ColoredLight_OUT_MG_EW : SF_Tele_OUT_Beam_MG_EW)
{
  effect = SF_Tele_ColoredLight_CE;
};

/*
// IN-teleport flares
datablock afxEffectWrapperData(SF_Tele_Flare1_IN_MG_EW : SF_Tele_Flare1_IN_EW)
{
  posConstraint = "#ghost.DroneMooring_MG";
};
datablock afxEffectWrapperData(SF_Tele_Flare2_IN_MG_EW : SF_Tele_Flare1_IN_MG_EW)
{
  delay       = 0.08;
  xfmModifiers[0] = "";
};
datablock afxEffectWrapperData(SF_Tele_Flare3_IN_MG_EW : SF_Tele_Flare1_IN_MG_EW)
{
  delay       = 0.16;
  xfmModifiers[0] = SF_Tele_Flare_offset2_XM;
};

// OUT-teleport flares
datablock afxEffectWrapperData(SF_Tele_Flare1_OUT_MG_EW : SF_Tele_Flare1_IN_MG_EW)
{
  delay = %SCIFI_Satellite_OUT_delay+2.5;
  xfmModifiers[0] = SF_Tele_Flare_offset2_XM;
};
datablock afxEffectWrapperData(SF_Tele_Flare2_OUT_MG_EW : SF_Tele_Flare1_OUT_MG_EW)
{
  delay = %SCIFI_Satellite_OUT_delay+2.5+0.08;
  xfmModifiers[0] = "";
};
datablock afxEffectWrapperData(SF_Tele_Flare3_OUT_MG_EW : SF_Tele_Flare1_OUT_MG_EW)
{
  delay = %SCIFI_Satellite_OUT_delay+2.5+0.16;
  xfmModifiers[0] = SF_Tele_Flare_offset1_XM;
};
*/

/*
// Laser Pulse Light Flashes
//  timing variables
%SF_LaserPulseFlash_1_delay = %SCIFI_Satellite_delay+%SCIFI_Satellite_aim_delay+%SCIFI_Satellite_aim_fade + 0.4;
%SF_LaserPulseFlash_2_delay = %SF_LaserPulseFlash_1_delay + 0.5;
%SF_LaserPulseFlash_3_delay = %SF_LaserPulseFlash_2_delay + 0.5;
//  light
datablock afxLightData(SF_LaserPulseLight_CE)
{
  type = "Point";
  color = "0.5 0.5 0.5";
  radius = 11.0;
  sgCastsShadows = true;
  sgLightingModelName = "Original Stock";
};
//  six flashes, 3 on each side (gun)
datablock afxEffectWrapperData(SF_LaserPulseFlash_LF_1_EW)
{
  effect = SF_LaserPulseLight_CE;
  posConstraint = "#ghost.DroneMooring_MG";

  delay       = %SF_LaserPulseFlash_1_delay;
  lifetime    = 0.3;
  fadeInTime  = 0.10;
  fadeOutTime = 0.15;

  xfmModifiers[0] = SF_Drone_GunOffset_LF_XM;
};
datablock afxEffectWrapperData(SF_LaserPulseFlash_RT_1_EW : SF_LaserPulseFlash_LF_1_EW)
{
  xfmModifiers[0] = SF_Drone_GunOffset_RT_XM;
};
datablock afxEffectWrapperData(SF_LaserPulseFlash_LF_2_EW : SF_LaserPulseFlash_LF_1_EW)
{
  delay       = %SF_LaserPulseFlash_2_delay;
};
datablock afxEffectWrapperData(SF_LaserPulseFlash_RT_2_EW : SF_LaserPulseFlash_RT_1_EW)
{
  delay       = %SF_LaserPulseFlash_2_delay;
};
datablock afxEffectWrapperData(SF_LaserPulseFlash_LF_3_EW : SF_LaserPulseFlash_LF_1_EW)
{
  delay       = %SF_LaserPulseFlash_3_delay;
};
datablock afxEffectWrapperData(SF_LaserPulseFlash_RT_3_EW : SF_LaserPulseFlash_RT_1_EW)
{
  delay       = %SF_LaserPulseFlash_3_delay;
};
*/

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// DRONE TELEPORTATION -- LIGHTING (REDUX - STATIC SHAPE)

//
// Most of the effects defined here are simply copies of the previous
// spell's datablocks with the new mooring constraint specified.
//

// Re-constrained Drone Light Effects
datablock afxEffectWrapperData(SF_Drone_PointLight_S_EW : SF_Drone_PointLight_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Drone_PointLightWhite_S_EW : SF_Drone_PointLight_S_EW)
{
  effect = SF_Drone_PointLightWhite_CE;
  xfmModifiers[0] = SF_PointLight_offset2_XM;
};

datablock afxEffectWrapperData(SF_Tele_IN_Light_S_EW : SF_Tele_IN_Light_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};
datablock afxEffectWrapperData(SF_Tele_OUT_Light_S_EW : SF_Tele_OUT_Light_EW)
{
  posConstraint = "#ghost.DroneMooring_S";
};

datablock afxEffectWrapperData(SF_Tele_ColoredLight_IN_S_EW : SF_Tele_IN_Beam_S_EW)
{
  effect = SF_Tele_ColoredLight_CE;
};
datablock afxEffectWrapperData(SF_Tele_ColoredLight_OUT_S_EW : SF_Tele_OUT_Beam_S_EW)
{
  effect = SF_Tele_ColoredLight_CE;
};

/*
datablock afxEffectWrapperData(SF_Tele_Flare1_IN_S_EW : SF_Tele_Flare1_IN_EW)
{   
  posConstraint = "#ghost.DroneMooring_S";   
};
datablock afxEffectWrapperData(SF_Tele_Flare2_IN_S_EW : SF_Tele_Flare1_IN_S_EW)
{
  delay       = 0.08;
  xfmModifiers[0] = "";
};
datablock afxEffectWrapperData(SF_Tele_Flare3_IN_S_EW : SF_Tele_Flare1_IN_S_EW)
{
  delay       = 0.16;
  xfmModifiers[0] = SF_Tele_Flare_offset2_XM;
};
*/
  
/*
datablock afxEffectWrapperData(SF_Tele_Flare1_OUT_S_EW : SF_Tele_Flare1_IN_S_EW)
{
  delay = %SCIFI_Satellite_OUT_delay+2.5;
  xfmModifiers[0] = SF_Tele_Flare_offset2_XM;
};
datablock afxEffectWrapperData(SF_Tele_Flare2_OUT_S_EW : SF_Tele_Flare1_OUT_S_EW)
{
  delay = %SCIFI_Satellite_OUT_delay+2.5+0.08;
  xfmModifiers[0] = "";
};
datablock afxEffectWrapperData(SF_Tele_Flare3_OUT_S_EW : SF_Tele_Flare1_OUT_S_EW)
{
  delay = %SCIFI_Satellite_OUT_delay+2.5+0.16;
  xfmModifiers[0] = SF_Tele_Flare_offset1_XM;
};
*/

datablock afxEffectWrapperData(SF_Beam_Blast_Light_S_EW : SF_Beam_Blast_Light_EW)
{
  constraint = "#ghost.DroneMooring_S";    
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

// add effects to main spell datablock
function OL_main_add_Lighting_FX(%spell_data)
{
  %spell_data.addCastingEffect(SF_Drone_PointLight_EW);
  %spell_data.addCastingEffect(SF_Drone_PointLightWhite_EW);
  %spell_data.addCastingEffect(SF_Tele_IN_Light_EW);
  %spell_data.addCastingEffect(SF_Tele_ColoredLight_IN_EW);
  /*
  %spell_data.addCastingEffect(SF_Tele_Flare1_IN_EW);
  %spell_data.addCastingEffect(SF_Tele_Flare2_IN_EW);
  %spell_data.addCastingEffect(SF_Tele_Flare3_IN_EW);
  */
  %spell_data.addCastingEffect(SF_Tele_OUT_Light_EW);
  %spell_data.addCastingEffect(SF_Tele_ColoredLight_OUT_EW);
  /*
  %spell_data.addCastingEffect(SF_Tele_Flare1_OUT_EW);
  %spell_data.addCastingEffect(SF_Tele_Flare2_OUT_EW);
  %spell_data.addCastingEffect(SF_Tele_Flare3_OUT_EW);
  */
  %spell_data.addDeliveryEffect(SF_Beam_Blast_Light_EW);
  %spell_data.addImpactEffect(SF_Beam_Impact_Light_EW);
}

// add effects to main spell datablock
function OL_reloaded_add_Lighting_FX(%spell_data)
{
  %spell_data.addCastingEffect(SF_Drone_PointLight_MG_EW);
  %spell_data.addCastingEffect(SF_Drone_PointLightWhite_MG_EW);
  %spell_data.addCastingEffect(SF_Tele_IN_Light_MG_EW);
  %spell_data.addCastingEffect(SF_Tele_ColoredLight_IN_MG_EW);
  /*
  %spell_data.addCastingEffect(SF_Tele_Flare1_IN_MG_EW);
  %spell_data.addCastingEffect(SF_Tele_Flare2_IN_MG_EW);
  %spell_data.addCastingEffect(SF_Tele_Flare3_IN_MG_EW);
  */
  %spell_data.addCastingEffect(SF_Tele_OUT_Light_MG_EW);
  %spell_data.addCastingEffect(SF_Tele_ColoredLight_OUT_MG_EW);
  /*
  %spell_data.addCastingEffect(SF_Tele_Flare1_OUT_MG_EW);
  %spell_data.addCastingEffect(SF_Tele_Flare2_OUT_MG_EW);
  %spell_data.addCastingEffect(SF_Tele_Flare3_OUT_MG_EW);
  */
  /*
  %spell_data.addDeliveryEffect(SF_LaserPulseFlash_LF_1_EW);
  %spell_data.addDeliveryEffect(SF_LaserPulseFlash_RT_1_EW);
  %spell_data.addDeliveryEffect(SF_LaserPulseFlash_LF_2_EW);
  %spell_data.addDeliveryEffect(SF_LaserPulseFlash_RT_2_EW);
  %spell_data.addDeliveryEffect(SF_LaserPulseFlash_LF_3_EW);
  %spell_data.addDeliveryEffect(SF_LaserPulseFlash_RT_3_EW);
  */
}

// add effects to main spell datablock
function OL_redux_add_Lighting_FX(%spell_data)
{
  %spell_data.addCastingEffect(SF_Drone_PointLight_S_EW);
  %spell_data.addCastingEffect(SF_Drone_PointLightWhite_S_EW);
  %spell_data.addCastingEffect(SF_Tele_IN_Light_S_EW);
  %spell_data.addCastingEffect(SF_Tele_ColoredLight_IN_S_EW);
  /*
  %spell_data.addCastingEffect(SF_Tele_Flare1_IN_S_EW);
  %spell_data.addCastingEffect(SF_Tele_Flare2_IN_S_EW);
  %spell_data.addCastingEffect(SF_Tele_Flare3_IN_S_EW);
  */
  %spell_data.addCastingEffect(SF_Tele_OUT_Light_S_EW);
  %spell_data.addCastingEffect(SF_Tele_ColoredLight_OUT_S_EW);
  /*
  %spell_data.addCastingEffect(SF_Tele_Flare1_OUT_S_EW);
  %spell_data.addCastingEffect(SF_Tele_Flare2_OUT_S_EW);
  %spell_data.addCastingEffect(SF_Tele_Flare3_OUT_S_EW);
  */
  %spell_data.addDeliveryEffect(SF_Beam_Blast_Light_S_EW);
  %spell_data.addImpactEffect(SF_Beam_Impact_Light_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
