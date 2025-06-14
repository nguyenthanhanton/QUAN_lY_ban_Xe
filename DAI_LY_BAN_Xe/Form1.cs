using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAI_LY_BAN_Xe
{
    
    public partial class login_main : Form
    {
        SQLcode SQLcode= new SQLcode();
        public login_main()
        {
            InitializeComponent();
            
        }

        private void login_main_Load(object sender, EventArgs e)
        {
            SQLcode.taoketnoi();
        }

        private void c_showpass_CheckedChanged(object sender, EventArgs e)
        {
            if(c_showpass.Checked)
            {
                txt_password.UseSystemPasswordChar = false;
            }
            else
            {
                txt_password.UseSystemPasswordChar = true;
            }
        }

        private void btn_login_Click(object sender, EventArgs e)
        {

            string taikhoan = txt_username.Text.Trim();
            string matkhau = txt_password.Text.Trim();
            int trangthai = SQLcode.kttaikhoan(taikhoan, matkhau);
            if (trangthai == 0)
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (trangthai == 1)
            {
                this.Hide(); // Ẩn form đăng nhập

                CuaHang adminForm = new CuaHang(taikhoan,1);
                adminForm.ShowDialog();
                SQLcode.dongketnoi(); // Đóng kết nối sau khi sử dụng

            }
            else if(  trangthai == 2)
            {
                this.Hide(); // Ẩn form đăng nhập
                CuaHang adminForm = new CuaHang(taikhoan,2);
                adminForm.ShowDialog();
                SQLcode.dongketnoi(); // Đóng kết nối sau khi sử dụng

            }

                this.Show();
            txt_username.Text = "";
            txt_password.Text = "";
            SQLcode.taoketnoi();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SQLcode.dongketnoi();
                // Thoát chương trình hoặc thực hiện hành động thoát
                Application.Exit();
            }


        }
    }
}
