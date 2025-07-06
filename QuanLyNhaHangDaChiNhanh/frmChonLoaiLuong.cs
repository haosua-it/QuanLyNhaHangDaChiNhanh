using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanLyNhaHangDaChiNhanh
{
    public partial class frmChonLoaiLuong : Form
    {
        public frmChonLoaiLuong()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cboLoaiLuong.Text == "Lương Full - Time")
            {
                frmLuongFull frm = new frmLuongFull();
                frm.Show();
                this.Close();
            }
            else
            {
                frmLuongPart frm = new frmLuongPart();
                frm.Show();
                this.Close();
            }
        }

        private void frmChonLoaiLuong_Load(object sender, EventArgs e)
        {
            cboLoaiLuong.DropDownStyle = ComboBoxStyle.DropDownList;
            DataTable dtLoaiLuong = new DataTable();
            dtLoaiLuong.Columns.Add("Text");
            dtLoaiLuong.Columns.Add("Value");

            dtLoaiLuong.Rows.Add("Lương Full - Time", 1);
            dtLoaiLuong.Rows.Add("Lương Part - Time", 0);

            cboLoaiLuong.DataSource = dtLoaiLuong;
            cboLoaiLuong.DisplayMember = "Text";
            cboLoaiLuong.ValueMember = "Value";
        }
    }
}
