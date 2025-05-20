using System;
using System.Drawing;
using System.Windows.Forms;

namespace arkanoid_component
{
    public partial class OptionsForm : Form
    {
        private NumericUpDown numLives;
        private NumericUpDown numPoints;
        private Button btnOk;
        private Button btnCancel;

        public OptionsForm()
        {
            InitializeComponent();
            BuildUI();
            LoadCurrentValues();
        }

        private void BuildUI()
        {
            this.Text = "Opcje";
            this.ClientSize = new Size(300, 180);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            var lblLives = new Label
            {
                Text = "Liczba żyć:",
                Location = new Point(20, 20),
                AutoSize = true
            };
            numLives = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 10,
                Location = new Point(150, 18),
                Width = 80
            };

            var lblPoints = new Label
            {
                Text = "Punkty za klocek:",
                Location = new Point(20, 60),
                AutoSize = true
            };
            numPoints = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 1000,
                Location = new Point(150, 58),
                Width = 80
            };

            btnOk = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(50, 110),
                Width = 80
            };
            btnOk.Click += (s, e) => SaveAndClose();

            btnCancel = new Button
            {
                Text = "Anuluj",
                DialogResult = DialogResult.Cancel,
                Location = new Point(160, 110),
                Width = 80
            };

            this.Controls.AddRange(new Control[]
            {
                lblLives, numLives,
                lblPoints, numPoints,
                btnOk, btnCancel
            });
        }

        private void LoadCurrentValues()
        {
            numLives.Value = OptionsManager.Lives;
            numPoints.Value = OptionsManager.PointsPerBrick;
        }

        private void SaveAndClose()
        {
            OptionsManager.Lives = (int)numLives.Value;
            OptionsManager.PointsPerBrick = (int)numPoints.Value;
            this.Close();
        }
    }
}
