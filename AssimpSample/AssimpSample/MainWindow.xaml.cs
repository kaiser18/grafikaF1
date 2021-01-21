using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using SharpGL.SceneGraph;
using SharpGL;
using Microsoft.Win32;


namespace AssimpSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Atributi

        /// <summary>
        ///	 Instanca OpenGL "sveta" - klase koja je zaduzena za iscrtavanje koriscenjem OpenGL-a.
        /// </summary>
        World m_world = null;
        Animation animation = null;

        #endregion Atributi

        #region Konstruktori

        public MainWindow()
        {
            // Inicijalizacija komponenti
            InitializeComponent();

            // Kreiranje OpenGL sveta
            try
            {
                m_world = new World(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\Honda"),
                    Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\Ferrari"),
                    "HondaF13ds.3DS", "model.dae", (int)openGLControl.ActualWidth, (int)openGLControl.ActualHeight, openGLControl.OpenGL);

                animation = new Animation(m_world);
                DataContext = animation;
            }
            catch (Exception e)
            {
                MessageBox.Show("Neuspesno kreirana instanca OpenGL sveta. Poruka greške: " + e.Message, "Poruka", MessageBoxButton.OK);
                this.Close();
            }
        }

        #endregion Konstruktori

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            m_world.Draw(args.OpenGL);
        }

        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            m_world.Initialize(args.OpenGL);
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, OpenGLEventArgs args)
        {
            m_world.Resize(args.OpenGL, (int)openGLControl.ActualWidth, (int)openGLControl.ActualHeight);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(animation.AnimationNotActive) { 
                switch (e.Key)
                {
                    case Key.I:
                        if (m_world.RotationX >= 0.0f)
                            m_world.RotationX -= 5.0f;
                        break;
                    case Key.K:
                        if (m_world.RotationX <= 90.0f)
                            m_world.RotationX += 5.0f;
                        break;
                    case Key.J:
                        m_world.RotationY -= 5.0f;
                        break;
                    case Key.L:
                        m_world.RotationY += 5.0f;
                        break;
                    case Key.OemPlus:
                        m_world.CameraZ -= 2.0f;
                        break;
                    case Key.OemMinus:
                        m_world.CameraZ += 2.0f;
                        break;
                    case Key.V:
                        animation.StartAnimation();
                        break;
                    case Key.F4:
                        Application.Current.Shutdown();
                        break;
                    case Key.R:
                        m_world.Restart();
                        break;
                }
            }
        }

        private void TranslationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (m_world != null && animation.AnimationNotActive)
                m_world.RightTranslate = (float)translate.Value;
        }

        private void RotationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (m_world != null && animation.AnimationNotActive)
                m_world.LeftRotate = (float)rotate.Value;
        }

        private void ColourSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (m_world != null & animation.AnimationNotActive)
            {
                m_world.AmbientRed = (float)sliderRed.Value;
                m_world.AmbientGreen = (float)sliderGreen.Value;
                m_world.AmbientBlue = (float)sliderBlue.Value;
            }
        }

        private void ButtonClick_Restart(object sender, RoutedEventArgs e)
        {
            if (m_world != null && animation.AnimationNotActive)
            {
                m_world.Restart();
            }
        }
    }
}
