using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;// UserControl
using System.ComponentModel; //PropertyChangedEventArgs
using System.Windows.Documents;
using System.Windows.Input; //for ExecutedRoutedEventArgs
using GraforWpfDll.UserControls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows; // Size

// SetAxisXStyle    - Set all parameters not depending on axis location
// SetAxisXLocation - set all coordinates and limits for axis. locate area for axis drawing
// DrawAxisX        - draw axis. Before drawing we need to call SetAxisXLocation each time when size of region was changed

namespace GraforWpfDll
{
    public class TAxisX
    {
        // Axis Scale:
        public int _nstep, _nsubstep, _powx, _dec;
       
        //Axis Location
        // if _y0AutoLocation = true then Axis X goes on _ymin of region
        // if _y0AutoLocation = false then Axis X goes on _y0 and _y0 should be set. y0 is a math coordinates. 
        public bool _y0AutoLocation = true;// was j_aut
        public double _y0 = 0.0;

        bool _autoStep =true; // if true, then  aвтомат. выбор шага основного деления оси Y.
        double _step; // Ruchnoe zadanie шагa основного деления оси Y. autoStep must be set to false

        public int _clr = 7;
        public bool _grid = true;
        public int _gridClr = 14;
        public GraforMain.LinePattern _gridPattern = GraforMain.LinePattern.Dash1;
        public int _gridWidth = 1; // 
        public List<UIElement> _UIElements = new List<UIElement>();// keeps track of all UIs added to canvas like: canvas.Children.Add(UIEl);


        
        // Axis Title:
        public string _txt;// Axis title

        
        // if _titleAutoLocation = true then axis title is at the middle of the axis
        // if _titleAutoLocation = false then _txtu1, _txtv1 should be set. default is true.
        public bool _titleAutoLocation = true; 
        public double _txtu1, _txtv1, _txtu2, _txtv2, _txth;

        // Axis Style:
        public int _txt_clr, _txt_fon;
        public bool _ascScale = true;// Ascending axis
        //if _titleLowerAxis = true then AXis title AND scale numbers are lower than XAxis scale. default is true.
        public bool _titleLowerAxis = true;

        // Axis height in pxls.
        public double _height;

        //---------------------------------------------------------------
        // Запись параметров оси X в структуру AX[i]
        // Возвращает 0, если есть ошибка в создании структуры
        // Возвращает 1, если все нормально.
        //---------------------------------------------------------------
        public int MakeAxisX(bool aut,    //Признак автомат-го выбора параметров оси X(1-авт.,0-ручн.)
                             double y0,   //Математическая Y-координата оси X (при ручном выборе)
                             int clr,     //номер цвета для оси координат X ( 0..15 ):
                             GraforMain.LinePattern gridPattern,    //тип линии для сетки по X.
                             bool autoStep,  // if true, then  aвтомат. выбор шага основного деления оси Y.
                             double step,    // Ruchnoe zadanie шагa основного деления оси Y. autoStep must be set to false
                             int nsubstep,//число подделений основного деления оси X ( max - 12, -1 - автомат(5) )
                             int dec,        //число знаков после запятой в числах под делениями оси X (0,1,2,3,4)
                             double txtu,    //X - координата лев. верхнего угла текста названия оси X (272)
                             double txtv,    //Y - координата лев. верхнего угла текста названия оси X (272)
                             double txth,    //размер букв названия оси X ( 1..4 )
                             int txt_clr, //номер цвета для букв названия оси X ( 0..15 )
                             int txt_fon, //номер цвета для фона названия оси X ( 0..15 )
                             string txt)  //название оси X
                        
        {
            ///int i;
            ///int ut2,vt2; //координаты конца подписи оси
            ///int u1;
            ///int v1,u2,v2; //координаты region
            ///char str[80];


            ///int len;
            ///int num;
            //TRegion rg;
            //TAxisX axx;

            //if(num_rg>=N_rg) return 0;
            //else rg=RG[num_rg];
            //rg = RG[num_rg];
            ///u1=rg.u1;
            ///v1=rg.v1;u2=rg.u2;v2=rg.v2;
            // Вычисление количества символов в названии
            ///len=strlen(txt); if(len>80) {len=80;txt[79]='\0';}
            //Определение конечных координат текста названия оси X


            //ut2=txtu+len*Symw;
            //vt2=txtv+Symh;

            _y0AutoLocation = aut;
            _y0 = y0;
            _clr = clr;
            _gridPattern = gridPattern;
            _autoStep = autoStep;
            _step = step;
            _nsubstep = nsubstep;
            _dec = dec;
            _txtu1 = txtu;
            _txtv1 = txtv;
            //_txtu2=ut2;
            //_txtv2=vt2;
            _txth = txth;
            _txt_clr = txt_clr;
            _txt_fon = txt_fon;
            _txt = txt;
            return (1);
        }
        //--------------------------------------------------------
        // This method is a part of AxisX.DrawAxisY()
        // Calculate the height of axisX in pixels.
        // The width of AXisX is predefined by chartRegion
        //--------------------------------------------------------
        public void CalculateAxisXHeight()
        {
            _height = 3.0 * GraforPage.Sym.H;//ticks+numbers+Axis title.
        }


        //---------------------------------------------------------------
        public void DrawAxisX(Canvas canvas, MathToScreenMatrix alpha, CurvesRegion curves)
        //---------------------------------------------------------------
        {
            int i, j;
            double[] xax = new double[GraforPage.NMAXNSTEP + 1]; //- мат.коорд.точек разбиения оси X
            int hstp, hsub;
            int[] uax = new int[GraforPage.NMAXNSTEP + 1];//-physical coordinates of main tikcs for axis X
            double v0; // V coord of X axis
            //double dusub;????
            //double awork;????
            int dusub;
            int awork, nsubstep;
            double utext, vtext;
            double usmb, vsmb, uend;
            int nstep; //nstep+1 - число точек разбиения
            //int xxxx;
            double bwork;
            int pfp;//количество знаков до (pow_for_point) десятичной тчк.
            int psp;//количество знаков после (pow_past_point) десятичной тчк.
            int k_step;
            int lng;//количество символов в подписи делений, считая точку и знак.
            int nstepd2;

            //int cl_h,cl_w;
            int txt_lmax;
            string str_aw;
            //double x1, x2, rgszu, dxfun;
            //double axx_alp11, axx_alp12;
            //int u1,u2;

    

            // We need it from Region:

            //Canvas _canvas = canvas;
            //_UIElements.ForEach(x => canvas.Children.Remove(x)); //todo: Zachem stirat' vse elemeny s canvas?? what it is ?? it deletes all elements (dazhe dlya osi Y????)

            double alp11 = alpha.a11;
            double alp12 = alpha.a12;
            double alp21 = alpha.a21;
            double alp22 = alpha.a22;

//            double umin_chart = chart.xmin * alp11 + alp12;
//            double vmin_chart = chart.ymax * alp21 + alp22;//!!!vmin <- ymax
//            double umax_chart = chart.xmax * alp11 + alp12;
//            double vmax_chart = chart.ymin * alp21 + alp22;
            double rgSymbw = GraforPage.Sym.W;
            double rgSymbh = GraforPage.Sym.H;
            if (_height < 1)
            {
                MessageBox.Show("\n DrawAxisX: The axis X has zero height");//todo :it is not used now
                return;
            }
            //if (AxisCommon.axes_scale(xmin, xmax, xax, axx.aut, ref  axx._step, ref axx._nstep) == 1) return;

            //if (AxisCommon.axes_scale(chart.xmin, chart.xmax, xax, ref _step, ref _nstep) == 1) return;
            if (AxisCommon.axes_scale(curves.mathLimits.xmin, curves.mathLimits.xmax, xax, _autoStep, ref _step, ref _nstep) == 1) return;
            nstep = _nstep;
            //rg=RG[num_rg];// сбой указателя rg
            //высота "палочки" деления оси:

            // oбращение оси в случае убывания.
            if (!_ascScale)
            {
                //for(i=0;i<=nstep;i++) printf("\n xax[%d]=%f",i,xax[i]);
                nstepd2 = (int)((nstep + 2) / 2.0);
                for (i = 0; i < nstepd2; i++)
                {
                    bwork = xax[i];
                    xax[i] = xax[nstep - i];
                    xax[nstep - i] = bwork;
                }
                //for(i=0;i<=nstep;i++) printf("\n xax[%d]=%f",i,xax[i]);
                //getch();return;
            }

            //Перевод делений оси в пиксели.
            for (i = 0; i <= nstep; i++)
                uax[i] = (int)(xax[i] * alp11 + alp12);


            //v0 = f(v0) точка на оси Y через которую проходит ось X
            if (_y0AutoLocation == true)
                v0 = curves.screenLimits.v2; //v0 = _v2 - neverno!!!;
            else
                v0 = _y0 * alp21 + alp22;

            //Нарисовать ось X
            Line line_x = new Line();
            //AddLinePattern(line, ds); if line is dashed or . . .
            line_x.Stroke = GraforMain.m_Brushes[_clr]; // line color
            line_x.StrokeThickness = 1;
            //line.StrokeDashArray =  new DoubleCollection(new double[2] { 4, 3 });


            //XAxis.Pen.Color=axx.clr;
            //XAxis.Brush.Color=axx.txt_fon;
            //XAxis.Pen.Style=psSolid; //setlinestyle(0,0,axx.type);
            //cl_h=XAxis.Height;
            //cl_w=XAxis.Width;
            //draw_bar_axx(0,0,cl_w,cl_h,axx.txt_fon);
            line_x.X1 = curves.screenLimits.u1;
            line_x.Y1 = v0;
            line_x.X2 = curves.screenLimits.u2;
            line_x.Y2 = v0;
            canvas.Children.Add(line_x);
            _UIElements.Add(line_x);
            //can.MoveTo(rg.u1,v0);
            //can.LineTo(rg.u2,v0);

            //lng=lng 1;//можно убрать

            // Рисование сетки по оси.
            if (_grid)
            {
                //BoxForSp.Canvas.Pen.Style=psDot; //setlinestyle(1,0,1);
                //BoxForSp.Canvas.Pen.Color=rg.grid_clr; //setlinestyle(1,0,1);
                for (i = 1; i < nstep; i++)
                {
                    Line grid_line = new Line();
                    //AddLinePattern(line, ds); if line is dashed or . . .
                    grid_line.Stroke = GraforMain.m_Brushes[_gridClr]; // line color
                    //grid_line.Stroke = Brushes.Transparent;
                    grid_line.StrokeThickness = _gridWidth;
                    grid_line.StrokeDashArray = GraforMain.GetPattern(_gridPattern); 
                    grid_line.X1 = uax[i];
                    grid_line.Y1 = curves.screenLimits.v1;
                    grid_line.X2 = uax[i];
                    grid_line.Y2 = curves.screenLimits.v2;
                    canvas.Children.Add(grid_line);
                    _UIElements.Add(grid_line);
                }
            }

            // Рисование делений по оси X
            //BoxForSp.Canvas.Pen.Style=psSolid; //setlinestyle(0,0,axy.type);

            // Рисование делений по оси X
            hstp = (int)(rgSymbh / 2.0);
            Line tick;
            for (i = 0; i < nstep; ++i)
            {
                tick = new Line();
                tick.Stroke = GraforMain.m_Brushes[_clr];
                tick.X1 = uax[i];
                tick.Y1 = v0;
                tick.X2 = uax[i];
                tick.Y2 = v0 + hstp;
                canvas.Children.Add(tick);
                _UIElements.Add(tick);
                // can.MoveTo(uax[i],vax );
                // can.LineTo(uax[i],vax + hstp);
            }

            // Рисование подделений оси X. nsubstep - число подделений оси X
            hsub = (int)(hstp / 2);//высота "палочки" подделения оси
            if (_nsubstep == -1) nsubstep = 5; //by default 
            else nsubstep = _nsubstep; //число подделений оси X

            if (nsubstep != 0)
            {
                dusub = (int)((uax[2] - uax[1]) / nsubstep);
                awork = uax[1];
                // Рисование подделений по оси X до первого основного деления
                Line sub_tick;
                while (awork > uax[0])
                {
                    sub_tick = new Line();
                    sub_tick.Stroke = GraforMain.m_Brushes[_clr];
                    sub_tick.X1 = awork;
                    sub_tick.Y1 = v0;
                    sub_tick.X2 = awork;
                    sub_tick.Y2 = v0 + hsub;
                    canvas.Children.Add(sub_tick);
                    _UIElements.Add(sub_tick); 

                    // can.MoveTo(awork, vax);
                    // can.LineTo(awork,vax+hsub);
                    awork = awork - dusub;
                }

                // Рисование подделений по оси X после первого основного деления до конца оси.  
                for (i = 1; ; i++)
                {
                    for (j = 1; j < nsubstep; j++)
                    {
                        //eu??
                        awork = uax[i] + j * dusub;
                        if (awork >= uax[nstep]) goto l1;
                        sub_tick = new Line();
                        sub_tick.Stroke = GraforMain.m_Brushes[_clr];
                        sub_tick.X1 = awork;
                        sub_tick.Y1 = v0;
                        sub_tick.X2 = awork;
                        sub_tick.Y2 = v0 + hsub;
                        canvas.Children.Add(sub_tick);
                        _UIElements.Add(sub_tick); 

                        //can.MoveTo(awork,vax);
                        //can.LineTo(awork,vax+hsub);
                    }
                }
            }
        l1: ;

        AxisCommon.clc_length(out pfp, out psp, out lng, out k_step, xax, uax, nstep, rgSymbw);
        uend = curves.screenLimits.u1;

            // draw numbers for X axis
            for (i = 1; i < nstep - 1; i += k_step)
            {
                //sprintf(str_w,"%-14.4f",xax[i]);
                //zero_del(str_w);
                //str_aw=(AnsiString)(str_w);
                str_aw = xax[i].ToString();
                txt_lmax = (int)(GraforPage.GetStringSize(str_aw)).Width;
                vsmb = v0 + 2 + hstp;
                usmb = uax[i] - (int)(0.5 * txt_lmax);
                if (usmb <= (uend + 3)) continue; // число "налезает"
                //draw_string_axx(usmb,vsmb,str_aw,axx.clr,axx.txt_fon);
                GraforPage.draw_string_horizontal(canvas, usmb, vsmb, str_aw, _clr, _txt_fon);
                uend = usmb + txt_lmax;
            }

            // Вывод надписи оси X
            txt_lmax = (int)(GraforPage.GetStringSize(_txt)).Width;
            utext = _txtu1;
            if (_titleAutoLocation)
            {
                utext = curves.screenLimits.u2 - txt_lmax - 5;
            }

            vtext = _txtv1;
            if (_titleAutoLocation)
            {
                if (_titleLowerAxis)
                    vtext = v0 + rgSymbh + 2;
                else
                    vtext = v0 - rgSymbh + 2;
            }

            GraforPage.draw_string_horizontal(canvas, utext, vtext, _txt, _txt_clr, _txt_fon);

        }



    }

    public class TAxisY
    {
        // Axis Scale:
        public int _nstep, _nsubstep, _powx, _dec;

        // if _x0AutoLocation = true then Axis Y goes on _xmin of region
        // if _x0AutoLocation = false then Axis X goes on _x0. _x0 should be set. x0 is a math coordinates. 
        public bool _x0AutoLocation = true;// was j_aut
        public double _x0 = 0.0;

        bool _autoStep = true; // if true, then  aвтомат. выбор шага основного деления оси Y.
        double _step; // Ruchnoe zadanie шагa основного деления оси Y. autoStep must be set to false

        public int _clr = 7;
        public bool _grid = true;
        public int _gridClr = 14;
        public GraforMain.LinePattern _gridPattern = GraforMain.LinePattern.Dash1;
        public int _gridWidth = 1;


        // Axis Title:
        public string _txt = "Def";// Axis title

        //if _titleLeftAxis = true, then Axis title AND scale numbers are on the left of XAxis scale. default is true.
        public bool _titleLeftAxis = true;

        // if _titleAutoLocation = true then axis title is at the middle of the axis
        // if _titleAutoLocation = false then _txtu1, _txtv1 should be set. 
        public bool _titleAutoLocation = true;
        public double _txtu1, _txtv1, _txtu2, _txtv2, _txth;

        // Axis Style:
        public int _txt_clr, _txt_fon;
        //public bool _ascScale = true;// Ascending axis

        // Axis + tics + numbers area width in pxls.
        // Shirina osi ochen' vazhnaya vesh'.
        // ee vychislyaut dlya MaxLimits. Potom ona postoyanna. 
        // Esli naprimer zoomed diapazon and prokruchivaem scrollBar, togda polozhenie osi ne dolzhno izmenyatsya.
        public double _width;
        public List<UIElement> _UIElements = new List<UIElement>();// keeps track of all UIs added to canvas like: canvas.Children.Add(UIEl);


        //-----------------------------------------------------------------------------------------
        // Запись параметров оси Y в структуру AxisY[i]
        // Возвращает 0, если есть ошибка в создании структуры
        // Возвращает 1, если все нормально.
        //-----------------------------------------------------------------------------------------
        public int MakeAxisY(bool aut,   //Признак автомат-го выбора параметров оси Y (1-авт.,0-ручн.)
                             double x0,  //атематическая X-координата оси Y (при ручном выборе)
                             int clr,    //номер цвета для оси координат Y ( 0..15 )
                             GraforMain.LinePattern gridPattern,  //тип линии для сетки по Y. 
                             bool autoStep, // if true, then  aвтомат. выбор шага основного деления оси Y.
                             double step, // Ruchnoe zadanie шагa основного деления оси Y. autoStep must be set to false
                             int nsubstep,//число подделений основного деления оси X ( max - 12, -1 - автомат(5) )
                             int dec,     //число знаков после запятой в числах под делениями оси Y (0,1,2,3,4)
                             double txtu,    //X - координата лев. верхнего угла текста названия оси Y (272)
                             double txtv,    //Y - координата лев. верхнего угла текста названия оси Y (272)
                             double txth,    //размер букв названия оси Y ( 1..4 )
                             int txt_clr, //номер цвета для букв названия оси Y ( 0..15 )
                             int txt_fon, //номер цвета для фона названия оси Y ( 0..15 )
                             string txt)  //название оси Y
                       
        {
            ///int i;
            ///int ut2,vt2; //координаты конца подписи оси
            ///int u1,v1,u2,v2; //координаты region
            ///char str[80];
            ///int len;
            ///int num;

            // RG[rg_num].u1 - область графика
            // u1 - область рисования кривых (внутри области графика)

            //if(num_rg>=N_rg) return 0;
            //else rg=RG[num_rg];
            //rg = RG[num_rg];
            ///u1=rg.u1;v1=rg.v1;u2=rg.u2;v2=rg.v2;

            // ычисление количества символов в названии
            ///len=strlen(txt); if(len>80) {len=80;txt[79]='\0';}
            //пределение конечных координат текста названия оси Y
            //ut2=txtu+len*Symw;
            //vt2=txtv+Symh;



            _x0AutoLocation = aut;
            _x0 = x0;
            _clr = clr;
            _gridPattern = gridPattern;
            _autoStep = autoStep;
            _step = step;
            _nsubstep = nsubstep;
            _dec = dec;
            _txtu1 = txtu;
            _txtv1 = txtv;
            //txtu2=ut2;
            //txtv2=vt2;
            _txth = txth;
            _txt_clr = txt_clr;
            _txt_fon = txt_fon;
            _txt = txt;
            return (1);
        }

        //--------------------------------------------------------
        // Calculate the width of axisY in pixels.
        // The height of AXisY is predefined by CurvesRegion
        //--------------------------------------------------------
        public int CalculateAxisYWidth(MathLimits limits)
        {
            //todo: pochemu ne vhodit ScreenLimits. Naprimer , esli dlinnaya os',
            //todo: to budut 0.1, 0.15, 0.2, a esli korotkaya , to tol'ko 0.1, 0.2.

            // _width 
            int i, j;
            double[] yax = new double[GraforPage.NMAXNSTEP + 1]; //- мат.коорд.точек разбиения оси Y
            int[] vax = new int[GraforPage.NMAXNSTEP + 1];
            double u0; // U coord of X axis
            //double ymin, ymax;
            int awork;
            string str_aw;
            int txt_lmax;

            // init values
            int nstep = 0;  //nstep+1 - число точек разбиения
            double step = 0.0;
            // get step and nstep
            if (AxisCommon.axes_scale(limits.ymin, limits.ymax, yax, _autoStep, ref step, ref nstep) == 1) return 1;

            int symW = (int)(GraforPage.Sym.W);

            // get maximum length (txt_lmax ) of numbers for Y axis (0.1, 1000, etc)
            // get initial value for txt_lmax
            str_aw = yax[1].ToString(); // 
            txt_lmax = (int)(GraforPage.GetStringSize(str_aw)).Width;

            for (i = 1; i < nstep - 1; i++)
            {
                str_aw = yax[1].ToString();
                awork = (int)(GraforPage.GetStringSize(str_aw)).Width;
                if (txt_lmax < awork) txt_lmax = awork;
            }

            double dusmb = 2 + symW + txt_lmax + symW+ 2; // width for numbers:2 + Axis title (vertical ~ 1 symW) + numbers + 2+ ticks(~1 symW)
            _width = dusmb;
            if (_titleAutoLocation) // vertical title for auto location 
            {
                //if horizontal title
                // It doesn't work for horizontal title as Axis soesn't have title at that time!!!!!
                //txt_lmax = (int)(GraforPage.GetStringSize(_txt)).Width; // title size
                //utext = u0 - txt_lmax - 5; // title left coordinate
                //vtext = vmin_chart + 2;

                //if vertical title
                _width = dusmb;// + (int)(GraforPage.GetStringSize(_txt)).Height + 5;// width for numbers plus title
                //vtext = vmin_chart + (int)((vmax_chart - vmin_chart - (int)(GraforPage.GetStringSize(_txt).Width)) / 2.0) + 5;
            }
            if (_width > 10.0 * symW) _width = 10.0 * symW;// to prevent oversize for axisY
            if (_width < 4.0 * symW) _width = 4.0 * symW;// to prevent oversize for axisY
            return 0;
        }


        //---------------------------------------------------------------
        public void DrawAxisY(Canvas canvas, MathToScreenMatrix alpha, CurvesRegion curves)
        //---------------------------------------------------------------
        {
            int i, j;
            double[] xax = new double[GraforPage.NMAXNSTEP + 1]; //- мат.коорд.точек разбиения оси Y
            int hstp, hsub;
            int[] vax = new int[GraforPage.NMAXNSTEP + 1];
            double u0; // U coord of X axis
            int dusub;
            int nsubstep;
            double utext, vtext;
            int nstep;  
            //nstep+1 - число точек разбиения
            //int xxxx;
            //float bwork,xsr;
            // for AxisX only:
            //int pfp;//Количество знаков до (pow_for_point) десятичной тчк.
            //int psp;//Количество знаков после (pow_past_point) десятичной тчк.
            //int k_step;
            int lng;//кол-во символов в подписи делений,считая точку и нуль перед ним .
            //int nstepd2;
            string str_w;
            //int cl_h,cl_w;
            //double ymin, ymax;
            string str_aw;

            // We need it from Region:
            //Canvas _canvas = canvas;
            if (_width < 1)
            {
                MessageBox.Show("\n DrawAxisY: The axisY has zero width");
                return ;
            }
            //_UIElements.ForEach(x => canvas.Children.Remove(x)); //todo: Zachem stirat' vse elemeny s canvas?? what it is ?? it deletes all elements (dazhe dlya osi X????)
            double alp11 = alpha.a11;
            double alp12 = alpha.a12;
            double alp21 = alpha.a21;
            double alp22 = alpha.a22;

//            double umin_chart = chart.xmin * alp11 + alp12;
//            double vmin_chart = chart.ymax * alp21 + alp22;//!!!vmin <- ymax
//            double umax_chart = chart.xmax * alp11 + alp12;
//            double vmax_chart = chart.ymin * alp21 + alp22;

            //Вычисление делений шкалы
            if (AxisCommon.axes_scale(curves.mathLimits.ymin, curves.mathLimits.ymax, xax, _autoStep, ref _step, ref _nstep) == 1) return;
            nstep = _nstep;

            var drawLimits = curves.screenLimits;
            //Перевод делений оси в пиксели.
            for (i = 0; i <= nstep; i++)
                vax[i] = (int)(xax[i] * alp21 + alp22);

            //u0 = f(x0) точка на оси Х через которую проходит ось Y
            //v0 = f(v0) точка на оси Y через которую проходит ось X
            if (_x0AutoLocation == true)
                u0 = drawLimits.u1; 
            else
                u0 = _x0 * alp11 + alp12;

            //Нарисовать ось Y
            Line line_y = new Line();
            //AddLinePattern(line, ds); if line is dashed or . . .
            line_y.Stroke = GraforMain.m_Brushes[_clr]; // line color
            line_y.StrokeThickness = 1;
            line_y.X1 = u0;
            line_y.Y1 = drawLimits.v1;
            line_y.X2 = u0;
            line_y.Y2 = drawLimits.v2;
            canvas.Children.Add(line_y);
            _UIElements.Add(line_y); 

            // Рисование сетки по оси Y.
            if (_grid)
            {
                //if (ToBitmap==0) 
                //{. . .}
                for (i = 1; i < nstep; i++)
                {
                    //if (ToBitmap==0)
                    //{ . . .}
                    Line grid_line = new Line();
                    //AddLinePattern(line, ds); if line is dashed or . . .
                    grid_line.Stroke = GraforMain.m_Brushes[_gridClr]; // line color
                    grid_line.StrokeThickness = _gridWidth;
                    grid_line.StrokeDashArray = GraforMain.GetPattern(_gridPattern); //= new DoubleCollection(new double[2] { 4, 3 }); //{ 1, 2 }
                    grid_line.X1 = drawLimits.u1;
                    grid_line.Y1 = vax[i];
                    grid_line.X2 = drawLimits.u2;
                    grid_line.Y2 = vax[i];
                    canvas.Children.Add(grid_line);
                    _UIElements.Add(grid_line);
                    //_cnvRegion.Children.Add(grid_line);
                    // BoxForSp.Canvas.MoveTo(rg.u1,vax[i]);
                    // BoxForSp.Canvas.LineTo(rg.u2,vax[i]);
                }
            }

            // Рисование делений по оси Y
            //if (ToBitmap==0) BoxForSp.Canvas.Pen.Style=psSolid; //setlinestyle(1,0,1);
            //else    Bitmap.Canvas.Pen.Style=psSolid; //Bitmap//setlinestyle(1,0,1);
            int symW = (int)(GraforPage.Sym.W); 
            Line tick;
            for (i = 0; i < nstep; ++i)
            {
                tick = new Line();
                tick.Stroke = GraforMain.m_Brushes[_clr];
                tick.X1 = u0;
                tick.Y1 = vax[i];
                tick.X2 = u0 - symW; //u0 - symW; //gluk
                tick.Y2 = vax[i];
                canvas.Children.Add(tick);
                _UIElements.Add(tick); 
                // can.MoveTo(u0,vax[i]);
                // can.LineTo(u0-hstp,vax[i]);
            }

            // Рисование подделений оси Y. nsubstep - число подделений оси Y
            hsub = (int)(symW / 2);//длина "палочки" подделения оси
            double awork;
            if (_nsubstep == -1) nsubstep = 5;
            else nsubstep = _nsubstep;
            if (nsubstep != 0)
            {

                dusub = (int)((vax[2] - vax[1]) / nsubstep);
                awork = vax[1];
                // Рисование подделений по оси Y до первого основного деления
                Line sub_tick;
                while (dusub != 0 && awork < vax[0])// error if no check for "dusub != 0 "
                {
                    sub_tick = new Line();
                    sub_tick.Stroke = GraforMain.m_Brushes[_clr];
                    sub_tick.X1 = u0;
                    sub_tick.Y1 = awork;
                    sub_tick.X2 = u0 - hsub;//u0 - hsub
                    sub_tick.Y2 = awork;
                    canvas.Children.Add(sub_tick);
                    _UIElements.Add(sub_tick); 

                    // can.MoveTo(u0,awork);
                    // can.LineTo(u0-hsub,awork);
                    awork = awork - dusub;
                }

                // Рисование подделений по оси Y после первого основного деления до конца оси.
                for (i = 1; ; i++)
                {
                    for (j = 1; j < nsubstep; j++)
                    {
                        awork = vax[i] + j * dusub;
                        if (awork <= vax[nstep]) goto l1;
                        sub_tick = new Line();
                        sub_tick.Stroke = GraforMain.m_Brushes[_clr];
                        sub_tick.X1 = u0;
                        sub_tick.Y1 = awork;
                        sub_tick.X2 = u0 - hsub;
                        sub_tick.Y2 = awork;
                        canvas.Children.Add(sub_tick);
                        _UIElements.Add(sub_tick); 
                        //can.MoveTo(uax,awork);
                        //can.LineTo(uax-hsub,awork);
                    }
                }
            }
        l1: ;

            // for XAxix we call clc_length(out pfp, out psp, out lng, out k_step, xax, uax, nstep);
            // uend = rg.u1;

            
            //
            // get maximum length of digits for axis Y (0.001, 0.1 or ..)
            // It is already calculated in this._width.
            //

            // get initial value for txt_lmax
//            double txt_lmax;// maximum number of pixels in digits
//            str_aw = xax[1].ToString(); // 
//            txt_lmax = (GraforPage.GetStringSize(str_aw)).Width;
//            
//            // get max length for chars
//            for (i = 1; i < nstep - 1; i++)
//            {
//                str_aw = xax[1].ToString();
//                awork = (int)(GraforPage.GetStringSize(str_aw)).Width;
//                if (txt_lmax < awork) txt_lmax = awork;
//            }

            //
            // draw numbers for Y axis
            //
            int symH = (int)(GraforPage.Sym.H); 
            double uFirstDigit, vFirstDigit;
            uFirstDigit = u0 - _width + 2 + symH + 2; 
            for (i = 1; i < nstep - 1; i++)
            {
                str_aw = xax[i].ToString();
                //sprintf(str_w,"%-14.4f",xax[i]);
                //zero_del(str_w);
                //str_aw=(AnsiString)(str_w);
                //lng=strlen(str_w);
                vFirstDigit = vax[i] - 5.0;// + 1.0;// 1 pixel below scale
                var symMinus = (GraforPage.GetStringSize("-")).Width;
                if (xax[i] < 0.0)
                    GraforPage.draw_string_horizontal(canvas, uFirstDigit - symMinus, vFirstDigit, str_aw, _clr, _txt_fon);
                else
                    GraforPage.draw_string_horizontal(canvas, uFirstDigit, vFirstDigit, str_aw, _clr, _txt_fon);
                //GraforPage.draw_string_vertical(canvas, usmb, vsmb, str_aw, _clr, _txt_fon);
                //gprintf(&usmb,&vsmb,"%s",str_w);
            }


            // Вывод надписи оси Y 
            if (_titleAutoLocation)
            {
                utext = u0 - _width + 2; 
                // todo: fix it
                //vtext = vmin_chart + (int)((vmax_chart - vmin_chart - (int)(GraforPage.GetStringSize(_txt).Width)) / 2.0) + 5;
                //if (vtext < 0) 
                vtext = drawLimits.v1 + ((GraforPage.GetStringSize(_txt)).Width) + 2.0 * GraforPage.Sym.W;
                //vtext = vmax_chart  + 5; //for test
                
                
                //utext = u0 - txt_lmax - 5;
                //vtext = rg.v1 + 2;
            }
            else
            {
                utext = _txtu1;
                vtext = _txtv1;
            }
            //GraforPage.draw_string_horizontal(canvas, utext, vtext, _txt, _txt_clr, _txt_fon);
            GraforPage.draw_string_vertical(canvas, utext, vtext, _txt, _txt_clr, _txt_fon);
        }


    }// class AxisY


    public class AxisCommon
    {

        //---------------------------------------------------------- ----
        // If input parameter step == 0 then step is calculated automatically ~ 0.5
        //---------------------------------------------------------- ----
        static public int axes_scale(double x1, double x2,
                        double[] xax,//xax[] физические координаты точек разбиения оси
                        bool autoStep, 
                        ref double pstep, //* pstep,//адрес для шага основного деления if autoStep = false
                        ref int pnstep)   // а* pnstep адрес для nstep
        {
            ///float k;
            ///float xsr;
            double dx, dx10, xmax, xmin;
            double[] xax10 = new double[20];
            double x110, x210, step;
            int i;
            int a;
            ///int powx;
            int powdx;
            int nstep;
            double a10int;
            double a10, a10new, a10old, h10;      //шаг оси
            ///float dmax,dmax10;

            step = pstep;
            if (x1 == x2)
            {
                x1 = 0.8 * x1; x2 = 1.2 * x2;
                //getch();
            }

            xmax = (x2 > x1) ? x2 : x1;
            xmin = (x2 < x1) ? x2 : x1;
            dx = (xmax - xmin);
            clc_pow(dx, out powdx);
            dx10 = dx * Math.Pow(10.0, powdx);

            //if (aut == 1)
            if (autoStep)
            {//Автоматичеое определение оптималного размера деления отреза
                h10 = 1.0;
                if (dx10 < 4.0) h10 = 0.5;
                if (dx10 < 2.0) h10 = 0.25;
            }
            else
            {
                h10 = step * Math.Pow(10.0, powdx);
                if (h10 == 0) h10 = 0.5;
            }

            x110 = xmin * Math.Pow(10.0, powdx);
            x210 = xmax * Math.Pow(10.0, powdx);
            xax10[0] = x110;

            ///lstep:;
            a10int = (int)x110;  //(float)floor(x110);
            a10old = a10int;
            a10new = a10int;
            a = (a10int >= x110) ? -1 : +1;
            for (i = 1; ; i++)
            {
                if ((a10new - xax10[0]) * (a10old - xax10[0]) <= 0) goto l10;//just a break
                a10old = a10new;
                a10new = a10old + a * h10;
            }
        l10: ;
            a10 = (a10old > a10new) ? a10old : a10new;
            if (a10old == a10new) a10 = a10old + h10;


            // Запись в нормированый (xax10[]) массив делений диапазона
            nstep = 0;
            for (i = 1; ; i++)
            {
                xax10[i] = a10 + h10 * (i - 1);
                nstep = nstep + 1;
                if (xax10[i] > x210) goto l1;
                if (nstep > GraforPage.NMAXNSTEP)
                {
                    MessageBox.Show("\nThe axis has too many divisions");//Слишком мелкий шаг разбиения оси
                    // printf("\ngetch();x1=%f, x2=%f",x1,x2 );
                    //getch();
                    //   *pnstep=nstep;
                    return 1;
                    //   h10=2*h10;goto lstep;
                }
            }
        l1: ;
            //   printf("\nx1=%f, x2=%f",x1,x2);
            xax10[nstep] = x210;

            // Запись в реальный (xax[]) массив делений диапазона
            for (i = 0; i <= nstep; i++)
            {
                xax[i] = xax10[i] * Math.Pow(10.0, -powdx);
                //printf("\nxax[%d]=%f",i,xax[i]);

            }
            pnstep = nstep;
            pstep = h10 * Math.Pow(10.0, -powdx);//???????????????
            return 0;
        }

        //---------------------------------------------------------------
        static void clc_pow(double x, out int powx)
        //  рассчитывает степень *powx для ОЛОЖИТЛЬОГО числа 'x'.
        // powx равно такому числу, чтобы 1 <= x*pow10(powx) <= 10
        // Т.е. x=100, powx=-2; x=0.03, powx= 1.
        {
            double k, dx10;
            int a = 0;

            if (x == 0.0) { powx = 1; return; }
            k = 1.0;
            powx = 0;

            dx10 = x;
            if (dx10 <= 1.0)
            { k = 10.0; a = +1; }
            if (dx10 >= 10.0)
            { k = 0.10; a = -1; }
            while ((dx10 > 10.0) || (dx10 <= 1.0))
            { dx10 = dx10 * k; powx += a; }
            //printf("\npowx=%d",*powx);
        }

        //---------------------------------------------------------------
        // Определение количества (lng) символов в числах 
        // ( точнее в среднем числе) массива xax[]
        //---------------------------------------------------------------
        static public void clc_length(out int ppfp, out int ppsp, out int plng, out int pk_step,
                                      double[] xax,
                                      int[] uax,
                                      int nstep, //xax[0]...xax[nstep]!!!
                                      double symw)
        {
            int i;
            int pfp;//Количество знаков до (pow_for_point) десятичной тчк.
            int psp;//Количество знаков после (pow_past_point) десятичной тчк.
            int k_step, du;
            int lng;//количество символов в числах, считая точку и знак.
            int awork;
            double bwork, xsr;

            // Вычисления pfp - степени среднего (xsr)
            pfp = 0;
            bwork = 0.0;
            for (i = 0; i <= nstep; i++)
                bwork = bwork + (double)(Math.Abs((double)(xax[i])));
            xsr = bwork / (nstep + 1);
            clc_pow(xsr, out pfp);
            //??pfp=-pfp;
            // Вычисления psp - степени шага шкалы
            psp = 0;
            bwork = (double)(Math.Abs(xax[2] - xax[1]));
            clc_pow(bwork, out psp);
            //??psp=-psp;
            lng = pfp - psp + 3;
            if (pfp >= 0 && psp >= 0) lng = psp + 2;//0.25-0.28
            if (pfp < 0 && psp >= 0) lng = psp - pfp + 2;//600.02-600.04
            if (pfp < 0 && psp <= 0) lng = -pfp + 1;//630-600
            //...............................................
            //Расчет k_step. kаждое ли деление надо подписывать?
            k_step = 0; // k_step=1-каждое
            du = uax[2] - uax[1];
            do { awork = k_step * du; k_step++; } while (awork > (lng * symw));
            ppfp = pfp;
            ppsp = psp;
            plng = lng;
            pk_step = k_step;
        }
    }// class AxisCommon
}
