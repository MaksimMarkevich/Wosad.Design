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
using Wosad.Common.Entities;


namespace Wosad.Wood.NDS.NDS_2015
{
    public abstract partial class WoodMember : AnalyticalElement
    {
        public virtual double GetColumnStabilityFactor(
            double FcE, double FcStar, double c)
        {

            double alpha = FcE / FcStar;
            double Cp = 1.0 + alpha / (2.0 * c) - Math.Sqrt(Math.Pow((1 + alpha) / 2 * c, 2.0) - alpha / c);

            return Cp;

        }


    }
}
