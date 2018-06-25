using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SQLtoXML
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
            string str = null;
            string node = null;
            string id = null;
            char[] any = { ' ', '(', ',' };
            bool b = false;
            foreach (string s in Etq(clean(sql)))
            {
                if (s.EndsWith(";") || s.Contains("PRIMARY KEY (") || s.Contains("CONSTRAINT"))
                {
                    b = false;
                }
                
                if (b)
                {
                    id = s.Substring(s.IndexOf(" ") + 1);
                    node = id.Substring(0, id.IndexOfAny(any, 0));
                    id = s.Substring(0, s.IndexOf(" "));
                    str += "<" + node + " id=\"" + id + "\">" + s + "</" + node + ">\n";
                }
                else
                    str += s + "\n";
                
                if (s.Contains("CREATE TABLE"))
                    b = true;
            }
            return str;
        }

        private string[] Etq(List<string> lst)
        {
            List<string> list = new List<string>();
            bool b = true;
            list.Add("<dbAdmin>");
            list.Add("<doSQL>");
            foreach (string s in lst)
            {
                if (s.Contains("END;") && b)
                {
                    list.RemoveAt(list.LastIndexOf("<doSQL>"));
                    list.RemoveAt(list.LastIndexOf("</doSQL>"));
                }
                if (s == "GO")
                {
                    list.RemoveAt(list.LastIndexOf("<doSQL>"));
                    list.Add("GO");
                    list.Add("<doSQL>");
                }
                else
                {
                    list.Add(s);
                    if (s.Contains("CREATE PROCEDURE") || s.Contains("CREATE FUNCTION"))
                        b = false;
                    if (s.EndsWith("END;"))
                        b = true;
                    if (s.EndsWith(";") && b)
                    {
                        list.Add("</doSQL>");
                        list.Add("<doSQL>");
                    }
                }
            }
            list.RemoveAt(list.LastIndexOf("<doSQL>"));
            list.Add("</dbAdmin>");
            return list.ToArray();
        }

        private List<string> clean(string[] sql)
        {
            List<string> list = new List<string>();
            string s = null;
            foreach(string str in sql)
            {
                s = str;
                if (s.Contains("--"))
                    s = s.Remove(s.IndexOf("--")).Trim();
                else
                    s = s.Trim();
                
                if (s != null && s != "" && s != "\n" && s != "\t")
                {
                    if (s.Contains("<") || s.Contains(">"))
                    {
                        s = s.Replace("<", "&lt;");
                        s = s.Replace(">", "&gt;");
                    }
                    list.Add(s);
                }
            }
            return list;
        }
    }
}
