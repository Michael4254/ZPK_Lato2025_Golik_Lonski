namespace paddleTest
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            paddleControl1 = new Arkanoid_biblioteka_komponentow.PaddleControl();
            ballControl1 = new Arkanoid_biblioteka_komponentow.BallControl();
            brickControl1 = new Arkanoid_biblioteka_komponentow.BrickControl();
            ballControl2 = new Arkanoid_biblioteka_komponentow.BallControl();
            SuspendLayout();
            // 
            // paddleControl1
            // 
            paddleControl1.BackColor = Color.Transparent;
            paddleControl1.Location = new Point(310, 432);
            paddleControl1.Name = "paddleControl1";
            paddleControl1.Size = new Size(80, 16);
            paddleControl1.Speed = 10;
            paddleControl1.TabIndex = 0;
            paddleControl1.TabStop = false;
            // 
            // ballControl1
            // 
            ballControl1.Location = new Point(319, 203);
            ballControl1.Name = "ballControl1";
            ballControl1.Radius = 8;
            ballControl1.Size = new Size(16, 16);
            ballControl1.TabIndex = 1;
            ballControl1.TabStop = false;
            ballControl1.VelocityX = 4F;
            ballControl1.VelocityY = -4F;
            // 
            // brickControl1
            // 
            brickControl1.BackColor = Color.OrangeRed;
            brickControl1.HitPoints = 1;
            brickControl1.Location = new Point(319, 36);
            brickControl1.Name = "brickControl1";
            brickControl1.Size = new Size(60, 24);
            brickControl1.TabIndex = 2;
            brickControl1.TabStop = false;
            // 
            // ballControl2
            // 
            ballControl2.Location = new Point(506, 261);
            ballControl2.Name = "ballControl2";
            ballControl2.Radius = 8;
            ballControl2.Size = new Size(16, 16);
            ballControl2.TabIndex = 3;
            ballControl2.TabStop = false;
            ballControl2.VelocityX = 4F;
            ballControl2.VelocityY = -4F;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(ballControl2);
            Controls.Add(brickControl1);
            Controls.Add(ballControl1);
            Controls.Add(paddleControl1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Arkanoid_biblioteka_komponentow.PaddleControl paddleControl1;
        private Arkanoid_biblioteka_komponentow.BallControl ballControl1;
        private Arkanoid_biblioteka_komponentow.BrickControl brickControl1;
        private Arkanoid_biblioteka_komponentow.BallControl ballControl2;
    }
}
