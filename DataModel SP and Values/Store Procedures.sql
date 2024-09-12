--- store Procudures of Coffee shop management system

--1. insert users
Create Proc [dbo].[PR_Insert_Users]
@Username varchar(100),
@Email varchar(100),
@Password varchar(100),
@MobileNo varchar(15),
@Address varchar(100),
@IsActive bit
As
Begin
	Insert Into [dbo].[Users]
	(
		[dbo].[Users].[UserName],
		[dbo].[users].[Email],
		[dbo].[users].[Password],
		[dbo].[users].[MobileNo],
		[dbo].[users].[Address],
		[dbo].[users].[IsActive]
	)
	Values
	(
		@Uname,
		@Email,
		@Password,
		@MobileNo,
		@Address,
		@IsActive
	);
End;

--2. Insert Customer
CREATE PROC [dbo].[PR_Insert_Customer]
    @CustomerName NVARCHAR(100),
    @HomeAddress NVARCHAR(100),
    @Email NVARCHAR(100),
    @MobileNo NVARCHAR(15),
    @GST_NO NVARCHAR(15),
    @CityName NVARCHAR(100),
    @PinCode NVARCHAR(15),
    @NetAmount DECIMAL(10,2),
    @UserID INT
AS
BEGIN
    INSERT INTO [dbo].[Customer] 
	(
		[dbo].[Customer].[CustomerName],
		[dbo].[Customer].[HomeAddress],
		[dbo].[Customer].[Email],
		[dbo].[Customer].[MobileNo],
		[dbo].[Customer].[GST_NO],
		[dbo].[Customer].[CityName],
		[dbo].[Customer].[PinCode],
		[dbo].[Customer].[NetAmount],
		[dbo].[Customer].[UserID]
	)
    VALUES 
	(
		@CustomerName, 
		@HomeAddress, 
		@Email, 
		@MobileNo, 
		@GST_NO, 
		@CityName, 
		@PinCode, 
		@NetAmount, 
		@UserID);
END;
 
---3. Insert Product
CREATE PROC [dbo].[PR_Insert_Product]
    @ProductName NVARCHAR(100),
    @ProductPrice DECIMAL(10,2),
    @ProductCode NVARCHAR(100),
    @Description NVARCHAR(100),
    @UserID INT
AS
BEGIN
    INSERT INTO [dbo].[Product] 
	(
		[dbo].[Product].[ProductName], 
		[dbo].[Product].[ProductPrice], 
		[dbo].[Product].[ProductCode], 
		[dbo].[Product].[Description],
		[dbo].[Product].[UserID]
	)
    VALUES 
	(
		@ProductName, 
		@ProductPrice,
		@ProductCode, 
		@Description, 
		@UserID
	);
END;

--4. Insert Order
CREATE PROC [dbo].[PR_Insert_Orders]
    @OrderDate DATETIME,
	@OrderNO varchar(10),
    @CustomerID INT,
    @PaymentMode NVARCHAR(100),
    @TotalAmount DECIMAL(10,2),
    @ShippingAddress NVARCHAR(100),
    @UserID INT
AS
BEGIN
    INSERT INTO [dbo].[Orders] 
	(
		[dbo].[Orders].[OrderDate],
		[dbo].[Orders].[OrderNO], 
		[dbo].[Orders].[CustomerID], 
		[dbo].[Orders].[PaymentMode], 
		[dbo].[Orders].[TotalAmount], 
		[dbo].[Orders].[ShippingAddress], 
		[dbo].[Orders].[UserID]
	)
    VALUES 
	(
		@OrderDate, 
		@OrderNO,
		@CustomerID, 
		@PaymentMode, 
		@TotalAmount, 
		@ShippingAddress, 
		@UserID
	);
END;

--- 5. Insert OrderDetail
CREATE PROC [PR_Insert_OrderDetail]
    @OrderID INT,
    @ProductID INT,
    @Quantity INT,
    @Amount DECIMAL(10,2),
    @TotalAmount DECIMAL(10,2),
    @UserID INT
AS
BEGIN
    INSERT INTO [dbo].[OrderDetail] 
	(
		[dbo].[OrderDetail].[OrderID], 
		[dbo].[OrderDetail].[ProductID], 
		[dbo].[OrderDetail].[Quantity], 
		[dbo].[OrderDetail].[Amount], 
		[dbo].[OrderDetail].[TotalAmount],
		[dbo].[OrderDetail].[UserID]
	)
    VALUES
	(
		@OrderID, 
		@ProductID, 
		@Quantity, 
		@Amount, 
		@TotalAmount, 
		@UserID
	);
END;

--6. Insert BIll
CREATE PROC [dbo].[PR_Insert_Bill]
    @BillNumber NVARCHAR(100),
    @BillDate DATETIME,
    @OrderID INT,
    @TotalAmount DECIMAL(10,2),
    @Discount DECIMAL(10,2) = NULL, -- Default value is NULL if not provided
    @NetAmount DECIMAL(10,2),
    @UserID INT
AS
BEGIN
    INSERT INTO [dbo].[Bill] 
	(
		[dbo].[Bill].[BillNumber], 
		[dbo].[Bill].[BillDate], 
		[dbo].[Bill].[OrderID], 
		[dbo].[Bill].[TotalAmount], 
		[dbo].[Bill].[Discount], 
		[dbo].[Bill].[NetAmount], 
		[dbo].[Bill].[UserID]
	)
	VALUES
	(
		@BillNumber, 
		@BillDate, 
		@OrderID, 
		@TotalAmount, 
		@Discount, 
		@NetAmount, 
		@UserID
	);
END;

--7. Update Users
CREATE PROC [dbo].[PR_Update_Users]
    @UserID INT,
    @UserName NVARCHAR(100),
    @Email NVARCHAR(100),
    @Password NVARCHAR(100),
    @MobileNo NVARCHAR(15),
    @Address NVARCHAR(100),
    @IsActive BIT
AS
BEGIN
    UPDATE [dbo].[Users]
    SET 
        [dbo].[Users].[UserName] = @UserName,
        [dbo].[Users].[Email] = @Email,
        [dbo].[Users].[Password] = @Password,
        [dbo].[Users].[MobileNo] = @MobileNo,
        [dbo].[Users].[Address] = @Address,
        [dbo].[Users].[IsActive] = @IsActive
    WHERE [dbo].[Users].[UserID] = @UserID;
END;

--8. Update Customer
CREATE PROC [dbo].[PR_Update_Customer]
    @CustomerID INT,
    @CustomerName NVARCHAR(100) ,
    @HomeAddress NVARCHAR(100) ,
    @Email NVARCHAR(100) ,
    @MobileNo NVARCHAR(15) ,
    @GST_NO NVARCHAR(15) ,
    @CityName NVARCHAR(100) ,
    @PinCode NVARCHAR(15) ,
    @NetAmount DECIMAL(10,2),
    @UserID INT
AS
BEGIN
    UPDATE [dbo].[Customer]
    SET 
        [dbo].[Customer].[CustomerName] = @CustomerName,
        [dbo].[Customer].[HomeAddress] = @HomeAddress,
        [dbo].[Customer].[Email] = @Email,
        [dbo].[Customer].[MobileNo] = @MobileNo,
        [dbo].[Customer].[GST_NO] = @GST_NO,
        [dbo].[Customer].[CityName] = @CityName, 
        [dbo].[Customer].[PinCode] = @PinCode, 
        [dbo].[Customer].[NetAmount] = @NetAmount, 
        [dbo].[Customer].[UserID] = @UserID
    WHERE [dbo].[Customer].[CustomerID] = @CustomerID;
END;

--9. Update Product
CREATE PROC [PR_Update_Product]
    @ProductID INT,
    @ProductName NVARCHAR(100) ,
    @ProductPrice DECIMAL(10,2) ,
    @ProductCode NVARCHAR(100) ,
    @Description NVARCHAR(100) ,
    @UserID INT
AS
BEGIN
    UPDATE [dbo].[Product]
    SET 
        [dbo].[Product].[ProductName] = @ProductName,
        [dbo].[Product].[ProductPrice] = @ProductPrice,
        [dbo].[Product].[ProductCode] = @ProductCode,
        [dbo].[Product].[Description] = @Description, 
        [dbo].[Product].[UserID] = @UserID
    WHERE [dbo].[Product].[ProductID] = @ProductID;
END;

--10. Update Orders
CREATE PROC [PR_Update_Orders]
    @OrderID INT,
	@OrderNO varchar(10),
    @OrderDate DATETIME ,
    @CustomerID INT ,
    @PaymentMode NVARCHAR(100) ,
    @TotalAmount DECIMAL(10,2) ,
    @ShippingAddress NVARCHAR(100) ,
    @UserID INT 
AS
BEGIN
    UPDATE [dbo].[Orders]
    SET 
        [dbo].[Orders].OrderDate = @OrderDate,
		[dbo].[Orders].OrderNO = @OrderNO,
        [dbo].[Orders].CustomerID = @CustomerID,
        [dbo].[Orders].PaymentMode = @PaymentMode,
        [dbo].[Orders].TotalAmount = @TotalAmount,
        [dbo].[Orders].ShippingAddress = @ShippingAddress,
        [dbo].[Orders].UserID = @UserID
    WHERE [dbo].[Orders].[OrderID] = @OrderID;
END;

--11. Update Order Detail
CREATE PROC [PR_Update_OrderDetail]
    @OrderDetailID INT,
    @OrderID INT ,
    @ProductID INT ,
    @Quantity INT ,
    @Amount DECIMAL(10,2) ,
    @TotalAmount DECIMAL(10,2) ,
    @UserID INT
AS
BEGIN
    UPDATE [dbo].[OrderDetail]
    SET 
        [dbo].[OrderDetail].[OrderID] = @OrderID,
        [dbo].[OrderDetail].[ProductID] = @ProductID,
        [dbo].[OrderDetail].[Quantity] = @Quantity,
        [dbo].[OrderDetail].[Amount] = @Amount, 
        [dbo].[OrderDetail].[TotalAmount] = @TotalAmount,
        [dbo].[OrderDetail].[UserID] = @UserID
    WHERE [dbo].[OrderDetail].[OrderDetailID] = @OrderDetailID;
END;

--12.Update Bill
CREATE PROC [PR_Update_Bill]
    @BillID INT,
    @BillNumber NVARCHAR(100) ,
    @BillDate DATETIME ,
    @OrderID INT,
    @TotalAmount DECIMAL(10,2) ,
    @Discount DECIMAL(10,2) ,
    @NetAmount DECIMAL(10,2),
    @UserID INT
AS
BEGIN
    UPDATE [dbo].[Bill]
    SET 
        [dbo].[Bill].[BillNumber] = @BillNumber,
        [dbo].[Bill].[BillDate] = @BillDate,
        [dbo].[Bill].[OrderID] = @OrderID, 
        [dbo].[Bill].[TotalAmount] = @TotalAmount, 
        [dbo].[Bill].[Discount] = @Discount,
        [dbo].[Bill].[NetAmount] = @NetAmount, 
        [dbo].[Bill].[UserID] = @UserID
    WHERE [dbo].[Bill].[BillID] = @BillID;
END;

--13. Delete Users
CREATE PROC [PR_Delete_Users]
    @UserID INT
AS
BEGIN
    DELETE FROM [dbo].[User]
    WHERE [dbo].[Users].[UserID] = @UserID;
END;

--14. Delete Customer
CREATE PROC [PR_Delete_Customer]
    @CustomerID INT
AS
BEGIN
    DELETE FROM [dbo].[Customer]
    WHERE [dbo].[Customer].[CustomerID] = @CustomerID;
END;

--15. Delete Product
CREATE PROC [PR_Delete_Product]
    @ProductID INT
AS
BEGIN
    DELETE FROM [dbo].[Product]
    WHERE [dbo].[Product].[ProductID] = @ProductID;
END;

--16. Delete Orders
CREATE PROC [PR_Delete_Order]
    @OrderID INT
AS
BEGIN
    DELETE FROM [dbo].[Orders]
    WHERE [dbo].[Orders].[OrderID] = @OrderID;
END;

--17. Delete OrderDetail
CREATE PROCEDURE [PR_Delete_OrderDetail]
    @OrderDetailID INT
AS
BEGIN
    DELETE FROM [dbo].[OrderDetail]
    WHERE [dbo].[OrderDetail].[OrderDetailID] = @OrderDetailID;
END;

--18. Delete Bill
CREATE PROC [PR_Delete_Bill]
    @BillID INT
AS
BEGIN
    DELETE FROM [dbo].[Bill]
    WHERE [dbo].[Bill].[BillID] = @BillID;
END;

--19. SelectAll Users
CREATE PROC [dbo].[PR_SelectAll_Users]
AS
BEGIN
    SELECT
		[dbo].[Users].[UserID],
		[dbo].[Users].[UserName],
		[dbo].[users].[Email],
		[dbo].[users].[Password],
		[dbo].[users].[MobileNo],
		[dbo].[users].[Address],
		[dbo].[users].[IsActive] 
	FROM [dbo].[Users];
END;

--20. Select All Customer
CREATE PROC [dbo].[PR_SelectAll_Customer]
AS
BEGIN
    SELECT
		[dbo].[Customer].[CustomerID],
		[dbo].[Customer].[CustomerName],
		[dbo].[Customer].[HomeAddress],
		[dbo].[Customer].[Email],
		[dbo].[Customer].[MobileNo],
		[dbo].[Customer].[GST_NO],
		[dbo].[Customer].[CityName],
		[dbo].[Customer].[PinCode],
		[dbo].[Customer].[NetAmount],
		[dbo].[Users].[UserName]
	FROM [dbo].[Customer];
	inner join [dbo].[Users] on [dbo].[Users].[UserID] = [dbo].[Customer].[UserID]

END;

--21. SelectAll Product
CREATE PROC [dbo].[PR_SelectAll_Product]
AS
BEGIN
    SELECT 
		[dbo].[Product].[ProductID],
		[dbo].[Product].[ProductName], 
		[dbo].[Product].[ProductPrice], 
		[dbo].[Product].[ProductCode], 
		[dbo].[Product].[Description],
		[dbo].[Users].[UserName]
	FROM [dbo].[Product];
	inner join [dbo].[Users] on [dbo].[Users].[UserID] = [dbo].[Product].[UserID]
END;

--22. SelectAll Orders
CREATE PROC [dbo].[PR_SelectAll_Orders]
AS
BEGIN
    SELECT 
		[dbo].[Orders].[OrderID],
		[dbo].[Orders].[OrderDate],
		[dbo].[Orders].[OrderNO], 
		[dbo].[Customer].[CustomerName], 
		[dbo].[Orders].[PaymentMode], 
		[dbo].[Orders].[TotalAmount], 
		[dbo].[Orders].[ShippingAddress], 
		[dbo].[Users].[UserName]
	FROM [dbo].[Orders];
	inner join [dbo].[Users] on [dbo].[Users].[UserID] = [dbo].[Orders].[UserID]
	inner join [dbo].[Customer] on [dbo].[Customer].[CustomerID] = [dbo].[Orders].[CustomerID]
 
END;

--23. Select All Order Detail
CREATE PROC [dbo].[PR_SelectAll_OrderDetail]
AS
BEGIN
    SELECT 
		[dbo].[OrderDetail].[OrderDetailID],
		[dbo].[Orders].[OrderNO], 
		[dbo].[Product].[ProductName], 
		[dbo].[OrderDetail].[Quantity], 
		[dbo].[OrderDetail].[Amount], 
		[dbo].[OrderDetail].[TotalAmount],
		[dbo].[Users].[UserName]
	FROM [dbo].[OrderDetail];
	inner join [dbo].[Orders] on [dbo].[Orders].[OrderID] = [dbo].[OrderDetail].[OrderID]
	inner join [dbo].[Users] on [dbo].[Users].[UserID] = [dbo].[OrderDetail].[UserID]
	inner join [dbo].[Product] on [dbo].[Product].[ProductID] = [dbo].[OrderDetail].[ProductID]
END;

--24. Select All Bill
CREATE PROC [dbo].[PR_SelectAll_Bill]
AS
BEGIN
    SELECT
		[dbo].[Bill].[BillID],
		[dbo].[Bill].[BillNumber], 
		[dbo].[Bill].[BillDate], 
		[dbo].[Orders].[OrderNO], 
		[dbo].[Bill].[TotalAmount], 
		[dbo].[Bill].[Discount], 
		[dbo].[Bill].[NetAmount], 
		[dbo].[Users].[UserName]
	FROM [dbo].[Bill];
	inner join [dbo].[Orders] on [dbo].[Orders].[OrderID] = [dbo].[Bill].[OrderID]
	inner join [dbo].[Users] on [dbo].[Users].[UserID] = [dbo].[Bill].[UserID]
END;

--25. Select DropDown Product
Create Proc [dbo].[PE_DropDown_Product]
AS
BEGIN
	Select
		[dbo].[Product].[ProductID],
		[dbo].[Product].[ProductName]
	From [dbo].[Product]
END;

--26. Select DropDown Users
Create Proc [dbo].[PR_DropDown_Users]
AS
BEGIN
	Select
		[dbo].[Users].[UserID],
		[dbo].[Users].[UserName]
	From [dbo].[Users]
END;

--27. Select DropDown Customer
Create Proc [dbo].[PR_DropDown_Customer]
AS
BEGIN
	Select
		[dbo].[Customer].[CustomerID],
		[dbo].[Customer].[CustomerName]
	From [dbo].[Customer]
END;

--28. Select DropDown Orders
Create Proc [dbo].[PR_DropDown_Order]
AS
BEGIN
	Select
		[dbo].[Orders].[OrderID],
		[dbo].[Orders].[OrderNO]
	From [dbo].[Orders]
END;

--29. SelectByPK Product
Create Proc [dbo].[PR_SelectByPK_Product]
@ProductID int
AS
BEGIN
    SELECT 
		[dbo].[Product].[ProductID],
		[dbo].[Product].[ProductName], 
		[dbo].[Product].[ProductPrice], 
		[dbo].[Product].[ProductCode], 
		[dbo].[Product].[Description],
		[dbo].[Product].[UserID]
	FROM [dbo].[Product]
	where [dbo].[Product].[ProductID] = @ProductID;
END;

-- 30.SelectByPK Customer
Create Proc [dbo].[PR_SelectByPK_Customer]
@CustomerID int
AS
BEGIN
    SELECT
		[dbo].[Customer].[CustomerID],
		[dbo].[Customer].[CustomerName],
		[dbo].[Customer].[HomeAddress],
		[dbo].[Customer].[Email],
		[dbo].[Customer].[MobileNo],
		[dbo].[Customer].[GST_NO],
		[dbo].[Customer].[CityName],
		[dbo].[Customer].[PinCode],
		[dbo].[Customer].[NetAmount],
		[dbo].[Customer].[UserID]
	FROM [dbo].[Customer]
	where [dbo].[Customer].[CustomerID] = @CustomerID;
END;

-- 31.SelectByPK Order
Create Proc [dbo].[PR_SelectByPK_Order]
@OrderID int
AS
BEGIN
    SELECT 
		[dbo].[Orders].[OrderID],
		[dbo].[Orders].[OrderDate],
		[dbo].[Orders].[OrderNO],
		[dbo].[Orders].[CustomerID], 
		[dbo].[Orders].[PaymentMode], 
		[dbo].[Orders].[TotalAmount], 
		[dbo].[Orders].[ShippingAddress], 
		[dbo].[Orders].[UserID]
	FROM [dbo].[Orders]
	where [dbo].[Orders].[OrderID] = @OrderID;
END;

-- 32.SelectByPK OrderDetails
Create Proc [dbo].[PR_SelectByPK_OrderDetail]
@OrderDetailID int
AS
BEGIN
	 SELECT 
		[dbo].[OrderDetail].[OrderDetailID],
		[dbo].[OrderDetail].[OrderID], 
		[dbo].[OrderDetail].[ProductID], 
		[dbo].[OrderDetail].[Quantity], 
		[dbo].[OrderDetail].[Amount], 
		[dbo].[OrderDetail].[TotalAmount],
		[dbo].[OrderDetail].[UserID]
	FROM [dbo].[OrderDetail]
	where [dbo].[OrderDetail].[OrderDetailID] = @OrderDetailID;
END;

-- 33.SelectByPK User
Create Proc [dbo].[PR_SelectByPK_Users]
@UserID int
AS
BEGIN
    SELECT
		[dbo].[Users].[UserID],
		[dbo].[Users].[UserName],
		[dbo].[users].[Email],
		[dbo].[users].[Password],
		[dbo].[users].[MobileNo],
		[dbo].[users].[Address],
		[dbo].[users].[IsActive] 
	FROM [dbo].[Users]
	where [dbo].[Users].[UserID] = @UserID;
END;

-- 34.SelectByPK User
Create Proc [dbo].[PR_SelectByPK_Bill]
@BillID int
AS
BEGIN
    SELECT
		[dbo].[Bill].[BillID],
		[dbo].[Bill].[BillNumber], 
		[dbo].[Bill].[BillDate], 
		[dbo].[Bill].[OrderID], 
		[dbo].[Bill].[TotalAmount], 
		[dbo].[Bill].[Discount], 
		[dbo].[Bill].[NetAmount], 
		[dbo].[Bill].[UserID]
	FROM [dbo].[Bill]
	where [dbo].[Bill].[BillID] = @BillID;
END;

-- 35.Authantication User
Alter Proc [dbo].[PR_Login_User]
@UserName varchar(100),
@Password varchar(100)
as
begin
    SELECT
		[dbo].[Users].[UserID],
		[dbo].[Users].[UserName],
		[dbo].[users].[Email],
		[dbo].[users].[Password],
		[dbo].[users].[MobileNo] 
	FROM [dbo].[Users]
	where UserName = @UserName and Password = @Password
end