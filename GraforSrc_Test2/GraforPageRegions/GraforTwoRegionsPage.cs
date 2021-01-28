using GraforWpfDll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Dscribes the regions on the page. Region names, number of regions, location and soon are hardcoded in this namespace.
namespace GraforDllTest2
{
    // The class keeps information about two regions page.
    class GraforTwoRegionsPage
    {
        // 
        // SetRegionsOnPage() creates layout for page: 
        // number of regions, position of each region, name, axese names, color and soon.
        //
        public static void SetRegionsOnPage(GraforPage graforPage)
        {
            //________________________________________ set region 1 __________________________________________________________________

            // Set empty regions: Allocate memory for region. Calculate and set left upper point and size of region 

            GraforRegion grRg1 = new GraforRegion();
            grRg1.Id = 0;
            grRg1._canvas = graforPage.GetCanvas();
            graforPage.graforRegions.Add(grRg1);

            // pu1_frm,..pv2_frm parts of page, allocated for region.
            // For example: 0,0,0.5,1.0 - region takes left part of page, 
            // i.e. left/upper = 0.0*PageWidth/0.0*PageHeight and right/lower = 0.5*PageWidth/1.0*PageHeight
            double pu1_frm = 0.0, pv1_frm = 0.0, pu2_frm = 1.0, pv2_frm = 0.5;
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
            grRg1.SetRegionPositionParameters(pu1_frm, pv1_frm, pu2_frm, pv2_frm);

            //grRg1.InitLimitsByCurves();
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
            //grRg1.DrawRegion();
            //for (int i = 0; i < _regionsCount; ++i)
            //        graforRegions[i].disp_rg_axes_crv();


            //________________________________________ set region 2 __________________________________________________________________

            // Empty region for test
            GraforRegion grRg2 = new GraforRegion();
            grRg2.Id = 1;
            grRg2._canvas = graforPage.GetCanvas();
            graforPage.graforRegions.Add(grRg2);

            // pu1_frm,..pv2_frm parts of page, allocated for region.
            // For example: 0.5,0.0,0.5,1.0 - region takes right part of page, 
            // i.e. left/upper = 0.5*PageWidth/0.0*PageHeight and right/lower = 0.95*PageWidth/1.0*PageHeight
            pu1_frm = 0.0; pv1_frm = 0.5; pu2_frm = 1.0; pv2_frm = 1.0;

            // Set region parameters, which are not dependent on region size: color, text, etc.
            aut = 1;
            bt = new double[] { 0.05, 0.05, 0.05, 0.05 }; //
            //int fon2 = 7;////

            txt = "Service Level[%] for skill id 1-red, 2-green. 10*Cost(K) - blue";
            //" Black curve is a total number of calls for skill = 1    ";

            grRg2.SetRegionStyle(bt, j_dnx,
                             fon, brd, brd_clr,
                             txth, txt_clr, txt_fon, txt);
            grRg2.SetRegionPositionParameters(pu1_frm, pv1_frm, pu2_frm, pv2_frm);

            //------------------ set 1st curve for the region -----------------

            //bt[] must be initialized
            grRg2._curvesRegion.SetMathLimits(0.0, 0.0, 1.0, 1.0);
            //grRg2.InitLimitsByCurves();
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

            //grRg2.DrawRegion();
        }


    }
}
