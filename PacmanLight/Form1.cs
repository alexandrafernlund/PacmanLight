namespace PacmanLight
{
    enum Direction
    {
        North, East, South, West
    }

    public struct Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public partial class Form1 : Form
    {
        TableLayoutPanel tpanel = new TableLayoutPanel();
        Panel panel;
        Panel[,] panelArray = new Panel[15, 15];
        Position oldPosition = new Position(0, 0);
        Position newPosition;
        Position enemyOld = new Position(14, 14);
        Position enemyNew;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        bool gameOver = false;
        public Form1()
        {
            InitializeComponent();
            timer.Interval = 60;
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < panelArray.GetLength(0); i++)
            {
                for (int j = 0; j < panelArray.GetLength(1); j++)
                {
                    panel = new Panel();
                    panel.Size = new Size(30, 30);
                    if (i == 0 && j == 0) panel.BackColor = Color.Green;
                    else panel.BackColor = Color.Gray;
                    tpanel.Controls.Add(panel, i, j);
                    panelArray[i, j] = panel;
                }
            }
            Controls.Add(tpanel);
            tpanel.Dock = DockStyle.Fill;
            panelArray[enemyOld.X, enemyOld.Y].BackColor = Color.Red;
            Size = new Size(563, 596);

            Random random = new Random();
            int yellowX, yellowY;
            do
            {
                yellowX = random.Next(panelArray.GetLength(0));
                yellowY = random.Next(panelArray.GetLength(1));
            }
            while (panelArray[yellowX, yellowY].BackColor != Color.Gray || panelArray[yellowX, yellowY].BackColor == Color.White);

            panelArray[yellowX, yellowY].BackColor = Color.Yellow;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            AddWhiteBlock();
        }

        private void AddWhiteBlock()
        {
            Random random = new Random();
            int x, y;

            do
            {
                x = random.Next(panelArray.GetLength(0));
                y = random.Next(panelArray.GetLength(1));
            }
            while (panelArray[x, y].BackColor != Color.Gray && panelArray[x, y].BackColor != Color.Yellow);

            if (panelArray[x, y].BackColor == Color.Yellow)
            {
                return;
            }

            if (panelArray[x, y].BackColor != Color.White && panelArray[x, y].BackColor != Color.Red && panelArray[x, y].BackColor != Color.Green)
            {
                panelArray[x, y].BackColor = Color.White;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) move(Direction.North);
            if (e.KeyCode == Keys.Down) move(Direction.South);
            if (e.KeyCode == Keys.Left) move(Direction.West);
            if (e.KeyCode == Keys.Right) move(Direction.East);
        }

        private void move(Direction d)
        {
            newPosition = oldPosition;
            switch (d)
            {
                case Direction.North:
                    if (oldPosition.Y != 0)
                    {
                        newPosition.Y -= 1;
                    }
                    else return;
                    break;
                case Direction.South:
                    if (oldPosition.Y < panelArray.GetLength(1) - 1)
                    {
                        newPosition.Y += 1;
                    }
                    else return;
                    break;
                case Direction.West:
                    if (oldPosition.X != 0)
                    {
                        newPosition.X -= 1;
                    }
                    else return;
                    break;
                case Direction.East:
                    if (oldPosition.X < panelArray.GetLength(0) - 1)
                    {
                        newPosition.X += 1;
                    }
                    else return;
                    break;
            }

            if (panelArray[newPosition.X, newPosition.Y].BackColor == Color.Yellow)
            {
                MessageBox.Show("Congratulations! You won the game by reaching the yellow block.", "Game Over", MessageBoxButtons.OK);
                ResetGame();
                return;
            }

            if (panelArray[newPosition.X, newPosition.Y].BackColor == Color.White)
            {
                MessageBox.Show("Game Over! You collided with a white block.");
                gameOver = true;
                ResetGame();
            }
            else
            {
                panelArray[oldPosition.X, oldPosition.Y].BackColor = Color.Gray;
                panelArray[newPosition.X, newPosition.Y].BackColor = Color.Green;
                oldPosition = newPosition;

                CheckGameOver();

                if (!gameOver)
                {
                    while (!moveEnemy()) ;
                }
            }
        }

        private bool moveEnemy()
        {
            Random r = new Random();
            Direction d = (Direction)r.Next(4);
            enemyNew = enemyOld;
            switch (d)
            {
                case Direction.North:
                    if (enemyOld.Y != 0)
                    {
                        enemyNew.Y -= 1;
                        break;
                    }
                    else return false;
                case Direction.South:
                    if (enemyOld.Y < panelArray.GetLength(1) - 1)
                    {
                        enemyNew.Y += 1;
                        break;
                    }
                    else return false;
                case Direction.West:
                    if (enemyOld.X != 0)
                    {
                        enemyNew.X -= 1;
                        break;
                    }
                    else return false;
                case Direction.East:
                    if (enemyOld.X < panelArray.GetLength(0) - 1)
                    {
                        enemyNew.X += 1;
                        break;
                    }
                    else return false;
            }
            panelArray[enemyOld.X, enemyOld.Y].BackColor = Color.Gray;
            panelArray[enemyNew.X, enemyNew.Y].BackColor = Color.Red;
            enemyOld = enemyNew;
            CheckGameOver();
            return true;
        }

        private void CheckGameOver()
        {
            if (oldPosition.X == enemyOld.X && oldPosition.Y == enemyOld.Y)
            {
                MessageBox.Show("Game Over! You collided with the enemy.");
                gameOver = true;
                ResetGame();
            }
        }

        private void ResetYellowBlock()
        {
            for (int i = 0; i < panelArray.GetLength(0); i++)
            {
                for (int j = 0; j < panelArray.GetLength(1); j++)
                {
                    if (panelArray[i, j].BackColor == Color.Yellow)
                    {
                        panelArray[i, j].BackColor = Color.Gray;
                        break;
                    }
                }
            }

            Random random = new Random();
            int yellowX, yellowY;
            do
            {
                yellowX = random.Next(panelArray.GetLength(0));
                yellowY = random.Next(panelArray.GetLength(1));
            } while (panelArray[yellowX, yellowY].BackColor != Color.Gray);

            panelArray[yellowX, yellowY].BackColor = Color.Yellow;
        }
        private void ResetGame()
        {
            panelArray[oldPosition.X, oldPosition.Y].BackColor = Color.Gray;

            oldPosition = new Position(0, 0);
            panelArray[oldPosition.X, oldPosition.Y].BackColor = Color.Green;

            for (int i = 0; i < panelArray.GetLength(0); i++)
            {
                for (int j = 0; j < panelArray.GetLength(1); j++)
                {
                    if (i == 0 && j == 0)
                    {
                        panelArray[i, j].BackColor = Color.Green;
                    }
                    else
                    {
                        if (panelArray[i, j].BackColor == Color.White)
                        {
                            panelArray[i, j].BackColor = Color.Gray;
                        }
                        else if (panelArray[i, j].BackColor == Color.Red)
                        {
                            panelArray[i, j].BackColor = Color.Gray;
                            enemyOld = new Position(i, j);
                        }
                    }
                }
            }
            ResetYellowBlock();
            gameOver = false;
        }

    }
}