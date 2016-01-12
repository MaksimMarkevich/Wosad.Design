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
using System.Text; using Wosad.Common.Entities; using Wosad.Common.Section.Interfaces; using Wosad.Steel.AISC.Interfaces;

namespace  Wosad.Steel.AISC.AISC360_10.HSS.TrussConnections
{
    public class HssKConnectionLoadCaseData : HssConnectionLoadCaseData
    {
        public HssKConnectionLoadCaseData(string LoadCaseName, 
            HssTrussConnectionBranch CompressionBranch,
            HssTrussConnectionBranch TensionBranch)
            :base(LoadCaseName)
        {
            this.compressionBranch = CompressionBranch;
            this.tensionBranch = TensionBranch;
        }

        private HssTrussConnectionBranch compressionBranch;

        public HssTrussConnectionBranch CompressionBranch
        {
            get { return compressionBranch; }
            set { compressionBranch = value; }
        }

        private HssTrussConnectionBranch tensionBranch;

        public HssTrussConnectionBranch TensionBranch
        {
            get { return tensionBranch; }
            set { tensionBranch = value; }
        }
        

        
        
        
        
    }
}
