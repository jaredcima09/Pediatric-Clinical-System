using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace PediatricClinicalSystem
{
    class Operations
    {
        public void PrepByImmunizedBy(OleDbConnection con, TextBox Code, TextBox Name, int checker)
        {
            bool found = false;
            con.Open();
            string query = "SELECT * FROM EMPLOYEEFILE WHERE EMPCODE ='" + Code.Text + "'";
            OleDbCommand command = new OleDbCommand(query, con);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                found = true;
                if (reader["EMPSTATUS"].ToString().Equals("AC"))
                {
                    if (checker == 1)
                    {
                        if (reader["EMPPOSITION"].ToString().Equals("DOCTOR"))
                        {
                            Name.Text = reader["EMPLNAME"].ToString() + " "
                                + reader["EMPFNAME"].ToString() + " "
                                + reader["EMPMNAME"].ToString() + ".";

                        }
                        else
                        {
                            MessageBox.Show("!Employee Cannot Immunize! Subject to Criminal Liabilities");
                            Name.Clear();
                            Code.Clear();
                            con.Close();
                            return;
                        }
                    }

                    Name.Text = reader["EMPLNAME"].ToString() + ", "
                                + reader["EMPFNAME"].ToString() + " "
                                + reader["EMPMNAME"].ToString() + ".";
                }
                else if (reader["EMPSTATUS"].ToString().Equals("IN"))
                {
                    if (checker == 1 && !reader["EMPPOSITION"].ToString().Equals("DOCTOR"))
                    {
                        MessageBox.Show("Inactive Employee and not a Doctor");
                        Name.Clear();
                        Code.Clear();
                        con.Close();
                        return;
                    }
                    MessageBox.Show("Inactive Employee");
                    Name.Clear();
                    Code.Clear();
                    con.Close();
                    return;
                }
            }
            con.Close();
            if (!found)
            {
                Name.Clear();
                Code.Clear();
                MessageBox.Show("Invalid Employee Code");
            }
        }

        public bool getPatient(OleDbConnection con, TextBox PATCODE, TextBox PATNAME, TextBox PATAGE, TextBox PATGENDER, TextBox PATBDAY, TextBox PATADDR, TextBox PATTEL, TextBox PATFATHNAME, TextBox PATMOTHNAME)
        {
            bool found = false;
            con.Open();
            string query = "SELECT * FROM PATIENTFILE WHERE PATCODE ='" + PATCODE.Text + "'";
            OleDbCommand command = new OleDbCommand(query, con);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                found = true;
                if (reader["PATSTATUS"].ToString().Equals("AC"))
                {
                    PATNAME.Text = reader["PATLNAME"].ToString() + ", "
                        + reader["PATFNAME"].ToString() + " "
                        + reader["PATMNAME"].ToString();

                    PATAGE.Text = reader["PATAGE"].ToString();
                    PATGENDER.Text = reader["PATGENDER"].ToString();
                    PATBDAY.Text = reader["PATBDATE"].ToString();
                    PATADDR.Text = reader["PATADDR"].ToString();
                    PATTEL.Text = reader["PATTEL"].ToString();
                    PATFATHNAME.Text = reader["PATFATHNAME"].ToString();
                    PATMOTHNAME.Text = reader["PATMOTHNAME"].ToString();
                }
                else
                {
                    MessageBox.Show("Inactive Patient");
                    return false;
                }
            }
            con.Close();
            if (!found)
            {
                MessageBox.Show("Not found");
                return false;
            }
            return found;
        }

        public void ClearAllText(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c is TextBox)
                    ((TextBox)c).Clear();
                else
                    ClearAllText(c);
            }
        }

        public bool CheckAllBlank(Control con)
        {
            foreach(Control c in con.Controls)
            {
                if (c is TextBox)
                {
                    TextBox textbox = c as TextBox;
                    if (textbox.Text == string.Empty)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        internal void VaccinePatient(OleDbConnection connect, TextBox VaccineCode,TextBox PatientCode, DataGridView DataGrid)
        {
            bool found = false;
            connect.Open();
            string vacQuery = "SELECT * FROM VACCINEFILE WHERE VACCODE = '" + VaccineCode.Text + "'";
            OleDbCommand VACcommand = new OleDbCommand(vacQuery, connect);
            OleDbDataReader VACreader = VACcommand.ExecuteReader();
            while (VACreader.Read())
            {
                found = true;
                if (VACreader["VACSTATUS"].ToString() == "AV")
                {
                    
                    bool foundVacPac = false;
                    string query = "SELECT * FROM VACCINEPATIENTFILE WHERE VACVCODE ='" + VaccineCode.Text + "' AND VACPCODE ='" + PatientCode.Text + "'";
                    OleDbCommand command = new OleDbCommand(query, connect);
                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DataGrid.Rows.Add(VACreader[0].ToString(), VACreader[1].ToString(), VACreader[2].ToString());
                        foundVacPac = true;
                    }
                    if (!foundVacPac)
                    {
                        MessageBox.Show("No record for patient with this vaccine");
                    }
                }
                else
                {
                    MessageBox.Show("Vaccine is unavailable");
                }
            }
            connect.Close();
            if (!found)
            {
                VaccineCode.Clear();
                MessageBox.Show("Vaccine Not Found");
            }
        }

        internal void saveToImmunoHeaderDetail(OleDbConnection connect,object IMMHIMMUNO, string Date, string PATCODE, string WEIGHT, string HEIGHT, string PREPBYCODE, string IMMUBYCODE, DataGridView datagrid)
        {
            string sql = "SELECT * FROM IMMUNIZATIONHEADERFILE";
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connect);
            OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
            DataSet dataset = new DataSet();

            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adapter.Fill(dataset, "IMMUNIZATIONHEADERFILE");

            DataRow row = dataset.Tables["IMMUNIZATIONHEADERFILE"].Rows.Find(IMMHIMMUNO);

            if (row == null)
            {
                DataRow immROW = dataset.Tables["IMMUNIZATIONHEADERFILE"].NewRow();

                immROW[0] = IMMHIMMUNO;
                immROW[1] = Date;
                immROW[2] = PATCODE;
                immROW[3] = WEIGHT;
                immROW[4] = HEIGHT;
                immROW[5] = PREPBYCODE;
                immROW[6] = IMMUBYCODE;

                dataset.Tables["IMMUNIZATIONHEADERFILE"].Rows.Add(immROW);
                adapter.Update(dataset, "IMMUNIZATIONHEADERFILE");
            }
            else
            {
                MessageBox.Show("Not Unique");
                return;
            }

            foreach(DataGridViewRow Row in datagrid.Rows)
            {
                if (Row.Cells[0].Value != null && Row.Cells[3].Value != null && Row.Cells[4].Value != null)
                {
                    saveToHeader(connect, IMMHIMMUNO,Row.Cells[0].Value.ToString(), Row.Cells[3].Value.ToString(), Row.Cells[4].Value.ToString());
                }
            }
            
        }

        private void saveToHeader(OleDbConnection connect, object IMMUNO, object VACCODE, object SHOTNUM, object REACTION)
        {
            string sql = "SELECT * FROM IMMUNIZATIONDETAILFILE";
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connect);
            OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
            DataSet dataset = new DataSet();

            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adapter.Fill(dataset, "IMMUNIZATIONDETAILFILE");

            DataRow row = dataset.Tables["IMMUNIZATIONDETAILFILE"].Rows.Find(new object [] {IMMUNO, VACCODE});

            if (row == null)
            {
                DataRow immROW = dataset.Tables["IMMUNIZATIONDETAILFILE"].NewRow();

                immROW[0] = IMMUNO;
                immROW[1] = VACCODE;
                immROW[2] = SHOTNUM;
                immROW[3] = REACTION;

                dataset.Tables["IMMUNIZATIONDETAILFILE"].Rows.Add(immROW);
                adapter.Update(dataset, "IMMUNIZATIONDETAILFILE");
            }
        }

        public bool checkShot(OleDbConnection connect, int shotNum, string PATCODE, string VACCODE)
        {
            int x = 0;
            connect.Open();
            string query = "SELECT * FROM VACCINEPATIENTFILE WHERE VACVCODE ='" + VACCODE + "' AND VACPCODE ='" + PATCODE + "'";
            OleDbCommand command = new OleDbCommand(query, connect);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                x = (int)(reader[3]);
            }

            string vacQuery = "SELECT * FROM VACCINEFILE WHERE VACCODE = '" + VACCODE + "'";
            OleDbCommand VACcommand = new OleDbCommand(vacQuery, connect);
            OleDbDataReader VACreader = VACcommand.ExecuteReader();
            while (VACreader.Read())
            {
                if ((x + shotNum) > (int)VACreader[4])
                {
                    MessageBox.Show("Shot Number Cannot be greater than allowed maximum....overdose");
                    connect.Close();
                    return false;
                }
            }
            connect.Close();
            return true;
        }

        public bool ReadImmuHeader(OleDbConnection connect, object p)
        {

            string sql = "SELECT * FROM IMMUNIZATIONHEADERFILE";
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connect);
            OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
            DataSet dataset = new DataSet();

            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adapter.Fill(dataset, "IMMUNIZATIONHEADERFILE");

            DataRow row = dataset.Tables["IMMUNIZATIONHEADERFILE"].Rows.Find(p);

            if (row == null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
