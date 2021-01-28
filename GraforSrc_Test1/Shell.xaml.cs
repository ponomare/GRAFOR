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
using GraforWpfDll;
using WpfGrafor;
using WpfGrafor.UserControls;

namespace GraforSrc_Test1
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
        private Gr_CurveSamplesGraphics graforPageGrafics;
        public Shell()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            GraforPage graforPage = new GraforPage();
            List<GraforRegion> graforRegions = new List<GraforRegion>();

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

            GraforRegion grRg1 = new GraforRegion();
            grRg1.Curves.Add(crv1);
            graforRegions.Add(grRg1);

            GraforRegion grRg2 = new GraforRegion();
            grRg2.Curves.Add(crv1);
            graforRegions.Add(grRg2);

            graforPage.SetPageStyleAndChartData(graforRegions);// se

            //graforPage.Show();
            graforPage.PageState = GraforMain.State.OnScreen;
        }

        //private void button1_Click(object sender, RoutedEventArgs e)
        //{
        //    var curveSamplesData = new Gr_CurveSamplesData();
        //    graforPageGrafics = new Gr_CurveSamplesGraphics(curveSamplesData);
        //    graforPageGrafics._graforPage.Show();
        //    graforPageGrafics._graforPage.PageState = GraforMain.State.OnScreen;
        //}

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (graforPageGrafics._graforPage.PageState == GraforMain.State.OnScreen)
            {
                graforPageGrafics._graforPage.ReDrawPage();
            }
        }
    }
}
