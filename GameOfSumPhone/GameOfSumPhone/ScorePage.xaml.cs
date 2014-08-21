using Microsoft.Phone.Controls;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;

namespace GameOfSumPhone
{
    public partial class ScorePage : PhoneApplicationPage
    {
        public static int score;

        public ScorePage()
        {
            InitializeComponent();
            Loaded += ScorePage_Loaded;
        }

        void ScorePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (score > 0)
            {
                score_block.Text = score.ToString();
                bottom_grid.Visibility = System.Windows.Visibility.Visible;
            }
            else
                ReadScores();
        }

        void ReadScores()
        {
            StreamReader reader = new StreamReader("Scores.xml");
            XmlReader xmlReader = XmlReader.Create(reader);
            while (xmlReader.Read())
            {
                if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "Entry"))
                    AddPlayerEntry(xmlReader["name"], xmlReader["score"]);
            }
            reader.Dispose();
        }

        void AddPlayerEntry(string name,string score)
        {
            PlayerEntry pe = new PlayerEntry();
            pe.Player = name;
            pe.Score = int.Parse(score);
            stack_panel.Children.Add(pe);
        }

        void AddScore(string name, int scr)
        {
            XDocument xDoc = XDocument.Load("Scores.xml");
            var newScore = new XElement("Entry");
            newScore.SetAttributeValue("name", name);
            newScore.SetAttributeValue("score", scr.ToString());
            xDoc.Root.LastNode.AddAfterSelf(newScore);
            StreamWriter writer = new StreamWriter("Scores.xml");
            xDoc.Save(writer);
            writer.Dispose();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            base.OnBackKeyPress(e);
        }

        private void yes_button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(name_box.Text.Trim().Length>0 && score>0)
            {
                //AddPlayerEntry(name_box.Text.Trim(), score.ToString());
                bottom_grid.Visibility = System.Windows.Visibility.Collapsed;
                AddScore(name_box.Text.Trim(), score);
                score = 0;
                ReadScores();
            }
        }
    }
}