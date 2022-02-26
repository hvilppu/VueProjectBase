CREATE OR ALTER PROCEDURE [dbo].[spGenerateRoles]
AS BEGIN
	SELECT MV._data FROM (
		SELECT '// ############# ROLES ############# ' as _data, 1 as _order
			UNION
		SELECT 'public const string '+Roles.Code+' = nameof('+Roles.Code+');' as _data, 2 as _order FROM Roles Roles
	 ) MV ORDER BY MV._order
END
