using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input; //for ExecutedRoutedEventArgs

namespace GraforWpfDll
{
    /// <summary>
    /// Interaction logic for GraforPage.xaml
    /// </summary>
    /// 
    // size for H and W
    public class SymbolSize
    {
        public double H, W;
    }
    public enum PageChangedReason
    {
        New = 0,
        ZoomChanged = 1,
        SizeCanvasChanged = 2,
        ColorRegionChanged = 3
    }

    public partial class GraforPage : UserControl //wpf page
    {
        public GraforMain gm;
        public int ToBitmap = 0;
        //public double _symh = 0, _symw = 0; // Symbol size on Grafor Page (for any region)

        // Window's window (GraforPageWindow) includes PageGrid. PageGgrid includes PageCanvas. PageCanvas is a GraforPage {PageWidth, PageHeight}

        //private double GraforPageWindowWidth, _graforPageWindowHeight; // Window Page size
        //private const double _defaultWindowWidth = 700, _defaultWindowHeight = 500; // Default Grafor Page size

        public int Scr_x, Scr_y,
            Bg_color, Ln_color,
            Scr_brd, Scr_brd_clr,
            Scr_fon_clr,
            Xasp, Yasp,
            Colbg_old, Col_old,
            Num_colors;

        //public string Axx_str = "X";
        public const int MAXREGIONCOUNT = 5; // max number of regions for one page 
        //        public GraforRegion[] graforRegions = new GraforRegion[MAXREGIONCOUNT];
        public List<GraforRegion> graforRegions = new List<GraforRegion>(); //[MAXREGIONCOUNT];
        public int _regionsCount = 0; // Number of allocated regions on the page. = 0 ... MAXREGIONCOUNT-1

        public static int NMAXNSTEP = 15; // максимальное количество делений осей//?? kogda bylo - byl gluk 10 gluk
        public static int NMAXCOLOR = 15; // максимальное количество делений осей

        public const double PI = 3.1415926535898;

        // like focus on some grafor's region
        public GraforRegion activeGraforRegion;

        //public double[] X_sp, Y_sp;
        public long N_sp;
        public int N_smooth_pnt; // количество точек усреднения
        // в сглаженном типе кривой
        public int J_act_rg; // Current Grafor window ??? 

        public void SetRegions(List<GraforRegion> graforRegions)
        {
            this.graforRegions = graforRegions;
        }

        // it is used as start and end for Zooming. It should be applyed for active page.
        private Shape rubberBand = null;
        private Point startPoint = new Point();
        private Point endPoint = new Point();

        public double[] Arrx_test = new double[3];
        public double[] Arry_test = new double[3];
        public static SymbolSize Sym = new SymbolSize();
        private TextBlock tmpTxtBlock = new TextBlock();

        public static List<UIElement> _UIElements = new List<UIElement>();
        public GraforMain.State PageState = GraforMain.State.Closed;
        // keeps track of all UIs added to canvas like: canvas.Children.Add(UIEl);

        //        public GraforPage() : this(_defaultWindowWidth, _defaultWindowHeight) { }
        //public GraforPage() : this(0, 0) { }

        public GraforPage()
        {
            //            Style = (Style)FindResource(typeof(Window));
            InitializeComponent();
            //this.Height = height;
            //this.Width = width;
            //if (SetPageSize(height,width) == 0)  PageState = GraforMain.State.Created;
        }

        ////
        //// InitPageForScheduleTestCases() creates layout for page to display results from "test cases for schedule"
        ////
        //public void SetPageStyleAndChartData(List<GraforRegion> lGraforRegions)
        //{
        //    //________________________________________ set region 1 __________________________________________________________________

        //    // Set empty regions: Allocate memory for region. Calculate and set left upper point and size of region 

        //    GraforRegion grRg1 = lGraforRegions[0];
        //    grRg1.Id = 0;
        //    grRg1._canvas = GetCanvas();
        //    graforRegions.Add(grRg1);

        //    // pu1_frm,..pv2_frm parts of page, allocated for region.
        //    // For example: 0,0,0.5,1.0 - region takes left part of page, 
        //    // i.e. left/upper = 0.0*PageWidth/0.0*PageHeight and right/lower = 0.5*PageWidth/1.0*PageHeight
        //    double pu1_frm = 0.0, pv1_frm = 0.0, pu2_frm = 1.0, pv2_frm = 0.5;
        //    grRg1.SetRegionSize(PageWidth, PageHeight, pu1_frm, pv1_frm, pu2_frm, pv2_frm);
        //    //grRg1.SetLimitsForRegion(0.0, 0.0, 1.0, 1.0); // it sets _xmin_chart,_ymin_chart,... and  _xmin,_ymin for Region



        //    // Set region parameters, which are not dependent on region size: color, text, etc.
        //    int aut = 1;
        //    double[] bt = new double[] { 0.05, 0.05, 0.2, 0.05 }; //

        //    int j_dnx = 1; //признак убывания (=-1) или возраст.(=+1) оси X
        //    //int ncrv, //количество кривых в данной области функции
        //    int fon = 12;// 10 - violet 11 - yellow 12 - blue; цвет фона области
        //    int brd = 1; // = 1...10 - меню с рамкой,0 - без рамки
        //    int brd_clr = 6; // цвет рамки области
        //    //int txtu = 0,  txtv = 0; //not used (координаты левого верхнего угла заголовка области)
        //    int txth = 1; //размер шрифта букв заголовка области
        //    int txt_clr = 15, txt_fon = 11; //цвет букв и фона заголовка области
        //    string txt = "Number of agents in groups:1 - red, 2 - green, 1,2 - black}."; //адрес cтроки названия области

        //    grRg1._curvesRegion.SetMathLimits(0.0, 0.0, 1.0, 1.0);
        //    grRg1.SetRegionStyle(bt, j_dnx,
        //                     fon, brd, brd_clr,
        //                     txth, txt_clr, txt_fon, txt);

        //    grRg1.InitLimitsByCurves();
        //    //            grRg1._curvesRegion.SetMaxLimitsByCurves(grRg1.Curves);
        //    //            grRg1._curvesRegion.SetMathLimits(grRg1._curvesRegion.maxLimits);

        //    //grRg1.SetChartLimits(grRg1._chartRegion.xmin, grRg1._chartRegion.ymin,
        //    //                        grRg1._chartRegion.xmax, grRg1._chartRegion.ymax);

        //    //------------------ set Axis X -----------------
        //    bool autoStep = true;
        //    bool axx_aut = true;
        //    double axx_y0 = 0.5;
        //    int axx_clr = 15;
        //    GraforMain.LinePattern grid_type = GraforMain.LinePattern.Dash1;
        //    double axx_step = 0; int axx_nsubstep = 5, axx_dec = 2;
        //    int axx_txtu = 20; int axx_txtv = 76; int axx_txth = 1;
        //    int axx_txt_clr = 15, axx_txt_fon = 11;
        //    string axx_txt = "Iteration";
        //    TAxisX axisx = new TAxisX();
        //    if (grRg1.AxisX.MakeAxisX(axx_aut, axx_y0, axx_clr, grid_type,
        //             autoStep, axx_step, axx_nsubstep, axx_dec,
        //             axx_txtu, axx_txtv, axx_txth, axx_txt_clr, axx_txt_fon,
        //                 axx_txt) == 0) return; // exit(1);


        //    //------------------ set Axis Y -----------------
        //    bool axy_aut = true;
        //    double axy_0 = 0.0;
        //    int axy_clr = 15;
        //    double axy_step = 0.0;// 5.0; 
        //    int axy_nsubstep = 5, axy_dec = 2;
        //    int axy_txtu = 420, axy_txtv = 133, axy_txth = 1;
        //    int axy_txt_clr = 15, axy_txt_fon = 12;
        //    string axy_txt = "N Agents";
        //    TAxisY axisy = new TAxisY();
        //    if (grRg1.AxisY.MakeAxisY(axy_aut, axy_0, axy_clr, grid_type,
        //             autoStep, axy_step, axy_nsubstep, axy_dec,
        //             axy_txtu, axy_txtv, axy_txth, axy_txt_clr, axy_txt_fon,
        //             axy_txt) == 0) return; //exit(1);
        //    //grRg1._curvesRegion.SetBothLimitsByCurves(grRg1.Curves);
        //    grRg1.DrawRegion();
        //    //for (int i = 0; i < _regionsCount; ++i)
        //    //        graforRegions[i].disp_rg_axes_crv();


        //    //________________________________________ set region 2 __________________________________________________________________

        //    // Empty region for test
        //    GraforRegion grRg2 = lGraforRegions[1];
        //    grRg2.Id = 1;
        //    grRg2._canvas = GetCanvas();
        //    graforRegions.Add(grRg2);

        //    // pu1_frm,..pv2_frm parts of page, allocated for region.
        //    // For example: 0.5,0.0,0.5,1.0 - region takes right part of page, 
        //    // i.e. left/upper = 0.5*PageWidth/0.0*PageHeight and right/lower = 0.95*PageWidth/1.0*PageHeight
        //    pu1_frm = 0.0; pv1_frm = 0.5; pu2_frm = 1.0; pv2_frm = 1.0;
        //    grRg2.SetRegionSize(PageWidth, PageHeight, pu1_frm, pv1_frm, pu2_frm, pv2_frm);

        //    // Set region parameters, which are not dependent on region size: color, text, etc.
        //    aut = 1;
        //    bt = new double[] { 0.05, 0.05, 0.05, 0.05 }; //
        //    //int fon2 = 7;////

        //    txt = "Service Level[%] for skill id 1-red, 2-green. 10*Cost(K) - blue";
        //    //" Black curve is a total number of calls for skill = 1    ";

        //    grRg2.SetRegionStyle(bt, j_dnx,
        //                     fon, brd, brd_clr,
        //                     txth, txt_clr, txt_fon, txt);

        //    //------------------ set 1st curve for the region -----------------

        //    //bt[] must be initialized
        //    grRg2._curvesRegion.SetMathLimits(0.0, 0.0, 1.0, 1.0);
        //    grRg2.InitLimitsByCurves();
        //    //------------------ set Axis X -----------------
        //    axx_txt = "Iteration";
        //    if (grRg2.AxisX.MakeAxisX(axx_aut, axx_y0, axx_clr, grid_type,
        //             autoStep, axx_step, axx_nsubstep, axx_dec,
        //             axx_txtu, axx_txtv, axx_txth, axx_txt_clr, axx_txt_fon,
        //                 axx_txt) == 0) return; // exit(1);
        //    //grRg2.AxisX = axisx2;

        //    //------------------ set Axis Y -----------------
        //    axy_txt = "SL, %/Cost(K)";
        //    if (grRg2.AxisY.MakeAxisY(axy_aut, axy_0, axy_clr, grid_type,
        //             autoStep, axy_step, axy_nsubstep, axy_dec,
        //             axy_txtu, axy_txtv, axy_txth, axy_txt_clr, axy_txt_fon,
        //             axy_txt) == 0) return; //exit(1);

        //    grRg2.DrawRegion();
        //}

        public Canvas GetCanvas()
        {
            return PageCanvas;
        }

        // When Grid (PageGrid) changes the size then Canvas (PageCanvas) will change it size too.
        // It will set new size for Canvas
        private void grdPage_SizeChanged(object sender, SizeChangedEventArgs e)
        { PageSizeChanged(); }

        public void PageSizeChanged()
        {
            Draw(PageChangedReason.SizeCanvasChanged);
        }
        public void Draw()
        {

            if (SetPageSize() == 0)
            {
                Draw(PageChangedReason.New);
            }
        }

        public void Draw(PageChangedReason pageChangedReason)
        {
            if (SetPageSize() == 0)
            {
                // If _UIElement (eg. TextBlock ) was drawing with  Canvas.SetZIndex(pl, m);  where m == 0 
                // then we don't need to remove it from canvas (when page is extended, for example) like tis:
                // RemovePageUIElements()
                //{
                // rg.AxisX._UIElements.ForEach(x => PageCanvas.Children.Remove(x));
                // rg.AxisX._UIElements.Clear();
                //}
                // If m!=0, then previous _UIElement will stay on canvas while page is extended and 
                // we have to delete it manually with RemovePageUIElements() method

                //RemovePageUIElements();

                switch (pageChangedReason)
                {
                    case PageChangedReason.New:
                        break;
                    case PageChangedReason.SizeCanvasChanged:
                        break;
                    case PageChangedReason.ColorRegionChanged:
                        break;
                }
                DrawRegions(pageChangedReason);
            }
        }

        private void DrawRegions(PageChangedReason pageChangedReason)
        {
            foreach (var grRg in graforRegions)
            {
                // Set rg.u1, rg.u2,.. Symh, Symw for each region based on Page Canvas (PageCanvas) size
                // and set previusly pu1_frm, pu2_frm, ...
                //        grRg.ChangeRegionSize(PageWidth, PageHeight, SymW, SymH);

                //                grRg.SetLimitsForRegion(grRg._chartRegion.xmin, grRg._chartRegion.ymin,
                //                                        grRg._chartRegion.xmax, grRg._chartRegion.ymax);
                switch (pageChangedReason)
                {
                    case PageChangedReason.SizeCanvasChanged:
                    case PageChangedReason.New:
                        grRg.CalculateRegionSizeAndPositionOnPage(PageWidth, PageHeight);
                        break;
                }
                grRg.DrawRegion(pageChangedReason);
            }
        }



        //private void ReDrawRegionsForNewSize()
        //{
        //    foreach (var grRg in graforRegions)
        //    {
        //        // Set rg.u1, rg.u2,.. Symh, Symw for each region based on Page Canvas (PageCanvas) size
        //        // and set previusly pu1_frm, pu2_frm, ...
        //        grRg.ChangeRegionSize(PageWidth, PageHeight, SymW, SymH);

        //        //                grRg.SetLimitsForRegion(grRg._chartRegion.xmin, grRg._chartRegion.ymin,
        //        //                                        grRg._chartRegion.xmax, grRg._chartRegion.ymax);
        //        //PageCanvas.Children.Add(grRg._scrollBarX);
        //        //PageCanvas.Children.Add(grRg._scrollBarY);
        //        grRg.DrawRegion(PageCanvas);
        //    }
        //}



        private void AddNewRegion(object sender, ExecutedRoutedEventArgs e)
        {
            AddNewRegion();
        }

        private void AddNewRegion()
        {
            int x = 5;
        }

        private double _pageWidth, _pageHeight; // Grafor Page size
        public double PageWidth
        {
            get { return _pageWidth; }
            set { _pageWidth = value; }
        }

        public double PageHeight
        {
            get { return _pageHeight; }
            set { _pageHeight = value; }
        }

        //        public int RegionsCount
        //        {
        //            get { return _regionsCount; }
        //            set { _regionsCount = value; }
        //        }

        //---------------------------------------------------------------
        // Set some sizes of Grafor page based on size of PageGrid (Grid Page)
        // It sets PageWidth, PageHeight, SymW, SymH, PageCanvas.Width, PageCanvas.Height
        //---------------------------------------------------------------
        public int SetPageSize()
        {
            // We use it just to init page. Real PageGrid, PageCanvas are not set here yet.
            // Set some fake parameters for Grafor page
            //            if (Math.Abs(PageGrid.ActualWidth) < 1.0 || Math.Abs(PageGrid.ActualHeight) < 1.0)

            if (PageGrid.ActualHeight < 1.0 || PageGrid.ActualWidth < 1.0)
            {
                PageHeight = PageGrid.ActualHeight;
                PageWidth = PageGrid.ActualWidth; //ctlGraforPage
                Sym.W = 10;
                Sym.H = 12;
                return 1;
            }

            PageHeight = PageGrid.ActualHeight;
            PageWidth = PageGrid.ActualWidth;
            //PageHeight = height;
            //PageWidth = width;

            // Pochemu-to  PageCanvas.Width vse ravno 0
            // V sootvetstvii s knigoi, nado ustanovit' width and height dlya PageCanvas
            //PageCanvas.Height = PageHeight;
            //PageCanvas.Width = PageWidth;

            //eu:??
            if (PageCanvas.Effect == null) //win.ToBitmap == 0)
            {
                Sym.W = (int)(GetStringSize("W").Width); //win.regionCanvas.t  TextWidth("W");
                Sym.H = (int)(GetStringSize("H").Height); // win.BoxForSp.Canvas.TextHeight("H");
            }
            else
            {
                //Symw = win.Bitmap.Canvas.TextWidth("W");
                //Symh = win.Bitmap.Canvas.TextHeight("H");
            }
            return 0;
        }
        public void InitLimitsByCurves()
        {
            foreach (var grRg in graforRegions)
            {
                grRg.InitLimitsByCurves();
            }
        }




        /*
        private TCurve GetCurvFunction1()
        {
              CurveSamples fft = new CurveSamples();
                  fft.CalculateFunctionAndSpectrum1();
            double[] x = new double[CurveSamples.DIMFUN + 1];
            double[] y = new double[CurveSamples.DIMFUN + 1];
            int i, n;
            n = CurveSamples.DIMFUN;
            for (i = 0; i <= n; i++)
            {
                x[i] = fft.Delt[i];
                y[i] = fft.Func[i];
            }
            return new TCurve(x, y, n);
        }
    */




        /*      private void DrawPage()
                {
                    //gm = new GraforMain();
           

            
                    // Create one region. If it exceeds MAXREGIONCOUNT then return.
                    //if ((J_act_rg = set_new_region(this)) == -1) return; ; // set empty region
                   // SetChartLimitsByCurves(J_act_rg); //tmp
                   // disp_rg_axes_crv(J_act_rg); //tmp
                    // set_region_parameters(J_act_rg); //??

                    //double width = pageCanvas.Width;
                    //double height = pageCanvas.Height;
                   // double ac_pageHeight = pageCanvas.ActualHeight;
                    for (int i = 0; i < MAXREGIONCOUNT; ++i)
                    {
                        // Set rg.u1, rg.u2,.. Symh, Symw for each region based on pageCanvas size
                        //if (RG[i] != null && RG[i].win_onscreen == 1)
                        if (RG[i] != null)
                        {
                            disp_rg_axes_crv(i);
                        }
                    }

                     // Create another region if need it
                     // Set next region for new curves
                    J_act_rg = set_new_region(this)
                    int num_crv = 0;
                    one_crv_alloc(J_act_rg, num_crv, n);//CRV[0].nall=n_sp;ncrv=ncrv!!!;
                    copy_arr_to_rgcrv(J_act_rg, num_crv, x, y, n);
                    SetChartLimitsByCurves(J_act_rg);
                    cs = new ChartStyleGridlines();
                    dc = new DataCollection();
                    ds = new DataSeries();
                    cs.pageCanvas = pageCanvas;
                    cs.TextCanvas = textCanvas;
                    cs.Title = "Sine and Cosine Chart";
                    cs.Xmin = 0;
                    cs.Xmax = 7;
                    cs.Ymin = -1.5;
                    cs.Ymax = 1.5;
                    cs.YTick = 0.5;
                    cs.GridlinePattern = ChartStyleGridlines.GridlinePatternEnum.Dot;
                    cs.GridlineColor = Brushes.Black;
                    cs.AddChartStyle(tbTitle, tbXLabel, tbYLabel);

                    // Draw Sine curve:
                    ds.LineColor = Brushes.Blue;
                    ds.LineThickness = 2;
                    for (int i = 0; i < 50; i++)
                    {
                        double x = i / 5.0;
                        double y = Math.Sin(x);
                        ds.LineSeries.Points.Add(new Point(x, y));
                    }
                    dc.DataList.Add(ds);

                    // Draw cosine curve:
                    ds = new DataSeries();
                    ds.LineColor = Brushes.Red;
                    ds.LinePattern = DataSeries.LinePatternEnum.DashDot;
                    ds.LineThickness = 2;
                    for (int i = 0; i < 50; i++)
                    {
                        double x = i / 5.0;
                        double y = Math.Cos(x);
                        ds.LineSeries.Points.Add(new Point(x, y));
                    }
                    dc.DataList.Add(ds);
                    dc.AddLines(cs);
      
                }
           */

        //---------------------------------------------------------------------------



        public void clear_rg_axes_crv()
        {
            //del_cur(num_rg);
            //clear_txtpnl();
            //clear_rg();
            // clear_axx();
            // clear_axy();
        }

        /*
                public void region(int num_rg) // == Init()
                {   //??? nobody calls region???
                    TRegion rg;
                    int u1,v1,u2,v2;

                    rg=RG[num_rg];
                    rg.u1= 0;
                    rg.v1= 0;
            
                    textCanvas.Width = chartGrid.ActualWidth;
                    textCanvas.Height = chartGrid.ActualHeight;
                    pageCanvas.Width = textCanvas.Width; // -20;//??? doesn't work at rg size  -> 0
                    pageCanvas.Height = textCanvas.Height; // -20; 
            
                    rg.u2 = (int)pageCanvas.Width - 50; // ActualWidth??
                    rg.v2 = (int)pageCanvas.Height - 50;
 
                    u1=rg.u1; v1=rg.v1;u2=rg.u2; v2=rg.v2;

                    draw_bar(u1, v1, u2, v2, rg.fon_clr);
                    Size str_size = GraforPage.GetStringSize(rg.txt);
                    if(rg.aut==1)
                    {
                        rg.txtu = u1 + (int)(0.5 * (double)(u2 - u1 - str_size.Width));
                        rg.txtv = v1+1;
                    }
                    draw_string_horizontal(_canvas, rg.txtu, rg.txtv, rg.txt, rg.txt_clr, rg.txt_fon);
            
            
                    Symw = (int) (str_size.Width / rg.txt.Length); // TextWidth("W");
                    Symh = (int) str_size.Height; // TextHeight("H");
            
                    //if (ToBitmap == 0)
                    //{
                    //    Symw = 8; // regionCanvas.TextWidth("W");
                    //    Symh = 10; // regionCanvas.TextHeight("H");
                    //}
                    //else
                    //{
                    //    Symw=Bitmap.Canvas.TextWidth("W");//Bitmap
                    //    Symh=Bitmap.Canvas.TextHeight("H");
                    //}

                   // nxl=5;nxs=3;
                   // nyl=7;nys=1;
                   // nxl=5;
                }
                */




        //---------------------------------------------------------------------------
        /*////
        public void del_rg_axes_crv(int num_rg)
        {
            TRegion rg;
            int i;
            
            //int u1,v1,u2,v2;
            //int num_crv=0;
            int  grid_clr,txt_clr,axx_clr,axx_txt_clr,axy_clr,axy_txt_clr;
            int  txt_fon;

            rg=RG[num_rg];
            int[] crv_color = new int[rg.NMAXCRV];

            grid_clr=rg.grid_clr;
            txt_clr=rg.txt_clr;
            //cur_clr=rg.cur_clr;
            txt_fon=rg.txt_fon;
            axx_clr=rg.AxisX.clr;
            axy_clr=rg.AxisY.clr;
            axx_txt_clr=rg.AxisX.txt_clr;
            axy_txt_clr=rg.AxisY.txt_clr;
            for(i=0;i<rg.ncrv;i++)
            crv_color[i]=rg.CRV[i].clr;

            rg.grid_clr=rg.fon_clr;
            rg.txt_clr=rg.fon_clr;
            //rg.cur_clr=rg.fon_clr;
            rg.txt_fon=rg.fon_clr;
            rg.AxisX.clr=rg.AxisX.txt_fon;
            rg.AxisY.clr=rg.AxisY.txt_fon;
            rg.AxisX.txt_clr=rg.AxisX.txt_fon;
            rg.AxisY.txt_clr=rg.AxisY.txt_fon;
            for(i=0;i<rg.ncrv;i++)
            rg.CRV[i].clr=rg.fon_clr;
            //disp_cur(num_rg);
            disp_region(num_rg);
            //DrawAxisX(num_rg);
            //DrawAxisY(num_rg);
            for(i=0;i<RG[num_rg].ncrv;i++)
                disp_crv(num_rg,i);

            rg.grid_clr=grid_clr;
            rg.txt_clr=txt_clr;
            //rg.cur_clr=cur_clr;
            rg.txt_fon=txt_fon;
            rg.AxisX.clr=axx_clr;
            rg.AxisY.clr=axy_clr;
            rg.AxisX.txt_clr=axx_txt_clr;
            rg.AxisY.txt_clr=axy_txt_clr;
            for(i=0;i<rg.ncrv;i++)
                rg.CRV[i].clr = i; // GraforMain.m_Colors[i]; // new Color(crv_color[i]);
        }
        */
        ////

        //---------------------------------------------------------------


        //---------------------------------------------------------------
        // New functhion to calculate string length in pxl coordinates
        //---------------------------------------------------------------
        public static Size GetStringSize1(string str)
        {
            Size size = new Size(0, 0);
            if (str.Length == 0) return size;
            TextBlock tb = new TextBlock();
            tb.FontSize = GraforPage.Sym.W;
            tb.Text = str;
            tb.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            size = (tb.DesiredSize); //keeps size  of string now
            //return new Size { Width = tb.Width*1.5, Height = tb.Height };
            return size;
        }


        public static Size GetStringSize(string text)
        {
            TextBlock tb = new TextBlock { Text = text, TextWrapping = TextWrapping.Wrap };
            var formattedText = new FormattedText(
                text,
                CultureInfo.CurrentUICulture,
                FlowDirection.RightToLeft,
                new Typeface(tb.FontFamily, tb.FontStyle, tb.FontWeight,
                    tb.FontStretch),
                tb.FontSize,
                Brushes.Black);

            return new Size(formattedText.Width * 1.5, formattedText.Height * 1.5);
        }




        /*
                //---------------------------------------------------------------
                            public void clear_txtpnl()
                //---------------------------------------------------------------
                {
                SpHandle.Canvas.Brush.Color=XAxis_fon;
                SpHandle.Canvas.FillRect(Rect
                    (0,0,SpHandle.ClientWidth,SpHandle.ClientHeight));
                }

                //---------------------------------------------------------------
                         public void clear_rg()
                //---------------------------------------------------------------
                {
                     BoxForSp.Canvas.Brush.Color=BoxForSp_fon;
                     BoxForSp.Canvas.FillRect(Rect
                            (0,0,BoxForSp.ClientWidth,BoxForSp.ClientHeight));
                }
        */



        /*        //---------------------------------------------------------------
                        public void draw_string_axx
                //---------------------------------------------------------------
                (int u1,int v1,  string s, int col_char, int col_fon)
                {
                int txt_l,u2,v2;
                int brd=0;

                XAxis.Canvas.Font.Color=col_char;
                txt_l=XAxis.Canvas.TextWidth(s);
                u2=u1+txt_l-1;
                v2=v1+Symh-1;
                //draw_bar_axx(u1-brd,v1-brd,u2+brd,v2+brd,col_fon);
                XAxis.Canvas.TextOut(u1,v1,s);
                }

                //---------------------------------------------------------------
                //---------------------------------------------------------------
                             public void draw_bar_axx
                //---------------------------------------------------------------
                (int u1,int v1,int u2,int v2, int color)
                //  Закрашивает прямоугольник с координатами u1,v1,u2,v2 цветом colr
                {
                    XAxis.Canvas.Brush.Color=color;//clBtnFace;
                    XAxis.Canvas.FillRect(Rect(u1,v1,u2,v2));
                }
 
                //---------------------------------------------------------------
                       public void clear_axx ()
                //---------------------------------------------------------------
                {
                    XAxis.Canvas.Brush.Color=XAxis_fon;
                    XAxis.Canvas.FillRect(Rect
                        (0,0,XAxis.ClientWidth,XAxis.ClientHeight));
                }

                //---------------------------------------------------------------
                public void draw_string_axy (int u1,int v1, string s , int col_char, int col_fon)
                {
                    int txt_l,u2,v2;
                    int brd=0;

                    YAxis.Canvas.Font.Color=col_char;
                    txt_l=YAxis.Canvas.TextWidth(s);
                    u2=u1+txt_l-1;
                    v2=v1+Symh-1;
                    //draw_bar_axy(u1-brd,v1-brd,u2+brd,v2+brd,col_fon);
                    YAxis.Canvas.TextOut(u1,v1,s);
               }

               //---------------------------------------------------------------
               public void draw_bar_axy(int u1, int v1, int u2, int v2, int color)
               {
                    YAxis.Canvas.Brush.Color=color;//clBtnFace;
                    YAxis.Canvas.FillRect(Rect(u1,v1,u2,v2));
                }

               //---------------------------------------------------------------
               public void clear_axy()
                {
                    YAxis.Canvas.Brush.Color=YAxis_fon;
                    YAxis.Canvas.FillRect(Rect
                        (0,0,YAxis.ClientWidth,YAxis.ClientHeight));
                }
                */

        private void Toolbar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            /*
             if (toolbar.IsSynchronizing) return;

            ComboBox source = e.OriginalSource as ComboBox;
            if (source == null) return;

            switch (source.Name)
            {
                case "fonts":
                    ctlMonthGrid.SetFontFamily((FontFamily)source.SelectedItem);
                    break;
                case "fontSize":
                    ctlMonthGrid.SetFontSize((double)source.SelectedItem);
                    break;
            }
             * 
             */
        }

        #region Logic for Custom Commands Binding

        private void HelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("For help, look at DublinCalendar.doc file in the root directory.", "Help!");
            //MonthGrid.GetBinCurrentDirectory();
        }

        private void CanHelpExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            //If e.CanExecute = false. Then Menu->Exit will be in shedow!!!
        }



        private void cmdBindingWordWrap_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //txbxQeen.TextWrapping = ((txbxQeen.TextWrapping == TextWrapping.NoWrap) ? TextWrapping.Wrap : TextWrapping.NoWrap);
        }


        /*
        void cmdBindingSaveChanges_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // it doesn't work for the last changes. it will bw calaled in Window_closing
            ctlMonthGrid.CopyDataFromGUIToDB(_monthData.CurrentYear, _monthData.CurrentMonth);
        }
        */


        #endregion

        // Command="ApplicationCommands.Open" 
        private void ReadDataFromCSVFile(object sender, ExecutedRoutedEventArgs e)
        {
            string[,] csvFileArr = null;
            int nRows = 0;
            /*
            if (_dBManager == null || _monthData == null) return;
            if (_CSVFileManager.ReadDataFromCSVFileToArr(out csvFileArr, out nRows) == false) return;//reading was cancelled
            _dBManager.CopyDataFromCSVFileArrToDB(csvFileArr, nRows);
            _dBManager.CopyDataFromDBToArr(_monthData.CurrentYear, _monthData.CurrentMonth, ctlMonthGrid.MonthNotes);
            ctlMonthGrid.GetPictureForMonth();
            ctlMonthGrid.CopyDataFromArrToViewModel();
            ctlMonthGrid.CreateChartGrid();
            status.Text = "Database loaded.";
             */
        }

        private void TextEditorToolbar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            if (toolbar.IsSynchronizing) return;

            ComboBox source = e.OriginalSource as ComboBox;
            if (source == null) return;

            switch (source.Name)
            {
                case "fonts":
                    ctlMonthGrid.SetFontFamily((FontFamily)source.SelectedItem);
                    break;
                case "fontSize":
                    ctlMonthGrid.SetFontSize((double)source.SelectedItem);
                    break;
            }
            */
            //body.Focus();
        }

        private void body_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //toolbar.SynchronizeWith(body.Selection);
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            PageState = GraforMain.State.Closed;
            //toolbar.Focus(); //try to execute binding
            /*
            ctlMonthGrid.CopyDataFromGUIToDB(_monthData.CurrentYear, _monthData.CurrentMonth);
            ctlMonthGrid.SaveSettings();
             */
            //working Binding TextBox-VM doesn't work
            /*
             MessageBoxResult result = MessageBox.Show("Do you really want to close application?", 
                                                       "Warning", 
                                                       MessageBoxButton.YesNo, 
                                                       MessageBoxImage.Question);
             if (result == MessageBoxResult.No)
             {
                 e.Cancel = true;
             }
             else
             {
                 //ctlMonthGrid.CopyDataFromGUIToDB(_monthData.CurrentYear, _monthData.CurrentMonth);
             }
              */
        }

        //Command="EditingCommands.ToggleBold" - execute
        private void SetFontWeight(object sender, ExecutedRoutedEventArgs e)
        {
            //ctlMonthGrid.SetFontWeight((bool)toolbar.boldButton.IsChecked);
        }

        private void SetFontStyle(object sender, ExecutedRoutedEventArgs e)
        {
            //ctlMonthGrid.SetFontStyle((bool)toolbar.italicButton.IsChecked);
        }

        private void SetFontDecoration(object sender, ExecutedRoutedEventArgs e)
        {
            //ctlMonthGrid.SetFontDecoration((bool)toolbar.underlineButton.IsChecked);
        }

        /*
        private void SynchronizeWith(object sender, ExecutedRoutedEventArgs e)
        {
            TextSelection ts = null;
            toolbar.SynchronizeWith(ts);
        }
        */


        //Command="ApplicationCommands.Save" - execute
        private void WriteDBToCSVFIle(object sender, ExecutedRoutedEventArgs e)
        {
            /*
            ctlMonthGrid.CopyDataFromGUIToDB(_monthData.CurrentYear, _monthData.CurrentMonth);
            _CSVFileManager.WriteDBToCSVFile(_dBManager);
             */
            //if (_CSVFileManager.SaveDocument())
            //    status.Text = "Database saved into CVS file.";
        }

        //Command="ApplicationCommands.Save" - Can execute
        private void WriteDBToCSVFIle_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //if (_dBManager == null || _monthData == null)
            //    e.CanExecute = false;
            //else 
            e.CanExecute = true;
        }

        private void cmdBindingReplace_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void cmdBindingReplace_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            /*
            ImageReplaceDialog dlg = new ImageReplaceDialog(ctlMonthGrid.Images, ctlMonthGrid.ImageLocations);
            bool? dialogResult = dlg.ShowDialog();
            // add 7 tmp images and strings to accept or cancel changes.
            if (dialogResult != null && (bool)dialogResult == true)
            {
                // Replace was clicked. Copy new images into original locations.
            }
            else
            {
                // Cancel was clicked. Don't copy images
            }
             */
        }

        // oneClick - region gets active
        // doubleClick - restore all curves limit
        // mouseCapture - made zoom ribbon
        private void PageCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                // remove active region if any
                foreach (var grRg in graforRegions)
                {
                    Point pnt = e.GetPosition(PageCanvas);
                    var region = grRg.screenLimits;
                    if (region.u1 < pnt.X && pnt.X < region.u2 && region.v1 < pnt.Y && pnt.Y < region.v2)
                    {
                        //if (grRg._activeRegion)
                        //{
                        //    grRg._activeRegion = false;
                        //    grRg._curvesRegion.SetMathLimits(grRg._curvesRegion.mathLimitsForAllCurves);
                        //}
                        //else
                        //{
                        foreach (var rg in graforRegions)
                            if (rg._activeRegion && rg._activeRegion != grRg._activeRegion)
                                rg._activeRegion = false;

                        grRg._activeRegion = true;
                        //}
                    }
                }
                Draw(PageChangedReason.ColorRegionChanged);
            }

            if (e.ClickCount == 2)
            {
                foreach (var grRg in graforRegions)
                {
                    Point pnt = e.GetPosition(PageCanvas);
                    var region = grRg.screenLimits;
                    if (region.u1 < pnt.X && pnt.X < region.u2 && region.v1 < pnt.Y && pnt.Y < region.v2)
                    {
                        grRg._curvesRegion.SetMathLimits(grRg._curvesRegion.mathLimitsForAllCurves);
                        //if (grRg._activeRegion)
                        //{
                        //    grRg._activeRegion = false;
                        //    grRg._curvesRegion.SetMathLimits(grRg._curvesRegion.mathLimitsForAllCurves);
                        //}
                        //else
                        //{
                        //    foreach (var rg in graforRegions)
                        //        if (rg._activeRegion && rg._activeRegion != grRg._activeRegion)
                        //            rg._activeRegion = false;

                        //    grRg._activeRegion = true;
                        //}
                    }
                }
                Draw(PageChangedReason.ZoomChanged);
            }

            if (!PageCanvas.IsMouseCaptured)
            {
                startPoint = e.GetPosition(PageCanvas);
                PageCanvas.CaptureMouse();
            }
        }

        private void PageCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (PageCanvas.IsMouseCaptured)
            {
                endPoint = e.GetPosition(PageCanvas);
                if (rubberBand == null)
                {
                    rubberBand = new Rectangle();
                    rubberBand.Stroke = Brushes.Red;
                    PageCanvas.Children.Add(rubberBand);
                }
                rubberBand.Width = Math.Abs(startPoint.X - endPoint.X);
                rubberBand.Height = Math.Abs(startPoint.Y - endPoint.Y);
                double left = Math.Min(startPoint.X, endPoint.X);
                double top = Math.Min(startPoint.Y, endPoint.Y);
                Canvas.SetLeft(rubberBand, left);
                Canvas.SetTop(rubberBand, top);
            }
        }


        // Draw zoom area in active region and set new limits for that region
        // If red rect goes to next region, then nado pererisovat' oba region, t.k. 
        // na sosednem ostanetsya "red rect". T.e. pererisovyvaem ves' page, for now.

        private void PageCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (rubberBand != null)
            {
                rubberBand = null;
                PageCanvas.ReleaseMouseCapture();
            }

            endPoint = e.GetPosition(PageCanvas);

            // we can not zoom chart more than 1000 times:
            double width = Math.Abs(startPoint.X - endPoint.X);
            double height = Math.Abs(startPoint.Y - endPoint.Y);
            if (width < 0.01 * _pageWidth || height < 0.01 * _pageHeight)
            {
                Draw(PageChangedReason.ZoomChanged);
                return;
            }

            activeGraforRegion = null;
            foreach (var rg in graforRegions)
                if (rg._activeRegion) activeGraforRegion = rg;
            if (activeGraforRegion == null)
            {
                Draw(PageChangedReason.ZoomChanged);
                return;
            }

            CurvesRegion curvesRegion = activeGraforRegion._curvesRegion;
            bool u_inChart = false;
            bool v_inChart = false;
            if (curvesRegion.screenLimits.u1 < startPoint.X && startPoint.X < curvesRegion.screenLimits.u2) u_inChart = true;
            if (curvesRegion.screenLimits.u1 < endPoint.X && endPoint.X < curvesRegion.screenLimits.u2) u_inChart = true;
            if (curvesRegion.screenLimits.v1 < startPoint.Y && startPoint.Y < curvesRegion.screenLimits.v2) v_inChart = true;
            if (curvesRegion.screenLimits.v1 < endPoint.Y && endPoint.Y < curvesRegion.screenLimits.v2) v_inChart = true;

            if (!u_inChart && !v_inChart)
            {
                Draw(PageChangedReason.ZoomChanged);
                return;
            }

            double umin = Math.Min(endPoint.X, startPoint.X);
            double umax = Math.Max(endPoint.X, startPoint.X);
            double vmin = Math.Min(endPoint.Y, startPoint.Y);
            double vmax = Math.Max(endPoint.Y, startPoint.Y);

            if (umin < curvesRegion.screenLimits.u1 || curvesRegion.screenLimits.u2 < umin) umin = curvesRegion.screenLimits.u1;
            if (umax < curvesRegion.screenLimits.u1 || curvesRegion.screenLimits.u2 < umax) umax = curvesRegion.screenLimits.u2;
            if (vmin < curvesRegion.screenLimits.v1 || curvesRegion.screenLimits.v2 < vmin) vmin = curvesRegion.screenLimits.v1;
            if (vmax < curvesRegion.screenLimits.v1 || curvesRegion.screenLimits.v2 < vmax) vmax = curvesRegion.screenLimits.v2;

            // Finally we got screenLimits for selected area (red frame). 

            ScreenLimits sl = new ScreenLimits(umin, vmin, umax, vmax);
            curvesRegion.SetMathLimits(activeGraforRegion.ConvertToMatLimits(sl, curvesRegion._alpha));
            //activeGraforRegion._curvesRegion.SetMathLimits(xmin, ymin, xmax, ymax);

            activeGraforRegion._repaintRegion = false;
            activeGraforRegion._scrollBarX.SetPosition(curvesRegion.mathLimits.xmin, curvesRegion.mathLimits.xmax,
                                                       curvesRegion.mathLimitsForAllCurves.xmin, curvesRegion.mathLimitsForAllCurves.xmax);
            activeGraforRegion._scrollBarY.SetPosition(curvesRegion.mathLimits.ymin, curvesRegion.mathLimits.ymax,
                                                       curvesRegion.mathLimitsForAllCurves.ymin, curvesRegion.mathLimitsForAllCurves.ymax);
            activeGraforRegion._repaintRegion = true;
            Draw(PageChangedReason.ZoomChanged);
        }

        private void PageCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void PageCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //// remove active region if any
            //foreach (var grRg in graforRegions)
            //{
            //    Point pnt = e.GetPosition(PageCanvas);
            //    var region = grRg.screenLimits;
            //    if (region.u1 < pnt.X && pnt.X < region.u2 && region.v1 < pnt.Y && pnt.Y < region.v2)
            //    {
            //        if (grRg._activeRegion)
            //        {
            //            grRg._activeRegion = false;
            //            grRg._curvesRegion.SetMathLimits(grRg._curvesRegion.mathLimitsForAllCurves);
            //        }
            //        else
            //        {
            //            foreach (var rg in graforRegions)
            //                if (rg._activeRegion && rg._activeRegion != grRg._activeRegion)
            //                    rg._activeRegion = false;

            //            grRg._activeRegion = true;
            //        }
            //    }
            //}

            //Draw(PageChangedReason.ZoomChanged);
        }

        //// it will redraw page if limits (curves) changed. ( What if page size was changed ???) 
        //public void ReDrawPage()
        //{
        //    _UIElements.ForEach(x => PageCanvas.Children.Remove(x));
        //    graforRegions.ForEach(x => x._repaintRegion = true);
        //    DrawRegions(PageChangedReason.CanvasSizeChanged);
        //}

        // it will remove all dorwing elements from the page.
        public void RemovePageUIElements()
        {
            foreach (var rg in graforRegions)
            {
                rg.AxisX._UIElements.ForEach(x => PageCanvas.Children.Remove(x));
                rg.AxisX._UIElements.Clear();

                rg.AxisY._UIElements.ForEach(x => PageCanvas.Children.Remove(x));
                rg.AxisY._UIElements.Clear();

                rg._UIElements.ForEach(x => PageCanvas.Children.Remove(x));
                rg._UIElements.Clear();
            }
            _UIElements.ForEach(x => PageCanvas.Children.Remove(x));
            _UIElements.Clear();

            // we can just remove everithing from canvas.
            //PageCanvas.Children.Clear();

            graforRegions.ForEach(x => x._repaintRegion = true);
        }

    }//partial class GraforPage
}
