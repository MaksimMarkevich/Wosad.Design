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
 
 
 using Wosad.Common.CalculationLogger;
using Wosad.Common.CalculationLogger.Interfaces; 
using Wosad.Steel.AISC.Interfaces;

using Wosad.Steel.AISC.Exceptions;
using Wosad.Steel.AISC.SteelEntities;


namespace Wosad.Steel.AISC.AISC360_10.Flexure
{
    public partial class BeamIDoublySymmetricNoncompactWeb : FlexuralMemberIBase
    {
        ISectionI SectionI;

        public BeamIDoublySymmetricNoncompactWeb(ISteelSection section, bool IsRolledMember,
            double UnbracedLength, double EffectiveLengthFactor, ICalcLog CalcLog)
            : base(section, IsRolledMember, UnbracedLength, EffectiveLengthFactor, CalcLog)
        {

            SectionI = this.Section as ISectionI;
            if (section == null)
            {
                throw new SectionWrongTypeException(typeof(ISectionI));
            }

            GetSectionValues();
        }


    //This section applies to doubly symmetric I-shaped members bent about their major
    //axis with noncompact webs and singly symmetric I-shaped members with webs
    //attached to the mid-width of the flanges, bent about their major axis, with compact
    //or noncompact webs, as defined in Section B4.1 for flexure.



        #region Limit States
        //Default implementations of limit states return 
        //empty limit states, set as Not Applicable
        //individual shape types must override  these default implementations

        public override SteelLimitStateValue GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            SteelLimitStateValue ls = new SteelLimitStateValue(-1, false); // not applicable
            return ls;
        }

        public override SteelLimitStateValue GetFlexuralLateralTorsionalBucklingStrength(double C_b, FlexuralCompressionFiberPosition CompressionLocation)
        {
            double phiM_n = GetLateralTorsionalBucklingCapacity(CompressionLocation, Cb);
            SteelLimitStateValue ls = new SteelLimitStateValue(phiM_n, true);
            return ls;
        }

        public override SteelLimitStateValue GetFlexuralFlangeLocalBucklingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            double phiM_n = GetCompressionFlangeLocalBucklingCapacity(CompressionLocation);
            SteelLimitStateValue ls = new SteelLimitStateValue(phiM_n, true); 
            return ls;
        }

        public override SteelLimitStateValue GetFlexuralTensionFlangeYieldingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            double phiM_n = GetTensionFlangeYieldingCapacity(CompressionLocation);
            SteelLimitStateValue ls = new SteelLimitStateValue(phiM_n, true); 
            return ls;
        }

        public override SteelLimitStateValue GetFlexuralCompressionFlangeYieldingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            double phiM_n = GetCompressionFlangeYieldingCapacity(CompressionLocation);
            SteelLimitStateValue ls = new SteelLimitStateValue(phiM_n, true);
            return ls;

        }



        public override SteelLimitStateValue GetLimitingLengthForInelasticLTB_Lr(FlexuralCompressionFiberPosition CompressionLocation)
        {
            double rt = GetEffectiveRadiusOfGyrationrt(CompressionLocation);
            double Lr = GetL_r();
            SteelLimitStateValue ls = new SteelLimitStateValue(Lr, true);
            return ls;
        }

        public override SteelLimitStateValue GetLimitingLengthForFullYielding_Lp(FlexuralCompressionFiberPosition CompressionLocation)
        {
            double r_t = GetEffectiveRadiusOfGyrationrt(CompressionLocation);
            double L_p = GetL_p(r_t);
            SteelLimitStateValue ls = new SteelLimitStateValue(L_p, true);
            return ls;
        }


        #endregion

        //public double GetFlexuralCapacityMajorAxis(FlexuralCompressionFiberPosition compressionFiberPosition, double Cb)
        //{
           
        //    double CompressionFlangeYielding = GetCompressionFlangeYieldingCapacity(compressionFiberPosition);
        //    double Ltb = GetLateralTorsionalBucklingCapacity(compressionFiberPosition, Cb);
        //    double CompressionFlangeLocalBuckling = GetCompressionFlangeLocalBucklingCapacity(compressionFiberPosition);
        //    double TensionFlangeYielding = GetTensionFlangeYieldingCapacity(compressionFiberPosition);

        //    double[] CapacityValues = new double[4] { CompressionFlangeYielding ,Ltb, CompressionFlangeLocalBuckling,TensionFlangeYielding};

        //    double Mn = CapacityValues.Min();

        //    return GetFlexuralDesignValue(Mn);
        //}

        //public override double GetFlexuralCapacityMajorAxis(FlexuralCompressionFiberPosition compressionFiberPosition)
        //{
        //    double Cb = 1.0;
        //    return this.GetFlexuralCapacityMajorAxis(compressionFiberPosition,Cb);
        //}


        internal void GetSectionValues()
        {

            E = Section.Material.ModulusOfElasticity;
            Fy = Section.Material.YieldStress;

            L = this.UnbracedLengthFlexure;
            K = this.EffectiveLengthFactorFlexure;

            Iy = Section.Shape.I_y;

            Sxbot = Section.Shape.S_xBot;
            Sxtop = Section.Shape.S_xTop;

            Sx = Math.Min(Sxbot, Sxtop);

            Zx = Section.Shape.Z_x;

            ry = Section.Shape.r_y;

            Cw = Section.Shape.C_w;

            J = Section.Shape.J;
            Lb = this.EffectiveLengthFactorFlexure * this.UnbracedLengthFlexure;

        }

        double Lb;
        double E;
        double Fy;

        double L;
        double K;
        double Iy;

        double Sxbot;
        double Sxtop;

        double Sx;
        double Zx;
        double ry;
        double Cw;
        double J;
        double c;
        double ho;




    }
}
