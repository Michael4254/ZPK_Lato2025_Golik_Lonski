using System;
using System.Drawing;
using System.Windows.Forms;

namespace arkanoid_component
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            // 1) Stały rozmiar i brak skalowania
            this.ClientSize = new Size(700, 425);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Arkanoid - Menu";

            // 2) Przyciski
            var btnGraj = MakeButton("Graj", new Point(300, 120));
            btnGraj.Click += (s, e) => StartGame();

            var btnRanking = MakeButton("Ranking", new Point(300, 180));
            btnRanking.Click += (s, e) => ShowRanking();

            var btnOpcje = MakeButton("Opcje", new Point(300, 240));
            btnOpcje.Click += (s, e) => ShowOptions();

            var btnWyjscie = MakeButton("Wyjście", new Point(300, 300));
            btnWyjscie.Click += (s, e) => Application.Exit();

            // 3) Dodaj do Controls
            Controls.AddRange(new Control[] { btnGraj, btnRanking, btnOpcje, btnWyjscie });
        }

        // Pomocnicza fabryka przycisków
        private Button MakeButton(string text, Point location)
        {
            return new Button
            {
                Text = text,
                Size = new Size(100, 40),
                Location = location,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
        }

        private void StartGame()
        {
            var game = new Form1();
            this.Hide();
            game.FormClosed += (s, e) => this.Show();
            game.Show();
        }

        private void ShowRanking()
        {
            using var rf = new RankingForm();
            rf.ShowDialog(this);
        }


        private void ShowOptions()
        {
            using var of = new OptionsForm();
            of.ShowDialog(this);
        }

    }
}
