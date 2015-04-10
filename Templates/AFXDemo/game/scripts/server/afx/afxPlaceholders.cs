
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
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

// Define spell placeholders for the demo spellbook. These
// placeholders reserve a spot in the demo spellbook for
// a spell which may or may not get auto-loaded later. If
// the actual spell is not loaded, the placeholder will
// display an inactive icon in the spellbank.

$CORE_TECH = "Core Tech";
$SPELLPACK_ONE = "Spell Pack 1";
$SPELLPACK_TWO = "Spell Pack 2";
$PREVIEW = "Preview";

if (afxGetEngine() $= "T3D")
  $PlaceholderIconPath = "art/afx/effects/icons/";
else
  $PlaceholderIconPath = "~/data/effects/icons/";

// PRIMARY PLACEHOLDERS

datablock afxRPGMagicSpellData(GBOF_Placeholder_RPG)
{
  spellName = "Great Ball of Fire";
  desc = "This enormous fireball spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "gbof";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(RM_Placeholder_RPG)
{
  spellName = "Reaper Madness";
  desc = "This shimmering resurrection spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "rm";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(AP_Placeholder_RPG)
{
  spellName = "Astral Passport";
  desc = "This essential transportation spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "ap";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(LMF_Placeholder_RPG)
{
  spellName = "Light My Fire";
  desc = "This hypnotic bonfire effect is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "lmf";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(FB_Placeholder_RPG)
{
  spellName = "Flame Broil";
  desc = "This fiery combat spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "fb";
  sourcePack = $SPELLPACK_ONE;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(SOR_Placeholder_RPG)
{
  spellName = "Spirit of Roach";
  desc = "This creepy crawly buff spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "sor";
  sourcePack = $SPELLPACK_ONE;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(MLF_Placeholder_RPG)
{
  spellName = "Mapleleaf Frag";
  desc = "This mighty nature spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "mlf";
  sourcePack = $SPELLPACK_ONE;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(CTAF_Placeholder_RPG)
{
  spellName = "Cantrip and Fall";
  desc = "This childish knockdown spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "ctaf";
  sourcePack = $SPELLPACK_ONE;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(SFM_Placeholder_RPG)
{
  spellName = "Summon Feckless Moth";
  desc = "This giant insect summoning spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "sfm";
  sourcePack = $SPELLPACK_ONE;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(TH_Placeholder_RPG)
{
  spellName = "Thor's Hammer";
  desc = "This anachronistic area effect spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "th";
  sourcePack = $SPELLPACK_ONE;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(AITM_Placeholder_RPG)
{
  spellName = "Arcane in the Membrane";
  desc = "This psycho debuff spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "aitm";
  sourcePack = $SPELLPACK_ONE;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(IOP_Placeholder_RPG)
{
  spellName = "Insectoplasm";
  desc = "This monster insect combat spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "iop";
  sourcePack = $SPELLPACK_ONE;
  isPlaceholder = true;
};

// EXPERIMENTAL PLACEHOLDERS

datablock afxRPGMagicSpellData(MGG_Placeholder_RPG)
{
  spellName = "Mark of GG";
  desc = "This novelty spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "mgg";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(RoHC_Placeholder_RPG)
{
  spellName = "Rid of Habeas Corpus";
  desc = "This waste management spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "rohc";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(SMKN_Placeholder_RPG)
{
  spellName = "Smokin'";
  desc = "This smoldering experimental spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "smkn";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(TMG_Placeholder_RPG)
{
  spellName = "Try MachineGun";
  desc = "This experimental spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "tmg";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(TFL_Placeholder_RPG)
{
  spellName = "Teleport to FaustLogic.com";
  desc = "This novelty spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "tfl";
  sourcePack = $SPELLPACK_ONE;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(TGT_Placeholder_RPG)
{
  spellName = "Try Gui Text";
  desc = "This experimental spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "guit";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(TPC_Placeholder_RPG)
{
  spellName = "Try Puppet Cam";
  desc = "This experimental spell is currently unavailable.";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(PHYZ_Placeholder_RPG)
{
  spellName = "Try Physical Zone";
  desc = "This experimental spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "phyz";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(TF_Placeholder_RPG)
{
  spellName = "Try Flying";
  desc = "This experimental spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "tfly";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(TRO_Placeholder_RPG)
{
  spellName = "Try Rocket Orc";
  desc = "This experimental spell is currently unavailable.";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

// SPELLPACK 2 PLACEHOLDERS

datablock afxRPGMagicSpellData(BFB_Placeholder_RPG)
{
  spellName = "Bolt from the Blue";
  desc = "This spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "bfb";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(BYOS_Placeholder_RPG)
{
  spellName = "Bron-Y-Orc Stomp";
  desc = "This spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "byos";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(CK_Placeholder_RPG)
{
  spellName = "Chill Kill";
  desc = "This icy spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "ck";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(FitS_Placeholder_RPG)
{
  spellName = "Fire in the Sky";
  desc = "This spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "ft";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(FST_Placeholder_RPG)
{
  spellName = "Flaming Stick Trick";
  desc = "This spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "fst";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(MoK_Placeholder_RPG)
{
  spellName = "Mark of Kork";
  desc = "This spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "mok";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(SMS_Placeholder_RPG)
{
  spellName = "Soul Miner's Slaughter";
  desc = "This spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "sms";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(RoF_Placeholder_RPG)
{
  spellName = "Ring of Fire";
  desc = "This spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "rof";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(GLaS_Placeholder_RPG)
{
  spellName = "Green Legs and Scram";
  desc = "This spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "glas";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(SSJ_Placeholder_RPG)
{
  spellName = "Soul Sucking Jerk";
  desc = "This spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "ssj";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(SoV_Placeholder_RPG)
{
  spellName = "Shards of Vesuvius";
  desc = "This effect element is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "sov";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(SM_Placeholder_RPG)
{
  spellName = "Soul Mine Solo";
  desc = "This effect element is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "sm";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

// SP2 ELEMENTS PLACEHOLDERS

datablock afxRPGMagicSpellData(LK_Placeholder_RPG)
{
  spellName = "The Letter K";
  desc = "This effect element is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "mok";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(DBP_Placeholder_RPG)
{
  spellName = "Dread Bull Portent";
  desc = "This effect element is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "sms";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(CoF_Placeholder_RPG)
{
  spellName = "Clusters of Fire";
  desc = "This effect element is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "cof";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(PoF_Placeholder_RPG)
{
  spellName = "Pillar of Fire";
  desc = "This effect element is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "pof";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(RoD_Placeholder_RPG)
{
  spellName = "Ring of Dust";
  desc = "This effect element is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "rod";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(SN_Placeholder_RPG)
{
  spellName = "Soul Nuke";
  desc = "This effect element is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "sn";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(UIF_Placeholder_RPG)
{
  spellName = "Up In Flames";
  desc = "This effect element is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "uif";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(WW_Placeholder_RPG)
{
  spellName = "Wandering Wisps";
  desc = "This effect element is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "ww";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(ICE_Placeholder_RPG)
{
  spellName = "Ice Shards Prop";
  desc = "This ice prop is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "ck";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(FTP_Placeholder_RPG)
{
  spellName = "Fire Tower Prop";
  desc = "This fire tower prop is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "ft";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(FPot_Placeholder_RPG)
{
  spellName = "Fiery Potato";
  desc = "This effect element is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "uif";
  sourcePack = $SPELLPACK_TWO;
  isPlaceholder = true;
};

// SCI-FI PLACEHOLDERS

datablock afxRPGMagicSpellData(SF_OL_Placeholder_RPG)
{
  spellName = "Occam's Laser (sci-fi)";
  desc = "This sci-fi effect is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "sf";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(SF_OL2_Placeholder_RPG)
{
  spellName = "Occam's Laser Reloaded (sci-fi)";
  desc = "This sci-fi effect is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "sf";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(SF_OL3_Placeholder_RPG)
{
  spellName = "Occam's Laser Redux (sci-fi)";
  desc = "This sci-fi effect is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "sf";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

// UTILITY PLACEHOLDERS

datablock afxRPGMagicSpellData(PCO_Placeholder_RPG)
{
  spellName = "Presto Change-Orc";
  desc = "This alien enhanced orc upgrade is currently unavailable. [TGEA only]";
  iconBitmap = $PlaceholderIconPath @ "pco";
  sourcePack = "TGEA";
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(PHT_Placeholder_RPG)
{
  spellName = "Phrase Tester";
  desc = "This testing spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "pht";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(WVT_Placeholder_RPG)
{
  spellName = "Wave Tester";
  desc = "This testing spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "wvt";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

datablock afxRPGMagicSpellData(TAF_Placeholder_RPG)
{
  spellName = "Try Alpha Fade";
  desc = "This experimental spell is currently unavailable.";
  iconBitmap = $PlaceholderIconPath @ "taf";
  sourcePack = $CORE_TECH;
  isPlaceholder = true;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

// Add placeholders to slots in the demo spellbook.
clearDemoSpellbookData();

$PRIMARY_BANK_SP2 = 0;
$PRIMARY_BANK = 1;
$ELEMENTS_BANK_SP2 = 2;
$LAB_BANK = 3;
$SCIFI_BANK = 4;
$UTILITY_BANK = 5;

// PRIMARY SPELLBANK
setDemoSpellbookBankName($PRIMARY_BANK, "Primary Spellbank 2");
addDemoSpellbookPlaceholder(GBOF_Placeholder_RPG,  $PRIMARY_BANK, 0);
addDemoSpellbookPlaceholder(RM_Placeholder_RPG,    $PRIMARY_BANK, 1);
addDemoSpellbookPlaceholder(AP_Placeholder_RPG,    $PRIMARY_BANK, 2);
addDemoSpellbookPlaceholder(LMF_Placeholder_RPG,   $PRIMARY_BANK, 3);
addDemoSpellbookPlaceholder(FB_Placeholder_RPG,    $PRIMARY_BANK, 4);
addDemoSpellbookPlaceholder(SOR_Placeholder_RPG,   $PRIMARY_BANK, 5);
addDemoSpellbookPlaceholder(MLF_Placeholder_RPG,   $PRIMARY_BANK, 6);
addDemoSpellbookPlaceholder(CTAF_Placeholder_RPG,  $PRIMARY_BANK, 7);
addDemoSpellbookPlaceholder(SFM_Placeholder_RPG,   $PRIMARY_BANK, 8);
addDemoSpellbookPlaceholder(TH_Placeholder_RPG,    $PRIMARY_BANK, 9);
addDemoSpellbookPlaceholder(AITM_Placeholder_RPG,  $PRIMARY_BANK, 10);
addDemoSpellbookPlaceholder(IOP_Placeholder_RPG,   $PRIMARY_BANK, 11);

// EXPERIMENTAL SPELLBANK
setDemoSpellbookBankName($LAB_BANK, "Experimental Spellbank");
addDemoSpellbookPlaceholder(MGG_Placeholder_RPG,   $LAB_BANK, 0);
addDemoSpellbookPlaceholder(RoHC_Placeholder_RPG,  $LAB_BANK, 1);
addDemoSpellbookPlaceholder(SMKN_Placeholder_RPG,  $LAB_BANK, 2);
addDemoSpellbookPlaceholder(TMG_Placeholder_RPG,   $LAB_BANK, 3);
addDemoSpellbookPlaceholder(TFL_Placeholder_RPG,   $LAB_BANK, 4);
addDemoSpellbookPlaceholder(TGT_Placeholder_RPG,   $LAB_BANK, 5);
addDemoSpellbookPlaceholder(TPC_Placeholder_RPG,   $LAB_BANK, 6);
addDemoSpellbookPlaceholder(PHYZ_Placeholder_RPG,  $LAB_BANK, 7);
addDemoSpellbookPlaceholder(TF_Placeholder_RPG,    $LAB_BANK, 8);
addDemoSpellbookPlaceholder(TRO_Placeholder_RPG,   $LAB_BANK, 9);

// SPELLPACK 2 SPELLBANK
setDemoSpellbookBankName($PRIMARY_BANK_SP2, "Primary Spellbank 1");
addDemoSpellbookPlaceholder(BFB_Placeholder_RPG,    $PRIMARY_BANK_SP2, 0);
addDemoSpellbookPlaceholder(BYOS_Placeholder_RPG,   $PRIMARY_BANK_SP2, 1);
addDemoSpellbookPlaceholder(CK_Placeholder_RPG,     $PRIMARY_BANK_SP2, 2);
addDemoSpellbookPlaceholder(FitS_Placeholder_RPG,   $PRIMARY_BANK_SP2, 3);
addDemoSpellbookPlaceholder(FST_Placeholder_RPG,    $PRIMARY_BANK_SP2, 4);
addDemoSpellbookPlaceholder(MoK_Placeholder_RPG,    $PRIMARY_BANK_SP2, 5);
addDemoSpellbookPlaceholder(SMS_Placeholder_RPG,    $PRIMARY_BANK_SP2, 6);
addDemoSpellbookPlaceholder(RoF_Placeholder_RPG,    $PRIMARY_BANK_SP2, 7);
addDemoSpellbookPlaceholder(GLaS_Placeholder_RPG,   $PRIMARY_BANK_SP2, 8);
addDemoSpellbookPlaceholder(SSJ_Placeholder_RPG,    $PRIMARY_BANK_SP2, 9);
addDemoSpellbookPlaceholder(SoV_Placeholder_RPG,    $PRIMARY_BANK_SP2, 10);
addDemoSpellbookPlaceholder(SM_Placeholder_RPG,     $PRIMARY_BANK_SP2, 11);

// SP2 ELEMENTS SPELLBANK
setDemoSpellbookBankName($ELEMENTS_BANK_SP2, "Elements Spellbank");
addDemoSpellbookPlaceholder(LK_Placeholder_RPG,     $ELEMENTS_BANK_SP2, 0);
addDemoSpellbookPlaceholder(DBP_Placeholder_RPG,    $ELEMENTS_BANK_SP2, 1);
addDemoSpellbookPlaceholder(CoF_Placeholder_RPG,    $ELEMENTS_BANK_SP2, 2);
addDemoSpellbookPlaceholder(PoF_Placeholder_RPG,    $ELEMENTS_BANK_SP2, 3);
addDemoSpellbookPlaceholder(RoD_Placeholder_RPG,    $ELEMENTS_BANK_SP2, 4);
addDemoSpellbookPlaceholder(SN_Placeholder_RPG,     $ELEMENTS_BANK_SP2, 5);
addDemoSpellbookPlaceholder(UIF_Placeholder_RPG,    $ELEMENTS_BANK_SP2, 6);
addDemoSpellbookPlaceholder(WW_Placeholder_RPG,     $ELEMENTS_BANK_SP2, 7);
addDemoSpellbookPlaceholder(ICE_Placeholder_RPG,    $ELEMENTS_BANK_SP2, 8);
addDemoSpellbookPlaceholder(FTP_Placeholder_RPG,    $ELEMENTS_BANK_SP2, 9);
addDemoSpellbookPlaceholder(FPot_Placeholder_RPG,   $ELEMENTS_BANK_SP2, 10);

// SCI-FI SPELLBANK
setDemoSpellbookBankName($SCIFI_BANK, "SCI-FI Mode");
addDemoSpellbookPlaceholder(SF_OL_Placeholder_RPG,  $SCIFI_BANK, 0);
addDemoSpellbookPlaceholder(SF_OL2_Placeholder_RPG, $SCIFI_BANK, 1);
addDemoSpellbookPlaceholder(SF_OL3_Placeholder_RPG, $SCIFI_BANK, 2);

// UTILITY SPELLBANK
setDemoSpellbookBankName($UTILITY_BANK, "Utility Spellbank");
addDemoSpellbookPlaceholder(PCO_Placeholder_RPG,  $UTILITY_BANK, 0);
addDemoSpellbookPlaceholder(WVT_Placeholder_RPG,  $UTILITY_BANK, 1);
addDemoSpellbookPlaceholder(PHT_Placeholder_RPG,  $UTILITY_BANK, 2);
addDemoSpellbookPlaceholder(TAF_Placeholder_RPG,  $UTILITY_BANK, 3);

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

