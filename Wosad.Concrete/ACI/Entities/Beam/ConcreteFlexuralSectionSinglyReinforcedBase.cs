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
using Wosad.Common.CalculationLogger;
using Wosad.Common.CalculationLogger.Interfaces;
using Wosad.Common.Entities;
using Wosad.Common.Section.Interfaces;
using Wosad.Concrete.ACI.Infrastructure.Entities.Rebar;
using Wosad.Concrete.ACI.Infrastructure.Entities.Section.Strains;
 

namespace Wosad.Concrete.ACI
{
    public class ConcreteFlexuralSectionSinglyReinforcedBase : ConcreteSectionBase, IConcreteFlexuralMember
    {
        public ConcreteFlexuralSectionSinglyReinforcedBase(IConcreteSectionRectangular Section, List<RebarPoint> LongitudinalBars, ICalcLog log)
            : base(Section,LongitudinalBars,log)
        {
            RectangularSection = Section;
        }

        IConcreteSectionRectangular RectangularSection;

        public SectionFlexuralAnalysisResult GetNominalFlexuralCapacity(FlexuralCompressionFiberPosition CompressionFiberPosition)
        {

            double Tforce = this.GetTForce();
            double DepthOfCompressionBlock_a = GetCompressionBlockDepth(Tforce, CompressionFiberPosition);
            double h = RectangularSection.Height;

            double d = this.Get_d();
            double Mn = Tforce * (d - DepthOfCompressionBlock_a / 2.0);
            double M = Mn / 12.0; //kip*ft 
            #region Mn
            ICalcLogEntry MnEntry = new CalcLogEntry();
            MnEntry.ValueName = "Mn";
            MnEntry.AddDependencyValue("F", Math.Round(Tforce, 3));
            MnEntry.AddDependencyValue("d", Math.Round(d, 3));
            MnEntry.AddDependencyValue("a", Math.Round(DepthOfCompressionBlock_a, 3));
            MnEntry.AddDependencyValue("M", Math.Round(M, 1));
            MnEntry.Reference = "";
            MnEntry.DescriptionReference = "/Templates/Concrete/ACI318_11/Flexure/NominalMomentCapacitySinglyReinforced.docx";
            MnEntry.FormulaID = null; //reference to formula from code
            MnEntry.VariableValue = Math.Round(Mn, 1).ToString();
            #endregion
            
            this.AddToLog(MnEntry);

            LinearStrainDistribution strainDistribution = GetStrainDistributionBasedOn_a(DepthOfCompressionBlock_a, CompressionFiberPosition);
            SectionFlexuralAnalysisResult Mn_result = new SectionFlexuralAnalysisResult(Mn, strainDistribution);
            return Mn_result;
        }

        double d;

        double Get_d()
        {
            double d = 0.0;
            double YMax = Section.SliceableShape.YMax;

            if (LongitudinalBars.Count > 1)
            {
                double As = LongitudinalBars.Sum(r => r.Rebar.Area);
                

                double SumAreaTimesDepth = LongitudinalBars.Sum(r => 
                {
                    double d_c = YMax - r.Coordinate.Y;
                    double Abar = r.Rebar.Area;
                    return Abar * d_c;
                });

                d = SumAreaTimesDepth / As;
            }
            else
            {
                RebarPoint rebar = LongitudinalBars.First();
                d = this.Section.SliceableShape.YMax-rebar.Coordinate.Y;
            }

            return d;
        }
        double GetTForce()
        {
            double Tforce = 0.0;
            if (LongitudinalBars.Count > 1)
            {
                double As = LongitudinalBars.Sum(a => a.Rebar.Area);
                Tforce = LongitudinalBars.Sum(a => a.Rebar.Area * a.Rebar.Material.YieldStress);

                #region Tforce
                ICalcLogEntry TforceEntry = new CalcLogEntry();
                TforceEntry.ValueName = "F";
                TforceEntry.Reference = "";
                TforceEntry.DescriptionReference = "/Templates/Concrete/ACI318_11/Flexure/SteelTensionForceMultipleBars.docx";
                TforceEntry.FormulaID = null; //reference to formula from code
                TforceEntry.VariableValue = Math.Round(Tforce, 3).ToString();
                #endregion
                this.AddToLog(TforceEntry);
            }
            else
            {
                RebarPoint rebar = LongitudinalBars.First();
                double As = rebar.Rebar.Area;
                double Fy = rebar.Rebar.Material.YieldStress;

                Tforce = As * Fy;

                #region Tforce
                ICalcLogEntry TforceEntry = new CalcLogEntry();
                TforceEntry.ValueName = "Tforce";
                TforceEntry.AddDependencyValue("A", Math.Round(As, 3));
                TforceEntry.AddDependencyValue("fy", Math.Round(Fy, 3));
                TforceEntry.Reference = "";
                TforceEntry.DescriptionReference = "/Templates/Concrete/ACI318_11/Flexure/SteelTensionForce.docx";
                TforceEntry.FormulaID = null; //reference to formula from code
                TforceEntry.VariableValue = Math.Round(Tforce, 3).ToString();
                #endregion
                this.AddToLog(TforceEntry);
            }

            return Tforce;
        }
    }
}