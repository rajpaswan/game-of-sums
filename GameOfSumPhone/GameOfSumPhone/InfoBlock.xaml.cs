using System.Windows.Controls;
using System.Windows.Media;

namespace GameOfSumPhone
{
    public partial class InfoBlock : UserControl
    {
        public InfoBlock()
        {
            InitializeComponent();
        }

        public string Data
        {
            get
            {
                return data_block.Text;
            }

            set
            {
                data_block.Text = value;
            }
        }


        public string Info
        {
            get
            {
                return info_block.Text;
            }

            set
            {
                info_block.Text = value;
            }
        }

        public Brush TextColor
        {
            get
            {
                return data_block.Foreground;
            }

            set
            {
                data_block.Foreground = value;
                info_block.Foreground = value;
            }
        }
    }
}
