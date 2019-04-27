using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public void ColumnEdit(DbContext dbContext, string tableName, ColumnModel model)
        {
            throw new NotImplementedException();
        }

        public List<ColumnModel> ColumnList(DbContext dbContext, string tableName)
        {
            throw new NotImplementedException();
        }

        public void CreateTable(DbContext dbContext, TableModel model)
        {
            throw new NotImplementedException();
        }

        public string FieldType(ColumnModel column)
        {
            throw new NotImplementedException();
        }

        public List<TableModel> TableList(DbContext dbContext)
        {
            throw new NotImplementedException();
        }
    }
}
