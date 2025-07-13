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
            string maLoai = cboLoaiLuong.SelectedValue.ToString();

            if (maLoai == "LNV002" || maLoai == "LNV003") // Nhân viên chính thức
            {

                frmLuongPart frm = new frmLuongPart(maLoai);
                frm.Show();
            }
            else
            {
                frmLuongFull frm = new frmLuongFull(maLoai);
                frm.Show();
            }

            this.Close();
        }

        private void frmChonLoaiLuong_Load(object sender, EventArgs e)
        {
            cboLoaiLuong.DropDownStyle = ComboBoxStyle.DropDownList;
            string sql = "SELECT MALOAI, TENLOAI FROM LOAINHANVIEN";
            HamXuLy.FillCombo(sql, cboLoaiLuong, "TENLOAI", "MALOAI");
        }
    }
}
