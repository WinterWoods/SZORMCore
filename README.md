# SZORMCore
.net core框架下的 ORM,支持mssql,mysql,oracle,sqlite

## 使用方式

#### 定义一个基础类

```
/// <summary>
/// 基础字段
/// </summary>
public class Basic
{
    /// <summary>
    /// 主键
    /// </summary>
    [SZColumn(MaxLength = 32, IsKey = true)]
    public string Key { get; set; }
    /// <summary>
    /// 添加时间,首次添加记录会自动更新
    /// </summary>
    [SZColumn(IsAddTime = true)]
    public DateTime? AddTime { get; set; }
    /// <summary>
    /// 修改时间,该时间会每次更新的时候自动更新
    /// </summary>
    [SZColumn(IsEditTime = true)]
    public DateTime? EditTime { get; set; }
}
```
#### 定义一个实体类
```
public class UserInfo:Basic
{
    /// <summary>
    /// 登录用户名
    /// </summary>
    [SZColumn(MaxLength = 100)]
    public string Name { get; set; }
    /// <summary>
    /// 手机号
    /// </summary>
    [SZColumn(MaxLength = 11)]
    public string Tel { get; set; }
    /// <summary>
    /// 是否锁定,如果锁定后,将不能登录
    /// </summary>
    public bool IsLock { get => isLock; set => isLock = value; }

    private bool isLock = false;
    /// <summary>
    /// 备注信息
    /// </summary>
    public string Remarks { get; set; }
}
```

#### szorm.json
```
{
  "DB": {
    "type": "MySql",
    "connStr": "Database='test';Data Source='127.0.0.1';User Id='root';Password='sz06181102';charset='utf8mb4';pooling=false"
  },
  "DB1": {
    "type": "Sqlite",
    "connStr": "data source=EF6.db"
  },
  "DB2": {
    "type": "Oracle",
    "connStr": "user id=szorm;password=szorm;data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=127.0.0.1)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)))"
  },
  "DB3": {
    "type": "SqlServer",
    "connStr": "Data Source=127.0.0.1,1433;Initial Catalog=test;User ID=yy;Password=yy;"
  }
}
  ```

  #### 继承DbContext,类名称与字符串名称一致"DB"
```
public class DB : DbContext
{
    /// <summary>
    /// 用户信息,用于登录
    /// </summary>
    public DbSet<UserInfo> UserInfos { get; set; }
    protected override void Initialization()
    {
    }
    protected override void UpdataDBExce()
    {
    }
}
```
#### ok大功告成,您可以使用啦.

## 常用方法

#### 添加
```
using (DB db = new DB())
{
    UserInfo model=new UserInfo();
    model.Name="张三";
    model.Tel="1888888888";
    model.Remarks="测试";
    db.UserInfos.Add(model);
    db.Save();
}
```

#### 修改
```
using (DB db = new DB())
{
    UserInfo model=db.UserInfos.Find(key)
    model.Name="张三";
    model.Tel="1888888888";
    model.Remarks="测试";
    db.UserInfos.Edit(model);
    db.Save();
}
```

#### 删除
```
using (DB db = new DB())
{
    db.UserInfos.Remove(key)
    db.Save();
}
```

#### 单表查询
```
using (DB db = new DB())
{
    var query = db.UserInfos.AsQuery();
    query = query.Where(w => w.LoginName.Contains("张"));
    query = query.OrderByDesc(o => o.AddTime);
    var result= query.ToList();
}
```
