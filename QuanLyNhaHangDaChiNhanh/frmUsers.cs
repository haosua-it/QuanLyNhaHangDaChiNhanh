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
    public partial class frmUsers : Form
    {
        public frmUsers()
        {
            InitializeComponent();
        }
        private int currentPage = 1;
        private int pageSize = 10;
        private void frmUsers_Load(object sender, EventArgs e)
        {
            HamXuLy.Connect();
            pnlNguoiDung.Enabled = false;
            txtMaNguoiDung.Enabled = false;
            cboMaChucVu.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMaChiNhanh.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;

            string sqlChucVu = "SELECT * FROM CHUCVU";
            string sqlChiNhanh = "SELECT * FROM CHINHANH";
            HamXuLy.FillCombo(sqlChucVu, cboMaChucVu, "TENCHUCVU", "MACHUCVU");
            HamXuLy.FillCombo(sqlChiNhanh, cboMaChiNhanh, "TENCHINHANH", "MACHINHANH");

            // Load trạng thái thủ công
            DataTable dtTrangThai = new DataTable();
            dtTrangThai.Columns.Add("Text");
            dtTrangThai.Columns.Add("Value");

            dtTrangThai.Rows.Add("Đang hoạt động", 1);
            dtTrangThai.Rows.Add("Ngừng hoạt động", 0);

            cboTrangThai.DataSource = dtTrangThai;
            cboTrangThai.DisplayMember = "Text";
            cboTrangThai.ValueMember = "Value";
            LoadNguoiDungPhanTrang();
        }
        private void reset()
        {
            pnlNguoiDung.Enabled = false;
            txtMaNguoiDung.Enabled = true;
            txtMaNguoiDung.Text = "";
            txtTenNguoiDung.Text = "";
            txtMatKhau.Text = "";
            txtHoTen.Text = "";
            txtEmail.Text = "";
            txtSDT.Text = "";
            btnAdd.Enabled = true;
            btnBack.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            btnPrint.Enabled = true;
            btnRedo.Enabled = true;
            btnSave.Enabled = true;
            btnSearch.Enabled = true;
            btnUndo.Enabled = true;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LoadNguoiDungPhanTrang()
        {
            DataTable dt = ShowNguoiDungPhanTrang(currentPage, pageSize);
            luoiNguoiDung.DataSource = dt;

            luoiNguoiDung.Columns["MANGUOIDUNG"].HeaderText = "Mã người dùng";
            luoiNguoiDung.Columns["TENNGUOIDUNG"].HeaderText = "Tên tài khoản";
            luoiNguoiDung.Columns["MATKHAU"].HeaderText = "Mật khẩu";
            luoiNguoiDung.Columns["HOVATEN"].HeaderText = "Họ và tên";
            luoiNguoiDung.Columns["EMAIL"].HeaderText = "Email";
            luoiNguoiDung.Columns["SODIENTHOAI"].HeaderText = "SĐT";
            luoiNguoiDung.Columns["MACHINHANH"].HeaderText = "Chi nhánh";
            luoiNguoiDung.Columns["MACHUCVU"].HeaderText = "Chức vụ";
            luoiNguoiDung.Columns["TRANGTHAI"].HeaderText = "Trạng thái";

            luoiNguoiDung.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            luoiNguoiDung.ReadOnly = true;
            luoiNguoiDung.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            lblPage.Text = string.Format("Trang {0}", currentPage);
        }
        public static DataTable ShowNguoiDungPhanTrang(int pageNumber, int pageSize)
        {
            HamXuLy.Connect();
            SqlConnection conn = HamXuLy.conn;
            DataTable dt = new DataTable();
            int offset = (pageNumber - 1) * pageSize;

            string sql = string.Format("SELECT * FROM NGUOIDUNG ORDER BY HOVATEN OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", offset, pageSize);
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phân trang: " + ex.Message);
            }

            return dt;
        }

        private void btnPrevPage_Click_1(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadNguoiDungPhanTrang();
            }
        }

        private void btnNextPage_Click_1(object sender, EventArgs e)
        {
            currentPage++;
            LoadNguoiDungPhanTrang();
        }

        private void luoiNguoiDung_Click(object sender, EventArgs e)
        {
            if (luoiNguoiDung.CurrentRow == null) return;

            DataGridViewRow row = luoiNguoiDung.CurrentRow;

            txtMaNguoiDung.Text = row.Cells["MANGUOIDUNG"].Value.ToString();
            txtTenNguoiDung.Text = row.Cells["TENNGUOIDUNG"].Value.ToString();
            txtMatKhau.Text = row.Cells["MATKHAU"].Value.ToString();
            txtHoTen.Text = row.Cells["HOVATEN"].Value.ToString();
            txtEmail.Text = row.Cells["EMAIL"].Value.ToString();
            txtSDT.Text = row.Cells["SODIENTHOAI"].Value.ToString();

            // Đặt giá trị cho combobox Chi Nhánh (giả sử ValueMember = "MACHINHANH")
            cboMaChiNhanh.SelectedValue = row.Cells["MACHINHANH"].Value.ToString();

            // Đặt giá trị cho combobox Chức Vụ
            cboMaChucVu.SelectedValue = row.Cells["MACHUCVU"].Value.ToString();

            // Đặt giá trị cho combobox Trạng Thái (nó là kiểu bit → convert về int)
            cboTrangThai.SelectedValue = Convert.ToInt32(row.Cells["TRANGTHAI"].Value);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            reset();
            txtMaNguoiDung.Text = HamXuLy.MaTuDong("NGUOIDUNG");
            pnlNguoiDung.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            txtTenNguoiDung.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            reset();
            pnlNguoiDung.Enabled = true;
            btnDelete.Enabled = false;
            btnAdd.Enabled = false;
            txtTenNguoiDung.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            if (txtMaNguoiDung.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn mã người dùng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                HamXuLy.Connect();
                string sqlDelete = "DELETE FROM NGUOIDUNG WHERE MANGUOIDUNG = '" + txtMaNguoiDung.Text + "'";
                if (MessageBox.Show("Bạn có chắc muốn xóa người dùng này không này không?", "Xác nhận xóa!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        HamXuLy.RunSQL(sqlDelete);
                        MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Thông báo");
                    }
                }
                LoadNguoiDungPhanTrang();
                reset();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnEdit.Enabled == true) // sửa
            {
                if (txtMaNguoiDung.Text == "")
                {
                    MessageBox.Show("Bạn chưa chọn mã người dùng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    HamXuLy.Connect();
                    string sqlUpdate = "UPDATE NGUOIDUNG SET " +
                                       "TENNGUOIDUNG = N'" + txtTenNguoiDung.Text + "', " +
                                       "MATKHAU = N'" + txtMatKhau.Text + "', " +
                                       "HOVATEN = N'" + txtHoTen.Text + "', " +
                                       "EMAIL = N'" + txtEmail.Text + "', " +
                                       "SODIENTHOAI = N'" + txtSDT.Text + "', " +
                                       "MACHINHANH = N'" + cboMaChiNhanh.SelectedValue.ToString() + "', " +
                                       "MACHUCVU = N'" + cboMaChucVu.SelectedValue.ToString() + "', " +
                                       "TRANGTHAI = " + Convert.ToInt32(cboTrangThai.SelectedValue) + " " +
                                       "WHERE MANGUOIDUNG = N'" + txtMaNguoiDung.Text + "'";

                    try
                    {
                        HamXuLy.RunSQL(sqlUpdate);
                        MessageBox.Show("Chỉnh sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Thông báo");
                    }

                    LoadNguoiDungPhanTrang();
                    reset();
                }
            }
            else // thêm
            {
                if (txtMaNguoiDung.Text == "" || txtTenNguoiDung.Text == "" || txtMatKhau.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                HamXuLy.Connect();

                string sqlInsert = "INSERT INTO NGUOIDUNG(" +
                                   "MANGUOIDUNG, TENNGUOIDUNG, MATKHAU, HOVATEN, EMAIL, SODIENTHOAI, MACHINHANH, MACHUCVU, TRANGTHAI" +
                                   ") VALUES (" +
                                   "N'" + txtMaNguoiDung.Text + "', " +
                                   "N'" + txtTenNguoiDung.Text + "', " +
                                   "N'" + txtMatKhau.Text + "', " +
                                   "N'" + txtHoTen.Text + "', " +
                                   "N'" + txtEmail.Text + "', " +
                                   "N'" + txtSDT.Text + "', " +
                                   "N'" + cboMaChiNhanh.SelectedValue.ToString() + "', " +
                                   "N'" + cboMaChucVu.SelectedValue.ToString() + "', " +
                                   Convert.ToInt32(cboTrangThai.SelectedValue) + ")";

                try
                {
                    HamXuLy.RunSQL(sqlInsert);
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thông báo");
                }

                LoadNguoiDungPhanTrang();
                reset();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadNguoiDungPhanTrang();
                return;
            }

            HamXuLy.Connect();
            string sql = "SELECT * FROM NGUOIDUNG WHERE HOVATEN LIKE N'%" + keyword + "%'";
            DataTable dt = HamXuLy.GetDataToTable(sql);
            luoiNguoiDung.DataSource = dt;
        }

        private Stack<object[]> undoStack = new Stack<object[]>();
        private Stack<object[]> redoStack = new Stack<object[]>();

        private void SaveCurrentState()
        {
            object[] state = new object[]
    {
        txtMaNguoiDung.Text,
        txtTenNguoiDung.Text,
        txtMatKhau.Text,
        txtHoTen.Text,
        txtEmail.Text,
        txtSDT.Text,
        cboMaChiNhanh.SelectedValue,
        cboMaChucVu.SelectedValue,
        cboTrangThai.SelectedValue
    };
            undoStack.Push(state);
            redoStack.Clear(); // xóa redo khi có thay đổi mới
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                object[] currentState = new object[]
        {
            txtMaNguoiDung.Text,
            txtTenNguoiDung.Text,
            txtMatKhau.Text,
            txtHoTen.Text,
            txtEmail.Text,
            txtSDT.Text,
            cboMaChiNhanh.SelectedValue,
            cboMaChucVu.SelectedValue,
            cboTrangThai.SelectedValue
        };
                redoStack.Push(currentState);

                object[] previousState = undoStack.Pop();
                txtMaNguoiDung.Text = previousState[0].ToString();
                txtTenNguoiDung.Text = previousState[1].ToString();
                txtMatKhau.Text = previousState[2].ToString();
                txtHoTen.Text = previousState[3].ToString();
                txtEmail.Text = previousState[4].ToString();
                txtSDT.Text = previousState[5].ToString();
                cboMaChiNhanh.SelectedValue = previousState[6];
                cboMaChucVu.SelectedValue = previousState[7];
                cboTrangThai.SelectedValue = previousState[8];
            }
            else
            {
                MessageBox.Show("Không có thao tác nào để hoàn tác.");
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                SaveCurrentState(); // lưu lại trước khi redo
                object[] redoState = redoStack.Pop();
                txtMaNguoiDung.Text = redoState[0].ToString();
                txtTenNguoiDung.Text = redoState[1].ToString();
                txtMatKhau.Text = redoState[2].ToString();
                txtHoTen.Text = redoState[3].ToString();
                txtEmail.Text = redoState[4].ToString();
                txtSDT.Text = redoState[5].ToString();
                cboMaChiNhanh.SelectedValue = redoState[6];
                cboMaChucVu.SelectedValue = redoState[7];
                cboTrangThai.SelectedValue = redoState[8];
            }
            else
            {
                MessageBox.Show("Không có thao tác nào để làm lại.");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }


    }
}
