using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace GameOfSumPhone
{
    public partial class PlayerEntry : UserControl
    {
        public PlayerEntry()
        {
            InitializeComponent();
        }

        public string Player
        {
            get
            {
                return name_block.Text;
            }

            set
            {
                name_block.Text = value;
            }
        }

        public int Score
        {
            get
            {
                return int.Parse(score_block.Text);
            }

            set
            {
                score_block.Text = value.ToString();
            }
        }

        public Brush Foreground
        {
            get
            {
                return name_block.Foreground;
            }
            set
            {
                name_block.Foreground = value;
                score_block.Foreground = value;
            }
        }
    }
}
