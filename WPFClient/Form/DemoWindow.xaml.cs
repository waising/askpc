using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFClient.Model;
using WPFClient.Util;

namespace WPFClient.Form
{
    /// <summary>
    /// DemoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DemoWindow : Window
    {

        log4net.ILog log = log4net.LogManager.GetLogger(typeof(DemoWindow));

        public MainWindow mWindow;

        private AskPen _pen;

        private bool _drawing = false;

        //记录笔画
        LinkedList<Stroke> _strokes = null;

        public AskPen Pen
        {
            get
            {
                return _pen;
            }

            set
            {
                _pen = value;
            }
        }

        public DemoWindow()
        {
            InitializeComponent();
            log.Info("练习窗体");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ////默认设置黑色
            DrawCanvas.DefaultDrawingAttributes.Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("Black");

            //double dh = DrawCanvas.ActualHeight;
            //DrawCanvas.Height = dh + this.ActualHeight;

            _strokes = new LinkedList<Stroke>();

            _pen = new AskPen();
            _pen.Size = 2;
            _pen.Color = "Black";

            ExitDemoButton.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(ExitDemoButton_MouseLeftButtonDown), true);
            ClearButton.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(ClearButton_MouseLeftButtonDown), true);
            ReBackButton.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(ReBackButton_MouseLeftButtonDown), true);

            PenRadio.AddHandler(RadioButton.MouseLeftButtonDownEvent, new MouseButtonEventHandler(PenStyleRadio_MouseLeftButtonDown), true);
            EraserRadio.AddHandler(RadioButton.MouseLeftButtonDownEvent, new MouseButtonEventHandler(PenStyleRadio_MouseLeftButtonDown), true); 

            SmallSizeRadio.AddHandler(RadioButton.MouseLeftButtonDownEvent, new MouseButtonEventHandler(PenSizeRadio_MouseLeftButtonDown), true);
            MidSizeRadio.AddHandler(RadioButton.MouseLeftButtonDownEvent, new MouseButtonEventHandler(PenSizeRadio_MouseLeftButtonDown), true);
            BigSizeRadio.AddHandler(RadioButton.MouseLeftButtonDownEvent, new MouseButtonEventHandler(PenSizeRadio_MouseLeftButtonDown), true);

            BlackColorRadio.AddHandler(RadioButton.MouseLeftButtonDownEvent, new MouseButtonEventHandler(PenColorRadio_MouseLeftButtonDown), true);
            RedColorRadio.AddHandler(RadioButton.MouseLeftButtonDownEvent, new MouseButtonEventHandler(PenColorRadio_MouseLeftButtonDown), true);
            BlueColorRadio.AddHandler(RadioButton.MouseLeftButtonDownEvent, new MouseButtonEventHandler(PenColorRadio_MouseLeftButtonDown), true);
            GreenColorRadio.AddHandler(RadioButton.MouseLeftButtonDownEvent, new MouseButtonEventHandler(PenColorRadio_MouseLeftButtonDown), true);
            YellowColorRadio.AddHandler(RadioButton.MouseLeftButtonDownEvent, new MouseButtonEventHandler(PenColorRadio_MouseLeftButtonDown), true);
        }


        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearPanel()
        {
            DrawCanvas.Strokes.Clear();
            if (_strokes != null)
                _strokes.Clear();
        }

        #region 笔画图
        private void DrawCanvas_StylusDown(object sender, System.Windows.Input.StylusDownEventArgs e)
        {
            if (!DrawCanvas.AreAnyTouchesOver)
            {
                //橡皮擦
                if (DrawCanvas.EditingMode != InkCanvasEditingMode.EraseByPoint)
                    DrawCanvas.EditingMode = InkCanvasEditingMode.Ink;

                _drawing = true;

            }
        }

        private void DrawCanvas_StylusUp(object sender, System.Windows.Input.StylusEventArgs e)
        {
            if (DrawCanvas.EditingMode == InkCanvasEditingMode.EraseByPoint)
            {
                _drawing = false;

                //DrawCanvas.Strokes.Add(_stroke);
            }

        }

        private void DrawCanvas_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if (DrawCanvas.EditingMode != InkCanvasEditingMode.EraseByPoint)
                DrawCanvas.EditingMode = InkCanvasEditingMode.None;

            _drawing = false;
        }


        private void DrawCanvas_StylusMove(object sender, System.Windows.Input.StylusEventArgs e)
        {
            if (DrawCanvas.EditingMode == InkCanvasEditingMode.EraseByPoint || _drawing)
            {

            }
        }

        #endregion

        private void DrawCanvas_StrokeErasing(object sender, InkCanvasStrokeErasingEventArgs e)
        {

        }

        private void DrawCanvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            if (_strokes != null)
                _strokes.AddLast(e.Stroke);
        }


        #region  鼠标画图

        private void DrawCanvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (AskUtil.IsDebug(this))
            {
                if (!DrawCanvas.AreAnyTouchesOver)
                {
                    //橡皮擦
                    if (DrawCanvas.EditingMode != InkCanvasEditingMode.EraseByPoint)
                        DrawCanvas.EditingMode = InkCanvasEditingMode.Ink;

                    _drawing = true;
                }
            }
        }

        private void DrawCanvas_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (AskUtil.IsDebug(this))
            {
                if (DrawCanvas.EditingMode == InkCanvasEditingMode.Ink)
                {
                    _drawing = false;

                    //DrawCanvas.Strokes.Add(_stroke);
                    //DrawCanvas.EditingMode = InkCanvasEditingMode.None;
                }
            }
        }

        private void DrawCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (AskUtil.IsDebug(this))
            {
                if (DrawCanvas.EditingMode == InkCanvasEditingMode.EraseByPoint || _drawing || DrawCanvas.EditingMode == InkCanvasEditingMode.Ink)
                {

                }
            }
        }
        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {

                if (mWindow != null)
                {
                    mWindow.Visibility = System.Windows.Visibility.Visible;
                }

            }
            finally
            {

            }
        }

        /// <summary>
        /// 回退一步
        /// </summary>
        private void BackLine()
        {
            if (_strokes.Count > 0)
            {
                DrawCanvas.Strokes.Remove(_strokes.Last.Value);
                _strokes.RemoveLast();
            }

        }

        private void ExitDemoButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //结算
            if (SMessageBox.Show("确定结束本次练习?", "结束教学", SMessageBoxButton.OKCancel) == SMessageBoxResult.OK)
            {
                Close();
            }
        }

        private void ClearButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DrawPenStyle(PenStyleEnum.Clear);
        }

        private void ReBackButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BackLine();
        }

        private void EraserButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DrawPenStyle(PenStyleEnum.Eraser);
        }

        private void DrawPenStyle(PenStyleEnum style)
        {
            //橡皮擦
            if (style == PenStyleEnum.Eraser)
            {
                DrawCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
                DrawCanvas.DefaultDrawingAttributes.IgnorePressure = true;
                DrawCanvas.EraserShape = new EllipseStylusShape(50, 50);

                DrawCanvas.DefaultDrawingAttributes.Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(DrawCanvas.Background.ToString());
                DrawCanvas.DefaultDrawingAttributes.Width = 50;
                DrawCanvas.DefaultDrawingAttributes.Height = 50;

                PenRadio.IsChecked = false;
                EraserRadio.IsChecked = true;
            }
            else if (style == PenStyleEnum.Clear)
            {
                //清除
                ClearPanel();
            }
            else if (style == PenStyleEnum.Back)
            {
                BackLine();
            }
            else
            {
                PenStyle();
            }
        }

        private void PenStyle()
        {
            PenRadio.IsChecked = true;
            EraserRadio.IsChecked = false;

            DrawCanvas.DefaultDrawingAttributes.Width = _pen.Size;
            DrawCanvas.DefaultDrawingAttributes.Height = _pen.Size;
            DrawCanvas.DefaultDrawingAttributes.IgnorePressure = true;
            //DrawCanvas.DefaultDrawingAttributes.IsHighlighter = true;
            DrawCanvas.EditingMode = InkCanvasEditingMode.Ink;

            DrawCanvas.DefaultDrawingAttributes.Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(_pen.Color);
        }

        private void PenStyleRadio_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            DrawPenStyle((PenStyleEnum)Enum.ToObject(typeof(PenStyleEnum), int.Parse(rb.CommandParameter.ToString())));
        }


        private void PenSizeRadio_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            _pen.Size = int.Parse(rb.CommandParameter.ToString());
            DrawPenStyle(PenStyleEnum.Pen);
        }

        private void PenColorRadio_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            _pen.Color = rb.CommandParameter.ToString();
            DrawPenStyle(PenStyleEnum.Pen);
        }
    }
}
