using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Алгоритм_Хаффмана
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // создание объекта формы
            Form2 newForm = new Form2();
            // открытие формы
            newForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // создание объекта формы
            Form3 newForm1 = new Form3();
            // открытие формы
            newForm1.Show();
        }
    }
}
