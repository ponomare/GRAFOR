using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;//
using GraforWpfDll;

namespace WpfGrafor.UserControls
{
    public class Gr_CurveSamplesGraphics
    {
        public GraforPage _graforPage;

        Gr_CurveSamplesData _curveSamplesData;

        public Gr_CurveSamplesGraphics(Gr_CurveSamplesData curveSamplesData)
        {
            _curveSamplesData = curveSamplesData;
            //Style = (Style)FindResource(typeof(Window));
            _graforPage = new GraforPage();
            //_graforPage = new GraforPage(800, 500);

            SetPageStyleAndChartData();// set regions' size, colors, limits, curves, axes
        }

        public void InitPageForProductionCurveSamples()
        {
        }
        
        //
        // InitPageForScheduleTestCases() creates layout for page to display results from "test cases for schedule"
        //
        public void SetPageStyleAndChartData()
        {
            //________________________________________ set region 1 __________________________________________________________________

            // Set empty regions: Allocate memory for region. Calculate and set left upper point and size of region 

            GraforRegion grRg1 = new GraforRegion(0, _graforPage.GetCanvas());
            _graforPage.graforRegions.Add(grRg1);

            // pu1_frm,..pv2_frm parts of page, allocated for region.
            // For example: 0,0,0.5,1.0 - region takes left part of page, 
            // i.e. left/upper = 0.0*PageWidth/0.0*PageHeight and right/lower = 0.5*PageWidth/1.0*PageHeight
            double pu1_frm = 0.0, pv1_frm = 0.0, pu2_frm = 1.0, pv2_frm = 0.5;
            grRg1.SetRegionSize(_graforPage.PageWidth, _graforPage.PageHeight, pu1_frm, pv1_frm, pu2_frm, pv2_frm);
            //grRg1.SetLimitsForRegion(0.0, 0.0, 1.0, 1.0); // it sets _xmin_chart,_ymin_chart,... and  _xmin,_ymin for Region



            // Set region parameters, which are not dependent on region size: color, text, etc.
            int aut = 1;
            double[] bt = new double[] { 0.05, 0.05, 0.2, 0.05 }; //

            int j_dnx = 1; //признак убывания (=-1) или возраст.(=+1) оси X
            //int ncrv, //количество кривых в данной области функции
            int fon = 12;// 10 - violet 11 - yellow 12 - blue; цвет фона области
            int brd = 1; // = 1...10 - меню с рамкой,0 - без рамки
            int brd_clr = 6; // цвет рамки области
            //int txtu = 0,  txtv = 0; //not used (координаты левого верхнего угла заголовка области)
            int txth = 1; //размер шрифта букв заголовка области
            int txt_clr = 15, txt_fon = 11; //цвет букв и фона заголовка области
            string txt = "Number of agents in groups:1 - red, 2 - green, 1,2 - black}."; //адрес cтроки названия области

            grRg1._curvesRegion.SetMathLimits(0.0, 0.0, 1.0, 1.0); 
            grRg1.SetRegionStyle(bt, j_dnx,
                             fon, brd, brd_clr,
                             txth, txt_clr, txt_fon, txt);

            var testClass = _curveSamplesData;

            // If test class has not a reference to TCurve class, then
            // we use x[], y[], and nMax is defined in test class:
            int pointsCounter = testClass._pointsCounter;
            TCurve crv1;
            
            //we can dynamically change x coordinate of curve defined in test class.
            var x = new double[pointsCounter];
            for (int jj = 0; jj < pointsCounter; jj++)
            {
                //x[jj] = cnsl.skillIntervals[iGroup, jj];
                x[jj] = testClass._sin1X[jj]*.02;
            }

            //we can just display a function from test class 
            crv1 = new TCurve(x, testClass._sin1Y, pointsCounter);
            crv1.SetCurveType(2, GraforMain.LinePattern.Solid, 2);//(crv_clr, crv_type, crv_width);
            grRg1.Curves.Add(crv1);
            crv1._crvIdx = grRg1.Curves.Count;

            //we can dynamically change y coordinate of curve read from test class.
            var y = new double[pointsCounter];
            for (int jj = 0; jj < pointsCounter; jj++)
            {
                y[jj] = testClass._sin1Y[jj] + 1;
            }
            crv1 = new TCurve(x, y, pointsCounter);
            crv1.SetCurveType(1, GraforMain.LinePattern.Solid, 1);//(crv_clr, crv_type, crv_width);
            grRg1.Curves.Add(crv1);
            crv1._crvIdx = grRg1.Curves.Count;
            
            //we can define fuction right here.
            y = new double[pointsCounter];
            for (int jj = 0; jj < pointsCounter; jj++)
            {
                y[jj] = 1.0;
            }
            crv1 = new TCurve(x, y, pointsCounter);
            crv1.SetCurveType(3, GraforMain.LinePattern.Dash1, 1);//(crv_clr, crv_type, crv_width);
            grRg1.Curves.Add(crv1);
            crv1._crvIdx = grRg1.Curves.Count;


            grRg1.InitLimitsByCurves();
//            grRg1._curvesRegion.SetMaxLimitsByCurves(grRg1.Curves);
//            grRg1._curvesRegion.SetMathLimits(grRg1._curvesRegion.maxLimits);

            //grRg1.SetChartLimits(grRg1._chartRegion.xmin, grRg1._chartRegion.ymin,
            //                        grRg1._chartRegion.xmax, grRg1._chartRegion.ymax);

            //------------------ set Axis X -----------------
            bool autoStep = true;
            bool axx_aut = true;
            double axx_y0 = 0.5;
            int axx_clr = 15;
            GraforMain.LinePattern grid_type = GraforMain.LinePattern.Dash1;
            double axx_step = 0; int axx_nsubstep = 5, axx_dec = 2;
            int axx_txtu = 20; int axx_txtv = 76; int axx_txth = 1;
            int axx_txt_clr = 15, axx_txt_fon = 11;
            string axx_txt = "Iteration";
            TAxisX axisx = new TAxisX();
            if (grRg1.AxisX.MakeAxisX(axx_aut, axx_y0, axx_clr, grid_type,
                     autoStep, axx_step, axx_nsubstep, axx_dec,
                     axx_txtu, axx_txtv, axx_txth, axx_txt_clr, axx_txt_fon,
                         axx_txt) == 0) return; // exit(1);


            //------------------ set Axis Y -----------------
            bool axy_aut = true;
            double axy_0 = 0.0;
            int axy_clr = 15;
            double axy_step = 0.0;// 5.0; 
            int axy_nsubstep = 5, axy_dec = 2;
            int axy_txtu = 420, axy_txtv = 133, axy_txth = 1;
            int axy_txt_clr = 15, axy_txt_fon = 12;
            string axy_txt = "N Agents";
            TAxisY axisy = new TAxisY();
            if (grRg1.AxisY.MakeAxisY(axy_aut, axy_0, axy_clr, grid_type,
                     autoStep, axy_step, axy_nsubstep, axy_dec,
                     axy_txtu, axy_txtv, axy_txth, axy_txt_clr, axy_txt_fon,
                     axy_txt) == 0) return; //exit(1);
            //grRg1._curvesRegion.SetBothLimitsByCurves(grRg1.Curves);
            grRg1.DrawRegion();
            //for (int i = 0; i < _regionsCount; ++i)
            //        graforRegions[i].disp_rg_axes_crv();


            //________________________________________ set region 2 __________________________________________________________________

            // Empty region for test
            GraforRegion grRg2 = new GraforRegion(1, _graforPage.GetCanvas());
            _graforPage.graforRegions.Add(grRg2);

            // pu1_frm,..pv2_frm parts of page, allocated for region.
            // For example: 0.5,0.0,0.5,1.0 - region takes right part of page, 
            // i.e. left/upper = 0.5*PageWidth/0.0*PageHeight and right/lower = 0.95*PageWidth/1.0*PageHeight
            pu1_frm = 0.0; pv1_frm = 0.5; pu2_frm = 1.0; pv2_frm = 1.0;
            grRg2.SetRegionSize(_graforPage.PageWidth, _graforPage.PageHeight, pu1_frm, pv1_frm, pu2_frm, pv2_frm);

            // Set region parameters, which are not dependent on region size: color, text, etc.
            aut = 1;
            bt = new double[] { 0.05, 0.05, 0.05, 0.05 }; //
            //int fon2 = 7;////

            txt = "Service Level[%] for skill id 1-red, 2-green. 10*Cost(K) - blue";
            //" Black curve is a total number of calls for skill = 1    ";

            grRg2.SetRegionStyle(bt, j_dnx,
                             fon, brd, brd_clr,
                             txth, txt_clr, txt_fon, txt);

            //------------------ set 1st curve for the region -----------------

            //bt[] must be initialized
            grRg2._curvesRegion.SetMathLimits(0.0, 0.0, 1.0, 1.0);

            // Or we use TCurve object defined in test class, (if the test class has a reference to TCurve class):
            crv1 = testClass.GetCurveSin();
            crv1.SetCurveType(1, GraforMain.LinePattern.Solid, 1);//(crv_clr, crv_type, crv_width);
            grRg2.Curves.Add(crv1);
            crv1._crvIdx = grRg2.Curves.Count;

            crv1 = testClass.GetCurveSin2();
            crv1.SetCurveType(3, GraforMain.LinePattern.Solid, 1);//(crv_clr, crv_type, crv_width);
            grRg2.Curves.Add(crv1);
            crv1._crvIdx = grRg2.Curves.Count;

//            grRg2._curvesRegion.SetMaxLimitsByCurves(grRg2.Curves);
//            grRg2._curvesRegion.SetMathLimits(grRg1._curvesRegion.maxLimits);
            grRg2.InitLimitsByCurves();
            //------------------ set Axis X -----------------
            axx_txt = "Iteration";
            if (grRg2.AxisX.MakeAxisX(axx_aut, axx_y0, axx_clr, grid_type,
                     autoStep, axx_step, axx_nsubstep, axx_dec,
                     axx_txtu, axx_txtv, axx_txth, axx_txt_clr, axx_txt_fon,
                         axx_txt) == 0) return; // exit(1);
            //grRg2.AxisX = axisx2;

            //------------------ set Axis Y -----------------
            axy_txt = "SL, %/Cost(K)";
            if (grRg2.AxisY.MakeAxisY(axy_aut, axy_0, axy_clr, grid_type,
                     autoStep, axy_step, axy_nsubstep, axy_dec,
                     axy_txtu, axy_txtv, axy_txth, axy_txt_clr, axy_txt_fon,
                     axy_txt) == 0) return; //exit(1);

            grRg2.DrawRegion();
        }
    }
}
