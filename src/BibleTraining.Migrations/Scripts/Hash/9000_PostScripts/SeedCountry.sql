USE $DbName$;
GO

IF NOT EXISTS (SELECT TOP 1 * FROM dbo.Country)
BEGIN
	INSERT INTO dbo.Country ( CountryCode , Name , Created , CreatedBy , Modified , ModifiedBy ) VALUES  ( 254 , N'Kenya' ,    SYSDATETIME() , N'DbUp' , SYSDATETIME() , N'Dbup' );
	INSERT INTO dbo.Country ( CountryCode , Name , Created , CreatedBy , Modified , ModifiedBy ) VALUES  ( 255 , N'Tanzania' , SYSDATETIME() , N'DbUp' , SYSDATETIME() , N'Dbup' );
	INSERT INTO dbo.Country ( CountryCode , Name , Created , CreatedBy , Modified , ModifiedBy ) VALUES  ( 256 , N'Uganda' ,   SYSDATETIME() , N'DbUp' , SYSDATETIME() , N'Dbup' );
END;

