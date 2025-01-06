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

namespace Du_an_1_new
{
    public partial class QuanLyKhach : Form
    {
        public QuanLyKhach()
        {
            InitializeComponent();
            LoadCustomerData();
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                guna2TextBox5.Text = selectedRow.Cells["Name"].Value.ToString();
                guna2TextBox1.Text = selectedRow.Cells["Phone"].Value.ToString();
                guna2TextBox2.Text = selectedRow.Cells["Address"].Value.ToString();
            }
        }



        private void LoadCustomerData()
        {
            using (var context = new ShoeStore2Entities())
            {
                var customers = context.Customers.Select(cus => new
                {
                    cus.CustomerID,
                    cus.Name,
                    cus.Phone,
                    cus.Address
                }).ToList();

                dataGridView1.DataSource = customers;
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            guna2TextBox5.Text = string.Empty;
            guna2TextBox1.Text = string.Empty;
            guna2TextBox2.Text = string.Empty;
            guna2TextBox7.Text = string.Empty;
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            OptionsA optionsA = new OptionsA();
            optionsA.Show();
            optionsA.FormClosed += (s, args) => this.Close();
            this.Hide();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            string customerName = guna2TextBox5.Text;
            string phone = guna2TextBox1.Text;
            string address = guna2TextBox2.Text;

            if (string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Phai dien tat ca o trong.");
                return;
            }

            if (!Regex.IsMatch(customerName, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Khach hang chi duoc chua chu cai.");
                return;
            }

            if (!Regex.IsMatch(phone, @"^\d{10}$"))
            {
                MessageBox.Show("sdt phai co 10 so va chi duoc chua so.");
                return;
            }

            using (var context = new ShoeStore2Entities())
            {
                var newCustomer = new Customer
                {
                    Name = customerName,
                    Phone = phone,
                    Address = address
                };

                context.Customers.Add(newCustomer);
                context.SaveChanges();
            }

            LoadCustomerData();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Moi chon khac hang can duoc update.");
                return;
            }

            var selectedRow = dataGridView1.SelectedRows[0];
            int customerId = (int)selectedRow.Cells["CustomerID"].Value;

            string customerName = guna2TextBox5.Text;
            string phone = guna2TextBox1.Text;
            string address = guna2TextBox2.Text;

            if (string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Phai dien tat ca o trong.");
                return;
            }

            if (!Regex.IsMatch(customerName, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Ten khach hang phai chua chu cai");
                return;
            }

            if (!Regex.IsMatch(phone, @"^\d{10}$"))
            {
                MessageBox.Show("sdt phai co 10 so va chi duoc chua so.");
                return;
            }

            using (var context = new ShoeStore2Entities())
            {
                var customer = context.Customers.Find(customerId);
                if (customer != null)
                {
                    customer.Name = customerName;
                    customer.Phone = phone;
                    customer.Address = address;
                    context.SaveChanges();
                }
            }

            LoadCustomerData();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Moi chon khac hang can duoc xoa.");
                return;
            }

            var selectedRow = dataGridView1.SelectedRows[0];
            int customerId = (int)selectedRow.Cells["CustomerID"].Value;

            var result = MessageBox.Show("Ban co chac muon xoa khach hang nay?", "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (var context = new ShoeStore2Entities())
                {
                    var customer = context.Customers.Find(customerId);
                    if (customer != null)
                    {
                        context.Customers.Remove(customer);
                        context.SaveChanges();
                    }
                }

                LoadCustomerData();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string searchValue = guna2TextBox7.Text;

            using (var context = new ShoeStore2Entities())
            {
                var customers = context.Customers
                    .Where(cus => cus.Name.Contains(searchValue) || cus.Phone.Contains(searchValue))
                    .Select(cus => new
                    {
                        cus.CustomerID,
                        cus.Name,
                        cus.Phone,
                        cus.Address
                    }).ToList();

                dataGridView1.DataSource = customers;

                if (customers.Count == 0)
                {
                    MessageBox.Show("Khach hang ko ton tai.");
                }
            }
        }
    }
}
