﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.CalculationLogger.Interfaces;
using Wosad.Common.Exceptions;
using Wosad.Common.Section.Interfaces;
using Wosad.Steel.AISC.AISC360v10.B_General;
using Wosad.Steel.AISC.AISC360v10.General.Compactness;
using Wosad.Steel.AISC.Interfaces;

namespace Wosad.Steel.AISC.AISC360v10.Flexure
{
    public class SinglySymmetricIBeam
    {
        public SinglySymmetricIBeam(ISteelSection section, bool IsRolledMember, FlexuralCompressionFiberPosition compressionFiberPosition, ICalcLog CalcLog)
        {
            this.section = section;
            this.IsRolledMember = IsRolledMember;
            this.CalcLog = CalcLog;
            this.compressionFiberPosition = compressionFiberPosition;
        }



        FlexuralCompressionFiberPosition compressionFiberPosition;
        ISteelSection section;
        bool IsRolledMember;
        ICalcLog CalcLog;


        public ISteelBeamFlexure GetBeamCase()
        {
            ISteelBeamFlexure beam = null;
            IShapeCompactness compactness = new ShapeCompactness.IShapeMember(section, IsRolledMember, compressionFiberPosition);

            CompactnessClassFlexure flangeCompactness = compactness.GetFlangeCompactnessFlexure();
            CompactnessClassFlexure webCompactness = compactness.GetWebCompactnessFlexure();



                if (webCompactness == CompactnessClassFlexure.Compact || webCompactness == CompactnessClassFlexure.Noncompact)
                {
                    if (flangeCompactness == CompactnessClassFlexure.Compact)
                    {
                        return new BeamINoncompactWebCompactFlange(section, IsRolledMember, CalcLog);
                    }
                    else
                    {
                        return new BeamINoncompactWeb(section, IsRolledMember, CalcLog);
                    }
                }

                else 
                {
                    if (flangeCompactness == CompactnessClassFlexure.Compact)
                    {
                        return new BeamISlenderWebCompactFlange(section, IsRolledMember, CalcLog);
                    }
                    else
                    {
                        return new BeamISlenderWeb(section, IsRolledMember, CalcLog);
                    }

                }

            
            return beam;

        }



        private IShapeCompactness compactness;

        public IShapeCompactness Compactness
        {
            get
            {
                if (compactness == null)
                {
                    compactness = GetCompactness();
                }
                return compactness;
            }
        }

        private IShapeCompactness GetCompactness()
        {
            ISectionI Isec = section as ISectionI;
            if (Isec != null)
            {
                compactness = new ShapeCompactness.IShapeMember(section, IsRolledMember, compressionFiberPosition);
            }
            else
            {
                throw new ShapeTypeNotSupportedException(" flexural calculation of I-beam");
            }
            return compactness;
        }
	

    }
}
