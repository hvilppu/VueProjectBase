GO  
USE master
GO
IF EXISTS(select * from sys.databases where name='ProjectDB')
BEGIN
alter database ProjectDB set single_user with rollback immediate
DROP DATABASE ProjectDB
END
GO
CREATE DATABASE ProjectDB 
GO  
USE ProjectDB;  
ALTER DATABASE [ProjectDB] SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE
GO

GO  
IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Users]  
(  
   [ID] [int] IDENTITY(1,1) NOT NULL,  
   [UserName] [nvarchar]  (255) NULL,  
   [FirstName] [nvarchar]  (255) NULL,  
   [SecondName] [nvarchar]  (255) NULL,  
   [Phone] [nvarchar] (255) NULL, 
   [Email] [nvarchar] (255) NULL, 
   [PasswordSet] [bit] NOT NULL DEFAULT(0),
   [Password] [nvarchar] (255) NULL, 
   [CreationDate] [datetime] NULL, 
   [Active] [bit] NOT NULL DEFAULT(1), 
   [Deleted] [bit] NOT NULL DEFAULT(0), 
   [LastModified] [datetime] NULL, 
   [Modifier] [nvarchar] (255) NULL,
   [ValidUntil] [datetime] NULL,
   CONSTRAINT PK_Users PRIMARY KEY CLUSTERED (ID)  
);  
END
GO 

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[UserRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserRoles]  
(  
   [UserID] [int] NOT NULL,  
   [RoleID] [int] NOT NULL,
   CONSTRAINT PK_UserRoles PRIMARY KEY CLUSTERED (
   [UserID] asc,
   [RoleID] asc
  ) 
);  
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[RolePermissions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RolePermissions]  
(  
   [RoleID] [int] NOT NULL,  
   [PermissionID] [int] NOT NULL,
   CONSTRAINT PK_RolePermissions PRIMARY KEY CLUSTERED (
   [RoleID] asc,
   [PermissionID] asc
  ) 
);  
END
GO  

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Roles]  
(  
   [ID] [int] IDENTITY(1,1) NOT NULL,  
   [Code] [nvarchar] (255) NULL,
   [CreationDate] [datetime] NULL, 
   [Active] [bit] NOT NULL DEFAULT(1), 
   [Deleted] [bit] NOT NULL DEFAULT(0), 
   [LastModified] [datetime] NULL, 
   [Modifier] [nvarchar] (255) NULL,
   [ValidUntil] [datetime] NULL,
   CONSTRAINT PK_Roles PRIMARY KEY CLUSTERED (ID)  
);  
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[dbo].[Permissions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Permissions]  
(  
   [ID] [int] IDENTITY(1,1) NOT NULL, 
   [Code] [nvarchar] (255) NULL,
   [CreationDate] [datetime] NULL, 
   [Active] [bit] NOT NULL DEFAULT(1), 
   [Deleted] [bit] NOT NULL DEFAULT(0), 
   [LastModified] [datetime] NULL, 
   [Modifier] [nvarchar] (255) NULL,
   [ValidUntil] [datetime] NULL,
   CONSTRAINT PK_Permissions PRIMARY KEY CLUSTERED (ID)  
);  
END
GO