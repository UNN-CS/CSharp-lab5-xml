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

namespace lab5
{
    public partial class Form1 : Form
    {
        List<Student> students = new List<Student>();
        List<Group> groups = new List<Group>();
        public class Student
        {
            public string name { get; set; }// ФИО
            public int id { get; set; }// номер зачетки
            public string group { get; set; } // название группы
            public List<int> marks; // список оценок 2-5
            public double Average_marks { get; set; } //средняя оценка
        }
        public class Group
        {
            public string name { get; set; }// Название
            public List<int> marks = new List<int>();//Список оценок
            public double avMarks { get; set; }//Средний балл группы
        }
       

        int[] marks = new int[] { 2, 3, 4, 5 };//для гистограммы заюзаю

        public Form1()
        {
            InitializeComponent();
            //students = new List<Student>();
            //groups = new List<Group>();
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
                Group FIIT = new Group();
                FIIT.name = "381606-3";
                Group PI1 = new Group();
                PI1.name = "381607-1";
                Group PI2 = new Group();
                PI2.name = "381607-2";
                Group gr = null;
                marks[0] = 0;
                marks[1] = 0;
                marks[2] = 0;
                marks[3] = 0;
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
                            if (st.group == "381606-3") { gr = FIIT; }
                            else if (st.group == "381607-1") { gr = PI1; }
                            else if (st.group == "381607-2") { gr = PI2; }
                        }
                        else if (reader.Name == "marks")
                        {
                            mstart = true;
                        }
                        else if (reader.Name == "mark" && mstart)
                        {
                            int mark = Convert.ToInt32(reader.GetAttribute("value"));
                            st.marks.Add(mark);
                            gr.marks.Add(mark);
                            if (mark == 2) { marks[0] = marks[0] + 1; }
                            else if (mark == 3) { marks[1] = marks[1] + 1; }
                            else if (mark == 4) { marks[2] = marks[2] + 1; }
                            else if (mark == 5) { marks[3] = marks[3] + 1; }
                        }
                     }
                        else if (reader.NodeType == XmlNodeType.EndElement)
                        {
                            if (reader.Name == "marks")
                            {
                                students.Add(st);
                                mstart = false;
                                double av_m = st.marks.Sum(x => x / 4.0);
                                st.Average_marks = av_m;
                            }
                        }
                    }
                    FIIT.avMarks = FIIT.marks.Sum() /(double)FIIT.marks.Count;
                    PI1.avMarks = PI1.marks.Sum() / (double)PI1.marks.Count;
                    PI2.avMarks = PI2.marks.Sum() / (double)PI2.marks.Count;
                    groups.Add(FIIT);
                    groups.Add(PI1);
                    groups.Add(PI2);
                
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
            int[] masx = { 2, 3, 4, 5 };
            this.chart1.Series["Series1"].Points.DataBindXY(masx, marks);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
