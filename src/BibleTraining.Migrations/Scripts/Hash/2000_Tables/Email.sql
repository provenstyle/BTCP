USE $DbName$
go;

IF OBJECT_ID(N'dbo.Email') IS NULL
BEGIN
	CREATE TABLE Email (
		Id          INT           NOT NULL IDENTITY(1,1),
		PersonId    INT           NOT NULL,
		EmailTypeId INT           NOT NULL,
		Address     NVARCHAR(500) NOT NULL,

		Created     DATETIME2     NOT NULL,
		CreatedBy   NVARCHAR(500) NOT NULL,
		Modified    DATETIME2     NOT NULL,
		ModifiedBy  NVARCHAR(500) NOT NULL,
		RowVersion  ROWVERSION

		CONSTRAINT PK_Email PRIMARY KEY CLUSTERED (Id ASC)
	);
END