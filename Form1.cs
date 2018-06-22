using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ConversorSQLtoXML
{
    public partial class Form1 : Form
    {
        private string[] sql;
        
        public Form1()
        {
            InitializeComponent();
            sql = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text=openFileDialog1.FileName;
                sql = File.ReadAllLines(textBox1.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, ToXML(sql));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string ToXML(string[] sql)
        {
            string str = "<dbAdmin>\n<doSQL>";
            foreach (string s in sql)
            {
                if (s.Contains("--"))
                    str += s.Remove(s.IndexOf("--"));
                else
                    str += s;
                if (s.Contains(";"))
                    str += "</doSQL>\n<doSQL>";
            }
            str = str.Remove(str.Length-7);
            str += "</dbAdmin>";
            return str;
        }
    }
}
