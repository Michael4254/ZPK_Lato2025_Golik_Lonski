using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Arkanoid_biblioteka_komponentow
{
    [ToolboxItem(true)]
    public partial class BrickControl : UserControl
    {
        private int _hitPoints = 1;

        public BrickControl()
        {
            InitializeComponent();
            TabStop = false;
            Width = 60;
            Height = 24;
            DoubleBuffered = true;
            BackColor = Color.OrangeRed;
        }

        [Category("Gameplay"), Description("Ile razy cegiełkę trzeba trafić, by się zniszczyła")]
        public int HitPoints
        {
            get => _hitPoints;
            set
            {
                _hitPoints = Math.Max(1, value);
                Invalidate();
            }
        }

        /// <summary>
        /// Wywołaj, gdy kulka trafi w cegiełkę. Zmniejsza hitpoints i ewentualnie usuwa.
        /// </summary>
        public void Hit()
        {
            _hitPoints--;
            if (_hitPoints <= 0)
            {
                // opcjonalne zdarzenie dla Form1
                BrickDestroyed?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                // zmień kolor, by pokazać uszkodzenie
                BackColor = ControlPaint.Dark(BackColor);
            }
        }

        /// <summary>
        /// Zdarzenie zgłaszane, gdy cegiełka zostanie zniszczona
        /// </summary>
        public event EventHandler? BrickDestroyed;

        protected override void OnPaint(PaintEventArgs e)
        {
            // rysujemy ramkę i wypełnienie
            using var brush = new SolidBrush(BackColor);
            using var pen = new Pen(Color.Black, 1);
            e.Graphics.FillRectangle(brush, 0, 0, Width, Height);
            e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            base.OnPaint(e);
        }
    }
}
