using System.IO;
using System.Data;
using System.Data.OleDb;
namespace DQA_Deskton
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void names_Tables()
        {
            string filePath = textBox1.Text;
            string connstr;
            if (filePath.EndsWith(".mdb"))
            {
                connstr = string.Format("Provider=Microsoft.ace.oledb.12.0;Data Source = " + filePath);
                // connstr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + filePath);
            }
            else
            {
                connstr = string.Format("Provider=Microsoft.ace.oledb.12.0;Data Source = " + filePath);
            }

            using (var conn = new OleDbConnection(connstr))
            {
                conn.Open();
                var dt = conn.GetSchema("Tables", new string[] {
                null, null, null, "Table"
                });

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "table_name";
                comboBox1.ValueMember = "table_name";
                conn.Close();
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "access|*.accdb;*.mdb";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = Path.GetFullPath(openFileDialog1.FileName);
                names_Tables();
            }
            else
            {
                MessageBox.Show("Please Select Dataset");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string filePath = textBox1.Text;
            string connstr;
            if (filePath.EndsWith(".mdb"))
            {
                connstr = string.Format("Provider=Microsoft.ace.oledb.12.0;Data Source = " + filePath);
                using (var conn = new OleDbConnection(connstr))
                {
                    OleDbCommand cmd = new OleDbCommand(textBox2.Text);
                    OleDbDataAdapter adapter = new OleDbDataAdapter();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    adapter.SelectCommand = cmd;
                    DataTable dt = new DataTable();
                    try
                    {
                        adapter.Fill(dt);
                    }
                    catch
                    {
                        MessageBox.Show("Please type correct code");
                    }

                    dataGridView1.DataSource = dt;

                }


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.SelectAll();
            DataObject copyData = dataGridView1.GetClipboardContent();
            if (copyData != null) Clipboard.SetDataObject(copyData);
            Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
            xlapp.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook xlWbook;
            Microsoft.Office.Interop.Excel.Worksheet xlWsheet;
            object miseddata = System.Reflection.Missing.Value;
            xlWbook = xlapp.Workbooks.Add(miseddata);

            xlWsheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWbook.Worksheets.get_Item(1);
            Microsoft.Office.Interop.Excel.Range xlr = (Microsoft.Office.Interop.Excel.Range)xlWsheet.Cells[1,1];
            xlr.Select();
            xlWsheet.PasteSpecial(xlr, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
           
        }
    }
}
