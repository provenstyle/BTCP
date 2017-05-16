USE $DbName$
go;

IF OBJECT_ID(N'dbo.PhoneType') IS NULL
BEGIN
	CREATE TABLE PhoneType (
		Id          INT           NOT NULL IDENTITY(1,1),
		Name        NVARCHAR(500) NOT NULL,
		Description NVARCHAR(500) NULL,

		Created     DATETIME2     NOT NULL,
		CreatedBy   NVARCHAR(500) NOT NULL,
		Modified    DATETIME2     NOT NULL,
		ModifiedBy  NVARCHAR(500) NOT NULL,
		RowVersion  ROWVERSION

		CONSTRAINT PK_PhoneType PRIMARY KEY CLUSTERED (Id ASC)
	);
END
