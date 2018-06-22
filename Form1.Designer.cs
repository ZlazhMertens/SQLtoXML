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

        private List<string> Etq(string[] sql)
        {
            List<string> list = new List<string>();
            list.Add("<dbAdmin>");
            list.Add("<doSQL>");
            foreach (string s in sql)
            {
                if (s.Contains("--"))
                    list.Add(s.Remove(s.IndexOf("--")).TrimStart(' ').TrimEnd(' '));
                else
                    list.Add(s.TrimStart(' ').TrimEnd(' '));
                if (s.Contains(";"))
                {
                    list.Add("</doSQL>");
                    list.Add("<doSQL>");
                }
            }
            list.RemoveAt(list.LastIndexOf("<doSQL>"));
            list.Add("</dbAdmin>");
            return list;
        }

        private string ToXML(string[] sql)
        {
            string str = null;
            string[] astr = Etq(sql).ToArray();
            foreach(string s in astr)
            {
                str += s + "\n";
            }
            return str;
        }
    }
}

