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

namespace Lab5WF
{
    public partial class Form1 : Form
    {
        List<Student> students = new List<Student>();
        List<Group> groups = new List<Group>();
        public class Student
        {
            public string name { get; set; }  // ФИО
            public int id { set; get; }       // номер зачетки
            public string group { set; get; } // название группы
            public List<int> marks;  // список оценок 2-5
            public double avMarks { get; set; }
        }
        
        public class Group
        {
            public string name { get; set; }
            public List<int> marks = new List<int>();
            public double avMarks { get; set; }
        }

        int[] raspredelenie = new int[4];

        public Form1()
        {
            InitializeComponent();
        }

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
                Student st = null;
                Group a = new Group();
                a.name = "A";
                Group b = new Group();
                b.name = "B";
                Group c = new Group();
                c.name = "C";
                Group temp = null;
                raspredelenie[0] = 0;
                raspredelenie[1] = 0;
                raspredelenie[2] = 0;
                raspredelenie[3] = 0;
                reader.WhitespaceHandling = WhitespaceHandling.None;
                bool mstart = false;

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "stud")
                        {
                            st = new Student();
                            st.marks = new List<int>();
                            st.name = reader.GetAttribute("name");
                            st.id = Convert.ToInt32(reader.GetAttribute("id"));
                            st.group = reader.GetAttribute("group");
                            if (st.group == "2018-A"){
                                temp = a;
                            } else if (st.group == "2018-B"){
                                temp = b;
                            } else if (st.group == "2018-C"){
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
                            st.marks.Add(cmark);
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
                            students.Add(st);
                            mstart = false;
                            double s = st.marks.Sum(x => x / 4.0);
                            st.avMarks = s;
                        }
                    }
                }
                double num = a.marks.Count();
                double s2 = a.marks.Sum(x => x / num);
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
            var source = new BindingSource();
            source.DataSource = students;
            dataGridView1.DataSource = source;
            var source2 = new BindingSource();
            source2.DataSource = groups;
            dataGridView2.DataSource = source2;
            int[] masx = { 2, 3, 4, 5};
            this.chart1.Series["Series1"].Points.DataBindXY(masx, raspredelenie);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
