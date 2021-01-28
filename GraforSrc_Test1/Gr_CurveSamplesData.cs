using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraforWpfDll;

namespace WpfGrafor
{
    public class Gr_CurveSamplesData
    {
        //--------------------------------------------------------------
        //	Внешние переменные для программы FFT
        //--------------------------------------------------------------
        public static readonly int DIMFUN = 5120;   //количество точек функции
        public static readonly int DIMSPEC = 2560;  //количество точек спектр 
        public int _pointsCounter;
        public double[] _funcX = new double[DIMFUN + 1];
        public double[] _funcY = new double[DIMFUN + 1];
        public double[] _sin1X = new double[DIMFUN + 1];
        public double[] _sin1Y = new double[DIMFUN + 1];

        //--------------------------------------------------------------

        public double Deltime, Delfreq;

        //------------
        //int Nfun = DIMFUN;
        //int Nspec = DIMSPEC;
        double[] Wr = new double[DIMFUN + 1];
        double[] Wi = new double[DIMFUN + 1];
        public float[] Vr = new float[DIMFUN + 1];
        public float[] Vi = new float[DIMFUN + 1];
        public double[] Dr = new double[DIMFUN + 1];
        public double[] Di = new double[DIMFUN + 1];

        public double[] Func = new double[DIMFUN + 1];
        public double[] Delt = new double[DIMFUN + 1];
        public double[] Spec = new double[DIMSPEC + 1];
        public double[] Delf = new double[DIMSPEC + 1];

        public Gr_CurveSamplesData()
        {
            GetCurveSinXY();
        }

        public TCurve GetCurvePoisson()
        {
            double[] x = new double[150];
            double[] y = new double[150];
            int i, n;
            n = 150;
            double lam = 10.0 / n;
            for (i = 0; i < n; i++)
            {
                x[i] = i;
                y[i] = 1.0 - Math.Exp(-lam*x[i]);
            }
            return new TCurve(x, y, n);
        }

        public TCurve GetCurveSin()
        {
            double[] x = new double[50];
            double[] y = new double[50];
            int i, n;
            n = 50;
            for (i = 0; i < n; i++)
            {
                x[i] = i / 10.0;
                y[i] = Math.Sin(x[i]) + 0.3 * Math.Sin(x[i] / 4);
            }
            return new TCurve(x, y, n);
        }

        public void GetCurveSinXY()
        {
            _pointsCounter = 70;
            for (int i = 0; i < _pointsCounter; i++)
            {
                _sin1X[i] = i / 10.0;
                _sin1Y[i] = Math.Sin(_sin1X[i]) + 0.3 * Math.Sin(_sin1X[i] / 4);
            }
        }

        public TCurve GetCurveSin2()
        {
            //GraphicCurveSamplesTestCases curves = new GraphicCurveSamplesTestCases();
            int n = 512; // size of Delt[], and Func[]
            double[] x = new double[n];
            double[] y = new double[n];
            int i;
            if (Sin5(n) == 0)
            {
                for (i = 0; i < n; i++)
                {
                    x[i] = Delt[i];
                    y[i] = Func[i];
                }
            }
            return new TCurve(x, y, n);
        }

        public TCurve GetCurvStep1()
        {
            int n = 512; // size of Delt[], and Func[]
            double[] x = new double[n];
            double[] y = new double[n];
            int i;
            if (h_Function(n) == 0)
            {
                for (i = 0; i < n; i++)
                {
                    x[i] = Delt[i];
                    y[i] = Func[i];
                }
            }
            return new TCurve(x, y, n);
        }
        //------------------------------------------------------------
        public int Sin5(int nFunc)
        //------------------------------------------------------------
        { // f0 = 10 Hz, PeriodCount = 5, delfi = 0.5;
            int i, nzero, nxx;
            double twopi, fsns;
            int defz;
            double freq;
            float ampl, delfi;

            Dr = new double[nFunc + 1];
            Di = new double[nFunc + 1];
            Dr[0] = 0.0;
            Di[0] = 0.0;

            Delt = new double[nFunc];
            Func = new double[nFunc];


            //if ( n > DIMFUN) return 1;

            freq = 10; // We want sin(2p*F0*t) with a sentral frequency for spectrum F0 = 10 Hz
            int PeriodCount = 5; // and 10 periods we would like for function sin(t) on chart
            double period = 1.0 / freq; // then period of function T0 = 1/F0 
            Deltime = PeriodCount * period / nFunc;// delta between points of function AllTime / n
            Delfreq = 1.0 / (Deltime * nFunc);     // delta between points of spectrum 1/AllTime
            //freq = (float)10.00;//10 kHz
            //Delfreq = freq;
            ampl = (float)1.0;
            delfi = (float)0.5; // fase shift

            // defz is a part of function padded with 0.0 to reduce DelFreq (to make spectr smooth) 
            //if you need spectr in the future. if defz=3 then only 1/3 of function is a function.

            defz = 3;
            twopi = (2.0 * Math.PI);
            int nNonZero = (int)(nFunc / defz);
            nzero = nFunc - nNonZero;

        lb1: ;//подгонка nzero (+/- 1) так, чтобы nzero+nfun=n
            if ((nNonZero + nzero) == nFunc) goto lb2;
            if ((nNonZero + nzero) > nFunc) { nzero--; goto lb1; };
            if ((nNonZero + nzero) < nFunc) { nzero++; goto lb1; };
        lb2: ;
            // Инициализация массива V (for float) D (for double). it start from 1..Nfun:
            for (i = 1; i <= nNonZero; i++)
            {
                fsns = (twopi * (i - 1) * Deltime * freq + delfi);
                //    fsns1=(twopi*(i-1)*Delt*9.80);
                //    fsns=abs(ampl*sin(fsns))+ampl/15.00*sin(fsns1);
                fsns = (float)(ampl * Math.Sin(fsns));
                Dr[i] = fsns;
                Di[i] = 0.0;
            }
            for (i = 1; i <= nzero; i++)
            {
                nxx = nNonZero + i;
                Dr[nxx] = 0.0;
                Di[nxx] = 0.0;
            }
            for (i = 0; i < nFunc; i++)
            {
                //Func[i]=sqrt(Vr[i+1]*Vr[i+1]+Vi[i+1]*Vi[i+1]);
                //Func[i] = Vr[i + 1];
                Func[i] = Dr[i + 1]; // Di[] are 0.0
                Delt[i] = i * Deltime;
            }

            return 0;
        }


        //------------------------------------------------------------
        public int h_Function(int nFunc)
        //------------------------------------------------------------

         /* Для инициализации функции, заданной в виде двух (одной) ступенек.
         Входные данные:
         nFunc - количество точек области задания функции
         jnstup- количество ступенек (1 или 2)
          j1 - левая точка первой ступеньки
         j2 - правая точка первой ступеньки
         j3 - левая точка второй ступеньки
         j4 - правая точка второй ступеньки
         Delt - шаг по t (time/space)
         результаты:
            Dr,Di - массив значений функции в точках области (Re,Im)
          -----------------------------------------------------------------
          */
        {
            int i;
            int j1, j2, j3, j4, jnstup;
            Deltime = 0.00010;//mks
            Delfreq = (float)1.0 / (Deltime * nFunc);
            Dr = new double[nFunc + 1];
            Di = new double[nFunc + 1];
            Dr[0] = 0.0;
            Di[0] = 0.0;

            Delt = new double[nFunc];
            Func = new double[nFunc];

            int defz = 3;
            int nNonZero = (int)(nFunc / defz);
            int nzero = nFunc - nNonZero;

        lb1: ;//подгонка nzero (+/- 1) так, чтобы nzero+nfun=n
            if ((nNonZero + nzero) == nFunc) goto lb2;
            if ((nNonZero + nzero) > nFunc) { nzero--; goto lb1; };
            if ((nNonZero + nzero) < nFunc) { nzero++; goto lb1; };
        lb2: ;


            jnstup = 2;
            j1 = (int)(nNonZero / 100);
            j2 = (int)(nNonZero / 60);
            j3 = (int)(nNonZero / 10);
            j4 = (int)(nNonZero / 2);
            // Инициализация массива D:
            for (i = 1; i <= nNonZero; i++) { Dr[i] = 0.0; Di[i] = 0.0; }
            if (jnstup == 1 || jnstup == 2)
                for (i = j1; i <= j2; i++) { Dr[i] = 1.0; Di[i] = 0.0; }
            if (jnstup == 2)
                for (i = j3; i <= j4; i++) { Dr[i] = 1.0; Di[i] = 0.0; }

            int nxx;
            for (i = 1; i <= nzero; i++)
            {
                nxx = nNonZero + i;
                Dr[nxx] = 0.0;
                Di[nxx] = 0.0;
            }

            for (i = 0; i < nFunc; i++)
            {
                //Func[i]=sqrt(Vr[i+1]*Vr[i+1]+Vi[i+1]*Vi[i+1]);
                //Func[i] = Vr[i + 1];
                Func[i] = Dr[i + 1]; // Di[] are 0.0
                Delt[i] = i * Deltime;
            }
            return 0;
        }


    }
}
