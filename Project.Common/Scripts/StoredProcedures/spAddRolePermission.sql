CREATE OR ALTER PROCEDURE [dbo].[spAddRolePermission]
	@RoleCode nvarchar(128),
	@PermissionCode nvarchar(128),
	@UserID int = -1
AS
BEGIN
	DECLARE @PermissionID INT, @RoleID INT
	SELECT @PermissionID = coalesce(ID, -1) FROM Permissions where Code = @PermissionCode
	SELECT @RoleID = coalesce(ID, -1) FROM Roles where Code = @RoleCode
	IF @PermissionID != -1 AND  @RoleID != -1
	BEGIN
		IF NOT EXISTS (SELECT 1 From [dbo].[RolePermissions] where PermissionID = @PermissionID and RoleID = @RoleID) BEGIN
			INSERT INTO [dbo].[RolePermissions]
				   (RoleID,
					PermissionID)
			 VALUES
				   (@RoleID,
					@PermissionID);
		END
	END ELSE BEGIN
		DECLARE @msg nvarchar(256)
		IF @RoleID = -1 BEGIN
			SELECT @msg = FORMATMESSAGE('Code %s do not have role.', @RoleCode); 
			THROW 50020, @msg, 0;
		END
		ELSE
			SELECT @msg = FORMATMESSAGE('Code %s do not have permission.', @PermissionCode);
			THROW 50021, @msg, 0;
	END
END
GO