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
using System.IO;
using System.Xml;
using System.Reflection;
using System.Diagnostics.Tracing;
using System.Reflection.Emit;
using System.Data.SqlClient;

namespace DAI_LY_BAN_Xe
{
    public partial class CuaHang : Form
    {
        SQLcode SQLcode = new SQLcode();
        public CuaHang()
        {
            InitializeComponent();
        }

        public CuaHang(string a,int b)
        {
            InitializeComponent();
            SQLcode.taoketnoi();
            manv = SQLcode.laymanhanvientutaikhoan(a);
            lammoithongtinbanxe();
            this.Size = new Size(1742, 930);
            ktquyenhan(b);
            
        }
        public static string LayQuyenHan(string tenTK)
        {
            string quyenHan = "";
            string connStr = "server=ADMIN\\SQLEXPRESS06; uid=hieutranminh;pwd=tranminhhieu;database=QUANLY_CUAHANG_BANXEMAY";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT QUYENHAN FROM TAIKHOAN WHERE TAIKHOAN = @taikhoan";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@taikhoan", tenTK);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        quyenHan = result.ToString();
                }

                conn.Close();
            }

            return quyenHan;
        }
        private void ktquyenhan(int b)
        {
            if (b == 2)
            {
                thốngKêToolStripMenuItem.Visible = false;
            }
        }

        int tongtien = 0;
        string manv;
        string makhban = "";
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
            dgv.Height = Math.Min(totalHeight, 330);
            
        }


        private void CuaHang_Load(object sender, EventArgs e)
        {
            comboBox_maxeban.DataSource = SQLcode.Laymaxemay();
            comboBox_maxeban.DisplayMember = "Mã xe";
            label45.Hide();
            comboBox_maxenhaplai.Hide();


            datag_baohanh.Visible = false;
            dataGridView_khachhang.Visible = false;
            data_xemay.Visible = false;
            datag_nhacc.Visible = false;
            SQLcode.laytencacnhacungcap(lb_hangxexemay);
            for (int i = 0; i < soluongnhap.Length; i++)
            {
                soluongnhap[i] = 0;
            }
            txt_giamgiaban.Text = "0";
            

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label45_Click(object sender, EventArgs e)
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
            datag_nhacc.Visible = true;
            txt_manhcc.Text = "";
            txt_tennhacc.Text = "";
            txt_diachi.Text = "";
            txt_sodienthoaincc.Text = "";
            DataTable dt = SQLcode.laydanhsachncc();
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
            else if (SQLcode.timkiemncc(a) != 0)
            {
                MessageBox.Show("Mã nhà cung cấp đã tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult result = MessageBox.Show("Bạn có muốn thêm  không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {

                    if (SQLcode.themncc(a, b, c, d) > 0)
                    {
                        MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SQLcode.laytencacnhacungcap(lb_hangxexemay);
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

                    if (SQLcode.suancc(a, b, c, d) > 0)
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

                int totalHeight = datag_nhacc.RowCount * datag_nhacc.RowTemplate.Height + datag_nhacc.ColumnHeadersHeight;
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
            combox_mahoadon.DataSource = SQLcode.laymahoadonnhap();
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
                string a = combox_mahoadon.Text.Trim();
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
        private void datag_hoadon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = datag_hoadon.Rows[e.RowIndex];

                
                try
                {
                    combox_mahoadon.Text= row.Cells["Mã hóa đơn"].Value?.ToString();
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi hiển thị thông tin nhà cung cấp: " + ex.Message);
                }
            }
        }
        private void btn_timkiemhoadon_Click(object sender, EventArgs e)
        {
            DateTime dto = datapc_ngaylamhd.Value;
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
            string a = combobox_baohanh.Text.Trim();
            DataTable v = SQLcode.timkiemtenxemay(a);
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
            else if (!Regex.IsMatch(a, @"^BH\d{3}$"))
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
                    SQLcode.suabaohanh(a, c);
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
                DataTable dt = SQLcode.checkhoadonconbaohanhkhong(a, DateTime.Now);
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
                DialogResult result = MessageBox.Show("Bạn có muốn thêm khách hàng không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("Thêm khách hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SQLcode.themkhachhang(a, b, c, sotienchi);
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
            txt_sotienchi.Text = "";
            DataTable dt = SQLcode.laydanhsachtimkiemkhachhang(a, b, c);
            load(dataGridView_khachhang, dt);
        }
        //nhap hang-----------------------------------------------------------------------------------
        string[,] nhaphang = new string[50, 6];

        decimal[] soluongnhap = new decimal[50]; // mặc định các phần tử = 0

        // Nếu muốn chắc chắn hoặc reset lại:

        byte[][] b = new byte[50][];
        int chiso = 0;           // Tổng số đơn hiện có
        int chisotrolai = 0;    // Chỉ số đơn đang hiển thị (-1 nếu chưa có đơn nào)
        string []maxenhaplai = new string[50];
        // Nút chọn ảnh
        private void checkBox_nhaplai_CheckedChanged(object sender, EventArgs e)
        {
            if( checkBox_nhaplai.Checked)
            {
                label45.Show();
                comboBox_maxenhaplai.Show();
                comboBox_maxenhaplai.DataSource = SQLcode.Laymaxemay();
                comboBox_maxenhaplai.DisplayMember = "Mã xe";
                txt_tenxe.ReadOnly = true;
                txt_xuatxu.ReadOnly = true;
                cb_tinhtrang.Enabled = false;
                txt_nxx.ReadOnly = true;
                txt_hangxecuaxe.ReadOnly = true;
                nmb_slnhap.Value = 0;
                tongtien = 0;
                txt_tongtiennhap.Text = tongtien.ToString();

                chiso = 0;
                chisotrolai = 0;
               
                for (int i = 0; i < nhaphang.GetLength(0); i++)
                {
                    for (int j = 0; j < nhaphang.GetLength(1); j++)
                    {
                        nhaphang[i, j] = "";
                    }
                }
                for (int i = 0; i < soluongnhap.Length; i++)
                {
                    soluongnhap[i] = 0;
                }
                for (int i = 0; i < b.Length; i++)
                {
                    b[i] = null;
                }
            }
            else
                {
                tongtien = 0;
                txt_tongtiennhap.Text = tongtien.ToString();

                chiso = 0;
                chisotrolai = 0;
                for (int i = 0; i < nhaphang.GetLength(0); i++)
                {
                    for (int j = 0; j < nhaphang.GetLength(1); j++)
                    {
                        nhaphang[i, j] = "";
                    }
                }
                for (int i = 0; i < soluongnhap.Length; i++)
                {
                    soluongnhap[i] = 0;
                }
                for (int i = 0; i < b.Length; i++)
                {
                    b[i] = null;
                }
                for (int i = 0; i < maxenhaplai.Length; i++)
                {
                    maxenhaplai[i] = "";   
                }
                label45.Hide();
                comboBox_maxenhaplai.Hide();
                txt_tenxe.ReadOnly = false;
                txt_xuatxu.ReadOnly = false;
                cb_tinhtrang.Enabled = true;
                txt_nxx.ReadOnly = false;
                txt_hangxecuaxe.ReadOnly = false;
                txt_tenxe.Text = "";
                txt_hangxe.Text = "";
                txt_nxx.Text = "";
                cb_tinhtrang.Text = "";
                txt_nxx.Text = "";
                txt_xuatxu.Text= "";
                txt_hangxecuaxe.Text = "";
                p_nhap.Image = null;
            }
            }
        private void datag_xemxemay_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (checkBox_nhaplai.Checked)
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = datag_xemxemay.Rows[e.RowIndex];

                    comboBox_maxenhaplai.Text = row.Cells["Mã xe"].Value.ToString();
                    txt_tenxe.Text = row.Cells["Tên xe"].Value.ToString();

                    //txt_hangxe.Text = row.Cells["Hãng sản xuất"].Value.ToString();
                    txt_nxx.Text = row.Cells["Năm sản xuất"].Value.ToString();

                    cb_tinhtrang.Text = row.Cells["Tình trạng"].Value.ToString();
                    txt_xuatxu.Text = row.Cells["Nguồn gốc"].Value.ToString();
                    txt_hangxecuaxe.Text = row.Cells["Hãng sản xuất"].Value.ToString();
                    // Nếu ảnh là byte[] thì convert sang Image
                    if (row.Cells["ANH"].Value != DBNull.Value)
                    {
                        byte[] imgBytes = (byte[])row.Cells["ANH"].Value;
                        using (MemoryStream ms = new MemoryStream(imgBytes))
                        {
                            p_nhap.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        p_nhap.Image = null;
                    }
                }
            }
        }
        private void comboBox_maxenhaplai_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = SQLcode.laythongtinxeban(comboBox_maxenhaplai.Text.Trim());

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                txt_tenxe.Text = row["TENXE"].ToString();
                //txt_hangxe.Text = row["HANGSX"].ToString();
                txt_nxx.Text = row["NAMSX"].ToString();
                cb_tinhtrang.Text = row["TINHTRANG"].ToString();
                txt_xuatxu.Text = row["NGUONGOC"].ToString();
                txt_hangxecuaxe.Text = row["HANGSX"].ToString();
                if (row["ANH"] != DBNull.Value)
                {
                    byte[] imgBytes = (byte[])row["ANH"];
                    using (MemoryStream ms = new MemoryStream(imgBytes))
                    {
                        p_nhap.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    p_nhap.Image = null;
                }


            }
        }
        private void p_nhap_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                p_nhap.Image = Image.FromFile(openFileDialog.FileName);
                
            }
        }


        // Convert hình sang mảng byte
        byte[] hinhanhnhap(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        // Convert byte sang hình
        Image HinhAnhTuByte(byte[] data)
        {
            if (data == null) return null;
            using (MemoryStream ms = new MemoryStream(data))
            {
                return Image.FromStream(ms);
            }
        }

        // Nút Lùi
        private void button3_Click(object sender, EventArgs e)
        {

            if (chisotrolai > 0)
            {

                chisotrolai--;

                LoadData(chisotrolai);
                if (checkBox_nhaplai.Checked)
                {
                    comboBox_maxenhaplai.Text = maxenhaplai[chisotrolai];
                }
            }

            else
            {
                MessageBox.Show("Đây là đơn đầu tiên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Nút Tiến
        private void button1_Click(object sender, EventArgs e)
        {
            if (chisotrolai < chiso)
            {
                chisotrolai++;
                LoadData(chisotrolai);
                if (checkBox_nhaplai.Checked)
                {
                    comboBox_maxenhaplai.Text = maxenhaplai[chisotrolai];
                }
                if (chisotrolai == chiso)
                {
                    ClearInputs();
                    comboBox_maxenhaplai.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Đây là đơn cuối cùng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Nút Thêm mới
        private void button5_Click(object sender, EventArgs e)
        {
            if (chisotrolai == chiso || chiso == 0)
            {
                if (chiso >= nhaphang.GetLength(0))
                {
                    MessageBox.Show("Danh sách đã đầy", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (nmb_slnhap.Value == 0)
                {
                    MessageBox.Show("Số lượng nhập không được bằng 0", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (checkBox_nhaplai.Checked)
                {
                    maxenhaplai[chiso] = comboBox_maxenhaplai.Text.Trim();
                }
                nhaphang[chiso, 0] = txt_hangxecuaxe.Text.Trim();
                nhaphang[chiso, 1] = txt_tenxe.Text;
                nhaphang[chiso, 2] = txt_xuatxu.Text;
                nhaphang[chiso, 3] = cb_tinhtrang.Text;
                nhaphang[chiso, 4] = txt_nxx.Text;
                nhaphang[chiso, 5] = txt_dongia.Text;
                soluongnhap[chiso] = nmb_slnhap.Value;

                if (p_nhap.Image != null)
                {
                    byte[] imgBytes = hinhanhnhap(p_nhap.Image);
                    b[chiso] = new byte[imgBytes.Length];
                    Array.Copy(imgBytes, b[chiso], imgBytes.Length);
                }
                else
                {
                    b[chiso] = null;
                }
                int dongia = int.Parse(txt_dongia.Text);
                tongtien = tongtien + dongia * (int)nmb_slnhap.Value;
                txt_tongtiennhap.Text = tongtien.ToString();
                chiso++;
                chisotrolai = chiso;
                MessageBox.Show("Thêm đơn mới thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs();
            }
            else
            {
                MessageBox.Show("Vui lòng về đơn cuối cùng để thêm đơn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Nút Xóa
        private void button4_Click(object sender, EventArgs e)
        {
            if (chisotrolai == chiso && chiso > 0)
            {
                
                chiso--;  // Giảm số lượng đơn
                chisotrolai = chiso;
                tongtien = tongtien - (int.Parse(nhaphang[chiso, 5]) * (int)soluongnhap[chiso]);
                txt_tongtiennhap.Text = tongtien.ToString();

                MessageBox.Show("Xóa đơn thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs();

                if (chisotrolai >= 0)
                    LoadData(chisotrolai);
                else
                    p_nhap.Image = null;
            }
            else
            {
                MessageBox.Show("Vui lòng về đơn cuối cùng để xóa đơn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Load dữ liệu theo chỉ số
        private void LoadData(int index)
        {
            if (index >= 0 && index < chiso)
            {
                txt_hangxecuaxe.Text= nhaphang[index, 0];
                txt_tenxe.Text = nhaphang[index, 1];
                txt_xuatxu.Text = nhaphang[index, 2];
                cb_tinhtrang.Text = nhaphang[index, 3];
                txt_nxx.Text = nhaphang[index, 4];
                txt_dongia.Text = nhaphang[index, 5];
                nmb_slnhap.Value = soluongnhap[index];
                p_nhap.Image = HinhAnhTuByte(b[index]);
            }
        }

        // Xóa trắng ô nhập
        private void ClearInputs()
        {

            txt_tenxe.Text = "";
            txt_xuatxu.Text = "";
            cb_tinhtrang.Text = "";
            txt_nxx.Text = "";
            txt_dongia.Text = "";
            nmb_slnhap.Value = 0;
            p_nhap.Image = null;
            txt_hangxecuaxe.Text = "";
        }
        private void btn_nhapxe_Click(object sender, EventArgs e)
        {
            if (nmb_slnhap.Value == 0)
            {
                MessageBox.Show("Số lượng nhập không được bằng 0", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = MessageBox.Show("Bạn có muốn nhập hàng hành không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                nhaphang[chiso, 0] = txt_hangxecuaxe.Text.Trim();
            nhaphang[chiso, 1] = txt_tenxe.Text;
            nhaphang[chiso, 2] = txt_xuatxu.Text;
            nhaphang[chiso, 3] = cb_tinhtrang.Text;
            nhaphang[chiso, 4] = txt_nxx.Text;
            nhaphang[chiso, 5] = txt_dongia.Text;
            soluongnhap[chiso] = nmb_slnhap.Value;
            if (checkBox_nhaplai.Checked)
            {
                maxenhaplai[chiso] = comboBox_maxenhaplai.Text.Trim();
            }
            int dongia = int.Parse(txt_dongia.Text);
            tongtien = tongtien + dongia * (int)nmb_slnhap.Value;
            txt_tongtiennhap.Text = tongtien.ToString();
            if (p_nhap.Image != null)
            {
                byte[] imgBytes = hinhanhnhap(p_nhap.Image);
                b[chiso] = new byte[imgBytes.Length];
                Array.Copy(imgBytes, b[chiso], imgBytes.Length);
            }
            else
            {
                b[chiso] = null;
            }
            for (int i = 0; i <= chiso; i++)
            {
                for (int j = 0; j <= 5; j++)
                {
                    if (nhaphang[i, j] == "")
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin xe o tat ca hoa don", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

            }
           
                if (checkBox_nhaplai.Checked) {
                    string mahoadon = SQLcode.laymahoadonlonnhat();
                    mahoadon = TangMaTuDong(mahoadon);
                    DateTime ngay = DateTime.Now;
                    string nhacc = SQLcode.laymanhacungcap(txt_hangxe.Text.Trim());
                    SQLcode.taohoadonnhap(mahoadon, ngay, tongtien, manv, nhacc);
                    for (int i = 0; i <= chiso; i++)
                    {
                        string maxe=maxenhaplai[i];
                        decimal sl = soluongnhap[i];
                        int dongianhap = int.Parse(nhaphang[i, 5]);
                        SQLcode.taoctnhap(mahoadon, maxe, (int)sl, dongianhap);
                        MessageBox.Show(mahoadon + " " + maxe + " " + dongianhap + " " + sl);
                        SQLcode.nhaplaixemay(maxe, (int)sl);
                    }
                    for (int i = 0; i < maxenhaplai.Length; i++)
                    {
                        maxenhaplai[i] = "";
                    }

                }
                else
                {
                    string mahoadon = SQLcode.laymahoadonlonnhat();
                    mahoadon = TangMaTuDong(mahoadon);
                    string maxe = SQLcode.laymaxemaylonnhat();
                    maxe = TangMaTuDong(maxe);
                    DateTime ngay = DateTime.Now;
                    string nhacc = SQLcode.laymanhacungcap(txt_hangxe.Text.Trim());
                    SQLcode.taohoadonnhap(mahoadon, ngay, tongtien, manv, nhacc);
                    for (int i = 0; i <= chiso; i++)
                    {
                        string tenxe = nhaphang[i, 1];
                        string namsx = nhaphang[i, 4];
                        string tinhtrang = nhaphang[i, 3];
                        string nguongoc = nhaphang[i, 2];
                        string hangxe = nhaphang[i, 0];
                        byte[] hinhanh = b[i];
                        decimal sl = soluongnhap[i];
                        SQLcode.taoxemay(maxe, tenxe, hangxe, namsx, tinhtrang, nguongoc, hinhanh, (int)sl);

                        int dongianhap = int.Parse(nhaphang[i, 5]);
                        MessageBox.Show(mahoadon + " " + maxe + " " + dongianhap + " " + sl);
                        SQLcode.taoctnhap(mahoadon, maxe, (int)sl, dongianhap);
                        maxe = TangMaTuDong(maxe);
                    }
                }
                MessageBox.Show("Nhập Hàng Thành Công ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tongtien = 0;
                txt_tongtiennhap.Text = tongtien.ToString();

                chiso = 0;
                chisotrolai = 0;
                comboBox_maxeban.DataSource = SQLcode.Laymaxemay();
                comboBox_maxeban.DisplayMember = "Mã xe";

                for (int i = 0; i < nhaphang.GetLength(0); i++)
                {
                    for (int j = 0; j < nhaphang.GetLength(1); j++)
                    {
                        nhaphang[i, j] = "";
                    }
                }


                for (int i = 0; i < soluongnhap.Length; i++)
                {
                    soluongnhap[i] = 0;
                }
                for (int i = 0; i < b.Length; i++)
                {
                    b[i] = null;
                }
                DataTable dt = SQLcode.layxemay();
                load(datag_xemxemay, dt);
                lammoithongtinbanxe();
                return;

            }



        }

        public string TangMaTuDong(string ma)
        {
            // Lấy phần tiền tố (chữ) và phần số
            string prefix = new string(ma.TakeWhile(char.IsLetter).ToArray());
            string numberPart = new string(ma.SkipWhile(char.IsLetter).ToArray());

            // Chuyển phần số sang int và tăng lên 1
            int so = int.Parse(numberPart) + 1;

            // Tạo lại chuỗi với định dạng số có 3 chữ số (ND002, ND003,...)
            return prefix + so.ToString("D3");
        }
        private void lb_hangxexemay_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lb_hangxexemay.SelectedItem != null)
            {
                // Dùng nó an toàn nè
                txt_hangxe.Text = lb_hangxexemay.SelectedItem.ToString();
                // hoặc ép kiểu nếu dùng DataRowView hay object phức tạp
            }
            else
            {
                // Có thể bỏ trống, hoặc thông báo, hoặc return sớm
            }
        }

        private void btn_lammoixe_Click(object sender, EventArgs e)
        {
            datag_xemxemay.Visible = true;
            ClearInputs();
            DataTable dt = SQLcode.layxemay();
            load(datag_xemxemay, dt);
        }
        //---------------------------------------------------------------------------------------------- bán

        int chisoban = 0;           // Tổng số đơn hiện có
        int chisotrolaiban = 0;    // Chỉ số đơn đang hiển thị (-1 nếu chưa có đơn nào)
        string[,] cthoadonbanhang = new string[50, 3];
        private void comboBox_maxeban_SelectedIndexChanged(object sender, EventArgs e)
        {


            DataTable dt = SQLcode.laythongtinxeban(comboBox_maxeban.Text.Trim());

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                txt_tenxeban.Text = row["TENXE"].ToString();
                txt_hangxeban.Text = row["HANGSX"].ToString();
                txt_sxban.Text = row["NAMSX"].ToString();
                txt_tinhtrangban.Text = row["TINHTRANG"].ToString();
                txt_xxban.Text = row["NGUONGOC"].ToString();
                txt_giaxeban.Text = row["GIABAN"].ToString();
                int soluong = Convert.ToInt32(row["SOLUONG"]);
                nud_slban.Maximum = soluong;

            }

        }

        private void txt_sdtkhban_Leave(object sender, EventArgs e)
        {
            makhban = SQLcode.laymakhtusdt(txt_sdtkhban.Text.Trim());
            if (makhban == "")
                MessageBox.Show("Số điện thoại không tồn tại vui lòng nhập lại hoặc tạo mới khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                MessageBox.Show("Mã khách hàng của bạn là: " + makhban, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void txt_vocherban_Leave(object sender, EventArgs e)
        {
            if (txt_vocherban.Text == "")
            {
                txt_giamgiaban.Text = "0";
            }
            else if (txt_vocherban.Text == "ton") { txt_giamgiaban.Text = "30"; }
            else if (txt_vocherban.Text == "hieu") { txt_giamgiaban.Text = "20"; }
            else if (txt_vocherban.Text == "phuc") { txt_giamgiaban.Text = "10"; }
            else
            {
                txt_giamgiaban.Text = "0";
                MessageBox.Show("mã voucher không tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }
        private void LoadDataban(int index)
        {
            if (index >= 0 && index < chisoban)
            {

                comboBox_maxeban.Text = cthoadonbanhang[index, 0];
                nud_slban.Value = Convert.ToInt32(cthoadonbanhang[index, 1]);
                txt_giaxeban.Text = cthoadonbanhang[index, 2];
            }
        }
        private void btn_quaylaidontruoc_Click(object sender, EventArgs e)
        {
            if (chisotrolaiban > 0)
            {

                chisotrolaiban--;

                LoadDataban(chisotrolaiban);
            }

            else
            {
                MessageBox.Show("Đây là đơn đầu tiên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        int tongtienban = 0;
        private void btn_themdon_Click(object sender, EventArgs e)
        {
            if (nud_slban.Value == 0)
            {
                MessageBox.Show("Vui lòng chọn số lượng mua nếu không chọn được thì có lẽ xe đã hết hàng cảm ơn");
                return;
            }
            else if (chisotrolaiban == chisoban || chisoban == 0)
            {
                if (chiso >= cthoadonbanhang.GetLength(0))
                {
                    MessageBox.Show("Danh sách đã đầy", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                cthoadonbanhang[chisoban, 0] = comboBox_maxeban.Text;
                cthoadonbanhang[chisoban, 1] = nud_slban.Value.ToString();
                cthoadonbanhang[chisoban, 2] = txt_giaxeban.Text;

                int dongia = int.Parse(txt_giaxeban.Text);
                tongtienban = tongtienban + dongia * (int)nud_slban.Value;
                txt_tongtienban.Text = tongtienban.ToString();
                chisoban++;
                chisotrolaiban = chisoban;
                MessageBox.Show("Thêm đơn mới thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                comboBox_maxeban.DataSource = SQLcode.Laymaxemay();
                comboBox_maxeban.DisplayMember = "Mã xe";
            }
            else
            {
                MessageBox.Show("Vui lòng về đơn cuối cùng để thêm đơn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_donsau_Click(object sender, EventArgs e)
        {
            if (chisotrolaiban < chisoban)
            {
                chisotrolaiban++;
                LoadDataban(chisotrolaiban);
                if (chisotrolaiban == chisoban)
                {
                    comboBox_maxeban.DataSource = SQLcode.Laymaxemay();
                    comboBox_maxeban.DisplayMember = "Mã xe";
                }
            }
            else
            {
                MessageBox.Show("Đây là đơn cuối cùng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_xoadon_Click(object sender, EventArgs e)
        {
            if (chisotrolaiban == chisoban && chisoban > 0)
            {

                chisoban--;  // Giảm số lượng đơn
                chisotrolaiban = chisoban;
                tongtienban = tongtienban - int.Parse(cthoadonbanhang[chisoban, 2]) * int.Parse(cthoadonbanhang[chisoban, 1]);
                txt_tongtienban.Text = tongtienban.ToString();


                MessageBox.Show("Xóa đơn thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);


                if (chisotrolaiban >= 0)
                    LoadDataban(chisotrolaiban);
                else
                    comboBox_maxeban.DataSource = SQLcode.Laymaxemay();
                comboBox_maxeban.DisplayMember = "Mã xe";
            }
            else
            {
                MessageBox.Show("Vui lòng về đơn cuối cùng để xóa đơn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_refreshban_Click(object sender, EventArgs e)
        {
            txt_sdtkhban.Text = "";
            txt_giamgiaban.Text = "0";
            txt_vocherban.Text = "";
            comboBox_maxeban.DataSource = SQLcode.Laymaxemay();
            comboBox_maxeban.DisplayMember = "Mã xe";
        }

        private void btn_sellban_Click(object sender, EventArgs e)
        {
            if (nud_slban.Value == 0)
            {
                MessageBox.Show("Vui lòng chọn số lượng mua nếu không chọn được thì có lẽ xe đã hết hàng cảm ơn");
                return;
            }
            else if (txt_sdtkhban.Text == "")
            {
                MessageBox.Show("Vui lòng nhập số điện thoại khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show("Bạn có muốn nhập bán không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    cthoadonbanhang[chisoban, 0] = comboBox_maxeban.Text;
                cthoadonbanhang[chisoban, 1] = nud_slban.Value.ToString();
                cthoadonbanhang[chisoban, 2] = txt_giaxeban.Text;


                int dongia = int.Parse(txt_giaxeban.Text);
                tongtienban = tongtienban + dongia * (int)nud_slban.Value;
                int phantramgiamgia = 1;
                if (txt_giamgiaban.Text.Trim() == "0")
                {
                    phantramgiamgia = 0;
                }
                if (txt_giamgiaban.Text.Trim() == "")
                {
                    phantramgiamgia = 0;
                }
                else if (txt_giamgiaban.Text.Trim() == "30")
                {
                    phantramgiamgia = 30;
                }
                else if (txt_giamgiaban.Text.Trim() == "20")
                {
                    phantramgiamgia = 20;
                }
                else if (txt_giamgiaban.Text.Trim() == "10")
                {
                    phantramgiamgia = 10;
                }
                txt_tongtienban.Text = tongtienban.ToString();
                int tongtiensaukhigiamgia = tongtienban / 100 * (100 - phantramgiamgia);
                MessageBox.Show("Tổng tiền bán là " + txt_tongtienban.Text + " Tổng tiền sau khi giảm giá là" + tongtiensaukhigiamgia);
               

                    string mahoadonban = SQLcode.laymahoadonbanlonnhat();
                    mahoadonban = TangMaTuDong(mahoadonban);


                    DateTime ngay = DateTime.Now;

                    SQLcode.taohoadonban(mahoadonban, ngay, tongtiensaukhigiamgia, makhban, manv);
                    for (int i = 0; i <= chisoban; i++)
                    {
                        string maxe = cthoadonbanhang[i, 0];
                        int soluong = int.Parse(cthoadonbanhang[i, 1]);
                        int giaban = int.Parse(cthoadonbanhang[i, 2]);
                        MessageBox.Show(mahoadonban + " " + maxe + " " + soluong + " " + giaban);
                        SQLcode.taoctban(mahoadonban, maxe, soluong, giaban);

                    }
                    MessageBox.Show("Nhập bán Thành Công ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tongtienban = 0;
                    txt_tongtienban.Text = tongtienban.ToString();

                    chisoban = 0;
                    chisotrolaiban = 0;
                    comboBox_maxeban.DataSource = SQLcode.Laymaxemay();
                    comboBox_maxeban.DisplayMember = "Mã xe";
                    for (int i = 0; i < 50; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            cthoadonbanhang[i, j] = "";
                        }
                    }
                    lammoithongtinbanxe();


                }
            }








            //----------------------------------------------------------------------------------------------
        }

        private void txt_lammoixemay_Click(object sender, EventArgs e)
        {
            comboBox_xemay.DataSource = SQLcode.Laymaxemay();
            comboBox_xemay.DisplayMember = "Mã xe";
            data_xemay.Visible = true;
            pictureb_xemay.Image = null;
            DataTable dt = SQLcode.layxemay();
            load(data_xemay, dt);
           
        }

      

        private void data_xemay_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = data_xemay.Rows[e.RowIndex];

                comboBox_xemay.Text = row.Cells["Mã xe"].Value.ToString();
                txt_tenxemay.Text = row.Cells["Tên xe"].Value.ToString();
                txt_hangxemay.Text = row.Cells["Hãng sản xuất"].Value.ToString();
                txt_namsxxemay.Text = row.Cells["Năm sản xuất"].Value.ToString();
                comb_tinhtrangxemay.Text = row.Cells["Tình trạng"].Value.ToString();
                txt_nguongocxemay.Text = row.Cells["Nguồn gốc"].Value.ToString();
                txt_soluongxemay.Text = row.Cells["Số lượng"].Value.ToString();
                txt_giabanxemay.Text = row.Cells["Giá bán"].Value.ToString();

                // Nếu ảnh là byte[] thì convert sang Image
                if (row.Cells["ANH"].Value != DBNull.Value)
                {
                    byte[] imgBytes = (byte[])row.Cells["ANH"].Value;
                    using (MemoryStream ms = new MemoryStream(imgBytes))
                    {
                        pictureb_xemay.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    pictureb_xemay.Image = null;
                }
            }
        }

        private void pictureb_xemay_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureb_xemay.Image = Image.FromFile(openFileDialog.FileName);
                
            }
        }

        private void comboBox_xemay_SelectedIndexChanged(object sender, EventArgs e)
        {



            DataTable dt = SQLcode.laythongtinxeban(comboBox_xemay.Text.Trim());

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                txt_tenxemay.Text = row["TENXE"].ToString();
                txt_hangxemay.Text = row["HANGSX"].ToString();
                txt_namsxxemay.Text = row["NAMSX"].ToString();
                comb_tinhtrangxemay.Text = row["TINHTRANG"].ToString();
                txt_nguongocxemay.Text = row["NGUONGOC"].ToString();
                txt_giabanxemay.Text = row["GIABAN"].ToString();
                txt_soluongxemay.Text = row["SOLUONG"].ToString();
                if (row["ANH"] != DBNull.Value)
                {
                    byte[] imgBytes = (byte[])row["ANH"];
                    using (MemoryStream ms = new MemoryStream(imgBytes))
                    {
                        pictureb_xemay.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    pictureb_xemay.Image = null;
                }
            }


        }


        private void btn_suaxemay_Click(object sender, EventArgs e)
        {
            string giaxetext = txt_giabanxemay.Text.Trim();

            if (!int.TryParse(giaxetext, out int giaxe) || giaxe <= 0)
            {
                MessageBox.Show("Giá bán phải là một số nguyên dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show("Bạn có muốn sửa xe không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {

                    string tenxe = txt_tenxemay.Text.Trim();
                    string hangxe = txt_hangxemay.Text.Trim();
                    string namsx = txt_namsxxemay.Text.Trim();
                    string tinhtrang = comb_tinhtrangxemay.Text.Trim();
                    string nguongoc = txt_nguongocxemay.Text.Trim();


                    // Kiểm tra giá bán có phải là số dương không

                    byte[] hinhanh = null;
                    if (pictureb_xemay.Image != null)
                    {
                        hinhanh = hinhanhnhap(pictureb_xemay.Image);
                    }
                    SQLcode.suaxemay(comboBox_xemay.Text.Trim(), tenxe, hangxe, namsx, tinhtrang, nguongoc, giaxe, hinhanh);
                    MessageBox.Show("Sữa xe thành công", "Thông báo", MessageBoxButtons.OK);
                    DataTable dt = SQLcode.layxemay();
                   load(data_xemay, dt);
                    
                    lammoithongtinbanxe();
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        int trangHienTai = 0;
        DataTable dulieu;
        private void hienp1(string maxe,string tenxe,string soluong, string giaban, byte[] hinhanh)
        {
            l_name1.Text = tenxe;
            l_sl1.Text = soluong;
            l_sell1.Text = giaban;
            l_daban1.Text = maxe;
            if (hinhanh != null)
            {
                using (MemoryStream ms = new MemoryStream(hinhanh))
                {
                    pictureBox1.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureBox1.Image = null;
            }
            p1.Show();
        }
        private void hienp2(string maxe, string tenxe, string soluong, string giaban, byte[] hinhanh)
        {
            l_name2.Text = tenxe;
            l_sl2.Text = soluong;
            l_sell2.Text = giaban;
            l_daban2.Text = maxe;
            if (hinhanh != null)
            {
                using (MemoryStream ms = new MemoryStream(hinhanh))
                {
                    pictureBox2.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureBox2.Image = null;
            }
            p2.Show();
        }
        private void hienp3(string maxe, string tenxe, string soluong, string giaban, byte[] hinhanh)
        {
            l_name3.Text = tenxe;
            l_sl3.Text = soluong;
            l_sell3.Text = giaban;
            l_daban3.Text = maxe;
            if (hinhanh != null)
            {
                using (MemoryStream ms = new MemoryStream(hinhanh))
                {
                    pictureBox3.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureBox3.Image = null;
            }
            p3.Show();
        }
        private void hienp4(string maxe, string tenxe, string soluong, string giaban, byte[] hinhanh)
        {
            l_name4.Text = tenxe;
            l_sl4.Text = soluong;
            l_sell4.Text = giaban;
            l_daban4.Text = maxe;
            if (hinhanh != null)
            {
                using (MemoryStream ms = new MemoryStream(hinhanh))
                {
                    pictureBox4.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureBox4.Image = null;
            }
            p4.Show();
        }
        private void hienp5(string maxe, string tenxe, string soluong, string giaban, byte[] hinhanh)
        {
            l_name5.Text = tenxe;
            l_sl5.Text = soluong;
            l_sell5.Text = giaban;
            l_daban5.Text = maxe;
            if (hinhanh != null)
            {
                using (MemoryStream ms = new MemoryStream(hinhanh))
                {
                    pictureBox5.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureBox5.Image = null;
            }
            p5.Show();
        }
       
        private void hienp6(string maxe, string tenxe, string soluong, string giaban, byte[] hinhanh)
        {
            l_name6.Text = tenxe;
            l_sl6.Text = soluong;
            l_sell6.Text = giaban;
            l_daban6.Text = maxe;
            if (hinhanh != null)
            {
                using (MemoryStream ms = new MemoryStream(hinhanh))
                {
                    pictureBox6.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureBox6.Image = null;
            }
            p6.Show();
        }
        private void hienp7(string maxe, string tenxe, string soluong, string giaban, byte[] hinhanh)
        {
            l_name7.Text = tenxe;
            l_sl7.Text = soluong;
            l_sell7.Text = giaban;
            l_daban7.Text = maxe;
            if (hinhanh != null)
            {
                using (MemoryStream ms = new MemoryStream(hinhanh))
                {
                    pictureBox7.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureBox7.Image = null;
            }
            p7.Show();
        }
        private void hienp8(string maxe, string tenxe, string soluong, string giaban, byte[] hinhanh)
        {
            l_name8.Text = tenxe;
            l_sl8.Text = soluong;
            l_sell8.Text = giaban;
            l_daban8.Text = maxe;
            if (hinhanh != null)
            {
                using (MemoryStream ms = new MemoryStream(hinhanh))
                {
                    pictureBox8.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureBox8.Image = null;
            }
            p8.Show();
        }


        private void lammoithongtinbanxe()
        {
            dulieu = SQLcode.layxemay(); // Lấy dữ liệu từ DB
            trangHienTai = 0;
            hienTrang(trangHienTai);
        }

        private void hienTrang(int trang)
        {
            // Ẩn tất cả các panel
            p1.Hide(); p2.Hide(); p3.Hide(); p4.Hide();
            p5.Hide(); p6.Hide(); p7.Hide(); p8.Hide();

            int start = trang * 8;
            int end = Math.Min(start + 8, dulieu.Rows.Count);

            for (int i = start; i < end; i++)
            {
                DataRow row = dulieu.Rows[i];
                string maxe = row["Mã xe"].ToString();
                string tenxe = row["Tên xe"].ToString();
                string soluong = row["Số lượng"].ToString();
                string giaban = row["Giá bán"].ToString();
                byte[] hinhanh = row["ANH"] != DBNull.Value ? (byte[])row["ANH"] : null;

                switch (i - start)
                {
                    case 0: hienp1(  maxe, tenxe, soluong, giaban, hinhanh); break;
                    case 1: hienp2( maxe, tenxe, soluong, giaban, hinhanh); break;
                    case 2: hienp3( maxe, tenxe, soluong, giaban, hinhanh); break;
                    case 3: hienp4( maxe, tenxe, soluong, giaban, hinhanh); break;
                    case 4: hienp5( maxe, tenxe, soluong, giaban, hinhanh); break;
                    case 5: hienp6( maxe, tenxe, soluong, giaban, hinhanh); break;
                    case 6: hienp7( maxe, tenxe, soluong, giaban, hinhanh); break;
                    case 7: hienp8( maxe, tenxe, soluong, giaban, hinhanh); break;
                }
            }

            label44.Text = $"Trang {trang + 1} / {Math.Ceiling(dulieu.Rows.Count / 8.0)}";
        }
        

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) //trang sau
        {
            int soTrangToiDa = (int)Math.Ceiling(dulieu.Rows.Count / 8.0);
            if (trangHienTai + 1 < soTrangToiDa)
            {
                trangHienTai++;
                hienTrang(trangHienTai);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)// trang truowc
        {
            if (trangHienTai > 0)
            {
                trangHienTai--;
                hienTrang(trangHienTai);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            comboBox_maxeban.Text= l_daban1.Text.Trim();
            comboBox_maxeban_SelectedIndexChanged(this, EventArgs.Empty);
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            comboBox_maxeban.Text = l_daban2.Text.Trim();
            comboBox_maxeban_SelectedIndexChanged(this, EventArgs.Empty);
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            comboBox_maxeban.Text = l_daban3.Text.Trim();
            comboBox_maxeban_SelectedIndexChanged(this, EventArgs.Empty);
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            comboBox_maxeban.Text = l_daban4.Text.Trim();
            comboBox_maxeban_SelectedIndexChanged(this, EventArgs.Empty);
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            comboBox_maxeban.Text = l_daban5.Text.Trim();
            comboBox_maxeban_SelectedIndexChanged(this, EventArgs.Empty);
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            comboBox_maxeban.Text = l_daban6.Text.Trim();
            comboBox_maxeban_SelectedIndexChanged(this, EventArgs.Empty);
        }
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            comboBox_maxeban.Text = l_daban7.Text.Trim();
            comboBox_maxeban_SelectedIndexChanged(this, EventArgs.Empty);
        }
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            comboBox_maxeban.Text = l_daban8.Text.Trim();
            comboBox_maxeban_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void tàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tenTK = layTenTk.TAIKHOAN;

            if (string.IsNullOrEmpty(tenTK))
            {
                MessageBox.Show("Không có tài khoản đăng nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Lấy quyền hạn từ cơ sở dữ liệu
            string quyenHan = LayQuyenHan(tenTK);

            // Hiển thị form phù hợp theo quyền hạn
            if (quyenHan == "admin")
            {
                form_admin frm = new form_admin(tenTK);
                frm.ShowDialog();
            }
            else if (quyenHan == "nhanvien")
            {
                form_NV frm = new form_NV(tenTK); // Giả sử bạn có form này
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Không xác định được quyền hạn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void doanhSốToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmThongKe a = new frmThongKe();
            a.ShowDialog();
        }














        //------------------------------------------------------------------------------------------------------------------------------
    }
}

