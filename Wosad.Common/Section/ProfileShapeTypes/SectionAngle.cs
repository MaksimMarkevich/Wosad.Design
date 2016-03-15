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
using Wosad.Common.Mathematics;
using Wosad.Common.Section.Interfaces;

namespace Wosad.Common.Section.SectionTypes
{
    /// <summary>
    /// Generic angle shape with geometric parameters provided in a constructor.
    /// Shape properties do not account for rounded corners, typical for rolled shapes
    /// </summary>
    public partial class SectionAngle : CompoundShape, ISectionAngle, ISliceableSection
    {
        public SectionAngle(string Name, double h, double b, double t)
            : base(Name)
        {
            this._d = h;
            this._b = b;
            this._t = t;
        }

        private double _b;

        public double b
        {
            get { return _b; }
        }

        private double _d;

        public double d
        {
            get { return _d; }
            set { _d = value; }
        }

        private double _t;

        public double t
        {
            get { return _t; }
        }
        


        public ISection GetWeakAxisClone()
        {
            string cloneName= this.Name+"_clone";
            return new SectionAngle(cloneName, b, _d, t);
        }



        #region Section properties specific to Angle

        bool PrincipalAxisPropertiesCalculated;
        
        private double  _I_w;

        public double  I_w
        {
            get {
                if (PrincipalAxisPropertiesCalculated == false)
                {
                    CalculatePrincipalAxisProperties();
                }
                return _I_w; }

        }


        private double _I_z;

        public double I_z
        {
            get {
                if (PrincipalAxisPropertiesCalculated == false)
                {
                    CalculatePrincipalAxisProperties();
                }
                return _I_z; }

        }


        private double _S_w;

        public double S_w
        {
            get {
                if (PrincipalAxisPropertiesCalculated == false)
                {
                    CalculatePrincipalAxisProperties();
                }
                return _S_w; }

        }


        private double _S_z;

        public double S_z
        {
            get {
                if (PrincipalAxisPropertiesCalculated==false)
                {
                    CalculatePrincipalAxisProperties();
                }
                return _S_z; }


        }


        private double _r_z;

        public double r_z
        {
            get {
                if (PrincipalAxisPropertiesCalculated == false)
                {
                    CalculatePrincipalAxisProperties();
                }
                return _r_z; }
        }

        private double _r_w;

        public double r_w
        {
            get {

                if (PrincipalAxisPropertiesCalculated == false)
                {
                    CalculatePrincipalAxisProperties();
                }
                return _r_w; }

        }
        


        private AngleOrientation angleOrientation;

        public AngleOrientation AngleOrientation
        {
            get { return angleOrientation; }
            set { angleOrientation = value; }
        }
        


        #endregion


        /// <summary>
        /// Defines a set of rectangles for analysis with respect to 
        /// x-axis, each occupying full width of section.
        /// </summary>
        /// <returns>List of analysis rectangles</returns>
        public override List<CompoundShapePart> GetCompoundRectangleXAxisList()
        {
            List<CompoundShapePart> rectX = new List<CompoundShapePart>()
            {
                new CompoundShapePart(t,d-t, new Point2D(t/2.0,(d-t)/2)),
                new CompoundShapePart(b,t, new Point2D(b/2,t/2)),
            };
            return rectX;
        }

        /// <summary>
        /// Defines a set of rectangles for analysis with respect to 
        /// y-axis, each occupying full height of section. The rectangles are rotated 90 deg., 
        /// because internally the properties are calculated  with respect to x-axis.
        /// </summary>
        /// <returns>List of analysis rectangles</returns>
        public override List<CompoundShapePart> GetCompoundRectangleYAxisList()
        {
            //angle is rotated 90 deg and converted to TEE
            //Insertion point at the top of TEE
            List<CompoundShapePart> rectY = new List<CompoundShapePart>()
            {
                new CompoundShapePart(d,t, new Point2D(0, -t/2.0)),
                new CompoundShapePart(t, b-t, new Point2D(0,-((b-t)/2+t))),
            };
            return rectY;
        }

        /// <summary>
        /// From:
        /// TORSIONAL SECTION PROPERTIES OF STEEL SHAPES
        ///Canadian Institute of Steel Construction, 2002
        /// </summary>
        protected override void CalculateWarpingConstant()
        {
            throw new NotImplementedException();
            double b_prime = this.b-t/2;
            double d_prime = this._d-t/2;
            this._C_w=((Math.Pow(t, 3)) / (36))*(Math.Pow((_d), 3)+Math.Pow((b), 3));
        }
        /// <summary>
        /// From:
        /// TORSIONAL SECTION PROPERTIES OF STEEL SHAPES
        ///Canadian Institute of Steel Construction, 2002
        /// </summary>
        protected override void CalculateTorsionalConstant()
        {
            double b_prime = this.b-t/2;
            double d_prime = this._d-t/2;
            this._J=(((d_prime+b_prime)*Math.Pow(t, 3)) / (3));
        }

        public double Angle_alpha { get; set; }


        public double beta_w { get; set; }

    }
}
