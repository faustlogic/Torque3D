
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// Arcane-FX for MIT Licensed Open Source version of Torque 3D from GarageGames
//
// This script implements client-side functionality for AFX including the
// loading if space paths from a text file, and server-callable commands
// for displaying on screen text messages.
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

function afxLoadPathsFile(%file, %name)
{
  %file = expandFilename(%file);
  if (!isFile(%file))
  { 
    error("afxLoadPathsFile():" SPC %file SPC "does not exist.");
    return -1;
  }

  %fp = new FileObject();
  if (!%fp.openForRead(%file))
  {
    error("afxLoadPathsFile():" SPC %file SPC "failed to open.");
    %fp.delete();
    return -1;
  }

  %obj = new ScriptObject(%name);
  %obj.count = 0;

  while (!%fp.isEOF()) 
  {
    %line = ltrim(%fp.readLine());
    // skip BLANKS and COMMENTS
    if (%line !$= "" && getSubStr(%line, 0, 2) !$= "//") 
    {
      %obj.paths[%obj.count] = %line;
      %obj.count++;
    }
  }
  %fp.close();
  %fp.delete();

  if (%obj.count == 0)
  {
    error("afxLoadPathsFile():" SPC %file SPC "failed to load any paths.");
    %obj.delete();
    return -1;
  }

  return %obj;
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
