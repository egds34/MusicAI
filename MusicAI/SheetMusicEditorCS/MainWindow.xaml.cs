using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Markup;
using System.Xml;

namespace SheetMusicEditorCS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Drawer variables
        private DispatcherTimer timer;

        private DispatcherTimer timerPreview;

        private DispatcherTimer timerProperties;
        private DispatcherTimer timerPlayback;
        private DispatcherTimer timerHome;

        private DispatcherTimer timerSlide;
        private DispatcherTimer timerFade;

        private DispatcherTimer timerSize;
        private DispatcherTimer timerOrientation;

        private DispatcherTimer timerPreviewFade;
        private DispatcherTimer timerListFade;

        private double MenuDrawerWidth;
        private bool MainDrawerHidden;
        private bool NoteMenuHidden;
        private bool PlaybackMenuHidden;
        private bool HomeMenuHidden;

        private bool PreviewMenuHidden;
        private bool ListMenuHidden;

        private bool utilityMenuHidden;
        private bool isFaded;

        private bool hideHomeMenu;
        private bool hideNoteMenu;
        private bool hidePlaybackMenu;

        private bool hidePreviewMunu;
        private bool hideListMenu;

        private bool canOpenMainDrawer;

        //Zooming variables, also used for translating click points raised from click events
        private int zoomStep; //used to make sure you cant zoom too much

        private Double imageWidthOffset;
        private Double imageHeightOffset;
        private int scrollOffsetX;
        private int scrollOffsetY;

        private Button oldSizeSelect;
        private Button newSizeSelect;
        private double selectLineStep;

        private Button oldOrientationSelect;
        private Button newOrientationSelect;

        private Button oldPreviewSelect;
        private Button newPreviewSelect;

        private Note.NoteName currNote; //also applies to rests
        private List<Score> ScoreList; //scores are added here when opened and created. Reasoning is that you can have multiple scores open. I need to figure out how to store bitmaps better. PNG temporarily?
        private Score tempScore; //used for previews, will become a part of the list when the user clicks create
        private ScoreRuler.PaperSize paperSize;
        private ScoreRuler.Orientation orientation;

        public MainWindow()
        {
            InitializeComponent();

            timerPreviewFade = new DispatcherTimer();
            timerPreviewFade.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timerPreviewFade.Tick += TimerPreviewFade_Tick;

            timerListFade = new DispatcherTimer();
            timerListFade.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timerListFade.Tick += TimerListFade_Tick;

            timerPreview = new DispatcherTimer();
            timerPreview.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timerPreview.Tick += TimerPreview_Tick;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Tick += Timer_Tick;

            timerProperties = new DispatcherTimer();
            timerProperties.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timerProperties.Tick += TimerProperties_Tick;

            timerPlayback = new DispatcherTimer();
            timerPlayback.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timerPlayback.Tick += TimerPlayback_Tick;

            timerHome = new DispatcherTimer();
            timerHome.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timerHome.Tick += TimerHome_Tick;

            timerSlide = new DispatcherTimer();
            timerSlide.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timerSlide.Tick += TimerSlide_Tick;

            timerFade = new DispatcherTimer();
            timerFade.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timerFade.Tick += TimerFade_Tick;

            timerSize = new DispatcherTimer();
            timerSize.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timerSize.Tick += TimerSize_Tick;

            timerOrientation = new DispatcherTimer();
            timerOrientation.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timerOrientation.Tick += TimerShorten_Tick;

            //set menu
            ColourMainDefaultButtons();
            BorderColor1.Color = Color.FromArgb(255, 111, 1, 122);
            BorderColor2.Color = Colors.White;
            ButtonColor2.Color = Color.FromArgb(255, 111, 1, 122);
            ButtonColor1.Color = Color.FromArgb(255, 83, 39, 87);
            PropertyOptionsIcon.Foreground = new SolidColorBrush(Colors.White);

            Canvas.SetZIndex(NoteMenu, 1);
            Canvas.SetZIndex(PlaybackMenu, 0);
            Canvas.SetZIndex(HomeMenu, 0);

            Canvas.SetZIndex(ScorePreviewGrid, 1);
            Canvas.SetZIndex(InstrumentListGrid, 0);

            ListMenuHidden = true;
            InstrumentListGrid.Opacity = 0;

            PlaybackMenuHidden = true;
            HomeMenuHidden = true;
            PlaybackMenu.Opacity = 0;
            HomeMenu.Opacity = 0;

            MenuDrawerWidth = MenuDrawer.Width;
            canOpenMainDrawer = true;
            MenuDrawer.Width = 20;
            MainDrawerHidden = true;

            utilityMenuHidden = true;
            isFaded = true;
            UtilityCanvas.Opacity = 0.0;

            ColorHomeDefaultButtons();
            NewMenuButton.Background = new SolidColorBrush(Color.FromArgb(255, 202, 15, 202));

            zoomStep = 0;
            scrollOffsetX = 0;
            scrollOffsetY = 0;

            oldSizeSelect = Button9X12;
            newSizeSelect = Button9X12;

            oldOrientationSelect = PortraitSelectButton;
            newOrientationSelect = PortraitSelectButton;

            oldPreviewSelect = ScorePreviewButton;
            newPreviewSelect = ScorePreviewButton;

            PopulateInstrumentOptions();

            paperSize = ScoreRuler.PaperSize.S9X12;
            orientation = ScoreRuler.Orientation.Portrait;
        }

        /// ///////////////////////////////////////////////////LOADING FUNCTIONS////////////////
        private void ScoreImage_Loaded(object sender, RoutedEventArgs e)
        {
            //BitmapImage bm = new BitmapImage();
            //bm.BeginInit();
            //bm.UriSource = new Uri(@"C:\\Users\\Christiana\\source\\repos\\SoundGenerationLearning\\SoundGenerationLearning\\image1.jpg");
            //bm.EndInit();
            //ScoreImage.Source = bm;

            imageWidthOffset = ScoreBorder.ActualWidth / 10.0;
            imageHeightOffset = ScoreBorder.ActualHeight / 10.0;
        }

        private void NoteMenu_Loaded(object sender, RoutedEventArgs e)
        {
            //NoteMenuHidden = TriggerDrawer(0, NoteMenuHidden, NoteMenuWidth, 20, NoteMenu);
        }

        private void Viewbox_Loaded(object sender, RoutedEventArgs e)
        {
            //ScoreView.Stretch = Stretch.Fill;
            ScoreView.Width = ScoreBorder.ActualWidth;
            ScoreView.Height = ScoreBorder.ActualHeight;
        }

        /// ////////////////////////Timers////////////////////////////////

        private void Timer_Tick(object sender, EventArgs e)
        {
            MainDrawerHidden = TriggerDrawer(MainDrawerHidden, MenuDrawerWidth, 20, MenuDrawer);
        }

        private void TimerProperties_Tick(object sender, EventArgs e)
        {
            NoteMenuHidden = TriggerFade(NoteMenuHidden, NoteMenu);
        }

        private void TimerPlayback_Tick(object sender, EventArgs e)
        {
            PlaybackMenuHidden = TriggerFade(PlaybackMenuHidden, PlaybackMenu);
        }

        private void TimerHome_Tick(object sender, EventArgs e)
        {
            HomeMenuHidden = TriggerFade(HomeMenuHidden, HomeMenu);
        }

        private void TimerSlide_Tick(object sender, EventArgs e)
        {
            utilityMenuHidden = TriggerSlide(utilityMenuHidden, UtilityCanvas);
        }

        private void TimerFade_Tick(object sender, EventArgs e)
        {
            isFaded = TriggerUtilityFade(isFaded, UtilityCanvas);
        }

        private void TimerSize_Tick(object sender, EventArgs e)
        {
            MoveSelectLine(SizeSelectLine, newSizeSelect);
        }

        private void TimerShorten_Tick(object sender, EventArgs e)
        {
            MoveSelectLine(OrientationSelectLine, newOrientationSelect);
        }

        private void TimerPreview_Tick(object sender, EventArgs e)
        {
            MoveSelectLine(PreviewSelectLine, newPreviewSelect);
        }

        private void TimerPreviewFade_Tick(object sender, EventArgs e)
        {
            PreviewMenuHidden = TriggerFade(PreviewMenuHidden, ScorePreviewGrid);
        }

        private void TimerListFade_Tick(object sender, EventArgs e)
        {
            ListMenuHidden = TriggerFade(ListMenuHidden, InstrumentListGrid);
        }

        //////////////////////////////////////////////BUTTON EVENTS///////////////////////////////////////////////////////

        private void MainDrawerButton_Click(object sender, RoutedEventArgs e)
        {
            if (canOpenMainDrawer) timer.Start();
        }

        private void PropertyOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (NoteMenuHidden == false) return;
            ChoosePropertyOptions();
        }

        private void ChoosePropertyOptions()
        {
            ColourMainDefaultButtons(); //clear color

            BorderColor1.Color = Color.FromArgb(255, 111, 1, 122);
            BorderColor2.Color = Colors.White;
            ButtonColor2.Color = Color.FromArgb(255, 111, 1, 122);
            ButtonColor1.Color = Color.FromArgb(255, 83, 39, 87);
            PropertyOptionsIcon.Foreground = new SolidColorBrush(Colors.White);

            Canvas.SetZIndex(NoteMenu, 1);
            Canvas.SetZIndex(PlaybackMenu, 0);
            Canvas.SetZIndex(HomeMenu, 0);

            hideHomeMenu = true;
            hidePlaybackMenu = true;
            hideNoteMenu = false;

            timerProperties.Start();

            if (utilityMenuHidden == false) ShowUtilityMenu();
        }

        private void PlaybackOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlaybackMenuHidden == false) return;
            ColourMainDefaultButtons(); //clear color

            BorderColor3.Color = Color.FromArgb(255, 111, 1, 122);
            BorderColor4.Color = Colors.White;
            ButtonColor4.Color = Color.FromArgb(255, 111, 1, 122);
            ButtonColor3.Color = Color.FromArgb(255, 83, 39, 87);
            PlaybackButtonIcon.Foreground = new SolidColorBrush(Colors.White);

            Canvas.SetZIndex(NoteMenu, 0);
            Canvas.SetZIndex(PlaybackMenu, 1);
            Canvas.SetZIndex(HomeMenu, 0);

            hideHomeMenu = true;
            hideNoteMenu = true;
            hidePlaybackMenu = false;

            timerPlayback.Start();

            if (utilityMenuHidden == false) ShowUtilityMenu();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            if (HomeMenuHidden == false) return;

            //START UI
            ColourMainDefaultButtons(); //clear color

            BorderColor7.Color = Color.FromArgb(255, 111, 1, 122);
            BorderColor8.Color = Colors.White;
            ButtonColor8.Color = Color.FromArgb(255, 111, 1, 122);
            ButtonColor7.Color = Color.FromArgb(255, 83, 39, 87);
            HomeButtonIcon.Foreground = new SolidColorBrush(Colors.White);

            Canvas.SetZIndex(NoteMenu, 0);
            Canvas.SetZIndex(PlaybackMenu, 0);
            Canvas.SetZIndex(HomeMenu, 1);

            hideNoteMenu = true;
            hidePlaybackMenu = true;
            hideHomeMenu = false;

            timerHome.Start();

            if (canOpenMainDrawer == true) PinMenuDrawer();

            //open menu hopefully
            ShowUtilityMenu();
            //END UI

            if (tempScore == null) //once created, temp score will no longer be null, or maybe when you leave the home menu. I think home menu makes more sense
            {
                tempScore = new Score(new ScoreRuler(paperSize, orientation));
                UpdatePreview();
            }
        }

        private void PinMenuToggle_Click(object sender, RoutedEventArgs e)
        {
            if (utilityMenuHidden) PinMenuDrawer();
        }

        private void ScoreView_MouseEnter(object sender, MouseEventArgs e)
        {
            ScoreView.Focus();
        }

        /////////////////////////////////////////////////MOUSE FUNCTIONS////////////////////////////////////
        private void PanelHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MenuDrawer_MouseEnter(object sender, MouseEventArgs e)
        {
            if (canOpenMainDrawer) timer.Start();
        }

        private void MenuDrawer_MouseLeave(object sender, MouseEventArgs e)
        {
            if (canOpenMainDrawer) timer.Start();
        }

        /////////////////////////Functions for animations below.//////////////////////////
        private bool TriggerDrawer(bool isHidden, double openSize, int closeSize, Grid Drawer)
        {
            bool result = isHidden;

            if (result)
            {
                Drawer.Width += 20;
                if (Drawer.Width >= openSize)
                {
                    timer.Stop();
                    result = false;
                }
            }
            else
            {
                Drawer.Width -= 20;
                if (Drawer.Width <= closeSize)
                {
                    timer.Stop();
                    result = true;
                }
            }

            return result;
        }

        private bool TriggerSlide(bool isHidden, Grid obj)
        {
            bool result = isHidden;
            Thickness thickness = obj.Margin;

            if (result) //start sliding in
            {
                thickness.Left -= 15;
                thickness.Right += 15;
                obj.Margin = thickness;
                if (thickness.Right == 280)
                {
                    timerSlide.Stop();
                    result = false;
                }
            }
            else
            {
                thickness.Left += 5;
                thickness.Right -= 5;
                obj.Margin = thickness;
                if (thickness.Left == 75)
                {
                    timerSlide.Stop();
                    result = true;
                }
            }

            return result;
        }

        private bool TriggerUtilityFade(bool isFade, Grid obj)
        {
            bool result = isFade;
            if (!isFade) //fade out
            {
                obj.Opacity -= 0.1;

                if (obj.Opacity <= 0.0)
                {
                    obj.Opacity = 0;
                    Canvas.SetZIndex(obj, -1);
                    timerFade.Stop();
                    result = true;
                }
            }
            else
            {
                obj.Opacity += 0.1;

                if (obj.Opacity >= 1.0)
                {
                    obj.Opacity = 1;
                    timerFade.Stop();
                    result = false;
                }
            }
            return result;
        }

        private bool TriggerFade(bool isHidden, Grid obj)
        {
            bool result = isHidden;
            if (!result)
            {
                obj.Opacity -= 0.1;
                if (obj.Opacity == 0)
                {
                    timerHome.Stop();
                    timerProperties.Stop();
                    timerPlayback.Stop();
                    timerPreviewFade.Stop();
                    timerListFade.Stop();
                    result = false;
                }
            }
            else
            {
                obj.Opacity += 0.1;
                if (obj.Opacity >= 0.999)
                {
                    obj.Opacity = 1;

                    timerHome.Stop();
                    timerProperties.Stop();
                    timerPlayback.Stop();
                    timerPreviewFade.Stop();
                    timerListFade.Stop();

                    if (hideHomeMenu)
                    {
                        HomeMenuHidden = true;
                        HomeMenu.Opacity = 0;
                    }
                    if (hidePlaybackMenu)
                    {
                        PlaybackMenuHidden = true;
                        PlaybackMenu.Opacity = 0;
                    }
                    if (hideNoteMenu)
                    {
                        NoteMenuHidden = true;
                        NoteMenu.Opacity = 0;
                    }
                    if (hidePreviewMunu)
                    {
                        PreviewMenuHidden = true;
                        ScorePreviewGrid.Opacity = 0;
                    }
                    if (hideListMenu)
                    {
                        ListMenuHidden = true;
                        InstrumentListGrid.Opacity = 0;
                    }

                    result = false;
                }
            }

            return result;
        }

        private void ScoreView_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                if (e.Delta < 0) ZoomOut();
                else if (e.Delta > 0) ZoomIn();
            }
        }

        //Used to change menu buttons to main color before shading them with a gradient for the selected option
        private void ColourMainDefaultButtons()
        {
            //PropertyOptionsButton.Background = new SolidColorBrush(Color.FromArgb(255, 83, 39, 87));
            //PlaybackOptionsButton.Background = new SolidColorBrush(Color.FromArgb(255, 83, 39, 87));
            //HomeButton.Background = new SolidColorBrush(Color.FromArgb(255, 83, 39, 87));

            BorderColor1.Color = Color.FromArgb(255, 83, 39, 87);
            BorderColor2.Color = Color.FromArgb(255, 83, 39, 87);
            ButtonColor1.Color = Color.FromArgb(255, 83, 39, 87);
            ButtonColor2.Color = Color.FromArgb(255, 83, 39, 87);
            PropertyOptionsIcon.Foreground = new SolidColorBrush(Color.FromArgb(255, 166, 166, 166));

            BorderColor3.Color = Color.FromArgb(255, 83, 39, 87);
            BorderColor4.Color = Color.FromArgb(255, 83, 39, 87);
            ButtonColor3.Color = Color.FromArgb(255, 83, 39, 87);
            ButtonColor4.Color = Color.FromArgb(255, 83, 39, 87);
            PlaybackButtonIcon.Foreground = new SolidColorBrush(Color.FromArgb(255, 166, 166, 166));

            BorderColor5.Color = Color.FromArgb(255, 83, 39, 87);
            BorderColor6.Color = Color.FromArgb(255, 83, 39, 87);
            ButtonColor5.Color = Color.FromArgb(255, 83, 39, 87);
            ButtonColor6.Color = Color.FromArgb(255, 83, 39, 87);
            ScorePropertiesButtonIcon.Foreground = new SolidColorBrush(Color.FromArgb(255, 166, 166, 166));

            BorderColor7.Color = Color.FromArgb(255, 83, 39, 87);
            BorderColor8.Color = Color.FromArgb(255, 83, 39, 87);
            ButtonColor7.Color = Color.FromArgb(255, 83, 39, 87);
            ButtonColor8.Color = Color.FromArgb(255, 83, 39, 87);
            HomeButtonIcon.Foreground = new SolidColorBrush(Color.FromArgb(255, 166, 166, 166));
        }

        private void ZoomIn()
        {
            if (zoomStep < 10)
            {
                ++zoomStep;
                ScoreView.Height += imageHeightOffset;
                ScoreView.Width += imageWidthOffset;
            }
        }

        private void ZoomOut()
        {
            if (zoomStep > -5)
            {
                --zoomStep;
                ScoreView.Height -= imageHeightOffset;
                ScoreView.Width -= imageWidthOffset;
            }
        }

        private void CalibrateZoom() //this is called when resizing the window. it zoom at the same ration for a different size window
        {
            int previousStep = zoomStep;
            while (zoomStep != 0)
            {
                if (zoomStep > 0) ZoomOut();
                if (zoomStep < 0) ZoomIn();
            }
            imageWidthOffset = tempScore.engraver.GetPaperSize().Width * 5 / 10.0;
            imageHeightOffset = tempScore.engraver.GetPaperSize().Height / 10.0;

            //ScoreView.Width = ScoreBorder.ActualWidth;
            //ScoreView.Height = ScoreBorder.ActualHeight;

            while (zoomStep != previousStep)
            {
                if (zoomStep > previousStep) ZoomOut();
                if (zoomStep < previousStep) ZoomIn();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //CalibrateZoom(); Wow I downt even need this
        }

        private Point TranslatePoint(double x, double y)
        {
            Point result = new Point(0, 0);
            result.Y = (y / ScoreView.ActualHeight) * tempScore.engraver.GetPaperSize().Height; //paper size
            result.X = (x / ScoreView.ActualWidth) * tempScore.engraver.GetPaperSize().Width;

            return result;
        }

        private void ScoreView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point newPoint = e.GetPosition(ScoreView);
            Point translatedPoint = TranslatePoint(newPoint.X * 5, newPoint.Y);

            MessageBox.Show(newPoint.X + " " + newPoint.Y + "\n" + translatedPoint.X + " " + translatedPoint.Y);
        }

        private void ScoreView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void ShowUtilityMenu()
        {
            Canvas.SetZIndex(UtilityCanvas, 5);
            timerSlide.Start();
            timerFade.Start();
        }

        private void PinMenuDrawer()
        {
            //change width of score image
            //make drawer menu uncolapsable
            if (canOpenMainDrawer)
            {
                ScoreGrid.Margin = new Thickness(0, 0, 280, 0);
                canOpenMainDrawer = false;
            }
            else
            {
                ScoreGrid.Margin = new Thickness(0, 0, 20, 0);
                canOpenMainDrawer = true;
            }
        }

        private void ColorHomeDefaultButtons()
        {
            NewMenuButton.Background = new SolidColorBrush(Color.FromArgb(255, 132, 0, 145));
            SaveMenuButton.Background = new SolidColorBrush(Color.FromArgb(255, 132, 0, 145));
            OpenMenuButton.Background = new SolidColorBrush(Color.FromArgb(255, 132, 0, 145));
        }

        private void NewMenuButton_Click(object sender, RoutedEventArgs e)
        {
            ColorHomeDefaultButtons();
            NewMenuButton.Background = new SolidColorBrush(Color.FromArgb(255, 202, 15, 202));
        }

        private void SaveMenuButton_Click(object sender, RoutedEventArgs e)
        {
            ColorHomeDefaultButtons();
            SaveMenuButton.Background = new SolidColorBrush(Color.FromArgb(255, 202, 15, 202));
        }

        private void OpenMenuButton_Click(object sender, RoutedEventArgs e)
        {
            ColorHomeDefaultButtons();
            OpenMenuButton.Background = new SolidColorBrush(Color.FromArgb(255, 202, 15, 202));
        }

        private void MoveSelectLine(Line line, Button newButton)
        {
            if (line.X2 < (newButton.Margin.Left + newButton.Width))
            {
                line.X2 += selectLineStep;
                line.X1 += selectLineStep;
                if (line.X2 >= (newButton.Margin.Left + newButton.Width))
                {
                    line.X2 = newButton.Margin.Left + newButton.Width;
                    line.X1 = newButton.Margin.Left;
                    timerSize.Stop();
                    timerOrientation.Stop();
                    timerPreview.Stop();
                }
            }
            else
            {
                line.X2 -= selectLineStep;
                line.X1 -= selectLineStep;
                if (line.X2 <= (newButton.Margin.Left + newButton.Width))
                {
                    line.X2 = newButton.Margin.Left + newButton.Width;
                    line.X1 = newButton.Margin.Left;
                    timerSize.Stop();
                    timerOrientation.Stop();
                    timerPreview.Stop();
                }
            }
        }

        private void Button9X12_Click(object sender, RoutedEventArgs e)
        {
            if (oldSizeSelect == Button9X12) return;
            newSizeSelect = Button9X12;
            selectLineStep = Math.Abs(oldSizeSelect.Margin.Left - newSizeSelect.Margin.Left) / 10;
            timerSize.Start();

            oldSizeSelect = Button9X12;

            paperSize = ScoreRuler.PaperSize.S9X12;
            tempScore.engraver.SetPaper(new ScoreRuler(paperSize, orientation));
            UpdatePreview();
        }

        private void Button85X11_Click(object sender, RoutedEventArgs e)
        {
            if (oldSizeSelect == Button85X11) return;
            newSizeSelect = Button85X11;
            selectLineStep = Math.Abs(oldSizeSelect.Margin.Left - newSizeSelect.Margin.Left) / 7;
            timerSize.Start();

            oldSizeSelect = Button85X11;

            paperSize = ScoreRuler.PaperSize.S85X11;
            tempScore.engraver.SetPaper(new ScoreRuler(paperSize, orientation));
            UpdatePreview();
        }

        private void Button11X13_Click(object sender, RoutedEventArgs e)
        {
            if (oldSizeSelect == Button11X13) return;
            newSizeSelect = Button11X13;
            selectLineStep = Math.Abs(oldSizeSelect.Margin.Left - newSizeSelect.Margin.Left) / 7;
            timerSize.Start();

            oldSizeSelect = Button11X13;

            paperSize = ScoreRuler.PaperSize.S11X13;
            tempScore.engraver.SetPaper(new ScoreRuler(paperSize, orientation));
            UpdatePreview();
        }

        private void Button11X14_Click(object sender, RoutedEventArgs e)
        {
            if (oldSizeSelect == Button11X14) return;
            newSizeSelect = Button11X14;
            selectLineStep = Math.Abs(oldSizeSelect.Margin.Left - newSizeSelect.Margin.Left) / 7;
            timerSize.Start();

            oldSizeSelect = Button11X14;

            paperSize = ScoreRuler.PaperSize.S11X14;
            tempScore.engraver.SetPaper(new ScoreRuler(paperSize, orientation));
            UpdatePreview();
        }

        private void Button11X17_Click(object sender, RoutedEventArgs e)
        {
            if (oldSizeSelect == Button11X17) return;
            newSizeSelect = Button11X17;
            selectLineStep = Math.Abs(oldSizeSelect.Margin.Left - newSizeSelect.Margin.Left) / 7;
            timerSize.Start();

            oldSizeSelect = Button11X17;

            paperSize = ScoreRuler.PaperSize.S11X17;
            tempScore.engraver.SetPaper(new ScoreRuler(paperSize, orientation));
            UpdatePreview();
        }

        private void PortraitSelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (oldOrientationSelect == PortraitSelectButton) return;
            newOrientationSelect = PortraitSelectButton;
            selectLineStep = Math.Abs(oldOrientationSelect.Margin.Left - newOrientationSelect.Margin.Left) / 7;
            timerOrientation.Start();

            oldOrientationSelect = PortraitSelectButton;

            orientation = ScoreRuler.Orientation.Portrait;
            tempScore.engraver.SetPaper(new ScoreRuler(paperSize, orientation));
            UpdatePreview();
        }

        private void LandscapeSelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (oldOrientationSelect == LandscapeSelectButton) return;
            newOrientationSelect = LandscapeSelectButton;
            selectLineStep = Math.Abs(oldOrientationSelect.Margin.Left - newOrientationSelect.Margin.Left) / 7;
            timerOrientation.Start();

            oldOrientationSelect = LandscapeSelectButton;

            orientation = ScoreRuler.Orientation.Landscape;
            tempScore.engraver.SetPaper(new ScoreRuler(paperSize, orientation));
            UpdatePreview();
        }

        private void TitleField_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TitleField.Text == "Title")
            {
                TitleField.Foreground = new SolidColorBrush(Colors.Gray);
                TitleField.Text = "";
            }
        }

        private void TitleField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TitleField.Text == "")
            {
                TitleField.Foreground = new SolidColorBrush(Colors.LightGray);
                TitleField.Text = "Title";
            }
        }

        private void SubtitleField_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SubtitleField.Text == "Subtitle")
            {
                SubtitleField.Foreground = new SolidColorBrush(Colors.Gray);
                SubtitleField.Text = "";
            }
        }

        private void SubtitleField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SubtitleField.Text == "")
            {
                SubtitleField.Foreground = new SolidColorBrush(Colors.LightGray);
                SubtitleField.Text = "Subtitle";
            }
        }

        private void ComposerField_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ComposerField.Text == "Composer")
            {
                ComposerField.Foreground = new SolidColorBrush(Colors.Gray);
                ComposerField.Text = "";
            }
        }

        private void ComposerField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ComposerField.Text == "")
            {
                ComposerField.Foreground = new SolidColorBrush(Colors.LightGray);
                ComposerField.Text = "Composer";
            }
        }

        private void ArrangerField_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ArrangerField.Text == "Arranger")
            {
                ArrangerField.Foreground = new SolidColorBrush(Colors.Gray);
                ArrangerField.Text = "";
            }
        }

        private void ArrangerField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ArrangerField.Text == "")
            {
                ArrangerField.Foreground = new SolidColorBrush(Colors.LightGray);
                ArrangerField.Text = "Arranger";
            }
        }

        private void LyricistField_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LyricistField.Text == "Lyricist")
            {
                LyricistField.Foreground = new SolidColorBrush(Colors.Gray);
                LyricistField.Text = "";
            }
        }

        private void LyricistField_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LyricistField.Text == "")
            {
                LyricistField.Foreground = new SolidColorBrush(Colors.LightGray);
                LyricistField.Text = "Lyricist";
            }
        }

        private void CopyrightField_GotFocus(object sender, RoutedEventArgs e)
        {
            if (CopyrightField.Text == "Copyright")
            {
                CopyrightField.Foreground = new SolidColorBrush(Colors.Gray);
                CopyrightField.Text = "\xA9";
            }
        }

        private void CopyrightField_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((CopyrightField.Text == "") || (CopyrightField.Text == "\xA9"))
            {
                CopyrightField.Foreground = new SolidColorBrush(Colors.LightGray);
                CopyrightField.Text = "Copyright";
            }
        }

        private void ScorePreviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (oldPreviewSelect == ScorePreviewButton) return;
            newPreviewSelect = ScorePreviewButton;
            selectLineStep = Math.Abs(oldPreviewSelect.Margin.Left - newPreviewSelect.Margin.Left) / 7;
            timerPreview.Start();

            oldPreviewSelect = ScorePreviewButton;

            Canvas.SetZIndex(ScorePreviewGrid, 1);
            Canvas.SetZIndex(InstrumentListGrid, 0);

            hideListMenu = true;
            hidePreviewMunu = false;

            timerPreviewFade.Start();
        }

        private void InstrumentPreviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (oldPreviewSelect == InstrumentPreviewButton) return;
            newPreviewSelect = InstrumentPreviewButton;
            selectLineStep = Math.Abs(oldPreviewSelect.Margin.Left - newPreviewSelect.Margin.Left) / 7;
            timerPreview.Start();

            oldPreviewSelect = InstrumentPreviewButton;

            Canvas.SetZIndex(ScorePreviewGrid, 0);
            Canvas.SetZIndex(InstrumentListGrid, 1);

            hideListMenu = false;
            hidePreviewMunu = true;

            timerListFade.Start();
        }

        private void PopulateInstrumentOptions()
        {
            foreach (var item in Enum.GetValues(typeof(Instrument.Instruments)))
            {
                Instrument newInstrument = new Instrument((Instrument.Instruments)item, Enum.GetName(typeof(Instrument.Instruments), item));
                InstrumentOptionsListBox.Items.Add(newInstrument);
            }
        }

        private void AddInstrumnentToScoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (InstrumentOptionsListBox.SelectedItem == null) return;
            Instrument newInstrument = new Instrument((Instrument)InstrumentOptionsListBox.SelectedItem);
            ScoreInstrumentsListBox.Items.Add(newInstrument);

            //Rename
        }

        private void RemoveInstrumentFromScoreButton_Click(object sender, RoutedEventArgs e)
        {
            ScoreInstrumentsListBox.Items.Remove(ScoreInstrumentsListBox.SelectedItem);
        }

        private void SortSoreInstrumentListBox(int type)
        {
        }

        private void ScoreInstrumentsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ScoreInstrumentsListBox.SelectedItem == null)
            {
                RenameInstrumentTextBox.Text = "";
                return;
            }
            RenameInstrumentTextBox.Text = ((Instrument)ScoreInstrumentsListBox.SelectedItem).instrumentName;
        }

        private void RenameInstrumentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ScoreInstrumentsListBox.SelectedItem == null) return;
            ((Instrument)ScoreInstrumentsListBox.SelectedItem).instrumentName = RenameInstrumentTextBox.Text;
            ScoreInstrumentsListBox.Items.Refresh();
        }

        private void UpdatePaperSize()
        {
            tempScore.engraver.SetPaper(new ScoreRuler(paperSize, orientation));
        }

        private void UpdatePreview()
        {
            PreviewImage.Source = tempScore.engraver.GetScoreBitmap();
            GC.Collect(2);
        }

        /// <summary>
        /// Adds 2 grid columns. One that is the width of the page (acts as a sort of place holder) and one that represents the space between pages
        /// pages go in order of the following equation: pg # = column / 2
        /// </summary>
        private void AddScorePage()
        {
            int previousStep = zoomStep;
            while (zoomStep != 0)
            {
                if (zoomStep > 0) ZoomOut();
                if (zoomStep < 0) ZoomIn();
            }

            ++numPages;

            ColumnDefinition c1 = new ColumnDefinition();
            c1.Width = new GridLength(10, GridUnitType.Pixel); //check here first
            ColumnDefinition c2 = new ColumnDefinition();
            c2.Width = new GridLength(tempScore.engraver.GetPaperSize().Width, GridUnitType.Star);

            ScoreView.Height = tempScore.engraver.GetPaperSize().Height;
            ScoreView.Width = (tempScore.engraver.GetPaperSize().Width * numPages) + (numPages * 10);

            ScoreView.ColumnDefinitions.Add(c1);
            ScoreView.ColumnDefinitions.Add(c2);

            Image image = new Image();
            image.Stretch = Stretch.Fill;
            image.Source = tempScore.engraver.GetScoreBitmap();
            Grid.SetColumn(image, (numPages * 2) - 1);
            ScoreView.Children.Add(image);

            imageWidthOffset = ((tempScore.engraver.GetPaperSize().Width * numPages) + (numPages * 10)) / 10.0;
            imageHeightOffset = tempScore.engraver.GetPaperSize().Height / 10.0;

            while (zoomStep != previousStep)
            {
                if (zoomStep > previousStep) ZoomOut();
                if (zoomStep < previousStep) ZoomIn();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ScoreView.ColumnDefinitions.Clear();
            ScoreView.Children.Clear();
            numPages = 0;

            AddScorePage();

            ChoosePropertyOptions(); //take away the main menu
        }

        private int numPages = 0; // this will never be used in reality. everytime we add a page, we get the number of pages from the score itself. This makes it so that I can dynamically add pages to multiple scores and keep track of them

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //test
            AddScorePage();
        }
    }
}