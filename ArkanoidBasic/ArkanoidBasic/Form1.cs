using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace ArkanoidBasic
{
    public partial class Form1 : Form
    {
        Timer timer = new Timer();
        Rectangle paddle;
        Rectangle ball;
        int ballSpeedX = 4;
        int ballSpeedY = 4;
        bool leftArrowDown;
        bool rightArrowDown;
        int defaultLives = 3;
        int pointsPerBlock = 100;
        int paddleLength = 100;




        List<Rectangle> blocks = new List<Rectangle>();
        int currentLevel = 1;
        int lives = 3;
        int score = 0;
        string playerName = "";

        Label lblStatus;
        Panel menuPanel;
        Button btnStart;
        Button btnRanking;
        Button btnOptions;
        Button btnExit;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Width = 800;
            this.Height = 600;



            InitMenu();

            lblStatus = new Label();
            lblStatus.Text = "";
            lblStatus.Font = new Font("Arial", 12, FontStyle.Bold);
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(10, 10);
            this.Controls.Add(lblStatus);

            timer.Interval = 20;
            timer.Tick += GameLoop;

            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;
            this.Paint += OnPaint;

            menuPanel.Visible = true;
        }

        private void InitMenu()
        {
            menuPanel = new Panel();
            menuPanel.Size = new Size(200, 200);
            menuPanel.Location = new Point((this.Width - 200) / 2, (this.Height - 200) / 2);
            this.Controls.Add(menuPanel);

            Label lblTitle = new Label();
            lblTitle.Text = "ARKANOID";
            lblTitle.Font = new Font("Arial", 16, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(50, 5);
            menuPanel.Controls.Add(lblTitle);

            btnStart = new Button();
            btnStart.Text = "Nowa Gra";
            btnStart.Location = new Point(30, 40);
            btnStart.Size = new Size(140, 30);
            btnStart.Click += (s, e) => StartNewGame();
            menuPanel.Controls.Add(btnStart);

            btnRanking = new Button();
            btnRanking.Text = "Ranking";
            btnRanking.Location = new Point(30, 80);
            btnRanking.Size = new Size(140, 30);
            btnRanking.Click += (s, e) => ShowRanking();
            menuPanel.Controls.Add(btnRanking);

            btnOptions = new Button();
            btnOptions.Text = "Opcje";
            btnOptions.Location = new Point(30, 120);
            btnOptions.Size = new Size(140, 30);
            btnOptions.Click += (s, e) =>
            {
                string inputLives = Interaction.InputBox("Podaj liczbę żyć (1-10):", "Opcje", defaultLives.ToString());
                if (int.TryParse(inputLives, out int parsedLives) && parsedLives >= 1 && parsedLives <= 10)
                    defaultLives = parsedLives;
                else
                    MessageBox.Show("Nieprawidłowa liczba żyć.");

                string inputPoints = Interaction.InputBox("Podaj punkty za klocek (10-1000):", "Opcje", pointsPerBlock.ToString());
                if (int.TryParse(inputPoints, out int parsedPoints) && parsedPoints >= 10 && parsedPoints <= 1000)
                    pointsPerBlock = parsedPoints;
                else
                    MessageBox.Show("Nieprawidłowa liczba punktów.");

                string inputLength = Interaction.InputBox("Podaj długość paletki (50-200):", "Opcje", paddleLength.ToString());
                if (int.TryParse(inputLength, out int parsedLength) && parsedLength >= 50 && parsedLength <= 200)
                    paddleLength = parsedLength;
                else
                    MessageBox.Show("Nieprawidłowa długość paletki.");
            };

            menuPanel.Controls.Add(btnOptions);

            btnExit = new Button();
            btnExit.Text = "Wyjście";
            btnExit.Location = new Point(30, 160);
            btnExit.Size = new Size(140, 30);
            btnExit.Click += (s, e) => Application.Exit();
            menuPanel.Controls.Add(btnExit);
        }

        private void StartNewGame()
        {


            string input = Interaction.InputBox("Podaj nazwę gracza:", "Nowa Gra");
            if (string.IsNullOrWhiteSpace(input))
                return;

            playerName = input;
            lives = defaultLives;
            score = 0;
            currentLevel = 1;


            menuPanel.Visible = false;
            StartGame();
            timer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Nic nie trzeba tu robić, jeśli nie używasz
        }


        private void StartGame()
        {
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;
            this.Paint += OnPaint;
            this.Focus(); // bardzo ważne – żąda fokusu, żeby działały klawisze
            paddle = new Rectangle(350, 500, paddleLength, 20);
            ball = new Rectangle(paddle.X + paddle.Width / 2 - 10, paddle.Y - 20, 20, 20);
            LoadLevel(currentLevel);

        }

        private void LoadLevel(int level)
        {
            blocks.Clear();
            if (level == 1)
            {
                for (int i = 0; i < 5; i++)
                    blocks.Add(new Rectangle(100 + i * 120, 150, 100, 30));
            }
            else if (level == 2)
            {
                for (int i = 0; i < 5; i++)
                    blocks.Add(new Rectangle(100 + i * 120, 150, 100, 30));
                for (int i = 0; i < 4; i++)
                    blocks.Add(new Rectangle(140 + i * 120, 190, 100, 30));
            }
            else if (level == 3)
            {
                for (int i = 0; i < 6; i++)
                    blocks.Add(new Rectangle(80 + i * 110, 150, 90, 30));
                for (int i = 0; i < 5; i++)
                    blocks.Add(new Rectangle(110 + i * 110, 190, 90, 30));
                for (int i = 0; i < 4; i++)
                    blocks.Add(new Rectangle(140 + i * 110, 230, 90, 30));
                blocks.RemoveAll(b => (b.X > 300 && b.X < 500) && b.Y == 190);
            }
            else if (level == 4)
            {
                for (int y = 150; y <= 210; y += 30)
                    for (int x = 60; x <= 660; x += 110)
                        blocks.Add(new Rectangle(x, y, 90, 30));
            }
            else 
            {
                timer.Stop();
                MessageBox.Show("Gratulacje! Wygrałeś wszystkie poziomy!");
                SaveScore();
                menuPanel.Visible = true;
                return;
            }

            ballSpeedX = (int)(ballSpeedX * 1.30);
            ballSpeedY = (int)(ballSpeedY * 1.30);
        }

        private void SaveScore()
        {
            string path = "ranking.txt";
            string line = $"{playerName}: {score} pkt";
            File.AppendAllLines(path, new[] { line });
        }

        private void ShowRanking()
        {
            string path = "ranking.txt";
            if (File.Exists(path))
            {
                var lines = File.ReadAllLines(path);
                var sorted = lines
                    .Select(line => new
                    {
                        Line = line,
                        Score = ExtractScore(line)
                    })
                    .OrderByDescending(x => x.Score)
                    .Select(x => x.Line)
                    .ToList();

                string content = string.Join(Environment.NewLine, sorted);
                MessageBox.Show(content, "Ranking");
            }
            else
            {
                MessageBox.Show("Brak wyników.", "Ranking");
            }
        }

        private int ExtractScore(string line)
        {
            // Oczekiwany format: "Gracz: 123 pkt"
            var parts = line.Split(':');
            if (parts.Length < 2) return 0;

            var scorePart = parts[1].Trim().Replace("pkt", "").Trim();
            int score;
            return int.TryParse(scorePart, out score) ? score : 0;
        }


        private void GameLoop(object sender, EventArgs e)
        {
            if (leftArrowDown && paddle.X > 0)
                paddle.X -= 8;
            if (rightArrowDown && paddle.X < this.ClientSize.Width - paddle.Width)
                paddle.X += 8;

            ball.X += ballSpeedX;
            ball.Y += ballSpeedY;

            if (ball.X <= 0 || ball.X >= this.ClientSize.Width - ball.Width)
                ballSpeedX = -ballSpeedX;

            if (ball.Y <= 0)
                ballSpeedY = -ballSpeedY;

            if (ball.IntersectsWith(paddle))
                ballSpeedY = -ballSpeedY;

            for (int i = 0; i < blocks.Count; i++)
            {
                if (ball.IntersectsWith(blocks[i]))
                {
                    ballSpeedY = -ballSpeedY;
                    blocks.RemoveAt(i);
                    score += pointsPerBlock;
                    break;
                }
            }

            if (blocks.Count == 0)
            {
                currentLevel++;
                StartGame();
            }

            if (ball.Y > this.ClientSize.Height)
            {
                lives--;
                if (lives <= 0)
                {
                    timer.Stop();
                    MessageBox.Show($"Koniec gry! Zdobyłeś {score} punktów.");
                    SaveScore();
                    menuPanel.Visible = true;
                    return;
                }
                else
                {
                    ball.X = 390;
                    ball.Y = 300;
                }
            }

            lblStatus.Text = $"Gracz: {playerName}  Punkty: {score}  Życia: {lives}  Poziom: {currentLevel}";
            this.Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.Blue, paddle);
            g.FillEllipse(Brushes.Red, ball);

            foreach (var block in blocks)
            {
                g.FillRectangle(Brushes.Green, block);
                g.DrawRectangle(Pens.Black, block);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                leftArrowDown = true;
            if (e.KeyCode == Keys.Right)
                rightArrowDown = true;

            if (e.KeyCode == Keys.Escape)
            {
                timer.Enabled = !timer.Enabled;
                lblStatus.Text = timer.Enabled ? "Wznawiam grę..." : "Gra zatrzymana (pauza)";
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                leftArrowDown = false;
            if (e.KeyCode == Keys.Right)
                rightArrowDown = false;
        }
    }
}
