using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DAI_LY_BAN_Xe
{
    internal class SQLcode
    {

         public SqlConnection conn;

        public void taoketnoi()
        {
            string ketnoi = "server=DESKTOP-392TCLG\\SQLEXPRESS01; uid=banxe;pwd=1;database=QUANLY_CUAHANG_BANXEMAY";

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
                else if (quyen == "nhanvien") return 2;
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
            string query = @"UPDATE NHAPHANG SET MANCC = 'NCC000' WHERE MANCC = @mancc;DELETE FROM NHACUNGCAP WHERE MANCC = @mancc;";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@mancc", a);

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                return cmd.ExecuteNonQuery(); // Trả về tổng số dòng bị ảnh hưởng
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
            taoketnoi();
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
        public DataTable layhanhsachkhachhang()
        {
            DataTable dt = new DataTable();
            string query = "laydanhsachkhachhang"; // tên procedure
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }
        public bool timkiemmakhachhang(string a)
        {
            DataTable dt = new DataTable();
            string query = "timkiemmakhachhang"; // tên procedure
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@makh", a);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            if (dt.Rows.Count > 0) return true;
            else return false;
        }

        public bool timkiemsdtkhachhang(string a,string sdt)
        {
            DataTable dt = new DataTable();
            string query = "timkiemsdtkhachhang"; // tên procedure
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@makh", a);
                cmd.Parameters.AddWithValue("@sdt", sdt);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            if (dt.Rows.Count > 0) return true;
            else return false;
        }
        public void themkhachhang(string a, string b, string c,int sotienchi)
        {


            try
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[themkhachhang]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Thêm tham số cho stored procedure
                    cmd.Parameters.AddWithValue("@makh", a);
                    cmd.Parameters.AddWithValue("@tenkh ", b);
                    cmd.Parameters.AddWithValue("@sdt", c);
                    cmd.Parameters.AddWithValue("@sotienchi", sotienchi);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
        }
        public void suakhachhang(string a, string b, string c, int sotienchi)
        {


            try
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[suakhachhang]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Thêm tham số cho stored procedure
                    cmd.Parameters.AddWithValue("@makh", a);
                    cmd.Parameters.AddWithValue("@tenkh ", b);
                    cmd.Parameters.AddWithValue("@sdt", c);
                    cmd.Parameters.AddWithValue("@sotienchi", sotienchi);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
        }
        public void xoakhachhang(string a)
        {


            try
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[xoakhachhang]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Thêm tham số cho stored procedure
                    cmd.Parameters.AddWithValue("@makh", a);
                    
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
        }
        public DataTable laydanhsachtimkiemkhachhang(string a, string b, string c)
        {
            DataTable dt = new DataTable();
            string query = "laydanhsachtimkiemkhachhang"; // tên procedure
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@makh", string.IsNullOrEmpty(a) ? (object)DBNull.Value : a);
                cmd.Parameters.AddWithValue("@tenkh", string.IsNullOrEmpty(b) ? (object)DBNull.Value : b);
                cmd.Parameters.AddWithValue("@sdt", string.IsNullOrEmpty(c) ? (object)DBNull.Value : c);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }
        public string laymahoadonlonnhat()
        {
            string maLonNhat = "";
            string query = "laymanhaphanglonnhat"; // tên procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        maLonNhat = dt.Rows[0][0].ToString(); // Lấy MANHAP từ dòng đầu tiên, cột đầu tiên
                    }
                }
            }

            return maLonNhat;
        }
        public string laymaxemaylonnhat()
        {
            string maLonNhat = "";
            string query = "laymaxelonnhat"; // tên procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        maLonNhat = dt.Rows[0][0].ToString(); // Lấy MANHAP từ dòng đầu tiên, cột đầu tiên
                    }
                }
            }

            return maLonNhat;
        }

        public ListBox laytencacnhacungcap(ListBox ten)
        {
            ten.Items.Clear();
            string query = "laytencacnhacungcap"; // tên stored procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ten.Items.Add(reader["TENNCC"].ToString());
                    }
                }

         
            }

            return ten;
        }
        public string laymanhacungcap(string a)
        {
            string maLonNhat = "";
            string query = "laymanhacungcap"; // tên procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ten", a);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        maLonNhat = dt.Rows[0][0].ToString(); 
                    }
                }
            }

            return maLonNhat;
        }
        public void taohoadonnhap(string manhap,DateTime ngaylap, int tongtien , string manv,string mancc)
        {


            try
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[taohoadonnhap]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Thêm tham số cho stored procedure
                    cmd.Parameters.AddWithValue("@mahd", manhap);
                    cmd.Parameters.AddWithValue("@ngaylap", ngaylap);
                    cmd.Parameters.AddWithValue("@tongtien", tongtien);
                    cmd.Parameters.AddWithValue("@manv", manv);
                    cmd.Parameters.AddWithValue("@mancc", mancc);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
        }
        public void taoxemay(string maxe, string tenxe,string hangsx,string namsx,string tinhtrang,string nguongoc,byte[] anh,int soluong)
        {


            try
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[themxe]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Thêm tham số cho stored procedure
                    cmd.Parameters.AddWithValue("@maxe", maxe);
                    cmd.Parameters.AddWithValue("@tenxe", tenxe);
                    cmd.Parameters.AddWithValue("@hangsx", hangsx);
                    cmd.Parameters.AddWithValue("@namsx", namsx);
                    cmd.Parameters.AddWithValue("@tinhtrang", tinhtrang);
                    cmd.Parameters.AddWithValue("@nguongoc", nguongoc);
                    cmd.Parameters.AddWithValue("@anh", anh);
                    cmd.Parameters.AddWithValue("@soluong", soluong);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
        }
        public void taoctnhap(string manhap ,string maxe, int sl, int dongia)
        {


            try
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[taochitietnhap]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Thêm tham số cho stored procedure
                    cmd.Parameters.AddWithValue("@manhap", manhap);
                    cmd.Parameters.AddWithValue("@maxe", maxe);
                    cmd.Parameters.AddWithValue("@soluong", sl);
                    cmd.Parameters.AddWithValue("@dongia", dongia);
                    
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
        }
        

        public string laymanhanvientutaikhoan(string a)
        {
            string maLonNhat = "";
            string query = "laymanhanvientutaikhoan"; // tên procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tendangnhap", a);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        maLonNhat = dt.Rows[0][0].ToString();
                    }
                }
            }

            return maLonNhat;
        }
        public DataTable layxemay()
        {
            DataTable dt = new DataTable();
            string query = "layxemay"; // tên procedure
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

    

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        public DataTable laythongtinxeban(string a)
        {
            DataTable dt = new DataTable();
            string query = "laythongtinxeban"; // tên procedure
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@maxe", a);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }
        public string laymakhtusdt(string a)
        {
            string maLonNhat = "";
            string query = "laymakhtusdt"; // tên procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@sdt", a);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        maLonNhat = dt.Rows[0][0].ToString();
                    }
                }
            }
            return maLonNhat;

        }
        public string laymahoadonbanlonnhat()
        {
            string maLonNhat = "";
            string query = "laymahdlonnhat"; // tên procedure

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        maLonNhat = dt.Rows[0][0].ToString(); // Lấy MANHAP từ dòng đầu tiên, cột đầu tiên
                    }
                }
            }

            return maLonNhat;
        }
        public void taohoadonban(string mahoadonban,DateTime ngay, int tongtiensaukhigiamgia, string makhban, string manv)
        {
           

            try
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[taohoadonban]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Thêm tham số cho stored procedure
                    cmd.Parameters.AddWithValue("@mahd", mahoadonban);
                    cmd.Parameters.AddWithValue("@ngay", ngay);
                    cmd.Parameters.AddWithValue("@tongtien", tongtiensaukhigiamgia);
                    cmd.Parameters.AddWithValue("@makh", makhban);
                    cmd.Parameters.AddWithValue("@manv", manv);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
        }
        public void taoctban(string mahoadonban,  string maxe, int soluong, int giaban)
        {
            

            try
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[taoctban]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Thêm tham số cho stored procedure
                    cmd.Parameters.AddWithValue("@mahd", mahoadonban);
                    cmd.Parameters.AddWithValue("@maxe", maxe);
                    cmd.Parameters.AddWithValue("@soluong", soluong);
                    cmd.Parameters.AddWithValue("@dongia", giaban);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
        }
        public void suaxemay(string maxe, string tenxe, string hangxe,string namsx,string tinhtrang,string nguongoc,int giaxe, byte[]hinhanh)
        {
         

           try
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[suaxemay]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Thêm tham số cho stored procedure
                    cmd.Parameters.AddWithValue("@maxe", maxe);
                    cmd.Parameters.AddWithValue("@tenxe", tenxe);
                    cmd.Parameters.AddWithValue("@hangxe", hangxe);
                    cmd.Parameters.AddWithValue("@namsx", namsx);
                    cmd.Parameters.AddWithValue("@tinhtrang", tinhtrang);
                    cmd.Parameters.AddWithValue("@nguongoc", nguongoc);
                    cmd.Parameters.AddWithValue("@giaxe", giaxe);
                    cmd.Parameters.AddWithValue("@anh", hinhanh);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
        }

        public void nhaplaixemay (string maxe ,int soluong)
        {


            try
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[nhalaixemay]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Thêm tham số cho stored procedure
                    cmd.Parameters.AddWithValue("@maxe", maxe);
                    cmd.Parameters.AddWithValue("@sl", soluong);
                    
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
        }

        public void dongketnoi()
        {
            conn.Close();
            conn.Dispose();
        }
    }
}
