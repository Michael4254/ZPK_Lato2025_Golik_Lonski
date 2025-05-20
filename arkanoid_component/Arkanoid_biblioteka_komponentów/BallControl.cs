using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Arkanoid_biblioteka_komponentow
{
    [ToolboxItem(true)]
    public partial class BallControl : UserControl
    {
        private float _dx = 4;     // prędkość pozioma
        private float _dy = -4;    // prędkość pionowa
        private int _radius = 8;   // promień kulki

        public BallControl()
        {
            InitializeComponent();
            TabStop = false;
            Width = _radius * 2;
            Height = _radius * 2;
            DoubleBuffered = true;
        }

        [Category("Gameplay"), Description("Prędkość pozioma kulki (px/klatkę)")]
        public float VelocityX
        {
            get => _dx;
            set => _dx = value;
        }

        [Category("Gameplay"), Description("Prędkość pionowa kulki (px/klatkę)")]
        public float VelocityY
        {
            get => _dy;
            set => _dy = value;
        }

        [Category("Appearance"), Description("Promień kulki (px)")]
        public int Radius
        {
            get => _radius;
            set
            {
                _radius = Math.Max(1, value);
                Width = _radius * 2;
                Height = _radius * 2;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillEllipse(Brushes.Red, 0, 0, Width, Height);
            base.OnPaint(e);
        }

        /// <summary>
        /// Przesuń kulkę o wektor (dx, dy).
        /// </summary>
        public void Move()
        {
            Left += (int)_dx;
            Top += (int)_dy;
        }

        /// <summary>
        /// Odbija kierunek poziomy.
        /// </summary>
        public void ReflectX() => _dx = -_dx;

        /// <summary>
        /// Odbija kierunek pionowy.
        /// </summary>
        public void ReflectY() => _dy = -_dy;
    }
}
