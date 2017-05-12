USE $DbName$
go;

IF OBJECT_ID(N'dbo.Address') IS NULL
BEGIN
	CREATE TABLE Address (
		Id          INT           NOT NULL IDENTITY(1,1),
		PersonId    INT           NOT NULL,
		Name        NVARCHAR(500) NOT NULL,
		Description NVARCHAR(500) NULL,

		Created     DATETIME2     NOT NULL,
		CreatedBy   NVARCHAR(500) NOT NULL,
		Modified    DATETIME2     NOT NULL,
		ModifiedBy  NVARCHAR(500) NOT NULL,
		RowVersion  ROWVERSION

		CONSTRAINT PK_Address PRIMARY KEY CLUSTERED (Id ASC)
	);
END