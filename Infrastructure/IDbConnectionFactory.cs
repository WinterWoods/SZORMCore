using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SZORM.Infrastructure
{
    /// <summary>
    /// 用于扩展自己的数据库
    /// </summary>
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
        string DatabaseType { get; }
    }
}
