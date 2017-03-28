USE $DbName$;
GO

IF OBJECT_ID(N'FK_ClassTeacher_Class') IS NULL
BEGIN
	ALTER TABLE dbo.ClassTeacher WITH CHECK ADD CONSTRAINT FK_ClassTeacher_Class FOREIGN KEY(ClassId)
	REFERENCES dbo.Class (Id);
END;

IF OBJECT_ID(N'FK_ClassTeacher_Teacher') IS NULL
BEGIN
	ALTER TABLE dbo.ClassTeacher WITH CHECK ADD CONSTRAINT FK_ClassTeacher_Teacher FOREIGN KEY(TeacherId)
	REFERENCES dbo.Teacher (Id);
END;
