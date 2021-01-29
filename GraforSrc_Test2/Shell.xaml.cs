using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GraforDllTest2;
using GraforWpfDll;

namespace GraforSrc_Test2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// It is a test for Grafor source code.
    /// 
    /// If the test works, then we can create GraforWpfDll (WPF User Control Library)
    /// and use it in solution, which uses GraforWpfDll
    /// 
    /// </summary>
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();// Create instance of GraforPage User Control.
            GraforTwoRegionsPage.SetRegionsOnPage(ctlGraforPage);// set page and regions before set data
            //SetGraforPageData(ctlGraforPage);
            SetOptionsData(ctlGraforPage);
            ctlGraforPage.Draw();
        }

        private void SetOptionsData(GraforPage l_graforPage)
        {
            List<GraforRegion> graforRegions = l_graforPage.graforRegions;
            // set data for region[0]
            graforRegions[0].InitLimitsByCurves();
            //graforRegions[0].AddCurve(SetCurve(0, 1, new List<double>() { 2483, 2473, 2470, 2486, 2488 })); //Fri, 04/03/2020
            //?? graforRegions[0].AddCurve(SetCurve(1, 1, new List<double>() { 2632, 2642, 2646, 2662, 2661 })); //Mon, 04/06/2020
            //graforRegions[0].AddCurve(SetCurve(2, 1, new List<double>() { 2668, 2687, 2693, 2675, 2658 })); //Tue, 04/07/2020
            //?? graforRegions[0].AddCurve(SetCurve(3, 1, new List<double>() { 2750, 2733, 2739, 2739, 2749 })); //Wed, 04/08/2020
            //graforRegions[0].AddCurve(SetCurve(4, 1, new List<double>() { 2752, 2756, 2760, 2753, 2761 })); //Mon, 04/13/2020
            //graforRegions[0].AddCurve(SetCurve(5, 1, new List<double>() { 2840, 2846, 2843, 2844, 2846 })); //Tue, 04/14/2020
      //      graforRegions[0].AddCurve(SetCurve(4, 2, new List<double>() { 2782, 2777, 2782, 2780, 2780 })); //Wed, 04/15/2020
            //graforRegions[0].AddCurve(SetCurve(7, 1, new List<double>() { 2793, 2793, 2791, 2799, 2794 })); //Thu, 04/16/2020
           // graforRegions[0].AddCurve(SetCurve(5, 2, new List<double>() { 2851, 2859, 2860, 2873, 2869 })); //Fri, 04/17/2020
            //graforRegions[0].AddCurve(SetCurve(9, 1, new List<double>() { 2828.7, 2830.3, 2828.5, 2825, 2823 })); //Mon, 04/20/2020
            //graforRegions[0].AddCurve(SetCurve(0, 1, new List<double>() { 2751, 2753.8, 2747.3, 2741, 2737 })); //Tue, 04/21/2020
        //    graforRegions[0].AddCurve(SetCurve(4, 2, new List<double>() { 2808.7, 2807.5, 2808.7, 2813.1, 2799 })); //Wed, 04/22/2020
            //graforRegions[0].AddCurve(SetCurve(2, 2, new List<double>() { 2800, 2807, 2804.85, 2803.85, 2797.39 })); //Thu, 04/23/2020
            graforRegions[0].AddCurve(SetCurve(5, 2, new List<double>() { 2834.3, 2839.74, 2842, 2832, 2836.74 })); //Fri, 04/24/2020
            //graforRegions[0].AddCurve(SetCurve(4, 2, new List<double>() { 2882, 2885.5, 2886.44, 2884.5, 2878.48 })); //Mon, 04/27/2020
            //graforRegions[0].AddCurve(SetCurve(5, 2, new List<double>() { 2882, 2878, 2875, 2868.7, 2863 })); //Tue, 04/28/2020
            //graforRegions[0].AddCurve(SetCurve(4, 2, new List<double>() { 2954, 2953, 2952.4, 2946.25, 2939.6 })); //Wed, 04/29/2020
            //graforRegions[0].AddCurve(SetCurve(7, 2, new List<double>() { 2912.3, 2907.4, 2907.48, 2910.5, 2911.35 })); //Thu, 04/30/2020
            graforRegions[0].AddCurve(SetCurve(5, 2, new List<double>() { 2832.7, 2830.75, 2830, 2827.6, 2830 })); //Fri, 05/01/2020
            //graforRegions[0].AddCurve(SetCurve(0, 2, new List<double>() { 2838, 2842, 2842.9, 2841, 2842 })); //Mon, 05/04/2020
            //graforRegions[0].AddCurve(SetCurve(1, 2, new List<double>() { 2877.5, 2868, 2870.6, 2873.5, 2868 })); //Tue, 05/05/2020
            //graforRegions[0].AddCurve(SetCurve(4, 2, new List<double>() { 2862.5, 2855.5, 2854.48, 2852, 2848 })); //Wed, 05/06/2020
            //graforRegions[0].AddCurve(SetCurve(3, 2, new List<double>() { 2881.2, 2877.5, 2880.9, 2882, 2881.1 })); //Thu, 05/07/2020
            graforRegions[0].AddCurve(SetCurve(5, 1, new List<double>() { 2923.5, 2928.5, 2927.6, 2928.5, 2929.8 })); //Fri, 05/08/2020
            //graforRegions[0].AddCurve(SetCurve(8, 2, new List<double>() { 2938.9, 2936.07, 2937.5, 2935.9, 2930.19})); //Mon, 05/11/2020
            //graforRegions[0].AddCurve(SetCurve(3, 2, new List<double>() { 2900.81, 2889.06, 2881.97, 2879.68, 2869.76 })); //Tue, 05/12/2020
            //graforRegions[0].AddCurve(SetCurve(7, 2, new List<double>() { 2800.5, 2809.4, 2808.8, 2814.42, 2820.0 }));       //Wed, 05/13/2020
            graforRegions[0].AddCurve(SetCurve(4, 3, new List<double>() { 2857.0, 2851, 2851, 2859, 2863.7 }));       //Fri, 05/15/2020

            // set data for region[1]
            graforRegions[1].AddCurve(SetCurve(5, 1, new List<double>() { 2668, 2687, 2693, 2675, 2658 }));//Tue, 04/07/2020
        }

        private TCurve SetCurve(int crvIndex, int width, List<double> yInput)
        {
            // normalised
            List<double> y = new List<double>();
            foreach (double yi in yInput) y.Add(yi - yInput[0]);

            List<Point> lPoints = new List<Point>()
            {
                new Point(30, y[0]), new Point(40,y[1]), new Point(45,y[2]), new Point(50,y[3]), new Point(60,y[4]),
            };

            TCurve crv = new TCurve(lPoints);
            crv._crvIdx = crvIndex;
            crv.SetCurveType(crvIndex + 1, GraforMain.LinePattern.Solid, width);
            return crv;
        }

        private void SetGraforPageData(GraforPage l_graforPage)
        {
            List<GraforRegion> graforRegions = l_graforPage.graforRegions;

            int n = 150;
            double lam = 10.0 / n;
            List<Point> lPoints = new List<Point>();
            for (int i = 0; i < n; i++)
            {
                Point p = new Point(i, 1.0 - Math.Exp(-lam * i));
                lPoints.Add(p);
            }
            TCurve crv1 = new TCurve(lPoints);
            crv1._crvIdx = 0;
            crv1.SetCurveType(1, GraforMain.LinePattern.Solid, 1);

            graforRegions[0].AddCurve(crv1);
            graforRegions[1].AddCurve(crv1);
        }

        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {   //call this command from VM
            ctlGraforPage.PageSizeChanged();
        }
    }
}
