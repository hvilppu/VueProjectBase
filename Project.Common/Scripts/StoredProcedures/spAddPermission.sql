CREATE OR ALTER PROCEDURE [dbo].[spAddPermission]
	@Code nvarchar(128)
AS
BEGIN
	IF NOT EXISTS(SELECT 1 FROM [dbo].[Permissions] WHERE [Code] = @Code)
	BEGIN
		INSERT INTO [dbo].[Permissions]
			   ([Code])
		 VALUES
			   (@Code);
	END
END
GO
