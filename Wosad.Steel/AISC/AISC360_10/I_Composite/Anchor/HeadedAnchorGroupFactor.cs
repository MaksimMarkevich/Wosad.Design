﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wosad.Common.CalculationLogger.Interfaces;
using Wosad.Common.Entities;

namespace Wosad.Steel.AISC.AISC360_10.Composite
{
    public partial class HeadedAnchor : AnalyticalElement
    {
      public double GetGroupFactorR_g(HeadedAnchorDeckCondition HeadedAnchorDeckCondition,HeadedAnchorWeldCase HeadedAnchorWeldCase, double N_saRib,double h_r,double w_r)
        {
            double R_g;

            if (HeadedAnchorWeldCase == AISC.HeadedAnchorWeldCase.WeldedDirectly)
            {
                //(1b) any number of steel headed stud anchors welded in a row directly to the steel shape
                R_g = 1.0;
            }
            else
            {
                double w_rTo_h_r = w_r / h_r;
                if (HeadedAnchorDeckCondition == AISC.HeadedAnchorDeckCondition.Parallel)
                {
                    if (w_r / h_r>=1.5)
                    {
                        //(1c)
                        // any number of steel headed stud anchors welded in a row through
                        // steel deck with the deck oriented parallel to the steel shape and the
                        // ratio of the average rib width to rib depth ≥ 1.5
                        R_g = 1.0;
                    }
                    else
                    {
                        if (N_saRib == 1)
                        {
                            //(2b) one steel headed stud anchor welded through steel deck with the deck
                            //oriented parallel to the steel shape and the ratio of the average rib
                            //width to rib depth < 1.5
                            R_g = 0.85;
                        }
                        else
                        {
                            R_g = 0.7; // this value is assumed as the spec does not explcitly cover the case
                        }
                    }
                }
                else if (HeadedAnchorDeckCondition == AISC.HeadedAnchorDeckCondition.Perpendicular)
	            {
                    if (N_saRib ==1)
                    {
                        //(1a) one steel headed stud anchor welded in a steel deck rib with the deck
                        //oriented perpendicular to the steel shape;
                        R_g = 1.0;
                    }
                    else if (N_saRib ==2)
                    {
                        //(2a) two steel headed stud anchors welded in a steel deck rib with the deck
                        //oriented perpendicular to the steel shape;
                        R_g = 0.85;
                    }
                    else
                    {
                        //(3) for three or more steel headed stud anchors welded in a steel deck rib
                        //with the deck oriented perpendicular to the steel shape
                        R_g = 0.7;
                    }
	            }

                else //No deck
                {
                    R_g = 1.0;
                }
            }
            return R_g;
        }
    }
}
