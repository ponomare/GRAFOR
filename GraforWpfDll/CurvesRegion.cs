using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraforWpfDll
{
    // Area for curves on GraforRegion. That region is a liitle bit less (see bt[] coefficients) than just 
    // a space between scroll bar and axisY (same for axisX).

    // CurvesRegion is an area on region, which is used to display curves. CurvesRegion class includes structures and methods 
    // to calculate range of screen coordinates and pfysical limits for that area. 
    // GraforRegion uses class CurvesRegion and List<curves> to display curves or part of them in region properly.
    // Coefficients to convert mathimatical coordinates to physical
    // todo : Don't use it to convert back  Why?????????
    // u = x * a11 + a12;
    // v = y * a21 + a22;
    public class MathToScreenMatrix
    {
        public double a11, a12, a21, a22;
    }
    public class CurvesRegion
    {

       

        double[] _bt = new double[] { 0.05, 0.05, 0.05, 0.05 };

        // Limits for area for curves in pixels. 
        // Tol'ko tochki krivyh, popadaushie v etu oblast' budut risobvat'sya.
        // alpha rasschityvautsya po mathLimits and screenLimits.
        // T.e krivye dolzhny but' vnutri screenLimits oblasti of region.
        public ScreenLimits screenLimits = new ScreenLimits();

        // Current physical limits for curves for area of region.
        // If we try to zoom chart then mathLimits is equel to limits defined by zoom ribbon.
        // If we try to show all points for all region's curves, then mathLimits equel to  mathLimitsForAllCurves.
        public MathLimits mathLimits = new MathLimits();

        // These Limits calculated by ALL curves data and we don't change it in case of zoom.
        // Ratio for zomm is calculated based on this mathLimitsForAllCurves.
        // We have to recalculate this structure if curves (pointes, number) are changed in the region.
        public MathLimits mathLimitsForAllCurves = new MathLimits();
        public MathToScreenMatrix _alpha = new MathToScreenMatrix();


        public void SetBt(double[] bt)
        {
            _bt = bt.ToArray();
        }

        // If we need to set curvesRegion limits manually
        public int SetMathLimits(double xmin, double ymin, double xmax, double ymax)
        {
            mathLimits.xmin = xmin;
            mathLimits.ymin = ymin;
            mathLimits.xmax = xmax;
            mathLimits.ymax = ymax;
            return 0;
        }

        public int SetMathLimits(MathLimits mLimits)
        {
            mathLimits = new MathLimits(mLimits);
            return 0;
        }

        public int SetMathLimitsForAllCurves(double xmin, double ymin, double xmax, double ymax)
        {
            mathLimitsForAllCurves.xmin = xmin;
            mathLimitsForAllCurves.ymin = ymin;
            mathLimitsForAllCurves.xmax = xmax;
            mathLimitsForAllCurves.ymax = ymax;
            return 0;
        }

        // Calculate the range of X,Y in all curves of region 
        // Call the method if curves (points, number) are changed in the region.
        public void SetMathLimitsForAllCurves(List<TCurve> curves)
        {
            int i;
            double xmin, xmax, ymin, ymax;
            List<double> px, py;
            double xi, yi; //значение X и Y по адресу px[i], py[i]

            TCurve crv0;
            if (curves.Count == 0)
            {
                xmin = 0.0;
                xmax = 0.0;
                ymin = 0.0;
                ymax = 0.0;
            }
            else
            {
                crv0 = curves.First();
                xmin = crv0.px[0]; xmax = xmin;
                ymin = crv0.py[0]; ymax = ymin;

                foreach (TCurve crv in curves)
                {
                    px = crv.px;
                    py = crv.py;
                    for (i = 0; i < crv.nall; i++)
                    {
                        xi = px[i];
                        yi = py[i];
                        if (xi < xmin) xmin = xi;
                        if (xi > xmax) xmax = xi;
                        if (yi < ymin) ymin = yi;
                        if (yi > ymax) ymax = yi;
                    }
                }
            }
            SetMathLimitsForAllCurves(xmin, ymin, xmax, ymax);
        }

        // Calculates area for curves on region. 
        // It takes region area in pixels (Region.ScreenLimits) and reduce it on: axis width, scrollBar width, bt[]  etc..
        public int CalculateScreenLimits(ScreenLimits region, double scrollBarYWidth, double scrollBarXHeight,
                                         double AxisYWidth, double AxisXHeight)
        {
            screenLimits.u1 = region.u1 + AxisYWidth + _bt[0] * 100;
            screenLimits.v1 = region.v1 + _bt[1] * 100;
            screenLimits.u2 = region.u2 - scrollBarYWidth - _bt[2] * 100;
            screenLimits.v2 = region.v2 - scrollBarXHeight - AxisXHeight - _bt[3] * 100;

            return 0;
        }
        //---------------------------------------------------------------
        // Функция расчета переводных коэффициентов Alp11 ... Alp22
        // для перевода математических координат X,Y в физические
        // координаты экрана U,V для области функции _chartRegion.
        // Output:
        //   RG.alp11
        //   RG.alp12
        //   RG.alp21
        //   RG.alp22
        // To get it, chart should have defined values for limits and size.
        //---------------------------------------------------------------
        public MathToScreenMatrix CalculateAlphaCoefficients(bool axisX_ascScale)
        {
            ///double xmin,xmax,ymin,ymax;
            double rgszu, rgszv;
            double dxfun, dyfun;
            double u1, u2, v1, v2;
            double x1, x2, y1, y2;

            // limits dlya oblasti, gde risuutsya krivue
            // todo: mozhno li ubrat' eti proverki na 0, na osi, etc. kuda-nibud' v method???
            var limits = mathLimits;
            // if xmin==xmax or ymin==ymax then make some distance between them
            if (Math.Abs(limits.xmax - limits.xmin) < 1.0e-30)
            {// "раздвинуть" диапазон по X, чтобы dxfun !=0;
                double tmpx = limits.xmax;
                if (tmpx > 0)
                {
                    SetMathLimits(0.90 * tmpx, limits.ymin, 1.10 * tmpx, limits.ymax);
                }
                if (tmpx < 0) // if xmax is negative then , to get xmin = 1.1*tmp<xmax = 0.9*tmp:
                {
                    SetMathLimits(1.10 * tmpx, limits.ymin, 0.90 * tmpx, limits.ymax);
                }
                if (Math.Abs(tmpx) < 1.0e-30)
                {
                    SetMathLimits(0.90, limits.ymin, 1.10, limits.ymax);// todo should it be  0.90, , 1.10 . Why xmax first??
                }
            }

            if (Math.Abs(limits.ymax - limits.ymin) < 1.0e-30)
            {//  ­ "раздвинуть" диапазон по Y, чтобы dyfun != 0;
                double tmpy = limits.ymax;
                if (tmpy > 0)
                {
                    SetMathLimits(limits.xmin, 0.90 * tmpy, limits.xmax, 1.10 * tmpy);
                }
                if (tmpy < 0) // if ymax is negative then , to get ymin = 1.1*tmp<ymax = 0.9*tmp:
                {
                    SetMathLimits(limits.xmin, 1.10 * tmpy, limits.xmax, 0.90 * tmpy);
                }
                if (Math.Abs(tmpy) < 1.0e-30)
                {
                    SetMathLimits(limits.xmin, 0.90, limits.xmax, 1.10);
                }
            }

            if (axisX_ascScale) //возрастающая шкала X
            { x1 = limits.xmin; x2 = limits.xmax; }
            else // убывающая шкала X
            { x1 = limits.xmax; x2 = limits.xmin; }

            y1 = limits.ymin; y2 = limits.ymax;

            // size (screen area) dlya oblasti, gde risuutsya krivue
            //eu
            //var curvesArea = _curvesRegion._curvesDrawLimits;
            var curvesArea = screenLimits;
            u1 = curvesArea.u1; u2 = curvesArea.u2;
            v1 = curvesArea.v1; v2 = curvesArea.v2;

            rgszu = u2 - u1; rgszv = v2 - v1;
            dxfun = x2 - x1; dyfun = y2 - y1;

            _alpha.a11 = (rgszu) / dxfun;
            _alpha.a12 = (x2 * u1 - x1 * u2) / dxfun;
            _alpha.a21 = (-rgszv) / dyfun;
            _alpha.a22 = (-y1 * v1 + y2 * v2) / dyfun;

            return (_alpha);
        }

    }
}
