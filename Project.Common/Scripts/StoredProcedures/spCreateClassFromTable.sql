IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spCreateClassFromTable')
    DROP PROCEDURE [dbo].[spCreateClassFromTable]
GO

CREATE PROCEDURE [dbo].[spCreateClassFromTable]
	@table varchar(max),
	@nameSpace varchar(max) = 'Project.Domain.Models',
	@schema varchar(max) = 'dbo',
	@IsUpdateable bit = 1
AS
BEGIN
	DECLARE @TableName VARCHAR(MAX) = @table;
    DECLARE @TableSchema VARCHAR(MAX) = @schema;
    DECLARE @result varchar(max) = ''

    SET @result = @result + 'using System;'+ CHAR(13) +
							'using Newtonsoft.Json;'+ CHAR(13)  +
							'using System.ComponentModel.DataAnnotations.Schema;'+ CHAR(13)  +
					+ CHAR(13) 
					+ CHAR(13); 


    SET @result = @result + 'namespace ' + @nameSpace  + CHAR(13) + '{' + CHAR(13) 

	SET @result = @result + '[Table("'+@TableName+'")]';

    SET @result = @result + 'public class ' + @TableName;

	IF @IsUpdateable = 1
	BEGIN
		SET @result = @result + ': IUpdateable<'+@TableName+'>' + CHAR(13);
	END
	ELSE
	BEGIN
		SET @result = @result + ': IModel' + CHAR(13);
	END

	SET @result = @result + '{' + CHAR(13) 

    SET @result = @result + '#region Instance Properties' + CHAR(13)  

   SELECT
      @result = @result + CHAR(13)
      + ' public ' + ColumnType + ' ' + ColumnName + ' { get; set; } ' + CHAR(13)
    FROM (SELECT
      c.COLUMN_NAME AS ColumnName,
      CASE c.DATA_TYPE
        WHEN 'bigint' THEN CASE C.IS_NULLABLE
            WHEN 'YES' THEN 'Int64?'
            ELSE 'Int64'
          END
        WHEN 'binary' THEN 'Byte[]'
        WHEN 'bit' THEN CASE C.IS_NULLABLE
            WHEN 'YES' THEN 'bool?'
            ELSE 'bool'
          END
        WHEN 'char' THEN 'string'
        WHEN 'date' THEN CASE C.IS_NULLABLE
            WHEN 'YES' THEN 'DateTime?'
            ELSE 'DateTime'
          END
        WHEN 'datetime' THEN CASE C.IS_NULLABLE
            WHEN 'YES' THEN 'DateTime?'
            ELSE 'DateTime'
          END
        WHEN 'datetime2' THEN CASE C.IS_NULLABLE
            WHEN 'YES' THEN 'DateTime?'
            ELSE 'DateTime'
          END
        WHEN 'datetimeoffset' THEN CASE C.IS_NULLABLE
            WHEN 'YES' THEN 'DateTimeOffset?'
            ELSE 'DateTimeOffset'
          END
        WHEN 'decimal' THEN CASE C.IS_NULLABLE
            WHEN 'YES' THEN 'decimal?'
            ELSE 'decimal'
          END
        WHEN 'float' THEN CASE C.IS_NULLABLE
            WHEN 'YES' THEN 'Single?'
            ELSE 'Single'
          END
        WHEN 'image' THEN 'Byte[]'
        WHEN 'int' THEN CASE C.IS_NULLABLE
            WHEN 'YES' THEN 'int?'
            ELSE 'int'
          END
        WHEN 'money' THEN CASE C.IS_NULLABLE
            WHEN 'YES' THEN 'decimal?'
            ELSE 'decimal'
          END
        WHEN 'nchar' THEN 'string'
        WHEN 'ntext' THEN 'string'
        WHEN 'numeric' THEN CASE C.IS_NULLABLE
            WHEN 'YES' THEN 'decimal?'
            ELSE 'decimal'
          END
        WHEN 'nvarchar' THEN 'string'
        WHEN 'real' THEN CASE C.IS_NULLABLE
            WHEN 'YES' THEN 'Double?'
            ELSE 'Double'
          END
        WHEN 'smalldatetime' THEN CASE C.IS_NULLABLE
            WHEN 'YES' THEN 'DateTime?'
            ELSE 'DateTime'
          END
        WHEN 'smallint' THEN CASE C.IS_NULLABLE
            WHEN 'YES' THEN 'Int16?'
            ELSE 'Int16'
          END
        WHEN 'smallmoney' THEN CASE C.IS_NULLABLE
            WHEN 'YES' THEN 'decimal?'
            ELSE 'decimal'
          END
        WHEN 'text' THEN 'string'
        WHEN 'time' THEN CASE C.IS_NULLABLE
            WHEN 'YES' THEN 'TimeSpan?'
            ELSE 'TimeSpan'
          END
        WHEN 'timestamp' THEN 'Byte[]'
        WHEN 'tinyint' THEN CASE C.IS_NULLABLE
            WHEN 'YES' THEN 'Byte?'
            ELSE 'Byte'
          END
        WHEN 'uniqueidentifier' THEN CASE C.IS_NULLABLE
            WHEN 'YES' THEN 'Guid?'
            ELSE 'Guid'
          END
        WHEN 'varbinary' THEN 'Byte[]'
        WHEN 'varchar' THEN 'string'
        ELSE 'Object'
      END AS ColumnType,
      c.ORDINAL_POSITION
    FROM INFORMATION_SCHEMA.COLUMNS c
    WHERE c.TABLE_NAME = @TableName
    AND ISNULL(@TableSchema, c.TABLE_SCHEMA) = c.TABLE_SCHEMA) t
    ORDER BY t.ORDINAL_POSITION

    SET @result = @result + CHAR(13) + '#endregion Instance Properties' + CHAR(13)  

    SET @result = @result  + '}' + CHAR(13)

    IF (@TableSchema IS NOT NULL) 
    BEGIN
        SET @result = @result + CHAR(13) + '}' 
    END

    PRINT @result
END

