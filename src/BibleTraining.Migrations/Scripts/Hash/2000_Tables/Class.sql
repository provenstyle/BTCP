USE $DbName$
go;

IF OBJECT_ID(N'dbo.Class') IS NULL
BEGIN
    CREATE TABLE Class (
        Id             INT       NOT NULL IDENTITY(1,1),
        CityId         INT       NOT NULL,
        CourseId	   INT       NOT NULL,
        StartDate	   DATETIME2 NOT NULL,
        GraduationDate DATETIME2 NOT NULL,

        Created     DATETIME2      NOT NULL,
        CreatedBy   NVARCHAR(500)  NOT NULL,
        Modified    DATETIME2      NOT NULL,
        ModifiedBy  NVARCHAR(500)  NOT NULL,
        RowVersion  ROWVERSION

        CONSTRAINT PK_Class PRIMARY KEY CLUSTERED (Id ASC)
    );
END;

