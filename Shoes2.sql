-- Tạo cơ sở dữ liệu ShoeStore2
CREATE DATABASE ShoeStore2;
GO

-- Sử dụng cơ sở dữ liệu ShoeStore2
USE ShoeStore2;
GO

-- Tạo bảng Employees
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeName NVARCHAR(150),
    EUserName NVARCHAR(50),
    Email NVARCHAR(100),
    EPhone NVARCHAR(20),
    EHireDate DATE,
    EPassword NVARCHAR(100)
);
GO

-- Tạo bảng Admins
CREATE TABLE Admins (
    AdminID INT PRIMARY KEY IDENTITY(1,1),
    AdminName NVARCHAR(150),
    AUserName NVARCHAR(50),
    AEmail NVARCHAR(100),
    APhone NVARCHAR(20),
    APassword NVARCHAR(100)
);
GO

-- Tạo bảng Customers
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50),
    Phone NVARCHAR(20),
    Address NVARCHAR(200)
);
GO

-- Tạo bảng Products
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100),
    Descriptions NVARCHAR(500),
    Price DECIMAL(10, 2),
    Stock INT,
    Size NVARCHAR(50),
    Color NVARCHAR(50)
);
GO

-- Tạo bảng Orders
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT,
    EmployeeID INT,
    OrderDate DATE,
    Status NVARCHAR(50),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);
GO

-- Tạo bảng OrderDetails
CREATE TABLE OrderDetails (
    OrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT,
    ProductID INT,
    Quantity INT,
    UnitPrice DECIMAL(10, 2),
    TotalPrice AS (Quantity * UnitPrice) PERSISTED,
    Note NVARCHAR(500),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
GO

-- Tạo bảng Bills
CREATE TABLE Bills (
    InvoiceID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT,
    InvoiceDate DATE,
    TotalQuantity INT,
    TotalAmount DECIMAL(10, 2),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID)
);
GO

-- Thêm dữ liệu vào bảng Employees
INSERT INTO Employees (EmployeeName, EUserName, Email, EPhone, EHireDate, EPassword) VALUES
('Nguyen Van A', 'nguyenvana', 'a.nguyen@example.com', '0123456789', '2022-01-10', 'password1'),
('Tran Thi B', 'tranthib', 'b.tran@example.com', '0987654321', '2023-02-15', 'password2');
GO

-- Thêm dữ liệu vào bảng Admins
INSERT INTO Admins (AdminName, AUserName, AEmail, APhone, APassword) VALUES
('Le Thi C', 'adm', 'c.le@example.com', '0123456788', '123'),
('Pham Van D', 'phamvand', 'd.pham@example.com', '0987654320', 'password4');
GO

-- Thêm dữ liệu vào bảng Customers
INSERT INTO Customers (Name, Phone, Address) VALUES
('Vo Thi E', '0123456787', '123 Đường ABC, Phường XYZ, TP. HCM'),
('Hoang Van F', '0987654319', '456 Đường DEF, Phường UVW, TP. Hà Nội');
GO

-- Thêm dữ liệu vào bảng Products
INSERT INTO Products (ProductName, Descriptions, Price, Stock, Size, Color) VALUES
('Giày Nike Air', 'Giày thể thao Nike Air', 1200.00, 100, '42', 'Đen'),
('Giày Adidas UltraBoost', 'Giày chạy bộ Adidas UltraBoost', 1300.00, 50, '44', 'Trắng');
GO

-- Thêm dữ liệu vào bảng Orders
INSERT INTO Orders (CustomerID, EmployeeID, OrderDate, Status) VALUES
(1, 1, '2023-05-10', 'Processing'),
(2, 2, '2023-06-20', 'Shipped');
GO

-- Thêm dữ liệu vào bảng OrderDetails
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice, Note) VALUES
(1, 1, 2, 1200.00, 'Ưu tiên giao buổi sáng'),
(2, 2, 1, 1300.00, 'Giao hàng tận nơi');
GO

-- Thêm dữ liệu vào bảng Bills
INSERT INTO Bills (OrderID, InvoiceDate, TotalQuantity, TotalAmount) VALUES
(1, '2023-05-15', 2, 2400.00),
(2, '2023-06-25', 1, 1300.00);
GO
