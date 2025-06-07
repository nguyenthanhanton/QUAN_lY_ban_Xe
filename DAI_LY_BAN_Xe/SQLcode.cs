using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI_LY_BAN_Xe
{
    internal class SQLcode
    {

        SqlConnection conn;

        public void taoketnoi()
        {
            string ketnoi = "server=ANTON; uid=svbanxe;pwd=1;database=QUANLY_CUAHANG_BANXEMAY";

            conn = new SqlConnection(ketnoi);
            conn.Open();
        }
        public int kttaikhoan(string a, string b)
        {
            SqlCommand cmd = new SqlCommand("[dbo].[dangnhap]", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ten",a);
            cmd.Parameters.AddWithValue("@matkhau",b);
            object result = cmd.ExecuteScalar();

            // (Optional) KHÔNG đóng vội nếu còn dùng lại kết nối sau
            // conn.Close();

            if (result != null)
            {
                string quyen = result.ToString();
                if (quyen == "admin") return 1;
                else if (quyen == "staff") return 2;
            }

            return 0;
        }
        public int timkiemncc(string a)
        {
            int ketqua = 0;
            string query = "SELECT COUNT(*) FROM NHACUNGCAP WHERE MANCC = @mancc";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@mancc", a);

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                ketqua = (int)cmd.ExecuteScalar();
            }

            return ketqua; // 0: không tồn tại, >0: có tồn tại
        }

        public int themncc(string a,string b,string c,string d)
        {



            string query = "INSERT INTO NHACUNGCAP (MANCC, TENNCC, DIACHI, SDTNCC) VALUES (@mancc, @tenncc, @diachi, @sodienthoaincc)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@mancc", a);
                cmd.Parameters.AddWithValue("@tenncc", b);
                cmd.Parameters.AddWithValue("@diachi", c);
                cmd.Parameters.AddWithValue("@sodienthoaincc", d);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                cmd.ExecuteNonQuery();
            }
            return 1;
        }

        public int xoancc(string a)
        {
            string query = "DELETE FROM NHACUNGCAP WHERE MANCC = @mancc";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@mancc", a);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                return cmd.ExecuteNonQuery(); // Trả về số dòng bị ảnh hưởng
            }
        }
        public int suancc(string a, string b, string c, string d)
        {
            string query = "UPDATE NHACUNGCAP SET TENNCC = @tenncc, DIACHI = @diachi, SDTNCC = @sodienthoaincc WHERE MANCC = @mancc";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@mancc", a);
                cmd.Parameters.AddWithValue("@tenncc", b);
                cmd.Parameters.AddWithValue("@diachi", c);
                cmd.Parameters.AddWithValue("@sodienthoaincc", d);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                return cmd.ExecuteNonQuery(); // Trả về số dòng bị ảnh hưởng
            }
        }
        public DataTable laydanhsachncc()
        {
            DataTable dt = new DataTable();
            string query = "SELECT MANCC N'Mã Nhà cung cấp', TENNCC N'Họ tên', DIACHI N'Địa chỉ',SDTNCC N'Số điện thoại' FROM NHACUNGCAP";
            using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
            {
                da.Fill(dt);
            }
            return dt;

        }
        public DataTable timkiemncc(string mancc, string tenncc)
        {
            DataTable dt = new DataTable();
            string query = "timkiemncc"; // tên procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mancc", string.IsNullOrEmpty(mancc) ? (object)DBNull.Value : mancc);
                cmd.Parameters.AddWithValue("@tenncc", string.IsNullOrEmpty(tenncc) ? (object)DBNull.Value : tenncc);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        public DataTable laymahoadonnhap()
        {
            DataTable dt = new DataTable();
            string query = "laymahoadonnhap"; // tên procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }
        public DataTable layhoadonnhap()
        {
            DataTable dt = new DataTable();
            string query = "LAYHOADONNHAP"; // tên procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        public DataTable laychitiethoadonnhap(string a)
        {
            DataTable dt = new DataTable();
            string query = "laychitiethoadonnhap"; // tên procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mahd", a);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }
        public DataTable timkiemhoadonnhap(DateTime a)
        {
            DataTable dt = new DataTable();
            string query = "timkiemhoadonnhap"; // tên procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ngay", a);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }
        public DataTable layhoadonban()
        {
            DataTable dt = new DataTable();
            string query = "LAYHOADONBAN"; // tên procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }
        public DataTable laymahoadonban()
        {
            DataTable dt = new DataTable();
            string query = "LAYmaHOADONBAN"; // tên procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }
        public DataTable laychitiethoadonban(string a)
        {
            DataTable dt = new DataTable();
            string query = "laychitiethoadonban"; // tên procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mahd", a);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }
        public DataTable timkiemhoadonban(DateTime a)
        {
            DataTable dt = new DataTable();
            string query = "timkiemhoadonban"; // tên procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ngay", a);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        public DataTable Laymaxemay()
        {
            DataTable dt = new DataTable();
            string query = "laymaxemay "; // tên procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        public DataTable timkiemtenxemay(string a)
        {
            DataTable dt = new DataTable();
            string query = "timkiemtenxemay"; // tên procedure
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@maxe", a);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        public DataTable laydanhsachbaohanh()
        {
            DataTable dt = new DataTable();
            string query = "laydanhsachbaohanh"; // tên procedure
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
       
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }
        public bool timkiemmabaonhanh(string a)
        {
            DataTable dt = new DataTable();
            string query = "timkiemmabaonhanh"; // tên procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mabh", a);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            if(dt.Rows.Count > 0) return true;
            else return false;    
        }
        public bool checkxecobaohanhchua(string a)
        {
            DataTable dt = new DataTable();
            string query = "checkxecobaohanhchua"; // tên procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@maxe", a);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            if (dt.Rows.Count > 0) return true;
            else return false;
        }
        public void thembaohanh(string a, string b, int c)
        {

            
            try
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[thembaohanh]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Thêm tham số cho stored procedure
                    cmd.Parameters.AddWithValue("@mabh", a);
                    cmd.Parameters.AddWithValue("@maxe", b);
                    cmd.Parameters.AddWithValue("@thoihan",c);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
        }
        public void Xoabaohanh(string a)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[xoabaohanh]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Thêm tham số cho stored procedure
                    cmd.Parameters.AddWithValue("@mabh", a);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
        }
        public void suabaohanh(string a, int c)
        {


            try
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[suabaohanh]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Thêm tham số cho stored procedure
                    cmd.Parameters.AddWithValue("@mabh", a);
                    cmd.Parameters.AddWithValue("@thoihan", c);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
        }
        public bool hoadontontai(string a)
        {
            DataTable dt = new DataTable();
            string query = "timkiemhoadon"; // tên procedure
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mahd", a);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            if (dt.Rows.Count > 0) return true;
            else return false;
        }
        public DataTable checkhoadonconbaohanhkhong(string a, DateTime p)
        {
            DataTable dt = new DataTable();
            string query = "checkhoadonconbaohanhkhong"; // tên procedure
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@hoadon", a);
                cmd.Parameters.AddWithValue("@date", p);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }
        public void dongketnoi()
        {
            conn.Close();
            conn.Dispose();
        }
    }
}
