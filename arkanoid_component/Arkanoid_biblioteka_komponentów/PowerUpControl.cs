using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Arkanoid_biblioteka_komponentow
{
    // Typ bonusu
    public enum PowerUpType
    {
        ExpandPaddle,
        ExtraLife,
        SlowBall
    }

    [ToolboxItem(true)]
    public class PowerUpControl : UserControl
    {
        private readonly System.Windows.Forms.Timer _dropTimer;
        private int _speed = 4;

        /// <summary>
        /// Który bonus to jest
        /// </summary>
        [Category("Gameplay"), Description("Typ bonusu")]
        public PowerUpType Type { get; set; }

        public PowerUpControl()
        {
            // konfiguracja kontrolki
            Width = 24;
            Height = 24;
            DoubleBuffered = true;
            BackColor = Color.Gold;

            // timer spadania
            _dropTimer = new System.Windows.Forms.Timer { Interval = 30 };
            _dropTimer.Tick += (s, e) => Top += _speed;
            _dropTimer.Start();
        }

        /// <summary>
        /// Stopuje animację spadania.
        /// </summary>
        public void Stop() => _dropTimer.Stop();

        /// <summary>
        /// Zdarzenie, gdy paletka zbierze bonus.
        /// </summary>
        public event EventHandler<PowerUpType>? Collected;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using var f = new Font("Segoe UI", 12, FontStyle.Bold);
            string txt = Type switch
            {
                PowerUpType.ExpandPaddle => "P",
                PowerUpType.ExtraLife => "L",
                PowerUpType.SlowBall => "S",
                _ => "?"
            };
            TextRenderer.DrawText(
                e.Graphics,
                txt,
                f,
                ClientRectangle,
                Color.Black,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
            );
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            // jeśli chcielibyśmy zbierać kliknięciem…
            base.OnMouseClick(e);
        }

        /// <summary>
        /// Powiadamia, że bonus został zebrany.
        /// </summary>
        protected void OnCollected()
        {
            Collected?.Invoke(this, Type);
        }


    }
}
