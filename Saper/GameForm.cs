using Saper.Interfaces;
using Saper.Presenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saper
{
    public partial class GameForm : Form, IGameForm
    {
        MyButton[,] btnField;
        TextBox xtext;
        TextBox ytext;
        Label xlabel;
        Label ylabel;
        Label TimerLabel;
        Label FlagsRemain;
        Button AgainGameButton;
        Button StartGameButton;
        PictureBox FlagImage;

        public event Action<int, int> LeftClickEvent;
        public event Action<int, int> RightClickEvent;
        public event Action<Level> StartGameEvent;
        public event Action<int, int> StartCustomGameEvent;
        public event Action TimerTick;
        public event Action AgainClicked;

        public GameForm()
        {
            InitializeComponent();
        }

        private void DestroyOldField(int width, int height)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    btnField[i, j].Dispose();
                    btnField[i, j] = null;                    
                }
            }

            btnField = null;
        }

        private void AddUpperPanel() 
        {
            TimerLabel = new Label()
            {
                Size = new Size(50, 25),
                Location = new Point(0, 25),
            };
            Controls.Add(TimerLabel);

            AgainGameButton = new Button()
            {
                Size = new Size(60, 25),
                Text = "Еще раз"
            };
            AgainGameButton.Click += AgainGame_Click;
            Controls.Add(AgainGameButton);

            FlagsRemain = new Label();
            Controls.Add(FlagsRemain);

            FlagImage = new PictureBox()
            {
                Image = Properties.Resources.Флажок2,
                Size = new Size(40, 25),
                BackgroundImageLayout = ImageLayout.Center
            };
            Controls.Add(FlagImage);
        }

        private void AgainGame_Click(object sender, EventArgs e)
        {
            AgainClicked();
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
            }
        }

        public void CreateField(int width, int height)
        {
            if (!TimerLabel.Visible) 
            {
                TimerLabel.Visible = true;
                AgainGameButton.Visible = true;
                FlagsRemain.Visible = true;
                FlagImage.Visible = true;
            }

            AgainGameButton.Location = new Point((height * 25) - 60, 25);
            FlagsRemain.Location = new Point((height * 25) / 2, 25);
            FlagImage.Location = new Point((height * 25) / 2 - 40 , 20);

            if (btnField != null)
            {
                DestroyOldField(btnField.GetUpperBound(0) + 1, btnField.GetUpperBound(1) + 1);
            }
            if (xtext != null)
            {
                xtext.Dispose();
                xtext = null;
                ytext.Dispose();
                ytext = null;
                StartGameButton.Dispose();
                StartGameButton = null;
                xlabel.Dispose();
                xlabel = null;
                ylabel.Dispose();
                ylabel = null;
            }
            Opacity = 0;

            int X = 0;
            int Y = 50;
            btnField = new MyButton[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    btnField[i, j] = new MyButton(i, j)
                    {
                        Location = new Point(X, Y),
                        Size = new Size(25, 25),
                    };
                    btnField[i, j].MouseDown += chk_MouseDown;                  
                    btnField[i, j].FlatStyle = FlatStyle.Flat;
                    btnField[i, j].BackColor = Color.YellowGreen;
                    Controls.Add(btnField[i, j]);
                    X += 25;
                }
                X = 0;
                Y += 25;
            }

            Opacity = 100;
        }

        //Клик мышкой по полю (правая кнопка/левая кнопка)
        private void chk_MouseDown(object sender, MouseEventArgs e)
        {
            MyButton chk = sender as MyButton;

            if (chk != null)
            {
                int x = chk.XPosition;
                int y = chk.YPosition;

                if (e.Button == MouseButtons.Left)
                {
                    if (!timer1.Enabled) 
                    {
                        timer1.Enabled = true;
                    }
                    LeftClickEvent(x, y);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    RightClickEvent(x, y);
                }
            }
        }

        public void Boom(int x, int y)
        {
            timer1.Enabled = false;
            btnField[x, y].BackColor = Color.Red;
            MessageBox.Show($"Поздравляем! Вы успешно подорвались на клетке {y + 1}, {x + 1}!");
        }

        public void Mark(int x, int y)
        {
            btnField[x, y].BackgroundImage = Properties.Resources.Флажок2;
            btnField[x, y].BackgroundImageLayout = ImageLayout.Stretch;
        }

        public void OpenBorder(int x, int y, int minesCount)
        {
            btnField[x, y].BackColor = Color.White;
            btnField[x, y].Text = minesCount.ToString();
        }

        public void OpenEmpty(int x, int y)
        {
            btnField[x, y].BackColor = Color.White;
        }

        public void UnMark(int x, int y)
        {
            btnField[x, y].BackgroundImage.Dispose();
            btnField[x, y].BackgroundImage = null;
        }

        public void BlockField()
        {
            for (int i = 0; i < btnField.GetLength(0); i++)
            {
                for (int j = 0; j < btnField.GetLength(1); j++)
                {
                    btnField[i, j].Enabled = false;
                }
            }
        }

        public void Victory()
        {
            timer1.Enabled = false;
            MessageBox.Show("Вы победили!");
        }

        private void легкаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
            }
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item.Checked)
            {
                StartGameEvent(Level.Легкая);
                средняяToolStripMenuItem.CheckState = CheckState.Unchecked;
                тяжелаяToolStripMenuItem.CheckState = CheckState.Unchecked;
                индивидуальнаяToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void средняяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
            }
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item.Checked)
            {
                StartGameEvent(Level.Средняя);
                легкаяToolStripMenuItem.CheckState = CheckState.Unchecked;
                тяжелаяToolStripMenuItem.CheckState = CheckState.Unchecked;
                индивидуальнаяToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void тяжелаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
            }
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item.Checked)
            {
                StartGameEvent(Level.Тяжелая);
                средняяToolStripMenuItem.CheckState = CheckState.Unchecked;
                легкаяToolStripMenuItem.CheckState = CheckState.Unchecked;
                индивидуальнаяToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }

        private void индивидуальнаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
            }

            TimerLabel.Visible = false;
            AgainGameButton.Visible = false;
            FlagsRemain.Visible = false;
            FlagImage.Visible = false;

            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item.Checked)
            {
                средняяToolStripMenuItem.CheckState = CheckState.Unchecked;
                легкаяToolStripMenuItem.CheckState = CheckState.Unchecked;
                тяжелаяToolStripMenuItem.CheckState = CheckState.Unchecked;
            }

            CreateCustomField();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AddUpperPanel();
            StartGameEvent(Level.Легкая);
        }

        public void CreateCustomField()
        {
            if (btnField != null)
            {
                DestroyOldField(btnField.GetUpperBound(0) + 1, btnField.GetUpperBound(1) + 1);
            }

            xtext = new TextBox()
            {
                Location = new Point(20, 50)
            };
            ytext = new TextBox()
            {
                Location = new Point(20, 100)
            };
            xlabel = new Label()
            {
                Location = new Point(20, 30),
                Text = "8 >= Ширина <= 30)",
                Width = 150
            };
            ylabel = new Label()
            {
                Location = new Point(20, 80),
                Text = "8 >= Высота <= 30)",
                Width = 150
            };
            StartGameButton = new Button()
            {
                Location = new Point(200, 100 - 20),
                Size = new Size(50, 50),
                Text = "Старт!"
            };
            StartGameButton.Click += StartGame_Click;
            xtext.Click += Xtext_Click; ytext.Click += Ytext_Click;
            Controls.Add(xtext);
            Controls.Add(ytext);
            Controls.Add(xlabel);
            Controls.Add(ylabel);
            Controls.Add(StartGameButton);
        }

        private void Ytext_Click(object sender, EventArgs e)
        {
            ytext.BackColor = Color.White;
        }

        private void Xtext_Click(object sender, EventArgs e)
        {
            xtext.BackColor = Color.White;
        }

        private void StartGame_Click(object sender, EventArgs e)
        {
            if (int.TryParse(xtext.Text, out int x) && int.TryParse(ytext.Text, out int y))
            {
                if ((x <= 30 && x >= 8) && (y <= 30 && y >= 8))
                {
                    StartCustomGameEvent(x, y);
                }
                else
                {
                    MessageBox.Show("Введены некорректные данные!");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(xtext.Text))
                {
                    xtext.BackColor = Color.Coral;
                }
                if (string.IsNullOrEmpty(ytext.Text))
                {
                    ytext.BackColor = Color.Coral;
                }
                MessageBox.Show("Введены некорректные данные!");
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Сапер... Просто сапер.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void рекордыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecordsForm Winners = new RecordsForm();
            Winners.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimerTick();
        }

        public void UpgradeTimer(int count)
        {
            TimerLabel.Text = count.ToString("D5");
        }

        public void AddNewRecord(string difficulty, string timerCount, string date)
        {
            ChampionForm Form = new ChampionForm();
            ChampionPresenter Presenter = new ChampionPresenter(Form, difficulty, timerCount, date);
            Form.ShowDialog();
        }

        public void UpgradeFlagsCounter(int count)
        {
            FlagsRemain.Text = $"= {count}";
        }
    }
}
