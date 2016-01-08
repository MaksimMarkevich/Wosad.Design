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
using Wosad.Common.Section.Interfaces;


namespace Wosad.Common.Section.Predefined
{
    /// <summary>
    /// Predefined I-section is used for I-shapes having known properties 
    /// from catalog (such as AISC shapes)
    /// </summary>
    public class PredefinedSectionI : SectionPredefinedBase, ISectionI
    {
        public PredefinedSectionI(ISection section)
            : base(section)
        {

        }
        public PredefinedSectionI(double Height,
        double FlangeCentroidDistance,
        double FlangeThicknessBottom,
        double FlangeThicknessTop,
        double FlangeWidthBottom,
        double FlangeWidthTop,
        double WebThickness,
        double FilletDistance, ISection section)
            : base(section)
        {
            this.height = Height;
            this.flangeCentroidDistance = FlangeCentroidDistance;
            this.flangeThicknessBottom = FlangeThicknessBottom;
            this.flangeThicknessTop = FlangeThicknessTop;
            this.flangeWidthBottom = FlangeWidthBottom;
            this.flangeWidthTop = FlangeWidthTop;
            this.webThickness = WebThickness;
            this.filletDistance = FilletDistance;
        }

        double height;

        public double d
        {
            get { return height; }
        }
        double flangeCentroidDistance;

        public double h_o
        {
            get { return flangeCentroidDistance; }
        }
        double flangeThicknessBottom;

        public double t_fBot
        {
            get { return flangeThicknessBottom; }
        }
        double flangeThicknessTop;

        public double t_fTop
        {
            get { return flangeThicknessTop; }
        }
        double flangeWidthBottom;

        public double b_fBot
        {
            get { return flangeWidthBottom; }
        }
        double flangeWidthTop;

        public double b_fTop
        {
            get { return flangeWidthTop; }
        }
        double webThickness;

        public double t_w
        {
            get { return webThickness; }
        }
        double filletDistance;

        public double FilletDistance
        {
            get { return filletDistance; }
        }


        public override ISection Clone()
        {
            throw new NotImplementedException();
        }


        public double h_web
        {
            get {

                return height - (t_fTop + t_fBot) - 2 * FilletDistance;
            }
        }
    }
}
