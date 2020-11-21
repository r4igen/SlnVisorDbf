using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
namespace VisorDbf
{
    public partial class Form1 : Form
    {
        string _cadenaConexionDBF = string.Empty;
        StringBuilder query;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBuscarRuta_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog oFbd = new FolderBrowserDialog())
            {
                if (oFbd.ShowDialog() == DialogResult.OK)
                {
                    txtRuta.Text = oFbd.SelectedPath;
                    DirectoryInfo dir = new DirectoryInfo(txtRuta.Text);
                    _cadenaConexionDBF = string.Format("provider=Microsoft.jet.oledb.4.0;data source={0};extended properties=dbase III", txtRuta.Text);
                    foreach (var file in dir.GetFiles())
                    {
                        lvArchivos.Items.Add(file.ToString());
                    }
                }
            }
        }

        private void lvArchivos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = this.lvArchivos.SelectedItems;
            query = new StringBuilder();

            if (items.Count > 0)
            {
                query.AppendFormat("Select top 100000 * from {0} ", items[0].Text);
                //OleDbDataReader dr;
                DataTable dt = new DataTable();
                OleDbConnection cn = new OleDbConnection(_cadenaConexionDBF);
                OleDbCommand cmd = new OleDbCommand(query.ToString(), cn);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                OleDbDataAdapter oda = new OleDbDataAdapter(query.ToString(), cn);
                oda.Fill(dt);
                cn.Close();
                dgvDatos.DataSource = dt;
                //dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);                               
            }
        }
    }
}
