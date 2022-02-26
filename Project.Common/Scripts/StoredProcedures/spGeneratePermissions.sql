CREATE OR ALTER PROCEDURE [dbo].[spGeneratePermissions]
AS BEGIN
	SELECT MV._data FROM (
		SELECT '// ############# PERMISSIONS ############# ' as _data, 3 as _order
			UNION
		SELECT 'public const string '+Perm.Code+' = nameof('+Perm.Code+');' as _data, 4 as _order FROM Permissions Perm
	 ) MV ORDER BY MV._order
END
