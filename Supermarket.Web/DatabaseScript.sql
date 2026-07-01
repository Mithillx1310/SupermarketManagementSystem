IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Products] (
    [ProductId] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Barcode] nvarchar(max) NOT NULL,
    [BrandSupplier] nvarchar(max) NOT NULL,
    [ExpiryDate] datetime2 NOT NULL,
    [ProductCategory] nvarchar(max) NOT NULL,
    [StockAvailabilityStatus] nvarchar(max) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [QuantityInStock] int NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([ProductId])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260629144542_InitialCreate', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260629161507_AddExpiryDate', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Suppliers] (
    [SupplierId] int NOT NULL IDENTITY,
    [SupplierName] nvarchar(max) NOT NULL,
    [ContactPerson] nvarchar(max) NOT NULL,
    [PhoneNumber] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Suppliers] PRIMARY KEY ([SupplierId])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260629175051_AddSuppliersTable', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Orders] (
    [OrderId] int NOT NULL IDENTITY,
    [CustomerName] nvarchar(max) NOT NULL,
    [ProductName] nvarchar(max) NOT NULL,
    [Quantity] int NOT NULL,
    [TotalPrice] decimal(18,2) NOT NULL,
    [OrderDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderId])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260629182501_AddOrdersTable', N'8.0.8');
GO

COMMIT;
GO

