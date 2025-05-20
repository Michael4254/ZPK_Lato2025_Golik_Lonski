using System;
using System.Windows.Forms;

namespace arkanoid_component
{
    public partial class RankingForm : Form
    {
        public RankingForm()
        {
            InitializeComponent();
            Text = "Ranking";
            ClientSize = new System.Drawing.Size(400, 500);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoGenerateColumns = false
            };

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Pozycja",
                Width = 60,
                CellTemplate = new DataGridViewTextBoxCell()
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Nick",
                DataPropertyName = "Nickname",
                Width = 150
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Punkty",       // zamiast Level
                DataPropertyName = "Points",
                Width = 80
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Data",
                DataPropertyName = "Date",
                Width = 100,
                DefaultCellStyle = { Format = "g" }
            });

            var list = RankingManager.LoadScores();
            for (int i = 0; i < list.Count; i++)
                dgv.Rows.Add(i + 1, list[i].Nickname, list[i].Points, list[i].Date);

            Controls.Add(dgv);
        }
    }
}
