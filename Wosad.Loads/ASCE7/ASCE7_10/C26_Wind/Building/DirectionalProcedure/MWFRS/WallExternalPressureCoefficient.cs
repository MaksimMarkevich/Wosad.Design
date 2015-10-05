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
using Wosad.Common.CalculationLogger; 
using Wosad.Common.CalculationLogger.Interfaces;
using Wosad.Common.CalculationLogger; 
using Wosad.Common.CalculationLogger.Interfaces;
using Wosad.Common.Mathematics;
using Wosad.Loads.ASCE7.Entities;

namespace Wosad.Loads.ASCE.ASCE7_10.WindLoads.Building.DirectionalProcedure.MWFRS
{
    public partial class Mwfrs : BuildingDirectionalProcedureElement
    {
        public double GetWallPressureCoefficient(WindFace face, double B, double L)
        {
            double Cp = 0.0;
            ICalcLogEntry CpEntry = new CalcLogEntry();
            CpEntry.ValueName = "Cp";

            switch (face)
            {
                case WindFace.Windward:
                    Cp = 0.8;

                    CpEntry.Reference = "";
                    CpEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/PressureCoefficient/WindCpWallMWFRSWindward.docx";
                    CpEntry.FormulaID = null; //reference to formula from code

                    

                    break;
                case WindFace.Leeward:
                    Cp = GetLeewardPressure(B, L);
                    CpEntry.AddDependencyValue("B", Math.Round(B, 3));
                    CpEntry.AddDependencyValue("L", Math.Round(L, 3));
                    CpEntry.AddDependencyValue("LB", Math.Round(L/B, 3));

                    CpEntry.Reference = "";
                    CpEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/PressureCoefficient/WindCpWallMWFRSLeeward.docx";
                    CpEntry.FormulaID = null; //reference to formula from code

                    break;
                case WindFace.Side:
                    Cp = 0.7;
                    
                    CpEntry.Reference = "";
                    CpEntry.DescriptionReference = "/Templates/Loads/ASCE7_10/Wind/PressureCoefficient/WindCpWallMWFRSside.docx";
                    CpEntry.FormulaID = null; //reference to formula from code

                    break;
                default:
                    break;
            }
            CpEntry.VariableValue = Math.Round(Cp, 3).ToString();
            this.AddToLog(CpEntry);
            return Cp;
            
        }

        private double GetLeewardPressure(double B, double L)
        {
            double CpLeeward = 0.0;
            if (B!=0)
            {
                double LBRatio = L / B;
                if (LBRatio<=1)
                {
                    CpLeeward = -0.5;
                }
                else if (LBRatio>=4.0)
                {
                    CpLeeward = -0.2;
                }
                else
                {
                    if (LBRatio<=2.0) //between 1 and 2
                    {
                        if (LBRatio==2.0)
                        {
                            CpLeeward = -0.3;
                        }
                        else
                        {
                            CpLeeward = Interpolation.InterpolateLinear(1.0, -0.5, 2.0, -0.3, LBRatio);
                        }
                        
                    }
                    else //between 2 and 4
                    {
                        CpLeeward = Interpolation.InterpolateLinear(2.0, -0.3, 4.0, -0.2, LBRatio);

                    }
                }
            }
            return CpLeeward;
        }
    }
}
