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
        List<student> students = new List<student>();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            int numb = 0,j=0,sum=0,y;
            int[] t = new int[5];
            for (int i = 0; i < 5; i++)
                t[i] = 0;
            List<int> l;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "files XML (*.xm)|*.xml";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            string filename = dlg.FileName;

            XmlTextReader reader = new XmlTextReader(filename);
            student st=null;
            reader.WhitespaceHandling = WhitespaceHandling.None;
            bool mstart = false;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "stud")
                    {
                        st = new student();
                        st.marks = new List<int>();
                        st.name = reader.GetAttribute("name");
                        st.id = Convert.ToInt32(reader.GetAttribute("id"));
                        st.group = reader.GetAttribute("group");
                        numb++;

                    }
                    else if (reader.Name == "marks")
                    {
                        mstart = true;
                    }
                    else if (reader.Name == "mark" && mstart)
                    {
                        st.marks.Add(Convert.ToInt32(reader.GetAttribute("value")));
                        j++;

                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name == "marks")
                    {
                        students.Add(st);
                        mstart = false;


                    }
                }
            }

            for (int i = 0; i < numb; i++)
            {
                listBox1.Items.AddRange(new string[] {Convert.ToString(students[i].id)});
                listBox2.Items.AddRange(new string[] { Convert.ToString(students[i].name) });
                listBox3.Items.AddRange(new string[] { Convert.ToString(students[i].group) });
                l = students[i].marks;




                for (int k = 0; k < 4; k++)
                {
                    sum += l[k];
                    switch (l[k])
                    {
                        case 2 :t[0] += 1;break;
                        case 3: t[1] += 1;break;
                        case 4: t[2] += 1;break;
                        case 5:t[3] += 1;break;


                    }
                }
                    listBox4.Items.AddRange(new string[] { Convert.ToString(sum) });
                sum =0;
                //  y = 0;
                for (int s = 2; s <= 5; s++)
                {
                    chart1.Series[0].Points.AddXY(s, t[s - 2]);
                }  
            }


        }
    }
    public class student
        {
            public string name;
            public int id;
            public string group;
            public List<int> marks;


        }
}
