﻿#region Copyright
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
using System.Threading.Tasks;
using Wosad.Common.Section.Interfaces;
using Wosad.Steel.AISC.Interfaces;

namespace Wosad.Steel.AISC.AISC360_10.Connections
{
    public abstract partial class BeamCopeBase : IBeamCope
    {
        protected double GetFcrGeneral()
        {
            double lambda = GetLambda();
            double F_y = Material.YieldStress;

            double Q;
            if (lambda<=0.7)
            {
                Q = 1.0;
            }
            else if (lambda<=1.41)
            {
                Q = (1.34 - 0.468 * lambda);
            }
            else
            {
                Q = ((1.3) / (Math.Pow(lambda, 2)));
            }
            double F_cr = F_y * Q;
            return F_cr;
        }
        public abstract double Get_t_w();

    
        private double _t_w;

        public double t_w
        {
            get {
                if (_t_w == 0.0)
                {
                    _t_w = Get_t_w();
                }
                return _d; }

        }

        private double GetLambda()
        {
            double F_y = Material.YieldStress;
            double lamda = ((h_o * Math.Sqrt(F_y)) / (10 * t_w * Math.Sqrt(475 + 280 * Math.Pow((((h_o) / (c))), 2))));
            return lamda;
        }
    }
}
