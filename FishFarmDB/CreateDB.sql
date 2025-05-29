
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
    SourceCageId INT NOT NULL,
    TransferDate DATE NOT NULL,
    CONSTRAINT FK_Transfer_SourceCage FOREIGN KEY (SourceCageId) REFERENCES Cages(CageId)
);
GO

-- Table: FishTransferDetails
CREATE TABLE FishTransferDetails (
    TransferDetailId INT IDENTITY(1,1) PRIMARY KEY,
    TransferId INT NOT NULL,
    DestinationCageId INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    CONSTRAINT FK_TransferDetails_Transfer FOREIGN KEY (TransferId) REFERENCES FishTransfers(TransferId),
    CONSTRAINT FK_TransferDetails_DestinationCage FOREIGN KEY (DestinationCageId) REFERENCES Cages(CageId)
);
GO

-- Table: DailyStockBalances
CREATE TABLE DailyStockBalances (
    CageId INT NOT NULL,
    BalanceDate DATE NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity >= 0),
    CONSTRAINT PK_DailyStock PRIMARY KEY (CageId, BalanceDate),
    CONSTRAINT FK_StockBalance_Cage FOREIGN KEY (CageId) REFERENCES Cages(CageId)
);
GO

-- View: Pivot-style Mortality Summary
-- (For use in front-end/reporting, not materialized)
-- Can be queried like:
-- SELECT CageId, YEAR(MortalityDate) AS Year, MONTH(MortalityDate) AS Month, SUM(Quantity) AS TotalMortalities
-- FROM Mortalities
-- GROUP BY CageId, YEAR(MortalityDate), MONTH(MortalityDate);

-- You can create an indexed view if necessary, depending on performance needs

