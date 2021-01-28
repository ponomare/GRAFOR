using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows; //
using GraforWpfDll;

namespace GraforSrc_Test2
{
    // The class keeps information about layout of one region page.
    public class GraforOneRegionPage //wpf page
    {
        //public GraforPage graforPage =  new GraforPage(700, 550);
        Gr_OneRegionData _oneRegionData = null;


        // 
        // SetRegionsOnPage() creates layout for page: 
        // number of regions, position of each region, name, axese names, color and soon.
        //
        public void SetRegionsOnPage(GraforPage graforPage)
        {
            //________________________________________ set region 1 __________________________________________________________________

            // Set empty regions: Allocate memory for region. Calculate and set left upper point and size of region 

            GraforRegion grRg1 = new GraforRegion(0, graforPage.GetCanvas());
            graforPage.graforRegions.Add(grRg1);

            // pu1_frm,..pv2_frm parts of page, allocated for region.
            // For example: 0,0,0.5,1.0 - region takes left part of page, 
            // i.e. left/upper = 0.0*PageWidth/0.0*PageHeight and right/lower = 0.5*PageWidth/1.0*PageHeight
            double pu1_frm = 0.0, pv1_frm = 0.0, pu2_frm = 1.0, pv2_frm = 1.0;
            //grRg1.CalculateRegionSizeAndPositionOnPage(graforPage.PageWidth, graforPage.PageHeight);
            //grRg1.SetLimitsForRegion(0.0, 0.0, 1.0, 1.0); // it sets _xmin_chart,_ymin_chart,... and  _xmin,_ymin for Region

            // Set region parameters, which are not dependent on region size: color, text, etc.
            int aut = 1;
            var bt = new double[] { 0.05, 0.05, 0.2, 0.05 }; 

            int j_dnx = 1; //признак убывания (=-1) или возраст.(=+1) оси X
            //int ncrv, //количество кривых в данной области функции
            int fon = 12;// 10 - violet 11 - yellow 12 - blue; цвет фона области
            int brd = 1; // = 1...10 - меню с рамкой,0 - без рамки
            int brd_clr = 6; // цвет рамки области
            //int txtu = 0,  txtv = 0; //not used (координаты левого верхнего угла заголовка области)
            int txth = 1; //размер шрифта букв заголовка области
            int txt_clr = 15, txt_fon = 11; //цвет букв и фона заголовка области
            string txt = "Grafor region."; //адрес cтроки названия области

            grRg1._curvesRegion.SetMathLimits(0.0, 0.0, 1.0, 1.0);
            grRg1._curvesRegion.SetMathLimitsForAllCurves(0.0, 0.0, 1.0, 1.0);
            grRg1.SetRegionStyle(bt, j_dnx,
                             fon, brd, brd_clr,
                             txth, txt_clr, txt_fon, txt);
            var testClass = _oneRegionData;

            // If test class has not a reference to TCurve class, then
            // we use x[], y[], and n defined in test class:
            int pointsCounter = testClass._pointsCounter;
//      //------------------ set Axis X -----------------
            bool autoStep = true;
            bool axx_aut = true;
            double axx_y0 = 0.5;
            int axx_clr = 15;
            GraforMain.LinePattern grid_type = GraforMain.LinePattern.Dash1;
            double axx_step = 0; int axx_nsubstep = 5, axx_dec = 2;
            int axx_txtu = 20; int axx_txtv = 76; int axx_txth = 1;
            int axx_txt_clr = 15, axx_txt_fon = 11;
            string axx_txt = "Axis X";
            TAxisX axisx = new TAxisX();
            if (grRg1.AxisX.MakeAxisX(axx_aut, axx_y0,  axx_clr, grid_type,
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
            string axy_txt = "Axis Y";
            TAxisY axisy = new TAxisY();
            if (grRg1.AxisY.MakeAxisY(axy_aut, axy_0, axy_clr, grid_type,
                     autoStep, axy_step, axy_nsubstep, axy_dec,
                     axy_txtu, axy_txtv, axy_txth, axy_txt_clr, axy_txt_fon,
                     axy_txt) == 0) return; //exit(1);
            
        }
    }
}
