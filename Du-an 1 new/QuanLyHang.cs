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
    public partial class QuanLyHang : Form
    {
        public QuanLyHang()
        {
            InitializeComponent();
            LoadProductData();
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }



        private void LoadProductData()
        {
            using (var context = new ShoeStore2Entities())
            {
                var products = context.Products.Select(prod => new
                {
                    prod.ProductID,
                    prod.ProductName,
                    prod.Descriptions,
                    prod.Price,
                    prod.Stock,
                    prod.Size,
                    prod.Color
                }).ToList();

                dataGridView1.DataSource = products;
            }
        }
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                guna2TextBox5.Text = selectedRow.Cells["ProductName"].Value.ToString();
                guna2TextBox1.Text = selectedRow.Cells["Descriptions"].Value.ToString();
                guna2TextBox2.Text = selectedRow.Cells["Price"].Value.ToString();
                guna2TextBox3.Text = selectedRow.Cells["Stock"].Value.ToString();
                guna2TextBox4.Text = selectedRow.Cells["Size"].Value.ToString();
                guna2TextBox6.Text = selectedRow.Cells["Color"].Value.ToString();
            }
        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            guna2TextBox5.Text = string.Empty;
            guna2TextBox1.Text = string.Empty;
            guna2TextBox2.Text = string.Empty;
            guna2TextBox3.Text = string.Empty;
            guna2TextBox4.Text = string.Empty;
            guna2TextBox6.Text = string.Empty;
            guna2TextBox7.Text = string.Empty;
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            string productName = guna2TextBox5.Text;
            string descriptions = guna2TextBox1.Text;
            if (!decimal.TryParse(guna2TextBox2.Text, out decimal price))
            {
                MessageBox.Show("Gia chi chua so.");
                return;
            }
            if (!int.TryParse(guna2TextBox3.Text, out int stock))
            {
                MessageBox.Show("hang ton kho chi chua so.");
                return;
            }
            string size = guna2TextBox4.Text;
            string color = guna2TextBox6.Text;

            if (string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(descriptions) || string.IsNullOrEmpty(size) || string.IsNullOrEmpty(color))
            {
                MessageBox.Show("Phai dien tat ca o trong.");
                return;
            }

            if (!Regex.IsMatch(productName, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Ten giay chi chua chu cai");
                return;
            }

            if (!Regex.IsMatch(color, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Mau cau giay chi chua chu cai.");
                return;
            }

            if (!Regex.IsMatch(size, @"^\d+$"))
            {
                MessageBox.Show("Size giay chi chua chu so.");
                return;
            }

            using (var context = new ShoeStore2Entities())
            {
                var newProduct = new Product
                {
                    ProductName = productName,
                    Descriptions = descriptions,
                    Price = price,
                    Stock = stock,
                    Size = size,
                    Color = color
                };

                context.Products.Add(newProduct);
                context.SaveChanges();
            }

            LoadProductData();
        }

        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Moi chon san pham de update.");
                return;
            }

            var selectedRow = dataGridView1.SelectedRows[0];
            int productId = (int)selectedRow.Cells["ProductID"].Value;

            string productName = guna2TextBox5.Text;
            string descriptions = guna2TextBox1.Text;
            if (!decimal.TryParse(guna2TextBox2.Text, out decimal price))
            {
                MessageBox.Show("Gia chi chua so.");
                return;
            }
            if (!int.TryParse(guna2TextBox3.Text, out int stock))
            {
                MessageBox.Show("Hang ton kho chi chua so.");
                return;
            }
            string size = guna2TextBox4.Text;
            string color = guna2TextBox6.Text;


            if (string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(descriptions) || string.IsNullOrEmpty(size) || string.IsNullOrEmpty(color))
            {
                MessageBox.Show("Phai dien tat ca cac o.");
                return;
            }

            if (!Regex.IsMatch(productName, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Ten giay chi co chu cai.");
                return;
            }

            if (!Regex.IsMatch(color, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Mau cua giay chi co chu cai.");
                return;
            }

            if (!Regex.IsMatch(size, @"^\d+$"))
            {
                MessageBox.Show("Size giay chi chua so.");
                return;
            }

            using (var context = new ShoeStore2Entities())
            {
                var product = context.Products.Find(productId);
                if (product != null)
                {
                    product.ProductName = productName;
                    product.Descriptions = descriptions;
                    product.Price = price;
                    product.Stock = stock;
                    product.Size = size;
                    product.Color = color;
                    context.SaveChanges();
                }
            }


            LoadProductData();
        }

        private void guna2Button5_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Chon san pham de xoa.");
                return;
            }

            var selectedRow = dataGridView1.SelectedRows[0];
            int productId = (int)selectedRow.Cells["ProductID"].Value;

            var result = MessageBox.Show("Ban co chac ban muon xoa du lieu ve chiec giay nay?", "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (var context = new ShoeStore2Entities())
                {
                    var product = context.Products.Find(productId);
                    if (product != null)
                    {
                        context.Products.Remove(product);
                        context.SaveChanges();
                    }
                }


                LoadProductData();
            }
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            string searchValue = guna2TextBox7.Text;

            using (var context = new ShoeStore2Entities())
            {
                var products = context.Products
                    .Where(prod => prod.ProductName.Contains(searchValue))
                    .Select(prod => new
                    {
                        prod.ProductID,
                        prod.ProductName,
                        prod.Descriptions,
                        prod.Price,
                        prod.Stock,
                        prod.Size,
                        prod.Color
                    }).ToList();

                dataGridView1.DataSource = products;

                if (products.Count == 0)
                {
                    MessageBox.Show("Du lieu ve chiec giay khong ton tai.");
                }
            }
        }

        private void guna2Button8_Click_1(object sender, EventArgs e)
        {
            OptionsA optionsA = new OptionsA();
            optionsA.Show();
            optionsA.FormClosed += (s, args) => this.Close();
            this.Hide();
        }
    }
}
