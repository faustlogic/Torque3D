
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// FLAME BROIL (audio)
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

// casting sounds //

datablock SFXProfile(FB_ZodeSnd_CE)
{
   fileName = %mySpellDataPath @ "/FB/sounds/fb_zodiac.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(FB_ZodeSnd_EW)
{
  effect = FB_ZodeSnd_CE;
  constraint = "caster";
  delay = 0;
  lifetime = 1.907;
};

datablock SFXProfile(FB_ConjureSnd_CE)
{
   fileName = %mySpellDataPath @ "/FB/sounds/fb_conjure.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(FB_ConjureSnd_EW)
{
  effect = FB_ConjureSnd_CE;
  constraint = "caster";
  delay = 0.6;
  lifetime = 1.907;
};

datablock SFXProfile(FB_FireRingSnd_CE)
{
   fileName = %mySpellDataPath @ "/FB/sounds/fb_firering.ogg";
   description = SpellAudioCasting_AD;
   preload = true;
};
datablock afxEffectWrapperData(FB_FireRingSnd_EW)
{
  effect = FB_FireRingSnd_CE;
  constraint = "caster";
  delay = 1.33;
  lifetime = 2.296;
};

// impact sounds //

datablock SFXProfile(FB_Impact1ASnd_CE)
{
   fileName = %mySpellDataPath @ "/FB/sounds/fb_impact1a.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock afxEffectWrapperData(FB_Impact1ASnd_EW)
{
  effect = FB_Impact1ASnd_CE;
  constraint = "impactedObject";
  delay = 0;
  lifetime = 2.417;
};

datablock SFXProfile(FB_Impact1BSnd_CE)
{
   fileName = %mySpellDataPath @ "/FB/sounds/fb_impact1b.ogg";
   description = SpellAudioImpact_AD;
   preload = true;
};
datablock afxEffectWrapperData(FB_Impact1BSnd_EW)
{
  effect = FB_Impact1BSnd_CE;
  posConstraint = "impactPoint";
  delay = 0;
  lifetime = 2.191;
  execConditions[0] = $AFX::IMPACTED_SOMETHING;
};

// fireball sounds //

datablock SFXProfile(FB_FireBallSnd_CE)
{
   fileName = %mySpellDataPath @ "/FB/sounds/projectile_loopFire1a_SR.ogg";
   description = SpellAudioMissileLoop_AD;
   preload = true;
};
datablock afxEffectWrapperData(FB_FireBallSnd_EW)
{
  effect = FB_FireBallSnd_CE;
  posConstraint = "caster.Bip01 R Hand";  
  delay = 1.8;
  lifetime = 1.5;
  fadeInTime = 0.5;
  fadeOutTime = 0.5;
};

datablock SFXProfile(FB_FireCrackle_CE)
{
   fileName = %mySpellDataPath @ "/FB/sounds/fireloop.ogg";
   description = SpellAudioLoop_AD;
   preload = true;
};

datablock afxEffectWrapperData(FB_FireCrackle1_EW)
{
  effect = FB_FireCrackle_CE;
  constraint = "caster";
  delay = 1.33;
  lifetime = 1.35;
  fadeInTime = 0.6;
  fadeOutTime = 1.25;
  scaleFactor = 0.4;
  xfmModifiers[0] = "FB_FireRing_spin6_XM";
  xfmModifiers[1] = "FB_FireRing_offset_XM";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

function FB_add_Audio_FX(%spell_data)
{
  %spell_data.addCastingEffect(FB_ZodeSnd_EW);
  %spell_data.addCastingEffect(FB_ConjureSnd_EW);
  %spell_data.addCastingEffect(FB_FireRingSnd_EW);
  %spell_data.addCastingEffect(FB_FireBallSnd_EW);
  %spell_data.addCastingEffect(FB_FireCrackle1_EW);
  %spell_data.addImpactEffect(FB_Impact1BSnd_EW);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
