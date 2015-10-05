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
using System.Text; using Wosad.Common.Entities; using Wosad.Common.Section.Interfaces; using Wosad.Steel.AISC.Interfaces;
using Wosad.Common.Section.Interfaces;
using Wosad.Steel.AISC.AISC360_10.General.Compactness;
using Wosad.Steel.AISC.Exceptions;
using Wosad.Steel.AISC.Interfaces;
 
 

namespace Wosad.Steel.AISC.AISC360_10.General.Compactness
{
    public class FlangeOfRolledIShape: UnstiffenedElementCompactness
    {
        public FlangeOfRolledIShape(ISteelMaterial Material, double Overhang, double Thickness)
            :base(Material,Overhang,Thickness)
        {
            
        }

        public FlangeOfRolledIShape(ISteelMaterial Material, ISectionI s, ElementLocation location)
            :base(Material)
        {
            double bf=0;
            double tf=0;

            switch (location)
            {
                case ElementLocation.Top:
                    bf = s.FlangeWidthTop;
                    tf = s.FlangeThicknessTop;
                    break;
                case ElementLocation.Bottom:
                    bf = s.FlangeWidthTop;
                    tf = s.FlangeThicknessTop;
                    break;
                default:
                    throw new Exception("Invalid location is specified for I-beam flange");
            }

            base.Overhang = bf;
            base.Thickness = tf;
        }

        public FlangeOfRolledIShape(ISteelMaterial Material): base (Material)
        {

        }

        public override double GetLambda_r(StressType stress)
        {
            if (stress== StressType.Flexure)
            {
                return 1.0 * SqrtE_Fy();
            }
            else
            {
                return 0.56 * SqrtE_Fy();
            }
        }

        public override double GetLambda_p(StressType stress)
        {
            if (stress == StressType.Flexure)
            {
                return 0.38 * SqrtE_Fy();
            }
            else
            {
                throw new ShapeParameterNotApplicableException("Lambda_p");
            }
            
        }
    }
}
