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
using Wosad.Common.Section.Interfaces;
using Wosad.Steel.AISC.Code;
using Wosad.Steel.AISC.Interfaces;
using Wosad.Steel.AISC.SteelEntities;
 
 

namespace  Wosad.Steel.AISC360_10.HSS.ConcentratedForces
{


    public partial class HssToPlateConnection : SteelDesignElement 
    {
        public double GetUtilizationRatio(ISteelSection Section, double RequiredAxialStrenghPro, double RequiredMomentStrengthMro)
        {
            double U = 0;
            double Fy = Section.Material.YieldStress;
            double Fc = 0.0;

                Fc = Fy;


            ISection sec = Section.SectionBase;
            double Ag = sec.Area;
            double S = Math.Min(sec.SectionModulusXBot, sec.SectionModulusXTop);
            double Pro = RequiredAxialStrenghPro;
            double Mro = RequiredMomentStrengthMro;
            //(K1-6) from TABLE K1.2
            U=Math.Abs(Pro/(Fc*Ag)+Mro/(Fc*S));
            return U;
        }

    }
}
