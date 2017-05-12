USE $DbName$;
GO

IF NOT EXISTS (SELECT TOP 1 * FROM AddressType)
BEGIN
    INSERT INTO dbo.AddressType ( Name , Created , CreatedBy , Modified , ModifiedBy) VALUES ('Home', SYSDATETIME() , N'DbUp' , SYSDATETIME() , N'DbUp');
    INSERT INTO dbo.AddressType ( Name , Created , CreatedBy , Modified , ModifiedBy) VALUES ('Work', SYSDATETIME() , N'DbUp' , SYSDATETIME() , N'DbUp');
END
