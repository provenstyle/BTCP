USE $DbName$
go;

IF OBJECT_ID(N'dbo.Country') IS NULL
BEGIN
	CREATE TABLE Country (
		Id          INT NOT NULL IDENTITY(1,1),
		CountryCode INT NOT NULL,
		Name        NVARCHAR(500)  NOT NULL,

		Created     DATETIME2      NOT NULL,
		CreatedBy   NVARCHAR(500)  NOT NULL,
		Modified    DATETIME2      NOT NULL,
		ModifiedBy  NVARCHAR(500)  NOT NULL,
		RowVersion  ROWVERSION

		CONSTRAINT PK_Country PRIMARY KEY CLUSTERED (Id ASC)
	);
END;
