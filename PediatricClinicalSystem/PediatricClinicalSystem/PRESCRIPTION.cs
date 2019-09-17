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
    public partial class PRESCRIPTIONForm : Form
    {
        private static string con = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\CiMAPRO\Desktop\Pediatric Clinical System_Cimafranca\PEDIATRICSCLINICALSYSTEM.mdb";
        private OleDbConnection connect = new OleDbConnection(con);
        private Operations ops = new Operations();
        public PRESCRIPTIONForm()
        {
            InitializeComponent();
        }

        private void PreparedbyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
               
               
            }
        }

        private void Prescription_Clear_Click(object sender, EventArgs e)
        {
            ops.ClearAllText(this);
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
