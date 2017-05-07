USE $DbName$
go;

IF OBJECT_ID(N'dbo.ClassStudent') IS NULL
BEGIN
    CREATE TABLE ClassStudent (
        Id       INT NOT NULL IDENTITY(1,1),
        ClassId  INT NOT NULL,
        PersonId INT NOT NULL,

        Created     DATETIME2      NOT NULL,
        CreatedBy   NVARCHAR(500)  NOT NULL,
        Modified    DATETIME2      NOT NULL,
        ModifiedBy  NVARCHAR(500)  NOT NULL,
        RowVersion  ROWVERSION

        CONSTRAINT PK_ClassStudent PRIMARY KEY CLUSTERED (Id ASC)
    );
END;

