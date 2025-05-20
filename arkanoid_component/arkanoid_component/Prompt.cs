using System.Drawing;
using System.Windows.Forms;

namespace arkanoid_component
{
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            var prompt = new Form()
            {
                Width = 350,
                Height = 160,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };
            var label = new Label()
            {
                Left = 20,
                Top = 20,
                Width = 300,
                Text = text
            };
            var input = new TextBox()
            {
                Left = 20,
                Top = 50,
                Width = 300
            };
            var ok = new Button()
            {
                Text = "OK",
                Left = 160,
                Width = 80,
                Top = 85,
                DialogResult = DialogResult.OK
            };
            prompt.Controls.Add(label);
            prompt.Controls.Add(input);
            prompt.Controls.Add(ok);
            prompt.AcceptButton = ok;

            return prompt.ShowDialog() == DialogResult.OK
                ? input.Text.Trim()
                : "";
        }
    }
}
