#region Copyright
   /*Copyright (C) 2015 Wosad Inc

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
   */
#endregion
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Wosad.Common.Entities; 
using Wosad.Common.Section.Interfaces; 
using Wosad.Steel.AISC.Interfaces;
 
 
using Wosad.Common.CalculationLogger.Interfaces; 
using Wosad.Steel.AISC.Interfaces;
using Wosad.Common.Section.Interfaces;
using Wosad.Steel.AISC.Exceptions;
using Wosad.Steel.AISC.Code;

namespace  Wosad.Steel.AISC.AISC360_10.HSS.TrussConnections
{
    public abstract partial class ChsTrussConnection : HssTrussConnection
    {

        public ChsTrussConnection(HssTrussConnectionChord chord, List<HssTrussConnectionBranch> branches, SteelDesignFormat DesignFormat, ICalcLog CalcLog)
            :base(chord, branches, DesignFormat, CalcLog)
        {
        }

        internal ISectionPipe GetChordSection()
        {
            ISectionPipe chord = Chord.Section as ISectionPipe;
            if (chord == null)
            {
                throw new SectionWrongTypeException(typeof(ISectionPipe));
            }
            return chord;
        }

        internal double GetChordDiameter()
        {
            ISectionPipe chordSec = GetChordSection();
            return chordSec.Diameter;
        }


        internal double GetChordWallThickness()
        {
            ISectionPipe chordSec = GetChordSection();
            return chordSec.DesignWallThickness;
        }


        internal ISectionPipe GetBranchSection(HssTrussConnectionBranch branch)
        {
            ISectionPipe chord = branch.Section as ISectionPipe;
            if (chord == null)
            {
                throw new SectionWrongTypeException(typeof(ISectionPipe));
            }
            return chord;
        }

        internal List<ISectionPipe> GetBranchSections()
        {
            List<ISectionPipe> pipeSections = new List<ISectionPipe>();
            foreach (var branch in Branches)
            {
                pipeSections.Add(GetBranchSection(branch));
            }
            return pipeSections;
        }

        protected HssTrussConnectionBranch GetBranch (string Id)
        {
            foreach (var b in Branches)
	        {
		    if (b.ID == Id)
	        {
		         return b;
	        }
            
	     }
            return null;
        }

        internal bool DetermineIfChordIsInTension()
        {
            //TODO
            return true;
        }

   }
}
