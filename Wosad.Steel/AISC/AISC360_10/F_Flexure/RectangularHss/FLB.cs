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
 
using Wosad.Steel.AISC.AISC360_10.General.Compactness;
using Wosad.Steel.AISC.SteelEntities;





namespace Wosad.Steel.AISC.AISC360_10.Flexure
{
    public partial class BeamRectangularHss : FlexuralMemberRhsBase, ISteelBeamFlexure
    {
        //Compression Flange Local Buckling F7.2


        public double GetCompressionFlangeLocalBucklingCapacity(FlexuralCompressionFiberPosition CompressionLocation, MomentAxis MomentAxis)
        {
            double Mn = 0.0;
            double phiM_n = 0.0;

            if (SectionTube != null)
            {

                    double lambda = this.GetLambdaCompressionFlange(CompressionLocation, MomentAxis);
                    double lambdapf = this.GetLambdapf(CompressionLocation, MomentAxis);
                    double lambdarf = this.GetLambdarf(CompressionLocation, MomentAxis);
                    //note: section is doubly symmetric so top flange is taken

                    double Sx = this.Section.Shape.S_xTop;
                    double Fy = this.Section.Material.YieldStress;
                    double E = this.Section.Material.ModulusOfElasticity;

                    CompactnessClassFlexure cClass = GetFlangeCompactness();

                    if (cClass == CompactnessClassFlexure.Noncompact)
                    {
                        double Mp = this.GetMajorPlasticMomentCapacity().Value;
                        Mn = GetMnNoncompact(Mp, Fy, Sx, lambda);

                    }
                    else
                    {
                        double Sxe = GetEffectiveSectionModulusX();
                        Mn = GetMnSlender(Sxe, Fy);
                    }

                
            }
            phiM_n = 0.9 * Mn;
            return phiM_n;
        }

        private double GetMnNoncompact(double Mp, double Fy, double S, double lambda )
        {
            double Mn = Mp - (Mp - Fy * S) * (3.57 * lambda * Math.Sqrt(Fy / E) - 4.0); //(F7-2)
            Mn = Mn > Mp ? Mp : Mn;
            return Mn;
        }
        
        private double GetMnSlender(double Se, double Fy)
        {
            double Mn = Fy * Se; //(F7-3)
            return Mn;
        }
    }
}
