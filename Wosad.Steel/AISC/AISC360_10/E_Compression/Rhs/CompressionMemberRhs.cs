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


namespace Wosad.Steel.AISC.AISC360_10.Compression
{
    public partial class CompressionMemberRhs : ColumnDoublySymmetric
    {

        public override double CalculateCriticalStress()
        {
            double Fcr = 0.0;

            //Flexural

            double FeFlexuralBuckling = GetElasticBucklingStressFe(); //this does not apply to unsymmetric sections
            double FcrFlexuralBuckling = GetCriticalStressFcr(FeFlexuralBuckling, 1.0);
            double Qflex = GetReductionFactorQ(FcrFlexuralBuckling);
            double FcrFlex = GetCriticalStressFcr(FeFlexuralBuckling, Qflex);

            return FcrFlex;

        }

        public CompressionMemberRhs(ISteelSection Section, double L_x, double L_y, double K_x, double K_y, ICalcLog CalcLog)
            : base(Section,L_x,L_y,K_x,K_y, CalcLog)
        {
            if (Section.Shape is ISectionTube)
            {
                SectionRhs = Section.Shape as ISectionTube;
            }
            else
            {
                throw new Exception("Section of wrong type: Need ISectionTube");
            }

        }


        ISectionTube SectionRhs; 
    }
}
