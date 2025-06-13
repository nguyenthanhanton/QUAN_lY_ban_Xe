using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace DAI_LY_BAN_Xe
{

    public partial class frmInReport : Form
    {
        private readonly DataTable _detail;
        private readonly DataTable _topXe;
        private readonly DataTable _topNhap;
        private readonly DataTable _topKH;
        //  DataTable _data;

        public frmInReport(DataTable detail, DataTable topXe, DataTable topNhap, DataTable topKH)
        {
            InitializeComponent();
            //  _data = data;
            _detail = detail;
            _topXe = topXe;
            _topNhap = topNhap;
            _topKH = topKH;
        }

        private void frmInReport_Load(object sender, EventArgs e)
        {

            reportViewer1.LocalReport.ReportPath = "Report1.rdlc";
        reportViewer1.LocalReport.DataSources.Clear();

        // DataSet1 – bảng chi tiết
        var rds1 = new ReportDataSource("DataSet1", _detail);
        reportViewer1.LocalReport.DataSources.Add(rds1);

        // DataSet2 – bảng TopXe
        var rds2 = new ReportDataSource("DataSet2", _topXe);
        reportViewer1.LocalReport.DataSources.Add(rds2);

        var  rds3 = new ReportDataSource("DataSet3", _topNhap);
        reportViewer1.LocalReport.DataSources.Add(rds3);

        var rds4 = new ReportDataSource("DataSetKHChiNhieu", _topKH);
        reportViewer1.LocalReport.DataSources.Add(rds4);

            reportViewer1.RefreshReport();
        }
    }
}
