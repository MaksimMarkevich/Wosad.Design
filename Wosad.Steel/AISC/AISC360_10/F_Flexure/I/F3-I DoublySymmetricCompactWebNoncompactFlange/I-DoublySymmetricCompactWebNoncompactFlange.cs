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

using Wosad.Steel.AISC.Exceptions;
using Wosad.Steel.AISC.SteelEntities;

namespace Wosad.Steel.AISC.AISC360_10.Flexure
{

    public partial class BeamIDoublySymmetricCompactWebNoncompactFlange : BeamIDoublySymmetricCompact
    {
        //This section applies to doubly symmetric I-shaped members bent about their major
        //axis having compact webs and noncompact or slender flanges as defined in Section
        //B4.1 for flexure.



        public BeamIDoublySymmetricCompactWebNoncompactFlange(ISteelSection section, bool IsRolled,
            double UnbracedLength, double EffectiveLengthFactor, ICalcLog CalcLog)
            : base(section,IsRolled, UnbracedLength, EffectiveLengthFactor,CalcLog)
        {
            GetSectionValues();
        }

        #region Limit States
        //Default implementations of limit states return 
        //empty limit states, set as Not Applicable
        //individual shape types must override  these default implementations

        public override SteelLimitStateValue GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            SteelLimitStateValue ls = new SteelLimitStateValue(-1, false);
            return ls;
        }

        public override SteelLimitStateValue GetFlexuralLateralTorsionalBucklingStrength(double C_b, FlexuralCompressionFiberPosition CompressionLocation)
        {
            return base.GetFlexuralLateralTorsionalBucklingStrength(C_b, CompressionLocation);
        }

        public virtual SteelLimitStateValue GetFlexuralFlangeLocalBucklingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
           double phiM_n =GetCompressionFlangeLocalBucklingCapacity();
           SteelLimitStateValue ls = new SteelLimitStateValue(phiM_n, true);
           return ls;
        }




        public virtual SteelLimitStateValue GetLimitingLengthForInelasticLTB_Lr(FlexuralCompressionFiberPosition CompressionLocation)
        {
            return base.GetLimitingLengthForInelasticLTB_Lr(CompressionLocation);
        }

        public virtual SteelLimitStateValue GetLimitingLengthForFullYielding_Lp(FlexuralCompressionFiberPosition CompressionLocation)
        {
            return base.GetLimitingLengthForFullYielding_Lp(CompressionLocation);
        }


        #endregion
    }
}
