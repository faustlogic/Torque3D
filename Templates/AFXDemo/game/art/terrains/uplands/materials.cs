
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

singleton Material( TerrainSoft )
{
   FootstepSoundId = 0;
   ImpactSoundId = 0;
   terrainMaterials = "1";
   ShowDust = false;
   ShowFootprints = true;
   materialTag0 = "Terrain";
};

singleton Material( TerrainHard )
{
   FootstepSoundId = 1;
   ImpactSoundId = 1;
   terrainMaterials = "1";
   ShowDust = true;
   ShowFootprints = true;
   materialTag0 = "Terrain";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

singleton Material( TerrainDirtGrass : TerrainSoft )
{
   MapTo = "dirt_grass";
};

singleton Material( TerrainGrass1Dry : TerrainSoft )
{
   MapTo = "grass1-dry";
};

singleton Material( TerrainGrass1 : TerrainSoft )
{
   MapTo = "grass1";
};

singleton Material( TerrainGrass2 : TerrainSoft )
{
   MapTo = "grass2";
};

singleton Material( TerrainRocks : TerrainHard )
{
   MapTo = "rocks1";
   EffectColor[ 0 ] = "0.5 0.5 0.5 1.0";
   EffectColor[ 1 ] = "1.0 1.0 1.0 0.0";
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//