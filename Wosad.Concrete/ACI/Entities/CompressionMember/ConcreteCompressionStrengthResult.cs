﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wosad.Concrete.ACI
{
    public class ConcreteCompressionStrengthResult: ConcreteFlexuralStrengthResult
    {
        public ConcreteCompressionStrengthResult(double a, double phiM_n, FlexuralFailureModeClassification FlexuralFailureModeClassification, double epsilon_t) 
            : base ( a,  phiM_n,  FlexuralFailureModeClassification,  epsilon_t) 
        {

        }
    }
}
