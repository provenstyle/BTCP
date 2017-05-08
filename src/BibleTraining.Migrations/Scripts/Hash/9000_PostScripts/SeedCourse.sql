USE $DbName$;
GO

IF NOT EXISTS (SELECT TOP 1 * FROM dbo.Course)
BEGIN
    INSERT INTO dbo.Course ( Name , Description , Created , CreatedBy , Modified , ModifiedBy ) VALUES  ( N'BTCP'    , N'Bible Training Centre for Pastors' , SYSDATETIME() , N'DbUp' , SYSDATETIME() , N'DbUp' )
    INSERT INTO dbo.Course ( Name , Description , Created , CreatedBy , Modified , ModifiedBy ) VALUES  ( N'BTCL'    , N'Bible Training for Church Leaders' , SYSDATETIME() , N'DbUp' , SYSDATETIME() , N'DbUp' )
    INSERT INTO dbo.Course ( Name , Description , Created , CreatedBy , Modified , ModifiedBy ) VALUES  ( N'2:7'     , N'Navigator 2:7 Series' ,              SYSDATETIME() , N'DbUp' , SYSDATETIME() , N'DbUp' )
    INSERT INTO dbo.Course ( Name , Description , Created , CreatedBy , Modified , ModifiedBy ) VALUES  ( N'Titus 2' , N'Titus 2' ,                           SYSDATETIME() , N'DbUp' , SYSDATETIME() , N'DbUp' )
END
