using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Windows.Controls;// UserControl
using System.ComponentModel; //PropertyChangedEventArgs
using System.Windows.Documents;
using System.Windows.Input; //for ExecutedRoutedEventArgs
using GraforWpfDll.UserControls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows; // Size


namespace GraforWpfDll
{
    public partial class GraforRegion
    {

        // is not used
        public int set_new_region()
        {
            TRegionStyle rg = new TRegionStyle();
            double[] py, px;
            int n_sp, i;
            //            set_default_parameters_for_region(J_act_rg);
            // rg.txt = string.Format("Spectrum {0} ", J_act_rg); ; // create new region title
            rg.win_onscreen = 1;//обязательно, исп-ся в Resize
            //rg.spec_onscreen=1;

            //выделить память под кривую, она не нарисуется т.к. disp_rg_axes_crv() checks if (crv has > 2 points),
            // но позволит видеть region
            /*
             n_sp=2;
             int num_crv = 0;
             int ret_stat = one_crv_alloc(J_act_rg, num_crv, n_sp);//CRV[0].nall=n_sp;ncrv=ncrv!!!;
             rg.ncrv=1;
	        
             //считать,cделать массивы X,Y
             py=rg.CRV[0].py; px=rg.CRV[0].px;
             for(i  = 0; i < n_sp; i++) py[i] = 0.0;
             for (i = 0; i < n_sp; i++) px[i] = (double)i;

             //Установить параметры области, кроме курсора и реперов
     //        SetChartLimitsByCurves(J_act_rg);
             */
            return 1;
        }

        public MathLimits ConvertToMatLimits(ScreenLimits lLimits, MathToScreenMatrix _alpha)
        {
            MathLimits limits = new MathLimits()
            {
                xmin = (lLimits.u1 - _alpha.a12) / _alpha.a11,
                ymin = (lLimits.v2 - _alpha.a22) / _alpha.a21,
                xmax = (lLimits.u2 - _alpha.a12) / _alpha.a11,
                ymax = (lLimits.v1 - _alpha.a22) / _alpha.a21 // Ymax raschiyvaetsya po Vmin, 
            };
            return limits;
        }

        public Point ConvertToMatPoint(Point sp, MathToScreenMatrix _alpha)
        {
            Point mPoint = new Point();

            mPoint.X = (sp.X - _alpha.a12) / _alpha.a11;
            mPoint.Y = (sp.Y - _alpha.a22) / _alpha.a21;
            return mPoint;
        }

        public ScreenLimits ConvertToScreenLimits(MathLimits mLimits, MathToScreenMatrix _alpha)
        {
            ScreenLimits sLimits = new ScreenLimits();

            sLimits.u1 = _alpha.a11 * mLimits.xmin + _alpha.a12;
            sLimits.v1 = _alpha.a21 * mLimits.ymax + _alpha.a22; // Vmin raschiyvaetsya po Ymax, 
            sLimits.u2 = _alpha.a11 * mLimits.xmax + _alpha.a12;
            sLimits.v2 = _alpha.a21 * mLimits.ymin + _alpha.a22;
            return sLimits;
        }

        public Point ConvertToScreenPoint(Point mp, MathToScreenMatrix _alpha)
        {
            Point sPoint = new Point();

            sPoint.X = _alpha.a11 * mp.X + _alpha.a12;
            sPoint.Y = _alpha.a21 * mp.Y + _alpha.a22;
            return sPoint;
        }

        //----------------------------------------------------------------------------------------
        //        Закрашивает и подписывает область рисования графиков
        //----------------------------------------------------------------------------------------
        public void disp_region()
        {
            // postavit' eto v setting: active_Region_color, non_active = default
            if (_activeRegion) _regionStyle.brd_clr = 14;
            else _regionStyle.brd_clr = 15;
            var rg = screenLimits;
            GraforPage.draw_bar(_canvas, rg.u1, rg.v1, rg.u2, rg.v2, _regionStyle.fon_clr);

            GraforPage.draw_border(_canvas, rg.u1 + 1, rg.v1 + 1, rg.u2 - 1, rg.v2 - 1, _regionStyle.brd_clr);

            //GraforPage.draw_bar_with_border(_canvas, _u1, _v1, _u2, _v2, _tRg.fon_clr, _tRg.brd_clr);
            //draw_bar(_u1 + _tRg.brd, _v1 + _tRg.brd, _u2 - _tRg.brd, _v2 - _tRg.brd, _tRg.fon_clr);
            Size str_size = GraforPage.GetStringSize(_regionStyle.title);
            if (_regionStyle._textLocationAuto)
            {
                _regionStyle.txtu = rg.u1 + (int)(0.5 * (double)(rg.u2 - rg.u1 - str_size.Width));
                _regionStyle.txtv = rg.v1 + 1;
            }
            GraforPage.draw_string_horizontal(_canvas, _regionStyle.txtu, _regionStyle.txtv, _regionStyle.title, _regionStyle.txt_clr, _regionStyle.txt_fon);
        }

        //---------------------------------------------------------------------------
        //        public void disp_rg_axes_crv()
        //        {
        //            disp_region();
        //            //DrawAxisX(_cnvRegion);
        //            //foreach (TCurve crv in Curves)
        //            //disp_crv(crv);
        //            //DrawAxisX(_cnvRegion);
        //        }

        //---------------------------------------------------------------
        public void disp_crv(TCurve crv, MathToScreenMatrix _alpha)
        {
            //if (ToBitmap==0) can=BoxForSp.Canvas;
            //else 			 can=Bitmap.Canvas;
            double u1, v1;
            //CurvesRegion rgLimits = _chartRegion.crvsRegion;
            var limits = _curvesRegion.screenLimits;
            if (crv._bar)// it doesn't work good
            {
                double u2, v3;
                for (int i = 0; i < crv.nall - 1; i++)
                //for (int i = 0; i < 3 - 1; i++)
                {
                    u1 = (double)(_alpha.a11 * crv.px[i] + _alpha.a12);
                    v1 = (double)(_alpha.a21 * crv.py[i] + _alpha.a22);
                    u2 = (double)(_alpha.a11 * crv.px[i + 1] + _alpha.a12);
                    v3 = _alpha.a22;//y=0

                    // it is not finished (see "not bar curves" below)
                    // if point is out of chart region, dont draw it, and dont
                    // leave the loop.
                    if (u1 < limits.u1 || u1 > limits.u2 || u2 < limits.u1 || u2 > limits.u2 || v1 < limits.v1 || v1 > limits.v2)
                        continue;
                    Rectangle rec = new Rectangle();
                    rec.Fill = GraforMain.m_Brushes[crv.clr];
                    double uStart = u1 + (crv._crvIdx) * (u2 - u1) / Curves.Count;
                    rec.Width = Math.Abs((u2 - u1 - 1) / Curves.Count);//+1
                    rec.Height = Math.Abs(v3 - v1);
                    _canvas.Children.Add(rec);
                    _UIElements.Add(rec);

                    Canvas.SetLeft(rec, uStart);
                    Canvas.SetTop(rec, v1);
                    //Canvas.SetZIndex(_cnvRegion, 0); // 10 - not trans
                    Canvas.SetZIndex(rec, 0); // 10 - not trans
                }
            }
            else
            {
                Polyline pl = new Polyline();
                pl.Stroke = GraforMain.m_Brushes[crv.clr];
                pl.StrokeThickness = crv.width;
                pl.StrokeDashArray = GraforMain.GetPattern(crv.pattern);
                //pl.Stroke = Brushes.Transparent;
                for (int i = 0; i < crv.nall; i++)
                {

                    if (crv.px[i] < _curvesRegion.mathLimits.xmin) continue;
                    if (_curvesRegion.mathLimits.xmax < crv.px[i]) continue;
                    //eu
                    //if (_curvesRegion.mathLimits.xmax < crv.px[i]) break;

                    u1 = (double)(_alpha.a11 * crv.px[i] + _alpha.a12);
                    v1 = (double)(_alpha.a21 * crv.py[i] + _alpha.a22);

                    // if point is out of chart region, dont draw it, and dont
                    // leave the loop.
                    if (u1 < limits.u1 || u1 > limits.u2 || v1 < limits.v1 || v1 > limits.v2)
                        continue;
                    pl.Points.Add(new Point(u1, v1));
                }
                _canvas.Children.Add(pl);
                _UIElements.Add(pl);

                Canvas.SetZIndex(pl, 0); // 10 - not trans
            }
        }
    }
}
