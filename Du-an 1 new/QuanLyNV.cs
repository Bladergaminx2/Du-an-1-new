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
    public partial class QuanLyNV : Form
    {
        public QuanLyNV()
        {
            InitializeComponent();
            LoadEmployeeData();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

        }


        private void LoadEmployeeData()
        {
            using (var context = new ShoeStore2Entities())
            {
                var employees = context.Employees.Select(emp => new
                {
                    emp.EmployeeID,
                    emp.EmployeeName,
                    emp.EUserName,
                    emp.Email,
                    emp.EPhone,
                    emp.EHireDate
                }).ToList();

                dataGridView1.DataSource = employees;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];

                guna2TextBox7.Text = selectedRow.Cells["EmployeeID"].Value.ToString();
                guna2TextBox5.Text = selectedRow.Cells["EmployeeName"].Value.ToString();
                guna2TextBox1.Text = selectedRow.Cells["EUserName"].Value.ToString();
                guna2TextBox2.Text = selectedRow.Cells["Email"].Value.ToString();
                guna2TextBox3.Text = selectedRow.Cells["EPhone"].Value.ToString();
                guna2DateTimePicker1.Value = Convert.ToDateTime(selectedRow.Cells["EHireDate"].Value);

            }
        }



        private void guna2Button2_Click_2(object sender, EventArgs e)
        {
            guna2TextBox1.Text = string.Empty;
            guna2TextBox2.Text = string.Empty;
            guna2TextBox3.Text = string.Empty;
            guna2TextBox5.Text = string.Empty;
            guna2TextBox6.Text = string.Empty;
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
            string employeeName = guna2TextBox5.Text;
            string username = guna2TextBox1.Text;
            string email = guna2TextBox2.Text;
            string phone = guna2TextBox3.Text;
            DateTime hireDate = guna2DateTimePicker1.Value;
            string password = guna2TextBox6.Text;

            if (string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Khong duoc de trong o nao.");
                return;
            }

            if (!Regex.IsMatch(employeeName, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Ten nhan vien chi duoc co chu cai.");
                return;
            }

            if (!email.Contains("@"))
            {
                MessageBox.Show("Email phai chua dau '@'.");
                return;
            }

            if (!Regex.IsMatch(phone, @"^\d{10}$"))
            {
                MessageBox.Show("SDT phai chua 10 so va chi duoc chua chu so.");
                return;
            }

            using (var context = new ShoeStore2Entities())
            {
                var newEmployee = new Employee
                {
                    EmployeeName = employeeName,
                    EUserName = username,
                    Email = email,
                    EPhone = phone,
                    EHireDate = hireDate,
                    EPassword = password
                };

                context.Employees.Add(newEmployee);
                context.SaveChanges();
            }

            LoadEmployeeData();
            MessageBox.Show("Them nhan vien thanh cong.");
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Moi chon nhan vien can update.");
                return;
            }

            var selectedRow = dataGridView1.SelectedRows[0];
            int employeeId = (int)selectedRow.Cells["EmployeeID"].Value;

            string employeeName = guna2TextBox5.Text;
            string username = guna2TextBox1.Text;
            string email = guna2TextBox2.Text;
            string phone = guna2TextBox3.Text;
            DateTime hireDate = guna2DateTimePicker1.Value;
            string password = guna2TextBox6.Text;

            if (string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Khong duoc de trong o nao.");
                return;
            }

            if (!Regex.IsMatch(employeeName, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Ten nhan vien chi duoc chua chu cai.");
                return;
            }

            if (!email.Contains("@"))
            {
                MessageBox.Show("Email phai chua '@'.");
                return;
            }

            if (!Regex.IsMatch(phone, @"^\d{10}$"))
            {
                MessageBox.Show("SDT phai chua 10 so va chi duoc chua chu so.");
                return;
            }

            using (var context = new ShoeStore2Entities())
            {
                var employee = context.Employees.Find(employeeId);
                if (employee != null)
                {
                    employee.EmployeeName = employeeName;
                    employee.EUserName = username;
                    employee.Email = email;
                    employee.EPhone = phone;
                    employee.EHireDate = hireDate;
                    employee.EPassword = password;
                    context.SaveChanges();
                }
            }

            LoadEmployeeData();
            MessageBox.Show("Cap nhat thong tin nhan vien thanh cong.");
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Moi chon nhan vien de xoa.");
                return;
            }

            var selectedRow = dataGridView1.SelectedRows[0];
            int employeeId = (int)selectedRow.Cells["EmployeeID"].Value;

            var result = MessageBox.Show("Ban chac muon xoa du lieu ve nhan vien nay?", "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (var context = new ShoeStore2Entities())
                {
                    var employee = context.Employees.Find(employeeId);
                    if (employee != null)
                    {
                        context.Employees.Remove(employee);
                        context.SaveChanges();
                    }
                }

                LoadEmployeeData();
                MessageBox.Show("Du lieu nhan vien da duoc xoa thanh cong.");
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string searchValue = guna2TextBox7.Text;

            using (var context = new ShoeStore2Entities())
            {
                var employees = context.Employees
                    .Where(emp => emp.EmployeeName.Contains(searchValue) || emp.EUserName.Contains(searchValue))
                    .Select(emp => new
                    {
                        emp.EmployeeID,
                        emp.EmployeeName,
                        emp.EUserName,
                        emp.Email,
                        emp.EPhone,
                        emp.EHireDate
                    }).ToList();

                dataGridView1.DataSource = employees;

                if (employees.Count == 0)
                {
                    MessageBox.Show("Nhan vien ko ton tai.");
                }
            }
        }
    }
}
