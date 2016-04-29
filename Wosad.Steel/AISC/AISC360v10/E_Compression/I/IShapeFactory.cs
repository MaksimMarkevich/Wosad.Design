﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.CalculationLogger;
using Wosad.Common.CalculationLogger.Interfaces;
using Wosad.Steel.AISC.AISC360v10.B_General;
using Wosad.Steel.AISC.AISC360v10.Compression;
using Wosad.Steel.AISC.AISC360v10.General.Compactness;
using Wosad.Steel.AISC.Interfaces;

namespace Wosad.Steel.AISC.AISC360v10.Compression
{
    public class IShapeFactory
    {
        public ISteelCompressionMember GetIshape(ISteelSection Section, bool IsRolled, double L_x, double L_y, double L_z, ICalcLog CalcLog)
        {

            ISteelCompressionMember column = null;
            IShapeCompactness compactnessTop = new ShapeCompactness.IShapeMember(Section, IsRolled, Common.Section.Interfaces.FlexuralCompressionFiberPosition.Top);
            IShapeCompactness compactnessBot = new ShapeCompactness.IShapeMember(Section, IsRolled, Common.Section.Interfaces.FlexuralCompressionFiberPosition.Bottom);
            ICalcLog Log = new CalcLog();


            CompactnessClassAxialCompression flangeCompactnessTop = compactnessTop.GetFlangeCompactnessCompression();
            CompactnessClassAxialCompression flangeCompactnessBot = compactnessTop.GetFlangeCompactnessCompression();
            CompactnessClassAxialCompression webCompactness = compactnessTop.GetWebCompactnessCompression();

            if (flangeCompactnessTop == CompactnessClassAxialCompression.NonSlender && webCompactness == CompactnessClassAxialCompression.NonSlender
                && flangeCompactnessBot == CompactnessClassAxialCompression.NonSlender)
            {
                column = new IShapeCompact(Section, IsRolled, L_x, L_y, L_z, Log);
                //if (IShape.b_fBot == IShape.b_fTop && IShape.t_fBot == IShape.t_fTop)
                //{
                //    return new IShapeCompact(SectionI, IsRolledShape, L_ex, L_ey, L_ez, log);
                //}
                //else
                //{
                //    throw new NotImplementedException();
                //}
            }
            else
            {
                column = new IShapeSlenderElements(Section, IsRolled, L_x, L_y, L_z, Log);
            }
            return column;
        }
    }
}
