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
using System.Diagnostics;

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
            Width = 300;
            Height = SystemParameters.PrimaryScreenHeight - 2 * Top;
        }
        private void InitGame()
        {
            var location = new System.Drawing.Point(Convert.ToInt32(Left + Width), Convert.ToInt32(Top));
            var size = new System.Drawing.Size(Convert.ToInt32(SystemParameters.PrimaryScreenWidth - 2 * Left - Width), Convert.ToInt32(Height));

            Task.Factory.StartNew(() =>
          {
              using (_game = new Game())
              {
                  _game.Location = location;
                  _game.Size = size;
                  _game.Run(30, 30);
              }
          });

        }

        private void CameraChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_game != null)
                _game.SceneController.ChangeCamera();
        }

        private void shadingChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_game != null)
            {
                _game.SceneController.ChangeShading();
                if (shadingModel.Text == "shading model: Phong")
                    shadingModel.Text = "shading model: Gouraud";
                else
                    shadingModel.Text = "shading model: Phong";
            }
        }

        private void lightingChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_game != null)
            {
                _game.SceneController.ChangeLighting();
                if (lightingModel.Text == "lighting model: Phong")
                    lightingModel.Text = "lighting model: Blinn";
                else
                    lightingModel.Text = "lighting model: Phong";
            }
        }

        private void AnimationSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_game != null)
                _game.SceneController.SceneScenario.SunAnimationSpeed = (float)e.NewValue / 3;
        }

        private void SunBrightness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_game != null)
                _game.SceneController.SceneScenario.SunBrightness = (float)e.NewValue/10;
        }

        private void movieAnimated_Click(object sender, RoutedEventArgs e)
        {
            _game.SceneController.SceneScenario.StartStopAnimation();
        }

        private void movieMode_Click(object sender, RoutedEventArgs e)
        {
            _game.SceneController.SceneScenario.StartStopMovie();

        }
    }
}
