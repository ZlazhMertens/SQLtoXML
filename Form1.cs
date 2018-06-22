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
        private string sql;
        
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
                sql = File.ReadAllText(textBox1.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, ToSQL(sql));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string ToSQL(string sql)
        {
            string s = null;
            string str = null;
            foreach (char c in sql)
            {
                s += c;
                if (c == ';')
                {
                    str += "\n<doSQL>" + s + "</doSQL>\n";
                    s = null;
                }
            }
            return str;
        }
    }
}
