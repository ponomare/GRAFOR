using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace GraforWpfDll
{
    public partial class GraforPage : UserControl
    {
        //  Pri rastyazhenii page UI element (eg. TextBlock ) ne ischezaet esli m != 0 !!!!!!!!!!!!
        //Canvas.SetZIndex(pl, m); 
        // 10 - not trans
        // 5 - 50% trans
        // 0 - trans


        //---------------------------------------------------------------
        static public void draw_string_horizontal(Canvas canvas,
                                                  double u1, double v1,
                                                  string str, int col_char, int col_fon)
        {
            double u2, v2;
            //int col_ch_old,brd;
            //int brd=1;
            //int h,w;

            if (str.Length == 0) return;
            Size size = new Size(0, 0);
            TextBlock tb = new TextBlock();
            tb.FontSize = 16;
            tb.Text = str;
            tb.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            size = tb.DesiredSize; //keeps size  of string now

            //Canvas.SetLeft(textCanvas, u1);
            //Canvas.SetTop(textCanvas, v1);
            Canvas.SetLeft(tb, u1);
            Canvas.SetTop(tb, v1);
            Canvas.SetZIndex(tb, 0); // 10 - not transperent


            /*
            if (ToBitmap == 0)
            {
                BoxForSp.Canvas.Font.Color = col_char;
                txt_l = BoxForSp.Canvas.TextWidth(s);
            }
            else
            {
                Bitmap.Canvas.Font.Color = col_char;//Bitmap
                txt_l = Bitmap.Canvas.TextWidth(s);//Bitmap
            }
             */
            //int brd = 1;
            u2 = u1 + size.Width - 1; // txt_l - 1;
            v2 = v1 + size.Height - 1; // +Symh - 1;
                                       //draw_bar(u1 - brd, v1 - brd, u2 + brd, v2 + brd, col_fon);

            canvas.Children.Add(tb);////
            _UIElements.Add(tb);
            /*
            if (ToBitmap == 0)
            {
                BoxForSp.Canvas.Brush.Color = col_fon; //!!!!!!!!!!!!!!!!!!
                BoxForSp.Canvas.TextOut(u1, v1, s);
            }
            else
            {
                Bitmap.Canvas.Brush.Color = col_fon; //!!!!!!!!!!!!!!!!!!
                Bitmap.Canvas.TextOut(u1, v1, s);    //Bitmap
            }
             */
        }

        //---------------------------------------------------------------
        static public void draw_string_vertical(Canvas canvas,
                                                 double u1, double v1,
                                                 string str, int col_char, int col_fon)
        {
            //int u2, v2;
            if (str.Length == 0) return;
            Size size = new Size(0, 0);
            TextBlock tb = new TextBlock();
            //tb.FontSize = 16;
            tb.Text = str;
            tb.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            size = tb.DesiredSize; //keeps size  of string now

            Canvas.SetLeft(tb, u1);
            Canvas.SetTop(tb, v1);
            Canvas.SetZIndex(tb, 0); // 10 - not transperent
            tb.RenderTransform = new RotateTransform(-90);

            //RotateTransform rt = new RotateTransform(-90);
            //TransformGroup tg = new TransformGroup();
            //tg.Children.Add(rt);
            //tb.RenderTransform = tg;

            //u2 = u1 + (int)size.Width - 1; // txt_l - 1;
            //v2 = v1 + (int)size.Height - 1; // +Symh - 1;
            //draw_bar(..)
            canvas.Children.Add(tb);
            _UIElements.Add(tb);
        }

        //---------------------------------------------------------------
        static public void draw_bar( Canvas canvas,
                                     double u1, double v1, 
                                     double u2, double v2, int color)
        {
            // get bar for  stirng

            // textCanvas.Width = chartGrid.ActualWidth;
            //  textCanvas.Height = chartGrid.ActualHeight;
            //  _cnvRegion.Width = textCanvas.Width;  // -20 padaet esli sil'no umen'shit' okno//???
            //  _cnvRegion.Height = textCanvas.Height;

            Rectangle rec = new Rectangle();
            rec.Fill = GraforMain.m_Brushes[color];
            rec.Width = Math.Abs(u2 - u1 + 1);
            rec.Height = Math.Abs(v2 - v1 + 1);
            Canvas.SetLeft(rec, u1);
            Canvas.SetTop(rec, v1);// u1);//?? why?? rec or _cnvRegion?? 
            //Canvas.SetZIndex(_cnvRegion, 0); // 10 - not trans
            Canvas.SetZIndex(rec, 0); // 10 - not trans
            canvas.Children.Add(rec);
            _UIElements.Add(rec);
        }
        //---------------------------------------------------------------
        static public void draw_bar_with_border(Canvas canvas,
                                                 double u1, double v1,
                                                 double u2, double v2, int color, int borderColor)
        {
            // get bar for  stirng

            // textCanvas.Width = chartGrid.ActualWidth;
            //  textCanvas.Height = chartGrid.ActualHeight;
            //  _cnvRegion.Width = textCanvas.Width;  // -20 padaet esli sil'no umen'shit' okno//???
            //  _cnvRegion.Height = textCanvas.Height;

            if (color < 0 || color > 15 || borderColor < 0 || borderColor > 15)
            { color = 7; borderColor = 1; }
            Rectangle rec = new Rectangle();
            rec.Fill = GraforMain.m_Brushes[color];
            rec.Stroke = GraforMain.m_Brushes[borderColor];
            rec.Width = Math.Abs(u2 - u1);
            rec.Height = Math.Abs(v2 - v1);
            Canvas.SetLeft(rec, u1);
            Canvas.SetTop(rec, v1);// u1);//?? why?? rec or _cnvRegion?? 
            //Canvas.SetZIndex(_cnvRegion, 0); // 10 - not trans
            Canvas.SetZIndex(rec, 0); // 10 - not trans
            canvas.Children.Add(rec);
            _UIElements.Add(rec);

        }

        //---------------------------------------------------------------
        static public void draw_border(Canvas canvas,
                                       double u1, double v1,
                                       double u2, double v2, int color)
        {
            // get bar for  stirng

            // textCanvas.Width = chartGrid.ActualWidth;
            //  textCanvas.Height = chartGrid.ActualHeight;
            //  _cnvRegion.Width = textCanvas.Width;  // -20 padaet esli sil'no umen'shit' okno//???
            //  _cnvRegion.Height = textCanvas.Height;

            if (color < 0 || color > 15) color = 7;

            Polyline pl = new Polyline();
            pl.Stroke = GraforMain.m_Brushes[color];
            pl.StrokeThickness  = 2;
            pl.Points.Add(new Point(u1, v1));
            pl.Points.Add(new Point(u2, v1));
            pl.Points.Add(new Point(u2, v2));
            pl.Points.Add(new Point(u1, v2));
            pl.Points.Add(new Point(u1, v1));
            
            canvas.Children.Add(pl);
            _UIElements.Add(pl);
            Canvas.SetZIndex(pl, 0); // 10 - not trans
            //Canvas.SetLeft(pl, 5); // 5 - 50% trans
            //Canvas.SetTop(pl, 0); // 0 - trans
        }

    } //public partial class GraforMain
}
