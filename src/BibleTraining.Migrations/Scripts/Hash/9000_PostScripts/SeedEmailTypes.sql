USE $DbName$;
GO

IF NOT EXISTS (SELECT TOP 1 * FROM EmailType)
BEGIN
    INSERT INTO dbo.EmailType ( Name , Created , CreatedBy , Modified , ModifiedBy) VALUES ('Personal', SYSDATETIME() , N'DbUp' , SYSDATETIME() , N'DbUp');
    INSERT INTO dbo.EmailType ( Name , Created , CreatedBy , Modified , ModifiedBy) VALUES ('Work',     SYSDATETIME() , N'DbUp' , SYSDATETIME() , N'DbUp');
END
