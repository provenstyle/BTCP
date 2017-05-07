USE $DbName$;
GO

IF OBJECT_ID(N'FK_ClassStudent_Class') IS NULL
BEGIN
	ALTER TABLE dbo.ClassStudent WITH CHECK ADD CONSTRAINT FK_ClassStudent_Class FOREIGN KEY(ClassId)
	REFERENCES dbo.Class (Id);
END;

IF OBJECT_ID(N'FK_ClassStudent_Student') IS NULL
BEGIN
	ALTER TABLE dbo.ClassStudent WITH CHECK ADD CONSTRAINT FK_ClassStudent_Student FOREIGN KEY(PersonId)
	REFERENCES dbo.Person (Id);
END;
