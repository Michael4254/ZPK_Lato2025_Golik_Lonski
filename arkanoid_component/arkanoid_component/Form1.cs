using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Arkanoid_biblioteka_komponentow;
// Alias, żeby Timer oznaczał WinFormsowy Timer
using Timer = System.Windows.Forms.Timer;

namespace arkanoid_component
{
    public partial class Form1 : Form
    {
        private PaddleControl paddle;
        private BallControl ball;
        private List<BrickControl> bricks = new();
        private List<PowerUpControl> powerUps = new();
        private Random rnd = new Random();
        private Timer gameTimer;

        private int currentLevel = 1;
        private int score = 0;
        private int lives = OptionsManager.Lives;
        private int blockPoints = OptionsManager.PointsPerBrick;

        private Label livesLabel;
        private Label scoreLabel;

        private const int BrickCols = 5;
        private const int BrickSpacing = 10;
        private const int BrickHeight = 24;

        public Form1()
        {
            InitializeComponent();
            ConfigureWindow();
            InitializeComponents();
            SetupInput();
            StartLevel();
            StartTimer();
        }

        private void ConfigureWindow()
        {
            ClientSize = new Size(700, 425);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
        }

        private void InitializeComponents()
        {
            // paletka
            paddle = new PaddleControl
            {
                Name = "paddleControl1",
                Location = new Point((ClientSize.Width - 80) / 2, ClientSize.Height - 75)
            };
            Controls.Add(paddle);

            // kulka
            ball = new BallControl { Name = "ballControl1" };
            Controls.Add(ball);

            // żyć label
            livesLabel = new Label
            {
                Text = $"Życia: {lives}",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.Transparent
            };
            Controls.Add(livesLabel);

            // punkty label
            scoreLabel = new Label
            {
                Text = $"Punkty: {score}",
                Location = new Point(ClientSize.Width - 120, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.Transparent
            };
            Controls.Add(scoreLabel);
        }

        private void SetupInput()
        {
            KeyPreview = true;
            Load += (s, e) =>
            {
                ActiveControl = null;
                Focus();
            };
        }

        private void StartLevel()
        {
            // usuń stare cegły i bonusy
            foreach (var c in Controls.OfType<BrickControl>().ToArray()) Controls.Remove(c);
            foreach (var p in Controls.OfType<PowerUpControl>().ToArray()) Controls.Remove(p);
            bricks.Clear();
            powerUps.Clear();

            // na początku gry zeruj punkty
            if (currentLevel == 1) score = 0;
            UpdateScoreLabel();

            // reset paletki i kulki
            paddle.Left = (ClientSize.Width - paddle.Width) / 2;
            paddle.Top = ClientSize.Height - 75;
            ball.Left = paddle.Left + (paddle.Width - ball.Width) / 2;
            ball.Top = paddle.Top - ball.Height - 1;

            int rows = currentLevel switch
            {
                1 => 1,
                2 => 5,
                3 => 5,
                _ => currentLevel
            };

            int totalSpacing = (BrickCols + 1) * BrickSpacing;
            int brickWidth = (ClientSize.Width - totalSpacing) / BrickCols;
            int startY = 50;

            foreach (int r in Enumerable.Range(0, rows))
                foreach (int c in Enumerable.Range(0, BrickCols))
                {
                    bool place = currentLevel switch
                    {
                        1 => r == 0,
                        2 => Math.Abs(r - 2) + Math.Abs(c - 2) <= 2,
                        3 => r == c || r + c == BrickCols - 1,
                        _ => true
                    };
                    if (!place) continue;

                    var brick = new BrickControl
                    {
                        Width = brickWidth,
                        Height = BrickHeight,
                        HitPoints = 1,
                        Location = new Point(
                            BrickSpacing + c * (brickWidth + BrickSpacing),
                            startY + r * (BrickHeight + BrickSpacing))
                    };
                    brick.BrickDestroyed += (s, e) =>
                    {
                        Controls.Remove(brick);
                        bricks.Remove(brick);
                        ball.ReflectY();

                        // 10% szansy na bonus
                        if (rnd.NextDouble() < 0.2)
                        {
                            var pu = new PowerUpControl
                            {
                                Type = (PowerUpType)rnd.Next(0, 3),
                                Location = brick.Location
                            };
                            Controls.Add(pu);
                            powerUps.Add(pu);
                        }

                        score += blockPoints;
                        UpdateScoreLabel();

                        if (bricks.Count == 0)
                        {
                            gameTimer.Stop();
                            MessageBox.Show($"Poziom {currentLevel} ukończony!");

                            if (currentLevel == 3)
                            {
                                var nick = Prompt.ShowDialog("Podaj swój nick:", "Koniec gry");
                                if (!string.IsNullOrEmpty(nick))
                                    RankingManager.AddScore(new ScoreEntry
                                    {
                                        Nickname = nick,
                                        Points = score,
                                        Date = DateTime.Now
                                    });
                                Close();
                            }
                            else
                            {
                                currentLevel++;
                                StartLevel();
                                UpdateLivesLabel();
                                StartTimer();
                            }
                        }
                    };

                    bricks.Add(brick);
                    Controls.Add(brick);
                }
        }

        private void StartTimer()
        {
            gameTimer = new Timer { Interval = 16 };
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
        }
        
        private void GameLoop(object? sender, EventArgs e)
        {
            // 1) Przesuń kulkę
            ball.Move();

            // 2) Zbieranie PowerUp
            foreach (var pu in powerUps.ToArray())
            {
                // jeśli bonus wypadł poza dół → usuń
                if (pu.Top > ClientSize.Height)
                {
                    Controls.Remove(pu);
                    powerUps.Remove(pu);
                    continue;
                }

                // kolizja z paletką?
                if (pu.Bounds.IntersectsWith(paddle.Bounds))
                {
                    pu.Stop();
                    Controls.Remove(pu);
                    powerUps.Remove(pu);

                    switch (pu.Type)
                    {
                        case PowerUpType.ExpandPaddle:
                            {
                                // zapamiętaj oryginalną szerokość
                                int originalWidth = paddle.Width;
                                // powiększ paletkę
                                paddle.Width = Math.Min(ClientSize.Width, originalWidth * 2);

                                // po 30 sekundach przywróć szerokość
                                var resetTimer = new System.Windows.Forms.Timer { Interval = 15_000 };
                                resetTimer.Tick += (ts, te) =>
                                {
                                    resetTimer.Stop();
                                    paddle.Width = originalWidth;
                                    resetTimer.Dispose();
                                };
                                resetTimer.Start();
                                break;
                            }
                        case PowerUpType.ExtraLife:
                            {
                                lives++;
                                UpdateLivesLabel();
                                break; // bonus stały, nie trzeba resetować
                            }
                        case PowerUpType.SlowBall:
                            {
                                // zapamiętaj oryginalne prędkości
                                float origX = ball.VelocityX;
                                float origY = ball.VelocityY;
                                // zwolnij kulkę
                                ball.VelocityX *= 0.5f;
                                ball.VelocityY *= 0.5f;

                                // po 30 sekundach przywróć prędkości
                                var resetTimer = new System.Windows.Forms.Timer { Interval = 10_000 };
                                resetTimer.Tick += (ts, te) =>
                                {
                                    resetTimer.Stop();
                                    ball.VelocityX = origX;
                                    ball.VelocityY = origY;
                                    resetTimer.Dispose();
                                };
                                resetTimer.Start();
                                break;
                            }
                    }
                }
            }

            // 3) Odbicia od krawędzi
            if (ball.Left <= 0 || ball.Right >= ClientSize.Width) ball.ReflectX();
            if (ball.Top <= 0) ball.ReflectY();

            // 4) Kolizja z paletką
            if (ball.Bounds.IntersectsWith(paddle.Bounds))
            {
                ball.Top = paddle.Top - ball.Height;
                ball.ReflectY();
            }

            // 5) Kolizja z cegłami
            foreach (var brick in bricks.ToArray())
            {
                if (ball.Bounds.IntersectsWith(brick.Bounds))
                {
                    brick.Hit();
                    return; // jedno odbicie na tick
                }
            }

            // 6) Dolna krawędź → tracimy życie / koniec gry
            if (ball.Bottom >= ClientSize.Height)
            {
                gameTimer.Stop();
                lives--;
                UpdateLivesLabel();

                if (lives > 0)
                {
                    paddle.Left = (ClientSize.Width - paddle.Width) / 2;
                    ball.Left = paddle.Left + (paddle.Width - ball.Width) / 2;
                    ball.Top = paddle.Top - ball.Height - 1;
                    StartTimer();
                }
                else
                {
                    MessageBox.Show("Koniec gry");
                    var nick = Prompt.ShowDialog("Podaj swój nick:", "Koniec gry");
                    if (!string.IsNullOrEmpty(nick))
                        RankingManager.AddScore(new ScoreEntry
                        {
                            Nickname = nick,
                            Points = score,
                            Date = DateTime.Now
                        });
                    Close();
                }
            }
        }


        private void UpdateLivesLabel()
        {
            livesLabel.Text = $"Życia: {lives}";
        }

        private void UpdateScoreLabel()
        {
            scoreLabel.Text = $"Punkty: {score}";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left)
            {
                paddle.MoveLeft();
                return true;
            }
            if (keyData == Keys.Right)
            {
                paddle.MoveRight();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
