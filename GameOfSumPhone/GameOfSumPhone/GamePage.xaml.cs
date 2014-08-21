using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace GameOfSumPhone
{
    public partial class GamePage : PhoneApplicationPage
    {
        public static int mode;

        DispatcherTimer timer;
        //Dictionary<string, Box> dict;
        List<Box>  selected;
        
        int score = 0;
        int sum = 0;
        int move = 30;
        TimeSpan time = TimeSpan.FromMinutes(3);

        public GamePage()
        {
            InitializeComponent();
            
            //dict = new Dictionary<string, Box>();
            selected = new List<Box>();
            
            Loaded += GamePage_Loaded;

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                switch (GamePage.mode)
                {
                    case 1: mode_infoblock.Visibility = System.Windows.Visibility.Collapsed; break;
                    case 2: mode_infoblock.Data = "03:00"; mode_infoblock.Info = "time left"; break;
                    case 3: mode_infoblock.Data = "30"; mode_infoblock.Info = "moves left"; break;
                }
            }
            base.OnNavigatedTo(e);
        }

        void ResetGame()
        {
            score = 0;
            sum = 0;
            move = 30;
            time = TimeSpan.FromMinutes(3);
        }

        void ResetTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1.0);
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            time = time.Subtract(TimeSpan.FromSeconds(1));
            mode_infoblock.Data = time.ToString(@"mm\:ss");

            if( time.TotalSeconds == 5)
            {
                play_sound("tick_tock");
            }
            else if (time.TotalSeconds <= 0)
            {
                timer.Stop();
                MessageBox.Show("Your score is " + score, "Game Over", MessageBoxButton.OK);
                ScorePage.score = score;
                this.NavigationService.Navigate(new Uri("/ScorePage.xaml", UriKind.Relative));
            }
        }

        void GamePage_Loaded(object sender, RoutedEventArgs e)
        {
            CreateBoxes();
            ResetGame();
            SetNewSum();
            
            if (GamePage.mode == 2)
            {
                ResetTimer();
                timer.Start();
            }
        }

        private void SetNewSum()
        {
            Random r = new Random();
            int t = r.Next(1, 31);
            if (sum == t)
                SetNewSum();
            else
            {
                sum = t;
                sum_infoblock.Data = sum.ToString();
            }
        }

        void CreateBoxes()
        {
            int row = (int)Application.Current.Host.Content.ActualWidth / 70;
            int col = ((int)Application.Current.Host.Content.ActualHeight - 100) / 70 - 1;
            Random rand = new Random();

            for (int i = 0; i < col; ++i)
            {
                StackPanel sp = new StackPanel();
                sp.Orientation = System.Windows.Controls.Orientation.Horizontal;
                for (int j = 0; j < row; ++j)
                {
                    Box box = new Box();
                    box.Text=rand.Next(1,6).ToString();
                    //box events
                    box.MouseEnter += box_MouseEnter;
                    box.Name = String.Format("b_{0}_{1}",i,j);
                    //dict.Add(box.Name, box);
                    sp.Children.Add(box);
                }
                stack_panel.Children.Add(sp);
            }
        }

        private void box_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Box box = sender as Box;
            selected.Add(sender as Box);

            //check for validity of the selected sequence
            if (selected.Count > 1)
            {
                for (int i = 1; i < selected.Count; i++)
                {
                    int ai = int.Parse(selected[i - 1].Name.Split('_')[1]);
                    int aj = int.Parse(selected[i - 1].Name.Split('_')[2]);
                    int bi = int.Parse(selected[i].Name.Split('_')[1]);
                    int bj = int.Parse(selected[i].Name.Split('_')[2]);

                    if (!((Math.Abs(ai - bi) == 1 && aj - bj == 0) || (Math.Abs(aj - bj) == 1 && ai - bi == 0)))
                    {
                        foreach (Box b in selected)
                            b.Foreground = new SolidColorBrush(Colors.Gray);
                        
                        selected.Clear();
                        selected.Add(box);
                    }
                }
            }

            play_sound("pick");

            if (selected.Contains(box))
            {
                int index = selected.IndexOf(box);
                int count = selected.Count - index - 1;

                foreach (Box b in selected.GetRange(index + 1, count))
                   b.Foreground = new SolidColorBrush(Colors.Gray);

                selected.RemoveRange(index + 1, count);
            }

            int c_sum = 0;
            SolidColorBrush brush = null;

            //calculating the sum of the selected sequence
            foreach (Box b in selected)
                c_sum += int.Parse(b.textblock.Text);


            if (c_sum == sum)
                brush = new SolidColorBrush(Colors.Green);
            else if (c_sum < sum)
                brush = new SolidColorBrush(Colors.Yellow);
            else
                brush = new SolidColorBrush(Colors.Red);

            foreach (Box b in selected)
                b.Foreground = brush;

            current_infoblock.Data = c_sum.ToString();
        }

        private void root_grid_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            int cur_sum = 0;
            foreach (Box b in selected)
                cur_sum += int.Parse(b.textblock.Text);

            if (sum == cur_sum)
            {
                play_sound("pop");
                
                //Add score
                double d = (double)sum/ selected.Count;
                score += (int)Math.Ceiling(d);
                score_infoblock.Data = score.ToString();

                Random r = new Random();
                foreach (Box b in selected)
                {
                    b.Text = r.Next(1, 6).ToString();
                    b.Foreground = new SolidColorBrush(Colors.Gray);
                }
                selected.Clear();

                if(GamePage.mode==3)
                {
                    move--;
                    mode_infoblock.Data = move.ToString();
                    if(move <=0)
                    {
                        MessageBox.Show("Your score is " + score, "Game Over", MessageBoxButton.OK);
                        ScorePage.score = score;
                        this.NavigationService.Navigate(new Uri("/ScorePage.xaml", UriKind.Relative));
                        return;
                    }
                }
                SetNewSum();
            }
            else
            {
                foreach (Box b in selected)
                    b.Foreground = new SolidColorBrush(Colors.Gray);
                selected.Clear();
            }

            current_infoblock.Data = "0";
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (mode == 2)
                timer.Stop();
            var result = MessageBox.Show("Do you want to leave the game?", "Game Paused", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                if (mode == 2)
                    timer.Start();
                e.Cancel = true;
            }
        }

        private void play_sound(string sound)
        {
            Stream stream = TitleContainer.OpenStream("Sounds/" + sound + ".wav");
            SoundEffect effect = SoundEffect.FromStream(stream);
            FrameworkDispatcher.Update();
            effect.Play();
        }
    }
}