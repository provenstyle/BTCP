USE $DbName$
go;

IF OBJECT_ID(N'dbo.Person') IS NULL
BEGIN
	CREATE TABLE Person (
		Id        INT            NOT NULL IDENTITY(1,1),
		Gender    INT            NOT NULL,
		FirstName NVARCHAR(500)  NOT NULL,
		LastName  NVARCHAR(500)  NOT NULL,
        BirthDate Datetime2      NULL,
		Image     NVARCHAR(500)  NULL,
		Bio       NVARCHAR(4000) NULL,


		Created    DATETIME2     NOT NULL,
		CreatedBy  NVARCHAR(500) NOT NULL,
		Modified   DATETIME2     NOT NULL,
		ModifiedBy NVARCHAR(500) NOT NULL,
		RowVersion ROWVERSION

		CONSTRAINT PK_Person PRIMARY KEY CLUSTERED (Id ASC)
	);
END;