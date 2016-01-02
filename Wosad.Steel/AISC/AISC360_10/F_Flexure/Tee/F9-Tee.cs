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



namespace Wosad.Steel.AISC.AISC360_10.Flexure
{
    public partial class BeamTee : FlexuralMemberTeeBase
    {
        public BeamTee(ISteelSection section,
            double UnbracedLength, double EffectiveLengthFactor, ICalcLog CalcLog)
            : base(section, UnbracedLength, EffectiveLengthFactor,CalcLog)
        {
            
            GetSectionValues();
        }

        ISectionTee sectionTee;

        public override double GetFlexuralCapacityMajorAxis( FlexuralCompressionFiberPosition compressionFiberLocation = FlexuralCompressionFiberPosition.Top)
        {
            double MYielding = GetYieldingMomentCapacity(compressionFiberLocation);
            double MFlb = GetCompressionFlangeLocalBucklingCapacity(compressionFiberLocation);
            double MLtb= GetFlexuralTorsionalBucklingMomentCapacity(compressionFiberLocation);
            double MStemLb = GetCompressedStemLocalBucklingCapacity(compressionFiberLocation);
            double[] CapacityValues = new double[4] { MYielding, MFlb, MLtb, MStemLb };
            double Mn = CapacityValues.Min();
            return GetFlexuralDesignValue(Mn);
        }

        public override double GetFlexuralCapacityMinorAxis(FlexuralCompressionFiberPosition compressionFiberLocation = FlexuralCompressionFiberPosition.Top)
        {
            throw new NotImplementedException();
            //read AISC commentary on this
        }


        internal void GetSectionValues()
        {

            E = Section.Material.ModulusOfElasticity;
            Fy = Section.Material.YieldStress;

            L = this.UnbracedLengthFlexure;
            K = this.EffectiveLengthFactorFlexure;

            Lb = this.EffectiveLengthFactorFlexure * this.UnbracedLengthFlexure;


        }

        double Lb;
        double E;
        double Fy;

        double L;
        double K;


    }
}
