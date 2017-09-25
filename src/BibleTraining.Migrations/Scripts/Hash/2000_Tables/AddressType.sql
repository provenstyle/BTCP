USE $DbName$
go;

IF OBJECT_ID(N'dbo.AddressType') IS NULL
BEGIN
	CREATE TABLE AddressType (
		Id          INT           NOT NULL IDENTITY(1,1),
		Name        NVARCHAR(500) NOT NULL,

		Created     DATETIME2     NOT NULL,
		CreatedBy   NVARCHAR(500) NOT NULL,
		Modified    DATETIME2     NOT NULL,
		ModifiedBy  NVARCHAR(500) NOT NULL,
		RowVersion  ROWVERSION

		CONSTRAINT PK_AddressType PRIMARY KEY CLUSTERED (Id ASC)
	);
END
