--- Table Create

/*1. Users Data Table*/
CREATE TABLE Users (
    UserID INT NOT NULL PRIMARY KEY identity(1,1),
    UserName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    MobileNo VARCHAR(15) NOT NULL,
    Address VARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL
);

/*2. Customer Data Table*/
CREATE TABLE Customer (
    CustomerID INT NOT NULL PRIMARY KEY identity(1,1),
    CustomerName VARCHAR(100) NOT NULL,
    HomeAddress VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    MobileNo VARCHAR(15) NOT NULL,
    GST_NO VARCHAR(15) NOT NULL,
    CityName VARCHAR(100) NOT NULL,
    PinCode VARCHAR(15) NOT NULL,
    NetAmount DECIMAL(10,2) NOT NULL,
    UserID INT NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

/*3. Product Data Table*/
CREATE TABLE Product (
    ProductID INT NOT NULL PRIMARY KEY identity(1,1),
    ProductName VARCHAR(100) NOT NULL,
    ProductPrice DECIMAL(10,2) NOT NULL,
    ProductCode VARCHAR(100) NOT NULL,
    Description VARCHAR(100) NOT NULL,
    UserID INT NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

/*4. Orders Data Table*/
CREATE TABLE Orders (
    OrderID INT NOT NULL PRIMARY KEY identity(1,1),
	OrderNO Varchar(10) NOT Null,
    OrderDate DATETIME NOT NULL,
    CustomerID INT NOT NULL,
    PaymentMode VARCHAR(100) NULL,
    TotalAmount DECIMAL(10,2) NULL,
    ShippingAddress VARCHAR(100) NOT NULL,
    UserID INT NOT NULL,
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

/*5. OrderDetail Data Table*/
CREATE TABLE OrderDetail (
    OrderDetailID INT NOT NULL PRIMARY KEY identity(1,1),
    OrderID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL,
    UserID INT NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

/*6. Bill Table*/
CREATE TABLE Bills (
    BillID INT NOT NULL PRIMARY KEY identity(1,1),
    BillNumber VARCHAR(100) NOT NULL,
    BillDate DATETIME NOT NULL,
    OrderID INT NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL,
    Discount DECIMAL(10,2) NULL,
    NetAmount DECIMAL(10,2) NOT NULL,
    UserID INT NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);


--- data insertion

/*1. User Data Insert*/
INSERT INTO Users (UserName, Email, Password, MobileNo, Address, IsActive) VALUES
('John Doe', 'john@example.com', 'password123', '1234567890', '123 Main St', 1),
('Jane Smith', 'jane@example.com', 'password456', '0987654321', '456 Elm St', 1),
('Alice Johnson', 'alice@example.com', 'password789', '1122334455', '789 Oak St', 1),
('Bob Brown', 'bob@example.com', 'password101', '2233445566', '321 Pine St', 1),
('Eve Davis', 'eve@example.com', 'password202', '3344556677', '654 Maple St', 1);

/*2. Customer Data Insert*/
INSERT INTO Customer (CustomerName, HomeAddress, Email, MobileNo, GST_NO, CityName, PinCode, NetAmount, UserID) VALUES
('Acme Corp', '101 Industrial Ave', 'contact@acme.com', '5551234567', 'GSTIN12345', 'Metropolis', '123456', 5000.00, 1),
('Global Inc', '202 Business Rd', 'info@global.com', '5559876543', 'GSTIN54321', 'Gotham', '654321', 7500.00, 2),
('Tech Solutions', '303 Silicon Blvd', 'support@techsolutions.com', '5552233445', 'GSTIN67890', 'Star City', '789012', 8500.00, 3),
('Innovate Ltd', '404 Startup Ln', 'hello@innovate.com', '5553344556', 'GSTIN09876', 'Central City', '890123', 6500.00, 4),
('Alpha Industries', '505 Production St', 'sales@alpha.com', '5554455667', 'GSTIN11223', 'Coast City', '901234', 9000.00, 5);

/*3. Product Data Insert*/
INSERT INTO Product (ProductName, ProductPrice, ProductCode, Description, UserID) VALUES
('Widget A', 100.00, 'WGT-A', 'High-quality widget', 1),
('Gadget B', 150.00, 'GDT-B', 'Advanced gadget', 2),
('Device C', 200.00, 'DVC-C', 'Next-gen device', 3),
('Tool D', 75.00, 'TL-D', 'Multi-purpose tool', 4),
('Instrument E', 125.00, 'INS-E', 'Precision instrument', 5);

/*4. Orders Data Insert*/
INSERT INTO Orders (OrderNO, OrderDate, CustomerID, PaymentMode, TotalAmount, ShippingAddress, UserID)
VALUES
('ORD001', '2023-08-01 14:30:00', 1, 'Credit Card', 150.50, '123 Main St, Springfield', 1),
('ORD002', '2023-08-05 10:15:00', 2, 'PayPal', 75.00, '456 Elm St, Springfield', 2),
('ORD003', '2023-08-10 16:45:00', 3, 'Cash on Delivery', 200.00, '789 Oak St, Springfield', 3),
('ORD004', '2023-08-12 09:20:00', 4, 'Debit Card', 50.75, '101 Maple St, Springfield', 4),
('ORD005', '2023-08-15 13:00:00', 5, 'Bank Transfer', 300.00, '202 Pine St, Springfield', 1);


/*5. OrderDetails Data Insert*/
INSERT INTO OrderDetail (OrderID, ProductID, Quantity, Amount, TotalAmount, UserID) VALUES
(1, 1, 2, 100.00, 200.00, 1),
(2, 2, 1, 150.00, 150.00, 2),
(3, 3, 4, 200.00, 800.00, 3),
(4, 4, 3, 75.00, 225.00, 4),
(5, 5, 2, 125.00, 250.00, 5);

/*6. Bills Data Insert*/
INSERT INTO Bill (BillNumber, BillDate, OrderID, TotalAmount, Discount, NetAmount, UserID) VALUES
('BILL-001', '2024-07-25 14:45:00', 1, 500.00, 50.00, 450.00, 1),
('BILL-002', '2024-07-26 09:30:00', 2, 750.00, 75.00, 675.00, 2),
('BILL-003', '2024-07-27 12:00:00', 3, 850.00, 0.00, 850.00, 3),
('BILL-004', '2024-07-28 16:15:00', 4, 650.00, 25.00, 625.00, 4),
('BILL-005', '2024-07-29 13:35:00', 5, 900.00, 90.00, 810.00, 5);
