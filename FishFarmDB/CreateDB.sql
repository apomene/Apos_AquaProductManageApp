

-- Table: Cages
CREATE TABLE Cages (
    CageId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL UNIQUE,
    IsActive BIT NOT NULL DEFAULT 1
);
GO

-- Table: FishStockings
CREATE TABLE FishStockings (
    StockingId INT IDENTITY(1,1) PRIMARY KEY,
    CageId INT NOT NULL,
    StockingDate DATE NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity >= 0),
    CONSTRAINT FK_FishStocking_Cage FOREIGN KEY (CageId) REFERENCES Cages(CageId)
);
GO

-- Table: Mortalities
CREATE TABLE Mortalities (
    MortalityId INT IDENTITY(1,1) PRIMARY KEY,
    CageId INT NOT NULL,
    MortalityDate DATE NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity >= 0),
    CONSTRAINT FK_Mortality_Cage FOREIGN KEY (CageId) REFERENCES Cages(CageId)
);
GO

-- Table: FishTransfers
CREATE TABLE FishTransfers (
    TransferId INT IDENTITY(1,1) PRIMARY KEY,
    FromCageId INT NOT NULL,
    ToCageId INT NOT NULL,
    TransferDate DATE NOT NULL,
    Quantity INT NOT NULL,
    CONSTRAINT FK_Transfer_FromCage FOREIGN KEY (FromCageId) REFERENCES Cages(CageId),
    CONSTRAINT FK_Transfer_ToCage FOREIGN KEY (ToCageId) REFERENCES Cages(CageId)
);
GO


