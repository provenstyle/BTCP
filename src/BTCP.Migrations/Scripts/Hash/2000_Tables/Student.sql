﻿USE $DbName$
go;

IF OBJECT_ID(N'dbo.Student') IS NULL
BEGIN
	CREATE TABLE Student (
		Id        INT            NOT NULL IDENTITY(1,1),
		FirstName NVARCHAR(500)  NOT NULL,
		LastName  NVARCHAR(500)  NOT NULL,

		Created    DATETIME2      NOT NULL,
		CreatedBy  NVARCHAR(500)  NOT NULL,
		Modified   DATETIME2      NOT NULL,
		ModifiedBy NVARCHAR(500)  NOT NULL,
		RowVersion ROWVERSION

		CONSTRAINT PK_Student PRIMARY KEY CLUSTERED (Id ASC)
	);
END;