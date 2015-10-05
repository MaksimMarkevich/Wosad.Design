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
using Wosad.Steel.AISC.Exceptions;




namespace Wosad.Steel.AISC.AISC360_10.Flexure
{
    public partial class BeamIDoublySymmetricCompactWebNoncompactFlange : BeamIDoublySymmetricCompact
    {
        //Compression Flange Local Buckling F3.2

        public double GetCompressionFlangeLocalBucklingCapacity()
        {
            double Mn = 0.0;
            ISectionI sectionI = Section as ISectionI;
            if (sectionI!=null)
	            {
                ShapeCompactness.ShapeIMember compactness = new ShapeCompactness.ShapeIMember(Section, IsRolledMember,FlexuralCompressionFiberPosition.Top);
                CompactnessClassFlexure cClass = compactness.GetFlangeCompactnessFlexure();
                if (cClass== CompactnessClassFlexure.Noncompact || cClass== CompactnessClassFlexure.Slender)
                {
                    double lambda = this.GetLambdaCompressionFlange(FlexuralCompressionFiberPosition.Top);
                    double lambdapf = this.GetLambdapf(FlexuralCompressionFiberPosition.Top);
                    double lambdarf = this.GetLambdarf(FlexuralCompressionFiberPosition.Top);
                    //note: section is doubly symmetric so top flange is taken

                    double Sx = this.Section.SectionBase.SectionModulusXTop;
                    double Fy = this.Section.Material.YieldStress;
                    double E = this.Section.Material.ModulusOfElasticity;
                    //double Zx = Section.SectionBase.PlasticSectionModulusX;

                    if (cClass== CompactnessClassFlexure.Noncompact)
	                {
                        double Mp = this.GetMajorPlasticMomentCapacity().Value;
		                 Mn = Mp - (Mp - 0.7 * Fy * Sx) * ((lambda - lambdapf) / (lambdarf - lambdapf)); //(F3-1)
	                }
                      else
	                {
                        double kc = this.Getkc();
                        Mn = 0.9 * E * kc * Sx / Math.Pow(lambda, 2); //(F3-2)
	                }

                }
                else
                {

                }
                if (cClass == CompactnessClassFlexure.Compact)
                {
                    throw new LimitStateNotApplicableException("Flange local buckling");
                }

	            }

            return Mn;
        }
    }
}
