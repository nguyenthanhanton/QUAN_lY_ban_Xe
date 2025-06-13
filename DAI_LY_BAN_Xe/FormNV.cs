using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAI_LY_BAN_Xe
{
    public partial class form_NV: Form
    {
        private string taiKhoanDangNhap;
        public form_NV(string tenTaiKhoan)
        {
            InitializeComponent();
            taiKhoanDangNhap = tenTaiKhoan;
        }


        private void form_NV_Load(object sender, EventArgs e)
        {

            HienThiThongTinTaiKhoan(taiKhoanDangNhap);
        }

        private void HienThiThongTinTaiKhoan(string tenTK)
        {
            string connStr = "server=DESKTOP-392TCLG\\SQLEXPRESS01; uid=banxe;pwd=1;database=QUANLY_CUAHANG_BANXEMAY"; // chỉnh lại tên CSDL

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = @"
            SELECT TK.TAIKHOAN, TK.QUYENHAN, TK.Manv, 
                   NV.TENNV, NV.CHUCVU, NV.EMAIL, NV.SDTNV
            FROM TAIKHOAN TK
            LEFT JOIN NHANVIEN NV ON TK.MANV = NV.MANV
            WHERE TK.TAIKHOAN = @taikhoan";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@taikhoan", tenTK);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txt_TenTKnv.Text = reader["TAIKHOAN"].ToString();
                        txt_QuyenHannv.Text = reader["QUYENHAN"].ToString();
                        txt_MaNv.Text = reader["Manv"].ToString();
                        txt_TenNV.Text = reader["TENNV"].ToString();
                        txt_ChucVunv.Text = reader["CHUCVU"].ToString();
                        txt_Emailnv.Text = reader["EMAIL"].ToString();
                        txt_SDTNV.Text = reader["SDTNV"].ToString();
                    }

                    reader.Close();
                }

                conn.Close();
            }
        }

        private void btn_Doimk_Click(object sender, EventArgs e)
        {
            form_DoiMK frm = new form_DoiMK(layTenTk.TAIKHOAN);
            frm.ShowDialog();
        }

        private void btn_SuaTT_Click(object sender, EventArgs e)
        {
            string maNV = txt_MaNv.Text.Trim();
            string tenNV = txt_TenNV.Text.Trim();
            string chucVu = txt_ChucVunv.Text.Trim();
            string email = txt_Emailnv.Text.Trim();
            string sdt = txt_SDTNV.Text.Trim();


            if (string.IsNullOrEmpty(maNV))
            {
                MessageBox.Show("Không tìm thấy mã nhân viên để sửa.");
                return;
            }

            string connStr = "server=DESKTOP-392TCLG\\SQLEXPRESS01; uid=banxe;pwd=1;database=QUANLY_CUAHANG_BANXEMAY";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // 1. Kiểm tra trùng EMAIL
                string emailQuery = "SELECT COUNT(*) FROM NHANVIEN WHERE EMAIL = @em AND MANV != @ma";
                using (SqlCommand emailCmd = new SqlCommand(emailQuery, conn))
                {
                    emailCmd.Parameters.AddWithValue("@em", email);
                    emailCmd.Parameters.AddWithValue("@ma", maNV);
                    int countEmail = (int)emailCmd.ExecuteScalar();
                    if (countEmail > 0)
                    {
                        MessageBox.Show("Email đã được sử dụng bởi nhân viên khác.");
                        return;
                    }
                }

                // 2. Kiểm tra trùng SDT
                string sdtQuery = "SELECT COUNT(*) FROM NHANVIEN WHERE SDTNV = @sdt AND MANV != @ma";
                using (SqlCommand sdtCmd = new SqlCommand(sdtQuery, conn))
                {
                    sdtCmd.Parameters.AddWithValue("@sdt", sdt);
                    sdtCmd.Parameters.AddWithValue("@ma", maNV);
                    int countSdt = (int)sdtCmd.ExecuteScalar();
                    if (countSdt > 0)
                    {
                        MessageBox.Show("Số điện thoại đã được sử dụng bởi nhân viên khác.");
                        return;
                    }
                }

                // 3. Cập nhật nếu không trùng
                string updateQuery = @"
            UPDATE NHANVIEN 
            SET TENNV = @ten, CHUCVU = @cv, EMAIL = @em, SDTNV = @sdt 
            WHERE MANV = @ma";

                using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@ten", tenNV);
                    updateCmd.Parameters.AddWithValue("@cv", chucVu);
                    updateCmd.Parameters.AddWithValue("@em", email);
                    updateCmd.Parameters.AddWithValue("@sdt", sdt);
                    updateCmd.Parameters.AddWithValue("@ma", maNV);

                    int rows = updateCmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin thành công.");
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thất bại.");
                    }
                }

                conn.Close();
            }
        }
    }
}
