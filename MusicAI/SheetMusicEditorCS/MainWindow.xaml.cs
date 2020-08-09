using System;
using System.Collections.Generic;
using System.IO.Packaging;
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

namespace SheetMusicEditorCS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Drawer variables
        private DispatcherTimer timer;

        private DispatcherTimer timerProperties;
        private DispatcherTimer timerPlayback;
        private DispatcherTimer timerHome;

        private double MenuDrawerWidth;
        private bool MainDrawerHidden;
        private bool NoteMenuHidden;
        private bool PlaybackMenuHidden;
        private bool HomeMenuHidden;

        private bool hideHomeMenu;
        private bool hideNoteMenu;
        private bool hidePlaybackMenu;

        private bool canOpenMainDrawer;

        //Zooming variables, also used for translating click points raised from click events
        private int zoomStep; //used to make sure you cant zoom too much

        private Double imageWidthOffset;
        private Double imageHeightOffset;
        private int scrollOffsetX;
        private int scrollOffsetY;

        private Note.NoteName currNote; //also applies to rests
        private List<Score> ScoreList; //scores are added here when opened and created. Reasoning is that you can have multiple scores open. I need to figure out how to store bitmaps better. PNG temporarily?

        public MainWindow()
        {
            InitializeComponent();

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

            PlaybackMenuHidden = true;
            HomeMenuHidden = true;
            PlaybackMenu.Opacity = 0;
            HomeMenu.Opacity = 0;

            MenuDrawerWidth = MenuDrawer.Width;
            canOpenMainDrawer = true;
            MenuDrawer.Width = 20;
            MainDrawerHidden = true;

            zoomStep = 0;
            scrollOffsetX = 0;
            scrollOffsetY = 0;
        }

        /// ///////////////////////////////////////////////////LOADING FUNCTIONS////////////////
        private void ScoreImage_Loaded(object sender, RoutedEventArgs e)
        {
            BitmapImage bm = new BitmapImage();
            bm.BeginInit();
            bm.UriSource = new Uri(@"C:\\Users\\Christiana\\source\\repos\\SoundGenerationLearning\\SoundGenerationLearning\\image1.jpg");
            bm.EndInit();
            ScoreImage.Source = bm;

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

        //////////////////////////////////////////////BUTTON EVENTS///////////////////////////////////////////////////////

        private void MainDrawerButton_Click(object sender, RoutedEventArgs e)
        {
            if (canOpenMainDrawer) timer.Start();
        }

        private void PropertyOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (NoteMenuHidden == false) return;
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
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            if (HomeMenuHidden == false) return;
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
        }

        private void PinMenuToggle_Click(object sender, RoutedEventArgs e)
        {
            //change width of score image
            //make drawer menu uncolapsable
            if (canOpenMainDrawer)
            {
                ScoreBorder.Margin = new Thickness(0, 30, 280, 20);
                canOpenMainDrawer = false;
            }
            else
            {
                ScoreBorder.Margin = new Thickness(0, 30, 20, 20);
                canOpenMainDrawer = true;
            }
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
            imageWidthOffset = ScoreBorder.ActualWidth / 10.0;
            imageHeightOffset = ScoreBorder.ActualHeight / 10.0;

            ScoreView.Width = ScoreBorder.ActualWidth;
            ScoreView.Height = ScoreBorder.ActualHeight;

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
            result.Y = (y / ScoreView.ActualHeight) * 2681.0; //paper size
            result.X = (x / ScoreView.ActualWidth) * 4000.0;

            return result;
        }

        private void ScoreView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point newPoint = e.GetPosition(ScoreView);
            Point translatedPoint = TranslatePoint(newPoint.X, newPoint.Y);

            MessageBox.Show(newPoint.X + " " + newPoint.Y + "\n" + translatedPoint.X + " " + translatedPoint.Y);
        }

        private void ScoreView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }
    }
}