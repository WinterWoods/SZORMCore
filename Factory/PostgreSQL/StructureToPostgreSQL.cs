using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SZORM;
using SZORM.Factory;
using SZORM.Factory.Models;

namespace SZORM.Factory.PostgreSQL
{
    class StructureToPostgreSQL : IStructure
    {
        public void ColumnAdd(DbContext dbContext, string tableName, ColumnModel model)
        {
            var SqlGenerator = dbContext.DatabaseProvider.GetSqlGenerator();
            //ALTER TABLE products ADD COLUMN description text;
            string sql = "ALTER TABLE " + SqlGenerator.GetQuoteName(tableName) + " ADD COLUMN " + FieldString(dbContext, model) + "";
            dbContext.ExecuteNoQuery(sql);
        }

        public void ColumnEdit(DbContext dbContext, string tableName, ColumnModel model)
        {
            var SqlGenerator = dbContext.DatabaseProvider.GetSqlGenerator();
            //alter table "member" alter  COLUMN  imgfileid  type int ;
            string sql = "ALTER TABLE " + SqlGenerator.GetQuoteName(tableName) + " alter  COLUMN " + EditFieldString(dbContext, model);
            dbContext.ExecuteNoQuery(sql);
        }

        public List<ColumnModel> ColumnList(DbContext dbContext, string tableName)
        {
            string sql = @"SELECT C.relname,
	                        A.attname,
	                        A.attnum,
                        CASE
		                        WHEN conkey IS NOT NULL THEN
		                        'PK' ELSE'' 
	                        END pk,
                        CASE
	                        A.attnotnull 
                        WHEN TRUE THEN
	                        'not null' ELSE'' 
	                        END isnotnull,
                         d.typname,
	                        pg_catalog.format_type ( A.atttypid, A.atttypmod ) format_type
                        FROM
	                        pg_attribute A 
	                        INNER JOIN pg_class C ON A.attrelid = C.oid
	                        LEFT JOIN pg_type d ON A.atttypid = d.oid
	                        LEFT JOIN ( SELECT conrelid, UNNEST ( conkey ) conkey FROM pg_constraint WHERE contype = 'p' ) b ON C.oid = b.conrelid 
	                        AND A.attnum = b.conkey 
                        WHERE
	                        C.relname = '" + tableName+"' AND A.attnum > 0 ORDER BY  A.attnum;";
            List<ColumnModel> result = new List<ColumnModel>();

            DataTable table = dbContext.ExecuteDataTable(sql);
            for (var i = 0; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                ColumnModel model = new ColumnModel();
                model.Name = row["attname"].ToString();
                //这个位置不对.明天再计算
                string dbType = row["typname"].ToString();
                string formateType= row["format_type"].ToString();
                var cloumsFullType = "";
                switch(dbType)
                {
                    case "varchar":
                        cloumsFullType = formateType.Replace("character varying", "varchar");
                        break;
                    case "bool":
                        cloumsFullType = "bool";
                        break;
                    case "timestamp":
                        cloumsFullType = "timestamp";
                        break;
                    case "text":
                        cloumsFullType = "text";
                        break;
                    case "int2":
                        cloumsFullType = "int2";
                        break;
                    case "int4":
                        cloumsFullType = "int4";
                        break;
                    case "int8":
                        cloumsFullType = "int8";
                        break;
                    case "float4":
                        cloumsFullType = "float4";
                        break;
                    case "float8":
                        cloumsFullType = "float8";
                        break;
                    case "numeric":
                        cloumsFullType = formateType;
                        break;
                    case "money":
                        cloumsFullType = "money";
                        break;
                    case "bytea":
                        cloumsFullType = "bytea";
                        break;
                    default:
                        break;
                }
                model.ColumnFullType = cloumsFullType.ToUpper();
                model.IsKey = !string.IsNullOrEmpty(row["pk"].ToString());
                if (model.IsKey)
                {
                    model.Required = true;
                }
                else
                {
                    model.Required = row["isnotnull"].ToString() == "not null";
                }
                result.Add(model);
            }
            return result;
        }

        public void CreateTable(DbContext dbContext, TableModel model)
        {
            var SqlGenerator = dbContext.DatabaseProvider.GetSqlGenerator();
            StringBuilder str = new StringBuilder();
            str.AppendFormat("CREATE TABLE {0}", SqlGenerator.GetQuoteName(model.Name));
            str.Append("(");
            List<string> fields = new List<string>();
            string key = "";
            model.Columns.ForEach(f =>
            {
                if (f.IsKey)
                {
                    key = f.Name;
                }
                fields.Add(FieldString(dbContext, f));
            });
            if (!string.IsNullOrEmpty(key))
                fields.Add("PRIMARY KEY (" + SqlGenerator.GetQuoteName(key) + ")");

            str.Append(string.Join(",", fields));
            str.Append(")");
            dbContext.ExecuteNoQuery(str.ToString());
        }
        string FieldString(DbContext dbContext, ColumnModel column)
        {
            var SqlGenerator = dbContext.DatabaseProvider.GetSqlGenerator();
            StringBuilder str = new StringBuilder();
            str.AppendFormat("{0} {1}", SqlGenerator.GetQuoteName(column.Name), FieldType(column));
            if (column.Required)
                str.Append("  NOT NULL ");

            return str.ToString();
        }
        string EditFieldString(DbContext dbContext, ColumnModel column)
        {
            var SqlGenerator = dbContext.DatabaseProvider.GetSqlGenerator();
            StringBuilder str = new StringBuilder();
            str.AppendFormat("{0} type {1}", SqlGenerator.GetQuoteName(column.Name), FieldType(column));
            if (column.Required)
                str.Append("  NOT NULL ");

            return str.ToString();
        }
        public string FieldType(ColumnModel column)
        {
            string result = string.Empty;
            if (column.type == typeof(bool))
            {
                result = "BOOL";
            }
            else if (column.type == typeof(string))
            {
                if (column.IsText)
                {
                    result = "TEXT";
                }
                else
                    result = "VARCHAR(" + column.MaxLength + ")";
            }
            else if (column.type == typeof(DateTime))
            {
                result = "TIMESTAMP";
            }
            else if (column.type == typeof(Int16))
            {
                result = "INT2";
            }
            else if (column.type == typeof(Int32))
            {
                result = "INT4";
            }
            else if (column.type == typeof(Int64))
            {
                result = "INT8";
            }
            else if (column.type == typeof(Single))
            {
                result = "FLOAT4";
            }
            else if (column.type == typeof(Double))
            {
                result = "FLOAT8";
            }
            else if (column.type == typeof(Decimal))
            {
                if (column.NumberSize == 0 && column.NumberPrecision == 0)
                {
                    result = "NUMERIC";
                }
                else if (column.NumberSize != 0 && column.NumberPrecision == 0)
                {
                    if (column.NumberSize < 1 || column.NumberSize > 1000)
                        throw new Exception("Decimal类型的长度必须是\">=1\",\"<=1000\"");
                    result = "NUMERIC(" + column.NumberSize + ",0)";
                }
                else
                {
                    if (column.NumberSize < 1 || column.NumberSize > 1000)
                        throw new Exception("Decimal类型的长度必须是\">=1\",\"<=1000\"");
                    if (column.NumberPrecision < -84 || column.NumberPrecision > 1000)
                        throw new Exception("Decimal类型的精度必须是\">=-84\",\"<=1000\"");
                    if (column.NumberPrecision > column.NumberSize)
                        throw new Exception("Decimal类型的精度不能大于长度");
                    result = "NUMERIC(" + column.NumberSize + "," + column.NumberPrecision + ")";
                }
            }
            else if (column.type == typeof(Byte[]))
            {
                result = "BYTEA";
            }
            else
            {
                if (column.type.BaseType == typeof(Enum))
                {
                    result = "INT4";
                }
                else
                    throw new Exception("暂时不支持的数据类型:" + column.Name + "-" + column.type.ToString());
            }
            return result;
        }

        public List<TableModel> TableList(DbContext dbContext)
        {
            string sql = "SELECT tablename FROM pg_tables;";

            List<TableModel> tableList = new List<TableModel>();
            DataTable table = dbContext.ExecuteDataTable(sql);
            for (var i = 0; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                var name = row[0].ToString();
                tableList.Add(new TableModel { Name = name });
            }
            return tableList;
        }
    }
}
