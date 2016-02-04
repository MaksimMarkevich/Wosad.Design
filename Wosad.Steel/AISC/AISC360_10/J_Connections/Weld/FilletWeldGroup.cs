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
using Wosad.Common.Mathematics;
using Wosad.Steel.AISC.SteelEntities.Welds;

namespace Wosad.Steel.AISC.AISC360_10.Connections
{
    public class FilletWeldGroup : FilletWeldLoadDeformationGroupBase
    {
        /// <summary>
        /// Weld group constructor, based on weld pattern
        /// </summary>
        /// <param name="WeldPattern">ParallelVertical,ParallelHorizontal,Rectangle,C or L pattern </param>
        /// <param name="l_horizontal"> Vertical dimension of group (for C length of "web")</param>
        /// <param name="l_vertical">Horizontal dimension of group</param>
        /// <param name="leg">Follet weld size (leg)</param>
        /// <param name="F_EXX">Electrode strength</param>
        public FilletWeldGroup(string WeldPattern, double l_horizontal, double l_vertical, double leg, double F_EXX, bool IsLoadedOutOfPlane=false)
        {
            this.l_horizontal= l_horizontal;
            this.l_vertical = l_vertical;
            this.leg = leg;
            this.F_EXX = F_EXX;
            Nsub = 20;
            this.IsLoadedOutOfPlane = IsLoadedOutOfPlane;

             switch (WeldPattern)
	        {
                case "ParallelVertical": AddParallelVertical(); pattern = WeldGroupPattern.ParallelVertical; break;
                case "ParallelHorizontal": AddWeldParallelHorizontal(); pattern = WeldGroupPattern.ParallelHorizontal; break;
                case "Rectangle": AddRectangle(); pattern = WeldGroupPattern.Rectangle; break;
                case "C": AddC(); pattern = WeldGroupPattern.C; break;
                case "L": AddL(); pattern = WeldGroupPattern.L; break;
                default: AddParallelVertical(); pattern = WeldGroupPattern.ParallelVertical; break;                                            
	        }


        }

        public FilletWeldGroup(WeldGroupPattern WeldPattern, double l_horizontal, double l_vertical, double leg, double F_EXX, bool IsLoadedOutOfPlane=false):
            this(WeldPattern.ToString(), l_horizontal, l_vertical, leg, F_EXX, IsLoadedOutOfPlane)
        {
        }

        int Nsub;
        WeldGroupPattern pattern;
        double l_horizontal; 
        double l_vertical; 
        double leg;
        double F_EXX;
        bool IsLoadedOutOfPlane;

        private void AddL()
        {
            CharacteristicDimension = l_vertical;

            Point2D p1_temp = new Point2D(-l_horizontal/2.0,-l_vertical/2.0);
            Point2D p2_temp = new Point2D(-l_horizontal/2.0,l_vertical/2.0);
            Point2D p3_temp = new Point2D(l_horizontal/2.0,l_vertical/2.0);


            List<FilletWeldLine> templines = new List<FilletWeldLine>()
            {
                new FilletWeldLine(p1_temp,p2_temp,leg,F_EXX,Nsub,0,IsLoadedOutOfPlane),
                new FilletWeldLine(p2_temp,p3_temp,leg,F_EXX,Nsub,0,IsLoadedOutOfPlane)
            };
            FilletWeldLoadDeformationGroupBase tempGroup = new FilletWeldLoadDeformationGroupBase(templines);
            var centroid = tempGroup.GetElasticCentroid();

            Point2D p1 = new Point2D(p1_temp.X - centroid.X, p1_temp.Y - centroid.Y);
            Point2D p2 = new Point2D(p2_temp.X - centroid.X, p2_temp.Y - centroid.Y);
            Point2D p3 = new Point2D(p3_temp.X - centroid.X, p3_temp.Y - centroid.Y);


            List<FilletWeldLine> lines = new List<FilletWeldLine>()
            {
                new FilletWeldLine(p1,p2,leg,F_EXX,Nsub,0,IsLoadedOutOfPlane),
                new FilletWeldLine(p2,p3,leg,F_EXX,Nsub,90.0,IsLoadedOutOfPlane) //horizontal leg
            };
            Lines = lines;

        }

        private void AddC()
        {
            CharacteristicDimension = l_vertical;

            Point2D p1_temp = new Point2D(0, -l_vertical / 2.0);
            Point2D p2_temp = new Point2D(0, l_vertical / 2.0);
            Point2D p3_temp = new Point2D(l_horizontal, l_vertical / 2.0);
            Point2D p4_temp = new Point2D(l_horizontal, -l_vertical / 2.0);


            List<FilletWeldLine> templines = new List<FilletWeldLine>()
            {
                new FilletWeldLine(p1_temp,p2_temp,leg,F_EXX,Nsub,0,IsLoadedOutOfPlane),
                new FilletWeldLine(p2_temp,p3_temp,leg,F_EXX,Nsub,0,IsLoadedOutOfPlane),
                new FilletWeldLine(p1_temp,p4_temp,leg,F_EXX,Nsub,0,IsLoadedOutOfPlane),
            };
            FilletWeldLoadDeformationGroupBase tempGroup = new FilletWeldLoadDeformationGroupBase(templines);
            var centroid = tempGroup.GetElasticCentroid();

            Point2D p1 = new Point2D(p1_temp.X-centroid.X, p1_temp.Y-centroid.Y);
            Point2D p2 = new Point2D(p2_temp.X-centroid.X, p2_temp.Y-centroid.Y);
            Point2D p3 = new Point2D(p3_temp.X-centroid.X, p3_temp.Y-centroid.Y);
            Point2D p4 = new Point2D(p4_temp.X-centroid.X, p4_temp.Y-centroid.Y);


            List<FilletWeldLine> lines = new List<FilletWeldLine>()
            {
                new FilletWeldLine(p1,p2,leg,F_EXX,Nsub,0,IsLoadedOutOfPlane),
                new FilletWeldLine(p2,p3,leg,F_EXX,Nsub,90.0,IsLoadedOutOfPlane),
                new FilletWeldLine(p1,p4,leg,F_EXX,Nsub,90.0,IsLoadedOutOfPlane),
            };
            Lines = lines;
        }
        private void AddRectangle()
        {
            CharacteristicDimension = l_vertical;

            Point2D p1 = new Point2D(-l_horizontal/2.0,-l_vertical/2.0);
            Point2D p2 = new Point2D(-l_horizontal/2.0,l_vertical/2.0);
            Point2D p3 = new Point2D(l_horizontal/2.0,l_vertical/2.0);
            Point2D p4 = new Point2D(l_horizontal/2.0,-l_vertical/2.0);

            List<FilletWeldLine> lines = new List<FilletWeldLine>()
            {
                new FilletWeldLine(p1,p2,leg,F_EXX,Nsub,0,IsLoadedOutOfPlane),
                new FilletWeldLine(p2,p3,leg,F_EXX,Nsub,90.0, IsLoadedOutOfPlane), //horizontal leg
                new FilletWeldLine(p3,p4,leg,F_EXX,Nsub,0,IsLoadedOutOfPlane),
                new FilletWeldLine(p4,p1,leg,F_EXX,Nsub,90.0,IsLoadedOutOfPlane), //horizontal leg
            };
            Lines = lines;
        }

        private void AddWeldParallelHorizontal()
        {
            CharacteristicDimension = l_horizontal;

            Point2D p1 = new Point2D(-l_horizontal/2.0,-l_vertical/2.0);
            Point2D p2 = new Point2D(-l_horizontal/2.0,l_vertical/2.0);
            Point2D p3 = new Point2D(l_horizontal/2.0,l_vertical/2.0);
            Point2D p4 = new Point2D(l_horizontal/2.0,-l_vertical/2.0);

            List<FilletWeldLine> lines = new List<FilletWeldLine>()
            {
                new FilletWeldLine(p2,p3,leg,F_EXX,Nsub,90,IsLoadedOutOfPlane),
                new FilletWeldLine(p1,p4,leg,F_EXX,Nsub,90,IsLoadedOutOfPlane),
            };
            Lines = lines;
        }

        private void AddParallelVertical()
        {
            CharacteristicDimension = l_vertical;
            Point2D p1 = new Point2D(-l_horizontal / 2.0, -l_vertical / 2.0);
            Point2D p2 = new Point2D(-l_horizontal / 2.0, l_vertical / 2.0);
            Point2D p3 = new Point2D(l_horizontal / 2.0, l_vertical / 2.0);
            Point2D p4 = new Point2D(l_horizontal / 2.0, -l_vertical / 2.0);

            List<FilletWeldLine> lines = new List<FilletWeldLine>()
            {
                new FilletWeldLine(p1,p2,leg,F_EXX,Nsub,0,IsLoadedOutOfPlane),
                new FilletWeldLine(p3,p4,leg,F_EXX,Nsub,0,IsLoadedOutOfPlane),
            };
            Lines = lines;
        }

        public double GetInstantaneousCenterCoefficient(double e_x, double AngleOfLoad)
        {

           double P_n= this.FindUltimateEccentricForce(e_x , AngleOfLoad);
            //Bring the coefficient that is in the format of the AISC manual

            //adjust to 1/6 in leg size.
            double ReductionFactor = (1.0/16.0)/this.leg;

            //Divide by length
            double C = P_n*ReductionFactor/CharacteristicDimension; //fix this!!!
            return C;
        }

        /// <summary>
        /// Calculates weld group strength for concentric load per AISC J2.4(c)
        /// page 16.1–116  of specification.
        /// </summary>
        /// <param name="theta">Angle from vertical (degrees)</param>
        /// <returns></returns>
        public double GetConcentricLoadStrenth(double theta=0.0)
        {

            List<FilletWeldLine> calculationLines =Lines.ConvertAll(x => new FilletWeldLine(
             x.NodeI, x.NodeJ,x.Leg,x.ElectrodeStrength, x.NumberOfSubdivisions, x.theta+theta));

            double phiRn1 = 0.0;
            //Case i
            foreach (var l in calculationLines)
	        {
                phiRn1 = phiRn1+l.GetStength();
	        }
            double phiRn2 = double.PositiveInfinity;
            if (Math.Abs(theta)<5.0) //set threshold of 5 degrees 
            {
                //Case ii
                phiRn2 = 0.0;
                foreach (var l in calculationLines)
                {
                    if (l.theta==90)
                    {
                        phiRn2 = phiRn2 + l.GetStength()*1.5;  
                    }
                    else
                    {
                        phiRn2 = phiRn2 + l.GetStength()*0.85;
                    }
                    
                }
            }
            double phiR_n = Math.Max(phiRn1, phiRn2);

            return phiR_n;
        }

        public double CharacteristicDimension { get; set; }
    }
}
