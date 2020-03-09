using System;
using System.Collections.Generic;
using System.Collections;
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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        // список для частоты встречаемости символов
        public static Dictionary<char, int> dict = new Dictionary<char, int>();
        // вспомогательный лист
        public static List<TreeNode> sup = new List<TreeNode>();
        // лист для кодов символов
        public static List<TreeNode> codes = new List<TreeNode>();
        // дерево
        public static List<TreeNode> tree = new List<TreeNode>();

        // структура узла дерева
        public struct TreeNode
        {
            // символы
            public string text;
            // двоичный код
            public string code;
            // частота встречаемости
            public int frequency;
            // коструктор структуры
            public TreeNode(string t, string c, int f)
            {
                text = t;
                code = c;
                frequency = f;
            }
        };

        // сортировка дерева частот
        static void sort(List<TreeNode> tree)
        {
            // пузырьковая сортировка
            for (int i = 0; i < tree.Count - 1; i++)
            {
                for (int j = i; j < tree.Count; j++)
                {
                    if (tree[i].frequency < tree[j].frequency)
                    {
                        TreeNode temp = tree[i];
                        tree[i] = tree[j];
                        tree[j] = temp;
                    }
                }
            }
        }

        static void TreeFileWriter(string path, string TreePath)
        {
            if (!File.Exists(TreePath))
            {
                // создание файла для дерева
                var myFile = File.Create(TreePath);
                myFile.Close();
            }
            // запись дерева в файл
            StreamWriter TreeWriter = new StreamWriter(TreePath);
            foreach (TreeNode item in codes)
            {
                TreeWriter.WriteLine(item.text + "" + item.code);
            }
            TreeWriter.Close();
        }

        static void Encoding(ref string encode, string zacode, string subCode)
        {
            // проверка на целое кол-во байт
            if (zacode.Length % 8 == 0)
            {
                for (int i = 0; i < zacode.Length - 7; i += 8)
                {
                    int CharCode = 0;
                    for (int j = 0; j < 8; j++)
                    {
                        if (zacode[i + j] == '1')
                        {
                            CharCode += Convert.ToInt32(Math.Pow(2, 7 - j));
                        }
                    }
                    encode += (char)CharCode;
                }
            }
            else if (zacode.Length % 8 != 0)
            {
                for (int i = 0; i <= zacode.Length; i += 8)
                {
                    int CharCode = 0;
                    if (i >= zacode.Length - 7)
                    {
                        // перевод для остатка бит
                        for (int j = 0; j < zacode.Length - i; j++)
                        {
                            subCode += zacode[i + j];
                        }
                        for (int j = 0; j < subCode.Length; j++)
                        {
                            if (subCode[j] == '1')
                            {
                                CharCode += Convert.ToInt32(Math.Pow(2, 7 - j));
                            }
                        }
                        encode += (char)CharCode;
                        break;
                    }
                    else
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (zacode[i + j] == '1')
                            {
                                CharCode += Convert.ToInt32(Math.Pow(2, 7 - j));
                            }
                        }
                        encode += (char)CharCode;
                    }
                }
            }
        }

        static void HuffmanAlgorithm()
        {
            // если мощность алфавита равна 1
            if (tree.Count == 1)
            {
                codes[0] = new TreeNode(codes[0].text, "0" + codes[0].code, codes[0].frequency);
            }

            // получение кодов для каждого символа
            while (tree.Count > 1)
            {
                // сортировка
                sort(tree);

                for (int i = 0; i < sup.Count; i++)
                {
                    if (tree[tree.Count - 2].text.Contains(sup[i].text))
                    {
                        // добавление 0 к коду
                        codes[i] = new TreeNode(codes[i].text, "0" + codes[i].code, codes[i].frequency);
                    }
                    else if (tree[tree.Count - 1].text.Contains(sup[i].text))
                    {
                        // добавление 1 к коду
                        codes[i] = new TreeNode(codes[i].text, "1" + codes[i].code, codes[i].frequency);
                    }
                }

                // создается новый узел с частой и именами двух прошлых узлов 
                tree[tree.Count - 2] = new TreeNode(tree[tree.Count - 2].text + tree[tree.Count - 1].text, "",
                    tree[tree.Count - 2].frequency + tree[tree.Count - 1].frequency);
                // удаление наименьшего узла
                tree.RemoveAt(tree.Count - 1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            // считывание пути
            string path = InputtextBox1.Text;
            // считывание содержимого файла
            StreamReader Reader = new StreamReader(path);
            string str = "";
            str = Reader.ReadToEnd();
            Reader.Close();
            // исходная память файла
            long lengthInput = new FileInfo(path).Length;

            StreamWriter Writer = new StreamWriter(path);

            // подсчет частоты символов
            for (int i = 0; i < str.Length; i++)
            {
                if (dict.ContainsKey(str[i]))
                {
                    dict[str[i]] += 1;
                }
                else
                {
                    dict.Add(str[i], 0);
                    i--;
                }
            }
          
            // начальное заполнение листов
            foreach (KeyValuePair<char, int> item in dict)
            {
                sup.Add(new TreeNode(item.Key.ToString(), "", item.Value));
                tree.Add(new TreeNode(item.Key.ToString(), "", item.Value));
                codes.Add(new TreeNode(item.Key.ToString(), "", item.Value));
            }

            // Построение дерева Хаффмана
            HuffmanAlgorithm();

            //заполнение дерева в форме
            for (int i = 0; i < codes.Count; i++)
            {
                treeView1.Nodes.Add(codes[i].text + " - " + codes[i].code);
            }

            // создание адреса файла
            int index = path.LastIndexOf('.');
            string TreePath = path.Insert(index, "Tree");
            // Запись дерева в файл
            TreeFileWriter(path, TreePath);

            // запись сообщение в двоичном виде по полученым кодам
            string zacode = "";
            for (int i = 0; i < str.Length; i++)
            {
                foreach (TreeNode item in codes)
                {
                    if (Convert.ToString(str[i]) == item.text)
                    {
                        zacode += item.code;
                    }
                }
            }

            // перевод из двоичного кода в символы
            string encode = "";
            string subCode = "";
            Encoding(ref encode, zacode, subCode);

            Writer.Write(encode);

            // закрытие потоков
            Writer.Close();

            // память
            long lengthOutput = new FileInfo(path).Length;
            long TreeLength = new FileInfo(TreePath).Length;
            double div = Convert.ToDouble(lengthOutput) / Convert.ToDouble(lengthInput);

            textBox1.Text = Convert.ToString(lengthInput);
            textBox2.Text = Convert.ToString(lengthOutput);
            textBox3.Text = Convert.ToString(TreeLength);
            textBox4.Text = Convert.ToString(Math.Round( div, 2 ) * 100 + "%");
        }

        private void button2_Click(object sender, EventArgs e)
        {

            openFileDialog1.ShowDialog();
            InputtextBox1.Text =  openFileDialog1.FileName;
        }
    }
}
