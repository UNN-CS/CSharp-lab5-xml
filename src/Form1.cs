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
namespace Lab5
{

    public partial class Form1 : Form
    {
        List<Student> students;
        float sum = 0;
        float c = 0, sum1;
        float sred = 0;
        int l = 0;
        int dva = 0, tri = 0, chet = 0, pit = 0;

        public Form1()
        {

            students = new List<Student>();
            InitializeComponent();
        }
        public class Student
        {

            public string name;
            public int id;
            public string group;
            public List<int> marks;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "‘‡ÈÎ˚ XML (*.xml)|*.xml";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            string filename = dlg.FileName;


            try
            {
                XmlTextReader reader = new XmlTextReader(filename);
                Student st = null;
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
                        }
                        else if (reader.Name == "marks")
                        {
                            mstart = true;
                        }
                        else if (reader.Name == "mark" && mstart)
                        {
                            st.marks.Add(Convert.ToInt32(reader.GetAttribute("value")));
                            int i = st.marks.Count;
                            c++;


                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (reader.Name == "marks")
                        {
                            students.Add(st);
                            mstart = false;
                            int i = st.marks.Count;
                            int n = 0;
                            for (int j = 0; j < i; j++)
                            {
                                if (st.marks[j] == 2)
                                    dva++;
                                if (st.marks[j] == 3)
                                    tri++;
                                if (st.marks[j] == 4)
                                    chet++;
                                if (st.marks[j] == 5)
                                    pit++;
                            }
                            for (int j = 0; j < i; j++)
                            {
                                sum = sum + st.marks[j];
                            }
                            dataGridView1.Rows.Add(st.id.ToString(), st.name, st.group.ToString(), st.marks[n].ToString());
                            for (n++; n < i; n++)
                            {
                                dataGridView1.Rows.Add("", "", "", st.marks[n].ToString());
                            }
                        }
                    }
                }
                sred = sum / c;
                textBox1.Text = sum.ToString();
                textBox2.Text = c.ToString();
                textBox3.Text = sred.ToString();
                chart1.Series[0].Points.AddXY(2, dva);
                chart1.Series[0].Points.AddXY(3, tri);
                chart1.Series[0].Points.AddXY(4, chet);
                chart1.Series[0].Points.AddXY(5, pit);

                sum1 = 0;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Œ¯Ë·Í‡: " + ex.Message);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

        }
    }
}