using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

// !!!!!!!!!!!!!!!!!!!
// Any changes on Grafor should be made on this project only.
// The separate Grafor project is absolete
// !!!!!!!!!!!!!!!!!!!

// GraforWpfDll is a WPF User Control Library.
// The Region just a class with Canvas reference, not a UserControl :(

// History:
// Removed parameters SymH, SymW from grRg1.SetRegionSize() method.

namespace GraforWpfDll
{
    public partial class GraforMain
    {

        public enum LinePattern
        {
            Solid = 1,
            Dash1 = 2,
            Dot = 3,
            DashDot = 4,
            Dash2 = 5
        }

        public enum State
        {
            Created = 1,
            OnScreen = 2,
            Closed = 3
        }

 
        /*
        public static Color[] m_Colors = new Color[16]  
        {
            Color.FromRgb(255, 255, 255),   // white
            Color.FromRgb(255, 0, 0),       // red
            Color.FromRgb(0, 255, 0),       // green
            Color.FromRgb(0, 0, 255),       // blue

            Color.FromRgb(255, 100, 100),   // red1
            Color.FromRgb(100, 255, 100),   // 
            Color.FromRgb(100, 100, 255),   // 

            Color.FromRgb(0, 255, 255),     // yellow
            Color.FromRgb(255, 0, 255),     // 
            Color.FromRgb(255, 255, 0),     // 

            Color.FromRgb(100, 255, 255),   // yellow1
            Color.FromRgb(255, 100, 255),   // 
            Color.FromRgb(255, 255, 100),   // 

            Color.FromRgb(150, 150, 150),   // gray1
            Color.FromRgb(77, 77, 77),      // gray2
            Color.FromRgb(0, 0, 0)          // black
        };
        */
        //        public static DoubleCollection[] ddd = new DoubleCollection[]  
        //        {
        //            new DoubleCollection(new double[2] { 1, 0 }),       // Solid. it is hard coded in disp_crv()
        //            new DoubleCollection(new double[2] { 4, 3 }),       // Dash1
        //            new DoubleCollection(new double[2] { 1, 2 }),       // Dot
        //            new DoubleCollection(new double[4] { 4, 2, 1, 2 }), // DashDot
        //            new DoubleCollection(new double[2] { 1, 2 }),       // Dash2
        //        };
        public static DoubleCollection GetPattern(LinePattern pattern)
        {
            var p = new DoubleCollection(new double[2] {1, 0});

            switch (pattern)
            {
                case LinePattern.Dash1:
                    p = new DoubleCollection(new double[2] { 4, 3 });
                    break;
                case LinePattern.Dot:
                    p = new DoubleCollection(new double[2] { 1, 2 });
                    break;
                case LinePattern.DashDot:
                    p = new DoubleCollection(new double[4] { 4, 2, 1, 2 });
                    break;
                case LinePattern.Dash2:
                    p = new DoubleCollection(new double[2] { 1, 2 });
                    break;
            }
            return p;
        }


        //RGB gamma
        public static Brush[] m_Brushes = new Brush[16]  
        {
            new SolidColorBrush(Color.FromRgb(255, 255, 255)),   // 0 white
            new SolidColorBrush(Color.FromRgb(255, 0, 0)),       // 1 red
            new SolidColorBrush(Color.FromRgb(0, 255, 0)),       // 2 green
            new SolidColorBrush(Color.FromRgb(0, 0, 255)),       // 3 blue

            new SolidColorBrush(Color.FromRgb(255, 100, 100)),   // 4 red1
            new SolidColorBrush(Color.FromRgb( 100,   0,  100)), // 5 violet
            new SolidColorBrush(Color.FromRgb( 99, 180, 171)),   // 6 blue region border

            new SolidColorBrush(Color.FromRgb(0, 255, 255)),     // 7 yellow
            new SolidColorBrush(Color.FromRgb(255, 0, 255)),     // 8
            new SolidColorBrush(Color.FromRgb(255, 255, 0)),     // 9

            new SolidColorBrush(Color.FromRgb(245, 243, 249)),   // 10 region fon violet
            new SolidColorBrush(Color.FromRgb(248, 247, 240)),   // 11 region fon yellow
            new SolidColorBrush(Color.FromRgb(239, 248, 247)),   // 12 region fon blue

            new SolidColorBrush(Color.FromRgb(150, 150, 150)),   // 13 gray1
            new SolidColorBrush(Color.FromRgb(255, 0, 0)),      // 14 gray2 // 77,77,77 - Gray
            new SolidColorBrush(Color.FromRgb(0, 0, 0))          // 15 black
        };

        // Blue gamma
//        public static Brush[] m_Brushes = new Brush[16]  
//        {
//            new SolidColorBrush(Color.FromRgb( 255, 255, 255)),   // 0 white
//            //new SolidColorBrush(Color.FromRgb( 136,  191,  252)),       // 1 light
//            //new SolidColorBrush(Color.FromRgb( 227,  202,  254)),       // 2 
//            //new SolidColorBrush(Color.FromRgb( 148,  254,  221)),       // 3 
//
//            new SolidColorBrush(Color.FromRgb( 123,  206,  243)),       // 2 medium
//            new SolidColorBrush(Color.FromRgb( 205,  125,  241)),       // 1 
//            new SolidColorBrush(Color.FromRgb( 061,  215,  098)),       // 6 
//
//            new SolidColorBrush(Color.FromRgb( 005,  008,  167)),       // 7 dark
//            new SolidColorBrush(Color.FromRgb( 121,  007,  248)),       // 8 
//            new SolidColorBrush(Color.FromRgb( 023,  111,  044)),       // 9
//
//            new SolidColorBrush(Color.FromRgb( 136,  191,  252)),       // 1 light
//            new SolidColorBrush(Color.FromRgb( 227,  202,  254)),       // 2 
//            new SolidColorBrush(Color.FromRgb( 179,  239,  193)),       // 3 
//
//
//            new SolidColorBrush(Color.FromRgb(245, 243, 249)),   // 10 region fon violet
//            new SolidColorBrush(Color.FromRgb(248, 247, 240)),   // 11 region fon yellow
//            new SolidColorBrush(Color.FromRgb(239, 248, 247)),   // 12 region fon blue
//
//            new SolidColorBrush(Color.FromRgb(251, 184, 143)),   // 13 red light
//            new SolidColorBrush(Color.FromRgb(250, 0, 0)),       // 14 red
//            new SolidColorBrush(Color.FromRgb(0, 0, 0))          // 15 black
//        };
    }

    public class TestVars
    {
        public double[] X_sp, Y_sp;
        public long N_sp;
        public int N_smooth_pnt; // количество точек усреднения
        // в сглаженном типе кривой
        // double[] X_sp2;
        // double[] Y_sp2;
        // long int N_sp1,N_sp2;

        public int N_rg, J_act_rg;//,N_rg_alloc;
        public double[] Arrx_test, Arry_test;
    };
}



