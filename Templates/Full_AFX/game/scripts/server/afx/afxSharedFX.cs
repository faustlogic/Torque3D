
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// Shared Effects Elements
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

// Shared Spell Audio Descriptions

datablock SFXDescription(SpellAudioDefault_AD)
{
  volume             = 1.0;
  isLooping          = false;
  is3D               = true;
  ReferenceDistance  = 20.0;
  MaxDistance        = 100.0;
  channel            = $SimAudioType;
};

datablock SFXDescription(SpellAudioLoop_AD : SpellAudioDefault_AD)
{
  isLooping= true;
};

// good for casting sounds near spellecaster //

datablock SFXDescription(SpellAudioCasting_soft_AD : SpellAudioDefault_AD)
{
  ReferenceDistance= 10.0;
  MaxDistance = 30;
};

datablock SFXDescription(SpellAudioCasting_AD : SpellAudioDefault_AD)
{
  ReferenceDistance= 20.0;
  MaxDistance = 55;
};

datablock SFXDescription(SpellAudioCasting_loud_AD : SpellAudioDefault_AD)
{
  ReferenceDistance= 30.0;
  MaxDistance = 80;
};

// good for impacts //

datablock SFXDescription(SpellAudioImpact_AD : SpellAudioDefault_AD)
{
  ReferenceDistance= 25.0;
  MaxDistance= 120.0;
};

// good for projectiles //

datablock SFXDescription(SpellAudioMissileLoop_AD : SpellAudioDefault_AD)
{
  isLooping= true;
  ReferenceDistance= 10.0;
};

datablock SFXDescription(SpellAudioMissileLoop_loud_AD : SpellAudioDefault_AD)
{
  isLooping= true;
  ReferenceDistance= 25.0;
};

// good for shockwaves //

datablock SFXDescription(SpellAudioShockwaveLoop_AD : SpellAudioDefault_AD)
{
  isLooping= true;
  ReferenceDistance= 35.0;
  MaxDistance= 70.0;
};

datablock SFXDescription(SpellAudioShockwaveLoop_soft_AD : SpellAudioDefault_AD)
{
  isLooping= true;
  ReferenceDistance= 8.0;
  MaxDistance= 25.0;
};

datablock afxXM_SpinData(SHARED_MainZodeRevealLight_spin1_XM)
{
  spinAxis = "0 0 1";
  spinAngle = 0;
  spinRate = -30;
};

// Shared Freeze X-Mod. Usually used to stick zodiacs to the
// ground.
datablock afxXM_FreezeData(SHARED_freeze_XM)
{
  mask = $afxXfmMod::POS;
};

// Shared AltitudeConform X-Mod. Used to measure separate altitudes
// of zodiacs over terrain and interiors. 
datablock afxXM_AltitudeConformData(SHARED_AltitudeConform_XM)
{
  height = 0.0;
};

datablock afxXM_AltitudeConformData(SHARED_freeze_AltitudeConform_XM)
{
  height = 0.0;
  freeze = true;
};

datablock afxZodiacData(SHARED_SelectronZodiac_CE)
{  
  altitudeFalloff = 1.0;
  altitudeMax = 2.5;
  altitudeFades = true;
  verticalRange = "1.0 1.0";
  scaleVerticalRange = false;
  gradientRange = "0.0 45.0";
  useGradientRange = true;
};

datablock afxZodiacData(SHARED_ZodiacBase_CE)
{  
  altitudeFalloff = 1.0;
  altitudeMax = 2.5;
  altitudeFades = true;
  verticalRange = "1.0 1.0";
  scaleVerticalRange = false;
  gradientRange = "0.0 45.0";
  useGradientRange = true;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
