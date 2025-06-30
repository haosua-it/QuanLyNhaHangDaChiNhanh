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
            conn.ConnectionString = @"Data Source = DESKTOP-TL47T6A\SQLEXPRESS; Initial Catalog = QLYNHAHANGDACHINHANH; Integrated Security= True";
            conn.Open();
        }
        public static void Disconnect()
        {
            if (conn.State == ConnectionState.Open)
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
            SqlCommand cmd = new SqlCommand();
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
            cmd.Dispose();
            cmd = null;
        }
        public static void FillCombo(string sql, ComboBox cbo, string tenCL, string maCL)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbo.DataSource = dt;
            cbo.DisplayMember = tenCL;
            cbo.ValueMember = maCL;
        }
        public static string GetFieldValue(string sql)
        {
            string ma = "";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ma = reader.GetValue(0).ToString();
            }
            reader.Close();
            return ma;
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

           return maMoi;
       }
       public static int GetCount(string sql)
       {
           int count = 0;
           SqlCommand cmd = new SqlCommand(sql, conn);
           count = (int)cmd.ExecuteScalar();
           return count;
       }

    }
}
