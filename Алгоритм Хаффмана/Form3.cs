using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Алгоритм_Хаффмана
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            InputtextBox1.Text = openFileDialog1.FileName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
            textBox1.Text = openFileDialog2.FileName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // считывание путей
            string path = InputtextBox1.Text;
            string PathTree = textBox1.Text;
            // открытие потоков
            StreamReader TreeReader = new StreamReader(PathTree);
            StreamReader CodeReader = new StreamReader(path);
            // считывание содержимого файла
            string code = CodeReader.ReadToEnd();

            // переводит закодированное сообщение в юникод. Между элементами массива 0
            byte[] result = System.Text.Encoding.Unicode.GetBytes(code);
            // новый массив для элементов без 0
            char[] c = new char[result.Length / 2];
            // убираем 0
            int x = 0;
            for (int i = 0; i < result.Length; i++)
            {
                if (i < result.Length - 1)
                {
                    if (result[i] != 0 && result[i + 1] == 0)
                    {
                        c[x] = Convert.ToChar(result[i]);
                        if (x < c.Length - 1)
                        {
                            x++;
                        }
                    }
                    else if (result[i] == 0 && result[i + 1] == 0)
                    {
                        c[x] = Convert.ToChar(result[i]);
                        if (x < c.Length - 1)
                        {
                            x++;
                        }
                    }
                }
            }

            // подолняем все коды до 8 знаков
            string CodeByte = "";
            string f = "";
            for (int i = 0; i < c.Length; i++)
            {
                // перевод в двоичную систему
                f = Convert.ToString(c[i],2);
                if (f.Length < 8)
                {
                    for (int j = f.Length; j < 8; j++)
                    {
                        f = "0" + f;
                    }
                }
                CodeByte += f;
            }

            // список с символами и их кодами
            Dictionary<char, string> dict = new Dictionary<char, string>();

            // записываем словарь с символами и их кодами
            string line;
            while ((line = TreeReader.ReadLine()) != null)
            {
                dict.Add(line[0], line.Substring(1, line.Length - 1));
            }

            // декодируем из бинарного кода в символы
            string DecodeLine = "";
            string SubLine = "";
            for (int i = 0; i < CodeByte.Length; i++)
            {
                SubLine += CodeByte[i];
                // поиск подстроки в списке
                if (dict.ContainsValue(SubLine))
                {
                    foreach (KeyValuePair<char, string> item in dict)
                    {
                        if (item.Value == SubLine)
                        {
                            DecodeLine += item.Key;
                            SubLine = "";
                            continue;
                        }
                    }
                }
            }

            CodeReader.Close();
            // запись сообщения в файл и форму
            StreamWriter CodeWriter = new StreamWriter(path);
            CodeWriter.Write(DecodeLine);
            richTextBox1.Text += DecodeLine;
            // закрытие потоков
            CodeWriter.Close();
            TreeReader.Close();
        }
    }
}
