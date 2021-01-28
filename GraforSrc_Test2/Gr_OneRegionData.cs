using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraforSrc_Test2
{
    public class Gr_OneRegionData
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

        public Gr_OneRegionData()
        {
            GetCurveSinXY();
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
    }
}
