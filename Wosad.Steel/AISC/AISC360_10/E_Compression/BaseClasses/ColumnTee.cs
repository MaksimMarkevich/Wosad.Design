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
using Wosad.Steel.AISC.Interfaces;
using Wosad.Steel.AISC.Code;
 

namespace Wosad.Steel.AISC.AISC360_10.Compression
{
    public abstract class ColumnTee: ColumnFlexuralAndTorsionalBuckling
    {
        public ColumnTee(ISteelSection Section, ICalcLog CalcLog)
            : base(Section,CalcLog)
        {

        }
        //public override double GetTorsionalElasticBucklingStressFe()
        //{
        //    double pi2 = Math.Pow(Math.PI, 2);
        //    double E = Material.ModulusOfElasticity;

        //    double Kz = EffectiveLengthFactorZ;
        //    double Lz = UnbracedLengthZ;

        //    double G = 11200; //ksi
        //    double J = Section.TorsionalConstant;
        //    double Ix = Section.MomentOfInertiaX;
        //    double Iy = Section.MomentOfInertiaY;


        //    throw new NotFiniteNumberException();
        //}
    }
}
