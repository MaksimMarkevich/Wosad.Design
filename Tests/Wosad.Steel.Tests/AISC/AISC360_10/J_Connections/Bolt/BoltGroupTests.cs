﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Steel.AISC.AISC360_10.Connections;

namespace Wosad.Steel.Tests.AISC.AISC360_10.J_Connections.Bolt
{

    [TestFixture]
    public class BoltGroupTests
    {
        public BoltGroupTests()
        {
            tolerance = 0.02; //2% can differ from rounding in the manual
        }

        double tolerance;
        /// <summary>
        /// AISC manual 14th Edition Table 7-6
        /// </summary>
        [Test]
        public void BoltGroupSingleLine0DegreesReturnsC()
        {
            BoltGroup bg = new BoltGroup(4, 1, 0, 3);
            double C = bg.FindUltimateStrengthCoefficient(8, 0);
            double refValue = 1.34; // from AISC Steel Manual
            double actualTolerance = EvaluateActualTolerance(C,refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        [Test]
        public void BoltGroupSingleLine45DegreesReturnsC()
        {
            BoltGroup bg = new BoltGroup(4, 1, 0, 3);
            double C = bg.FindUltimateStrengthCoefficient(8, 45);
            double refValue = 1.64; // from AISC Steel Manual
            double actualTolerance = EvaluateActualTolerance(C, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);
        }

        /// <summary>
        /// Elastic moment. Checked against spreadsheet calculation
        /// </summary>
        [Test]
        public void BoltGroupElasticReturnsMoment()
        {
            BoltGroup bg = new BoltGroup(4, 2, 3, 3);
            double C = bg.CalculateElasticGroupMomentCoefficientC();
            double boltStrength = 4.39205;
            double MomentCapacity = C * boltStrength;
            Assert.AreEqual(100.0, Math.Round(MomentCapacity));
        }

        private double EvaluateActualTolerance(double C, double refValue)
        {
            double smallerVal = C<refValue? C :refValue;
            double largerVal = C>=refValue? C :refValue;
            double thisTolerance = largerVal / smallerVal-1;

            return thisTolerance;
        }
    }
}
