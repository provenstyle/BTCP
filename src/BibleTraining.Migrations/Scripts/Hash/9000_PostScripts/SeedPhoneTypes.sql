--USE $DbName$;
--GO

--IF NOT EXISTS (SELECT TOP 1 * FROM PhoneType)
--BEGIN
--    INSERT INTO dbo.PhoneType ( Name , Created , CreatedBy , Modified , ModifiedBy) VALUES ('Cell', SYSDATETIME() , N'DbUp' , SYSDATETIME() , N'DbUp');
--    INSERT INTO dbo.PhoneType ( Name , Created , CreatedBy , Modified , ModifiedBy) VALUES ('Home', SYSDATETIME() , N'DbUp' , SYSDATETIME() , N'DbUp');
--    INSERT INTO dbo.PhoneType ( Name , Created , CreatedBy , Modified , ModifiedBy) VALUES ('Work', SYSDATETIME() , N'DbUp' , SYSDATETIME() , N'DbUp');
--    INSERT INTO dbo.PhoneType ( Name , Created , CreatedBy , Modified , ModifiedBy) VALUES ('Fax', SYSDATETIME() , N'DbUp' , SYSDATETIME() , N'DbUp');
--END