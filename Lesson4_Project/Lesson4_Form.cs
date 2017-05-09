using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geodatabase;

namespace Lesson4_Project
{
    public partial class Lesson4_Form : Form
    {
        public Lesson4_Form()
        {
            InitializeComponent();
            comboBox1.Items.Add("Greater than 500");
            comboBox1.Items.Add("Less than 500");

            comboBox2.Items.Add("Greater than 150");
            comboBox2.Items.Add("Less than 150");

            comboBox3.Items.Add("Greater than 25,000");
            comboBox3.Items.Add("Less than 25,000");

            comboBox4.Items.Add("Yes");
            comboBox4.Items.Add("No");

            comboBox5.Items.Add("Greater than 0.02");
            comboBox5.Items.Add("Less than 0.02");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Call selection query and pass the comboBox results as arguements
            SelectionQuery.selectionQuery(comboBox1.SelectedIndex, comboBox2.SelectedIndex, comboBox3.SelectedIndex, comboBox4.SelectedIndex, comboBox5.SelectedIndex);          
        }
    }
}
