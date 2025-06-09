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
            datag_baohanh.Visible = false;
            dataGridView_khachhang.Visible = false;

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

        int sukienbaohanhnhan = 0;

        private void btn_lammoibh_Click(object sender, EventArgs e)
        {
            datag_baohanh.Visible = true;
            combobox_baohanh.DataSource = SQLcode.Laymaxemay();
            combobox_baohanh.DisplayMember = "Mã xe";
            string a=combobox_baohanh.Text.Trim();
            DataTable v=SQLcode.timkiemtenxemay(a);
            txt_tenxebh.Text = v.Rows[0]["Tên xe"].ToString();
            txt_mabh.Text = "";
            nmb_thoihan.Value = 0;
            DataTable dt = SQLcode.laydanhsachbaohanh();
            load(datag_baohanh, dt);
            txt_checkbaohanh.Text = "";
            sukienbaohanhnhan = 0;
           
        }

        private void combobox_baohanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a = combobox_baohanh.Text.Trim();
            DataTable v = SQLcode.timkiemtenxemay(a);
            txt_tenxebh.Text = v.Rows[0]["Tên xe"].ToString();
        }

        private void datag_baohanh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (sukienbaohanhnhan == 0)
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = datag_baohanh.Rows[e.RowIndex];

                    // Nếu dùng header tiếng Việt thì cần đúng chính tả hoàn toàn!
                    try
                    {
                        txt_mabh.Text = row.Cells["Mã bảo hành"].Value?.ToString();
                        combobox_baohanh.Text = row.Cells["Mã Xe"].Value?.ToString();
                        nmb_thoihan.Value = Convert.ToDecimal(row.Cells["Thời hạn"].Value);
                        string a = combobox_baohanh.Text.Trim();
                        DataTable v = SQLcode.timkiemtenxemay(a);
                        txt_tenxebh.Text = v.Rows[0]["Tên xe"].ToString();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi hiển thị thông tin nhà cung cấp: " + ex.Message);
                    }
                }
            }
        }

        private void btn_addbh_Click(object sender, EventArgs e)
        {
            string a, b;
            int c;
            a = txt_mabh.Text.Trim();
            b = combobox_baohanh.Text.Trim();
            c = (int)nmb_thoihan.Value;
            if (a == "")
            {
                MessageBox.Show("Vui lòng nhập mã bảo hành!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if(!Regex.IsMatch(a, @"^BH\d{3}$"))
            {

                MessageBox.Show("Nhập không đúng, vui lòng nhập theo mẫu BH___ với mỗi _ là 1 số");
                return;
            }
            else if (SQLcode.timkiemmabaonhanh(a))
            {
                MessageBox.Show("Mã bảo hành đã tồn tại");
                return;
            }
            else if (SQLcode.checkxecobaohanhchua(b))
            {
                MessageBox.Show("Xe này đã có bảo hành");
                return;

            }
            else if (c < 1)
            {
                MessageBox.Show("thời hạn bảo hành không dưới 1 năm");
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show("Bạn có muốn them mã bảo hành không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                        MessageBox.Show("Thêm bảo hành thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SQLcode.thembaohanh(a, b, c);
                        DataTable dt = SQLcode.laydanhsachbaohanh();
                        load(datag_baohanh, dt);

                }

            }
        }

        private void btn_deletebh_Click(object sender, EventArgs e)
        {
            string a;
            a = txt_mabh.Text.Trim();
            if (a == "")
            {
                MessageBox.Show("Vui lòng nhập mã bảo hành!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (!Regex.IsMatch(a, @"^BH\d{3}$"))
            {

                MessageBox.Show("Nhập không đúng, vui lòng nhập theo mẫu BH___ với mỗi _ là 1 số");
                return;
            }
            else if (!SQLcode.timkiemmabaonhanh(a))
            {
                MessageBox.Show("Mã bảo hành không tồn tại");
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show("Bạn có muốn Xóa mã bảo hành không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("Xóa bảo hành thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SQLcode.Xoabaohanh(a);
                    DataTable dt = SQLcode.laydanhsachbaohanh();
                    load(datag_baohanh, dt);

                }

            }
        }

        private void btn_editbh_Click(object sender, EventArgs e)
        {
            string a;
            int c;
            a = txt_mabh.Text.Trim();
            c = (int)nmb_thoihan.Value;
            if (a == "")
            {
                MessageBox.Show("Vui lòng nhập mã bảo hành!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (!Regex.IsMatch(a, @"^BH\d{3}$"))
            {

                MessageBox.Show("Nhập không đúng, vui lòng nhập theo mẫu BH___ với mỗi _ là 1 số");
                return;
            }
            else if (!SQLcode.timkiemmabaonhanh(a))
            {
                MessageBox.Show("Mã bảo hành không tồn tại");
                return;
            }
         
            else if (c < 1)
            {
                MessageBox.Show("thời hạn bảo hành không dưới 1 năm");
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show("Bạn có muốn sữa mã bảo hành không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("Thêm bảo sữa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SQLcode.suabaohanh(a,c);
                    DataTable dt = SQLcode.laydanhsachbaohanh();
                    load(datag_baohanh, dt);

                }

            }

        }

        private void btn_searchbh_Click(object sender, EventArgs e)
        {
            string a;
            a = txt_checkbaohanh.Text.Trim();
            if (a == "")
            {
                MessageBox.Show("Vui lòng nhập mã hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (!Regex.IsMatch(a, @"^HD\d{3}$"))
            {

                MessageBox.Show("Nhập không đúng, vui lòng nhập theo mẫu HD___ với mỗi _ là 1 số");
                return;
            }
            else if (!SQLcode.hoadontontai(a))
            {
                MessageBox.Show("Mã bảo hành không tồn tại");
                return;
            }

            else
            {
                datag_baohanh.Visible = true;
                sukienbaohanhnhan = 1;
                DataTable dt = SQLcode.checkhoadonconbaohanhkhong(a,DateTime.Now);
                load(datag_baohanh, dt);
                
            }
        }

        private void btn_lammoikhachhang_Click(object sender, EventArgs e)
        {
            dataGridView_khachhang.Visible = true;
            DataTable dt = SQLcode.layhanhsachkhachhang();
            load(dataGridView_khachhang, dt);
            txt_makhachhang.Text = "";
            txt_tenkhachhang.Text = "";
            txt_sdtkhachhang.Text = "";
            txt_sotienchi.Text = "";
        }

        private void dataGridView_khachhang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = dataGridView_khachhang.Rows[e.RowIndex];

                // Nếu dùng header tiếng Việt thì cần đúng chính tả hoàn toàn!
                try
                {
                    txt_makhachhang.Text = row.Cells["Mã khách hàng"].Value?.ToString();
                    txt_tenkhachhang.Text = row.Cells["Tên khách hàng"].Value?.ToString();
                    txt_sdtkhachhang.Text = row.Cells["Số điện thoại"].Value?.ToString();
                    txt_sotienchi.Text = row.Cells["Số tiền chi"].Value?.ToString();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi hiển thị thông tin nhà cung cấp: " + ex.Message);
                }


            }
        }

        private void btn_addkhachhang_Click(object sender, EventArgs e)
        {
            string a, b, c, d;

            a = txt_makhachhang.Text.Trim();   
            b = txt_tenkhachhang.Text.Trim();
            c = txt_sdtkhachhang.Text.Trim();
            d = txt_sotienchi.Text.Trim();
            if (a == "")
            {
                MessageBox.Show("Vui lòng nhập mã Khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (!Regex.IsMatch(a, @"^KH\d{3}$"))
            {

                MessageBox.Show("Nhập không đúng, vui lòng nhập theo mẫu KH___ với mỗi _ là 1 số");
                return;
            }
            else if (SQLcode.timkiemmakhachhang(a))
            {
                MessageBox.Show("Mã khách hàng đã tồn tại");
                return;
            }
            else if (b == "")
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!Regex.IsMatch(c, @"^0[3|5|7|8|9][0-9]{8}$"))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại đúng định dạng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (SQLcode.timkiemsdtkhachhang(a,c))
            {
                MessageBox.Show("số điện thoại đã tồn tại");
                return;
            }
            else if (!Regex.IsMatch(d, @"^\d+$"))
            {
                MessageBox.Show("Vui lòng nhập số tiền chi là 1 số nguyên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                int sotienchi=int.Parse(d);
                DialogResult result = MessageBox.Show("Bạn có muốn thêm khách hàng không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("Thêm khách hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SQLcode.themkhachhang(a, b, c,sotienchi);
                    DataTable dt = SQLcode.layhanhsachkhachhang();
                    load(dataGridView_khachhang, dt);

                }

            }

        }

        private void btn_editkhachhang_Click(object sender, EventArgs e)
        {
            string a, b, c, d;

            a = txt_makhachhang.Text.Trim();
            b = txt_tenkhachhang.Text.Trim();
            c = txt_sdtkhachhang.Text.Trim();
            d = txt_sotienchi.Text.Trim();
            if (a == "")
            {
                MessageBox.Show("Vui lòng nhập mã Khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (!Regex.IsMatch(a, @"^KH\d{3}$"))
            {

                MessageBox.Show("Nhập không đúng, vui lòng nhập theo mẫu KH___ với mỗi _ là 1 số");
                return;
            }
            else if (!SQLcode.timkiemmakhachhang(a))
            {
                MessageBox.Show("Mã khách hàng không tồn tại");
                return;
            }
            else if (b == "")
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!Regex.IsMatch(c, @"^0[3|5|7|8|9][0-9]{8}$"))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại đúng định dạng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (SQLcode.timkiemsdtkhachhang(a, c))
            {
                MessageBox.Show("số điện thoại đã tồn tại");
                return;
            }
            else if (!Regex.IsMatch(d, @"^\d+$"))
            {
                MessageBox.Show("Vui lòng nhập số tiền chi là 1 số nguyên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                int sotienchi = int.Parse(d);
                DialogResult result = MessageBox.Show("Bạn có muốn sữa khách hàng không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("sữa khách hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SQLcode.suakhachhang(a, b, c, sotienchi);
                    DataTable dt = SQLcode.layhanhsachkhachhang();
                    load(dataGridView_khachhang, dt);

                }

            }
        }

        private void btn_deletekhachahang_Click(object sender, EventArgs e)
        {
            string a;

            a = txt_makhachhang.Text.Trim();
            
            if (a == "")
            {
                MessageBox.Show("Vui lòng nhập mã Khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (!Regex.IsMatch(a, @"^KH\d{3}$"))
            {

                MessageBox.Show("Nhập không đúng, vui lòng nhập theo mẫu KH___ với mỗi _ là 1 số");
                return;
            }
            else if (!SQLcode.timkiemmakhachhang(a))
            {
                MessageBox.Show("Mã khách hàng không tồn tại");
                return;
            }
            else
            {
                
                DialogResult result = MessageBox.Show("Bạn có muốn xóa khách hàng không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("xóa khách hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SQLcode.xoakhachhang(a);
                    DataTable dt = SQLcode.layhanhsachkhachhang();
                    load(dataGridView_khachhang, dt);

                }

            }
        }

        private void btn_searchkhachhang_Click(object sender, EventArgs e)
        {
            string a, b, c;

            a = txt_makhachhang.Text.Trim();
            b = txt_tenkhachhang.Text.Trim();
            c = txt_sdtkhachhang.Text.Trim();
            txt_sotienchi.Text="";
            DataTable dt = SQLcode.laydanhsachtimkiemkhachhang(a,b,c);
            load(dataGridView_khachhang, dt);
        }
    }
}
