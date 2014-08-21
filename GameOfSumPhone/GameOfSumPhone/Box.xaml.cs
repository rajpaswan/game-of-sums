using System.Windows.Controls;
using System.Windows.Media;

namespace GameOfSumPhone
{
    public partial class Box : UserControl
    {
        public Box()
        {
            InitializeComponent();
        }

        public string Text
        {
            get
            {
                return textblock.Text;
            }
            set
            {
                textblock.Text = value;
            }
        }

        public Brush Foreground
        {
            get
            {
                return textblock.Foreground;
            }
            set
            {
                border.BorderBrush = value;
                textblock.Foreground = value;
            }
        }
    }
}
