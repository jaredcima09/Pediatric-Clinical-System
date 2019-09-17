using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace PediatricClinicalSystem
{
    public partial class CONSULTATIONTRANSACTIONForm : Form
    {
        private static string con = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Q:\Pediatric Clinical System_Cimafranca\PEDIATRICSCLINICALSYSTEM.mdb";
        private OleDbConnection connect = new OleDbConnection(con);
        private Operations ops = new Operations();
        public CONSULTATIONTRANSACTIONForm()
        {
            InitializeComponent();
        }

        private void Consultation_PrepByTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
               
            }
        }

        private void Consultation_ImmunizedTextBox_KeyUp(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                
            }
        }

    }
}
