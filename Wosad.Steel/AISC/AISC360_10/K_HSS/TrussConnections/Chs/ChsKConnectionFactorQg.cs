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

namespace  Wosad.Steel.AISC.AISC360_10.HSS.TrussConnections
{
    public abstract  partial class ChsKConnection : ChsNonOverlapConnection
    {
        public double GetConnectionFactorQg()
        {
            double Qg=0.0;
            double gamma = GetChordSlendernessRatio();
            double g = this.gap;
            double t = GetChordWallThickness();

            if (t==0.0)
            {
                throw new Exception("Wall thickness cannot be 0");
            }

            //(K2-6)
            Qg = Math.Pow(gamma,0.2)*(1.0+0.024*Math.Pow(gamma,1.2)/(Math.Exp(0.5*g/t-1.33)+1.0));

            return Qg;
        }
    }
}