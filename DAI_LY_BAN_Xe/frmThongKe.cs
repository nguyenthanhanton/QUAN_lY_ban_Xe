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
    public partial class frmThongKe : Form
    {
       
        public frmThongKe()
        {
            InitializeComponent();
        }

        private void frmThongKe_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void frmThongKe_Load(object sender, EventArgs e)
        {

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
        private void btnThucHien_Click(object sender, EventArgs e)
        {
            SQLcode a = new SQLcode();
            a.taoketnoi(); // mở kết nối trước

            try
            {
                SqlCommand cmd = new SqlCommand("dbo.sp_ThongKeTongQuat", a.conn); 
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TuNgay", dtpTuNgay.Value.Date);
                cmd.Parameters.AddWithValue("@DenNgay", dtpDenNgay.Value.Date);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

               // dgvHoaDon.DataSource = dt;
                load(dgvHoaDon, dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                a.dongketnoi(); // đóng kết nối
            }
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            DataTable dt = ((DataTable)dgvHoaDon.DataSource);

            if (dt != null && dt.Rows.Count > 0)
            {
                // Lấy thêm dữ liệu từ thủ tục "sp_ThongKe_TopXeBanChay"
                SQLcode sql = new SQLcode();
                sql.taoketnoi();

                SqlDataAdapter daTopXe = new SqlDataAdapter("sp_ThongKe_TopXeBanChay", sql.conn);
                daTopXe.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtTopXe = new DataTable();
                daTopXe.Fill(dtTopXe);
                
                SqlDataAdapter daTopNhap = new SqlDataAdapter("sp_TopXeDuocNhapNhieuNhat", sql.conn);
                daTopNhap.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtTopNhap = new DataTable();
                daTopNhap.Fill(dtTopNhap);

                SqlDataAdapter daTopKH = new SqlDataAdapter("sp_KhachHangChiNhieuNhat1", sql.conn);
                daTopKH.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtTopKH = new DataTable();
                daTopKH.Fill(dtTopKH);

                SqlDataAdapter daTopNV = new SqlDataAdapter("sp_ThongKe_DoanhThu_TheoNhanVien", sql.conn);
                daTopNV.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtTopNV = new DataTable();
                daTopNV.Fill(dtTopNV);

                sql.dongketnoi();

                // Truyền cả hai bảng vào form InReport
                frmInReport frm = new frmInReport(dt, dtTopXe,dtTopNhap,dtTopKH,dtTopNV);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để in báo cáo.");
            }
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }    
}
