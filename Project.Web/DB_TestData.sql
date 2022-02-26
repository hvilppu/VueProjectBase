---
--- RUN AFTER SERVER IS STARTED!!
---

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
			SELECT @msg = FORMATMESSAGE('Koodilla %s ei löydy roolia.', @RoleCode); 
			THROW 50020, @msg, 0;
		END
		ELSE
			SELECT @msg = FORMATMESSAGE('Koodilla %s ei löydy oikeutta.', @PermissionCode);
			THROW 50021, @msg, 0;
	END
END
GO

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

--Antti 	aaaa!	Admin
INSERT [dbo].[Users] ([UserName], [FirstName], [SecondName] ,[Password], [PasswordSet]) Values ('Antti','Antti', 'Admin', 'P/58hgTrwnWQ5dp5LTX3cOQH1i4klXjveuE4WPOJAEI=', 'True') -- Password = aaaa!		

INSERT [dbo].[Roles] ([Code]) Values ('Admin') -- ID 1

INSERT [dbo].[Permissions] ([Code]) Values ('Users_Admin')
INSERT [dbo].[Permissions] ([Code]) Values ('Common_Admin')

exec spAddRolePermission 'Admin','Users_Admin'
exec spAddRolePermission 'Admin','Common_Admin'

select @userID = ID from Users where UserName like 'Antti';
exec spAddUserRole @userID,'Admin'
exec spAddUserRole @userID,'User'

