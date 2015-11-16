
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

singleton CubemapData(AFX_DaySky_Cubemap)
{
   cubeFace[0] = "./cubemap/skybox_1";
   cubeFace[1] = "./cubemap/skybox_2";
   cubeFace[2] = "./cubemap/skybox_3";
   cubeFace[3] = "./cubemap/skybox_4";
   cubeFace[4] = "./cubemap/skybox_5";
   cubeFace[5] = "./cubemap/skybox_6";
};

singleton Material(AFX_DaySky_MTL)
{
   cubemap = AFX_DaySky_Cubemap;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

singleton CubemapData(AFX_NightSky_Cubemap)
{
   cubeFace[0] = "./cubemap/afx_darksky_LF";
   cubeFace[1] = "./cubemap/afx_darksky_RT";
   cubeFace[2] = "./cubemap/afx_darksky_BK";
   cubeFace[3] = "./cubemap/afx_darksky_FR";
   cubeFace[4] = "./cubemap/afx_darksky_UP";
   cubeFace[5] = "./cubemap/afx_darksky_DN";
};

singleton Material(AFX_NightSky_MTL)
{
   cubemap = AFX_NightSky_Cubemap;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

singleton CubemapData(AFX_Sandstorm_Cubemap)
{
   cubeFace[0] = "./cubemap/sandstorm_LF";
   cubeFace[1] = "./cubemap/sandstorm_RT";
   cubeFace[2] = "./cubemap/sandstorm_BK";
   cubeFace[3] = "./cubemap/sandstorm_FR";
   cubeFace[4] = "./cubemap/sandstorm_UP";
   cubeFace[5] = "./cubemap/sandstorm_DN";
};

singleton Material(AFX_Sandstorm_MTL)
{
   cubemap = AFX_Sandstorm_Cubemap;
};

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
