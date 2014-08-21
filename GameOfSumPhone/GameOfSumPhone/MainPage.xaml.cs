using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Media;

namespace GameOfSumPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            //title_block.FontFamily = new FontFamily(@"\Fonts\Adventure.ttf#Adventure Subtitles");
        }

        private void free_play_button_Click(object sender, RoutedEventArgs e)
        {
            GamePage.mode = 1;
            this.NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void time_trial_button_Click(object sender, RoutedEventArgs e)
        {
            GamePage.mode = 2;
            this.NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void move_trial_button_Click(object sender, RoutedEventArgs e)
        {
            GamePage.mode = 3;
            this.NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void scores_button_Click(object sender, RoutedEventArgs e)
        {
            ScorePage.score = 0;
            this.NavigationService.Navigate(new Uri("/ScorePage.xaml", UriKind.Relative));
        }

        private void about_button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Terminate();
        }
    }
}