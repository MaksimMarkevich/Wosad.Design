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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.CalculationLogger;
using Wosad.Common.Entities;
using Wosad.Common.Section.Interfaces;
using Wosad.Common.Section.Predefined;
using Wosad.Common.Section.SectionTypes;
using Wosad.Steel.AISC.Interfaces;
using Wosad.Steel.AISC.SteelEntities.Materials;
using Wosad.Steel.AISC.SteelEntities.Sections;

namespace Wosad.Steel.AISC.AISC360v10.Compression
{
    public class CompressionMemberFactory
    {
        public ISteelCompressionMember GetCompressionMember(ISection section,  double L_ex,  double L_ey, double L_ez, double F_y, double E)
        {
            throw new NotImplementedException();
            string DEFAULT_EXCEPTION_STRING = "Selected shape is not supported. Select a different shape.";
            ISteelCompressionMember col = null;
            CalcLog log = new CalcLog();
            SteelMaterial mat = new SteelMaterial(F_y);


            //switch (shapeType)
            //{
            //    case ShapeTypeSteel.Angle:
            //        throw new Exception(DEFAULT_EXCEPTION_STRING);
            //        break;
            //    case ShapeTypeSteel.Box:
            //        throw new Exception(DEFAULT_EXCEPTION_STRING);
            //        break;
            //    case ShapeTypeSteel.Channel:
            //        throw new Exception(DEFAULT_EXCEPTION_STRING);
            //        break;
            //    case ShapeTypeSteel.Circular:
            //        throw new Exception(DEFAULT_EXCEPTION_STRING);
            //        break;
            //    case ShapeTypeSteel.CircularHSS:
            //        break;
            //    case ShapeTypeSteel.DoubleAngle:
            //        throw new Exception(DEFAULT_EXCEPTION_STRING);
            //        break;
            //    case ShapeTypeSteel.IShapeAsym:
            //        throw new Exception(DEFAULT_EXCEPTION_STRING);
            //        break;
            //    case ShapeTypeSteel.IShapeBuiltUp:
            //        throw new Exception(DEFAULT_EXCEPTION_STRING);
            //        break;
            //    case ShapeTypeSteel.IShapeRolled:
            //        ISectionI secI = section as ISectionI;
            //        if (secI != null)
            //        {
            //            SteelSectionI sectionI = new SteelSectionI(secI, mat);
            //            col = new CompressionMemberIDoublySymmetric(sectionI, true, L_x, L_y, K_x, K_y, log);
            //        }

            //        break;
            //    case ShapeTypeSteel.Rectangular:
            //        break;
            //    case ShapeTypeSteel.RectangularHSS:
            //        ISectionTube secTube = section as ISectionTube;
            //        if (secTube != null)
            //        {
            //            SteelRhsSection sectionTube = new SteelRhsSection(secTube, mat);
            //            col = new CompressionMemberRhs(sectionTube, L_x, L_y, K_x, K_y, log);
            //        }
            //        break;
            //    case ShapeTypeSteel.TeeBuiltUp:
            //        throw new Exception(DEFAULT_EXCEPTION_STRING);
            //        break;
            //    case ShapeTypeSteel.TeeRolled:
            //        throw new Exception(DEFAULT_EXCEPTION_STRING);
            //        break;
            //    default:
            //        break;
            //}
            //return col;
        }

    }
    
}
