using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using SZORM.Core;
using SZORM.Core.Emit;
using SZORM.Descriptors;
using SZORM.Factory;
using SZORM.Infrastructure;
using SZORM.Infrastructure.Interception;
using SZORM.Query;
using SZORM.Query.Internals;
using SZORM.Utility;

namespace SZORM
{
    public abstract partial  class DbContext : IDbContext
    {
        bool _disposed = false;
        internal InternalAdoSession _internalAdoSession;
        IStructure _dbStructCheck;
        internal ConcurrentDictionary<Type, TypeDescriptor> _typeDescriptors;
        IsolationLevel _il;
        internal IDbConnectionFactory _dbConnectionFactory;
        internal IDatabaseProvider _dbDatabaseProvider;
        internal string _dbConnectionStr;
        internal string _dbType;
        /// <summary>
        /// 默认打开事物处理
        /// </summary>
        public DbContext(IsolationLevel il = IsolationLevel.ReadCommitted)
        {
            Type type = GetType();
            DbContextInit(type.Name,il);
        }
        public DbContext(string dbContextName, IsolationLevel il = IsolationLevel.ReadCommitted)
        {
            DbContextInit(dbContextName, il);
        }
        public DbContext(string dbType, string connStr, IsolationLevel il = IsolationLevel.ReadCommitted)
        {
            _dbType = dbType;
            _dbConnectionStr = connStr;
            Init(il);
        }
        public void DbContextInit(string dbContextName,IsolationLevel il = IsolationLevel.ReadCommitted)
        {
            //获取名字
            var isHave = Cache.Get(dbContextName);
            if (isHave == null)
            {
                var builder = new ConfigurationBuilder();
                if (!File.Exists("szorm.json"))
                {
                    throw new Exception("没有配置文件szorm.json");
                }
                builder.AddJsonFile("szorm.json");


                var configuration = builder.Build();

                var tmpType = configuration[dbContextName + ":type"];
                if (string.IsNullOrEmpty(tmpType))
                {
                    throw new Exception("未配置" + dbContextName + "类型type");
                }
                var tmpConnStr = configuration[dbContextName + ":connStr"];
                if (string.IsNullOrEmpty(tmpConnStr))
                {
                    throw new Exception("未配置"+ dbContextName + "类型connStr");
                }

                Cache.Add(dbContextName, "1");
                Cache.Add(dbContextName + ":type", configuration[dbContextName + ":type"]);
                Cache.Add(dbContextName + ":connStr", configuration[dbContextName + ":connStr"]);
            }
            _dbType = Cache.Get(dbContextName + ":type").ToString();
            _dbConnectionStr = Cache.Get(dbContextName + ":connStr").ToString();
            Init(il);
        }
        public DbContext(IDbConnection conn,string DatabaseTyp, IsolationLevel il = IsolationLevel.ReadCommitted)
        {
            //直接进行初始化
            _dbConnectionStr = conn.ConnectionString;
            _dbType = DatabaseTyp;
            Init(il);
        }
        private void Init(IsolationLevel il)
        {
            _il = il;
            _dbDatabaseProvider = DatabaseProviderFactory.CreateDatabaseProvider(_dbType);

            //开始缓存数据结构,并初始化dbset
            _typeDescriptors = TypeDescriptors.GetTypeDescriptors(this);
            //初始化数据链接
            InitDb();
            //初始化数据库结构
            InternalAdoSession.BeginTransaction(_il);
        }

        public virtual IQuery<TEntity> Query<TEntity>()
        {
            return this.Query<TEntity>(null);
        }
        public virtual IQuery<TEntity> Query<TEntity>(string table)
        {
            return new Query<TEntity>(this, table);
        }

        internal IDatabaseProvider DatabaseProvider
        {
            get
            {
                this.CheckDisposed();
                return this._dbDatabaseProvider;
            }
        }
        

        internal InternalAdoSession InternalAdoSession
        {
            get
            {
                this.CheckDisposed();
                if (this._internalAdoSession == null)
                {
                    //如果为空,就去初始化工厂进行初始化
                    this._internalAdoSession = new InternalAdoSession(this._dbDatabaseProvider.CreateConnection(_dbConnectionStr));
                }
                return this._internalAdoSession;
            }
        }

        public void Dispose()
        {
            this._internalAdoSession.Dispose();
        }
        public void SetCommandTimeout(int time)
        {
            InternalAdoSession.CommandTimeout = time;
        }
        public int ExecuteNoQuery(string cmdText, params DbParam[] parameters)
        {
            return ExecuteNoQuery(cmdText, CommandType.Text, parameters);
        }

        public int ExecuteNoQuery(string cmdText, CommandType cmdType, params DbParam[] parameters)
        {
            Checks.NotNull(cmdText, "cmdText");
            return InternalAdoSession.ExecuteNonQuery(cmdText, parameters, cmdType);
        }

        public IDataReader ExecuteReader(string cmdText, params DbParam[] parameters)
        {
            return ExecuteReader(cmdText, CommandType.Text, parameters);
        }

        public IDataReader ExecuteReader(string cmdText, CommandType cmdType, params DbParam[] parameters)
        {
            Checks.NotNull(cmdText, "cmdText");
            return InternalAdoSession.ExecuteReader(cmdText, parameters, cmdType);
        }

        public object ExecuteScalar(string cmdText, params DbParam[] parameters)
        {
            return ExecuteScalar(cmdText, CommandType.Text, parameters);
        }
        

        public object ExecuteScalar(string cmdText, CommandType cmdType, params DbParam[] parameters)
        {
            Checks.NotNull(cmdText, "cmdText");
            return InternalAdoSession.ExecuteScalar(cmdText, parameters, cmdType);
        }

        public void Rollback()
        {
            this._internalAdoSession.CommitTransaction();
        }

        public void Save()
        {
            this._internalAdoSession.CommitTransaction();
            //初始化数据库结构
            this._internalAdoSession.BeginTransaction(_il);
        }
        void CheckDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        public DataTable ExecuteDataTable(string cmdText, params DbParam[] parameters)
        {
            return ExecuteDataTable(cmdText, CommandType.Text, parameters);
        }

        public DataTable ExecuteDataTable(string cmdText, CommandType cmdType, params DbParam[] parameters)
        {
            var reader = ExecuteReader(cmdText, parameters);
            DataTable dt = new DataTable();
            try
            {
                int fieldCount = reader.FieldCount;
                for (int i = 0; i < fieldCount; i++)
                {
                    DataColumn dc = new DataColumn(reader.GetName(i), reader.GetFieldType(i));
                    dt.Columns.Add(dc);
                }
                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < fieldCount; i++)
                    {
                        dr[i] = reader[i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            finally
            {
                reader.Close();
            }
            return dt;
        }

        public IEnumerable<T> ExecuteSqlToList<T>(string cmdText, params DbParam[] parameters)
        {
            return ExecuteSqlToList<T>(cmdText, CommandType.Text, parameters);
        }

        public IEnumerable<T> ExecuteSqlToList<T>(string cmdText, CommandType cmdType, params DbParam[] parameters)
        {
            Checks.NotNull(cmdText, "cmdText");
            return new InternalSqlQuery<T>(this, cmdText, cmdType, parameters);
        }

        protected abstract void AddInterceptor(IDbCommandInterceptor interceptor);

        protected abstract void RemoveInterceptor(IDbCommandInterceptor interceptor);

        public DbSet<SZORM_Upgrade> SZORM_Upgrades { get; set; }
    }
    public class SZORM_Upgrade
    {
        public int? Version { get; set; }
        [SZColumn(MaxLength =4000)]
        public string UPContent { get; set; }
        public DateTime? UPTime { get; set; }
        public DateTime? ReleaceTime { get; set; }
        [SZColumn(MaxLength = 4000)]
        public string ErrorMsg { get; set; }
    }
}
