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
using Wosad.Common.Entities;
using Wosad.Concrete.ACI.Infrastructure.Entities.Section.Strains;
using Wosad.Concrete.ACI.Infrastructure.Entities.Rebar;
using Wosad.Common.Mathematics;
using Wosad.Common.Interfaces;

namespace Wosad.Concrete.ACI
{
    public abstract partial class ConcreteSectionBase : AnalyticalElement, IStructuralMember, IConcreteMember
    {

        protected virtual  List<RebarPointResult> CalculateRebarResults(LinearStrainDistribution StrainDistribution, FlexuralAnalysisType AnalysisType)
        {
            List<RebarPointResult> ResultList = new List<RebarPointResult>();

            double c = StrainDistribution.NeutralAxisTopDistance;
            double h = StrainDistribution.Height;
            double YMax = Section.SliceableShape.YMax;
            double YMin = Section.SliceableShape.YMin;
            double XMax = Section.SliceableShape.XMax;
            double XMin = Section.SliceableShape.XMin;

            double sectionHeight = Section.SliceableShape.YMax - Section.SliceableShape.YMin;
            double distTopToSecNeutralAxis = sectionHeight - Section.SliceableShape.CentroidYtoBottomEdge;

                foreach (RebarPoint rbrPnt in LongitudinalBars)
                {
                    double BarDistanceToTop = YMax - rbrPnt.Coordinate.Y;
                    double BarDistanceToNa = BarDistanceToTop - distTopToSecNeutralAxis;
                    double Strain = StrainDistribution.GetStrainAtPointOffsetFromTop(BarDistanceToTop);
                    double Force;
                    double Stress;

                    if (AnalysisType== FlexuralAnalysisType.StrainCompatibility)
                        {
                            Force = rbrPnt.Rebar.GetForce(Strain);
                            Stress = rbrPnt.Rebar.GetStress(Strain);
                            ResultList.Add(new RebarPointResult(Stress, Strain, Force, BarDistanceToNa, rbrPnt));
                        }
                    else
                    {
                        //Ignore compression rebar
                        if (BarDistanceToNa<=0)
                        {
                            
                        }
                        //TODO: design force for tension 
                    }
                } 


            return ResultList;
        }


       protected Point2D FindRebarGeometricCentroid(List<RebarPoint> bars)
       {
           double totalArea=0;
           double SumAreaMomentsX=0;
           double SumAreaMomentsY=0;

           if (bars.Count == 0)
           {
               throw new NoRebarException();
           }

           foreach (var bar in bars)
           {
               double Area = bar.Rebar.Area;
               double MomentX = Area * bar.Coordinate.Y;
               double MomentY = Area * bar.Coordinate.X;
               //RebarAreaMoment fm = new RebarAreaMoment() { Area = Area, MomentX = MomentX, MomentY = MomentY };
               totalArea += Area;
               SumAreaMomentsX += MomentX;
               SumAreaMomentsY += MomentY;
           }
           double LocationX = SumAreaMomentsY / totalArea;
           double LocationY = SumAreaMomentsX / totalArea;
           return new Point2D(LocationX, LocationY);
       }

    protected RebarPoint FindRebarWithExtremeCoordinate(BarCoordinateFilter CoordinateFilter, BarCoordinateLimitFilterType LimitFilter, double CutoffCoordinate)
    {
        RebarPoint returnBar = null;

        RebarPoint maxXRebar = null;
        RebarPoint maxYRebar = null;
        RebarPoint minXRebar = null;
        RebarPoint minYRebar = null;

        double MaxY = double.NegativeInfinity;
        double MaxX = double.NegativeInfinity;
        double MinY = double.PositiveInfinity;
        double MinX = double.PositiveInfinity;

        if (longitBars.Count == 0)
        {
            throw new NoRebarException();
        }

        foreach (var bar in longitBars)
        {
            if (bar.Coordinate.X > MaxX)
            {
                if (bar.Coordinate.X<CutoffCoordinate)
                {
                    maxXRebar = bar;
                    MaxX = bar.Coordinate.X; 
                }
            }

            if (bar.Coordinate.Y > MaxY)
            {
                if (bar.Coordinate.Y < CutoffCoordinate)
                {
                    maxYRebar = bar;
                    MaxY = bar.Coordinate.Y;
                }
            }
            if (bar.Coordinate.X < MinX)
            {
                if (bar.Coordinate.X > CutoffCoordinate)
                {
                    minXRebar = bar;
                    MinX = bar.Coordinate.X;
                }
            }

            if (bar.Coordinate.Y < MinY)
            {
                if (bar.Coordinate.Y > CutoffCoordinate)
                {
                    minYRebar = bar;
                    MinY = bar.Coordinate.Y;
                }
            }

        }

        if (CoordinateFilter == BarCoordinateFilter.X && LimitFilter == BarCoordinateLimitFilterType.Maximum)
        {
            returnBar = maxXRebar;
        }

        if (CoordinateFilter == BarCoordinateFilter.X && LimitFilter == BarCoordinateLimitFilterType.Minimum)
        {
            returnBar = minXRebar;
        }

        if (CoordinateFilter == BarCoordinateFilter.Y && LimitFilter == BarCoordinateLimitFilterType.Maximum)
        {
            returnBar = maxYRebar;
        }

        if (CoordinateFilter == BarCoordinateFilter.Y && LimitFilter == BarCoordinateLimitFilterType.Minimum)
        {
            returnBar = minYRebar;
        }

        return returnBar;

    }

    protected ForceMomentContribution GetRebarResultant(LinearStrainDistribution StrainDistribution, ResultantType resType, FlexuralAnalysisType AnalysisType)
    {
        ForceMomentContribution resultant = new ForceMomentContribution();
        //tension is negative
        List<RebarPointResult> RebarResults = CalculateRebarResults(StrainDistribution, AnalysisType);
        foreach (var barResult in RebarResults)
        {
            if (resType == ResultantType.Tension)
            {
                if (barResult.Strain < 0)
                {
                    resultant.Force += barResult.Force;
                    resultant.Moment += Math.Abs(barResult.Force * barResult.DistanceToNeutralAxis);
                }
            }
            else
            {
                if (barResult.Strain > 0)
                {
                    resultant.Force += barResult.Force;
                    resultant.Moment += Math.Abs(barResult.Force * barResult.DistanceToNeutralAxis);
                }
            }
        }
        return resultant;
    }
    protected ForceMomentContribution GetRebarResultant(BarCoordinateFilter CoordinateFilter, BarCoordinateLimitFilterType LimitFilter, double CutoffCoordinate)
    {
        ForceMomentContribution resultant = new ForceMomentContribution();

            foreach (var bar in longitBars)
            {
                double barLimitForce = 0;
                double barLimitForceMoment = 0;

                if (CoordinateFilter == BarCoordinateFilter.X && LimitFilter == BarCoordinateLimitFilterType.Maximum)
                {
                    if (bar.Coordinate.X <= CutoffCoordinate)
                    {
                        barLimitForce = bar.Rebar.GetDesignForce();
                        barLimitForceMoment = barLimitForce * bar.Coordinate.X;
                    }
                }

                if (CoordinateFilter == BarCoordinateFilter.Y && LimitFilter == BarCoordinateLimitFilterType.Maximum)
                {
                    if (bar.Coordinate.Y <= CutoffCoordinate)
                    {
                        barLimitForce = bar.Rebar.GetDesignForce();
                        barLimitForceMoment = barLimitForce * bar.Coordinate.Y;
                    }
                }

                if (CoordinateFilter == BarCoordinateFilter.X && LimitFilter == BarCoordinateLimitFilterType.Minimum)
                {
                    if (bar.Coordinate.X >= CutoffCoordinate)
                    {
                        barLimitForce = bar.Rebar.GetDesignForce();
                        barLimitForceMoment = barLimitForce * bar.Coordinate.X;
                    }
                }

                if (CoordinateFilter == BarCoordinateFilter.Y && LimitFilter == BarCoordinateLimitFilterType.Minimum)
                {
                    if (bar.Coordinate.Y >= CutoffCoordinate)
                    {
                        barLimitForce = bar.Rebar.GetDesignForce();
                        barLimitForceMoment = barLimitForce * bar.Coordinate.Y;
                    }
                }

                ForceMomentContribution barResultant = new ForceMomentContribution(){ Force = barLimitForce, Moment=barLimitForceMoment};
                resultant += barResultant;
            }

            return resultant;

    }

    protected ForceMomentContribution GetApproximateRebarResultant(BarCoordinateFilter CoordinateFilter, BarCoordinateLimitFilterType LimitFilterType,
double CutoffCoordinate)
    {
        ForceMomentContribution resultant = new ForceMomentContribution();

        foreach (var bar in longitBars)
        {
            double barLimitForce = 0;
            double barLimitForceMoment = 0;

            if (CoordinateFilter == BarCoordinateFilter.X && LimitFilterType == BarCoordinateLimitFilterType.Maximum)
            {
                if (bar.Coordinate.X <= CutoffCoordinate)
                {
                    barLimitForce = bar.Rebar.GetDesignForce();
                    barLimitForceMoment = barLimitForce * bar.Coordinate.X;
                }
            }

            if (CoordinateFilter == BarCoordinateFilter.Y && LimitFilterType == BarCoordinateLimitFilterType.Maximum)
            {
                if (bar.Coordinate.Y <= CutoffCoordinate)
                {
                    barLimitForce = bar.Rebar.GetDesignForce();
                    barLimitForceMoment = barLimitForce * bar.Coordinate.Y;
                }
            }

            if (CoordinateFilter == BarCoordinateFilter.X && LimitFilterType == BarCoordinateLimitFilterType.Minimum)
            {
                if (bar.Coordinate.X >= CutoffCoordinate)
                {
                    barLimitForce = bar.Rebar.GetDesignForce();
                    barLimitForceMoment = barLimitForce * bar.Coordinate.X;
                }
            }

            if (CoordinateFilter == BarCoordinateFilter.Y && LimitFilterType == BarCoordinateLimitFilterType.Minimum)
            {
                if (bar.Coordinate.Y >= CutoffCoordinate)
                {
                    barLimitForce = bar.Rebar.GetDesignForce();
                    barLimitForceMoment = barLimitForce * bar.Coordinate.Y;
                }
            }

            ForceMomentContribution barResultant = new ForceMomentContribution() { Force = barLimitForce, Moment = barLimitForceMoment };
            resultant += barResultant;
        }

        return resultant;

    }



        protected enum BarCoordinateFilter
        {
            X,
            Y
        }

        protected enum BarCoordinateLimitFilterType
        {
            Maximum,
            Minimum
        }

       private class RebarAreaMoment
       {
           public double Area { get; set; }
           public double MomentX { get; set; }
           public double MomentY { get; set; }
       }
    }
}
