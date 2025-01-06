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
    public partial class QuanLyDonHang : Form
    {
        private string _userRole;

        public QuanLyDonHang(string userRole)
        {
            InitializeComponent();
            _userRole = userRole;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            LoadOrderData();
        }

        private void LoadOrderData()
        {
            using (var context = new ShoeStore2Entities())
            {
                var orders = from order in context.Orders
                             join detail in context.OrderDetails on order.OrderID equals detail.OrderID
                             join customer in context.Customers on order.CustomerID equals customer.CustomerID
                             join employee in context.Employees on order.EmployeeID equals employee.EmployeeID
                             join product in context.Products on detail.ProductID equals product.ProductID
                             select new
                             {
                                 order.OrderID,
                                 CustomerName = customer.Name,
                                 EmployeeName = employee.EmployeeName,
                                 order.OrderDate,
                                 ProductName = product.ProductName,
                                 detail.Quantity,
                                 detail.Note
                             };
                dataGridView1.DataSource = orders.ToList();
            }
        }



        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                guna2TextBox7.Text = selectedRow.Cells["OrderID"].Value.ToString();
                guna2TextBox5.Text = selectedRow.Cells["CustomerName"].Value.ToString();
                guna2TextBox1.Text = selectedRow.Cells["EmployeeName"].Value.ToString();
                guna2TextBox2.Text = selectedRow.Cells["ProductName"].Value.ToString();
                guna2TextBox4.Text = selectedRow.Cells["Quantity"].Value.ToString();
                guna2TextBox3.Text = selectedRow.Cells["Note"].Value.ToString();
                guna2DateTimePicker1.Value = selectedRow.Cells["OrderDate"].Value is DateTime date ? date : DateTime.Now;
            }
        }






        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            guna2TextBox1.Clear();
            guna2TextBox2.Clear();
            guna2TextBox3.Clear();
            guna2TextBox4.Clear();
            guna2TextBox5.Clear();
            guna2TextBox7.Clear();
            guna2DateTimePicker1.Value = DateTime.Now;
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            using (var context = new ShoeStore2Entities())
            {
                var customer = context.Customers.SingleOrDefault(c => c.Name == guna2TextBox5.Text);
                var employee = context.Employees.SingleOrDefault(emp => emp.EmployeeName == guna2TextBox1.Text);
                var product = context.Products.SingleOrDefault(p => p.ProductName == guna2TextBox2.Text);

                if (customer == null || employee == null || product == null)
                {
                    MessageBox.Show("Moi dien du lieu hop le vao cac o.");
                    return;
                }

                int quantity = int.Parse(guna2TextBox4.Text);
                DateTime orderDate = guna2DateTimePicker1.Value;
                string note = guna2TextBox3.Text;

                if (product.Stock < quantity)
                {
                    MessageBox.Show("Het hang trong kho.");
                    return;
                }

                var order = new Order
                {
                    CustomerID = customer.CustomerID,
                    EmployeeID = employee.EmployeeID,
                    OrderDate = orderDate,
                    Status = "Pending"
                };
                context.Orders.Add(order);
                context.SaveChanges();

                var orderDetail = new OrderDetail
                {
                    OrderID = order.OrderID,
                    ProductID = product.ProductID,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    Note = note
                };
                context.OrderDetails.Add(orderDetail);
                context.SaveChanges();

                var bill = new Bill
                {
                    OrderID = order.OrderID,
                    InvoiceDate = orderDate,
                    TotalQuantity = quantity,
                    TotalAmount = orderDetail.TotalPrice
                };
                context.Bills.Add(bill);
                context.SaveChanges();

                product.Stock -= quantity;
                context.SaveChanges();

                LoadOrderData();
                MessageBox.Show("Them don hang thanh cong.");
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (_userRole == "Admin")
            {
                int orderId;
                int quantity;
                DateTime orderDate = guna2DateTimePicker1.Value;
                string note = guna2TextBox3.Text;

                if (!int.TryParse(guna2TextBox7.Text, out orderId) ||
                    !int.TryParse(guna2TextBox4.Text, out quantity))
                {
                    MessageBox.Show("Moi dien du lieu hop le vao cac o trong.");
                    return;
                }

                using (var context = new ShoeStore2Entities())
                {
                    var order = context.Orders.FirstOrDefault(o => o.OrderID == orderId);
                    if (order == null)
                    {
                        MessageBox.Show("Order not found.");
                        return;
                    }

                    var customer = context.Customers.SingleOrDefault(c => c.Name == guna2TextBox5.Text);
                    var employee = context.Employees.SingleOrDefault(emp => emp.EmployeeName == guna2TextBox1.Text);
                    var product = context.Products.SingleOrDefault(p => p.ProductName == guna2TextBox2.Text);
                    if (customer == null || employee == null || product == null)
                    {
                        MessageBox.Show("du lieu khong hop le moi dien lai.");
                        return;
                    }

                    var orderDetail = context.OrderDetails.FirstOrDefault(od => od.OrderID == orderId && od.ProductID == product.ProductID);
                    if (orderDetail == null)
                    {
                        MessageBox.Show("Order detail ko ton tai.");
                        return;
                    }

                    order.CustomerID = customer.CustomerID;
                    order.EmployeeID = employee.EmployeeID;
                    order.OrderDate = orderDate;
                    order.Status = "Pending";

                    orderDetail.Quantity = quantity;
                    orderDetail.UnitPrice = product.Price;
                    orderDetail.Note = note;

                    var bill = context.Bills.FirstOrDefault(b => b.OrderID == orderId);
                    if (bill != null)
                    {
                        bill.TotalQuantity = quantity;
                        bill.TotalAmount = orderDetail.TotalPrice;
                    }

                    context.SaveChanges();
                    LoadOrderData();
                    MessageBox.Show("Order cap nhat thanh cong.");
                }
            }
            else
            {
                MessageBox.Show("Chi admin moi thuc hien duoc thao tac nay.");
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            if (_userRole == "Admin")
            {
                int orderId;
                if (!int.TryParse(guna2TextBox7.Text, out orderId))
                {
                    MessageBox.Show("Moi chon don hang.");
                    return;
                }

                using (var context = new ShoeStore2Entities())
                {
                    var order = context.Orders.FirstOrDefault(o => o.OrderID == orderId);
                    if (order == null)
                    {
                        MessageBox.Show("Order ko ton tai.");
                        return;
                    }

                    var orderDetails = context.OrderDetails.Where(od => od.OrderID == orderId).ToList();
                    var bill = context.Bills.FirstOrDefault(b => b.OrderID == orderId);


                    context.OrderDetails.RemoveRange(orderDetails);
                    if (bill != null)
                    {
                        context.Bills.Remove(bill);
                    }
                    context.Orders.Remove(order);

                    context.SaveChanges();
                    LoadOrderData();
                    MessageBox.Show("Xoa don hang thanh cong");
                }
            }
            else
            {
                MessageBox.Show("Admin moi xoa dc.");
            }
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int orderId = (int)dataGridView1.SelectedRows[0].Cells["OrderID"].Value;

                using (var context = new ShoeStore2Entities())
                {
                    var bill = context.Bills.FirstOrDefault(b => b.OrderID == orderId);
                    if (bill != null)
                    {
                        var order = context.Orders.FirstOrDefault(o => o.OrderID == orderId);
                        if (order != null)
                        {
                            var customer = context.Customers.FirstOrDefault(c => c.CustomerID == order.CustomerID);
                            if (customer != null)
                            {
                                string invoiceDate = bill.InvoiceDate.HasValue ? bill.InvoiceDate.Value.ToShortDateString() : "N/A";
                                MessageBox.Show($"Invoice ID: {bill.InvoiceID}\n" +
                                                $"Order ID: {bill.OrderID}\n" +
                                                $"Invoice Date: {invoiceDate}\n" +
                                                $"Total Quantity: {bill.TotalQuantity}\n" +
                                                $"Total Amount: {bill.TotalAmount}\n" +
                                                $"Customer Phone: {customer.Phone}");
                            }
                            else
                            {
                                MessageBox.Show("Customer not found.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Order not found.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Bill not found.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Moi chon don hang muon xem hoa don.");
            }
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            int orderId;
            if (!int.TryParse(guna2TextBox7.Text, out orderId))
            {
                MessageBox.Show("Moi dien id hop le.");
                return;
            }

            using (var context = new ShoeStore2Entities())
            {
                var orders = from order in context.Orders
                             join detail in context.OrderDetails on order.OrderID equals detail.OrderID
                             where order.OrderID == orderId
                             select new
                             {
                                 order.OrderID,
                                 order.CustomerID,
                                 order.EmployeeID,
                                 order.OrderDate,
                                 detail.ProductID,
                                 detail.Quantity,
                                 detail.Note
                             };

                if (orders.Any())
                {
                    dataGridView1.DataSource = orders.ToList();
                }
                else
                {
                    MessageBox.Show("Order ko ton tai.");
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

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int orderId = (int)dataGridView1.SelectedRows[0].Cells["OrderID"].Value;

                using (var context = new ShoeStore2Entities())
                {
                    var orderDetails = context.OrderDetails.Where(od => od.OrderID == orderId).ToList();
                    if (orderDetails.Any())
                    {
                        var details = new StringBuilder();
                        foreach (var detail in orderDetails)
                        {
                            details.AppendLine($"Product ID: {detail.ProductID}");
                            details.AppendLine($"Quantity: {detail.Quantity}");
                            details.AppendLine($"Unit Price: {detail.UnitPrice}");
                            details.AppendLine($"Total Price: {detail.TotalPrice}");
                            details.AppendLine($"Note: {detail.Note}");
                            details.AppendLine();
                        }
                        MessageBox.Show(details.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Order details ko ton tai.");
                    }
                }
            }

        }
    }
}
