﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Mocks;
using NUnit.Framework;
using Wosad.Common.CalculationLogger.Interfaces;
using Wosad.Concrete.ACI;
using Wosad.Concrete.ACI318_14;
using Wosad.Concrete.ACI318_14.Materials;
using Wosad.Concrete.ACI.Entities;
using Wosad.Concrete.ACI.Infrastructure.Entities.Rebar;


namespace Wosad.Analytics.ACI318_14.Tests.Flexure
{
    [TestFixture]
    public partial class RectangularBeamTests
    {
        private ICalcLog log;

        public ICalcLog Log
        {
            get { return log; }
            set { log = value; }
        }

        private IConcreteMaterial mat;

        public IConcreteMaterial Material
        {
            get
            {
                if (mat == null)
                {
                    mat = new ConcreteMaterial(4000, ConcreteTypeByWeight.Normalweight, log);
                }
                return mat;
            }
            set { mat = value; }
        }

        public RectangularBeamTests()
        {
            //ICalcLogEntry entryStub = mocks.Stub<ICalcLogEntry>();
            MockRepository mocks = new MockRepository();
            log = mocks.Stub<ICalcLog>();
        }

        public IConcreteMaterial GetConcreteMaterial(double fc)
        {
            ConcreteMaterial concrete = new ConcreteMaterial(fc, ConcreteTypeByWeight.Normalweight, log);
            return concrete;
        }

        public IConcreteSection GetRectangularSection(double Width, double Height, double fc)
        {
            IConcreteMaterial mat = GetConcreteMaterial(fc);
            CrossSectionRectangularShape section = new CrossSectionRectangularShape(mat, null, Width, Height);
            return section;
        }

        public ConcreteSectionFlexure GetConcreteBeam(double Width, double Height, double fc, params RebarInput[] rebarInput)
        {

            IConcreteSection Section = GetRectangularSection(Width, Height, fc);

            List<RebarPoint> LongitudinalBars = new List<RebarPoint>();
            foreach (var bar in rebarInput)
            {
                Rebar thisBar = new Rebar(bar.Area, new MaterialAstmA615(A615Grade.Grade60));
                RebarPoint point = new RebarPoint(thisBar, new RebarCoordinate() { X = 0, Y = -Height / 2.0 + bar.Cover });
                LongitudinalBars.Add(point);
            }

            ConcreteSectionFlexure beam = new ConcreteSectionFlexure(Section,LongitudinalBars, log);
            return beam;
        }
    }


}
