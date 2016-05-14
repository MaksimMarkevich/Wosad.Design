﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wosad.Steel.AISC.AISC360v10.Connections.BasePlate
{
    public class BasePlateIShape : BasePlateTypeBase
    {

        public BasePlateIShape(double B_bp, double N_bp, double d_c, double b_f,
            double P_u, double f_c, double F_y, double A_2)
            :base(B_bp,N_bp, f_c, F_y, A_2)

        {
            this.d_c = d_c;
            this.b_f = b_f;
            this.P_u = P_u;
        }

        double d_c;
        double b_f;
        double P_u;
       

        public override double GetLength()
        {
            double m = Get_m();
            double n = Get_n();
           double  lambda_n_prime = Get_lambda_n_prime();

           List<double> ls = new List<double>
           {
               m,n,lambda_n_prime
           };
           var l_max = ls.Max();

           return l_max;
        }

        private double Get_lambda_n_prime()
        {
            double phiP_p = GetphiP_p();
            double X=(((4.0*d_c*b_f) / (Math.Pow((d_c+b_f), 2))))*((P_u) / (phiP_p));
            double lambda1 = ((2.0 * Math.Sqrt(X)) / (1 + Math.Sqrt(1 - X)));
            double lambda2 = 1.0;
            double lambda = Math.Min(lambda1, lambda2);
            double lambda_n_prime = lambda * ((Math.Sqrt(d_c * b_f)) / (4.0));
            return lambda_n_prime;
        }

        public override double Get_m()
        {
            double m = ((N_bp - 0.95 * d_c) / (2.0));
            return m;
        }

        public override  double Get_n()
        {
            double n =((B_bp - 0.8 * b_f) / (2.0));
            return n;
        }
    }
}
