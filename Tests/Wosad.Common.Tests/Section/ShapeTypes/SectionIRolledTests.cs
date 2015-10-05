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
using Wosad.Common.Section.SectionTypes;

namespace Wosad.Common.Tests.Section.ShapeTypes
{
    /// <summary>
    /// Compare calculated properties to W18X35 listed properties.
    /// </summary>
     [TestFixture]
    public class SectionIRolledTests
    {

         [Test]
         public void SectionIRolledReturnsArea()
         {
             SectionIRolled shape = new SectionIRolled("", 17.7, 6.0, 0.425, 0.3, 0.827);
             double A = shape.Area;
             Assert.AreEqual(10.74, Math.Round(A, 2));
             //Manual gives 10.3 but actual area checked in Autocad is 10.42
         }

         [Test]
         public void SectionIRolledReturnsIx()
         {
             SectionIRolled shape = new SectionIRolled("", 17.7, 6.0, 0.425, 0.3, 0.827);
             double Ix = shape.MomentOfInertiaX;
             Assert.AreEqual(540.05, Math.Round(Ix, 2));
             //Manual gives 510 but actual area checked in Autocad is 540.0505
         }

         [Test]
         public void SectionIRolledReturnsIy()
         {
             SectionIRolled shape = new SectionIRolled("", 17.7, 6.0, 0.425, 0.3, 0.827);
             double Ix = shape.MomentOfInertiaY;
             Assert.AreEqual(15.42, Math.Round(Ix, 2));
             //Checked in Autocad is 540.0505
         }

    }
}
