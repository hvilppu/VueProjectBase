CREATE OR ALTER PROCEDURE [dbo].[spAddUserRole]
	@UserID int,
	@RoleCode nvarchar(128)
AS
BEGIN
	DECLARE @RoleID INT
	SELECT @RoleID = coalesce(ID, -1) FROM Roles where Code = @RoleCode
	IF @RoleID != -1
	BEGIN
		IF NOT EXISTS (SELECT 1 From [dbo].[UserRoles] where RoleID = @RoleID and UserID = @UserID) BEGIN
			INSERT INTO [dbo].[UserRoles]
				   (UserID,
					RoleID)
			 VALUES
				   (@UserID,
					@RoleID);
		END
	END ELSE BEGIN
		DECLARE @msg nvarchar(256)
		IF @RoleID = -1 BEGIN
			SELECT @msg = FORMATMESSAGE('Koodilla %s ei löydy roolia.', @RoleCode); 
			THROW 50020, @msg, 0;
		END
	END
END
GO