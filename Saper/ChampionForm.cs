using Saper.Interfaces;
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
    public partial class ChampionForm : Form, IChampionForm
    {
        public ChampionForm()
        {
            InitializeComponent();
        }

        public event Action<string> OKClicked;

        /// <summary>
        /// Клик на кнопку OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OK_Button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text.Trim(' ')))
            {
                textBox1.BackColor = Color.LightCoral;
                MessageBox.Show("Введите имя!");
            }
            else 
            {
                if (textBox1.Text.Contains('|'))
                {
                    textBox1.BackColor = Color.LightCoral;
                    MessageBox.Show("Введен некорректный символ '|'!");
                }
                else
                {
                    OKClicked(textBox1.Text);
                    Close();
                }
            }
        }

        /// <summary>
        /// Обработка ввода текста
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.BackColor == Color.LightCoral) 
            {
                textBox1.BackColor = Color.White;
            }
        }
    }
}
