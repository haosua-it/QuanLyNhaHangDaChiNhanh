﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhaHangDaChiNhanh
{
    public partial class frmDashBoard : Form
    {
        public frmDashBoard()
        {
            InitializeComponent();
        }
        public Button NguoiDung
        {
            get { return btnUsers; }
            set { btnUsers = value; }
        }
        public Button GiamGia
        {
            get { return btnSales; }
            set { btnSales = value; }
        }
        public Button NhanVien
        {
            get { return btnStaffs; }
            set { btnStaffs = value; }
        }
        public Button ChiNhanh
        {
            get { return btnBrands; }
            set { btnBrands = value; }
        }
        public Button BaoCao
        {
            get { return btnReport; }
            set { btnReport = value; }
        }
        public Button ThucDon
        {
            get { return btnMenu; }
            set { btnMenu = value; }
        }
        public Button DanhMucMonAn
        {
            get { return btnCatalogue; }
            set { btnCatalogue = value; }
        }
        public Button NhaCungCap
        {
            get { return btnSupplier; }
            set { btnSupplier = value; }
        }
        public Button NguyenLieu
        {
            get { return btnIngredients; }
            set { btnIngredients = value; }
        }

        public Button TimKiem
        {
            get { return btnSearch; }
            set { btnSearch = value; }
        }
        public Button CaLam
        {
            get { return btnShift; }
            set { btnShift = value; }
        }
        public Button KhachHang
        {
            get { return btnClients; }
            set { btnClients = value; }
        }
        public Button Luong
        {
            get { return btnWage; }
            set { btnWage = value; }
        }
        public Button BanAn
        {
            get { return btnTable; }
            set { btnTable = value; }
        }
        public Button HoaDon
        {
            get { return btnInvoice; }
            set { btnInvoice = value; }
        }
        public Button DangXuat
        {
            get { return btnLogout; }
            set { btnLogout = value; }
        }
        public Button HoaDonNhaHang
        {
            get { return btnInvoiceNhaHang; }
            set { btnInvoiceNhaHang = value; }
        }
        public Button BanAnNhaHang
        {
            get { return btnTableNhaHang; }
            set { btnTableNhaHang = value; }
        }
        public Button BaoCaoNhaHang
        {
            get { return btnReportNhaHang; }
            set { btnReportNhaHang = value; }
        }
        public Button NhanVienNhaHang
        {
            get { return btnStaffsNhaHang; }
            set { btnStaffsNhaHang = value; }
        }
        public Button NguyenLieuNhaHang
        {
            get { return btnIngredientsNhaHang; }
            set { btnIngredientsNhaHang = value; }
        }
        public Button ThucDonNhaHang
        {
            get { return btnMenuNhaHang; }
            set { btnMenuNhaHang = value; }
        }
        public Button KhachHangNhaHang
        {
            get { return btnClientsNhaHang; }
            set { btnClientsNhaHang = value; }
        }
        public Button TimKiemNhaHang
        {
            get { return btnSearchNhaHang; }
            set { btnSearchNhaHang = value; }
        }
        public Button DangXuatNhaHang
        {
            get { return btnLogoutNhaHang; }
            set { btnLogoutNhaHang = value; }
        }
        public void SetAllButtonsEnabled(bool enable)
        {
            btnUsers.Enabled = enable;
            btnSales.Enabled = enable;
            btnStaffs.Enabled = enable;
            btnBrands.Enabled = enable;
            btnReport.Enabled = enable;
            btnMenu.Enabled = enable;
            btnCatalogue.Enabled = enable;
            btnSupplier.Enabled = enable;
            btnIngredients.Enabled = enable;
            btnSearch.Enabled = enable;
            btnShift.Enabled = enable;
            btnClients.Enabled = enable;
            btnWage.Enabled = enable;
            btnTable.Enabled = enable;
            btnInvoice.Enabled = enable;
            btnLogout.Enabled = enable;
            btnInvoiceNhaHang.Enabled = enable;
            btnTableNhaHang.Enabled = enable;
            btnReportNhaHang.Enabled = enable;
            btnStaffsNhaHang.Enabled = enable;
            btnIngredientsNhaHang.Enabled = enable;
            btnMenuNhaHang.Enabled = enable;
            btnClientsNhaHang.Enabled = enable;
            btnSearchNhaHang.Enabled = enable;
            btnLogoutNhaHang.Enabled = enable;
        }


        private void btnNhaHang_Click(object sender, EventArgs e)
        {

        }

        private void btnHeThong_Click(object sender, EventArgs e)
        {

        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmDangNhap f = new frmDangNhap();
            f.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) // CHỨC NĂNG QUẢN LÝ KHUYẾN MÃI Ở ĐÂY
        {
            this.Hide();
            frmSales f = new frmSales();
            f.ShowDialog();
            this.Show();
        }

        private void button13_Click(object sender, EventArgs e) // CHỨC NĂNG QUẢN LÝ HÓA ĐƠN Ở TAB NHÀ HÀNG
        {
            this.Hide();
            frmInvoice f = new frmInvoice();
            f.ShowDialog();
            this.Show();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)  // CHỨC NĂNG QUẢN LÝ NHÂN VIÊN Ở ĐÂY
        {
            this.Hide();
            frmStaffs f = new frmStaffs();
            f.ShowDialog();
            this.Show();

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmDashBoard_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void btnMenu_Click(object sender, EventArgs e) // chức năng của Menu
        {
            this.Hide();
            frmMenu f = new frmMenu();
            f.ShowDialog();
            this.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e) 
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất và thoát ứng dụng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit(); 
            }
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmSupplier f = new frmSupplier();
            f.ShowDialog();
            this.Show();
        }
        private void btnUsers_Click(object sender, EventArgs e)
        {
            frmUsers frm = new frmUsers();
            frm.Show();
        }




        private void button19_Click(object sender, EventArgs e)
        {
            
        }

        private void btnIngredients_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmNguyenVatLieu f = new frmNguyenVatLieu();
            f.ShowDialog();
            this.Show();
        }

        private void btnShift_Click(object sender, EventArgs e)
        {
            frmXepCa frm = new frmXepCa();
            frm.ShowDialog();
        }

        private void btnClients_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmClients frm = new frmClients();
            frm.ShowDialog();
            this.Show();
    	}
        private void btnStaffsNhaHang_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmStaffsOnTabNhaHang f = new frmStaffsOnTabNhaHang();
            f.ShowDialog();
            this.Show();

        }

        private void btnMenuNhaHang_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMenu f = new frmMenu();
            f.ShowDialog();
            this.Show();
        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmBan frm = new FrmBan();
            frm.ShowDialog();
            this.Show();
        }

        private void btnLogoutNhaHang_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất và thoát ứng dụng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnTableNhaHang_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmBanOnTabNhaHang f = new frmBanOnTabNhaHang();
            f.ShowDialog();
            this.Show();
        }

        private void btnWage_Click(object sender, EventArgs e)
        {
            frmChonLoaiLuong frm = new frmChonLoaiLuong();
            frm.Show();
        }


        private void btnBrands_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmBranch frm = new frmBranch();
            frm.ShowDialog();
            this.Show();
        }

        private void btnCatalogue_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmDanhMucMonAn frm = new frmDanhMucMonAn();
            frm.ShowDialog();
            this.Show();
        }
        private void btnInvoice_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmHoaDon frm = new frmHoaDon();
            frm.ShowDialog();
            this.Show();
        }

        private void btnClientsNhaHang_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmClients frm = new frmClients();
            frm.ShowDialog();
            this.Show();
        }


    }
}
