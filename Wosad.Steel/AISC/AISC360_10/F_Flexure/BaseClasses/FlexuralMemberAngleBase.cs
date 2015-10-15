#region Copyright
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
using Wosad.Common.Entities; 
using Wosad.Common.Section.Interfaces; 
using Wosad.Steel.AISC.Interfaces;
using Wosad.Steel.AISC.AISC360_10.General.Compactness;
using Wosad.Common.CalculationLogger.Interfaces; 
using Wosad.Steel.AISC.Interfaces;
 using Wosad.Common.CalculationLogger;
using Wosad.Steel.AISC.Exceptions;
using Wosad.Steel.AISC.Code;


namespace Wosad.Steel.AISC.AISC360_10.Flexure
{
    public abstract class FlexuralMemberAngleBase: FlexuralMember
    {
        public FlexuralMemberAngleBase(ISteelSection section, 
            double UnbracedLength, double EffectiveLengthFactor, ICalcLog CalcLog)
            : base(section, UnbracedLength, EffectiveLengthFactor, CalcLog)
        {
            sectionAngle = null;
            ISectionAngle s = Section as ISectionAngle;

            if (s == null)
            {
                throw new SectionWrongTypeException(typeof(ISectionAngle));
            }
            else
            {
                sectionAngle = s;
                compactness = new ShapeCompactness.AngleMember(Section);
            }
        }

        ShapeCompactness.AngleMember compactness;

        private ISectionAngle sectionAngle;

        public ISectionAngle SectionAngle
        {
            get { return sectionAngle; }
            set { sectionAngle = value; }
        }


    }
}
