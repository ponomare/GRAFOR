using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;


namespace GraforWpfDll
{
    // !!!!!!!!!!!!!!!!!!!
    //!!!!!!!!! TRegion dolzhen imet' CnvPage. Peredat' v constructore:
    // GraforRegion grRg1 = new GraforRegion(0); =>GraforRegion grRg1 = new GraforRegion(0, graforPage.GetCanvas());
        // Параметры области 
        // u,v - экранные (в pxl)
        // x,y - математические
        public class TRegionStyle
        {
            //int lgx,lgy; // =1, если вызывалась ylgax(),xlgax()
            //int lgy; //=1 если необхо\димо перевести ось Y в lg масштаб, 0- в лин.
            //int jlgy; //=1 если на экране lg ось Y, 0- если линейная
            public string title; //
            public bool _textLocationAuto = true;
            public double[] bt = new double[4];
            public int ncrv, naxx, naxy, aut;
            public int[] jcrv;// = new int[NMAXCRV];
            public int j_dnx;//j_dnx == 1  - возрастающая шкала X
            public double symw, symh; //??
            public int fon_clr, brd_clr;
            public int brd;
            public double txtu, txtv, txth; // was int
            public int txt_clr, txt_fon;
            public int cfg_disp;  // отображать линии конфигурации на график

            public readonly int NMAXCRV;
            public List<TCurve> Curves = new List<TCurve>();// = new TCurve[NMAXCRV];
            public int cur_clr = 14;// GraforMain.m_Brushes[14];// Brush color = Brushes.LightGray;
            
            public int u1_frm, v1_frm, u2_frm, v2_frm; //coordinates of region on page

            // pu1_frm,..pv2_frm parts of page, allocated for region.
            // For example: 0,0,0.5,1.0 - region takes left part of page, 
            // i.e. left/upper = 0.0*PageWidth/0.0*PageHeight and right/lower = 0.5*PageWidth/1.0*PageHeight
            public double pu1_frm, pv1_frm, pu2_frm, pv2_frm;
            
            public int frm_clr;
            public int win_onscreen; // параметры окна записаны в структуру RG[]
            public int spec_onscreen;//параметры спектра записаны в структуру RG[]
            public int j_autorange;  // если =1, то выбор minx,...maxy области функции "по кривым". 
            // Если =0, то minx,... maxy выбираются вручную . См region(num)


//            public TRegion()
//            {
//                int i = 0;
//                bt = new double[4];
//            }

        }
    }
