USE $DbName$;
GO

IF OBJECT_ID(N'FK_Address_AddressType') IS NULL
BEGIN
	ALTER TABLE dbo.Address WITH CHECK ADD CONSTRAINT FK_Address_AddressType FOREIGN KEY(AddressTypeId)
	REFERENCES dbo.AddressType (Id);
END;
