using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAI_LY_BAN_Xe
{
    public partial class CuaHang : Form
    {
        SQLcode SQLcode = new SQLcode();
        public CuaHang()
        {
            InitializeComponent();
        }

 
        private void load(DataGridView dgv, DataTable dt)
        {                       
            dgv.DataSource = dt;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            dgv.ScrollBars = ScrollBars.Vertical;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                int totalHeight = dgv.RowCount * dgv.RowTemplate.Height + dgv.ColumnHeadersHeight;
            dgv.Height = Math.Min(totalHeight, 500);
         
        }


        private void CuaHang_Load(object sender, EventArgs e)
        {
            SQLcode.taoketnoi();
            this.Size = new Size(1742, 896);

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label45_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void label57_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_lammoincc_Click(object sender, EventArgs e)
        {
            txt_manhcc.Text = "";
                txt_tennhacc.Text = "";
                txt_diachi.Text = "";
            txt_sodienthoaincc.Text = "";
            DataTable dt=SQLcode.laydanhsachncc();
            load(datag_nhacc, dt);
        }

        private void datag_nhacc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = datag_nhacc.Rows[e.RowIndex];

                // Nếu dùng header tiếng Việt thì cần đúng chính tả hoàn toàn!
                try
                {
                    txt_manhcc.Text = row.Cells["Mã Nhà cung cấp"].Value?.ToString();
                    txt_tennhacc.Text = row.Cells["Họ tên"].Value?.ToString();
                    txt_diachi.Text = row.Cells["Địa chỉ"].Value?.ToString();
                    txt_sodienthoaincc.Text = row.Cells["Số điện thoại"].Value?.ToString();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi hiển thị thông tin nhà cung cấp: " + ex.Message);
                }
            }
        }

        private void btn_addncc_Click(object sender, EventArgs e)
        {
            string a, b, c, d;
                a = txt_manhcc.Text.Trim();
            b= txt_tennhacc.Text.Trim();
            c= txt_diachi.Text.Trim();
            d= txt_sodienthoaincc.Text.Trim();

            if (a == "" || b == "" || c == "" || d == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!Regex.IsMatch(a, @"^NCC\d{3}$"))
            {
                MessageBox.Show("Nhập không đúng, vui lòng nhập theo mẫu NCC___ với mỗi _ là 1 số");
            }
            else if (SQLcode.timkiemncc(a) != 0)
            {
                MessageBox.Show("Mã nhà cung cấp đã tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                DialogResult result = MessageBox.Show("Bạn có muốn thêm  không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {

                    if (SQLcode.themncc(a, b, c, d) > 0)
                    {
                        MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataTable dt = SQLcode.laydanhsachncc();
                        load(datag_nhacc, dt);
                        txt_manhcc.Text = "";
                        txt_tennhacc.Text = "";
                        txt_diachi.Text = "";
                        txt_sodienthoaincc.Text = "";
                    }
                    
                }
                    
                        
           }
        }

        private void btn_deletencc_Click(object sender, EventArgs e)
        {
            string a;
            a = txt_manhcc.Text.Trim();
            if (!Regex.IsMatch(a, @"^NCC\d{3}$"))
            {
                MessageBox.Show("Nhập không đúng, vui lòng nhập theo mẫu NCC___ với mỗi _ là 1 số");
            }
            else if (SQLcode.timkiemncc(a) == 0)
            {
                MessageBox.Show("Mã nhà cung cấp không tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                txt_tennhacc.Text = "";
                txt_diachi.Text = "";
                txt_sodienthoaincc.Text = "";
                DialogResult result = MessageBox.Show("Bạn có muốn xóa  không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {

                    if (SQLcode.xoancc(a) > 0)
                    {
                        
                        MessageBox.Show("Xóanhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataTable dt = SQLcode.laydanhsachncc();
                        load(datag_nhacc, dt);
                        txt_manhcc.Text = "";
                        
                    }

                }


            }
        }

        private void btn_editncc_Click(object sender, EventArgs e)
        {
            string a, b, c, d;
            a = txt_manhcc.Text.Trim();
            b = txt_tennhacc.Text.Trim();
            c = txt_diachi.Text.Trim();
            d = txt_sodienthoaincc.Text.Trim();

            if (a == "" || b == "" || c == "" || d == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!Regex.IsMatch(a, @"^NCC\d{3}$"))
            {
                MessageBox.Show("Nhập không đúng, vui lòng nhập theo mẫu NCC___ với mỗi _ là 1 số");
            }
            else if (SQLcode.timkiemncc(a) == 0)
            {
                MessageBox.Show("Mã nhà cung cấp Không tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult result = MessageBox.Show("Bạn có muốn sữa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {

                    if (SQLcode.suancc(a,b, c, d) > 0)
                    {
                        MessageBox.Show("sữa nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataTable dt = SQLcode.laydanhsachncc();
                        load(datag_nhacc, dt);
                        txt_manhcc.Text = "";
                        txt_tennhacc.Text = "";
                        txt_diachi.Text = "";
                        txt_sodienthoaincc.Text = "";
                    }

                }
            }
        }

        private void btn_searchncc_Click(object sender, EventArgs e)
        {
            string ma = txt_manhcc.Text.Trim();
            string ten = txt_tennhacc.Text.Trim();

            txt_diachi.Text = "";
            txt_sodienthoaincc.Text = "";
            DataTable dt = SQLcode.timkiemncc(ma, ten);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy nhà cung cấp nào phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                datag_nhacc.DataSource = null;
            }
            else
            {
                datag_nhacc.DataSource = dt;
                datag_nhacc.DataSource = dt;
                datag_nhacc.ReadOnly = true;
                datag_nhacc.AllowUserToAddRows = false;
                datag_nhacc.AllowUserToDeleteRows = false;
                datag_nhacc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                datag_nhacc.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
                datag_nhacc.ScrollBars = ScrollBars.Vertical;
                datag_nhacc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                int totalHeight = datag_nhacc.RowCount * datag_nhacc.RowTemplate.Height +datag_nhacc.ColumnHeadersHeight;
                datag_nhacc.Height = Math.Min(totalHeight, 500);
            }
        }

        private void radioButton_nhaphang_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = SQLcode.layhoadonnhap();
            load(datag_hoadon, dt);
            combox_mahoadon.DataSource = SQLcode.laymahoadonnhap();
            combox_mahoadon.DisplayMember = "Mã hóa đơn";
        }

        private void btn_lammoihoadon_Click(object sender, EventArgs e)
        {
            datag_hoadon.Visible = true;
            radioButton_nhaphang.Checked = true;
           combox_mahoadon.DataSource=SQLcode.laymahoadonnhap();
            combox_mahoadon.DisplayMember = "Mã hóa đơn";
            DataTable dt = SQLcode.layhoadonnhap();
            load(datag_hoadon, dt);
            btn_chitiet.Visible = true;
            btn_timkiemhoadon.Visible = true;
        }

        private void btn_chitiet_Click(object sender, EventArgs e)
        {
            btn_timkiemhoadon.Visible = false;
            if (radioButton_nhaphang.Checked)
            {
                string a=combox_mahoadon.Text.Trim();
                DataTable dt = SQLcode.laychitiethoadonnhap(a);
                load(datag_hoadon, dt);
            }
            if (radioButton_banhang.Checked)
            {
                string a = combox_mahoadon.Text.Trim();
                DataTable dt = SQLcode.laychitiethoadonban(a);
                load(datag_hoadon, dt);
            }
        }

        private void btn_timkiemhoadon_Click(object sender, EventArgs e)
        {
            DateTime dto= datapc_ngaylamhd.Value;
            string a = combox_mahoadon.Text.Trim();
            if (radioButton_nhaphang.Checked)
            {
                
                DataTable dt = SQLcode.timkiemhoadonnhap(dto);
                load(datag_hoadon, dt);
            }
            if (radioButton_banhang.Checked)
            {
                DataTable dt = SQLcode.timkiemhoadonban(dto);
                load(datag_hoadon, dt);
            }
        }

        private void radioButton_banhang_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = SQLcode.layhoadonban();
            load(datag_hoadon, dt);
            combox_mahoadon.DataSource = SQLcode.laymahoadonban();
            combox_mahoadon.DisplayMember = "Mã hóa đơn";
        }
    }
}
