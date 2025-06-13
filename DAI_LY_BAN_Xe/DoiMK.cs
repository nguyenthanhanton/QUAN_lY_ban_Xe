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
    public partial class form_DoiMK: Form
    {
        private string tenTaiKhoan;
        public form_DoiMK(string taikhoan)
        {
            InitializeComponent();
            tenTaiKhoan = taikhoan;
        }

        private void c_showpass_CheckedChanged(object sender, EventArgs e)
        {
            if (c_showpass.Checked)
            {
                txt_MKmoi.UseSystemPasswordChar = false;
                txt_MKcu.UseSystemPasswordChar = false;
                txt_XNmk.UseSystemPasswordChar = false;
            }
            else
            {
                txt_MKcu.UseSystemPasswordChar = true;
                txt_MKmoi.UseSystemPasswordChar = true;
                txt_XNmk.UseSystemPasswordChar = true;
            }
        }
        

        private void btn_Xacnhandoi_Click(object sender, EventArgs e)
        {
            string mkCu = txt_MKcu.Text.Trim();
            string mkMoi = txt_MKmoi.Text.Trim();
            string mkXacNhan = txt_XNmk.Text.Trim();

            if (string.IsNullOrEmpty(mkMoi))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới.");
                return;
            }

            if (mkMoi != mkXacNhan)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp.");
                return;
            }

            string connStr = "server=DESKTOP-392TCLG\\SQLEXPRESS01; uid=banxe;pwd=1;database=QUANLY_CUAHANG_BANXEMAY";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Kiểm tra mật khẩu cũ
                string checkQuery = "SELECT COUNT(*) FROM TAIKHOAN WHERE TAIKHOAN = @tk AND MATKHAU = @mkcu";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@tk", tenTaiKhoan);
                checkCmd.Parameters.AddWithValue("@mkcu", mkCu);

                int count = (int)checkCmd.ExecuteScalar();
                if (count == 0)
                {
                    MessageBox.Show("Mật khẩu cũ không đúng.");
                    return;
                }

                // Cập nhật mật khẩu mới
                string updateQuery = "UPDATE TAIKHOAN SET MATKHAU = @mkmoi WHERE TAIKHOAN = @tk";
                SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@mkmoi", mkMoi);
                updateCmd.Parameters.AddWithValue("@tk", tenTaiKhoan);
                updateCmd.ExecuteNonQuery();

                MessageBox.Show("Đổi mật khẩu thành công.");
                this.Close();
            }
        }

        private void form_DoiMK_Load(object sender, EventArgs e)
        {

        }
    }
}
