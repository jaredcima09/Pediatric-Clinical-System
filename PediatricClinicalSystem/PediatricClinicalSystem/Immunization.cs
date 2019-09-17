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
    public partial class ImmunizationForm : Form
    {
        private static string con = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\CiMAPRO\Desktop\Pediatric Clinical System_Cimafranca\PEDIATRICSCLINICALSYSTEM.mdb";
        private OleDbConnection connect = new OleDbConnection(con);
        private Operations ops = new Operations();
        private int counter = 0;
        public ImmunizationForm()
        {
            InitializeComponent();
        }

        private void Immunization_ClearButton_Click(object sender, EventArgs e)
        {
            ops.ClearAllText(this);
            Immunization_DataGrid.Rows.Clear();
            Immunization_DataGrid.Refresh();

        }

        private void Immunization_PrepByTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                ops.PrepByImmunizedBy(connect, Immunization_PrepByTextBox, PreparedByResult, 0);
            }
        }

        private void Immunization_ImmunizedTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                ops.PrepByImmunizedBy(connect, Immunization_ImmunizedTextBox, ImmnunizedResult, 1);
            }
        }

        private void Immunization_VaccineCodeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (uniqueDataGrid())
                {
                    ops.VaccinePatient(connect, Immunization_VaccineCodeTextBox, ImmunizationPatientCodeTextBox, Immunization_DataGrid);
                }
            }
        }

        private bool uniqueDataGrid()
        {
            if (Immunization_VaccineCodeTextBox.Text == string.Empty)
            {
                return false;
            }

            foreach (DataGridViewRow row in Immunization_DataGrid.Rows)
            {
                if ((string)row.Cells[0].Value == Immunization_VaccineCodeTextBox.Text)
                {
                    MessageBox.Show("Vaccine already inputted");
                    return false;
                }

            }
            return true;
        }

        private void ImmunizationForm_Load(object sender, EventArgs e)
        {
            Immunization_DataGrid.Columns.Add("VACCODE", "VACCINE CODE");
            Immunization_DataGrid.Columns.Add("NAME", "NAME");
            Immunization_DataGrid.Columns.Add("DESCRIPTION", "DESCRIPTION");
            Immunization_DataGrid.Columns.Add("SHOT NUMBER", "SHOT NUMBER");
            Immunization_DataGrid.Columns.Add("REACTION", "REACTION");
        }

        private void Immunization_ShotTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {

                if (ops.checkShot(connect, int.Parse(Immunization_ShotTextBox.Text), ImmunizationPatientCodeTextBox.Text, Immunization_VaccineCodeTextBox.Text))
                {
                    Immunization_DataGrid.Rows[counter].Cells["SHOT NUMBER"].Value = Immunization_ShotTextBox.Text;
                }
            }
        }

        private void Immunization_ReactionTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                Immunization_DataGrid.Rows[counter].Cells["REACTION"].Value = Immunization_ReactionTextBox.Text;
                counter++;
            }
        }

        private void Immunization_SaveButton_Click(object sender, EventArgs e)
        {
            if (ops.CheckAllBlank(this))
            {
                ops.saveToImmunoHeaderDetail(connect,ImmunizationNoTextBox.Text, Immunization_DatePicker.Text, ImmunizationPatientCodeTextBox.Text, Immunization_WeightTextBox.Text, Immunization_HeightTextBox.Text, Immunization_PrepByTextBox.Text, Immunization_ImmunizedTextBox.Text, Immunization_DataGrid);
            }
            else
            {
                MessageBox.Show("all fields must not be empty");
            }
        }

        public void SaveInSample(string Code, string name)
        {
            string sql = "SELECT * from SAMPLE";
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connect);
            OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "SAMPLE");
            DataRow row = dataSet.Tables["SAMPLE"].NewRow();
            row["ID"] = Code;
            row["NAME"] = name;
            dataSet.Tables["SAMPLE"].Rows.Add(row);
            adapter.Update(dataSet, "SAMPLE");


        }

        private void ImmunizationPatientCodeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (ops.getPatient(connect, ImmunizationPatientCodeTextBox, ImmunizationPatientNameTextBox, Immunization_AgeTextBox, Immunization_GenderTextBox, Immunization_BirthdayTextBox, Immunization_AddressTextBox, Immunization_TelephoneTextBox, Immunization_FathersNameTextBox, Immunization_MothersNameTextBox))
                {
                    Immunization_WeightTextBox.Focus();
                }
            }
        }

        private void Immunization_ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ImmunizationNoTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (ImmunizationNoTextBox.Text == String.Empty)
                {
                    MessageBox.Show("Cannot Be Empty");
                    return;
                }
                if (!ops.ReadImmuHeader(connect, ImmunizationNoTextBox.Text))
                {
                    ImmunizationNoTextBox.Clear();
                    MessageBox.Show("Immunization Number already in file");
                    return;
                }

                ImmunizationPatientCodeTextBox.Focus();
            }
        }

        private void Immunization_WeightTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (Immunization_WeightTextBox.Text == String.Empty)
                {
                    return;
                }
                Immunization_HeightTextBox.Focus();
            }
        }

        private void Immunization_HeightTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (Immunization_HeightTextBox.Text == String.Empty)
                {
                    return;
                }
                Immunization_VaccineCodeTextBox.Focus();
            }
        }
    }
}
