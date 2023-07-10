IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'STGenetics')
BEGIN
    CREATE DATABASE STGenetics;
    ALTER DATABASE STGenetics SET RECOVERY SIMPLE;
END
GO

USE STGenetics;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Animal')
BEGIN
    CREATE TABLE Animal (
        AnimalId INT PRIMARY KEY,
        Name VARCHAR(50) NOT NULL,
        Breed VARCHAR(50) NOT NULL,
        BirthDate DATE NOT NULL,
        Sex VARCHAR(10) NOT NULL,
        Price DECIMAL(10, 2) NOT NULL,
        Status VARCHAR(10) NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'User')
BEGIN
    CREATE TABLE [User] (
        UserId INT PRIMARY KEY,
        Username VARCHAR(50) NOT NULL,
        Password VARCHAR(50) NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Order')
BEGIN
    CREATE TABLE [Order] (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT,
    Freight DECIMAL(10, 2),
    TotalAmount DECIMAL(10, 2),
    CONSTRAINT FK_Order_User FOREIGN KEY (UserId) REFERENCES [User](UserId)
);
END
GO


IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderItem')
BEGIN
    CREATE TABLE OrderItem (
    ItemId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT,
    AnimalId INT,
    AnimalPrice DECIMAL(10, 2),
    CONSTRAINT FK_OrderItem_Order FOREIGN KEY (OrderId) REFERENCES [Order](OrderId),
    CONSTRAINT FK_OrderItem_Animal FOREIGN KEY (AnimalId) REFERENCES Animal(AnimalId)
);
END
GO


IF NOT EXISTS (SELECT * FROM Animal)
BEGIN
    DECLARE @Counter INT = 1;
    
    WHILE @Counter <= 5000
    BEGIN
        DECLARE @AnimalId INT = @Counter;
        DECLARE @Name VARCHAR(50) = 'Animal ' + CAST(@Counter AS VARCHAR(10));
        DECLARE @Breed VARCHAR(50) = 'Breed ' + CAST(@Counter AS VARCHAR(10));
        DECLARE @BirthDate DATE = DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 365, GETDATE());
        DECLARE @Sex VARCHAR(10) = CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN 'Male' ELSE 'Female' END;
        DECLARE @Price DECIMAL(10, 2) = CAST(RAND() * 900 + 100 AS DECIMAL(10, 2));
        DECLARE @Status VARCHAR(10) = CASE WHEN ABS(CHECKSUM(NEWID())) % 2 = 0 THEN 'Active' ELSE 'Inactive' END;
        
        INSERT INTO Animal (AnimalId, Name, Breed, BirthDate, Sex, Price, Status)
        VALUES (@AnimalId, @Name, @Breed, @BirthDate, @Sex, @Price, @Status);
        
        SET @Counter += 1;
    END;
END;
GO

IF NOT EXISTS (SELECT * FROM [User])
BEGIN
    INSERT INTO [User] (UserId, Username, Password)
    VALUES
        (1, 'User1', 'Password1'),
        (2, 'User2', 'Password2'),
        -- Insertar más registros de prueba aquí
        (10, 'User10', 'Password10');
END