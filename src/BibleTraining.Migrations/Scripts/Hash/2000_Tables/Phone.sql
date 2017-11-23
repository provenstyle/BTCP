USE $DbName$
go;

IF OBJECT_ID(N'dbo.Phone') IS NULL
BEGIN
	CREATE TABLE Phone (
		Id          INT           NOT NULL IDENTITY(1,1),
		PersonId    INT           NOT NULL,
		PhoneTypeId INT           NOT NULL,
		Number      NVARCHAR(50)  NOT NULL,
		Extension   NVARCHAR(50)  NOT NULL,

		Created     DATETIME2     NOT NULL,
		CreatedBy   NVARCHAR(500) NOT NULL,
		Modified    DATETIME2     NOT NULL,
		ModifiedBy  NVARCHAR(500) NOT NULL,
		RowVersion  ROWVERSION

		CONSTRAINT PK_Phone PRIMARY KEY CLUSTERED (Id ASC)
	);
END

IF COL_LENGTH('dbo.Phone', 'Name') IS NOT NULL
BEGIN
    ALTER TABLE Phone DROP COLUMN Name
END

IF COL_LENGTH('dbo.Phone', 'Number') IS NULL
BEGIN
    ALTER TABLE Phone ADD Number NVARCHAR(50) NOT NULL
END

IF COL_LENGTH('dbo.Phone', 'Extension') IS NULL
BEGIN
    ALTER TABLE Phone ADD Extension NVARCHAR(50) NULL
END