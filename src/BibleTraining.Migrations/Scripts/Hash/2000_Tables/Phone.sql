USE $DbName$
go;

IF OBJECT_ID(N'dbo.Phone') IS NULL
BEGIN
	CREATE TABLE Phone (
		Id          INT           NOT NULL IDENTITY(1,1),
		Name        NVARCHAR(500) NOT NULL,

		Created     DATETIME2     NOT NULL,
		CreatedBy   NVARCHAR(500) NOT NULL,
		Modified    DATETIME2     NOT NULL,
		ModifiedBy  NVARCHAR(500) NOT NULL,
		RowVersion  ROWVERSION

		CONSTRAINT PK_Phone PRIMARY KEY CLUSTERED (Id ASC)
	);
END
