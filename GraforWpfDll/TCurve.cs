using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;


namespace GraforWpfDll
{
    //---------------------------------------------------------
    //     Внешние переменные для страницы и области
    //---------------------------------------------------------
    public class TCurve
    {
        public int Id;
        public int _crvIdx;   //index of curve in Region CRV[] array. start from 0??

        //public Polyline polyline = new Polyline(); 
        // curve rendering
        public GraforMain.LinePattern pattern = GraforMain.LinePattern.Solid;
        private TSymbols symbols;
        public int width;
        public bool _bar = false; //draw bar (not line) for each point
        public int clr;    // curve color

        // curve data, current state, etc.
        public int crv_alloc;//=1,если выделена динамич. память под кривую// delete??
        public List<double> px, py, pxold, pyold;// math points
        public int u_old, v_old;//координаты предыдущей точки кривой (см.disp_point())
        public int jout_old;   //=1, если предыдущая точка кривой не попала в page
        public long nall; // all number of points allocated for px[] and py[]
        public int determ;   //=1, если кривая определена
        public int active;   //=1, если кривая на экране
        public int _activeCurve;// krivaya "active" ("selected",etc.) 

        public TCurve()
        { }

        public TCurve(int idx, double[] x, double[] y, int n)
        {
            AllocMemory(n);
            _crvIdx = idx;
            for (int i = 0; i < n; i++)
            {
                px[i] = x[i];
                py[i] = y[i];
            }
        }

        public TCurve(double[] x, double[] y, int n)
        {
            AllocMemory(n);
            for (int i = 0; i < n; i++)
            {
                px[i] = x[i];
                py[i] = y[i];
            }
        }

        public TCurve(double[] x, double[] y, int n, bool bar)
        {
            AllocMemory(n);
            for (int i = 0; i < n; i++)
            {
                px[i] = x[i];
                py[i] = y[i];
            }
            _bar = bar;
        }

        public TCurve(double[] x, double[] y, int n1, int n)
        {
            AllocMemory(n);
            for (int i = 0; i < n; i++)
            {
                px[i] = x[i + n1];
                py[i] = y[i + n1];
            }
        }

        public void SetDataToCurve(double[] x, double[] y, int n)
        {
            AllocMemory(n);
            for (int i = 0; i < n; i++)
            {
                px[i] = x[i];
                py[i] = y[i];
            }
        }
        public TCurve(List<Point> lPoints, bool bar = false)
        {
            var n = lPoints.Count;
            px = new List<double>();
            py = new List<double>();
            for (int i = 0; i < n; i++)
            {
                px.Add(lPoints[i].X);
                py.Add(lPoints[i].Y);
            }
            nall = n;
            _bar = bar;
        }

        public void SetDataToCurve(List<Point> lPoints)
        {
            var n = lPoints.Count;
            px = new List<double>();
            py = new List<double>();
            for (int i = 0; i < n; i++)
            {
                px.Add(lPoints[i].X);
                py.Add(lPoints[i].Y);
            }
            nall = n;
        }

        //----------------------------------------------------------------
        // alloc dynamic memory for curve ( nspec of double)
        // set crv_alloc, nall
        //---------------------------------------------------------------
        public int AllocMemory(int nspec)
        {
            //if (crv.crv_alloc == 1) one_crv_free(num_rg, num_crv);
            //if ((crv.px = (double*)calloc(nspec, sizeof(double))) == NULL) k == 0;
            //if ((crv.py = (double*)calloc(nspec, sizeof(double))) == NULL) k == 0;
            //            px = new double[nspec];
            //            py = new double[nspec];
            px = Enumerable.Repeat(0.0, nspec).ToList();
            py = Enumerable.Repeat(0.0, nspec).ToList();

            if (px == null || py == null)
            {
                MessageBox.Show(string.Format(" AllocMemory: Memory allocation error for curve Idx = {0} ; for points",
                    _crvIdx, nspec));
                return 1;
            }
            //init_crv:
            crv_alloc = 1;
            nall = nspec;
            return 0;
        }
        //---------------------------------------------------------------
        // Set curve parameters like color, width, etc.
        //---------------------------------------------------------------
        public int SetCurveType(int clr, //номер цвета кривой ( 0..15 )
                                  GraforMain.LinePattern type, //тип кривой(0-непрерыв.1-точки,2-штрих-пунктир.,3-пункт.)
                                  int width) //толщина кривой (1 - норм.толщина,3 - жирн.)
        {
            pxold = px;
            pyold = py;
            this.clr = clr;
            this.pattern = type;
            this.width = width;
            determ = 0;
            active = 0;
            crv_alloc = 0;
            u_old = 0;
            v_old = 0;
            jout_old = 1;
            return 1;
        }
    }// classs TCurve
}
