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
 
using Wosad.Common.CalculationLogger.Interfaces;
using Wosad.Steel.AISC.Code;
using Wosad.Steel.AISC.Interfaces;
using Wosad.Steel.AISC.SteelEntities.Bolts;
using d = Wosad.Steel.AISC.AISC360_10.Connections.Bolted.BoltDescriptions;
using f = Wosad.Steel.AISC.AISC360_10.Connections.Bolted.BoltFormulas;
using v = Wosad.Steel.AISC.AISC360_10.Connections.Bolted.BoltValues;

namespace Wosad.Steel.AISC.AISC360_10.Connections.Bolted
{
    public abstract partial class BoltSlipCritical : Bolt, IBoltSlipCritical
    {
        public double GetSlipResistance()
        {
            double mu = GetSlipCoefficient();
            double Du =pretensionMultiplier;
            double hf = GetFactorForFillers();
            double Tb = T_b;
            int ns = numberOfSlipPlanes;

            //(J3-4)
            double R_n = mu * Du * hf * Tb * ns;

            
            ICalcLogEntry ent = Log.CreateNewEntry();

            ent.AddDependencyValue(v.mu, mu);
            ent.AddDependencyValue(v.Du, Du);
            ent.AddDependencyValue(v.hf, hf);
            ent.AddDependencyValue(v.Tb, Tb);
            ent.AddDependencyValue(v.ns, ns);

            ent.Reference = "AISC Formula J3-4";
            double phiR_n = 0;


                    double phi = GetPhiFactor();
                    ent.ValueName = v.phiRn;
                    ent.DescriptionReference = d.phiRn.SlipResistance;
                    ent.FormulaID = f.J3_4.LRFD;
                    phiR_n = R_n * phi;
                    

            ent.VariableValue = phiR_n.ToString();
            AddToLog(ent);
            return phiR_n;
        }

        internal double GetPhiFactor()
        {
            switch (HoleType)
            {
                case BoltHoleType.STD:
                    return 1.0;
                case BoltHoleType.SSL_Perpendicular:
                    return 1.0;
                case BoltHoleType.SSL_Parallel:
                    return 0.85;
                case BoltHoleType.OVS:
                    return 0.85;
                case BoltHoleType.LSL_Parallel:
                    return 0.7;
                case BoltHoleType.LSL_Perpendicular:
                    return 0.7;
                default:
                    return 0.85;
 
            }
        }

    }
}
