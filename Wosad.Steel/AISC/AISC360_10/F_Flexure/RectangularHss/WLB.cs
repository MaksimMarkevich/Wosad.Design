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
 
using Wosad.Steel.AISC.AISC360_10.General.Compactness;





namespace Wosad.Steel.AISC.AISC360_10.Flexure
{
    public partial class BeamRectangularHss : FlexuralMemberRhsBase, ISteelBeamFlexure
    {

        public double GetWebLocalBucklingCapacity(MomentAxis MomentAxis,
            FlexuralCompressionFiberPosition CompressionLocation)
        {
            double M_n = 0.0;
            double S;
            double Mp;

            if (MomentAxis ==MomentAxis.XAxis)
            {
                Mp = GetMajorPlasticMomentCapacity().Value;
                S = Math.Min(Section.Shape.S_yLeft, Section.Shape.S_yRight);
            }
            else if (MomentAxis == MomentAxis.YAxis)
            {
                 Mp = GetMinorPlasticMomentCapacity().Value;
                 S = Math.Min(Section.Shape.S_xTop, Section.Shape.S_xBot);
            }
            else
            {
                throw new FlexuralBendingAxisException();
            }


                double lambdaWeb = GetLambdaWeb(MomentAxis);
                M_n = Mp - (Mp - Fy * S) * (0.305 * lambdaWeb * Math.Sqrt(Fy / E) - 0.738); //(F7-5)
                M_n = M_n > Mp ? Mp : M_n;

                double phiM_n = 0.9 * M_n;
            return phiM_n;
        }

    }
}
