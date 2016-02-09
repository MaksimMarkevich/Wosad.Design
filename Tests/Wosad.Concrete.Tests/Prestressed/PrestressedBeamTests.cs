﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Wosad.Concrete.ACI318_14;
using Wosad.Concrete.ACI;
using Wosad.Analytics.ACI318_14.Tests.Prestressed;
using Wosad.Common.Section.Interfaces;
using Wosad.Analytics.ACI318_14.Tests.Flexure;


namespace Wosad.Analytics.ACI318_14.Tests.Prestressed
{
    //public class PrestressedRectangularBeamTests : PrestressedRectangularTestBase
    //{
    //    [Test]
    //    public void GetSimpleBeamFlexuralCapacityTop()
    //    {
    //        PrestressedConcreteSection beam = GetRectangularPrestressedConcreteBeam(12, 12, 4000, 3500, new RebarInput(1, 1));
    //        SectionFlexuralAnalysisResult MResult = beam.GetNominalFlexuralCapacity(FlexuralCompressionFiberPosition.Top, FlexuralAnalysisType.StrainCompatibility);
    //        double phiMn = MResult.Moment;
    //        Assert.AreEqual(615882, Math.Round(Mn, 0));
    //    }
    //}

    public class PrestressedGeneralBeamTests : PrestressedGeneralTestsBase
    {
        //[Test]
        //public void CheckTeeServiceability()
        //{
            //from PCA notes on ACI examples
            //PrestressedConcreteSection beam = GetGeneralPrestressedConcreteBeam
            //   (4000, 3500, new RebarInput(1, 1));
            //SectionFlexuralAnalysisResult MResult = beam.GetFlexuralCapacity(CompressionLocation.Top, FlexuralAnalysisType.StrainCompatibility);
            //double phiMn = MResult.Moment;
            //Assert.AreEqual(615882, Math.Round(Mn, 0));
        //}
    }
}
