using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication21
{
    public partial class Form1 : Form
    {
        MySqlConnection conn = new MySqlConnection("server=localhost;database=mysql;uid=root;password=;");
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            enableStandart(0);
        }
        private void enableStandart(int param)
        {
            if (param == 0)
            {
                dataGridView1.Location = new Point(12, 115);
                this.Size = new Size(469, 400);
                menuStrip1.Enabled = false;
                button1.Visible = false;
                dataGridView1.Enabled = false;
                groupBox1.Visible = true;
            }
            else if (param == 1)
            {
                dataGridView1.Location = new Point(12, 58);
                this.Size = new Size(469, 343);
                menuStrip1.Enabled = true;
                button1.Visible = true;
                dataGridView1.Enabled = true;
                groupBox1.Visible = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            enableStandart(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            enableStandart(1);
            tambahData(textBox2.Text, Convert.ToInt16(numericUpDown1.Value));
            LoadMatakuliah();
        }
        private void tambahData(string matakuliah, int iSks)
        {
            conn.Open();
            string sSQl = "insert into matakuliah(matakuliah, sks) values('" + matakuliah + "'," + iSks.ToString() + ")";
            MySqlCommand cmd = new MySqlCommand(sSQl, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        private void LoadMatakuliah()
        {
            #region DEFINE datagridview
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].HeaderText = "No";
            dataGridView1.Columns[2].HeaderText = "Mata kuliah";
            dataGridView1.Columns[3].HeaderText = "SKS";
            dataGridView1.Columns[4].HeaderText = "Hapus";
            dataGridView1.Columns[5].HeaderText = "Update";
            dataGridView1.Columns[0].ReadOnly = true;

            dataGridView1.Rows.Clear();
            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Width = 200;
            dataGridView1.Columns[3].Width = 40;
            dataGridView1.Columns[4].Width = 55;
            dataGridView1.Columns[5].Width = 55;

            dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView1.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            #endregion


            #region SCRIPT LOAD DATA FROM MYSQL
            conn.Open();
            int i = 0;
            MySqlCommand dbcmd = conn.CreateCommand();
            string sql = "SELECT * FROM matakuliah;";
            dbcmd.CommandText = sql;
            MySqlDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                dataGridView1.Rows.Add(1);
                dataGridView1.Rows[i].Cells[4] = new DataGridViewButtonCell();
                dataGridView1.Rows[i].Cells[5] = new DataGridViewButtonCell();

                dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString();
                dataGridView1.Rows[i].Cells[1].Value = reader.GetInt16(0).ToString(); //id
                dataGridView1.Rows[i].Cells[2].Value = reader.GetString(1).ToString(); //matakuliah
                dataGridView1.Rows[i].Cells[3].Value = reader.GetInt16(2).ToString(); //sks
                dataGridView1.Rows[i].Cells[4].Value = "Hapus";
                dataGridView1.Rows[i].Cells[5].Value = "Update";
                i++;
            }
            conn.Close();
            #endregion
        }
        private void delById(int iId)
        {
            conn.Open();
            string sSQl = "DELETE FROM matakuliah where id=" + iId;
            MySqlCommand cmd = new MySqlCommand(sSQl, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                #region KOLOM MENGHAPUS
                if (e.ColumnIndex == dataGridView1.Columns[4].Index)
                {
                    if (DialogResult.OK == MessageBox.Show("Apakah yakin ingin menghapus Matakuliah ini dengan jumlah SKS: " + dataGridView1[3, e.RowIndex].Value.ToString(), "Konfirmasi", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                    {
                        delById(Convert.ToInt16(dataGridView1[1, e.RowIndex].Value));
                        LoadMatakuliah();
                    }
                }
                #endregion
                if (e.ColumnIndex == dataGridView1.Columns[5].Index)
                {
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1[2, e.RowIndex];
                    dataGridView1.BeginEdit(true);
                    conn.Open();
                    string sSQl = "UPDATE matakuliah SET matakuliah='" + dataGridView1[2, e.RowIndex].Value.ToString() + "', SKS=" + dataGridView1[3, e.RowIndex].Value.ToString() + " where id=" + dataGridView1[1, e.RowIndex].Value.ToString();
                    MySqlCommand cmd = new MySqlCommand(sSQl, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadMatakuliah();
                }
            }
        }

        private void loadDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadMatakuliah();
        }

        private void addDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            enableStandart(0);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }

}
