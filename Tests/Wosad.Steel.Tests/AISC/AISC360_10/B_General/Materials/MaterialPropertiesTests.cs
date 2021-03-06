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
using Wosad.Steel.AISC;
using Wosad.Steel.AISC.AISC360v10.Connections.BasePlate;
using Wosad.Steel.AISC.AISC360v10.Connections.Bolted;
using Wosad.Steel.AISC.SteelEntities.Bolts;

namespace Wosad.Steel.Tests.AISC.AISC360v10.Connections.BasePlate
{
    [TestFixture]
    public class MaterialPropertiesTests : ToleranceTestBase
    {

         [Test]
        public void MaterialReturnsF_y()
        {



            //double refValue = 1.51;
            //double actualTolerance = EvaluateActualTolerance(t_pMin, refValue);
            //Assert.LessOrEqual(actualTolerance, tolerance);
 
        }

         public MaterialPropertiesTests()
        {
            tolerance = 0.01; 
        }

        double tolerance;       
    }
}
