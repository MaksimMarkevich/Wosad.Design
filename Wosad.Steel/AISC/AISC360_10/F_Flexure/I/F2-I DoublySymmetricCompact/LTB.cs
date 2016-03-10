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
using Wosad.Common.CalculationLogger;

using Wosad.Steel.AISC.Exceptions;
 using Wosad.Common.CalculationLogger;

namespace Wosad.Steel.AISC.AISC360_10.Flexure
{
    public partial class BeamIDoublySymmetricCompact : BeamIDoublySymmetricBase
    {

        public virtual double Get_c()
        {
            double c = 1.0;


            return c;
        }


        public virtual double Get_ho()
        {
            double ho;

            ISectionI section = this.Section.Shape as ISectionI;
            if (section != null)
            {
                //ho = section.h_o;
                ho = section.d - (section.t_fTop / 2.0 + section.t_fBot / 2.0);
            }
            else
            {
                throw new SectionWrongTypeException(typeof(ISectionI));
            }
            return ho;
        }

        //Lateral Torsional Buckling F2.2
        public double GetFlexuralTorsionalBucklingMomentCapacity(double Cb)
        {

            double Lp = GetLp(ry, E, Fy); //(F2-5)

            double rts = Getrts(Iy, Cw, Sx);

            double Lr = GetLr(rts, E, Fy, Sx, J, c, ho);  // (F2-6)


            LateralTorsionalBucklingType BucklingType = GetLateralTorsionalBucklingType(Lb, Lp, Lr);
            double M_p;
            double M_n = 0.0;


            switch (BucklingType)
            {

                case LateralTorsionalBucklingType.NotApplicable:
                    M_n = double.PositiveInfinity;
                    break;
                case LateralTorsionalBucklingType.Inelastic:

                    M_p = GetYieldingMomentCapacity();
                    M_n = Cb * (M_p - (0.7 * Fy * Sx) * ((Lb - Lp) / (Lr - Lp))); //(F2-2)
                    M_n = M_n > M_p ? M_p : M_n;
                    break;
                case LateralTorsionalBucklingType.Elastic:
                    double Fcr = GetFcr(Cb, E, Lb, rts, J, c, Sx, ho);
                    M_n = Fcr * Sx; //(F2-3)
                    M_p = GetYieldingMomentCapacity();
                    M_n = M_n > M_p ? M_p : M_n;
                    break;
            }


            double phiM_n =  M_n * 0.9;
            return phiM_n;

        }

        public double GetLp(double ry, double E, double Fy)
        {
            double Lp = 1.76 * ry * Math.Sqrt(E / Fy); //(F2-5)


            return Lp;
        }

        public double GetLr(double rts, double E, double Fy, double Sx, double J, double c, double ho)
        {
            double Lr = 1.95 * rts * E / (0.7 * Fy) * Math.Sqrt((J * c / (Sx * ho)) + Math.Sqrt(Math.Pow(J * c / (Sx * ho), 2.0) + 6.76 * Math.Pow(0.7 * Fy / E, 2.0)));  // (F2-6)
            

            return Lr;
        }

        public double Getrts(double Iy, double Cw, double Sx)
        {
            double rts = Math.Sqrt(Math.Sqrt(Iy * Cw) / Sx);
            
            return rts;
        }

        public double GetFcr(double Cb, double E, double Lb, double rts, double J, double c, double Sx, double ho)
        {
            double Fcr;
            double pi2 = Math.Pow(Math.PI, 2);
            Fcr = Cb * pi2 * E / (Math.Pow(Lb / rts, 2)) * Math.Sqrt(1.0 + 0.078 * J * c / (Sx * ho) * Math.Pow(Lb / rts, 2)); //(F2-4)
            return Fcr;
        }


    }
}
