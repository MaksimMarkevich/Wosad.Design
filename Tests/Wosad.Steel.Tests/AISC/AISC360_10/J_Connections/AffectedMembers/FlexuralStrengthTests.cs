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
 
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.CalculationLogger;
using Wosad.Common.CalculationLogger.Interfaces;
using Wosad.Common.Section.SectionTypes;
using Wosad.Steel.AISC.AISC360v10.Connections.AffectedMembers;
using Wosad.Steel.AISC.Interfaces;
using Wosad.Steel.AISC.SteelEntities.Materials;

namespace Wosad.Steel.Tests.AISC.AISC360v10.Connections.AffectedMembers
{
    [TestFixture]
    public class FlexuralStrengthTests
    {
        [Test]
        public void ConnectedPlateReturnsFlexuralStrength()
        {
            ICalcLog log = new  CalcLog();
            SectionRectangular Section = new SectionRectangular(0.5, 8);
            ISteelMaterial Material = new SteelMaterial(50);
            AffectedElementInFlexure element = new AffectedElementInFlexure(Section, Material, log);
            double phiM_n = element.GetFlexuralStrength();
            Assert.AreEqual(360.0, phiM_n);
        }
    }
}
