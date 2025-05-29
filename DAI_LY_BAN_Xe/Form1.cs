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
        public login_main()
        {
            InitializeComponent();
        }

        private void login_main_Load(object sender, EventArgs e)
        {

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
            CuaHang form2;
            form2 = new CuaHang();
            form2.Show();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Thoát chương trình hoặc thực hiện hành động thoát
                Application.Exit();
            }


        }
    }
}
