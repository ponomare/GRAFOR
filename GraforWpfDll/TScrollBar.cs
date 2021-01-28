using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace GraforWpfDll
{
    public class TScrollBar : ScrollBar
    {
        protected readonly long _scrollBarWidth = 17;
        protected readonly long _scrollBarDist = 1000;//distance of scrollBar. If min =1 then dits=max.
        protected readonly Color _color = Color.FromRgb(200, 228, 243); //move to regionStyle
        protected readonly long _min = 1;

        public long ScrollBarDist
        {
            get { return _scrollBarDist; }
        }
        public long ScrollBarWidth
        {
            get { return _scrollBarWidth; }
        }
        
        protected TScrollBar()
        {
           Background = new SolidColorBrush(_color); //new SolidColorBrush(Colors.Aqua);
           Minimum = _min;
           Maximum = _scrollBarDist;
           long reminder;
            // Value - position of scroll on the scrollBar. Value = min...max.
           Value = Math.DivRem(_scrollBarDist, 2, out reminder);//initial position is in the midle of scrollbar
        }
        
        public void DrawScrollBar(Canvas canvas)
        {
            if (canvas.Children.Contains(this))
                canvas.Children.Remove(this); 

           canvas.Children.Add(this);
        }
    }

    public class TScrollBarX : TScrollBar
    {
        public TScrollBarX() : base()
        { 
            Orientation = Orientation.Horizontal;
            HorizontalAlignment = HorizontalAlignment.Left;
        }
        public void SetScrollBarSizeAndPosition(ScreenLimits region)
        {
            Width = region.u2 - region.u1 - _scrollBarWidth;
            Height = _scrollBarWidth;

            // Set lef-upper corner of ScrollBarX
            Canvas.SetLeft(this, (region.u1));
            Canvas.SetTop(this, (region.v2 - _scrollBarWidth));
        }

        // Create the Scroll event handler. 
        //        private void ScrollBar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        // public void SetPosition(Object sender, ScrollEventArgs e)
        public void SetPosition(double xmin, double xmax, double xmin_all, double xmax_all)
        {
            double x_ost = ((xmax_all - xmin_all) - (xmax - xmin));
            long reminder;
            if (x_ost <= 0.0) Value = Math.DivRem(_scrollBarDist, 2, out reminder);
            else Value = (xmin - xmin_all) / x_ost * _scrollBarDist;
        }
    }

    public class TScrollBarY : TScrollBar
    {
        public TScrollBarY()
            : base()
        { 
            Orientation = Orientation.Vertical;
            VerticalAlignment = VerticalAlignment.Bottom;
        }

        // Set lef-upper corner of ScrollBarX
        public void SetScrollBarSizeAndPosition(ScreenLimits region)
        {
            Width = _scrollBarWidth;
            Height = region.v2 - region.v1 - _scrollBarWidth;
            Canvas.SetLeft(this, region.u2 - _scrollBarWidth);
            Canvas.SetTop(this, region.v1);
        }

        // Create the Scroll event handler. 
        //        private void ScrollBar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        // private void SetPositionForScrollBar(double ymin , double ymax )
        public void SetPosition(double ymin, double ymax, double ymin_all, double ymax_all)
        {
            // Display the new value in the label.
            string  Text = "VScrollBar Value:(OnScroll Event) " ;
            double y_ost = ((ymax_all - ymin_all) - (ymax - ymin));
            //repaintRegion = false;
            long reminder;
            if (y_ost <= 0.0) Value = Math.DivRem(_scrollBarDist, 2, out reminder);
            else Value = (ymin - ymin_all) / y_ost * _scrollBarDist;
        }

        private void button1_Click(Object sender,
                                  EventArgs e)
        {
            // Add 40 to the Value property if it will not exceed the Maximum value. 
            if (Value + 40 < Maximum)
            {
                Value = Value + 40;
            }
        }



    }
}
