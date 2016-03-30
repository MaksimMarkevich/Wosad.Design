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
using Wosad.Steel.AISC.SteelEntities.Members;


namespace Wosad.Steel.AISC.AISC360v10.Flexure
{
    public abstract partial class FlexuralMember : SteelFlexuralMember, ISteelBeamFlexure
    {
        internal LateralTorsionalBucklingType GetLateralTorsionalBucklingType(double Lb, double Lp, double Lr)
        {
            LateralTorsionalBucklingType retType;
            if (Lb <= Lp)
            {
                retType = LateralTorsionalBucklingType.NotApplicable;
            }
            if (Lb > Lp && Lb <= Lr)
            {
                retType = LateralTorsionalBucklingType.Inelastic;
            }
            else
            {
                retType = LateralTorsionalBucklingType.Elastic;
            }
            return retType;
        }

        public enum LateralTorsionalBucklingType
        {
            NotApplicable,
            Inelastic,
            Elastic
        }
    }
}
