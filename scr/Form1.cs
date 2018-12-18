using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace xml
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class Student
        {
            public string name { get; set; } // ФИО
            public int id { set; get; }     // номер зачетки
            public string group { set; get; }    // название группы
            public List<int> marks_list;  // список оценок 2-5
            public double markS { get; set; }
        }

        List<Student> ucheniki = new List<Student>();
        public class Group
        {
            public string name_group { get; set; }
            public List<int> marks = new List<int>();
            public double avMarks { get; set; }
        }
        List<Group> groups = new List<Group>();
        int[] raspredelenie = new int[4];

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Files XML (*.xml)|*.xml";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string filename = dlg.FileName;
            try
            {
                XmlTextReader reader = new XmlTextReader(filename);
                Student bedolaga;
                reader.WhitespaceHandling = WhitespaceHandling.None;
                bool mstart = false;
                bedolaga = null;
                Group a = new Group();
                a.name_group = "381607-1";
                Group b = new Group();
                b.name_group = "381607-2";
                Group c = new Group();
                c.name_group = "381607-3";
                Group temp = null;
                raspredelenie[0] = 0;
                raspredelenie[1] = 0;
                raspredelenie[2] = 0;
                raspredelenie[3] = 0;
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "stud")
                        {
                            bedolaga = new Student();
                            bedolaga.marks_list = new List<int>();
                            bedolaga.name = reader.GetAttribute("name");
                            bedolaga.id = Convert.ToInt32(reader.GetAttribute("id"));
                            bedolaga.group = reader.GetAttribute("group");
                            if (bedolaga.group == "2018-381607-1")
                            {
                                temp = a;
                            }
                            else if (bedolaga.group == "2018-381607-2")
                            {
                                temp = b;
                            }
                            else if (bedolaga.group == "2018-381607-3")
                            {
                                temp = c;
                            }
                        }
                        else if (reader.Name == "marks")
                        {
                            mstart = true;
                        }
                        else if (reader.Name == "mark" && mstart)
                        {
                            int cmark = Convert.ToInt32(reader.GetAttribute("value"));
                            bedolaga.marks_list.Add(cmark);
                            temp.marks.Add(cmark);
                            switch (cmark)
                            {
                                case 2:
                                    raspredelenie[0] = raspredelenie[0] + 1;
                                    break;
                                case 3:
                                    raspredelenie[1] = raspredelenie[1] + 1;
                                    break;
                                case 4:
                                    raspredelenie[2] = raspredelenie[2] + 1;
                                    break;
                                case 5:
                                    raspredelenie[3] = raspredelenie[3] + 1;
                                    break;
                            }
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (reader.Name == "marks")
                        {
                            ucheniki.Add(bedolaga);
                            mstart = false;
                            double s = bedolaga.marks_list.Sum(x => x / 4.0);
                            bedolaga.markS = s;
                        }

                    }
                }
                double s2 = a.marks.Sum(x => x / (double)a.marks.Count());
                a.avMarks = s2;
                s2 = b.marks.Sum(x => x / (double)b.marks.Count());
                b.avMarks = s2;
                s2 = c.marks.Sum(x => x / (double)c.marks.Count());
                c.avMarks = s2;
                groups.Add(a);
                groups.Add(b);
                groups.Add(c);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            var sour = new BindingSource();
            sour.DataSource = ucheniki;
            dataGridView1.DataSource = sour;
            var source2 = new BindingSource();
            source2.DataSource = groups;
            dataGridView2.DataSource = source2;
            int[] masx = { 2, 3, 4, 5 };
            this.chart1.Series["Series1"].Points.DataBindXY(masx, raspredelenie);
        }
    }
}
