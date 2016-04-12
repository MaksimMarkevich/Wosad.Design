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
using Wosad.Steel.AISC.Interfaces;
 

namespace  Wosad.Steel.AISC360v10.HSS.ConcentratedForces
{
    public abstract partial class ChsToPlateConnection: HssToPlateConnection
    {
        public ISteelSection GetHssSteelSection()
        {
            ISteelSection s = Hss.Section as ISteelSection;
            if (s==null)
            {
                throw new Exception("Hss must implement ISteelSection interface");   
            }
            return s;
        }

        public double GetChordStressInteractionQf(bool ConnectingSurfaceInTension)
        {
            return this.GetChordStressInteractionQf(0.0, ConnectingSurfaceInTension);
        }

        public double GetChordStressInteractionQf(double RequiredAxialStrenghPro, double RequiredMomentStrengthMro, bool ConnectingSurfaceInTension)
        {
            double U = GetUtilizationRatio(Hss, RequiredAxialStrenghPro, RequiredMomentStrengthMro);
            return this.GetChordStressInteractionQf( U,  ConnectingSurfaceInTension);
        }


        internal double GetChordStressInteractionQf( double HssUtilizationRatio, bool ConnectingSurfaceInTension)
        {
            double U = HssUtilizationRatio;
            double Qf = 0.0;

            if (ConnectingSurfaceInTension==false)
            {
                    Qf =  1.0-0.3*U*(1 + U);
            }
            else
            {
                Qf = 1.0;
            }
            return Qf;
        }
    }
}
