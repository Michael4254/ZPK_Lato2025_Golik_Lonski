using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Arkanoid_biblioteka_komponentow
{
    [ToolboxItem(true)]
    public partial class PaddleControl : UserControl
    {
        private int _speed = 10;

        public PaddleControl()
        {
            InitializeComponent();
            // niech form i inne kontrolki nie odbierają focusa od paletki
            TabStop = false;
            Width = 80;
            Height = 16;
            DoubleBuffered = true;
            // opcjonalnie: ustaw tło w designerze
            BackColor = Color.Transparent;
        }

        [Category("Gameplay"), Description("Prędkość ruchu paletki (px/klatka)")]
        public int Speed
        {
            get => _speed;
            set => _speed = Math.Max(1, value);
        }

        // aby strzałki nie były traktowane jako nawigacja, tylko input
        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Left || keyData == Keys.Right)
                return true;
            return base.IsInputKey(keyData);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // rysujemy ciemnoniebieską paletkę
            e.Graphics.FillRectangle(Brushes.DarkBlue, 0, 0, Width, Height);
            base.OnPaint(e);
        }

        public void MoveLeft()
        {
            Left = Math.Max(0, Left - Speed);
        }

        public void MoveRight()
        {
            if (Parent == null) return;
            int maxX = Parent.ClientSize.Width - Width;
            Left = Math.Min(maxX, Left + Speed);
        }
    }
}
