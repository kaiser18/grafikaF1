using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace AssimpSample
{
    class Animation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private World m_world = null;
        private DispatcherTimer leftCarTimer;
        private DispatcherTimer rightCarTimer;
        private DispatcherTimer cameraTimer;

        private bool animationNotActive = true;

        public Animation(World world)
        {
            m_world = world;
        }

        public bool AnimationNotActive
        {
            get
            {
                return animationNotActive;
            }
            set
            {
                animationNotActive = value;
                OnPropertyChanged("AnimationNotActive");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public void LeftCarAnimation(object sender, EventArgs e)
        {
            m_world.LeftMoveForward -= 2.0f;
            m_world.LightTranslate -= 2.0f;

            if (m_world.LeftMoveForward <= -30.0f)
            {
                leftCarTimer.Stop();
                m_world.LeftMoveForward -= 0.35f;
            }
        }

        public void RightCarAnimation(object sender, EventArgs e)
        {
            m_world.RightMoveForward -= 2.0f;

            if (m_world.RightMoveForward <= -30.0f)
            {
                rightCarTimer.Stop();
                cameraTimer.Stop();
                AnimationNotActive = true;
            }
        }

        public void CameraAnimation(object sender, EventArgs e)
        {
            if (m_world.CameraZ > (m_world.PointZ + 10.0f))
            {
                m_world.CameraZ -= 2.0f;
            }
            else
            {
                m_world.CameraY += 7.0f;
            }
        }

        public void StartAnimation()
        {
            AnimationNotActive = false;
            leftCarTimer = new DispatcherTimer();
            leftCarTimer.Interval = TimeSpan.FromMilliseconds(5);
            leftCarTimer.Tick += new EventHandler(LeftCarAnimation);

            rightCarTimer = new DispatcherTimer();
            rightCarTimer.Interval = TimeSpan.FromMilliseconds(60);
            rightCarTimer.Tick += new EventHandler(RightCarAnimation);

            cameraTimer = new DispatcherTimer();
            cameraTimer.Interval = TimeSpan.FromMilliseconds(60);
            cameraTimer.Tick += new EventHandler(CameraAnimation);

            leftCarTimer.Start();
            rightCarTimer.Start();
            cameraTimer.Start();
        }
    }
}
