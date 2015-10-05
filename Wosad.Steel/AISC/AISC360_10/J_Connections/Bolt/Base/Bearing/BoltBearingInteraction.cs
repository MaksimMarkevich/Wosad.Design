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
using p = Wosad.Steel.AISC.AISC360_10.Connections.Bolted.BoltParagraphs;
using f = Wosad.Steel.AISC.AISC360_10.Connections.Bolted.BoltFormulas;
using v = Wosad.Steel.AISC.AISC360_10.Connections.Bolted.BoltValues;
using d = Wosad.Steel.AISC.AISC360_10.Connections.Bolted.BoltDescriptions;
using Wosad.Common.CalculationLogger.Interfaces; 
using Wosad.Steel.AISC.Code;
using Wosad.Common.Interfaces;

namespace Wosad.Steel.AISC.AISC360_10.Connections.Bolted
{
    public abstract partial class BoltBearing : Bolt, IBoltBearing
    {

        public double GetAvailableTensileStrength(double V)
        {
            double F_pp_nt = 0.0;
            double Fnt = NominalTensileStress;
            double Fnv = NominalShearStress;
            //double frv = GetRequiredShearStress();
            double Ab = Area;
            double frv = V / Ab;
            double R = 0.0;

            // check if interaction needs to be investigated:
            double f_available;

            if (DesignFormat == SteelDesignFormat.LRFD)
            {
                f_available = 0.75 * Fnv;

            }
            else
            {
                f_available = Fnv/2.0;
            }

            if (f_available * 0.3 > frv)
            {
                //interation need not be investigated
                return f_available * Ab;
            }


            // interaction 
            ICalcLogEntry ent = Log.CreateNewEntry();
            ent.ValueName = v.F_pp_nt;
            ent.AddDependencyValue(v.Fnt, Fnt);

            ent.DescriptionReference = d.F_pp_nt;
            AddToLog(ent);

            if (DesignFormat == SteelDesignFormat.LRFD)
            {
                ent.Reference = "AISC Formula J3-3a";
                ent.FormulaID = f.J3_3.a;
                F_pp_nt = 1.3 * Fnt - Fnt / (0.75 * Fnv) * frv;

            }
            else
            {
                ent.Reference = "AISC Formula J3-3b";
                ent.FormulaID = f.J3_3.b;
                F_pp_nt = 1.3 * Fnt - (Fnt * 2.0)/Fnv * frv;
            }

            F_pp_nt = CheckMaximumAvailableTensileStrength(F_pp_nt);

            ent.VariableValue = F_pp_nt.ToString();


            ICalcLogEntry ent2 = Log.CreateNewEntry();
            ent2.AddDependencyValue(v.F_pp_nt, F_pp_nt);
            ent2.AddDependencyValue(v.Ab,Ab);
            ent2.Reference = "AISC Formula J3-2";

            if (DesignFormat== SteelDesignFormat.LRFD)
            {
                ent2.DescriptionReference = d.phiRn.AvailableTensileStrengthForCombinedLoad;
                ent2.FormulaID = f.J3_2.LRFD;
                R = 0.75* F_pp_nt * Ab;
            }
            else
            {
                ent2.DescriptionReference = d.Rn_Omega.AvailableTensileStrengthForCombinedLoad;
                ent2.FormulaID = f.J3_2.ASD;
                R = F_pp_nt * Ab / 2.0;
            }
            ent2.ValueName = v.R;
            ent2.VariableValue = R.ToString();
            AddToLog(ent2);


            return R;
        }

        internal double GetRequiredShearStress()
        {

            double frv = 0.0;
            double F2 = Math.Abs(this.FindMaximumForce(ForceType.F2, true).F2);
            double F3 = Math.Abs(this.FindMaximumForce(ForceType.F3, true).F3);
            double V = Math.Max(F2, F3);
            double A = this.Area;
            
            if (A!=0.0)
            {
                frv = V / A;

                ICalcLogEntry ent = Log.CreateNewEntry();
                ent.ValueName = v.frv;
                ent.AddDependencyValue(v.Ab, A);
                ent.AddDependencyValue(v.Vb, V);
                ent.DescriptionReference = d.frv;
                ent.FormulaID = f.frv;
                ent.VariableValue = frv.ToString();
                AddToLog(ent);
            }
            else
            {
                throw new Exception("Bolt load information not available");
            }

            return frv;
        }

        internal double CheckMaximumAvailableTensileStrength(double F_pp_nt)
        {
            double Fnt = NominalTensileStress;
            if (F_pp_nt>Fnt)
            {
                ICalcLogEntry ent = Log.CreateNewEntry();
                ent.ValueName = v.F_pp_nt;
                ent.AddDependencyValue(v.Fnt, Fnt);
                ent.DescriptionReference = d.F_pp_nt;
                ent.FormulaID = f.F_pp_ntMax;
                ent.VariableValue = F_pp_nt.ToString();
                AddToLog(ent);
                return Fnt;
            }
            return F_pp_nt;
        }
    }
}
