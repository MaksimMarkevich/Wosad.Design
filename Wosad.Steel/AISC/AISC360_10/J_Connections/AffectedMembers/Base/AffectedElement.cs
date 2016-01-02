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

using Wosad.Common.Section.Interfaces; 
using Wosad.Steel.AISC.Interfaces;
using Wosad.Common.CalculationLogger.Interfaces;
using Wosad.Steel.AISC.SteelEntities;
using Wosad.Steel.AISC.SteelEntities.Sections;
using Wosad.Steel.AISC.SteelEntities.Materials;

namespace  Wosad.Steel.AISC360_10.Connections.AffectedElements
{
    public partial class AffectedElement: SteelDesignElement
    {
        public AffectedElement()
        {

        }
        public AffectedElement(double F_y, double F_u)
        {
            SteelMaterial material = new SteelMaterial(F_y, F_u, SteelConstants.ModulusOfElasticity, SteelConstants.ShearModulus);
            this.Section = new SteelGeneralSection(null, material);
        }
        public AffectedElement(ISteelSection Section, ICalcLog CalcLog)
            : base(CalcLog)
        {
            this.section = Section;
        }

        public AffectedElement(ISection Section, ISteelMaterial Material, ICalcLog CalcLog)
            :base(CalcLog)
        {
            this.section = new SteelGeneralSection(Section, Material); 
        }

        private ISteelSection section;

        public ISteelSection Section
        {
            get { return section; }
            set { section = value; }
        }
     
        
    }
}
