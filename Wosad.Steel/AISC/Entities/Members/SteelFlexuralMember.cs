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
using Wosad.Common.Entities;
using Wosad.Steel.AISC.Code;
using Wosad.Steel.AISC.Interfaces;


namespace Wosad.Steel.AISC.SteelEntities.Members
{
    public abstract class SteelFlexuralMember : SteelMemberBase, ISteelFlexuralMember
    {
        public double UnbracedLengthFlexure { get; set; }
        public double EffectiveLengthFactorFlexure { get; set; }

        public SteelFlexuralMember(ISteelSection Section, ICalcLog CalcLog)
            :this(Section,0,1, CalcLog)
        {

        }

        public SteelFlexuralMember(ISteelSection Section,
            double UnbracedLength, double EffectiveLengthFactor, ICalcLog CalcLog) //, ISteelMaterial Material)
            : base(Section, CalcLog)//, Material)
        {
            this.EffectiveLengthFactorFlexure = EffectiveLengthFactor;
            this.UnbracedLengthFlexure = UnbracedLength;
        }

        public abstract double GetFlexuralCapacityMajorAxis( FlexuralCompressionFiberPosition compressionFiberLocation);
        public abstract double GetFlexuralCapacityMinorAxis( FlexuralCompressionFiberPosition compressionFiberLocation);
    }
}
