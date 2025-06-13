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
    public partial class form_ThemTK: Form
    {
        public form_ThemTK()
        {
            InitializeComponent();
        }

        private void btn_XN_Click(object sender, EventArgs e)
        {
            string taikhoan = txt_TenTK.Text.Trim();
            string matkhau = txt_Mk.Text.Trim();
            string quyenhan = txt_QuyenHan.Text.Trim();
            string manv = txt_MaNv.Text.Trim();
            string tennv = txt_TenNV.Text.Trim();
            string chucvu = txt_ChucVu.Text.Trim();
            string email = txt_Email.Text.Trim();
            string sdt = txt_SDTad.Text.Trim();

            if (taikhoan == "" || matkhau == "" || quyenhan == "" || manv == "" || tennv == "" || chucvu == "" || email == "" || sdt == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            string connStr = "server=DESKTOP-392TCLG\\SQLEXPRESS01; uid=banxe;pwd=1;database=QUANLY_CUAHANG_BANXEMAY";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Kiểm tra trùng mã nhân viên
                string checkMaNV = "SELECT COUNT(*) FROM NHANVIEN WHERE MANV = @manv";
                using (SqlCommand cmd = new SqlCommand(checkMaNV, conn))
                {
                    cmd.Parameters.AddWithValue("@manv", manv);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Mã nhân viên đã tồn tại.");
                        return;
                    }
                }
                string checkTaiKhoan = "SELECT COUNT(*) FROM TAIKHOAN WHERE TAIKHOAN = @taikhoan";
                using (SqlCommand cmd = new SqlCommand(checkTaiKhoan, conn))
                {
                    cmd.Parameters.AddWithValue("@taikhoan", taikhoan);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Tên tài khoản đã tồn tại.");
                        return;
                    }
                }
                // Kiểm tra trùng email
                string checkEmail = "SELECT COUNT(*) FROM NHANVIEN WHERE EMAIL = @Email";
                using (SqlCommand cmd = new SqlCommand(checkEmail, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Email đã tồn tại.");
                        return;
                    }
                }

                // Kiểm tra trùng SDT
                string checkSDT = "SELECT COUNT(*) FROM NHANVIEN WHERE SDTNV = @SDT";
                using (SqlCommand cmd = new SqlCommand(checkSDT, conn))
                {
                    cmd.Parameters.AddWithValue("@SDT", sdt);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Số điện thoại đã tồn tại.");
                        return;
                    }
                }

                // Thêm vào bảng NHANVIEN
                string insertNhanVien = @"
            INSERT INTO NHANVIEN (MANV, TENNV, CHUCVU, EMAIL, SDTNV)
            VALUES (@manv, @tennv, @chucvu, @email, @sdt)";
                using (SqlCommand cmd = new SqlCommand(insertNhanVien, conn))
                {
                    cmd.Parameters.AddWithValue("@manv", manv);
                    cmd.Parameters.AddWithValue("@tennv", tennv);
                    cmd.Parameters.AddWithValue("@chucvu", chucvu);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@sdt", sdt);
                    cmd.ExecuteNonQuery();
                }

                // Thêm vào bảng TAIKHOAN
                string insertTaiKhoan = @"
            INSERT INTO TAIKHOAN (TAIKHOAN, MATKHAU, QUYENHAN, MANV)
            VALUES (@tk, @mk, @qh, @manv)";
                using (SqlCommand cmd = new SqlCommand(insertTaiKhoan, conn))
                {
                    cmd.Parameters.AddWithValue("@tk", taikhoan);
                    cmd.Parameters.AddWithValue("@mk", matkhau);
                    cmd.Parameters.AddWithValue("@qh", quyenhan);
                    cmd.Parameters.AddWithValue("@manv", manv);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Thêm tài khoản thành công.");
                this.Close();
            }
        }

        private void lbl_TenTK_Click(object sender, EventArgs e)
        {

        }

        private void form_ThemTK_Load(object sender, EventArgs e)
        {

        }
    }
}
