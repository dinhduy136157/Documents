using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Documents
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            HienThi();
        }
        XmlDocument doc = new XmlDocument();
        string tentep = @"C:\Users\Admin\source\repos\Documents\Documents\thongtin.xml";
        int d;
        private void HienThi()
        {
            dataTables.Rows.Clear();
            doc.Load(tentep);
            XmlNodeList DS = doc.SelectNodes("/ds/nhanvien");
            int sd = 0;
            dataTables.ColumnCount = 6;
            dataTables.Rows.Add();
            foreach (XmlNode nhan_vien in DS)
            {
                XmlNode ma_nv = nhan_vien.SelectSingleNode("@manv");
                dataTables.Rows[sd].Cells[0].Value = ma_nv.InnerText.ToString();
                XmlNode ho_ten = nhan_vien.SelectSingleNode("hoten");
                if (ho_ten != null)
                {
                    XmlNode ho = ho_ten.SelectSingleNode("ho");
                    XmlNode ten = ho_ten.SelectSingleNode("ten");
                    dataTables.Rows[sd].Cells[1].Value = ho?.InnerText; // Họ
                    dataTables.Rows[sd].Cells[2].Value = ten?.InnerText; // Tên
                }
                XmlNode gioi_tinh = nhan_vien.SelectSingleNode("gioitinh"); 
                dataTables.Rows[sd].Cells[3].Value = gioi_tinh.InnerText.ToString();
                XmlNode trinh_do = nhan_vien.SelectSingleNode("trinhdo");
                dataTables.Rows[sd].Cells[4].Value = trinh_do.InnerText.ToString();
                if (trinh_do != null && !cbTrinhDo.Items.Contains(trinh_do.InnerText))
                {
                    cbTrinhDo.Items.Add(trinh_do.InnerText);
                }
                XmlNode dia_chi = nhan_vien.SelectSingleNode("diachi");
                dataTables.Rows[sd].Cells[5].Value = dia_chi.InnerText.ToString();
                dataTables.Rows.Add();
                sd++;

            }
            if (cbTrinhDo.Items.Count > 0)
            {
                cbTrinhDo.SelectedIndex = 0;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            HienThi();
        }
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            doc.Load(tentep);
            XmlElement goc = doc.DocumentElement;
            XmlNode nhan_vien = doc.CreateElement("nhanvien");
            XmlAttribute ma_nv = doc.CreateAttribute("manv");
            ma_nv.InnerText = txtMaNV.Text;
            nhan_vien.Attributes.Append(ma_nv);
            //
            // Tạo phần tử <hoten>
            XmlNode hoten = doc.CreateElement("hoten");

            // Tạo phần tử <ho> và thêm vào <hoten>
            XmlNode txt_ho = doc.CreateElement("ho");
            txt_ho.InnerText = txtHo.Text;
            hoten.AppendChild(txt_ho);

            // Tạo phần tử <ten> và thêm vào <hoten>
            XmlNode txt_ten = doc.CreateElement("ten");
            txt_ten.InnerText = txtTen.Text;
            hoten.AppendChild(txt_ten);

            // Thêm <hoten> vào <nhanvien>
            nhan_vien.AppendChild(hoten);
            // Thêm giới tính
            XmlNode gioi_tinh = doc.CreateElement("gioitinh");
            if (radioNam.Checked)
            {
                gioi_tinh.InnerText = "Nam";
            }
            else if (radioNu.Checked)
            {
                gioi_tinh.InnerText = "Nữ";
            }
            nhan_vien.AppendChild(gioi_tinh);

            // Thêm trình độ
            XmlNode trinh_do = doc.CreateElement("trinhdo");
            trinh_do.InnerText = cbTrinhDo.SelectedItem?.ToString() ?? "";
            nhan_vien.AppendChild(trinh_do);
            XmlNode dia_chi = doc.CreateElement("diachi");
            dia_chi.InnerText = txtDiaChi.Text;
            nhan_vien.AppendChild(dia_chi);
            goc.AppendChild(nhan_vien);
            doc.Save(tentep);
            HienThi();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            doc.Load(tentep);
            XmlElement goc = doc.DocumentElement;
            XmlNode nhan_vien_cu = goc.SelectSingleNode("/ds/nhanvien[@manv='" + txtMaNV.Text + "']");
            XmlNode nhan_vien_moi = doc.CreateElement("nhanvien");
            XmlAttribute ma_nv = doc.CreateAttribute("manv");
            ma_nv.InnerText = txtMaNV.Text;
            nhan_vien_moi.Attributes.Append(ma_nv);
            XmlNode hoten = doc.CreateElement("hoten");
            XmlNode txt_ho = doc.CreateElement("ho");
            txt_ho.InnerText = txtHo.Text;
            // Tạo phần tử <ten> và thêm vào <hoten>
            XmlNode txt_ten = doc.CreateElement("ten");
            txt_ten.InnerText = txtTen.Text;
            hoten.AppendChild(txt_ho);
            hoten.AppendChild(txt_ten);
            // Thêm <hoten> vào <nhanvien>
            nhan_vien_moi.AppendChild(hoten);

            // Xử lý giới tính (RadioButton)
            XmlNode gioi_tinh = doc.CreateElement("gioitinh");
            if (radioNam.Checked)
            {
                gioi_tinh.InnerText = "Nam";
            }
            else if (radioNu.Checked)
            {
                gioi_tinh.InnerText = "Nữ";
            }
            nhan_vien_moi.AppendChild(gioi_tinh);

            // Xử lý trình độ (ComboBox)
            XmlNode trinh_do = doc.CreateElement("trinhdo");
            trinh_do.InnerText = cbTrinhDo.SelectedItem?.ToString() ?? "";
            nhan_vien_moi.AppendChild(trinh_do);

            XmlNode dia_chi = doc.CreateElement("diachi");
            dia_chi.InnerText = txtDiaChi.Text;
            nhan_vien_moi.AppendChild(dia_chi);
            goc.ReplaceChild(nhan_vien_moi, nhan_vien_cu);
            doc.Save(tentep);
            HienThi();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            doc.Load(tentep);
            XmlElement goc = doc.DocumentElement;
            XmlNode nhan_vien_xoa = goc.SelectSingleNode("/ds/nhanvien[@manv='" + txtMaNV.Text + "']");
            goc.RemoveChild(nhan_vien_xoa);
            doc.Save(tentep);
            HienThi();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            XmlNodeList DS = doc.SelectNodes("/ds/nhanvien");
            string manvtimkiem = txtMaNV.Text;
            bool found = false;

            foreach (XmlNode nhan_vien in DS)
            {
                XmlNode manvne = nhan_vien.SelectSingleNode("@manv");
                if (manvne != null && manvne.InnerText == manvtimkiem)
                {
                    XmlNode nameNode = nhan_vien.SelectSingleNode("hoten");
                    XmlNode diachi = nhan_vien.SelectSingleNode("diachi");
                    string thongTinTimKiem = $"Mã NV: {manvne.InnerText}\nHọ Tên: {nameNode?.InnerText}\nĐịa chỉ: {diachi?.InnerText}";
                    MessageBox.Show(thongTinTimKiem, "Thông tin nhân viên", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    found = true;
                    break;
                }
            }
        }
    }
}
