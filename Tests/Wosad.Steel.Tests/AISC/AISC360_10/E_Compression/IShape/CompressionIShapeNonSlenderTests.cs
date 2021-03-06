﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.Entities;
using Wosad.Common.Section.Interfaces;
using Wosad.Common.Section.Predefined;
using Wosad.Steel.AISC.AISC360v10.Compression;
using Wosad.Steel.AISC.AISC360v10.Flexure;
using Wosad.Steel.AISC.Interfaces;
using Wosad.Steel.AISC.Steel.Entities;
using Wosad.Steel.AISC.SteelEntities;
using Wosad.Steel.AISC.SteelEntities.Materials;

namespace Wosad.Steel.Tests.AISC.AISC360v10.Compression
{

    [TestFixture]
    public class CompressionIShapeSlenderTests : ToleranceTestBase
    {
        public CompressionIShapeSlenderTests()
        {
            tolerance = 0.02; //2% can differ from rounding in the manual
        }

        double tolerance;


        ISteelCompressionMember column { get; set; }
        private void CreateColumn(double L_ex, double L_ey, double L_ez=0)
        {
            CompressionMemberFactory factory = new CompressionMemberFactory();
            AiscShapeFactory AiscShapeFactory = new AiscShapeFactory();
            ISection section = AiscShapeFactory.GetShape("W14X43", ShapeTypeSteel.IShapeRolled);
            SteelMaterial mat = new SteelMaterial(50.0,29000);
            L_ez = L_ez == 0? L_ex : L_ez;
            column = factory.GetCompressionMember(section,mat, L_ex, L_ey, L_ez);

        }
        /// <summary>
        /// AISC Steel Manual Table 4-1
        /// </summary>
        [Test]
        public void IShapeReturns_0ft_LengthAxialStrength() 
        {
            CreateColumn(0, 0);
            SteelLimitStateValue colFlexure = column.GetFlexuralBucklingStrength();
            double phiP_n = colFlexure.Value;
            double refValue = 562.0;
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        /// <summary>
        /// AISC Steel Manual Table 4-1
        /// </summary>
        [Test]
        public void IShapeReturns_16ft_LengthAxialStrength()
        {
            CreateColumn(16.0 * 12.0, 16.0 * 12.0);
            SteelLimitStateValue colFlexure = column.GetFlexuralBucklingStrength();
            double phiP_n = colFlexure.Value;
            double refValue = 267.0;
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        /// <summary>
        /// AISC Steel Manual Table 4-1
        /// </summary>
        [Test]
        public void IShapeReturns_30ft_LengthAxialStrength()
        {
            CreateColumn(30.0 * 12.0, 30.0 * 12.0);
            SteelLimitStateValue colFlexure = column.GetFlexuralBucklingStrength();
            double phiP_n = colFlexure.Value;
            double refValue = 78.5;
            double actualTolerance = EvaluateActualTolerance(phiP_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        }

    
}
