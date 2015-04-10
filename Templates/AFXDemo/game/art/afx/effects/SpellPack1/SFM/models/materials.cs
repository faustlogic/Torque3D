
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// SUMMON FECKLESS MOTH (Materials)
//    This file contains shader and material definitions compatible
//    with the T3D engine.
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

// CATERPILLAR

new Material(SFM_caterpillar_head_MTL)
{
   mapTo = "SFM_caterpillar_head.png";
   diffuseMap[0] = "SFM_caterpillar_head";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = true;
};

new Material(SFM_caterpillar_headseg_MTL)
{
   mapTo = "SFM_caterpillar_headsegment.png";
   diffuseMap[0] = "SFM_caterpillar_headsegment";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = true;
};

new Material(SFM_caterpillar_hair_MTL)
{
   mapTo = "SFM_caterpillar_hair.png";
   diffuseMap[0] = "SFM_caterpillar_hair";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

new Material(SFM_caterpillar_seg_MTL)
{
   mapTo = "SFM_caterpillar_segment.png";
   diffuseMap[0] = "SFM_caterpillar_segment";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = true;
};

// TINY MOTHS

new Material(SFM_tmoth_body_MTL)
{
   mapTo = "SFM_tmoth_body.png";
   diffuseMap[0] = "SFM_tmoth_body";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

new Material(SFM_tmoth_body_blue_MTL)
{
   mapTo = "SFM_tmoth_body_blue.png";
   diffuseMap[0] = "SFM_tmoth_body_blue";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

new Material(SFM_tmoth_body_green_MTL)
{
   mapTo = "SFM_tmoth_body_green.png";
   diffuseMap[0] = "SFM_tmoth_body_green";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

new Material(SFM_tmoth_body_magenta_MTL)
{
   mapTo = "SFM_tmoth_body_magenta.png";
   diffuseMap[0] = "SFM_tmoth_body_magenta";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

new Material(SFM_tmoth_body_orange_MTL)
{
   mapTo = "SFM_tmoth_body_orange.png";
   diffuseMap[0] = "SFM_tmoth_body_orange";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

new Material(SFM_tmoth_wings_MTL)
{
   mapTo = "SFM_tmoth_wings.png";
   diffuseMap[0] = "SFM_tmoth_wings";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

new Material(SFM_tmoth_wings_blue_MTL)
{
   mapTo = "SFM_tmoth_wings_blue.png";
   diffuseMap[0] = "SFM_tmoth_wings_blue";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

new Material(SFM_tmoth_wings_green_MTL)
{
   mapTo = "SFM_tmoth_wings_green.png";
   diffuseMap[0] = "SFM_tmoth_wings_green";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

new Material(SFM_tmoth_wings_magenta_MTL)
{
   mapTo = "SFM_tmoth_wings_magenta.png";
   diffuseMap[0] = "SFM_tmoth_wings_magenta";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

new Material(SFM_tmoth_wings_orange_MTL)
{
   mapTo = "SFM_tmoth_wings_orange.png";
   diffuseMap[0] = "SFM_tmoth_wings_orange";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

// COCOON

new Material(SFM_cocoon_top_MTL)
{
   mapTo = "SFM_cocoon_top.png";
   diffuseMap[0] = "SFM_cocoon_top";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(SFM_cocoon_bot_MTL)
{
   mapTo = "SFM_cocoon_bot.png";
   diffuseMap[0] = "SFM_cocoon_bot";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

// LARVA
new Material(SFM_larvaA2_MTL)
{
   mapTo = "SFM_larvaA2.png";
   diffuseMap[0] = "SFM_larvaA2";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(SFM_larvaA_MTL)
{
   mapTo = "SFM_larvaA.png";
   diffuseMap[0] = "SFM_larvaA";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(SFM_larvaB2_MTL)
{
   mapTo = "SFM_larvaB2.png";
   diffuseMap[0] = "SFM_larvaB2";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(SFM_larvaB_MTL)
{
   mapTo = "SFM_larvaB.png";
   diffuseMap[0] = "SFM_larvaB";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

new Material(SFM_larvaC_MTL)
{
   mapTo = "SFM_larvaC.png";
   diffuseMap[0] = "SFM_larvaC";
   emissive[0] = true;
   translucent = true;
   translucentBlendOp = AddAlpha;
   castShadows = false;
};

// BIG MOTH

new Material(SFM_mothbody_MTL)
{
   mapTo = "SFM_mothbody.png";
   diffuseMap[0] = "SFM_mothbody";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = true;
};

new Material(SFM_mothhead_MTL)
{
   mapTo = "SFM_mothhead.png";
   diffuseMap[0] = "SFM_mothhead";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = true;
};

new Material(SFM_mothlegs_MTL)
{
   mapTo = "SFM_mothlegs.png";
   diffuseMap[0] = "SFM_mothlegs";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = true;
};

new Material(SFM_mothwing_lower_MTL)
{
   mapTo = "SFM_mothwing_lower.png";
   diffuseMap[0] = "SFM_mothwing_lower";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = true;
};

new Material(SFM_mothwing_upper_MTL)
{
   mapTo = "SFM_mothwing_upper.png";
   diffuseMap[0] = "SFM_mothwing_upper";
   translucent = false;
   translucentZWrite = true;
   translucentBlendOp = LerpAlpha;
   castShadows = true;
};

new Material(SFM_mothwing_blur_MTL)
{
   mapTo = "SFM_mothwing_blur.png";
   diffuseMap[0] = "SFM_mothwing_blur";
   translucent = true;
   translucentZWrite = false;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

new Material(SFM_mothwing_lower_trans1_MTL)
{
   mapTo = "SFM_mothwing_lower_trans1.png";
   diffuseMap[0] = "SFM_mothwing_lower_trans1";
   translucent = true;
   translucentZWrite = false;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

new Material(SFM_mothwing_lower_trans2_MTL)
{
   mapTo = "SFM_mothwing_lower_trans2.png";
   diffuseMap[0] = "SFM_mothwing_lower_trans2";
   translucent = true;
   translucentZWrite = false;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

new Material(SFM_mothwing_upper_trans1_MTL)
{
   mapTo = "SFM_mothwing_upper_trans1.png";
   diffuseMap[0] = "SFM_mothwing_upper_trans1";
   translucent = true;
   translucentZWrite = false;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

new Material(SFM_mothwing_upper_trans2_MTL)
{
   mapTo = "SFM_mothwing_upper_trans2.png";
   diffuseMap[0] = "SFM_mothwing_upper_trans2";
   translucent = true;
   translucentZWrite = false;
   translucentBlendOp = LerpAlpha;
   castShadows = false;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

