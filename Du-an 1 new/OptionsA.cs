using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Du_an_1_new
{
    public partial class OptionsA : Form
    {
        public OptionsA()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            QuanLyNV quanLyNVForm = new QuanLyNV();
            quanLyNVForm.Show();
            this.Hide();
        }
        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            QuanLyNV quanLyNVForm = new QuanLyNV();
            quanLyNVForm.Show();
            this.Hide();
        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            QuanLyKhach quanLyKhachForm = new QuanLyKhach();
            quanLyKhachForm.Show();
            this.Hide();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            string userRole = "Admin";
            QuanLyDonHang quanLyDonHangForm = new QuanLyDonHang(userRole);
            quanLyDonHangForm.Show();
            this.Hide();
        }
    }
}
