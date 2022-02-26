CREATE OR ALTER PROCEDURE [dbo].[spAddRole]
	@Code nvarchar(128),
	@UserID int = -1
AS
BEGIN
	IF NOT EXISTS(SELECT 1 FROM [dbo].[Role] WHERE [Code] = @Code)
	BEGIN
		INSERT INTO [dbo].[Role]
			   (Code,
				DictionaryKey,
				CTS, 
				CID,
				UTS,
				UID,
				Deleted,
				DTS,
				DID)
		 VALUES
			   (@Code,
				@DictionaryKey,
				GETUTCDATE(), 
				@UserID,
				GETUTCDATE(),
				@UserID,
			    0,
			    null,
			    null);
	END
	ELSE
	BEGIN
		UPDATE [dbo].[SecurityRole] SET 
			[DictionaryKey] = @DictionaryKey, [UTS] = GETUTCDATE(), [UID] = @UserID
		WHERE 
			[Code] = @Code
	END
END
GO
