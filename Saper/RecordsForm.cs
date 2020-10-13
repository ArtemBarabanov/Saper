using Saper.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saper
{
    public partial class RecordsForm : Form
    {
        DataGridView RecordsGridView;
        Label InfoLabel;
        public RecordsForm()
        {
            InitializeComponent();
            Focus();
        }

        /// <summary>
        /// Загрузка данных из файла Records.txt, если такой будет в директории с игрой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecordsForm_Load(object sender, EventArgs e)
        {
            var records = (from record in Records.LoadRecordsList() orderby record.Difficulty select record).ToList();
            if (records != null && records.Count != 0)
            {
                RecordsGridView = new DataGridView() 
                {
                AutoSize = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                Enabled = false,
                DataSource = records,
                BackgroundColor = Color.White,
                CellBorderStyle = DataGridViewCellBorderStyle.None,
                };
               
                Controls.Add(RecordsGridView);
                RecordsGridView.ClearSelection();
            }
            else 
            {
                InfoLabel = new Label()
                {
                    Text = "Нет данных!",
                    Size = new Size(100, 20),
                    Font = new Font("Times New Roman", 12),
                    Location = new Point((Width / 2) - 50, (Height / 2) - 30)                  
                };
                Controls.Add(InfoLabel);
            }
        }
    }
}
