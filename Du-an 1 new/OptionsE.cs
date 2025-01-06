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
    public partial class OptionsE : Form
    {
        public OptionsE()
        {
            InitializeComponent();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            QuanLyKhach quanLyKhachForm = new QuanLyKhach();
            quanLyKhachForm.Show();
            this.Hide();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            string userRole = "Employee";
            QuanLyDonHang quanLyDonHangForm = new QuanLyDonHang(userRole);
            quanLyDonHangForm.Show();
            this.Hide();
        }
    }
}
