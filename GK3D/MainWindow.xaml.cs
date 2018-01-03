using GK3D.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace GK3D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game _game;
        public MainWindow()
        {
            InitializeComponent();
            InitWindow();
            InitGame();
        }

        private void InitWindow()
        {
            WindowStartupLocation = WindowStartupLocation.Manual;
            Left = 100;
            Top = 100;
            Width = 400;
            Height = SystemParameters.PrimaryScreenHeight - 2 * Top;
        }
        private void InitGame()
        {
            var location = new System.Drawing.Point(Convert.ToInt32(Left + Width), Convert.ToInt32(Top));
            var size = new System.Drawing.Size(Convert.ToInt32(SystemParameters.PrimaryScreenWidth - 2 * Left - Width), Convert.ToInt32(Height));

            Task.Factory.StartNew(() =>
            {
                _game = new Game();
                _game.Location = location;
                _game.Size = size;
                _game.Run(30, 30);
            });
            
        }
    }
}
