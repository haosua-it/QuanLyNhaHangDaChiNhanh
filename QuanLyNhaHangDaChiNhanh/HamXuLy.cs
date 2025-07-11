using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace QuanLyNhaHangDaChiNhanh
{

    class HamXuLy
    {
        public static SqlConnection conn;
        public static void Connect()
        {
            conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=10.242.50.74;Initial Catalog=QLYNHAHANGDACHINHANH;User ID=Nguyen;Password=123";
            conn.Open();
        }
        public static void Disconnect()
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
                conn = null;
            }
        }

        public static Boolean TruyVan(string strSQL, DataTable dt)
        {
            bool kq = false;
            SqlDataAdapter da;
            try
            {
                da = new SqlDataAdapter(strSQL, conn);
                da.Fill(dt);
                if (dt.Rows.Count != 0)
                    kq = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo");
            }
            return kq;
        }
        public static void RunSQL(string sql)
        {
            Connect(); // Đảm bảo kết nối được mở

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = sql;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thông báo");
                }
            }

            Disconnect(); 
        }

        // HÀM FILL DỮ LIỆU VÀO CB
        public static void FillCombo(string sql, ComboBox cbo, string tenCL, string maCL)
        {
            Connect(); 

            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cbo.DataSource = dt;
            cbo.DisplayMember = tenCL;
            cbo.ValueMember = maCL;

            Disconnect();
        }

        public static string GetFieldValue(string sql, Dictionary<string, object> parameters = null)
        {
            string ma = "";
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ma = reader.GetValue(0).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi SQL: " + ex.Message, "Lỗi truy vấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return ma;
        }
        public static DataTable GetDataToTable(string sql, Dictionary<string, object> parameters = null)
        {
            Connect(); 

            DataTable dt = new DataTable();
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        foreach (var p in parameters)
                        {
                            cmd.Parameters.AddWithValue(p.Key, p.Value);
                        }
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy dữ liệu: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Disconnect(); 
            }

            return dt;
        }



       public static int MaLonNhat(string bang, string cot)
       {
           int maMax = 0;
           Connect();
            string sql = string.Format("SELECT TOP 1 {0} FROM {1} ORDER BY {0} DESC", cot, bang);
            string ma = GetFieldValue(sql);  // Lấy mã lớn nhất (ví dụ: CV10)

            if (!string.IsNullOrEmpty(ma))
            {
                // Cắt bỏ phần chữ đầu, lấy phần số
                string so = "";
                for (int i = 0; i < ma.Length; i++)
                {
                    if (char.IsDigit(ma[i]))
                    {
                        so += ma[i];
                    }
                }
                int soNguyen = int.Parse(so);
                maMax = soNguyen;
           }

           return maMax;
       }
        // HÀM TẠO MÃ TỰ ĐỘNG
       public static string MaTuDong(string bang)
       {
           string maMoi = "";
           string cot = "";

           // Xác định cột mã tương ứng theo từng bảng
           switch (bang)
           {
               case "CHUCVU":
                   cot = "MACHUCVU";
                   break;
               case "NGUOIDUNG":
                   cot = "MANGUOIDUNG";
                   break;
               case "CALAMVIEC":
                   cot = "MACA";
                   break;
               // Thêm các bảng khác nếu có
               default:
                   MessageBox.Show("Không xác định được cột mã cho bảng: " + bang);
                   return maMoi;
           }

           int soMax = MaLonNhat(bang, cot);
           int soMoi = soMax + 1;

           // Tạo mã mới theo dạng CVxx, NVxx...
           if (bang == "CHUCVU")
           {
               maMoi = "CV" + soMoi.ToString("D3");  // Tạo dạng CV01, CV02...
           }
           if (bang == "NGUOIDUNG")
           {
               maMoi = "ND" + soMoi.ToString("D3");  // Tạo dạng CV01, CV02...
           }
           if (bang == "CALAMVIEC")
           {
               maMoi = "CA" + soMoi.ToString("D3");  // Tạo dạng CV01, CV02...
           }
           return maMoi;
       }

       public static int GetCount(string sql)
       {
           int count = 0;
           SqlCommand cmd = new SqlCommand(sql, conn);
           count = (int)cmd.ExecuteScalar();
           return count;
       }
        //HÀM SHOW NHÂN VIÊN
       public static DataTable ShowNhanVien()
       {
           Connect(); // Kết nối CSDL
           DataTable dt = new DataTable();
           string sql = "SELECT * FROM NHANVIEN";
           try
           {
               SqlDataAdapter da = new SqlDataAdapter(sql, conn);
               da.Fill(dt);
           }
           catch (Exception ex)
           {
               MessageBox.Show("Lỗi khi lấy dữ liệu nhân viên: " + ex.Message);
           }
           finally
           {
               Disconnect();
           }
           return dt;
       }
        // HÀM SHOW NHÂN VIÊN THEO MÃ CHI NHÁNH
        public static DataTable ShowNhanVienTheoChiNhanh(string MaCN)
        {
           Connect();
           DataTable dt = new DataTable();
           string sql = "SELECT * FROM NHANVIEN WHERE MACHINHANH = '" + MaCN + "'";

           try
           {
               SqlDataAdapter da = new SqlDataAdapter(sql, conn);
               da.Fill(dt);
           }
           catch (Exception ex)
           {
               MessageBox.Show("Lỗi khi lấy nhân viên theo chi nhánh: " + ex.Message);
           }
           finally
           {
               Disconnect();
           }

           return dt;
        }

        //HÀM SHOW NHÂN VIÊN THEO TÊN
        public static DataTable ShowNhanVienTheoTen(string keyword)
        {
            Connect();
            DataTable dt = new DataTable();

            // Dùng nối chuỗi, thêm dấu % thủ công
            string sql = "SELECT * FROM NHANVIEN WHERE HOTEN LIKE N'%" + keyword + "%'";

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm nhân viên theo tên: " + ex.Message);
            }
            finally
            {
                Disconnect();
            }

            return dt;
        }
        //HÀM PHÂN TRANG
        public static DataTable ShowNhanVienPhanTrang(int pageNumber, int pageSize)
        {
            Connect();
            DataTable dt = new DataTable();
            int offset = (pageNumber - 1) * pageSize;

            string sql = string.Format("SELECT * FROM NHANVIEN ORDER BY HOTEN OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", offset, pageSize);
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phân trang: " + ex.Message);
            }
            finally
            {
                Disconnect();
            }

            return dt;
        }


        //tổng số trang
        public static int GetTotalNhanVienCount()
        {
            Connect();
            int count = 0;
            string sql = "SELECT COUNT(*) FROM NHANVIEN";

            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                count = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đếm nhân viên: " + ex.Message);
            }
            finally
            {
                Disconnect();
            }

            return count;
        }

        public static DataTable GetDataToTable(string sql)
        {
            Connect(); // mở kết nối
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        public static object ExecuteScalar(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, conn); // con là SqlConnection
            return cmd.ExecuteScalar();
        }

        //====================================================KHUYẾN MÃI============================================//
        // HÀM SHOW KHUYẾN MÃI
        public static DataTable ShowKhuyenMai()
        {
            Connect(); // Mở kết nối
            DataTable dt = new DataTable();
            string sql = "SELECT * FROM KHUYENMAI";
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy dữ liệu khuyến mãi: " + ex.Message);
            }
            finally
            {
                Disconnect();
            }

            return dt;
        }
        public static DataTable ShowKhuyenMaiPhanTrang(int page, int pageSize)
        {
            Connect();
            int offset = (page - 1) * pageSize;
            string sql = string.Format("SELECT * FROM KHUYENMAI ORDER BY MAKM OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", offset, pageSize);
            DataTable dt = new DataTable();

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phân trang khuyến mãi: " + ex.Message);
            }
            finally
            {
                Disconnect();
            }

            return dt;
        }
        //==================================================MÓN ÁN======================================================//
        public static DataTable ShowMonAnPhanTrang(int page, int pageSize)
        {
            Connect();
            int startRow = (page - 1) * pageSize + 1;
            int endRow = page * pageSize;

            string sql = string.Format(@"
        SELECT * FROM ( SELECT ROW_NUMBER() OVER (ORDER BY MAMON ASC) AS RowNum, *FROM MONAN ) AS Temp WHERE RowNum BETWEEN {0} AND {1}", startRow, endRow);

            DataTable dt = GetDataToTable(sql);
            Disconnect();
            return dt;
        }
        //===============================================BÀN ĂN============================================================//
        public static DataTable ShowBanAn()
        {
            Connect(); // Kết nối CSDL
            DataTable dt = new DataTable();
            string sql = "SELECT * FROM BANAN";
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy dữ liệu bàn ăn: " + ex.Message);
            }
            finally
            {
                Disconnect();
            }
            return dt;
        }
        //====Phân Trang====

        public static DataTable ShowBanAnPhanTrang(int pageNumber, int pageSize)
        {
            Connect();
            DataTable dt = new DataTable();
            int offset = (pageNumber - 1) * pageSize;

            string sql = string.Format("SELECT * FROM BANAN ORDER BY MABAN OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", offset, pageSize);
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phân trang bàn ăn: " + ex.Message);
            }
            finally
            {
                Disconnect();
            }

            return dt;
        }
        public static object ExecuteScalar(string sql, Dictionary<string, object> parameters = null)
        {
            object result = null;
            try
            {
                Connect(); // <--- KẾT NỐI 

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    result = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thực thi SQL: " + ex.Message, "Lỗi truy vấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Disconnect(); // <--- ĐÓNG KẾT NỐI 
            }
            return result;
        }
        public static void RunSqlWithParams(string sql, Dictionary<string, object> parameters)
        {
            Connect();
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi SQL: " + ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }
        //============================KHACHHANG==================================
        public static DataTable ShowKhachHangPhanTrang(int page, int pageSize)
        {
            Connect();
            int offset = (page - 1) * pageSize;
            string sql = string.Format("SELECT * FROM KHACHHANG ORDER BY MAKH OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", offset, pageSize);
            DataTable dt = new DataTable();

            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phân trang khách hàng: " + ex.Message);
            }
            finally
            {
                Disconnect();
            }

            return dt;
        }
        public static DataTable ShowKhachHang()
        {
            Connect(); // Kết nối CSDL
            DataTable dt = new DataTable();
            string sql = "SELECT * FROM KHACHHANG";
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy dữ liệu khách hàng: " + ex.Message);
            }
            finally
            {
                Disconnect();
            }
            return dt;
        }
    }
}
