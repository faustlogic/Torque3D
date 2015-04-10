
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// INSECTOPLASM SPELL
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
$MIN_REQUIRED_VERSION = 1.12;

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
$spell_reload = isObject(InsectoplasmSpell);
if ($spell_reload)
{
  // mark datablocks so we can detect which are reloaded this script
  markDataBlocks();
  // reset data path from previously saved value
  %mySpellDataPath = InsectoplasmSpell.spellDataPath;
}
else
{
  // set data path from default plus containing folder name
  %mySpellDataPath = $afxSpellDataPath @ "/" @ $afxAutoloadScriptFolder;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

$BrightLighting_mask = 0x800000; // BIT(23);


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// SPELLCASTER ANIMATION

//
// The spellcaster begins the summoning of dark insects from the
// netherworld by standing straight and raising his arms.  As the
// bugs appear, they near, and bite their way inside him!  The orc's
// body pulsates with their movement beneath his skin.  And he begins
// to hover and spin, faster and faster as the bugs coalesce inside
// him, merging, terrifying!  At the height of the spin the orc
// contorts in three powerful heaves -- a giant centipede shooting
// out from within his body in a shower of slime!
//
// Initially the orc's feet remain planted near the origin so that
// all the bugs can easily "enter" him there.  The pulsations are
// done by animating scales on the orc's bones in Maya, then
// exporting with the proper scale flags set on the sequence node.
//

datablock afxAnimClipData(IOP_Casting_Clip_CE)
{
  clipName = "iop";
  ignoreCorpse = true;
  rate = 1.0;
};
//
datablock afxEffectWrapperData(IOP_Casting_Clip_EW)
{
  effect = IOP_Casting_Clip_CE;
  constraint = "caster";
  lifetime = 510/30;
  delay = 0.0;
};

datablock afxAnimLockData(IOP_AnimLock_CE)
{
  priority = 0;
};
//
datablock afxEffectWrapperData(IOP_AnimLock_EW)
{
  effect = IOP_AnimLock_CE;
  constraint = caster;
  delay = 8.0;
  lifetime = 9.0;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING ZODIACS

// this mooring anchors the casting zodiacs and measures
// their altitudes

datablock afxMooringData(IOP_CastingZodeMooring_CE)
{
  networking = $AFX::CLIENT_ONLY;
  displayAxisMarker = false;
};
datablock afxEffectWrapperData(IOP_CastingZodeMooring_EW)
{
  effect = IOP_CastingZodeMooring_CE;
  posConstraint = caster;
  effectName = "CastingZodeMooring";
  isConstraintSrc = true;
  lifetime = 18;
  xfmModifiers[0] = SHARED_freeze_AltitudeConform_XM;
};

//
// This spell uses multiple zodiacs during casting for a more complex
// effect.  As in most of the other spells, the main casting zodiac
// is formed by two zodiacs plus a white reveal glow, used when the
// casting begins.  Additionally, a subtractive eye is used at the
// center, and within the outer ring a cloud forms -- a portal
// through which the bugs come from the other world.  The main intent
// is to suggest the giant centipede's face (the monster projectile)
// while also providing a point of entry for the bugs.  Note that
// the face is completed when the eye glow zodiac kicks in later
// (see "IOP_EyeGlowZodiac_EW").
//
// When using complex overlapping zodiacs like this, the compositing
// order is important.  This is controlled by the order the zodiac
// effect wrappers are added to the phrase coreographer: those added
// later in the sequence are compositied over those added previously.
//

// white reveal glow
datablock afxZodiacData(IOP_CastingZode_Reveal_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/IOP/zodiacs/IOP_caster_reveal";
  radius = 3.0;
  startAngle = 7.5; //0.0+7.5
  rotationRate = -30.0;  
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(IOP_CastingZode_Reveal_EW)
{
  effect = IOP_CastingZode_Reveal_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = 0.0;
  lifetime = 0.75;
  fadeInTime = 0.5;
  fadeOutTime = 0.25;
};

// main casting zodiac
datablock afxZodiacData(IOP_CastingZode_Main_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/IOP/zodiacs/IOP_caster";
  radius = 3.0;
  startAngle = 0.0;
  rotationRate = -30.0;
  color = "1.0 1.0 1.0 0.9";
  blend = additive;
};
//
datablock afxEffectWrapperData(IOP_CastingZode_Main_EW)
{
  effect = IOP_CastingZode_Main_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = 0.25;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 19.5-4.0;
};

// Main Zode Underglow
//  Glowing zodiacs blended additively tend to be washed-out atop
//  light groundplanes, as is the case with TLK and the sand-colored
//  ground in the demo.  To make them visually "pop" the ground must
//  be darkened, and this is done here.  The "underglow" zodiac is
//  a copy of the glow zodiac that is blended normally.  Because the
//  glow zodiacs have halos extending beyond their opaque regions
//  that blend with black, the ground is subtly darkened.  As the
//  glow is layered atop it -- it pops!  Increasing the color value
//  increases the effect.
datablock afxZodiacData(IOP_CastingZode_Main_underglow_CE : IOP_CastingZode_Main_CE)
{
  color = "0.7 0.7 0.7 0.7";
  blend = normal;
};
//
datablock afxEffectWrapperData(IOP_CastingZode_Main_underglow_EW : IOP_CastingZode_Main_EW)
{
  effect = IOP_CastingZode_Main_underglow_CE;
  execConditions = $BrightLighting_mask;
};

// text detail zodiac: inner ring of runes, outer ring of skulls
datablock afxZodiacData(IOP_CastingZode_Text_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/IOP/zodiacs/zode_text";
  radius = 3.0;
  startAngle = 0.0;
  rotationRate = 20.0;
  color = "0.5 0.0 0.9 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(IOP_CastingZode_Text_EW)
{
  effect = IOP_CastingZode_Text_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = 0.25;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 19.5-4.0;
};

// Runes & Skulls Zode Underglow
//  Here the zode is made white but only slightly opaque to subtly
//  lighten the ground; this seems to be matching the non-TLK look
//  a tiny bit better...
datablock afxZodiacData(IOP_CastingZode_Text_underglow_CE : IOP_CastingZode_Text_CE)
{
  color = "1 1 1 0.10";
  blend = normal;
};
//
datablock afxEffectWrapperData(IOP_CastingZode_Text_underglow_EW : IOP_CastingZode_Text_EW)
{
  effect = IOP_CastingZode_Text_underglow_CE;
  execConditions = $BrightLighting_mask;
};

// center eye zodiac, subtractive
datablock afxZodiacData(IOP_CastingZode_Eye_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/IOP/zodiacs/IOP_casterEye";
  radius = 1.10; //0.85;
  startAngle = 0.0;
  rotationRate = -30.0;

  //color = "0.45 0.45 0.0 0.45";
  color = "0.9 0.9 0.7 0.9";
  //color = "0.7 0.7 0.4 0.7";

  blend = subtractive;
};
//
datablock afxEffectWrapperData(IOP_CastingZode_Eye_EW)
{
  effect = IOP_CastingZode_Eye_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = 0.25;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = 19.5-4.0;
};

// black outer ring
datablock afxZodiacData(IOP_CastingZode_DarkBand_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/IOP/zodiacs/IOP_darkBand";
  radius = 2.12;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 1.0";
  blend = normal;
};
//
datablock afxEffectWrapperData(IOP_CastingZode_DarkBand_EW)
{
  effect = IOP_CastingZode_DarkBand_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = 1.0;
  fadeInTime = 0.5;
  fadeOutTime = 1.75;
  lifetime = 5.5;
};

// cloud outer ring A, layered over black ring
datablock afxZodiacData(IOP_CastingZode_CloudBandA_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/IOP/zodiacs/IOP_cloudBand";
  radius = 2.12;
  startAngle = 0.0;
  rotationRate = -70.0;
  color = "0.35 0.35 0.35 0.75";
  blend = normal;
};
//
datablock afxEffectWrapperData(IOP_CastingZode_CloudBandA_EW)
{
  effect = IOP_CastingZode_CloudBandA_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = 1.0;
  fadeInTime = 0.5;
  fadeOutTime = 1.75;
  lifetime = 5.5;
};

// cloud outer ring B, layered over black ring
datablock afxZodiacData(IOP_CastingZode_CloudBandB_CE : IOP_CastingZode_CloudBandA_CE)
{  
  startAngle = 123.0;
  rotationRate = 70.0;
};
//
datablock afxEffectWrapperData(IOP_CastingZode_CloudBandB_EW : IOP_CastingZode_CloudBandA_EW)
{
  effect = IOP_CastingZode_CloudBandB_CE;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// BUGS

//
// During casting, twenty bugs emerge from the cloud band zodiac, an
// opening into the dark world below.  Each bug is hunting for flesh,
// and as it squirms it finds it in the legs of dear orcy.
//
// The bug's movement is controlled by means of paths, one path
// unique to each bug; the paths were drawn in Maya and their control
// point positions exported as text.  Additionally the bugs conform
// to the ground using a ground conform modifier, with the
// "conformOrientation" flag set so that the bugs orient correctly.
//
// The orc stands with his feet near his origin.  This is done to
// simplify the effect: while it would have been possible to have the
// bugs enter his feet in a more normal, standing position, this 
// would have required the bugs being fully constrained to the orc,
// and thus when the orc's orientation changed due to camera movement
// the bugs would have slid strangely across the ground; the orc's
// simplified standing position and only a posConstraint solve this.
//

// bug model
datablock afxModelData(IOP_Bug_CE)
{
  shapeFile = %mySpellDataPath @ "/IOP/models/IOP_bug.dts";
  sequence = "squirm";
  sequenceRate = 3.0;
};

// bug paths, one per bug:
datablock afxPathData(IOP_Bug1_Path)
{
  points = "-1.825065786 -0.9171021926 0" SPC
           "-3.133970111 -0.7350543754 0" SPC
           "-2.76235053 -1.871722426 0" SPC
           "-4.036990646 -2.535554019 0" SPC
           "-2.862804875 -3.992038567 0" SPC
           "-1.408048774 -3.293739381 0" SPC
           "-2.34675689 -1.979649711 0" SPC
           "-0.9489892169 -1.344998675 0" SPC
           "-0.8207936389 -0.3843806662 0" SPC
           "-0.1745072011 -0.0652413412 0";
  roll = "0 0 0 0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug2_Path)
{
  points = "-0.6402115012 2.004316493 0" SPC
           "-1.287203307 2.492810749 0" SPC
           "-0.4865561899 3.239583187 0" SPC
           "-0.5301363439 4.528724559 0" SPC
           "-0.1777204525 3.414475721 0" SPC
           "0.7061598022 3.096298656 0" SPC
           "1.435850955 2.281429844 0" SPC
           "0.14121917 2.037525867 0" SPC
           "0.673064812 1.135878026 0" SPC
           "-0.1081752198 0.4899538001 0" SPC
           "0.08030551639 0.04239068174 0";
  roll = "0 0 0 0 0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug3_Path)
{
  points = "1.735917283 -1.049954565 0" SPC
           "1.050797608 -0.9728981425 0" SPC
           "1.35025389 -0.3608314645 0" SPC
           "2.697883413 -0.3256013137 0" SPC
           "2.015318306 0.01636754162 0" SPC
           "2.564245233 0.5904207344 0" SPC
           "1.217988313 0.6908876687 0" SPC
           "0.1606790638 0.00220390802 0";
  roll = "0 0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug4_Path)
{
  points = "-0.3014072589 -2.004905167 0" SPC
           "-0.8353499337 -1.235713542 0" SPC
           "-1.438837841 -2.017334853 0" SPC
           "-1.927484608 -2.827933344 0" SPC
           "-2.86096922 -2.222653426 0" SPC
           "-2.439008096 -0.7960229591 0" SPC
           "-1.328000127 -0.7417670017 0" SPC
           "-0.211660135 -0.09098175124 0";
  roll = "0 0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug5_Path)
{
  points = "-0.0692085379 2.009234988 0" SPC
           "-0.6100904848 1.443379287 0" SPC
           "-1.664592952 1.204508189 0" SPC
           "-2.342582716 2.07092588 0" SPC
           "-0.9218239402 1.915592943 0" SPC
           "-1.33349054 0.7701173801 0" SPC
           "-2.197887453 0.932008311 0" SPC
           "-2.077327132 0.1885529972 0" SPC
           "-1.414245366 -0.5348089299 0" SPC
           "-0.6506966651 -0.6955560248 0" SPC
           "-0.1111932007 -0.03070159066 0";
  roll = "0 0 0 0 0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug6_Path)
{
  points = "-1.979180624 0.4978324743 0" SPC
           "-3.17107974 1.615307457 0" SPC
           "-1.825392594 2.724340633 0" SPC
           "-1.067006178 0.8245450402 0" SPC
           "1.439015569 2.318452004 0" SPC
           "1.961443627 1.012381858 0" SPC
           "1.358642021 0.05302946845 0" SPC
           "0.4343462254 0.4296736395 0" SPC
           "-0.02780167238 0.2689265446 0" SPC
           "-6.803106066e-005 -0.09826302629 0";
  roll = "0 0 0 0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug7_Path)
{
  points = "-0.6286489242 -1.962782646 0" SPC
           "-1.060748629 -2.859167639 0" SPC
           "0.5736559461 -2.768289439 0" SPC
           "0.6219656882 -3.805877986 0" SPC
           "-0.8431414929 -2.979606182 0" SPC
           "0.1128520357 -2.423587295 0" SPC
           "-0.6671256381 -0.9628774103 0" SPC
           "-6.803106066e-005 -0.05807625256 0";
  roll = "0 0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug8_Path)
{
  points = "-2.042025506 0.04887646614 0" SPC
           "-3.432744717 -0.7408729778 0" SPC
           "-1.694208794 -1.25719368 0" SPC
           "-1.303639851 -0.07779121137 0" SPC
           "-2.148844828 1.163601477 0" SPC
           "-0.9160636966 1.266022766 0" SPC
           "-0.1714733613 0.04967195679 0";
  roll = "0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug9_Path)
{
  points = "-1.910167867 -0.7425217795 0" SPC
           "0.6393105802 -1.527446596 0" SPC
           "2.660986495 -1.589742369 0" SPC
           "3.348864497 -0.3995909535 0" SPC
           "2.26284443 1.233409114 0" SPC
           "1.177801539 1.012381858 0" SPC
           "0.8161205758 -0.1932213532 0" SPC
           "0.0200253558 -0.1183564131 0";
  roll = "0 0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug10_Path)
{
  points = "1.314852092 -1.55370786 0" SPC
           "0.4700744445 -2.553308281 0" SPC
           "2.41870564 -3.026083348 0" SPC
           "0.7725136825 -3.760316739 0" SPC
           "-0.4095760227 -2.142279879 0" SPC
           "0.3718674166 -0.9952467679 0" SPC
           "-0.1312865876 0.02957856993 0";
  roll = "0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug11_Path)
{
  points = "-2.022106135 -0.3137694231 0" SPC
           "-2.20015757 -0.6434943015 0" SPC
           "-2.061673121 -1.26997157 0" SPC
           "-1.072498485 -1.098514634 0" SPC
           "-1.118659968 -0.4786318623 0" SPC
           "-1.54730231 0.0555224407 0" SPC
           "-1.052714993 0.6226492315 0" SPC
           "-0.2613752845 0.5369207632 0" SPC
           "-0.0635403575 -0.003828037417 0";
  roll = "0 0 0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug12_Path)
{
  points = "-0.3075367675 2.020682716 0" SPC
           "-0.7493681046 1.625012862 0" SPC
           "-0.9933645146 0.9194016221 0" SPC
           "-1.178010447 0.4445977972 0" SPC
           "-0.8878525535 0.325896841 0" SPC
           "0.2398065306 0.5896767437 0" SPC
           "0.3980744723 0.3654638264 0" SPC
           "0.127700072 -0.04339502282 0";
  roll = "0 0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug13_Path)
{
  points = "-0.6174781532 -1.94920482 0" SPC
           "-0.9142305438 -1.955799318 0" SPC
           "-1.151632456 -1.375483532 0" SPC
           "-0.5647221727 -0.9732191799 0" SPC
           "0.2200230379 -1.085325639 0" SPC
           "0.7409883458 -0.7094392772 0" SPC
           "0.1342945695 -0.04339502282 0";
  roll = "0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug14_Path)
{
  points = "2.013726376 0.1676288994 0" SPC
           "1.492761069 0.4182198069 0" SPC
           "1.301520639 0.7743226756 0" SPC
           "1.670812503 0.9128071245 0" SPC
           "1.763135469 0.6226492315 0" SPC
           "1.558706044 0.2269793775 0" SPC
           "0.8003388239 -0.0697730131 0" SPC
           "0.3519129893 0.4380032996 0" SPC
           "-0.0833238502 -0.01042253498 0";
  roll = "0 0 0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug15_Path)
{
  points = "0.5959093993 -1.942610322 0" SPC
           "0.3057515063 -1.738180898 0" SPC
           "0.325534999 -1.494184488 0" SPC
           "0.6222873896 -1.164459609 0" SPC
           "0.6750433701 -0.544576838 0" SPC
           "0.4244524625 -0.4324703793 0" SPC
           "0.1408890671 -0.6896557845 0" SPC
           "-0.1756468162 -0.6171163112 0" SPC
           "-0.208619304 -0.0697730131 0";
  roll = "0 0 0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug16_Path)
{
  points = "-1.936377667 0.6094602364 0" SPC
           "-1.501140827 0.7413501878 0" SPC
           "-1.210982934 0.5435152607 0" SPC
           "-1.454979344 -0.02361153012 0" SPC
           "-1.877027189 0.04892794313 0" SPC
           "-1.76492073 0.4511922948 0" SPC
           "-1.184604944 0.4511922948 0" SPC
           "-0.9142305438 -0.1027455009 0" SPC
           "-0.7955295876 -0.4456593745 0" SPC
           "-0.452615714 -0.5379823404 0" SPC
           "-0.05035136236 -0.07636751067 0";
  roll = "0 0 0 0 0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug17_Path)
{
  points = "1.545517049 -1.329322049 0" SPC
           "1.228981166 -1.204026595 0" SPC
           "1.103685712 -0.7094392772 0" SPC
           "0.9256342777 -0.2807969353 0" SPC
           "0.6025038969 -0.1423124863 0" SPC
           "0.3585074869 -0.6105218137 0" SPC
           "0.02218811089 -0.6698722918 0" SPC
           "0.1145110768 -0.1093399985 0";
  roll = "0 0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug18_Path)
{
  points = "1.301520639 1.565662384 0" SPC
           "1.182819683 1.809658794 0" SPC
           "0.8069333215 2.033871711 0" SPC
           "0.5761259066 1.730524823 0" SPC
           "0.4903974382 1.229343008 0" SPC
           "0.6948268628 0.813889661 0" SPC
           "0.371696482 0.6094602364 0" SPC
           "-0.2020248064 0.6754052121 0" SPC
           "-0.406454231 0.3918418167 0" SPC
           "-0.5119661921 0.06211693826 0" SPC
           "-0.1492688259 -0.06317851553 0";
  roll = "0 0 0 0 0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug19_Path)
{
  points = "0.9256342777 -1.810720371 0" SPC
           "0.4969919358 -1.606290946 0" SPC
           "0.06834959386 -1.118298126 0" SPC
           "0.5167754285 -0.7028447796 0" SPC
           "0.9915792534 -0.2742024377 0" SPC
           "0.661854375 -0.03680052526 0" SPC
           "0.1079165793 -0.04339502282 0";
  roll = "0 0 0 0 0 0 0";
};
datablock afxPathData(IOP_Bug20_Path)
{
  points = "-1.527518818 -1.335916546 0" SPC
           "-1.501140827 -0.8083567407 0" SPC
           "-0.9208250413 -0.2478244474 0" SPC
           "-0.5779111678 -0.01701703255 0" SPC
           "-0.5976946605 0.6358382267 0" SPC
           "-1.105470973 0.6951887048 0" SPC
           "-0.8746635584 0.0950894261 0" SPC
           "-0.1558633235 -0.02361153012 0";
  roll = "0 0 0 0 0 0 0 0";
};

// bug path modifiers, one per path:
datablock afxXM_PathConformData(IOP_Bug1_path_XM)
{
  paths = "IOP_Bug1_Path";
  orientToPath = true;
};
datablock afxXM_PathConformData(IOP_Bug2_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug2_Path";
};
datablock afxXM_PathConformData(IOP_Bug3_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug3_Path";
};
datablock afxXM_PathConformData(IOP_Bug4_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug4_Path";
};
datablock afxXM_PathConformData(IOP_Bug5_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug5_Path";
};
datablock afxXM_PathConformData(IOP_Bug6_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug6_Path";
};
datablock afxXM_PathConformData(IOP_Bug7_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug7_Path";
};
datablock afxXM_PathConformData(IOP_Bug8_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug8_Path";
};
datablock afxXM_PathConformData(IOP_Bug9_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug9_Path";
};
datablock afxXM_PathConformData(IOP_Bug10_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug10_Path";
};
datablock afxXM_PathConformData(IOP_Bug11_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug11_Path";
};
datablock afxXM_PathConformData(IOP_Bug12_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug12_Path";
};
datablock afxXM_PathConformData(IOP_Bug13_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug13_Path";
};
datablock afxXM_PathConformData(IOP_Bug14_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug14_Path";
};
datablock afxXM_PathConformData(IOP_Bug15_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug15_Path";
};
datablock afxXM_PathConformData(IOP_Bug16_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug16_Path";
};
datablock afxXM_PathConformData(IOP_Bug17_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug17_Path";
};
datablock afxXM_PathConformData(IOP_Bug18_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug18_Path";
};
datablock afxXM_PathConformData(IOP_Bug19_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug19_Path";
};
datablock afxXM_PathConformData(IOP_Bug20_path_XM : IOP_Bug1_path_XM)
{
  paths = "IOP_Bug20_Path";
};

// bug ground transform, with orientation
datablock afxXM_GroundConformData(IOP_Bugs_Ground)
{
  height = 0.0;
  conformOrientation = true;
};

// BUG GLOBALS
//
//  Varying Scales
%IOP_Bug_Scale_A = 1.2;
%IOP_Bug_Scale_B = 1.5;
%IOP_Bug_Scale_C = 0.9;
//
//  Varying Lifetimes
%IOP_Bug_Lifetime_A = 4.0-0.75;
%IOP_Bug_Lifetime_B = 5.0-0.75;
%IOP_Bug_Lifetime_C = 3.0-0.75;
%IOP_Bug_Lifetime_D = 2.0-0.75;
//
//  Varying Delays (seperated into 3 "waves" of bugs)
%IOP_Bug_Delay_Wave1_delta = 0.5;
%IOP_Bug_Delay_Wave2_delta = 0.3;
%IOP_Bug_Delay_Wave3_delta = 0.1;
//   Wave 1
%IOP_Bug_Delay_1  = 1.5 - 1.0 +(%IOP_Bug_Delay_Wave1_delta*0);
%IOP_Bug_Delay_2  = 1.5 - 1.0 +(%IOP_Bug_Delay_Wave1_delta*1);
%IOP_Bug_Delay_3  = 1.5 - 1.0 +(%IOP_Bug_Delay_Wave1_delta*2);
%IOP_Bug_Delay_4  = 1.5 - 1.0 +(%IOP_Bug_Delay_Wave1_delta*3);
%IOP_Bug_Delay_5  = 1.5 - 1.0 +(%IOP_Bug_Delay_Wave1_delta*4);
//   Wave 2
%IOP_Bug_Delay_6  = 3.5 - 1.0 +(%IOP_Bug_Delay_Wave2_delta*0);
%IOP_Bug_Delay_7  = 3.5 - 1.0 +(%IOP_Bug_Delay_Wave2_delta*1);
%IOP_Bug_Delay_8  = 3.5 - 1.0 +(%IOP_Bug_Delay_Wave2_delta*2);
%IOP_Bug_Delay_9  = 3.5 - 1.0 +(%IOP_Bug_Delay_Wave2_delta*3);
%IOP_Bug_Delay_10 = 3.5 - 1.0 +(%IOP_Bug_Delay_Wave2_delta*4);
//   Wave 3
%IOP_Bug_Delay_11 = 5.0 - 1.0 +(%IOP_Bug_Delay_Wave3_delta*0);
%IOP_Bug_Delay_12 = 5.0 - 1.0 +(%IOP_Bug_Delay_Wave3_delta*1);
%IOP_Bug_Delay_13 = 5.0 - 1.0 +(%IOP_Bug_Delay_Wave3_delta*2);
%IOP_Bug_Delay_14 = 5.0 - 1.0 +(%IOP_Bug_Delay_Wave3_delta*3);
%IOP_Bug_Delay_15 = 5.0 - 1.0 +(%IOP_Bug_Delay_Wave3_delta*4);
%IOP_Bug_Delay_16 = 5.0 - 1.0 +(%IOP_Bug_Delay_Wave3_delta*5);
%IOP_Bug_Delay_17 = 5.0 - 1.0 +(%IOP_Bug_Delay_Wave3_delta*6);
%IOP_Bug_Delay_18 = 5.0 - 1.0 +(%IOP_Bug_Delay_Wave3_delta*7);
%IOP_Bug_Delay_19 = 5.0 - 1.0 +(%IOP_Bug_Delay_Wave3_delta*8);
%IOP_Bug_Delay_20 = 5.0 - 1.0 +(%IOP_Bug_Delay_Wave3_delta*9);

// bug 1
datablock afxEffectWrapperData(IOP_Bug1_EW)
{
  effect = IOP_Bug_CE;
  posConstraint = caster;

  delay       = %IOP_Bug_Delay_1;
  fadeInTime  = 0.2;
  fadeOutTime = 0.2;
  lifetime    = %IOP_Bug_Lifetime_B;
  scaleFactor = %IOP_Bug_Scale_A;

  xfmModifiers[0] = IOP_Bug1_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 2
datablock afxEffectWrapperData(IOP_Bug2_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_2;
  lifetime    = %IOP_Bug_Lifetime_A;
  scaleFactor = %IOP_Bug_Scale_B;

  xfmModifiers[0] = IOP_Bug2_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 3
datablock afxEffectWrapperData(IOP_Bug3_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_3;
  lifetime    = %IOP_Bug_Lifetime_C;
  scaleFactor = %IOP_Bug_Scale_B;

  xfmModifiers[0] = IOP_Bug3_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 4
datablock afxEffectWrapperData(IOP_Bug4_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_4;
  lifetime    = %IOP_Bug_Lifetime_A;
  scaleFactor = %IOP_Bug_Scale_A;

  xfmModifiers[0] = IOP_Bug4_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 5
datablock afxEffectWrapperData(IOP_Bug5_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_5;
  lifetime    = %IOP_Bug_Lifetime_B;
  scaleFactor = %IOP_Bug_Scale_C;

  xfmModifiers[0] = IOP_Bug5_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 6
datablock afxEffectWrapperData(IOP_Bug6_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_6;
  lifetime    = %IOP_Bug_Lifetime_C;
  scaleFactor = %IOP_Bug_Scale_A;

  xfmModifiers[0] = IOP_Bug6_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 7
datablock afxEffectWrapperData(IOP_Bug7_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_7;
  lifetime    = %IOP_Bug_Lifetime_A;
  scaleFactor = %IOP_Bug_Scale_B;

  xfmModifiers[0] = IOP_Bug7_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 8
datablock afxEffectWrapperData(IOP_Bug8_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_8;
  lifetime    = %IOP_Bug_Lifetime_C;
  scaleFactor = %IOP_Bug_Scale_B;

  xfmModifiers[0] = IOP_Bug8_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 9
datablock afxEffectWrapperData(IOP_Bug9_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_9;
  lifetime    = %IOP_Bug_Lifetime_B;
  scaleFactor = %IOP_Bug_Scale_B;

  xfmModifiers[0] = IOP_Bug9_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 10
datablock afxEffectWrapperData(IOP_Bug10_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_10;
  lifetime    = %IOP_Bug_Lifetime_B;
  scaleFactor = %IOP_Bug_Scale_A;

  xfmModifiers[0] = IOP_Bug10_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 11
datablock afxEffectWrapperData(IOP_Bug11_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_11;
  lifetime    = %IOP_Bug_Lifetime_A;
  scaleFactor = %IOP_Bug_Scale_A;

  xfmModifiers[0] = IOP_Bug11_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 12
datablock afxEffectWrapperData(IOP_Bug12_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_12;
  lifetime    = %IOP_Bug_Lifetime_C;
  scaleFactor = %IOP_Bug_Scale_B;

  xfmModifiers[0] = IOP_Bug12_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 13
datablock afxEffectWrapperData(IOP_Bug13_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_13;
  lifetime    = %IOP_Bug_Lifetime_A;
  scaleFactor = %IOP_Bug_Scale_B;

  xfmModifiers[0] = IOP_Bug13_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 14
datablock afxEffectWrapperData(IOP_Bug14_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_14;
  lifetime    = %IOP_Bug_Lifetime_C;
  scaleFactor = %IOP_Bug_Scale_C;

  xfmModifiers[0] = IOP_Bug14_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 15
datablock afxEffectWrapperData(IOP_Bug15_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_15;
  lifetime    = %IOP_Bug_Lifetime_A;
  scaleFactor = %IOP_Bug_Scale_C;

  xfmModifiers[0] = IOP_Bug15_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 16
datablock afxEffectWrapperData(IOP_Bug16_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_16;
  lifetime    = %IOP_Bug_Lifetime_C;
  scaleFactor = %IOP_Bug_Scale_A;

  xfmModifiers[0] = IOP_Bug16_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 17
datablock afxEffectWrapperData(IOP_Bug17_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_17;
  lifetime    = %IOP_Bug_Lifetime_D;
  scaleFactor = %IOP_Bug_Scale_B;

  xfmModifiers[0] = IOP_Bug17_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 18
datablock afxEffectWrapperData(IOP_Bug18_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_18;
  lifetime    = %IOP_Bug_Lifetime_C;
  scaleFactor = %IOP_Bug_Scale_B;

  xfmModifiers[0] = IOP_Bug18_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 19
datablock afxEffectWrapperData(IOP_Bug19_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_19;
  lifetime    = %IOP_Bug_Lifetime_D;
  scaleFactor = %IOP_Bug_Scale_A;

  xfmModifiers[0] = IOP_Bug19_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};
// bug 20
datablock afxEffectWrapperData(IOP_Bug20_EW : IOP_Bug1_EW)
{
  delay       = %IOP_Bug_Delay_20;
  lifetime    = %IOP_Bug_Lifetime_D;
  scaleFactor = %IOP_Bug_Scale_C;

  xfmModifiers[0] = IOP_Bug20_path_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// BUG PULSES

//
// Preceding the appearance of each bug from the cloud ring of the
// casting zodiac is a pulse of light.  This is done using a dts
// model that includes animated scaling spheres and animated rotating
// planes.  The planes must be aim constrained to the camera to look
// correct.
//

// bug pulse model, animated
datablock afxModelData(IOP_BugPulse_CE)
{
  shapeFile = %mySpellDataPath @ "/IOP/models/IOP_bugPulse.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  sequence = "pulse";
  sequenceRate = 1.0;
  useVertexAlpha = true;
};

// pulse aim modifier
datablock afxXM_AimData(IOP_BugPulse_aim_XM)
{
  aimZOnly = false;
};

// pulse offsets, corresponding to first points of bug paths
//  (one per bug):
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_1_XM)
{
  localOffset = "-1.825065786 -0.9171021926 0";
};
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_2_XM)
{
  localOffset = "-0.6402115012 2.004316493 0";
};
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_3_XM)
{
  localOffset = "1.735917283 -1.049954565 0";
};
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_4_XM)
{
  localOffset = "-0.3014072589 -2.004905167 0";
};
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_5_XM)
{
  localOffset = "-0.0692085379 2.009234988 0";
};
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_6_XM)
{
  localOffset = "-1.979180624 0.4978324743 0";
};
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_7_XM)
{
  localOffset = "-0.6286489242 -1.962782646 0";
};
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_8_XM)
{
  localOffset = "-2.042025506 0.04887646614 0";
};
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_9_XM)
{
  localOffset = "-1.910167867 -0.7425217795 0";
};
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_10_XM)
{
  localOffset = "1.314852092 -1.55370786 0";
};

datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_11_XM)
{
  localOffset = "-2.022106135 -0.3137694231 0";
};
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_12_XM)
{
  localOffset = "-0.3075367675 2.020682716 0";
};
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_13_XM)
{
  localOffset = "-0.6174781532 -1.94920482 0";
};
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_14_XM)
{
  localOffset = "2.013726376 0.1676288994 0";
};
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_15_XM)
{
  localOffset = "0.5959093993 -1.942610322 0";
};
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_16_XM)
{
  localOffset = "-1.936377667 0.6094602364 0";
};
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_17_XM)
{
  localOffset = "1.545517049 -1.329322049 0";
};
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_18_XM)
{
  localOffset = "1.301520639 1.565662384 0";
};
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_19_XM)
{
  localOffset = "0.9256342777 -1.810720371 0";
};
datablock afxXM_LocalOffsetData(IOP_BugPulse_Offset_20_XM)
{
  localOffset = "-1.527518818 -1.335916546 0";
};

// bug pulse 1
datablock afxEffectWrapperData(IOP_BugPulse1_EW)
{
  effect = IOP_BugPulse_CE;
  posConstraint = caster;
  posConstraint2 = "camera"; // aim

  delay       = %IOP_Bug_Delay_1-0.3;
  fadeInTime  = 0;
  fadeOutTime = 0;
  lifetime    = 0.65;

  xfmModifiers[0] = IOP_BugPulse_Offset_1_XM;
  xfmModifiers[1] = IOP_Bugs_Ground;

  xfmModifiers[2] = IOP_BugPulse_aim_XM;
};
// bug pulse 2
datablock afxEffectWrapperData(IOP_BugPulse2_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_2-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_2_XM;
};
// bug pulse 3
datablock afxEffectWrapperData(IOP_BugPulse3_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_3-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_3_XM;
};
// bug pulse 4
datablock afxEffectWrapperData(IOP_BugPulse4_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_4-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_4_XM;
};
// bug pulse 5
datablock afxEffectWrapperData(IOP_BugPulse5_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_5-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_5_XM;
};
// bug pulse 6
datablock afxEffectWrapperData(IOP_BugPulse6_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_6-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_6_XM;
};
// bug pulse 7
datablock afxEffectWrapperData(IOP_BugPulse7_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_7-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_7_XM;
};
// bug pulse 8
datablock afxEffectWrapperData(IOP_BugPulse8_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_8-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_8_XM;
};
// bug pulse 9
datablock afxEffectWrapperData(IOP_BugPulse9_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_9-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_9_XM;
};
// bug pulse 10
datablock afxEffectWrapperData(IOP_BugPulse10_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_10-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_10_XM;
};
// bug pulse 11
datablock afxEffectWrapperData(IOP_BugPulse11_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_11-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_11_XM;
};
// bug pulse 12
datablock afxEffectWrapperData(IOP_BugPulse12_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_12-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_12_XM;
};
// bug pulse 13
datablock afxEffectWrapperData(IOP_BugPulse13_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_13-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_13_XM;
};
// bug pulse 14
datablock afxEffectWrapperData(IOP_BugPulse14_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_14-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_14_XM;
};
// bug pulse 15
datablock afxEffectWrapperData(IOP_BugPulse15_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_15-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_15_XM;
};
// bug pulse 16
datablock afxEffectWrapperData(IOP_BugPulse16_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_16-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_16_XM;
};
// bug pulse 17
datablock afxEffectWrapperData(IOP_BugPulse17_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_17-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_17_XM;
};
// bug pulse 18
datablock afxEffectWrapperData(IOP_BugPulse18_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_18-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_18_XM;
};
// bug pulse 19
datablock afxEffectWrapperData(IOP_BugPulse19_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_19-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_19_XM;
};
// bug pulse 20
datablock afxEffectWrapperData(IOP_BugPulse20_EW : IOP_BugPulse1_EW)
{
  delay           = %IOP_Bug_Delay_20-0.3;  
  xfmModifiers[0] = IOP_BugPulse_Offset_20_XM;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// BUG SYMBOLS

//
// As as each bug appears from the cloud ring zodiac, a symbol
// appears, a magic rune floating upward.  Together they spin about
// the caster, rising and falling and growing until the caster
// himself is caught in their magic vortex, writhing with the pain
// of what grows inside him.
//
// What these symbols demonstrate is the power of using xfmModifiers
// as tools for animation.  Because each xfmModifier has timing
// parameters, its influence can be controlled over a time range.
// Thus, a localOffset can fade-in and move an effect, then a spin
// can fade-in and spin it, then a path can provide a more complex
// translation, etc.  And for more power these xfmModifiers need
// not be used in sequence, but can be layered, creating even more
// complex animations.
//
// In many situations it may be easier to animate a complex motion 
// inside an animation package and export an animated dts.  However
// doing it here can sometimes be faster, and allow for easier 
// orchestration with other effects that share modifiers and other
// data.  Also, in some instances it will be necessary, such as when
// aim constraints or non-dts effects are involved (barring future
// developments allowing constraints to animated mount points inside
// afxModels).
//
// To understand the symbol motion, here is the first symbol's
// modifier stack:
//   xfmModifiers[0] = IOP_BugSymbol_spin_XM;
//   xfmModifiers[1] = IOP_BugSymbol_end_spin_XM;
//   xfmModifiers[2] = IOP_BugPulse_Offset_1_XM;
//   xfmModifiers[3] = IOP_Bugs_Ground;
//   xfmModifiers[4] = IOP_BugSymbol1_path_XM;
//   xfmModifiers[5] = IOP_BugSymbol_aim_XM;
//   xfmModifiers[6] = IOP_BugSymbol1_scale_XM;
//   xfmModifiers[7] = IOP_BugSymbol_1_swirl_scale_XM;
//
// IOP_BugSymbol_spin_XM:
//   This spin fades-in slowly and lasts throughout the effect, and
//    is the main spin.
// IOP_BugSymbol_end_spin_XM:
//   This spin fades-in quickly towards the end of the effect, and
//    adding to the previous makes the symbol spin faster.
// IOP_BugPulse_Offset_1_XM:
//   This is a localOffset borrowed from the BugPulses that starts
//    the symbol at its correct position; its timing is not animated
//    and therefore is always active.
// IOP_Bugs_Ground:
//   This conforms the symbol to the ground, and is necessary (along
//    with IOP_BugPulse_Offset_1_XM) to line-up the symbol with the
//    bug it corresponds to; it is always active.
// IOP_BugSymbol1_path_XM:
//   This adds four paths to each symbol, each with their own timing;
//    the paths cause the symbol to move up, hover, move down, then
//    up again.
// IOP_BugSymbol_aim_XM:
//   This aim constraint is always active.
// IOP_BugSymbol1_scale_XM:
//   This scale fades-in as each symbol moves down, and causes the
//    symbol to stretch-out along the Z axis; the scale then fades-
//    off returning the symbol to its default scale.
// IOP_BugSymbol_1_swirl_scale_XM:
//   This scale causes the symbol to grow slightly larger towards the
//    end of the effect, as the symbols spin rapidly about the caster.
//
// In looking at this section, it may seem enormously complex.  This
// is because of the difficulty in timing such a large number of 
// xfmModifiers, which influence wrappers that begin at unique times,
// yet were wanted to act in concert.  To correctly time them all 
// a large number of variables are defined.  The math is simple, but
// the overall impression is cumbersome.  Ideally this large amount
// of timing information could be input interactively in a graphical
// interface with some kind of timeline, like a dopesheet.
//

// symbol A model
datablock afxModelData(IOP_BugSymbolA_CE)
{
  shapeFile = %mySpellDataPath @ "/IOP/models/IOP_bugSymbolA.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  useVertexAlpha = true;
};
// symbol B model
datablock afxModelData(IOP_BugSymbolB_CE : IOP_BugSymbolA_CE)
{
  shapeFile = %mySpellDataPath @ "/IOP/models/IOP_bugSymbolB.dts";
};

// IOP_BugSymbol_spin_XM: \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
//   This spin fades-in slowly and lasts throughout the effect, and
//    is the main spin.
datablock afxXM_SpinData(IOP_BugSymbol_spin_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 0.0;
  spinRate  = 360;
  delay = 2.0;
  fadeInTime  = 4.0;
  fadeOutTime = 0.0;
};
// \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

// IOP_BugSymbol_aim_XM: \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
//   This aim constraint is always active.
datablock afxXM_AimData(IOP_BugSymbol_aim_XM)
{
  aimZOnly = true;
};
// \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

// IOP_BugSymbol*_path_XM: \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
//   This adds four paths to each symbol, each with their own timing;
//    the paths cause the symbol to move up, hover, move down, then
//    up again.

// Path 1: initial up paths 
//   -- three varieties (ABC) for randomness
//   -- one of these three is used for each symbol
datablock afxPathData(IOP_BugSymbolA_Path)
{
  points = "0 0 0.5" SPC
           "0 0 3";
  lifetime = 2.0;
};
datablock afxPathData(IOP_BugSymbolB_Path : IOP_BugSymbolA_Path)
{
  points = "0 0 0.5" SPC
           "0 0 2.5";
};
datablock afxPathData(IOP_BugSymbolC_Path : IOP_BugSymbolA_Path)
{
  points = "0 0 0.5" SPC
           "0 0 3.5";
};
// (Path 1)

// Path 2: hover path
//   -- used for all symbols
datablock afxPathData(IOP_BugSymbol_hover_Path)
{
  points = "0 0  0.0" SPC
           "0 0 -0.2" SPC
           "0 0  0.3" SPC
           "0 0 -0.1" SPC
           "0 0  0.15" SPC
           "0 0 -0.25" SPC
           "0 0 -0.35" SPC
           "0 0  0.0";
  lifetime = 4.0;
  delay = 2.0;
};
// (Path 2)

// Path 3: down path
//   -- all symbols must move down at the same time, but because they
//       appear at different times, the down delay must be computed
//       uniquely for each symbol
//   -- unique path for each symbol

// Symbol Lifetimes:
//   -- necessary for subsequent timing calculations
%IOP_BugSymbol_fallTime   = 10.3-2.0;
%IOP_BugSymbol_fadeOut    = 1.0;
%IOP_BugSymbol1_lifetime  = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_1-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol2_lifetime  = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_2-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol3_lifetime  = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_3-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol4_lifetime  = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_4-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol5_lifetime  = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_5-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol6_lifetime  = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_6-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol7_lifetime  = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_7-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol8_lifetime  = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_8-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol9_lifetime  = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_9-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol10_lifetime = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_10-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol11_lifetime = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_11-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol12_lifetime = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_12-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol13_lifetime = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_13-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol14_lifetime = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_14-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol15_lifetime = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_15-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol16_lifetime = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_16-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol17_lifetime = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_17-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol18_lifetime = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_18-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol19_lifetime = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_19-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol20_lifetime = %IOP_BugSymbol_fallTime-%IOP_Bug_Delay_20-%IOP_BugSymbol_fadeOut;
%IOP_BugSymbol_extension  = 6.0;
%IOP_BugSymbol1_lifetime  += %IOP_BugSymbol_extension;
%IOP_BugSymbol2_lifetime  += %IOP_BugSymbol_extension;
%IOP_BugSymbol3_lifetime  += %IOP_BugSymbol_extension;
%IOP_BugSymbol4_lifetime  += %IOP_BugSymbol_extension;
%IOP_BugSymbol5_lifetime  += %IOP_BugSymbol_extension;
%IOP_BugSymbol6_lifetime  += %IOP_BugSymbol_extension;
%IOP_BugSymbol7_lifetime  += %IOP_BugSymbol_extension;
%IOP_BugSymbol8_lifetime  += %IOP_BugSymbol_extension;
%IOP_BugSymbol9_lifetime  += %IOP_BugSymbol_extension;
%IOP_BugSymbol10_lifetime += %IOP_BugSymbol_extension;
%IOP_BugSymbol11_lifetime += %IOP_BugSymbol_extension;
%IOP_BugSymbol12_lifetime += %IOP_BugSymbol_extension;
%IOP_BugSymbol13_lifetime += %IOP_BugSymbol_extension;
%IOP_BugSymbol14_lifetime += %IOP_BugSymbol_extension;
%IOP_BugSymbol15_lifetime += %IOP_BugSymbol_extension;
%IOP_BugSymbol16_lifetime += %IOP_BugSymbol_extension;
%IOP_BugSymbol17_lifetime += %IOP_BugSymbol_extension;
%IOP_BugSymbol18_lifetime += %IOP_BugSymbol_extension;
%IOP_BugSymbol19_lifetime += %IOP_BugSymbol_extension;
%IOP_BugSymbol20_lifetime += %IOP_BugSymbol_extension;

// Down Path Delays:
%IOP_BugSymbol_downPath_lifetime = 1.0;
%IOP_BugSymbol1_downPath_delay  = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_1;
%IOP_BugSymbol2_downPath_delay  = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_2;
%IOP_BugSymbol3_downPath_delay  = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_3;
%IOP_BugSymbol4_downPath_delay  = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_4;
%IOP_BugSymbol5_downPath_delay  = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_5;
%IOP_BugSymbol6_downPath_delay  = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_6;
%IOP_BugSymbol7_downPath_delay  = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_7;
%IOP_BugSymbol8_downPath_delay  = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_8;
%IOP_BugSymbol9_downPath_delay  = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_9;
%IOP_BugSymbol10_downPath_delay = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_10;
%IOP_BugSymbol11_downPath_delay = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_11;
%IOP_BugSymbol12_downPath_delay = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_12;
%IOP_BugSymbol13_downPath_delay = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_13;
%IOP_BugSymbol14_downPath_delay = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_14;
%IOP_BugSymbol15_downPath_delay = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_15;
%IOP_BugSymbol16_downPath_delay = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_16;
%IOP_BugSymbol17_downPath_delay = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_17;
%IOP_BugSymbol18_downPath_delay = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_18;
%IOP_BugSymbol19_downPath_delay = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_19;
%IOP_BugSymbol20_downPath_delay = %IOP_BugSymbol_fallTime-%IOP_BugSymbol_downPath_lifetime-%IOP_Bug_Delay_20;

// Down Path points:
//   -- each symbol's initial up-path was chosen from three
//       varieties; so that the symbol returns to its initial
//       position, three inverse point arrays must be defined
%IOP_BugSymbol_downPointsA = "0 0  0" SPC "0 0 -3.0";
%IOP_BugSymbol_downPointsB = "0 0  0" SPC "0 0 -2.5";
%IOP_BugSymbol_downPointsC = "0 0  0" SPC "0 0 -3.5";

// Down Paths:
//   -- looking forward ("Final Path xfmModifiers"), the initial
//       paths I'll be using for each symbol are:
//         symbol  1 -- A
//         symbol  2 -- B
//         symbol  3 -- C
//         symbol  4 -- C
//         symbol  5 -- A
//         symbol  6 -- B
//         symbol  7 -- A
//         symbol  8 -- B
//         symbol  9 -- C
//         symbol 10 -- A
//         symbol 11 -- C
//         symbol 12 -- C
//         symbol 13 -- B
//         symbol 14 -- A
//         symbol 15 -- B
//         symbol 16 -- A
//         symbol 17 -- C
//         symbol 18 -- A
//         symbol 19 -- A
//         symbol 20 -- C
//       the corresponding down path points must be used
datablock afxPathData(IOP_BugSymbol1_down_Path)
{
  points   = %IOP_BugSymbol_downPointsA;
  lifetime = %IOP_BugSymbol_downPath_lifetime;
  delay    = %IOP_BugSymbol1_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol2_down_Path : IOP_BugSymbol1_down_Path)
{
  points = %IOP_BugSymbol_downPointsB;
  delay  = %IOP_BugSymbol2_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol3_down_Path : IOP_BugSymbol1_down_Path)
{
  points = %IOP_BugSymbol_downPointsC;
  delay  = %IOP_BugSymbol3_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol4_down_Path : IOP_BugSymbol1_down_Path)
{
  points = %IOP_BugSymbol_downPointsC;
  delay  = %IOP_BugSymbol4_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol5_down_Path : IOP_BugSymbol1_down_Path)
{
  points = %IOP_BugSymbol_downPointsA;
  delay  = %IOP_BugSymbol5_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol6_down_Path : IOP_BugSymbol1_down_Path)
{
  points = %IOP_BugSymbol_downPointsB;
  delay  = %IOP_BugSymbol6_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol7_down_Path : IOP_BugSymbol1_down_Path)
{
  points = %IOP_BugSymbol_downPointsA;
  delay  = %IOP_BugSymbol7_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol8_down_Path : IOP_BugSymbol1_down_Path)
{
  points = %IOP_BugSymbol_downPointsB;
  delay  = %IOP_BugSymbol8_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol9_down_Path : IOP_BugSymbol1_down_Path)
{
  points = %IOP_BugSymbol_downPointsC;
  delay  = %IOP_BugSymbol9_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol10_down_Path : IOP_BugSymbol1_down_Path)
{
  points = %IOP_BugSymbol_downPointsA;
  delay  = %IOP_BugSymbol10_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol11_down_Path : IOP_BugSymbol1_down_Path)
{
  points = %IOP_BugSymbol_downPointsC;
  delay  = %IOP_BugSymbol11_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol12_down_Path : IOP_BugSymbol1_down_Path)
{
  points = %IOP_BugSymbol_downPointsC;
  delay  = %IOP_BugSymbol12_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol13_down_Path : IOP_BugSymbol1_down_Path)
{
  points = %IOP_BugSymbol_downPointsB;
  delay  = %IOP_BugSymbol13_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol14_down_Path : IOP_BugSymbol1_down_Path)
{
  points = %IOP_BugSymbol_downPointsA;
  delay  = %IOP_BugSymbol14_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol15_down_Path : IOP_BugSymbol1_down_Path)
{
  points = %IOP_BugSymbol_downPointsB;
  delay  = %IOP_BugSymbol15_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol16_down_Path : IOP_BugSymbol1_down_Path)
{ 
  points = %IOP_BugSymbol_downPointsA;
  delay  = %IOP_BugSymbol16_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol17_down_Path : IOP_BugSymbol1_down_Path)
{
  points = %IOP_BugSymbol_downPointsC;
  delay  = %IOP_BugSymbol17_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol18_down_Path : IOP_BugSymbol1_down_Path)
{ 
  points = %IOP_BugSymbol_downPointsA;
  delay  = %IOP_BugSymbol18_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol19_down_Path : IOP_BugSymbol1_down_Path)
{
  points = %IOP_BugSymbol_downPointsA;
  delay  = %IOP_BugSymbol19_downPath_delay;
};
datablock afxPathData(IOP_BugSymbol20_down_Path : IOP_BugSymbol1_down_Path)
{
  points = %IOP_BugSymbol_downPointsC;
  delay  = %IOP_BugSymbol20_downPath_delay;
};
// (Path 3)

// Path 4: upAgain path
//   -- so that the symbols move up at the same time, the timing
//       calculations must be carried through (again given the unique
//       starting times of each symbol)
//   -- unique path for each symbol

// upAgain Path Delays:
%IOP_BugSymbol_upPath_delay = 1.0+0.75;
%IOP_BugSymbol1_upPath_delay  = %IOP_BugSymbol1_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol2_upPath_delay  = %IOP_BugSymbol2_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol3_upPath_delay  = %IOP_BugSymbol3_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol4_upPath_delay  = %IOP_BugSymbol4_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol5_upPath_delay  = %IOP_BugSymbol5_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol6_upPath_delay  = %IOP_BugSymbol6_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol7_upPath_delay  = %IOP_BugSymbol7_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol8_upPath_delay  = %IOP_BugSymbol8_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol9_upPath_delay  = %IOP_BugSymbol9_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol10_upPath_delay = %IOP_BugSymbol10_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol11_upPath_delay = %IOP_BugSymbol11_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol12_upPath_delay = %IOP_BugSymbol12_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol13_upPath_delay = %IOP_BugSymbol13_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol14_upPath_delay = %IOP_BugSymbol14_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol15_upPath_delay = %IOP_BugSymbol15_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol16_upPath_delay = %IOP_BugSymbol16_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol17_upPath_delay = %IOP_BugSymbol17_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol18_upPath_delay = %IOP_BugSymbol18_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol19_upPath_delay = %IOP_BugSymbol19_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;
%IOP_BugSymbol20_upPath_delay = %IOP_BugSymbol20_downPath_delay+%IOP_BugSymbol_downPath_lifetime+%IOP_BugSymbol_upPath_delay;

// upAgain Paths:
datablock afxPathData(IOP_BugSymbol1_upAgain_Path)
{
  points = "0 0 0" SPC
           "0 0 1" SPC
           "0 0 2" SPC
           "0 0 1.4" SPC
           "0 0 2" SPC
           "0 0 5";
  lifetime = 6.0;
  delay = %IOP_BugSymbol1_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol2_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol2_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol3_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol3_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol4_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol4_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol5_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol5_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol6_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol6_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol7_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol7_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol8_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol8_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol9_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol9_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol10_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol10_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol11_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol11_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol12_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol12_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol13_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol13_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol14_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol14_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol15_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol15_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol16_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol16_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol17_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol17_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol18_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol18_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol19_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol19_upPath_delay;
};
datablock afxPathData(IOP_BugSymbol20_upAgain_Path : IOP_BugSymbol1_upAgain_Path)
{
  delay = %IOP_BugSymbol20_upPath_delay;
};
// (Path 4)

// Final Path xfmModifiers:
//   -- at last!
datablock afxXM_PathConformData(IOP_BugSymbol1_path_XM)
{
  paths = "IOP_BugSymbolA_Path IOP_BugSymbol_hover_Path IOP_BugSymbol1_down_Path IOP_BugSymbol1_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol2_path_XM)
{
  paths = "IOP_BugSymbolB_Path IOP_BugSymbol_hover_Path IOP_BugSymbol2_down_Path IOP_BugSymbol2_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol3_path_XM)
{
  paths = "IOP_BugSymbolC_Path IOP_BugSymbol_hover_Path IOP_BugSymbol3_down_Path IOP_BugSymbol3_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol4_path_XM)
{
  paths = "IOP_BugSymbolC_Path IOP_BugSymbol_hover_Path IOP_BugSymbol4_down_Path IOP_BugSymbol4_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol5_path_XM)
{
  paths = "IOP_BugSymbolA_Path IOP_BugSymbol_hover_Path IOP_BugSymbol5_down_Path IOP_BugSymbol5_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol6_path_XM)
{
  paths = "IOP_BugSymbolB_Path IOP_BugSymbol_hover_Path IOP_BugSymbol6_down_Path IOP_BugSymbol6_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol7_path_XM)
{
  paths = "IOP_BugSymbolA_Path IOP_BugSymbol_hover_Path IOP_BugSymbol7_down_Path IOP_BugSymbol7_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol8_path_XM)
{
  paths = "IOP_BugSymbolB_Path IOP_BugSymbol_hover_Path IOP_BugSymbol8_down_Path IOP_BugSymbol8_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol9_path_XM)
{
  paths = "IOP_BugSymbolC_Path IOP_BugSymbol_hover_Path IOP_BugSymbol9_down_Path IOP_BugSymbol8_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol10_path_XM)
{
  paths = "IOP_BugSymbolA_Path IOP_BugSymbol_hover_Path IOP_BugSymbol10_down_Path IOP_BugSymbol10_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol11_path_XM)
{
  paths = "IOP_BugSymbolC_Path IOP_BugSymbol_hover_Path IOP_BugSymbol11_down_Path IOP_BugSymbol11_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol12_path_XM)
{
  paths = "IOP_BugSymbolC_Path IOP_BugSymbol_hover_Path IOP_BugSymbol12_down_Path IOP_BugSymbol12_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol13_path_XM)
{
  paths = "IOP_BugSymbolB_Path IOP_BugSymbol_hover_Path IOP_BugSymbol13_down_Path IOP_BugSymbol13_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol14_path_XM)
{
  paths = "IOP_BugSymbolA_Path IOP_BugSymbol_hover_Path IOP_BugSymbol14_down_Path IOP_BugSymbol14_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol15_path_XM)
{
  paths = "IOP_BugSymbolB_Path IOP_BugSymbol_hover_Path IOP_BugSymbol15_down_Path IOP_BugSymbol15_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol16_path_XM)
{
  paths = "IOP_BugSymbolA_Path IOP_BugSymbol_hover_Path IOP_BugSymbol16_down_Path IOP_BugSymbol16_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol17_path_XM)
{
  paths = "IOP_BugSymbolC_Path IOP_BugSymbol_hover_Path IOP_BugSymbol17_down_Path IOP_BugSymbol17_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol18_path_XM)
{
  paths = "IOP_BugSymbolA_Path IOP_BugSymbol_hover_Path IOP_BugSymbol18_down_Path IOP_BugSymbol18_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol19_path_XM)
{
  paths = "IOP_BugSymbolA_Path IOP_BugSymbol_hover_Path IOP_BugSymbol19_down_Path IOP_BugSymbol19_upAgain_Path";
};
datablock afxXM_PathConformData(IOP_BugSymbol20_path_XM)
{
  paths = "IOP_BugSymbolC_Path IOP_BugSymbol_hover_Path IOP_BugSymbol20_down_Path IOP_BugSymbol20_upAgain_Path";
};
// \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

// IOP_BugSymbol*_scale_XM: /////////////////////////////////////////
//   This scale fades-in as each symbol moves down, and causes the
//    symbol to stretch-out along the Z axis; the scale then fades-
//    off returning the symbol to its default scale.

// Scale Timing Variables:
//   -- all symbols must scale at the same time, but because they
//       appear at different times, the scale delay must be computed
//       uniquely for each symbol
%IOP_BugSymbol1_scale_preDown = 0.25;
%IOP_BugSymbol1_scale_delay   = %IOP_BugSymbol1_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol2_scale_delay   = %IOP_BugSymbol2_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol3_scale_delay   = %IOP_BugSymbol3_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol4_scale_delay   = %IOP_BugSymbol4_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol5_scale_delay   = %IOP_BugSymbol5_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol6_scale_delay   = %IOP_BugSymbol6_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol7_scale_delay   = %IOP_BugSymbol7_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol8_scale_delay   = %IOP_BugSymbol8_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol9_scale_delay   = %IOP_BugSymbol9_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol10_scale_delay  = %IOP_BugSymbol10_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol11_scale_delay  = %IOP_BugSymbol11_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol12_scale_delay  = %IOP_BugSymbol12_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol13_scale_delay  = %IOP_BugSymbol13_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol14_scale_delay  = %IOP_BugSymbol14_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol15_scale_delay  = %IOP_BugSymbol15_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol16_scale_delay  = %IOP_BugSymbol16_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol17_scale_delay  = %IOP_BugSymbol17_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol18_scale_delay  = %IOP_BugSymbol18_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol19_scale_delay  = %IOP_BugSymbol19_downPath_delay-%IOP_BugSymbol1_scale_preDown;
%IOP_BugSymbol20_scale_delay  = %IOP_BugSymbol20_downPath_delay-%IOP_BugSymbol1_scale_preDown;
//
%IOP_BugSymbol_scale_lifetime    = 2.5;
%IOP_BugSymbol_scale_fadeOutTime = 1.0;

// scale xfmModifiers:
datablock afxXM_ScaleData(IOP_BugSymbol1_scale_XM)
{
  scale = "2 2 8";

  delay       = %IOP_BugSymbol1_scale_delay;
  fadeInTime  = 2.0;
  lifetime    = %IOP_BugSymbol_scale_lifetime;
  fadeOutTime = %IOP_BugSymbol_scale_fadeOutTime;
};
datablock afxXM_ScaleData(IOP_BugSymbol2_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol2_scale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol3_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol3_scale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol4_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol4_scale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol5_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol5_scale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol6_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol6_scale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol7_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol7_scale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol8_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol8_scale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol9_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol9_scale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol10_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol10_scale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol11_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol11_scale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol12_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol12_scale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol13_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol13_scale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol14_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol14_scale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol15_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol15_scale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol16_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol16_scale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol17_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol17_scale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol18_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol18_scale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol19_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol19_scale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol20_scale_XM : IOP_BugSymbol1_scale_XM)
{
  delay = %IOP_BugSymbol20_scale_delay;
};
// \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

// IOP_BugSymbol_*_swirl_scale_XM: //////////////////////////////////
//   This scale causes the symbol to grow slightly larger towards the
//    end of the effect, as the symbols spin rapidly about the caster.

// Swirl Scale Timing Variables:
//   -- all symbols must scale at the same time, but because they
//       appear at different times, the scale delay must be computed
//       uniquely for each symbol
%IOP_BugSymbol_swirlScale_delay   = %IOP_BugSymbol_scale_lifetime+%IOP_BugSymbol_scale_fadeOutTime-1.0;
%IOP_BugSymbol1_swirlScale_delay  = %IOP_BugSymbol1_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol2_swirlScale_delay  = %IOP_BugSymbol2_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol3_swirlScale_delay  = %IOP_BugSymbol3_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol4_swirlScale_delay  = %IOP_BugSymbol4_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol5_swirlScale_delay  = %IOP_BugSymbol5_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol6_swirlScale_delay  = %IOP_BugSymbol6_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol7_swirlScale_delay  = %IOP_BugSymbol7_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol8_swirlScale_delay  = %IOP_BugSymbol8_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol9_swirlScale_delay  = %IOP_BugSymbol9_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol10_swirlScale_delay = %IOP_BugSymbol10_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol11_swirlScale_delay = %IOP_BugSymbol11_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol12_swirlScale_delay = %IOP_BugSymbol12_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol13_swirlScale_delay = %IOP_BugSymbol13_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol14_swirlScale_delay = %IOP_BugSymbol14_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol15_swirlScale_delay = %IOP_BugSymbol15_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol16_swirlScale_delay = %IOP_BugSymbol16_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol17_swirlScale_delay = %IOP_BugSymbol17_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol18_swirlScale_delay = %IOP_BugSymbol18_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol19_swirlScale_delay = %IOP_BugSymbol19_scale_delay+%IOP_BugSymbol_swirlScale_delay;
%IOP_BugSymbol20_swirlScale_delay = %IOP_BugSymbol20_scale_delay+%IOP_BugSymbol_swirlScale_delay;

// swirl-scale xfmModifiers:
datablock afxXM_ScaleData(IOP_BugSymbol_1_swirl_scale_XM)
{
  scale = "1.3 1.3 1.3";

  delay = %IOP_BugSymbol1_swirlScale_delay;
  fadeInTime = 0.5;
};
datablock afxXM_ScaleData(IOP_BugSymbol_2_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol2_swirlScale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol_3_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol3_swirlScale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol_4_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol4_swirlScale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol_5_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol5_swirlScale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol_6_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol6_swirlScale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol_7_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol7_swirlScale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol_8_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol8_swirlScale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol_9_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol9_swirlScale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol_10_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol10_swirlScale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol_11_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol11_swirlScale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol_12_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol12_swirlScale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol_13_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol13_swirlScale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol_14_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol14_swirlScale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol_15_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol15_swirlScale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol_16_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol16_swirlScale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol_17_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol17_swirlScale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol_18_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol18_swirlScale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol_19_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol19_swirlScale_delay;
};
datablock afxXM_ScaleData(IOP_BugSymbol_20_swirl_scale_XM : IOP_BugSymbol_1_swirl_scale_XM)
{
  delay = %IOP_BugSymbol20_swirlScale_delay;
};
// \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

// IOP_BugSymbol_end_spin_XM: ///////////////////////////////////////
//   This spin fades-in quickly towards the end of the effect, and
//    adding to the previous makes the symbol spin faster.
datablock afxXM_SpinData(IOP_BugSymbol_end_spin_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 0.0;
  spinRate  = 360; 

  delay = %IOP_BugSymbol1_swirlScale_delay-1.0;
  fadeInTime  = 1.0;
};
// \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

// Various symbol scales to add variety:
%IOP_BugSymbol_scaleA = 0.6;
%IOP_BugSymbol_scaleB = 0.7;
%IOP_BugSymbol_scaleC = 0.9;

// Bug Symbol 1
datablock afxEffectWrapperData(IOP_BugSymbol1_EW)
{
  effect = IOP_BugSymbolA_CE;
  posConstraint = caster;
  posConstraint2 = "camera"; // aim

  scaleFactor = %IOP_BugSymbol_scaleB;

  delay       = %IOP_Bug_Delay_1-0.0;
  fadeInTime  = 0.4;
  fadeOutTime = %IOP_BugSymbol_fadeOut;
  lifetime    = %IOP_BugSymbol1_lifetime;

  xfmModifiers[0] = IOP_BugSymbol_spin_XM;
  xfmModifiers[1] = IOP_BugSymbol_end_spin_XM;
  xfmModifiers[2] = IOP_BugPulse_Offset_1_XM;
  xfmModifiers[3] = IOP_Bugs_Ground;
  xfmModifiers[4] = IOP_BugSymbol1_path_XM;
  xfmModifiers[5] = IOP_BugSymbol_aim_XM;
  xfmModifiers[6] = IOP_BugSymbol1_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_1_swirl_scale_XM;
};
// Bug Symbol 2
datablock afxEffectWrapperData(IOP_BugSymbol2_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolB_CE;
  scaleFactor     = %IOP_BugSymbol_scaleA;
  delay           = %IOP_Bug_Delay_2-0.0;
  lifetime        = %IOP_BugSymbol2_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_2_XM;
  xfmModifiers[4] = IOP_BugSymbol2_path_XM;
  xfmModifiers[6] = IOP_BugSymbol2_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_2_swirl_scale_XM;
};
// Bug Symbol 3
datablock afxEffectWrapperData(IOP_BugSymbol3_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolA_CE;
  scaleFactor     = %IOP_BugSymbol_scaleC;
  delay           = %IOP_Bug_Delay_3-0.0;
  lifetime        = %IOP_BugSymbol3_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_3_XM;
  xfmModifiers[4] = IOP_BugSymbol3_path_XM;
  xfmModifiers[6] = IOP_BugSymbol3_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_3_swirl_scale_XM;
};
// Bug Symbol 4
datablock afxEffectWrapperData(IOP_BugSymbol4_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolB_CE;
  scaleFactor     = %IOP_BugSymbol_scaleB;
  delay           = %IOP_Bug_Delay_4-0.0;
  lifetime        = %IOP_BugSymbol4_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_4_XM;
  xfmModifiers[4] = IOP_BugSymbol4_path_XM;
  xfmModifiers[6] = IOP_BugSymbol4_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_4_swirl_scale_XM;
};
// Bug Symbol 5
datablock afxEffectWrapperData(IOP_BugSymbol5_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolA_CE;
  scaleFactor     = %IOP_BugSymbol_scaleA;
  delay           = %IOP_Bug_Delay_5-0.0;
  lifetime        = %IOP_BugSymbol5_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_5_XM;
  xfmModifiers[4] = IOP_BugSymbol5_path_XM;
  xfmModifiers[6] = IOP_BugSymbol5_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_5_swirl_scale_XM;
};
// Bug Symbol 6
datablock afxEffectWrapperData(IOP_BugSymbol6_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolB_CE;
  scaleFactor     = %IOP_BugSymbol_scaleC;
  delay           = %IOP_Bug_Delay_6-0.0;
  lifetime        = %IOP_BugSymbol6_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_6_XM;
  xfmModifiers[4] = IOP_BugSymbol6_path_XM;
  xfmModifiers[6] = IOP_BugSymbol6_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_6_swirl_scale_XM;
};
// Bug Symbol 7
datablock afxEffectWrapperData(IOP_BugSymbol7_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolA_CE;
  scaleFactor     = %IOP_BugSymbol_scaleA;
  delay           = %IOP_Bug_Delay_7-0.0;
  lifetime        = %IOP_BugSymbol7_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_7_XM;
  xfmModifiers[4] = IOP_BugSymbol7_path_XM;
  xfmModifiers[6] = IOP_BugSymbol7_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_7_swirl_scale_XM;
};
// Bug Symbol 8
datablock afxEffectWrapperData(IOP_BugSymbol8_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolB_CE;
  scaleFactor     = %IOP_BugSymbol_scaleB;
  delay           = %IOP_Bug_Delay_8-0.0;
  lifetime        = %IOP_BugSymbol8_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_8_XM;
  xfmModifiers[4] = IOP_BugSymbol8_path_XM;
  xfmModifiers[6] = IOP_BugSymbol8_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_8_swirl_scale_XM;
};
// Bug Symbol 9
datablock afxEffectWrapperData(IOP_BugSymbol9_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolA_CE;
  scaleFactor     = $IOP_BugSymbol_scaleA;
  delay           = %IOP_Bug_Delay_9-0.0;
  lifetime        = %IOP_BugSymbol9_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_9_XM;
  xfmModifiers[4] = IOP_BugSymbol9_path_XM;
  xfmModifiers[6] = IOP_BugSymbol9_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_9_swirl_scale_XM;
};
// Bug Symbol 10
datablock afxEffectWrapperData(IOP_BugSymbol10_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolB_CE;
  scaleFactor     = %IOP_BugSymbol_scaleC;
  delay           = %IOP_Bug_Delay_10-0.0;
  lifetime        = %IOP_BugSymbol10_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_10_XM;
  xfmModifiers[4] = IOP_BugSymbol10_path_XM;
  xfmModifiers[6] = IOP_BugSymbol10_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_10_swirl_scale_XM;
};
// Bug Symbol 11
datablock afxEffectWrapperData(IOP_BugSymbol11_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolA_CE;
  scaleFactor     = %IOP_BugSymbol_scaleA;
  delay           = %IOP_Bug_Delay_11-0.0;
  lifetime        = %IOP_BugSymbol11_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_11_XM;
  xfmModifiers[4] = IOP_BugSymbol11_path_XM;
  xfmModifiers[6] = IOP_BugSymbol11_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_11_swirl_scale_XM;
};
// Bug Symbol 12
datablock afxEffectWrapperData(IOP_BugSymbol12_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolB_CE;
  scaleFactor     = %IOP_BugSymbol_scaleB;
  delay           = %IOP_Bug_Delay_12-0.0;
  lifetime        = %IOP_BugSymbol12_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_12_XM;
  xfmModifiers[4] = IOP_BugSymbol12_path_XM;
  xfmModifiers[6] = IOP_BugSymbol12_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_12_swirl_scale_XM;
};
// Bug Symbol 13
datablock afxEffectWrapperData(IOP_BugSymbol13_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolA_CE;
  scaleFactor     = %IOP_BugSymbol_scaleA;
  delay           = %IOP_Bug_Delay_13-0.0;
  lifetime        = %IOP_BugSymbol13_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_13_XM;
  xfmModifiers[4] = IOP_BugSymbol13_path_XM;
  xfmModifiers[6] = IOP_BugSymbol13_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_13_swirl_scale_XM;
};
// Bug Symbol 14
datablock afxEffectWrapperData(IOP_BugSymbol14_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolB_CE;
  scaleFactor     = %IOP_BugSymbol_scaleA;
  delay           = %IOP_Bug_Delay_14-0.0;
  lifetime        = %IOP_BugSymbol14_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_14_XM;
  xfmModifiers[4] = IOP_BugSymbol14_path_XM;
  xfmModifiers[6] = IOP_BugSymbol14_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_14_swirl_scale_XM;
};
// Bug Symbol 15
datablock afxEffectWrapperData(IOP_BugSymbol15_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolA_CE;
  scaleFactor     = %IOP_BugSymbol_scaleC;
  delay           = %IOP_Bug_Delay_15-0.0;
  lifetime        = %IOP_BugSymbol15_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_15_XM;
  xfmModifiers[4] = IOP_BugSymbol15_path_XM;
  xfmModifiers[6] = IOP_BugSymbol15_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_15_swirl_scale_XM;
};
// Bug Symbol 16
datablock afxEffectWrapperData(IOP_BugSymbol16_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolB_CE;
  scaleFactor     = %IOP_BugSymbol_scaleB;
  delay           = %IOP_Bug_Delay_16-0.0;
  lifetime        = %IOP_BugSymbol16_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_16_XM;
  xfmModifiers[4] = IOP_BugSymbol16_path_XM;
  xfmModifiers[6] = IOP_BugSymbol16_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_16_swirl_scale_XM;
};
// Bug Symbol 17
datablock afxEffectWrapperData(IOP_BugSymbol17_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolA_CE;
  scaleFactor     = %IOP_BugSymbol_scaleB;
  delay           = %IOP_Bug_Delay_17-0.0;
  lifetime        = %IOP_BugSymbol17_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_17_XM;
  xfmModifiers[4] = IOP_BugSymbol17_path_XM;
  xfmModifiers[6] = IOP_BugSymbol17_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_17_swirl_scale_XM;
};
// Bug Symbol 18
datablock afxEffectWrapperData(IOP_BugSymbol18_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolA_CE;
  scaleFactor     = %IOP_BugSymbol_scaleA;
  delay           = %IOP_Bug_Delay_18-0.0;
  lifetime        = %IOP_BugSymbol18_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_18_XM;
  xfmModifiers[4] = IOP_BugSymbol18_path_XM;
  xfmModifiers[6] = IOP_BugSymbol18_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_18_swirl_scale_XM;
};
// Bug Symbol 19
datablock afxEffectWrapperData(IOP_BugSymbol19_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolB_CE;
  scaleFactor     = %IOP_BugSymbol_scaleB;
  delay           = %IOP_Bug_Delay_19-0.0;
  lifetime        = %IOP_BugSymbol19_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_19_XM;
  xfmModifiers[4] = IOP_BugSymbol19_path_XM;
  xfmModifiers[6] = IOP_BugSymbol19_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_19_swirl_scale_XM;
};
// Bug Symbol 20
datablock afxEffectWrapperData(IOP_BugSymbol20_EW : IOP_BugSymbol1_EW)
{
  effect          = IOP_BugSymbolA_CE;
  scaleFactor     = %IOP_BugSymbol_scaleC;
  delay           = %IOP_Bug_Delay_20-0.0;
  lifetime        = %IOP_BugSymbol20_lifetime;

  xfmModifiers[2] = IOP_BugPulse_Offset_20_XM;
  xfmModifiers[4] = IOP_BugSymbol20_path_XM;
  xfmModifiers[6] = IOP_BugSymbol20_scale_XM;
  xfmModifiers[7] = IOP_BugSymbol_20_swirl_scale_XM;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// BEAMS

//
// As the bugs coalesce within him, the orc rises spinning through
// the air.  Beneath him demonic energies surge up, glowing and
// building terribly -- towards the terrible climax.
//
// This effect consists of two dts geometries and two zodiacs.  The
// initial geometry is a cylindrical shape that animates up and then
// down again; a glowing zodiac ties this and the other shapes to the
// ground nicely.  Following this more beams appear in sequence,
// basically thin planes that spin about the caster while rising and
// falling.  To complete the illusion of the main zodiac symbolizing
// the monster about to be born, a glowing eye completes this effect.
// 

// Beam Timing Variables
$IOP_BeamZodiac_delay = 10.0-2.0;
$IOP_BeamRing_delay   = $IOP_BeamZodiac_delay+0.75;
$IOP_Beams1_delay     = $IOP_BeamRing_delay+0.5;
$IOP_Beams2_delay     = $IOP_Beams1_delay+1.0;
$IOP_Beams3_delay     = $IOP_Beams2_delay+1.0;
$IOP_Beams4_delay     = $IOP_Beams3_delay+1.0;
$IOP_BeamSequenceRate = 1.4;
$IOP_BeamLifetime     = (300/30)*(1.0/$IOP_BeamSequenceRate);

// glow ring zodiac
datablock afxZodiacData(IOP_BeamZodiac_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/IOP/zodiacs/IOP_caster_outerRing";
  radius = 3.0;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(IOP_BeamZodiac_EW)
{
  effect = IOP_BeamZodiac_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = $IOP_BeamZodiac_delay;
  fadeInTime  = 0.75;
  fadeOutTime = 1.0;
  lifetime = $IOP_BeamLifetime+0.5;
};

// cylindrical beam ring model, animated
datablock afxModelData(IOP_BeamRing_CE)
{
  shapeFile = %mySpellDataPath @ "/IOP/models/IOP_beamRing.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  sequence = "beam";
  sequenceRate = $IOP_BeamSequenceRate;
  alphaMult = 1.0;
  useVertexAlpha = true;
};
//
datablock afxEffectWrapperData(IOP_BeamRing_EW)
{
  effect = IOP_BeamRing_CE;
  posConstraint = caster;

  delay = $IOP_BeamRing_delay;
  fadeInTime = 0.75;
  fadeOutTime = 0.5;
  lifetime = $IOP_BeamLifetime-0.5;
};

// Unique spins for each of the twelve beams to follow:
datablock afxXM_SpinData(IOP_Beam1_spin_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 0.0;
  spinRate  = 170;
};
datablock afxXM_SpinData(IOP_Beam2_spin_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 122.0;
  spinRate  = -150;
};
datablock afxXM_SpinData(IOP_Beam3_spin_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 56.0;
  spinRate  = 125;
};
datablock afxXM_SpinData(IOP_Beam4_spin_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 6.0;
  spinRate  = -60;
};
datablock afxXM_SpinData(IOP_Beam5_spin_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = -90.0;
  spinRate  = 78;
};
datablock afxXM_SpinData(IOP_Beam6_spin_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = -56.0;
  spinRate  = -125;
};
datablock afxXM_SpinData(IOP_Beam7_spin_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 144.0;
  spinRate  = 89;
};
datablock afxXM_SpinData(IOP_Beam8_spin_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = -34.0;
  spinRate  = -55;
};
datablock afxXM_SpinData(IOP_Beam9_spin_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 177.0;
  spinRate  = 101;
};
datablock afxXM_SpinData(IOP_Beam10_spin_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 77.0;
  spinRate  = -101;
};
datablock afxXM_SpinData(IOP_Beam11_spin_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = -36.0;
  spinRate  = 133;
};
datablock afxXM_SpinData(IOP_Beam12_spin_XM)
{
  spinAxis  = "0 0 1";
  spinAngle = 59.0;
  spinRate  = 75;
};

// beam model A
datablock afxModelData(IOP_BeamsA_CE)
{
  shapeFile = %mySpellDataPath @ "/IOP/models/IOP_beamsA.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  sequence = "beam";
  sequenceRate = $IOP_BeamSequenceRate;
  alphaMult = 0.5;
  useVertexAlpha = true;
};
// beam model B
datablock afxModelData(IOP_BeamsB_CE : IOP_BeamsA_CE)
{
  shapeFile = %mySpellDataPath @ "/IOP/models/IOP_beamsB.dts";
};
// beam model C
datablock afxModelData(IOP_BeamsC_CE : IOP_BeamsA_CE)
{
  shapeFile = %mySpellDataPath @ "/IOP/models/IOP_beamsC.dts";
};

// Beams A1
datablock afxEffectWrapperData(IOP_BeamsA1_EW)
{
  effect = IOP_BeamsA_CE;
  posConstraint = caster;

  delay = $IOP_Beams1_delay;
  fadeInTime = 0.5;
  fadeOutTime = 1.0;
  lifetime = $IOP_BeamLifetime-1.5;

  xfmModifiers[0] = "IOP_Beam1_spin_XM";
};
// Beams A2
datablock afxEffectWrapperData(IOP_BeamsA2_EW : IOP_BeamsA1_EW)
{
  delay = $IOP_Beams2_delay;
  lifetime = $IOP_BeamLifetime-2.5;

  xfmModifiers[0] = "IOP_Beam2_spin_XM";
};
// Beams A3
datablock afxEffectWrapperData(IOP_BeamsA3_EW : IOP_BeamsA1_EW)
{
  delay = $IOP_Beams3_delay;
  lifetime = $IOP_BeamLifetime-3.5;

  xfmModifiers[0] = "IOP_Beam3_spin_XM";
};
// Beams A4
datablock afxEffectWrapperData(IOP_BeamsA4_EW : IOP_BeamsA1_EW)
{
  delay = $IOP_Beams4_delay;
  lifetime = $IOP_BeamLifetime-4.5;

  xfmModifiers[0] = "IOP_Beam4_spin_XM";
};
// Beams B1
datablock afxEffectWrapperData(IOP_BeamsB1_EW : IOP_BeamsA1_EW)
{
  effect          = IOP_BeamsB_CE;
  xfmModifiers[0] = "IOP_Beam5_spin_XM";
};
// Beams B2
datablock afxEffectWrapperData(IOP_BeamsB2_EW : IOP_BeamsA2_EW)
{
  effect          = IOP_BeamsB_CE;
  xfmModifiers[0] = "IOP_Beam6_spin_XM";
};
// Beams B3
datablock afxEffectWrapperData(IOP_BeamsB3_EW : IOP_BeamsA3_EW)
{
  effect          = IOP_BeamsB_CE;
  xfmModifiers[0] = "IOP_Beam7_spin_XM";
};
// Beams B4
datablock afxEffectWrapperData(IOP_BeamsB4_EW : IOP_BeamsA4_EW)
{
  effect          = IOP_BeamsB_CE;
  xfmModifiers[0] = "IOP_Beam8_spin_XM";
};
// Beams C1
datablock afxEffectWrapperData(IOP_BeamsC1_EW : IOP_BeamsA1_EW)
{
  effect          = IOP_BeamsC_CE;
  xfmModifiers[0] = "IOP_Beam9_spin_XM";
};
// Beams C2
datablock afxEffectWrapperData(IOP_BeamsC2_EW : IOP_BeamsA2_EW)
{
  effect          = IOP_BeamsC_CE;
  xfmModifiers[0] = "IOP_Beam10_spin_XM";
};
// Beams C3
datablock afxEffectWrapperData(IOP_BeamsC3_EW : IOP_BeamsA3_EW)
{
  effect          = IOP_BeamsC_CE;
  xfmModifiers[0] = "IOP_Beam11_spin_XM";
};
// Beams C4
datablock afxEffectWrapperData(IOP_BeamsC4_EW : IOP_BeamsA4_EW)
{
  effect          = IOP_BeamsC_CE;
  xfmModifiers[0] = "IOP_Beam12_spin_XM";
};

// eye glow zodiac
datablock afxZodiacData(IOP_EyeGlowZodiac_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/IOP/zodiacs/IOP_eyeGlow";
  radius = 0.7;
  color = "1.0 1.0 1.0 1.0";
  blend = additive;
};
//
datablock afxEffectWrapperData(IOP_EyeGlowZodiac_EW)
{
  effect = IOP_EyeGlowZodiac_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = $IOP_BeamZodiac_delay+3.5;
  fadeInTime = 0.75;
  fadeOutTime = 1.0;
  lifetime = $IOP_gag2+0.5-3.5;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING GOO SPURT

//
// Inevitably the thing growing and growing inside the orc's belly
// must be born.  And when it is, it bursts!  Out from the convulsing
// orc shoots a giant insect monster of flame and anger.  And with it
// comes great spurts of green goo.
//
// Cone particle emitters are used here, placed approximately at the
// orc's belly.  Various particle goo textures are used, as well as
// various emitters with differenct parameters, to create a full and
// randomized effect.  The emission is timed to match the convulsions
// of the orc.
//

// Main Goo Particles
//   -- the goo particles used during casting and during impact are
//       defined here
//   -- six particles (A-F) are used, each with a unique texture, for
//       sufficient variety
//
// goo spurt particle A -- small goo
datablock ParticleData(IOP_GooSpurtA_P)
{
  // TGE textureName        = %mySpellDataPath @ "/IOP/particles/IOP_gooA";
  dragCoeffiecient   = 0.5;
  gravityCoefficient = 1.0;
  inheritedVelFactor = 0.00;
  lifetimeMS         = 1200;
  lifetimeVarianceMS = 300;
  useInvAlpha        = true;
  spinRandomMin      = -360.0;
  spinRandomMax      = 360.0;
  colors[0]          = "1.0 1.0 1.0 0.8";
  colors[1]          = "1.0 1.0 1.0 0.0";
  sizes[0]           = 0.5;   
  sizes[1]           = 0.6;
  times[0]           = 0.0;   
  times[1]           = 1.0;

  // TGEA
  textureName        = %mySpellDataPath @ "/IOP/particles/iop_tiled_goo"; // gooA
  textureCoords[0]   = "0.0  0.5";
  textureCoords[1]   = "0.0  0.75";
  textureCoords[2]   = "0.25 0.75";
  textureCoords[3]   = "0.25 0.5";
};
// goo spurt particle B
datablock ParticleData(IOP_GooSpurtB_P : IOP_GooSpurtA_P)
{
  // TGE textureName        = %mySpellDataPath @ "/IOP/particles/IOP_gooB";
  sizes[0]           = 1.0;   
  sizes[1]           = 1.3;

  // TGEA -- gooB
  textureCoords[0]    = "0.0 0.0";
  textureCoords[1]    = "0.0 0.5";
  textureCoords[2]    = "0.5 0.5";
  textureCoords[3]    = "0.5 0.0";
};
// goo spurt particle C
datablock ParticleData(IOP_GooSpurtC_P : IOP_GooSpurtA_P)
{
  // TGE textureName        = %mySpellDataPath @ "/IOP/particles/IOP_gooC";
  sizes[0]           = 1.0;   
  sizes[1]           = 1.3;

  // TGEA -- gooC
  textureCoords[0]   = "0.25 0.5";
  textureCoords[1]   = "0.25 1.0";
  textureCoords[2]   = "0.65 1.0";
  textureCoords[3]   = "0.65 0.5";
};
// goo spurt particle D
datablock ParticleData(IOP_GooSpurtD_P : IOP_GooSpurtA_P)
{
  // TGE textureName        = %mySpellDataPath @ "/IOP/particles/IOP_gooD";
  sizes[0]           = 1.0;   
  sizes[1]           = 1.3;

  // TGEA -- gooD
  textureCoords[0]   = "0.6 0.5";
  textureCoords[1]   = "0.6 1.0";
  textureCoords[2]   = "1.0 1.0";
  textureCoords[3]   = "1.0 0.5";
};
// goo spurt particle E -- small goo
datablock ParticleData(IOP_GooSpurtE_P : IOP_GooSpurtA_P)
{
  // TGE textureName        = %mySpellDataPath @ "/IOP/particles/IOP_gooE";
  sizes[0]           = 0.7;   
  sizes[1]           = 0.8;

  // TGEA -- gooE
  textureCoords[0]   = "0.0  0.75";
  textureCoords[1]   = "0.0  1.0";
  textureCoords[2]   = "0.25 1.0";
  textureCoords[3]   = "0.25 0.75";
};
// goo spurt particle F
datablock ParticleData(IOP_GooSpurtF_P : IOP_GooSpurtA_P)
{
  // TGE textureName        = %mySpellDataPath @ "/IOP/particles/IOP_gooF";
  sizes[0]           = 1.0;   
  sizes[1]           = 1.3;

  // TGEA -- gooF
  textureCoords[0]   = "0.5 0.0";
  textureCoords[1]   = "0.5 0.5";
  textureCoords[2]   = "1.0 0.5";
  textureCoords[3]   = "1.0 0.0";
};

// Tiny Goo Particles
//   -- these small goo particles are used only during casting to
//       fill-out the goo spurts
//
// goo spurt tiny 1
datablock ParticleData(IOP_GooSpurtA_tiny1_P : IOP_GooSpurtA_P)
{
  sizes[0]           = 0.5 * 0.5;
  sizes[1]           = 0.6 * 0.5;
};
// goo spurt tiny 2
datablock ParticleData(IOP_GooSpurtA_tiny2_P : IOP_GooSpurtA_P)
{
  sizes[0]           = 0.5 * 0.35;
  sizes[1]           = 0.6 * 0.35;
};
// goo spurt tiny 3
datablock ParticleData(IOP_GooSpurtA_tiny3_P : IOP_GooSpurtA_P)
{
  sizes[0]           = 0.5 * 0.2;
  sizes[1]           = 0.6 * 0.2;
};

// Tiny Goo Spurt Emitters
//   -- these emitters are used to emit tiny goo particles from the
//       orc's belly
//   -- various speeds and emission-densities are used for randomness
//   -- cone emitters are used to project the goo out like a water
//       fountain
//
// tiny goo spurt emitter -- medium speed & density
datablock afxParticleEmitterConeData(IOP_GooSpurt_Tiny_Casting_E) // TGEA
{
  // TGE emitterType = "cone";
  vector      = "0 0 1";
  spreadMin   = 0.0;
  spreadMax   = 90.0;

  ejectionPeriodMS = 5;
  periodVarianceMS = 1;
  ejectionVelocity = 8.0;
  velocityVariance = 8.0/3.0;  
  particles        = "IOP_GooSpurtA_tiny1_P IOP_GooSpurtA_tiny2_P IOP_GooSpurtA_tiny3_P";
};
// tiny goo spurt emitter -- high speed & density
datablock afxParticleEmitterConeData(IOP_GooSpurt_Tiny_Casting_BIG_E : IOP_GooSpurt_Tiny_Casting_E) // TGEA
{
  ejectionPeriodMS = 2;
  ejectionVelocity = 13.0;
  velocityVariance = 11.0/3.0;
};
// tiny goo spurt emitter -- low speed & density
datablock afxParticleEmitterConeData(IOP_GooSpurt_Tiny_Casting_SMALL_E : IOP_GooSpurt_Tiny_Casting_E) // TGEA
{
  ejectionPeriodMS = 8;
  ejectionVelocity = 6.0;
  velocityVariance = 6.0/3.0;
};

// Main Goo Spurt Emitters
//   -- these emitters are used to emit the main, mostly larger goo
//       particles from the orc's belly
//   -- various speeds and emission-densities are used for randomness
//   -- cone emitters are used to project the goo out like a water
//       fountain
//
// goo spurt emitter -- medium speed & density
datablock afxParticleEmitterConeData(IOP_GooSpurt_Casting_E) // TGEA
{
  // TGE emitterType = "cone";
  vector      = "0 0 1";
  spreadMin   = 0.0;
  spreadMax   = 90.0;

  ejectionPeriodMS = 30;
  periodVarianceMS = 1;
  ejectionVelocity = 5.0;
  velocityVariance = 5.0/3.0;  
  particles        = "IOP_GooSpurtA_P IOP_GooSpurtA_P IOP_GooSpurtA_P " @
                     "IOP_GooSpurtE_P IOP_GooSpurtE_P " @
                     "IOP_GooSpurtB_P IOP_GooSpurtC_P IOP_GooSpurtD_P IOP_GooSpurtF_P";
};
// goo spurt emitter -- high speed & density
datablock afxParticleEmitterConeData(IOP_GooSpurt_Casting_BIG_E : IOP_GooSpurt_Casting_E) // TGEA
{
  ejectionPeriodMS = 21;
  ejectionVelocity = 7.0;
  velocityVariance = 7.0/3.0;
};
// goo spurt emitter -- low speed & density
datablock afxParticleEmitterConeData(IOP_GooSpurt_Casting_SMALL_E : IOP_GooSpurt_Casting_E) // TGEA
{
  ejectionPeriodMS = 45;
  ejectionVelocity = 4.0;
  velocityVariance = 4.0/3.0;
};
// goo spurt spray emitter -- highest speed, intended to shoot some
//  goo out from the main "explosion"
datablock afxParticleEmitterConeData(IOP_GooSpurt_Casting_Spray_E : IOP_GooSpurt_Casting_E) // TGEA
{
  spreadMin        = 70.0;
  spreadMax        = 90.0;
  ejectionPeriodMS = 20;
  ejectionVelocity = 16.0;
  velocityVariance = 16.0/3.0;
};

// Goo Spurt Timing Variables
//   -- most of the integers here are frame numbers, used to sync-up
//       the goo spurts with the convulsions of the orc inside the
//       maya animation files
//   -- 30 is the frames-per-second
%IOP_GooSpurt_Delay_nudge = 0.30;
%IOP_GooSpurt_Delay_1 = ((487-60)/30)-%IOP_GooSpurt_Delay_nudge;
%IOP_GooSpurt_Delay_2 = ((502-60)/30)-%IOP_GooSpurt_Delay_nudge;
%IOP_GooSpurt_Delay_3 = ((513-60)/30)-%IOP_GooSpurt_Delay_nudge;
%IOP_GooSpurt_Delay_4 = ((524-60)/30)-%IOP_GooSpurt_Delay_nudge;
%IOP_GooSpurt_Delay_5 = ((530-60)/30)-%IOP_GooSpurt_Delay_nudge;

// Goo Spurt Offset
//   -- approximates the position of the orc's belly, from which the
//      insect-monster will shoot out
datablock afxXM_LocalOffsetData(IOP_GooSpurt_Offset_XM)
{
  localOffset = "0 0 2.5";
};

// Goo Spurt Tiny 1
datablock afxEffectWrapperData(IOP_Casting_GooSpurt_Tiny_1_EW)
{
  effect = IOP_GooSpurt_Tiny_Casting_BIG_E;
  posConstraint = caster;
  delay    = %IOP_GooSpurt_Delay_1;
  lifetime = 0.2;
  xfmModifiers[0] = IOP_GooSpurt_Offset_XM;
};
// Goo Spurt Tiny 2
datablock afxEffectWrapperData(IOP_Casting_GooSpurt_Tiny_2_EW : IOP_Casting_GooSpurt_Tiny_1_EW)
{
  effect = IOP_GooSpurt_Tiny_Casting_E;
  delay  = %IOP_GooSpurt_Delay_2;
};
// Goo Spurt Tiny 3
datablock afxEffectWrapperData(IOP_Casting_GooSpurt_Tiny_3_EW : IOP_Casting_GooSpurt_Tiny_1_EW)
{
  effect = IOP_GooSpurt_Tiny_Casting_E;
  delay  = %IOP_GooSpurt_Delay_3;
};
// Goo Spurt Tiny 4
datablock afxEffectWrapperData(IOP_Casting_GooSpurt_Tiny_4_EW : IOP_Casting_GooSpurt_Tiny_1_EW)
{
  effect = IOP_GooSpurt_Tiny_Casting_E;
  delay  = %IOP_GooSpurt_Delay_4;
};
// Goo Spurt Tiny 5
datablock afxEffectWrapperData(IOP_Casting_GooSpurt_Tiny_5_EW : IOP_Casting_GooSpurt_Tiny_1_EW)
{
  effect = IOP_GooSpurt_Tiny_Casting_SMALL_E;
  delay  = %IOP_GooSpurt_Delay_5;
};

// Goo Spurt 1
datablock afxEffectWrapperData(IOP_Casting_GooSpurt1_EW)
{
  effect = IOP_GooSpurt_Casting_BIG_E;
  posConstraint = caster;
  delay    = %IOP_GooSpurt_Delay_1;
  lifetime = 0.2;
  xfmModifiers[0] = IOP_GooSpurt_Offset_XM;
};
// Goo Spurt 2
datablock afxEffectWrapperData(IOP_Casting_GooSpurt2_EW : IOP_Casting_GooSpurt1_EW)
{
  effect = IOP_GooSpurt_Casting_E;
  delay  = %IOP_GooSpurt_Delay_2;
};
// Goo Spurt 3
datablock afxEffectWrapperData(IOP_Casting_GooSpurt3_EW : IOP_Casting_GooSpurt2_EW)
{
  delay  = %IOP_GooSpurt_Delay_3;
};
// Goo Spurt 4
datablock afxEffectWrapperData(IOP_Casting_GooSpurt4_EW : IOP_Casting_GooSpurt2_EW)
{
  effect = IOP_GooSpurt_Casting_SMALL_E;
  delay  = %IOP_GooSpurt_Delay_4;
};
// Goo Spurt 5
datablock afxEffectWrapperData(IOP_Casting_GooSpurt5_EW : IOP_Casting_GooSpurt4_EW)
{
  delay  = %IOP_GooSpurt_Delay_5;
};

// Goo Spurt Spray
//   -- coincides with the first, primary convulsion and spurt
datablock afxEffectWrapperData(IOP_Casting_GooSpurtSpray1_EW : IOP_Casting_GooSpurt1_EW)
{
  effect   = IOP_GooSpurt_Casting_Spray_E;
  lifetime = 0.1;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// CASTING GOO SPLATTER

//
// As the monster flies-out from the orc's belly in a rain of goo,
// the ground beneath him is covered in the green sticky, sickly
// stuff.
//
// A number of zodiacs are used here to create the effect of goo on
// the ground.  Their appearance is timed to match the seperate
// spurts of goo.
//

// Goo Splatter A Zodiac
datablock afxZodiacData(IOP_GooSplatterA1_CE : SHARED_ZodiacBase_CE)
{
  texture = %mySpellDataPath @ "/IOP/zodiacs/IOP_gooSplatterA";
  radius = 3.5;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 0.9";
};
// Goo Splatter A Zodiac -- variation 2
datablock afxZodiacData(IOP_GooSplatterA2_CE : IOP_GooSplatterA1_CE)
{
  radius = 3.9;
  startAngle = 123.0;
};
// Goo Splatter A Zodiac -- variation 3
datablock afxZodiacData(IOP_GooSplatterA3_CE : IOP_GooSplatterA1_CE)
{
  radius = 3.3;
  startAngle = -200.0;
};
// Goo Splatter A Zodiac -- variation 4
datablock afxZodiacData(IOP_GooSplatterA4_CE : IOP_GooSplatterA1_CE)
{
  radius = 6.0;
  startAngle = 189.0;
};

// Goo Splatter B Zodiac
datablock afxZodiacData(IOP_GooSplatterB1_CE : SHARED_ZodiacBase_CE)
{
  texture = %mySpellDataPath @ "/IOP/zodiacs/IOP_gooSplatterB";
  radius = 3.5;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 0.9";
};
// Goo Splatter A Zodiac -- variation 1
datablock afxZodiacData(IOP_GooSplatterB2_CE : IOP_GooSplatterB1_CE)
{
  radius = 4.2;
  startAngle = -95.0;
};
// Goo Splatter A Zodiac -- variation 2
datablock afxZodiacData(IOP_GooSplatterB3_CE : IOP_GooSplatterB1_CE)
{
  radius = 3.2;
  startAngle = 222.0;
};

// This delay variable accounts for the time the goo takes, once
//  emitted, to reach the ground:
%IOP_GooSplatter_delayOffset = 0.6;

// Goo Splatter 1
datablock afxEffectWrapperData(IOP_GooSplatter_1_EW)
{
  effect = IOP_GooSplatterA1_CE;
  posConstraint = "#effect.CastingZodeMooring";
  borrowAltitudes = true;
  delay = %IOP_GooSpurt_Delay_1+%IOP_GooSplatter_delayOffset;
  fadeInTime = 0.3;
  lifetime = 1.0;
  residueLifetime = 20;
  fadeOutTime = 5;
};
// Goo Splatter 2
datablock afxEffectWrapperData(IOP_GooSplatter_2_EW : IOP_GooSplatter_1_EW)
{
  effect = IOP_GooSplatterB1_CE;
  delay = %IOP_GooSpurt_Delay_1+%IOP_GooSplatter_delayOffset;
};
// Goo Splatter 3
datablock afxEffectWrapperData(IOP_GooSplatter_3_EW : IOP_GooSplatter_1_EW)
{
  effect = IOP_GooSplatterA4_CE;
  delay = %IOP_GooSpurt_Delay_1+%IOP_GooSplatter_delayOffset;
};
// Goo Splatter 4
datablock afxEffectWrapperData(IOP_GooSplatter_4_EW : IOP_GooSplatter_1_EW)
{
  effect = IOP_GooSplatterA2_CE;
  delay = %IOP_GooSpurt_Delay_2+%IOP_GooSplatter_delayOffset;
};
// Goo Splatter 5
datablock afxEffectWrapperData(IOP_GooSplatter_5_EW : IOP_GooSplatter_1_EW)
{
  effect = IOP_GooSplatterB2_CE;
  delay = %IOP_GooSpurt_Delay_3+%IOP_GooSplatter_delayOffset;
};
// Goo Splatter 6
datablock afxEffectWrapperData(IOP_GooSplatter_6_EW : IOP_GooSplatter_1_EW)
{
  effect = IOP_GooSplatterA3_CE;
  delay = %IOP_GooSpurt_Delay_4+%IOP_GooSplatter_delayOffset;
};
// Goo Splatter 7
datablock afxEffectWrapperData(IOP_GooSplatter_7_EW : IOP_GooSplatter_1_EW)
{
  effect = IOP_GooSplatterB3_CE;
  delay = %IOP_GooSpurt_Delay_5+%IOP_GooSplatter_delayOffset;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GIANT INSECT

//
// Out from the belly of the orc shoots a mighty insect, a flying
// one-eyed centipede-like creature full of fire and desire to kill
// its target.
//
// The insect is made primarily of individual segments, all dts
// objects, that are constrained to the missile.  They appear in
// sequence from near the orc's chest.  Their curling, twisting
// motion comes from two sources: the motion of the missile, which
// can "wiggle" (see "GIANT INSECT MISSILE" below), and the use of
// constraint history.
//
// Constraint history works by storing the motion of a source 
// constraint so that constrained objects can refer to it, and thus
// be where the source was some time ago.  Constrained objects 
// "ghost" the constraint, seeming to follow it.  With history on, as
// the missile wiggles back and forth, instead of the entire insect
// instantaneously inheriting that motion, each segment takes it
// from a different time in the past, and the wiggle becomes a wave
// through the entire insect.  The time in the past each segment uses
// is equal to its delay, which, from the head to the tail, increases
// linearly: the head delay is 0, the next segment 0+x, the next
// 0+2x, etc.  The constraint is set with:
//     constraint = "missile.#history(0.08/60)";
// The first number is a positive float that defines how far back to
// go in the past; the second (optional) integer is a sampling number,
// and the higher it is the smoother the motion of the constrained 
// object will be.
//
// The insect segments are arranged as follows:
//   segment 0 -- Head and Eye/Jaws (IOP_GiantInsectHead_CE
//                                   IOP_GiantInsectEyeJaws_CE)
//           1 -- Large Wings       (IOP_GiantInsectWingsLarge_CE)
//           2 -- Legs A            (IOP_GiantInsectLegsA_CE)
//           3 -- Legs B            (IOP_GiantInsectLegsB_CE)
//           4 -- Legs C            (IOP_GiantInsectLegsC_CE)
//           5 -- Wings             (IOP_GiantInsectWings_CE)
//           6 -- Default Segment   (IOP_GiantInsectSegment_CE)
//           7 -- Default Segment   (IOP_GiantInsectSegment_CE)
//           8 -- Wings             (IOP_GiantInsectWings_CE)
//           9 -- Default Segment   (IOP_GiantInsectSegment_CE)
//          10 -- Default Segment   (IOP_GiantInsectSegment_CE)
//          11 -- Wings             (IOP_GiantInsectWings_CE)
//          12 -- Default Segment   (IOP_GiantInsectSegment_CE)
//          13 -- Default Segment   (IOP_GiantInsectSegment_CE)
//          14 -- Wings             (IOP_GiantInsectWings_CE)
//          15 -- Default Segment   (IOP_GiantInsectSegment_CE)
//          16 -- Default Segment   (IOP_GiantInsectSegment_CE)
//          17 -- Wings             (IOP_GiantInsectWings_CE)
//          18 -- Default Segment   (IOP_GiantInsectSegment_CE)
//          19 -- Default Segment   (IOP_GiantInsectSegment_CE)
//          20 -- Tail              (IOP_GiantInsectTail_CE)
//
// In addition to these dts segments, particle effects complete the 
// giant insect (see "GIANT INSECT MAGIC (FIRE)" below).
//

// Giant Insect Model
//   -- the insect is made up of segments using the following models:
//        1 head
//        1 animated and self-illuminating jaw/eye
//        1 large wings segment, animated
//        3 unique leg segments, A, B and C
//        5 wing segments, animated
//        10 normal segments
//        1 tail segment
//   -- all the models are Additive for visual clarity
//
// Head
datablock afxModelData(IOP_GiantInsectHead_CE)
{
  shapeFile = %mySpellDataPath @ "/IOP/models/IOP_insectHead.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive;
};
// Eye/Jaw
datablock afxModelData(IOP_GiantInsectEyeJaws_CE)
{
  shapeFile = %mySpellDataPath @ "/IOP/models/IOP_insectEyeJaws.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  sequence = "bite";
  sequenceRate = 1.0;
  remapTextureTags = "IOP_insectHead.png:IOP_insectEye IOP_insectHead.png:IOP_insectJaws"; // TGEA (ignored by TGE)
};
// Segment
datablock afxModelData(IOP_GiantInsectSegment_CE)
{
  shapeFile = %mySpellDataPath @ "/IOP/models/IOP_insectSegment.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive;
};
// Wing Segment
datablock afxModelData(IOP_GiantInsectWings_CE)
{
  shapeFile = %mySpellDataPath @ "/IOP/models/IOP_insectWings.dts";
  sequence = "flap";
  sequenceRate = 10.0;
  forceOnMaterialFlags = $MaterialFlags::Additive;
};
// Large Wing Segment
datablock afxModelData(IOP_GiantInsectWingsLarge_CE)
{
  shapeFile = %mySpellDataPath @ "/IOP/models/IOP_insectWingsLarge.dts";
  sequence = "flap";
  sequenceRate = 10.0;
  forceOnMaterialFlags = $MaterialFlags::Additive;
};
// Leg Segment A
datablock afxModelData(IOP_GiantInsectLegsA_CE)
{
  shapeFile = %mySpellDataPath @ "/IOP/models/IOP_insectLegs_A.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive;
};
// Leg Segment B
datablock afxModelData(IOP_GiantInsectLegsB_CE)
{
  shapeFile = %mySpellDataPath @ "/IOP/models/IOP_insectLegs_B.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive;
};
// Leg Segment C
datablock afxModelData(IOP_GiantInsectLegsC_CE)
{
  shapeFile = %mySpellDataPath @ "/IOP/models/IOP_insectLegs_C.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive;
};
// Tail Segment
datablock afxModelData(IOP_GiantInsectTail_CE)
{
  shapeFile = %mySpellDataPath @ "/IOP/models/IOP_insectTail.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive;
};

// Gaint Insect Delay Variables
//   -- the segments are created in sequence
//   -- thus %IOP_GiantInsectSegment_Delay essentially controls the
//       distance between segments
//
%IOP_GiantInsectSegment_Delay = 0.08;
//
%IOP_GiantInsectSegment_1_Delay  = %IOP_GiantInsectSegment_Delay * 1;
%IOP_GiantInsectSegment_2_Delay  = %IOP_GiantInsectSegment_Delay * 2;
%IOP_GiantInsectSegment_3_Delay  = %IOP_GiantInsectSegment_Delay * 3;
%IOP_GiantInsectSegment_4_Delay  = %IOP_GiantInsectSegment_Delay * 4;
%IOP_GiantInsectSegment_5_Delay  = %IOP_GiantInsectSegment_Delay * 5;
%IOP_GiantInsectSegment_6_Delay  = %IOP_GiantInsectSegment_Delay * 6;
%IOP_GiantInsectSegment_7_Delay  = %IOP_GiantInsectSegment_Delay * 7;
%IOP_GiantInsectSegment_8_Delay  = %IOP_GiantInsectSegment_Delay * 8;
%IOP_GiantInsectSegment_9_Delay  = %IOP_GiantInsectSegment_Delay * 9;
%IOP_GiantInsectSegment_10_Delay = %IOP_GiantInsectSegment_Delay * 10;
%IOP_GiantInsectSegment_11_Delay = %IOP_GiantInsectSegment_Delay * 11;
%IOP_GiantInsectSegment_12_Delay = %IOP_GiantInsectSegment_Delay * 12;
%IOP_GiantInsectSegment_13_Delay = %IOP_GiantInsectSegment_Delay * 13;
%IOP_GiantInsectSegment_14_Delay = %IOP_GiantInsectSegment_Delay * 14;
%IOP_GiantInsectSegment_15_Delay = %IOP_GiantInsectSegment_Delay * 15;
%IOP_GiantInsectSegment_16_Delay = %IOP_GiantInsectSegment_Delay * 16;
%IOP_GiantInsectSegment_17_Delay = %IOP_GiantInsectSegment_Delay * 17;
%IOP_GiantInsectSegment_18_Delay = %IOP_GiantInsectSegment_Delay * 18;
%IOP_GiantInsectSegment_19_Delay = %IOP_GiantInsectSegment_Delay * 19;
%IOP_GiantInsectSegment_20_Delay = %IOP_GiantInsectSegment_Delay * 20;

// Segment 0 -- Head
datablock afxEffectWrapperData(IOP_GiantInsectHead_EW)
{
  effect = IOP_GiantInsectHead_CE;

  constraint = missile;

  delay = 0;
  fadeInTime  = 0.25;
  fadeOutTime = 0.25;
};
// Segment 0 -- Eye and Jaws
datablock afxEffectWrapperData(IOP_GiantInsectEyeJaws_EW)
{
  effect = IOP_GiantInsectEyeJaws_CE;
  effectName = "InsectEyeJaws";
  isConstraintSrc = true;

  constraint = missile;

  delay = 0;
  fadeInTime  = 0.25;
  fadeOutTime = 0.25;
};
// Segment 1 -- Large Wings
datablock afxEffectWrapperData(IOP_GiantInsectSegment_1_EW)
{
  effect = IOP_GiantInsectWingsLarge_CE;
  // NOTE: a sample-rate of 60 is set here the missile constraint history.
  // At least one effect needs to set the sample-rate for values other than 30.
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_1_Delay @ "/60)";
  delay       = %IOP_GiantInsectSegment_1_Delay;
  fadeInTime  = 0.25;
  fadeOutTime = 0.25;
};
// Segment 2 -- Legs A
datablock afxEffectWrapperData(IOP_GiantInsectSegment_2_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectLegsA_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_2_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_2_Delay;  
};
// Segment 3 -- Legs B
datablock afxEffectWrapperData(IOP_GiantInsectSegment_3_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectLegsB_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_3_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_3_Delay;
};
// Segment 4 -- Legs C
datablock afxEffectWrapperData(IOP_GiantInsectSegment_4_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectLegsC_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_4_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_4_Delay;
};
// Segment 5 -- Wings
datablock afxEffectWrapperData(IOP_GiantInsectSegment_5_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectWings_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_5_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_5_Delay;
};
// Segment 6 -- Default Segment
datablock afxEffectWrapperData(IOP_GiantInsectSegment_6_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectSegment_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_6_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_6_Delay;
};
// Segment 7 -- Default Segment
datablock afxEffectWrapperData(IOP_GiantInsectSegment_7_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectSegment_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_7_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_7_Delay;
};
// Segment 8 -- Wings
datablock afxEffectWrapperData(IOP_GiantInsectSegment_8_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectWings_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_8_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_8_Delay;
};
// Segment 9 -- Default Segment
datablock afxEffectWrapperData(IOP_GiantInsectSegment_9_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectSegment_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_9_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_9_Delay;
};
// Segment 10 -- Default Segment
datablock afxEffectWrapperData(IOP_GiantInsectSegment_10_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectSegment_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_10_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_10_Delay;
};
// Segment 11 -- Wings
datablock afxEffectWrapperData(IOP_GiantInsectSegment_11_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectWings_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_11_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_11_Delay;
};
// Segment 12 -- Default Segment
datablock afxEffectWrapperData(IOP_GiantInsectSegment_12_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectSegment_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_12_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_12_Delay;
};
// Segment 13 -- Default Segment
datablock afxEffectWrapperData(IOP_GiantInsectSegment_13_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectSegment_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_13_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_13_Delay;
};
// Segment 14 -- Wings
datablock afxEffectWrapperData(IOP_GiantInsectSegment_14_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectWings_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_14_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_14_Delay;
};
// Segment 15 -- Default Segment
datablock afxEffectWrapperData(IOP_GiantInsectSegment_15_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectSegment_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_15_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_15_Delay;
};
// Segment 16 -- Default Segment
datablock afxEffectWrapperData(IOP_GiantInsectSegment_16_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectSegment_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_16_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_16_Delay;
};
// Segment 17 -- Wings
datablock afxEffectWrapperData(IOP_GiantInsectSegment_17_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectWings_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_17_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_17_Delay;
};
// Segment 18 -- Default Segment
datablock afxEffectWrapperData(IOP_GiantInsectSegment_18_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectSegment_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_18_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_18_Delay;
};
// Segment 19 -- Default Segment
datablock afxEffectWrapperData(IOP_GiantInsectSegment_19_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectSegment_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_19_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_19_Delay;
};
// Segment 20 -- Tail
datablock afxEffectWrapperData(IOP_GiantInsectSegment_20_EW : IOP_GiantInsectSegment_1_EW)
{
  effect      = IOP_GiantInsectTail_CE;
  constraint  = "missile.#history(" @ %IOP_GiantInsectSegment_20_Delay @ ")";
  delay       = %IOP_GiantInsectSegment_20_Delay;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GIANT INSECT MAGIC (FIRE)

//
// To give the giant insect volume and to add to its terrifying and
// magical qualities, particles are emitted between each insect 
// segment.  There are 19 emitters in total (being none between the
// last segment and the tail).  To place these emitters correctly,
// the same constraint history mechanism is used as explained above
// (see "GIANT INSECT").
//
// Unfortunately, because the size of the particles decreases for
// each emitter along the length of the insect, giving the whole form
// a snake-like shape, a large number of datablocks are required.
//
// Originally intended as a generic "magic", they immediately took
// on an attractive fiery appearance.  Magic or fire -- it's all
// good.
//

// Magic Particle Size Variables
//   -- the fire particles decrease linearly in size, so that the
//       largest particles are at the front of the monster
//
%IOP_GiantInsectMagic_1_SizeShrinkFactor = 0.10;
//
%IOP_GiantInsectMagic_1_Size  = 3.0;
%IOP_GiantInsectMagic_2_Size  = %IOP_GiantInsectMagic_1_Size-(1*%IOP_GiantInsectMagic_1_SizeShrinkFactor);
%IOP_GiantInsectMagic_3_Size  = %IOP_GiantInsectMagic_1_Size-(2*%IOP_GiantInsectMagic_1_SizeShrinkFactor);
%IOP_GiantInsectMagic_4_Size  = %IOP_GiantInsectMagic_1_Size-(3*%IOP_GiantInsectMagic_1_SizeShrinkFactor);
%IOP_GiantInsectMagic_5_Size  = %IOP_GiantInsectMagic_1_Size-(4*%IOP_GiantInsectMagic_1_SizeShrinkFactor);
%IOP_GiantInsectMagic_6_Size  = %IOP_GiantInsectMagic_1_Size-(5*%IOP_GiantInsectMagic_1_SizeShrinkFactor);
%IOP_GiantInsectMagic_7_Size  = %IOP_GiantInsectMagic_1_Size-(6*%IOP_GiantInsectMagic_1_SizeShrinkFactor);
%IOP_GiantInsectMagic_8_Size  = %IOP_GiantInsectMagic_1_Size-(7*%IOP_GiantInsectMagic_1_SizeShrinkFactor);
%IOP_GiantInsectMagic_9_Size  = %IOP_GiantInsectMagic_1_Size-(8*%IOP_GiantInsectMagic_1_SizeShrinkFactor);
%IOP_GiantInsectMagic_10_Size = %IOP_GiantInsectMagic_1_Size-(9*%IOP_GiantInsectMagic_1_SizeShrinkFactor);
%IOP_GiantInsectMagic_11_Size = %IOP_GiantInsectMagic_1_Size-(10*%IOP_GiantInsectMagic_1_SizeShrinkFactor);
%IOP_GiantInsectMagic_12_Size = %IOP_GiantInsectMagic_1_Size-(11*%IOP_GiantInsectMagic_1_SizeShrinkFactor);
%IOP_GiantInsectMagic_13_Size = %IOP_GiantInsectMagic_1_Size-(12*%IOP_GiantInsectMagic_1_SizeShrinkFactor);
%IOP_GiantInsectMagic_14_Size = %IOP_GiantInsectMagic_1_Size-(13*%IOP_GiantInsectMagic_1_SizeShrinkFactor);
%IOP_GiantInsectMagic_15_Size = %IOP_GiantInsectMagic_1_Size-(14*%IOP_GiantInsectMagic_1_SizeShrinkFactor);
%IOP_GiantInsectMagic_16_Size = %IOP_GiantInsectMagic_1_Size-(15*%IOP_GiantInsectMagic_1_SizeShrinkFactor);
%IOP_GiantInsectMagic_17_Size = %IOP_GiantInsectMagic_1_Size-(16*%IOP_GiantInsectMagic_1_SizeShrinkFactor);
%IOP_GiantInsectMagic_18_Size = %IOP_GiantInsectMagic_1_Size-(17*%IOP_GiantInsectMagic_1_SizeShrinkFactor);
%IOP_GiantInsectMagic_19_Size = %IOP_GiantInsectMagic_1_Size-(18*%IOP_GiantInsectMagic_1_SizeShrinkFactor);

// Magic Timing Variables
//   -- these delay variables place the particles between insect
//       segments
//
%IOP_GiantInsectMagic_1_Delay  = 0+(%IOP_GiantInsectSegment_Delay*0.5*0.5);
%IOP_GiantInsectMagic_2_Delay  = %IOP_GiantInsectSegment_1_Delay+(%IOP_GiantInsectSegment_Delay*0.5);
%IOP_GiantInsectMagic_3_Delay  = %IOP_GiantInsectSegment_2_Delay+(%IOP_GiantInsectSegment_Delay*0.5);
%IOP_GiantInsectMagic_4_Delay  = %IOP_GiantInsectSegment_3_Delay+(%IOP_GiantInsectSegment_Delay*0.5);
%IOP_GiantInsectMagic_5_Delay  = %IOP_GiantInsectSegment_4_Delay+(%IOP_GiantInsectSegment_Delay*0.5);
%IOP_GiantInsectMagic_6_Delay  = %IOP_GiantInsectSegment_5_Delay+(%IOP_GiantInsectSegment_Delay*0.5);
%IOP_GiantInsectMagic_7_Delay  = %IOP_GiantInsectSegment_6_Delay+(%IOP_GiantInsectSegment_Delay*0.5);
%IOP_GiantInsectMagic_8_Delay  = %IOP_GiantInsectSegment_7_Delay+(%IOP_GiantInsectSegment_Delay*0.5);
%IOP_GiantInsectMagic_9_Delay  = %IOP_GiantInsectSegment_8_Delay+(%IOP_GiantInsectSegment_Delay*0.5);
%IOP_GiantInsectMagic_10_Delay = %IOP_GiantInsectSegment_9_Delay+(%IOP_GiantInsectSegment_Delay*0.5);
%IOP_GiantInsectMagic_11_Delay = %IOP_GiantInsectSegment_10_Delay+(%IOP_GiantInsectSegment_Delay*0.5);
%IOP_GiantInsectMagic_12_Delay = %IOP_GiantInsectSegment_11_Delay+(%IOP_GiantInsectSegment_Delay*0.5);
%IOP_GiantInsectMagic_13_Delay = %IOP_GiantInsectSegment_12_Delay+(%IOP_GiantInsectSegment_Delay*0.5);
%IOP_GiantInsectMagic_14_Delay = %IOP_GiantInsectSegment_13_Delay+(%IOP_GiantInsectSegment_Delay*0.5);
%IOP_GiantInsectMagic_15_Delay = %IOP_GiantInsectSegment_14_Delay+(%IOP_GiantInsectSegment_Delay*0.5);
%IOP_GiantInsectMagic_16_Delay = %IOP_GiantInsectSegment_15_Delay+(%IOP_GiantInsectSegment_Delay*0.5);
%IOP_GiantInsectMagic_17_Delay = %IOP_GiantInsectSegment_16_Delay+(%IOP_GiantInsectSegment_Delay*0.5);
%IOP_GiantInsectMagic_18_Delay = %IOP_GiantInsectSegment_17_Delay+(%IOP_GiantInsectSegment_Delay*0.5);
%IOP_GiantInsectMagic_19_Delay = %IOP_GiantInsectSegment_18_Delay+(%IOP_GiantInsectSegment_Delay*0.5);


// Magic Particles
//
datablock ParticleData(IOP_InsectMagicA1_P)
{
  textureName        = %mySpellDataPath @ "/IOP/particles/IOP_insectMagicA";
  dragCoeffiecient   = 0.5;
  gravityCoefficient = 0.2;
  inheritedVelFactor = 0.00;
  lifetimeMS         = 250;
  lifetimeVarianceMS = 50;
  useInvAlpha        = false;
  spinRandomMin      = -360.0;
  spinRandomMax      = 360.0;
  colors[0]          = "0.4 0.4 0.4 0.4";
  colors[1]          = "0.2 0.2 0.2 0.0";
  sizes[0]           = 2.0;
  sizes[1]           = 1.0;
  times[0]           = 0.9;
  times[1]           = 1.0;
};
datablock ParticleData(IOP_InsectMagicA2_P : IOP_InsectMagicA1_P)
{
  sizes[0]           = %IOP_GiantInsectMagic_2_Size;
  sizes[1]           = %IOP_GiantInsectMagic_2_Size*0.5;
};
datablock ParticleData(IOP_InsectMagicA3_P : IOP_InsectMagicA1_P)
{
  sizes[0]           = %IOP_GiantInsectMagic_3_Size;
  sizes[1]           = %IOP_GiantInsectMagic_3_Size*0.5;
};
datablock ParticleData(IOP_InsectMagicA4_P : IOP_InsectMagicA1_P)
{
  sizes[0]           = %IOP_GiantInsectMagic_4_Size;
  sizes[1]           = %IOP_GiantInsectMagic_4_Size*0.5;
};
datablock ParticleData(IOP_InsectMagicA5_P : IOP_InsectMagicA1_P)
{
  sizes[0]           = %IOP_GiantInsectMagic_5_Size;
  sizes[1]           = %IOP_GiantInsectMagic_5_Size*0.5;
};
datablock ParticleData(IOP_InsectMagicA6_P : IOP_InsectMagicA1_P)
{
  sizes[0]           = %IOP_GiantInsectMagic_6_Size;
  sizes[1]           = %IOP_GiantInsectMagic_6_Size*0.5;
};
datablock ParticleData(IOP_InsectMagicA7_P : IOP_InsectMagicA1_P)
{
  sizes[0]           = %IOP_GiantInsectMagic_7_Size;
  sizes[1]           = %IOP_GiantInsectMagic_7_Size*0.5;
};
datablock ParticleData(IOP_InsectMagicA8_P : IOP_InsectMagicA1_P)
{
  sizes[0]           = %IOP_GiantInsectMagic_8_Size;
  sizes[1]           = %IOP_GiantInsectMagic_8_Size*0.5;
};
datablock ParticleData(IOP_InsectMagicA9_P : IOP_InsectMagicA1_P)
{
  sizes[0]           = %IOP_GiantInsectMagic_9_Size;
  sizes[1]           = %IOP_GiantInsectMagic_9_Size*0.5;
};
datablock ParticleData(IOP_InsectMagicA10_P : IOP_InsectMagicA1_P)
{
  sizes[0]           = %IOP_GiantInsectMagic_10_Size;
  sizes[1]           = %IOP_GiantInsectMagic_10_Size*0.5;
};
datablock ParticleData(IOP_InsectMagicA11_P : IOP_InsectMagicA1_P)
{
  sizes[0]           = %IOP_GiantInsectMagic_11_Size;
  sizes[1]           = %IOP_GiantInsectMagic_11_Size*0.5;
};
datablock ParticleData(IOP_InsectMagicA12_P : IOP_InsectMagicA1_P)
{
  sizes[0]           = %IOP_GiantInsectMagic_12_Size;
  sizes[1]           = %IOP_GiantInsectMagic_12_Size*0.5;
};
datablock ParticleData(IOP_InsectMagicA13_P : IOP_InsectMagicA1_P)
{
  sizes[0]           = %IOP_GiantInsectMagic_13_Size;
  sizes[1]           = %IOP_GiantInsectMagic_13_Size*0.5;
};
datablock ParticleData(IOP_InsectMagicA14_P : IOP_InsectMagicA1_P)
{
  sizes[0]           = %IOP_GiantInsectMagic_14_Size;
  sizes[1]           = %IOP_GiantInsectMagic_14_Size*0.5;
};
datablock ParticleData(IOP_InsectMagicA15_P : IOP_InsectMagicA1_P)
{
  sizes[0]           = %IOP_GiantInsectMagic_15_Size;
  sizes[1]           = %IOP_GiantInsectMagic_15_Size*0.5;
};
datablock ParticleData(IOP_InsectMagicA16_P : IOP_InsectMagicA1_P)
{
  sizes[0]           = %IOP_GiantInsectMagic_16_Size;
  sizes[1]           = %IOP_GiantInsectMagic_16_Size*0.5;
};
datablock ParticleData(IOP_InsectMagicA17_P : IOP_InsectMagicA1_P)
{
  sizes[0]           = %IOP_GiantInsectMagic_17_Size;
  sizes[1]           = %IOP_GiantInsectMagic_17_Size*0.5;
};
datablock ParticleData(IOP_InsectMagicA18_P : IOP_InsectMagicA1_P)
{
  sizes[0]           = %IOP_GiantInsectMagic_18_Size;
  sizes[1]           = %IOP_GiantInsectMagic_18_Size*0.5;
};
datablock ParticleData(IOP_InsectMagicA19_P : IOP_InsectMagicA1_P)
{
  sizes[0]           = %IOP_GiantInsectMagic_19_Size;
  sizes[1]           = %IOP_GiantInsectMagic_19_Size*0.5;
};

// Magic Emitters
//
datablock ParticleEmitterData(IOP_InsectMagicA1_E)
{
  ejectionOffset        = 0.02;
  ejectionPeriodMS      = 5;
  periodVarianceMS      = 0;
  ejectionVelocity      = 1.0;
  velocityVariance      = 0.3;
  thetaMin              = 0.0;
  thetaMax              = 180.0;
  particles             = IOP_InsectMagicA1_P;
  overrideAdvance       = false;
};
datablock ParticleEmitterData(IOP_InsectMagicA2_E :IOP_InsectMagicA1_E)
{
  particles             = IOP_InsectMagicA2_P;
};
datablock ParticleEmitterData(IOP_InsectMagicA3_E :IOP_InsectMagicA1_E)
{
  particles             = IOP_InsectMagicA3_P;
};
datablock ParticleEmitterData(IOP_InsectMagicA4_E :IOP_InsectMagicA1_E)
{
  particles             = IOP_InsectMagicA4_P;
};
datablock ParticleEmitterData(IOP_InsectMagicA5_E :IOP_InsectMagicA1_E)
{
  particles             = IOP_InsectMagicA5_P;
};
datablock ParticleEmitterData(IOP_InsectMagicA6_E :IOP_InsectMagicA1_E)
{
  particles             = IOP_InsectMagicA6_P;
};
datablock ParticleEmitterData(IOP_InsectMagicA7_E :IOP_InsectMagicA1_E)
{
  particles             = IOP_InsectMagicA7_P;
};
datablock ParticleEmitterData(IOP_InsectMagicA8_E :IOP_InsectMagicA1_E)
{
  particles             = IOP_InsectMagicA8_P;
};
datablock ParticleEmitterData(IOP_InsectMagicA9_E :IOP_InsectMagicA1_E)
{
  particles             = IOP_InsectMagicA9_P;
};
datablock ParticleEmitterData(IOP_InsectMagicA10_E :IOP_InsectMagicA1_E)
{
  particles             = IOP_InsectMagicA10_P;
};
datablock ParticleEmitterData(IOP_InsectMagicA11_E :IOP_InsectMagicA1_E)
{
  particles             = IOP_InsectMagicA11_P;
};
datablock ParticleEmitterData(IOP_InsectMagicA12_E :IOP_InsectMagicA1_E)
{
  particles             = IOP_InsectMagicA12_P;
};
datablock ParticleEmitterData(IOP_InsectMagicA13_E :IOP_InsectMagicA1_E)
{
  particles             = IOP_InsectMagicA13_P;
};
datablock ParticleEmitterData(IOP_InsectMagicA14_E :IOP_InsectMagicA1_E)
{
  particles             = IOP_InsectMagicA14_P;
};
datablock ParticleEmitterData(IOP_InsectMagicA15_E :IOP_InsectMagicA1_E)
{
  particles             = IOP_InsectMagicA15_P;
};
datablock ParticleEmitterData(IOP_InsectMagicA16_E :IOP_InsectMagicA1_E)
{
  particles             = IOP_InsectMagicA16_P;
};
datablock ParticleEmitterData(IOP_InsectMagicA17_E :IOP_InsectMagicA1_E)
{
  particles             = IOP_InsectMagicA17_P;
};
datablock ParticleEmitterData(IOP_InsectMagicA18_E :IOP_InsectMagicA1_E)
{
  particles             = IOP_InsectMagicA18_P;
};
datablock ParticleEmitterData(IOP_InsectMagicA19_E :IOP_InsectMagicA1_E)
{
  particles             = IOP_InsectMagicA19_P;
};

// Insect Magic 1
datablock afxEffectWrapperData(IOP_GiantInsectMagic_1_EW)
{
  effect          = IOP_InsectMagicA1_E;
  posConstraint   = "missile.#history(" @ %IOP_GiantInsectMagic_1_Delay @ ")";
  delay           = %IOP_GiantInsectMagic_1_Delay;
};
// Insect Magic 2
datablock afxEffectWrapperData(IOP_GiantInsectMagic_2_EW : IOP_GiantInsectMagic_1_EW)
{
  effect        = IOP_InsectMagicA2_E;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_2_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_2_Delay;
};
// Insect Magic 3
datablock afxEffectWrapperData(IOP_GiantInsectMagic_3_EW : IOP_GiantInsectMagic_1_EW)
{
  effect        = IOP_InsectMagicA3_E;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_3_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_3_Delay;
};
// Insect Magic 4
datablock afxEffectWrapperData(IOP_GiantInsectMagic_4_EW : IOP_GiantInsectMagic_1_EW)
{
  effect        = IOP_InsectMagicA4_E;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_4_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_4_Delay;
};
// Insect Magic 5
datablock afxEffectWrapperData(IOP_GiantInsectMagic_5_EW : IOP_GiantInsectMagic_1_EW)
{
  effect        = IOP_InsectMagicA5_E;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_5_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_5_Delay;
};
// Insect Magic 6
datablock afxEffectWrapperData(IOP_GiantInsectMagic_6_EW : IOP_GiantInsectMagic_1_EW)
{
  effect        = IOP_InsectMagicA6_E;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_6_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_6_Delay;
};
// Insect Magic 7
datablock afxEffectWrapperData(IOP_GiantInsectMagic_7_EW : IOP_GiantInsectMagic_1_EW)
{
  effect        = IOP_InsectMagicA7_E;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_7_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_7_Delay;
};
// Insect Magic 8
datablock afxEffectWrapperData(IOP_GiantInsectMagic_8_EW : IOP_GiantInsectMagic_1_EW)
{
  effect        = IOP_InsectMagicA8_E;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_8_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_8_Delay;
};
// Insect Magic 9
datablock afxEffectWrapperData(IOP_GiantInsectMagic_9_EW : IOP_GiantInsectMagic_1_EW)
{
  effect        = IOP_InsectMagicA9_E;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_9_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_9_Delay;
};
// Insect Magic 10
datablock afxEffectWrapperData(IOP_GiantInsectMagic_10_EW : IOP_GiantInsectMagic_1_EW)
{
  effect        = IOP_InsectMagicA10_E;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_10_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_10_Delay;
};
// Insect Magic 11
datablock afxEffectWrapperData(IOP_GiantInsectMagic_11_EW : IOP_GiantInsectMagic_1_EW)
{
  effect        = IOP_InsectMagicA11_E;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_11_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_11_Delay;
};
// Insect Magic 12
datablock afxEffectWrapperData(IOP_GiantInsectMagic_12_EW : IOP_GiantInsectMagic_1_EW)
{
  effect        = IOP_InsectMagicA12_E;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_12_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_12_Delay;
};
// Insect Magic 13
datablock afxEffectWrapperData(IOP_GiantInsectMagic_13_EW : IOP_GiantInsectMagic_1_EW)
{
  effect        = IOP_InsectMagicA13_E;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_13_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_13_Delay;
};
// Insect Magic 14
datablock afxEffectWrapperData(IOP_GiantInsectMagic_14_EW : IOP_GiantInsectMagic_1_EW)
{
  effect        = IOP_InsectMagicA14_E;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_14_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_14_Delay;
};
// Insect Magic 15
datablock afxEffectWrapperData(IOP_GiantInsectMagic_15_EW : IOP_GiantInsectMagic_1_EW)
{
  effect        = IOP_InsectMagicA15_E;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_15_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_15_Delay;
};
// Insect Magic 16
datablock afxEffectWrapperData(IOP_GiantInsectMagic_16_EW : IOP_GiantInsectMagic_1_EW)
{
  effect        = IOP_InsectMagicA16_E;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_16_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_16_Delay;
};
// Insect Magic 17
datablock afxEffectWrapperData(IOP_GiantInsectMagic_17_EW : IOP_GiantInsectMagic_1_EW)
{
  effect        = IOP_InsectMagicA17_E;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_17_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_17_Delay;
};
// Insect Magic 18
datablock afxEffectWrapperData(IOP_GiantInsectMagic_18_EW : IOP_GiantInsectMagic_1_EW)
{
  effect        = IOP_InsectMagicA18_E;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_18_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_18_Delay;
};
// Insect Magic 19
datablock afxEffectWrapperData(IOP_GiantInsectMagic_19_EW : IOP_GiantInsectMagic_1_EW)
{
  effect        = IOP_InsectMagicA19_E;
  posConstraint = "missile.#history(" @ %IOP_GiantInsectMagic_19_Delay @ ")";
  delay         = %IOP_GiantInsectMagic_19_Delay;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// IMPACT MAGIC

//
// When the giant insect strikes its target, each segment explodes in
// fire magic and goo.
//
// Three variations of magic particle are used, differntly sized, to
// add randomness to the effect.  Each corresponds to and is timed to
// match one of the fiery segments of the insect (see
// "GIANT INSECT MAGIC (FIRE)").
//

// Magic Impact Particle -- small size
datablock ParticleData(IOP_InsectMagicImpact_small_P)
{
  textureName          = %mySpellDataPath @ "/IOP/particles/IOP_insectMagicA";
  dragCoeffiecient     = 0.5;
  gravityCoefficient   = 0.2;
  inheritedVelFactor   = 0.00;
  lifetimeMS           = 250;
  lifetimeVarianceMS   = 50;
  useInvAlpha          = false;
  spinRandomMin        = -360.0;
  spinRandomMax        = 360.0;
  colors[0]            = "0.4 0.4 0.4 0.4";
  colors[1]            = "0.2 0.2 0.2 0.0";
  sizes[0]             = 2.0;
  sizes[1]             = 4.0;
  times[0]             = 0.9;
  times[1]             = 1.0;
};
// Magic Impact Particle -- medium size
datablock ParticleData(IOP_InsectMagicImpact_medium_P : IOP_InsectMagicImpact_small_P)
{
  sizes[0]             = 3.0;
  sizes[1]             = 7.0;
};
// Magic Impact Particle -- large size
datablock ParticleData(IOP_InsectMagicImpact_large_P : IOP_InsectMagicImpact_small_P)
{ 
  sizes[0]             = 5.0;
  sizes[1]             = 15.0;
};

// Magic Impact Emitter -- small (default "sprinkler" type)
datablock ParticleEmitterData(IOP_InsectMagicImpact_small_E)
{
  ejectionOffset   = 0.02;
  ejectionPeriodMS = 10;
  periodVarianceMS = 0;
  ejectionVelocity = 1.0;
  velocityVariance = 0.3;
  thetaMin         = 0.0;
  thetaMax         = 180.0;
  particles        = IOP_InsectMagicImpact_small_P;
};
// Magic Impact Emitter -- medium
datablock ParticleEmitterData(IOP_InsectMagicImpact_medium_E : IOP_InsectMagicImpact_small_E)
{
  particles        = IOP_InsectMagicImpact_medium_P;
};
// Magic Impact Emitter -- large
datablock ParticleEmitterData(IOP_InsectMagicImpact_large_E : IOP_InsectMagicImpact_small_E)
{
  particles        = IOP_InsectMagicImpact_large_P;
};

// Impact Magic 1
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_large_E;

  delay = %IOP_GiantInsectMagic_1_Delay;
  constraint = "impactedObject.Bip01 Spine1";
  delay    = 0.0;
  lifetime = 0.2;
  execConditions[0] = $AFX::IMPACTED_SOMETHING;
};
// Impact Magic 2
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_2_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_small_E;
  delay  = %IOP_GiantInsectMagic_2_Delay;
};
// Impact Magic 3
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_3_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_medium_E;
  delay  = %IOP_GiantInsectMagic_3_Delay;
};
// Impact Magic 4
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_4_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_small_E;
  delay  = %IOP_GiantInsectMagic_4_Delay;
};
// Impact Magic 5
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_5_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_large_E;
  delay  = %IOP_GiantInsectMagic_5_Delay;
};
// Impact Magic 6
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_6_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_medium_E;
  delay  = %IOP_GiantInsectMagic_6_Delay;
};
// Impact Magic 7
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_7_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_small_E;
  delay  = %IOP_GiantInsectMagic_7_Delay;
};
// Impact Magic 8
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_8_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_small_E;
  delay  = %IOP_GiantInsectMagic_8_Delay;
};
// Impact Magic 9
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_9_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_medium_E;
  delay  = %IOP_GiantInsectMagic_9_Delay;
};
// Impact Magic 10
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_10_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_large_E;
  delay  = %IOP_GiantInsectMagic_10_Delay;
};
// Impact Magic 11
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_11_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_small_E;
  delay  = %IOP_GiantInsectMagic_11_Delay;
};
// Impact Magic 12
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_12_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_large_E;
  delay  = %IOP_GiantInsectMagic_12_Delay;
};
// Impact Magic 13
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_13_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_medium_E;
  delay  = %IOP_GiantInsectMagic_13_Delay;
};
// Impact Magic 14
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_14_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_large_E;
  delay  = %IOP_GiantInsectMagic_14_Delay;
};
// Impact Magic 15
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_15_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_medium_E;
  delay  = %IOP_GiantInsectMagic_15_Delay;
};
// Impact Magic 16
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_16_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_large_E;
  delay  = %IOP_GiantInsectMagic_16_Delay;
};
// Impact Magic 17
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_17_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_large_E;
  delay  = %IOP_GiantInsectMagic_17_Delay;
};
// Impact Magic 18
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_18_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_medium_E;
  delay  = %IOP_GiantInsectMagic_18_Delay;
};
// Impact Magic 19
datablock afxEffectWrapperData(IOP_GiantInsectMagicImpact_19_EW : IOP_GiantInsectMagicImpact_1_EW)
{
  effect = IOP_InsectMagicImpact_small_E;
  delay  = %IOP_GiantInsectMagic_19_Delay;
};




//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// IMPACT GOO

//
// When the giant insect strikes its target, each segment explodes in
// fire magic and goo.
//
// The goo particles are reused from above (see "CASTING GOO SPURT").
// Each goo spurt corresponds to and is timed to match one of the
// segments of the insect (see "GIANT INSECT").  To get some variety,
// different emitters with different emission velocities are used for
// the head, tail and main segments.  
//

// Impact Goo Velocity Variables:
%IOP_GiantInsectGooSpurt_head_Velocity    = 16.0;
%IOP_GiantInsectGooSpurt_segment_Velocity = 10.0;
%IOP_GiantInsectGooSpurt_tail_Velocity    = 5.0;

// Impact Goo Emitter -- head (default "sprinker" type)
datablock ParticleEmitterData(IOP_GooSpurt_head_E)
{
  // TGE emitterType = "sprinkler";

  ejectionOffset   = 0.02;
  ejectionPeriodMS = 20;
  periodVarianceMS = 3;
  ejectionVelocity = %IOP_GiantInsectGooSpurt_head_Velocity;
  velocityVariance = %IOP_GiantInsectGooSpurt_head_Velocity/3.0;  
  particles        = "IOP_GooSpurtA_P IOP_GooSpurtA_P IOP_GooSpurtA_P " @
                     "IOP_GooSpurtE_P IOP_GooSpurtE_P " @
                     "IOP_GooSpurtB_P IOP_GooSpurtC_P IOP_GooSpurtD_P IOP_GooSpurtF_P";
};
// Impact Goo Emitter -- segments
datablock ParticleEmitterData(IOP_GooSpurt_segment_E : IOP_GooSpurt_head_E)
{
  ejectionPeriodMS = 30;
  periodVarianceMS = 3;
  ejectionVelocity = %IOP_GiantInsectGooSpurt_segment_Velocity;
  velocityVariance = %IOP_GiantInsectGooSpurt_segment_Velocity/3.0;  
};
// Impact Goo Emitter -- tail
datablock ParticleEmitterData(IOP_GooSpurt_tail_E : IOP_GooSpurt_head_E)
{
  ejectionPeriodMS = 40;
  periodVarianceMS = 3;
  ejectionVelocity = %IOP_GiantInsectGooSpurt_tail_Velocity;
  velocityVariance = %IOP_GiantInsectGooSpurt_tail_Velocity/3.0;  
};

// Impact Goo Head
datablock afxEffectWrapperData(IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_head_E;
  constraint = "impactedObject.Bip01 Spine1";
  delay    = 0;
  lifetime = 0.2;
  //execConditions[0] = $AFX::IMPACTED_SOMETHING;
};
// Impact Goo 1
datablock afxEffectWrapperData(IOP_GooSpurt_1_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_1_Delay; 
};
// Impact Goo 2
datablock afxEffectWrapperData(IOP_GooSpurt_2_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_2_Delay; 
};
// Impact Goo 3
datablock afxEffectWrapperData(IOP_GooSpurt_3_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_3_Delay; 
};
// Impact Goo 4
datablock afxEffectWrapperData(IOP_GooSpurt_4_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_4_Delay; 
};
// Impact Goo 5
datablock afxEffectWrapperData(IOP_GooSpurt_5_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_5_Delay; 
};
// Impact Goo 6
datablock afxEffectWrapperData(IOP_GooSpurt_6_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_6_Delay; 
};
// Impact Goo 7
datablock afxEffectWrapperData(IOP_GooSpurt_7_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_7_Delay; 
};
// Impact Goo 8
datablock afxEffectWrapperData(IOP_GooSpurt_8_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_8_Delay; 
};
// Impact Goo 9
datablock afxEffectWrapperData(IOP_GooSpurt_9_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_9_Delay; 
};
// Impact Goo 10
datablock afxEffectWrapperData(IOP_GooSpurt_10_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_10_Delay; 
};
// Impact Goo 11
datablock afxEffectWrapperData(IOP_GooSpurt_11_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_11_Delay; 
};
// Impact Goo 12
datablock afxEffectWrapperData(IOP_GooSpurt_12_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_12_Delay; 
};
// Impact Goo 13
datablock afxEffectWrapperData(IOP_GooSpurt_13_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_13_Delay; 
};
// Impact Goo 14
datablock afxEffectWrapperData(IOP_GooSpurt_14_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_14_Delay; 
};
// Impact Goo 15
datablock afxEffectWrapperData(IOP_GooSpurt_15_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_15_Delay; 
};
// Impact Goo 16
datablock afxEffectWrapperData(IOP_GooSpurt_16_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_16_Delay; 
};
// Impact Goo 17
datablock afxEffectWrapperData(IOP_GooSpurt_17_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_17_Delay; 
};
// Impact Goo 18
datablock afxEffectWrapperData(IOP_GooSpurt_18_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_18_Delay; 
};
// Impact Goo 19
datablock afxEffectWrapperData(IOP_GooSpurt_19_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_segment_E;  
  delay  = %IOP_GiantInsectSegment_19_Delay; 
};
// Impact Goo 20 (tail)
datablock afxEffectWrapperData(IOP_GooSpurt_20_EW : IOP_GooSpurt_Head_EW)
{
  effect = IOP_GooSpurt_tail_E;  
  delay  = %IOP_GiantInsectSegment_20_Delay; 
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// IMPACT BLOOD

//
// If the giant insect strikes the orc, each of its segments causes
// an explosion of blood from the orc's belly.
//
// The blood particles used here are reused from the MapleLeaf Frag
// spell (see "BLOOD EFFECTS" in mapleleaf_frag.cs).  Because the
// velocity of the spurts increases with each impact, a different
// emitter is required.
//

// Impact Blood Velocity Variables
//
%IOP_GiantInsectBloodSpurt_Velocity_Base = 5.0;
%IOP_GiantInsectBloodSpurt_Velocity_Add  = 0.15;
//
%IOP_GiantInsectBloodSpurt_1_Velocity  = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*1);
%IOP_GiantInsectBloodSpurt_2_Velocity  = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*2);
%IOP_GiantInsectBloodSpurt_3_Velocity  = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*3);
%IOP_GiantInsectBloodSpurt_4_Velocity  = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*4);
%IOP_GiantInsectBloodSpurt_5_Velocity  = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*5);
%IOP_GiantInsectBloodSpurt_6_Velocity  = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*6);
%IOP_GiantInsectBloodSpurt_7_Velocity  = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*7);
%IOP_GiantInsectBloodSpurt_8_Velocity  = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*8);
%IOP_GiantInsectBloodSpurt_9_Velocity  = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*9);
%IOP_GiantInsectBloodSpurt_10_Velocity = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*10);
%IOP_GiantInsectBloodSpurt_11_Velocity = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*11);
%IOP_GiantInsectBloodSpurt_12_Velocity = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*12);
%IOP_GiantInsectBloodSpurt_13_Velocity = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*13);
%IOP_GiantInsectBloodSpurt_14_Velocity = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*14);
%IOP_GiantInsectBloodSpurt_15_Velocity = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*15);
%IOP_GiantInsectBloodSpurt_16_Velocity = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*16);
%IOP_GiantInsectBloodSpurt_17_Velocity = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*17);
%IOP_GiantInsectBloodSpurt_18_Velocity = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*18);
%IOP_GiantInsectBloodSpurt_19_Velocity = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*19);
%IOP_GiantInsectBloodSpurt_20_Velocity = %IOP_GiantInsectBloodSpurt_Velocity_Base+(%IOP_GiantInsectBloodSpurt_Velocity_Add*20);

// Impact Blood Emitters
//   -- one for each insect segment
//   -- default "sprinkler" type
//

// GUSH particles are each a single blood drop
datablock ParticleData(IOP_BloodGush_P)
{
   // TGE textureName          = %mySpellDataPath @ "/IOP/particles/IOP_bloodsquirt";
   dragCoeffiecient     = 0.5;
   gravityCoefficient   = 1.0; //0.2;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 800;
   lifetimeVarianceMS   = 200;
   useInvAlpha          = true;
   //spinRandomMin        = -360.0;
   //spinRandomMax        = 360.0;
   colors[0]            = "1.0 1.0 1.0 1.0";   
   colors[1]            = "1.0 1.0 1.0 0.4";
   sizes[0]             = 0.05;   
   sizes[1]             = 0.15;
   times[0]             = 0.0;   
   times[1]             = 1.0;

   textureName          = %mySpellDataPath @ "/IOP/particles/iop_tiled_blood"; // bloodsquirt
   textureCoords[0]     = "0.75 0.75";
   textureCoords[1]     = "0.75 1.0";
   textureCoords[2]     = "1.0  1.0";
   textureCoords[3]     = "1.0  0.75";
};
// SPURT particles are a splatter of blood drops
datablock ParticleData(IOP_BloodSpurt_P)
{
   // TGE textureName          = %mySpellDataPath @ "/IOP/particles/IOP_bloodspurt";
   dragCoeffiecient     = 0.5;
   gravityCoefficient   = 1.0; //0.2;
   inheritedVelFactor   = 0.00;
   lifetimeMS           = 800;
   lifetimeVarianceMS   = 200;
   useInvAlpha          = true;
   spinRandomMin        = -360.0;
   spinRandomMax        = 360.0;
   colors[0]            = "1.0 1.0 1.0 1.0";   
   colors[1]            = "1.0 1.0 1.0 0.4";
   sizes[0]             = 0.9;   
   sizes[1]             = 0.95;
   times[0]             = 0.0;   
   times[1]             = 1.0;

   textureName          = %mySpellDataPath @ "/IOP/particles/iop_tiled_blood"; // bloodspurt
   textureCoords[0]     = "0.0 0.0";
   textureCoords[1]     = "0.0 0.5";
   textureCoords[2]     = "0.5 0.5";
   textureCoords[3]     = "0.5 0.0";
};
//
datablock ParticleEmitterData(IOP_BloodSpurt_head_E)
{
  // TGE emitterType = "sprinkler";

  ejectionOffset   = 0.02;
  ejectionPeriodMS = 20;
  periodVarianceMS = 3;
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_Velocity_Base;
  velocityVariance = %IOP_GiantInsectBloodSpurt_Velocity_Base/3.0;  
  particles        = "IOP_BloodSpurt_P IOP_BloodGush_P";
};
datablock ParticleEmitterData(IOP_BloodSpurt_1_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_1_Velocity;
  velocityVariance = %IOP_GiantInsectBloodSpurt_1_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_2_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_2_Velocity;
  velocityVariance = %IOP_GiantInsectBloodSpurt_2_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_3_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_3_Velocity;
  velocityVariance = %IOP_GiantInsectBloodSpurt_3_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_4_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_4_Velocity;
  velocityVariance = %IOP_GiantInsectBloodSpurt_4_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_5_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_5_Velocity;
  velocityVariance = %IOP_GiantInsectBloodSpurt_5_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_6_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_6_Velocity*5.0;
  velocityVariance = %IOP_GiantInsectBloodSpurt_6_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_7_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_7_Velocity;
  velocityVariance = %IOP_GiantInsectBloodSpurt_7_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_8_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_8_Velocity;
  velocityVariance = %IOP_GiantInsectBloodSpurt_8_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_9_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_9_Velocity;
  velocityVariance = %IOP_GiantInsectBloodSpurt_9_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_10_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_10_Velocity*4.0;
  velocityVariance = %IOP_GiantInsectBloodSpurt_10_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_11_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_11_Velocity;
  velocityVariance = %IOP_GiantInsectBloodSpurt_11_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_12_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_12_Velocity;
  velocityVariance = %IOP_GiantInsectBloodSpurt_12_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_13_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_13_Velocity*5.0;
  velocityVariance = %IOP_GiantInsectBloodSpurt_13_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_14_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_14_Velocity;
  velocityVariance = %IOP_GiantInsectBloodSpurt_14_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_15_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_15_Velocity;
  velocityVariance = %IOP_GiantInsectBloodSpurt_15_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_16_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_16_Velocity;
  velocityVariance = %IOP_GiantInsectBloodSpurt_16_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_17_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_17_Velocity;
  velocityVariance = %IOP_GiantInsectBloodSpurt_17_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_18_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_18_Velocity;
  velocityVariance = %IOP_GiantInsectBloodSpurt_18_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_19_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_19_Velocity;
  velocityVariance = %IOP_GiantInsectBloodSpurt_19_Velocity/3.0;  
};
datablock ParticleEmitterData(IOP_BloodSpurt_20_E : IOP_BloodSpurt_head_E)
{
  ejectionVelocity = %IOP_GiantInsectBloodSpurt_20_Velocity;
  velocityVariance = %IOP_GiantInsectBloodSpurt_20_Velocity/3.0;  
};

// Impact Blood Head
datablock afxEffectWrapperData(IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_head_E;
  constraint = "impactedObject.Bip01 Spine1";
  delay    = 0+%IOP_GiantInsectSegment_Delay;
  lifetime = 0.2;
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
};
// Impact Blood 1
datablock afxEffectWrapperData(IOP_BloodSpurt_1_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_1_E;
  delay  = %IOP_GiantInsectSegment_1_Delay+%IOP_GiantInsectSegment_Delay;
};
// Impact Blood 2
datablock afxEffectWrapperData(IOP_BloodSpurt_2_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_2_E;
  delay  = %IOP_GiantInsectSegment_2_Delay+%IOP_GiantInsectSegment_Delay; 
};
// Impact Blood 3
datablock afxEffectWrapperData(IOP_BloodSpurt_3_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_3_E;
  delay  = %IOP_GiantInsectSegment_3_Delay+%IOP_GiantInsectSegment_Delay; 
};
// Impact Blood 4
datablock afxEffectWrapperData(IOP_BloodSpurt_4_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_4_E;
  delay  = %IOP_GiantInsectSegment_4_Delay+%IOP_GiantInsectSegment_Delay; 
};
// Impact Blood 5
datablock afxEffectWrapperData(IOP_BloodSpurt_5_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_5_E;
  delay  = %IOP_GiantInsectSegment_5_Delay+%IOP_GiantInsectSegment_Delay; 
};
// Impact Blood 6
datablock afxEffectWrapperData(IOP_BloodSpurt_6_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_6_E;
  delay  = %IOP_GiantInsectSegment_6_Delay+%IOP_GiantInsectSegment_Delay; 
};
// Impact Blood 7
datablock afxEffectWrapperData(IOP_BloodSpurt_7_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_7_E;
  delay  = %IOP_GiantInsectSegment_7_Delay+%IOP_GiantInsectSegment_Delay;
};
// Impact Blood 8
datablock afxEffectWrapperData(IOP_BloodSpurt_8_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_8_E;
  delay  = %IOP_GiantInsectSegment_8_Delay+%IOP_GiantInsectSegment_Delay; 
};
// Impact Blood 9
datablock afxEffectWrapperData(IOP_BloodSpurt_9_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_9_E;
  delay  = %IOP_GiantInsectSegment_9_Delay+%IOP_GiantInsectSegment_Delay;
};
// Impact Blood 10
datablock afxEffectWrapperData(IOP_BloodSpurt_10_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_10_E;
  delay  = %IOP_GiantInsectSegment_10_Delay+%IOP_GiantInsectSegment_Delay;
};
// Impact Blood 11
datablock afxEffectWrapperData(IOP_BloodSpurt_11_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_11_E;
  delay  = %IOP_GiantInsectSegment_11_Delay+%IOP_GiantInsectSegment_Delay;
};
// Impact Blood 12
datablock afxEffectWrapperData(IOP_BloodSpurt_12_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_12_E;
  delay  = %IOP_GiantInsectSegment_12_Delay+%IOP_GiantInsectSegment_Delay; 
};
// Impact Blood 13
datablock afxEffectWrapperData(IOP_BloodSpurt_13_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_13_E;
  delay  = %IOP_GiantInsectSegment_13_Delay+%IOP_GiantInsectSegment_Delay; 
};
// Impact Blood 14
datablock afxEffectWrapperData(IOP_BloodSpurt_14_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_14_E;
  delay  = %IOP_GiantInsectSegment_14_Delay+%IOP_GiantInsectSegment_Delay;
};
// Impact Blood 15
datablock afxEffectWrapperData(IOP_BloodSpurt_15_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_15_E;
  delay  = %IOP_GiantInsectSegment_15_Delay+%IOP_GiantInsectSegment_Delay;
};
// Impact Blood 16
datablock afxEffectWrapperData(IOP_BloodSpurt_16_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_16_E;
  delay  = %IOP_GiantInsectSegment_16_Delay+%IOP_GiantInsectSegment_Delay;
};
// Impact Blood 17
datablock afxEffectWrapperData(IOP_BloodSpurt_17_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_17_E;
  delay  = %IOP_GiantInsectSegment_17_Delay+%IOP_GiantInsectSegment_Delay;
};
// Impact Blood 18
datablock afxEffectWrapperData(IOP_BloodSpurt_18_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_18_E;
  delay  = %IOP_GiantInsectSegment_18_Delay+%IOP_GiantInsectSegment_Delay;
};
// Impact Blood 19
datablock afxEffectWrapperData(IOP_BloodSpurt_19_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_19_E;
  delay  = %IOP_GiantInsectSegment_19_Delay+%IOP_GiantInsectSegment_Delay;
};
// Impact Blood 20
datablock afxEffectWrapperData(IOP_BloodSpurt_20_EW : IOP_BloodSpurt_Head_EW)
{
  effect = IOP_BloodSpurt_20_E;
  delay  = %IOP_GiantInsectSegment_20_Delay+%IOP_GiantInsectSegment_Delay;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// IMPACT RESIDUE

// this mooring anchors the impact zodiacs and measures
// their altitudes

datablock afxMooringData(IOP_ImpactZodeMooring_CE)
{
  networking = $AFX::CLIENT_ONLY;
  displayAxisMarker = false;
};
datablock afxEffectWrapperData(IOP_ImpactZodeMooring_EW)
{
  effect = IOP_ImpactZodeMooring_CE;
  posConstraint = impactPoint;
  effectName = "ImpactZodeMooring";
  isConstraintSrc = true;
  lifetime = 5.0;
  xfmModifiers[0] = SHARED_freeze_AltitudeConform_XM;
};

//
// Following an impact of the orc, the blood shooting from his chest
// and the goo exploding from the insect leave bloody, gooey residue
// on the ground, and the fiery magic of the insect leaves the earth
// scorched.
//
// Zodiacs are used for these effects.  The blood/goo zodiacs are
// placed psuedo-randomly and timed to the impacting insect segments.
// A unique blood/goo zodiac is not used for every segment, however,
// just a selected few.
//

// Scorched Earth Residue
datablock afxZodiacData(IOP_MagicScorchedEarth_CE : SHARED_ZodiacBase_CE)
{  
  texture = %mySpellDataPath @ "/IOP/zodiacs/IOP_magicBurn";
  radius = 3.5;
  growInTime = 1.0;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 0.9";
  altitudeFalloff = 0.0;
  altitudeMax = 5.0;
};
//
datablock afxEffectWrapperData(IOP_MagicScorchedEarth_EW)
{
  effect = IOP_MagicScorchedEarth_CE;
  posConstraint = "#effect.ImpactZodeMooring";
  borrowAltitudes = true;
  delay = 0.5;
  fadeInTime = 1.0;
  lifetime = 1.0;
  residueLifetime = 30;
  fadeOutTime = 5;
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
};


// Blood/Goo Residue Zodiacs
//
datablock afxZodiacData(IOP_BloodGooResidueA_CE : SHARED_ZodiacBase_CE)
{
  texture = %mySpellDataPath @ "/IOP/zodiacs/IOP_bloodGooResidueA";
  radius = 3.5;
  startAngle = 0.0;
  rotationRate = 0.0;
  color = "1.0 1.0 1.0 0.9";
  altitudeFalloff = 0.0;
  altitudeMax = 3.0;
};
datablock afxZodiacData(IOP_BloodGooResidueB_CE : IOP_BloodGooResidueA_CE)
{
  texture = %mySpellDataPath @ "/IOP/zodiacs/IOP_bloodGooResidueB";
};
datablock afxZodiacData(IOP_BloodGooResidueC_CE : IOP_BloodGooResidueA_CE)
{
  texture = %mySpellDataPath @ "/IOP/zodiacs/IOP_bloodGooResidueC";
};
datablock afxZodiacData(IOP_BloodGooResidueD_CE : IOP_BloodGooResidueA_CE)
{
  texture = %mySpellDataPath @ "/IOP/zodiacs/IOP_bloodGooResidueD";
};

// Blood/Goo Placement Offsets
//
datablock afxXM_LocalOffsetData(IOP_Residue_Offset_1_XM)
{
  localOffset = "-2 2 0";
};
datablock afxXM_LocalOffsetData(IOP_Residue_Offset_2_XM)
{
  localOffset = "2.5 1.8 0";
};
datablock afxXM_LocalOffsetData(IOP_Residue_Offset_3_XM)
{
  localOffset = "1.5 -2 0";
};
datablock afxXM_LocalOffsetData(IOP_Residue_Offset_4_XM)
{
  localOffset = "-3 -3 0";
};
datablock afxXM_LocalOffsetData(IOP_Residue_Offset_5_XM)
{
  localOffset = "1 1 0";
};
datablock afxXM_LocalOffsetData(IOP_Residue_Offset_6_XM)
{
  localOffset = "-1.3 -0.5 0";
};
datablock afxXM_LocalOffsetData(IOP_Residue_Offset_7_XM)
{
  localOffset = "2 2 0";
};
datablock afxXM_LocalOffsetData(IOP_Residue_Offset_8_XM)
{
  localOffset = "1 -0.6 0";
};

// Blood/Goo Residue 1
datablock afxEffectWrapperData(IOP_BloodGooResidue_1_EW)
{
  effect = IOP_BloodGooResidueA_CE;
  posConstraint = "#effect.ImpactZodeMooring";
  borrowAltitudes = true;
  delay = %IOP_GiantInsectSegment_3_Delay;
  fadeInTime = 0.3;
  lifetime = 1.0;
  residueLifetime = 30;
  fadeOutTime = 5;

  xfmModifiers[0] = IOP_Residue_Offset_1_XM;
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
};
// Blood/Goo Residue 2
datablock afxEffectWrapperData(IOP_BloodGooResidue_2_EW : IOP_BloodGooResidue_1_EW)
{
  effect = IOP_BloodGooResidueB_CE;
  delay  = %IOP_GiantInsectSegment_8_Delay;
  xfmModifiers[0] = IOP_Residue_Offset_2_XM;
};
// Blood/Goo Residue 3
datablock afxEffectWrapperData(IOP_BloodGooResidue_3_EW : IOP_BloodGooResidue_1_EW)
{
  effect = IOP_BloodGooResidueC_CE;
  delay  = %IOP_GiantInsectSegment_10_Delay;
  xfmModifiers[0] = IOP_Residue_Offset_3_XM;
};
// Blood/Goo Residue 4
datablock afxEffectWrapperData(IOP_BloodGooResidue_4_EW : IOP_BloodGooResidue_1_EW)
{
  effect = IOP_BloodGooResidueD_CE;
  delay  = %IOP_GiantInsectSegment_14_Delay;
  xfmModifiers[0] = IOP_Residue_Offset_4_XM;
};
// Blood/Goo Residue 5
datablock afxEffectWrapperData(IOP_BloodGooResidue_5_EW : IOP_BloodGooResidue_1_EW)
{
  effect = IOP_BloodGooResidueA_CE;
  delay  = %IOP_GiantInsectSegment_15_Delay;
  xfmModifiers[0] = IOP_Residue_Offset_5_XM;
};
// Blood/Goo Residue 6
datablock afxEffectWrapperData(IOP_BloodGooResidue_6_EW : IOP_BloodGooResidue_1_EW)
{
  effect = IOP_BloodGooResidueB_CE;
  delay  = %IOP_GiantInsectSegment_17_Delay;
  xfmModifiers[0] = IOP_Residue_Offset_6_XM;
};
// Blood/Goo Residue 7
datablock afxEffectWrapperData(IOP_BloodGooResidue_7_EW : IOP_BloodGooResidue_1_EW)
{
  effect = IOP_BloodGooResidueC_CE;
  delay  = %IOP_GiantInsectSegment_19_Delay;
  xfmModifiers[0] = IOP_Residue_Offset_7_XM;
};
// Blood/Goo Residue 8
datablock afxEffectWrapperData(IOP_BloodGooResidue_8_EW : IOP_BloodGooResidue_1_EW)
{
  effect = IOP_BloodGooResidueD_CE;
  delay  = %IOP_GiantInsectSegment_20_Delay;
  xfmModifiers[0] = IOP_Residue_Offset_8_XM;
};


//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// GIANT INSECT MISSILE

//
// The missile to which the giant insect effects are constrained is
// largely responsible for the interesting, curling motion of the
// monster (see "GIANT INSECT" above for further information).
//
// The curl is created using the missiles wiggle parameters.  Wiggle
// rotates the missiles velocity vector using a sine wave.  The
// wiggles occur about a specified axis, and their magnitude (or
// amplitude) and their speed can be set.  Also, every wiggle
// attribute  can take an array, and therefore muliple wiggles can
// be defined for the missile, allowing for complex twisting
// behaviors.
//
// This missile uses two wiggles for it's effect: one on the XY (top)
// plane, and the other on the YZ (side) plane.
//
// The other important attribute used here is hover.  Hover
// influences the missiles guidance system, allowing it to hover at a
// specified altitude (hoverAltitude) until the missile is within a
// certain distance to the target (hoverAttackDistance).  Once there,
// it hovers dramatically for an additional time (hoverTime in ms)
// before plunging down to strike the target.  A gradient is provided
// (hoverAttackGradient) allowing the missile to ease-in and out as
// it enters the attacking range.
//

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// TARGET ANIMATION

datablock afxAnimClipData(IOP_Death_Clip_CE)
{
  clipName = "death9";
  ignoreCorpse = true;
  treatAsDeathAnim = true;
  rate = 1.0;
};
datablock afxEffectWrapperData(IOP_Death_Clip_EW)
{
  effect = IOP_Death_Clip_CE;
  constraint = "impactedObject";
  lifetime = 3.633;
  delay = 0.1;
  lifeConditions = $AFX::DYING;
  execConditions[0] = $AFX::IMPACTED_PRIMARY;
};

datablock afxModelData(IOP_GiantInsectJawSparkle_CE)
{
  shapeFile = %mySpellDataPath @ "/IOP/models/IOP_insectJawSparkles.dts";
  forceOnMaterialFlags = $MaterialFlags::Additive | $MaterialFlags::SelfIlluminating;
  sequence = "sparkle";
  sequenceRate = 1.0;
  alphaMult = 0.5;
};
datablock afxXM_AimData(IOP_GiantInsectJawSparkle_aim_XM)
{
  aimZOnly = false;
};

datablock afxXM_SpinData(IOP_GiantInsectJawSparkle_spin1_XM)
{
  spinAxis = "0 1 0";
  spinAngle = 0;
  spinRate = -540;
};
datablock afxXM_SpinData(IOP_GiantInsectJawSparkle_spin2_XM)
{
  spinAxis = "0 1 0";
  spinAngle = 0;
  spinRate = 540;
};

datablock afxEffectWrapperData(IOP_GiantInsectJawSparkle_LF_EW)
{
  effect = IOP_GiantInsectJawSparkle_CE;
  posConstraint = "#effect.InsectEyeJaws.mountMandibleLF";
  posConstraint2 = "camera";

  scaleFactor = 0.75;

  delay = 0;
  fadeInTime  = 0.25;
  fadeOutTime = 0.25;

  xfmModifiers[0] = IOP_GiantInsectJawSparkle_aim_XM;
  xfmModifiers[1] = IOP_GiantInsectJawSparkle_spin1_XM;
};
datablock afxEffectWrapperData(IOP_GiantInsectJawSparkle_RT_EW : IOP_GiantInsectJawSparkle_LF_EW)
{
  posConstraint = "#effect.InsectEyeJaws.mountMandibleRT";
  xfmModifiers[1] = IOP_GiantInsectJawSparkle_spin2_XM;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// AUDIO AND LIGHTING

switch$ (afxGetEngine())
{
  case "TGE":
    exec("./lighting/iop_lighting_tge_sub.cs");
    exec("./audio/iop_audio_sub.cs");
  case "TGEA":
    exec("./lighting/iop_lighting_tgea_sub.cs");
    exec("./audio/iop_audio_sub.cs");
 case "T3D":
    exec("./lighting/iop_lighting_t3d_sub.cs");
    exec("./audio/iop_audio_sub.cs");
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// MAGIC MISSILE
//

datablock afxMagicMissileData(IOP_GiantInsect_Missile)
{
  muzzleVelocity        = 15;
  velInheritFactor      = 0;
  lifetime              = 20000;
  isBallistic           = false;
  gravityMod            = 0.05;
  isGuided              = true;
  precision             = 20;
  trackDelay            = 7;

  sound = IOP_BugLoopSnd_CE;

  wiggleAxis       = "0 0 1 " @
                     "1 0 0";
  wiggleMagnitudes = "0.15 0.1";
  wiggleSpeeds     = "5.0 7.0";

  hoverAltitude       = 20;
  hoverAttackDistance = 15;
  hoverAttackGradient = 5;
  hoverTime           = 3000;

  // NOTE - These launch values start the missile at approximately the
  // character's chest (while levitating) and shoot it straight up.
  // Adjustment will be required for different characters and animation.
  launchOffset     = "0.0 -0.3 1.4";
  launchAimPitch   = -90;
  echoLaunchOffset = false;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
// INSECTOPLASM SPELL
//

datablock afxMagicSpellData(InsectoplasmSpell)
{
  castingDur = 16-2;
  extraDeliveryTime = %IOP_GiantInsectSegment_20_Delay;

  missile = IOP_GiantInsect_Missile;

    // spellcaster animation //
  addCastingEffect = IOP_Casting_Clip_EW;
  addCastingEffect = IOP_AnimLock_EW;
    // casting zodiacs //
  addCastingEffect = IOP_CastingZodeMooring_EW;
  addCastingEffect = IOP_CastingZode_Reveal_EW;
  addCastingEffect = IOP_CastingZode_Eye_EW;
  addCastingEffect = IOP_CastingZode_DarkBand_EW;
  addCastingEffect = IOP_CastingZode_CloudBandA_EW;
  addCastingEffect = IOP_CastingZode_CloudBandB_EW;
    // (note: the main and text zodiacs should be composited over
    //        the cloudbands, so they are added after)
  addCastingEffect = IOP_CastingZode_Main_underglow_EW;
  addCastingEffect = IOP_CastingZode_Main_EW;
  addCastingEffect = IOP_CastingZode_Text_underglow_EW;
  addCastingEffect = IOP_CastingZode_Text_EW;

    // bugs //
  addCastingEffect = IOP_Bug1_EW;
  addCastingEffect = IOP_Bug2_EW;
  addCastingEffect = IOP_Bug3_EW;
  addCastingEffect = IOP_Bug4_EW;
  addCastingEffect = IOP_Bug5_EW;
  addCastingEffect = IOP_Bug6_EW;
  addCastingEffect = IOP_Bug7_EW;
  addCastingEffect = IOP_Bug8_EW;
  addCastingEffect = IOP_Bug9_EW;
  addCastingEffect = IOP_Bug10_EW;
  addCastingEffect = IOP_Bug11_EW;
  addCastingEffect = IOP_Bug12_EW;
  addCastingEffect = IOP_Bug13_EW;
  addCastingEffect = IOP_Bug14_EW;
  addCastingEffect = IOP_Bug15_EW;
  addCastingEffect = IOP_Bug16_EW;
  addCastingEffect = IOP_Bug17_EW;
  addCastingEffect = IOP_Bug18_EW;
  addCastingEffect = IOP_Bug19_EW;
  addCastingEffect = IOP_Bug20_EW;
    // bug pulses //
  addCastingEffect = IOP_BugPulse1_EW;
  addCastingEffect = IOP_BugPulse2_EW;
  addCastingEffect = IOP_BugPulse3_EW;
  addCastingEffect = IOP_BugPulse4_EW;
  addCastingEffect = IOP_BugPulse5_EW;
  addCastingEffect = IOP_BugPulse6_EW;
  addCastingEffect = IOP_BugPulse7_EW;
  addCastingEffect = IOP_BugPulse8_EW;
  addCastingEffect = IOP_BugPulse9_EW;
  addCastingEffect = IOP_BugPulse10_EW;
  addCastingEffect = IOP_BugPulse11_EW;
  addCastingEffect = IOP_BugPulse12_EW;
  addCastingEffect = IOP_BugPulse13_EW;
  addCastingEffect = IOP_BugPulse14_EW;
  addCastingEffect = IOP_BugPulse15_EW;
  addCastingEffect = IOP_BugPulse16_EW;
  addCastingEffect = IOP_BugPulse17_EW;
  addCastingEffect = IOP_BugPulse18_EW;
  addCastingEffect = IOP_BugPulse19_EW;
  addCastingEffect = IOP_BugPulse20_EW;
    // bug symbols //
  addCastingEffect = IOP_BugSymbol1_EW;
  addCastingEffect = IOP_BugSymbol2_EW;
  addCastingEffect = IOP_BugSymbol3_EW;
  addCastingEffect = IOP_BugSymbol4_EW;
  addCastingEffect = IOP_BugSymbol5_EW;
  addCastingEffect = IOP_BugSymbol6_EW;
  addCastingEffect = IOP_BugSymbol7_EW;
  addCastingEffect = IOP_BugSymbol8_EW;
  addCastingEffect = IOP_BugSymbol9_EW;
  addCastingEffect = IOP_BugSymbol10_EW;
  addCastingEffect = IOP_BugSymbol11_EW;
  addCastingEffect = IOP_BugSymbol12_EW;
  addCastingEffect = IOP_BugSymbol13_EW;
  addCastingEffect = IOP_BugSymbol14_EW;
  addCastingEffect = IOP_BugSymbol15_EW;
  addCastingEffect = IOP_BugSymbol16_EW;
  addCastingEffect = IOP_BugSymbol17_EW;
  addCastingEffect = IOP_BugSymbol18_EW;
  addCastingEffect = IOP_BugSymbol19_EW;
  addCastingEffect = IOP_BugSymbol20_EW;
    // beams //
  addCastingEffect = IOP_BeamZodiac_EW;
  addCastingEffect = IOP_BeamRing_EW;
  addCastingEffect = IOP_EyeGlowZodiac_EW;
  addCastingEffect = IOP_BeamsA1_EW;
  addCastingEffect = IOP_BeamsA2_EW;
  addCastingEffect = IOP_BeamsA3_EW;
  addCastingEffect = IOP_BeamsA4_EW;
  addCastingEffect = IOP_BeamsB1_EW;
  addCastingEffect = IOP_BeamsB2_EW;
  addCastingEffect = IOP_BeamsB3_EW;
  addCastingEffect = IOP_BeamsB4_EW;
  addCastingEffect = IOP_BeamsC1_EW;
  addCastingEffect = IOP_BeamsC2_EW;
  addCastingEffect = IOP_BeamsC3_EW;
  addCastingEffect = IOP_BeamsC4_EW;

    // casting goo spurt //
  addCastingEffect = IOP_Casting_GooSpurt_Tiny_1_EW;
  addCastingEffect = IOP_Casting_GooSpurt_Tiny_2_EW;
  addCastingEffect = IOP_Casting_GooSpurt_Tiny_3_EW;
  addCastingEffect = IOP_Casting_GooSpurt_Tiny_4_EW;
  addCastingEffect = IOP_Casting_GooSpurt_Tiny_5_EW;
  addCastingEffect = IOP_Casting_GooSpurt1_EW;
  addCastingEffect = IOP_Casting_GooSpurtSpray1_EW;
  addCastingEffect = IOP_Casting_GooSpurt2_EW;
  addCastingEffect = IOP_Casting_GooSpurt3_EW;
  addCastingEffect = IOP_Casting_GooSpurt4_EW;
  addCastingEffect = IOP_Casting_GooSpurt5_EW;
    // casting goo splatter //
  addCastingEffect = IOP_GooSplatter_1_EW;
  addCastingEffect = IOP_GooSplatter_2_EW;
  addCastingEffect = IOP_GooSplatter_3_EW;
  addCastingEffect = IOP_GooSplatter_4_EW;
  addCastingEffect = IOP_GooSplatter_5_EW;
  addCastingEffect = IOP_GooSplatter_6_EW;
  addCastingEffect = IOP_GooSplatter_7_EW;

    // giant insect //
  addDeliveryEffect = IOP_GiantInsectHead_EW;
  addDeliveryEffect = IOP_GiantInsectEyeJaws_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_1_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_2_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_3_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_4_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_5_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_6_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_7_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_8_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_9_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_10_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_11_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_12_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_13_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_14_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_15_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_16_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_17_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_18_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_19_EW;
  addDeliveryEffect = IOP_GiantInsectSegment_20_EW;
    // giant insect magic (fire) //
  addDeliveryEffect = IOP_GiantInsectMagic_1_EW;
  addDeliveryEffect = IOP_GiantInsectMagic_2_EW;
  addDeliveryEffect = IOP_GiantInsectMagic_3_EW;
  addDeliveryEffect = IOP_GiantInsectMagic_4_EW;
  addDeliveryEffect = IOP_GiantInsectMagic_5_EW;
  addDeliveryEffect = IOP_GiantInsectMagic_6_EW;
  addDeliveryEffect = IOP_GiantInsectMagic_7_EW;
  addDeliveryEffect = IOP_GiantInsectMagic_8_EW;
  addDeliveryEffect = IOP_GiantInsectMagic_9_EW;
  addDeliveryEffect = IOP_GiantInsectMagic_10_EW;
  addDeliveryEffect = IOP_GiantInsectMagic_11_EW;
  addDeliveryEffect = IOP_GiantInsectMagic_12_EW;
  addDeliveryEffect = IOP_GiantInsectMagic_13_EW;
  addDeliveryEffect = IOP_GiantInsectMagic_14_EW;
  addDeliveryEffect = IOP_GiantInsectMagic_15_EW;
  addDeliveryEffect = IOP_GiantInsectMagic_16_EW;
  addDeliveryEffect = IOP_GiantInsectMagic_17_EW;
  addDeliveryEffect = IOP_GiantInsectMagic_18_EW;
  addDeliveryEffect = IOP_GiantInsectMagic_19_EW;

  addDeliveryEffect = IOP_GiantInsectJawSparkle_LF_EW;
  addDeliveryEffect = IOP_GiantInsectJawSparkle_RT_EW;

    // target animation //
  addImpactEffect = IOP_Death_Clip_EW;

    // impact magic //
  addImpactEffect = IOP_GiantInsectMagicImpact_1_EW;
  addImpactEffect = IOP_GiantInsectMagicImpact_2_EW;
  addImpactEffect = IOP_GiantInsectMagicImpact_3_EW;
  addImpactEffect = IOP_GiantInsectMagicImpact_4_EW;
  addImpactEffect = IOP_GiantInsectMagicImpact_5_EW;
  addImpactEffect = IOP_GiantInsectMagicImpact_6_EW;
  addImpactEffect = IOP_GiantInsectMagicImpact_7_EW;
  addImpactEffect = IOP_GiantInsectMagicImpact_8_EW;
  addImpactEffect = IOP_GiantInsectMagicImpact_9_EW;
  addImpactEffect = IOP_GiantInsectMagicImpact_10_EW;
  addImpactEffect = IOP_GiantInsectMagicImpact_11_EW;
  addImpactEffect = IOP_GiantInsectMagicImpact_12_EW;
  addImpactEffect = IOP_GiantInsectMagicImpact_13_EW;
  addImpactEffect = IOP_GiantInsectMagicImpact_14_EW;
  addImpactEffect = IOP_GiantInsectMagicImpact_15_EW;
  addImpactEffect = IOP_GiantInsectMagicImpact_16_EW;
  addImpactEffect = IOP_GiantInsectMagicImpact_17_EW;
  addImpactEffect = IOP_GiantInsectMagicImpact_18_EW;
  addImpactEffect = IOP_GiantInsectMagicImpact_19_EW;

  // impact goo //
  addImpactEffect = IOP_GooSpurt_Head_EW;
  addImpactEffect = IOP_GooSpurt_1_EW;
  addImpactEffect = IOP_GooSpurt_2_EW;
  addImpactEffect = IOP_GooSpurt_3_EW;
  addImpactEffect = IOP_GooSpurt_4_EW;
  addImpactEffect = IOP_GooSpurt_5_EW;
  addImpactEffect = IOP_GooSpurt_6_EW;
  addImpactEffect = IOP_GooSpurt_7_EW;
  addImpactEffect = IOP_GooSpurt_8_EW;
  addImpactEffect = IOP_GooSpurt_9_EW;
  addImpactEffect = IOP_GooSpurt_10_EW;
  addImpactEffect = IOP_GooSpurt_11_EW;
  addImpactEffect = IOP_GooSpurt_12_EW;
  addImpactEffect = IOP_GooSpurt_13_EW;
  addImpactEffect = IOP_GooSpurt_14_EW;
  addImpactEffect = IOP_GooSpurt_15_EW;
  addImpactEffect = IOP_GooSpurt_16_EW;
  addImpactEffect = IOP_GooSpurt_17_EW;
  addImpactEffect = IOP_GooSpurt_18_EW;
  addImpactEffect = IOP_GooSpurt_19_EW;
  addImpactEffect = IOP_GooSpurt_20_EW;

    // impact blood //
  //addImpactEffect = IOP_BloodSpurt_Head_EW;
  addImpactEffect = IOP_BloodSpurt_1_EW;
  addImpactEffect = IOP_BloodSpurt_2_EW;
  addImpactEffect = IOP_BloodSpurt_3_EW;
  addImpactEffect = IOP_BloodSpurt_4_EW;
  addImpactEffect = IOP_BloodSpurt_5_EW;
  addImpactEffect = IOP_BloodSpurt_6_EW;
  addImpactEffect = IOP_BloodSpurt_7_EW;
  addImpactEffect = IOP_BloodSpurt_8_EW;
  addImpactEffect = IOP_BloodSpurt_9_EW;
  addImpactEffect = IOP_BloodSpurt_10_EW;
  addImpactEffect = IOP_BloodSpurt_11_EW;
  addImpactEffect = IOP_BloodSpurt_12_EW;
  addImpactEffect = IOP_BloodSpurt_13_EW;
  addImpactEffect = IOP_BloodSpurt_14_EW;
  addImpactEffect = IOP_BloodSpurt_15_EW;
  addImpactEffect = IOP_BloodSpurt_16_EW;
  addImpactEffect = IOP_BloodSpurt_17_EW;
  addImpactEffect = IOP_BloodSpurt_18_EW;
  addImpactEffect = IOP_BloodSpurt_19_EW;
  addImpactEffect = IOP_BloodSpurt_20_EW;
    // impact residue //
  addImpactEffect = IOP_ImpactZodeMooring_EW;
  addImpactEffect = IOP_MagicScorchedEarth_EW;
  addImpactEffect = IOP_BloodGooResidue_1_EW;
  addImpactEffect = IOP_BloodGooResidue_2_EW;
  addImpactEffect = IOP_BloodGooResidue_3_EW;
  addImpactEffect = IOP_BloodGooResidue_4_EW;
  addImpactEffect = IOP_BloodGooResidue_5_EW;
  addImpactEffect = IOP_BloodGooResidue_6_EW;
  addImpactEffect = IOP_BloodGooResidue_7_EW;
  addImpactEffect = IOP_BloodGooResidue_8_EW;
};

// sounds and lights added via sub-script functions //
IOP_add_Lighting_FX(InsectoplasmSpell);
IOP_add_Audio_FX(InsectoplasmSpell);

datablock afxRPGMagicSpellData(InsectoplasmSpell_RPG)
{
  spellName = "Insectoplasm";
  desc = "Summon the sacrificial BUGS, let them inside you, eat" SPC
         "you, coalescing into an insectoid TERROR that flies"   SPC
         "and strikes your enemy.  Disgusting." @
         "\n" @
         "\nspell design: Matthew Durante" @
         "\nsound effects: Dave Schroeder" @ 
         "\nspell concept: Jeff Faust";
  sourcePack = "Spell Pack 1";

  iconBitmap = %mySpellDataPath @ "/IOP/icons/iop";
  target = "enemy";
  range = 75;
  manaCost = 10;

  directDamage = 200.0;

  castingDur = InsectoplasmSpell.castingDur;
};

// set a level of detail
function InsectoplasmSpell::onActivate(%this, %spell, %caster, %target)
{
  Parent::onActivate(%this, %spell, %caster, %target);
  if (MissionInfo.hasBrightLighting)
    %spell.setExecConditions($BrightLighting_mask);
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
  InsectoplasmSpell.scriptFile = $afxAutoloadScriptFile;
  InsectoplasmSpell.spellDataPath = %mySpellDataPath;
  if (isFunction(addDemoSpellbookSpell))
    addDemoSpellbookSpell(InsectoplasmSpell, InsectoplasmSpell_RPG);
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//