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
using Wosad.Steel.AISC.SteelEntities.Sections;
using Wosad.Steel.AISC.SteelEntities;
using Wosad.Steel.AISC.Steel.Entities.Sections;
using Wosad.Steel.AISC.Steel.Entities;


namespace  Wosad.Steel.AISC360v10.HSS.ConcentratedForces
{
    public partial class RhsCapPlate : RhsToPlateConnection, IHssCapPlateConnection
    {
        public RhsCapPlate(SteelRhsSection Hss, SteelPlateSection Plate, double t_plCap, ICalcLog CalcLog, bool IsTensionHss, double P_uHss, double M_uHss)
            : base(Hss, Plate, CalcLog, IsTensionHss,P_uHss,M_uHss)
        {
            this.t_plCap = t_plCap;
        }

       public  double t_plCap { get; set; }


       public SteelLimitStateValue GetHssYieldingOrCrippling()
       {
           double phiR_n = GetAvailableStrength();
           return new SteelLimitStateValue(phiR_n, true);
       }
    }
}
