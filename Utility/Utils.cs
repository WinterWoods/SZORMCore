using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SZORM.DbExpressions;
using SZORM.Extensions;

namespace SZORM.Utility
{
    static class Utils
    {
        static readonly Dictionary<Type, Type> ToStringableNumericTypes;
        static Utils()
        {
            List<Type> toStringableNumericTypes = new List<Type>();
            toStringableNumericTypes.Add(typeof(byte));
            toStringableNumericTypes.Add(typeof(sbyte));
            toStringableNumericTypes.Add(typeof(short));
            toStringableNumericTypes.Add(typeof(ushort));
            toStringableNumericTypes.Add(typeof(int));
            toStringableNumericTypes.Add(typeof(uint));
            toStringableNumericTypes.Add(typeof(long));
            toStringableNumericTypes.Add(typeof(ulong));
            ToStringableNumericTypes = toStringableNumericTypes.ToDictionary(a => a, a => a);
        }
        public static void CheckNull(object obj, string paramName = null)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName);
        }
        public static bool AreEqual(object obj1, object obj2)
        {
            if (obj1 == null && obj2 == null)
                return true;

            if (obj1 != null)
            {
                return obj1.Equals(obj2);
            }

            if (obj2 != null)
            {
                return obj2.Equals(obj1);
            }

            return object.Equals(obj1, obj2);
        }
        public static string GenerateUniqueColumnAlias(DbSqlQueryExpression sqlQuery, string defaultAlias = UtilConstants.DefaultColumnAlias)
        {
            string alias = defaultAlias;
            int i = 0;
            while (sqlQuery.ColumnSegments.Any(a => string.Equals(a.Alias, alias, StringComparison.OrdinalIgnoreCase)))
            {
                alias = defaultAlias + i.ToString();
                i++;
            }

            return alias;
        }

        public static Dictionary<TKey, TValue> Clone<TKey, TValue>(Dictionary<TKey, TValue> source)
        {
            Dictionary<TKey, TValue> ret = Clone<TKey, TValue>(source, source.Count);
            return ret;
        }
        public static Dictionary<TKey, TValue> Clone<TKey, TValue>(Dictionary<TKey, TValue> source, int capacity)
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(capacity);

            foreach (var kv in source)
            {
                ret.Add(kv.Key, kv.Value);
            }

            return ret;
        }
        public static Dictionary<TKey, TValue> Clone<TKey, TValue>(Dictionary<TKey, TValue> source, IEqualityComparer<TKey> comparer = null)
        {
            Dictionary<TKey, TValue> ret;
            if (comparer == null)
                ret = new Dictionary<TKey, TValue>(source.Count);
            else
                ret = new Dictionary<TKey, TValue>(source.Count, comparer);

            foreach (var kv in source)
            {
                ret.Add(kv.Key, kv.Value);
            }

            return ret;
        }
        public static Task<T> MakeTask<T>(Func<T> func)
        {
            var task = new Task<T>(func);
            task.Start();
            return task;
        }
        public static List<T> Clone<T>(List<T> source, int? capacity = null)
        {
            List<T> ret = new List<T>(capacity ?? source.Count);
            ret.AddRange(source);
            return ret;
        }
        public static List<T> CloneAndAppendOne<T>(List<T> source, T t)
        {
            List<T> ret = new List<T>(source.Count + 1);
            ret.AddRange(source);
            ret.Add(t);
            return ret;
        }

        public static DbJoinType AsDbJoinType(this JoinType joinType)
        {
            switch (joinType)
            {
                case JoinType.InnerJoin:
                    return DbJoinType.InnerJoin;
                case JoinType.LeftJoin:
                    return DbJoinType.LeftJoin;
                case JoinType.RightJoin:
                    return DbJoinType.RightJoin;
                case JoinType.FullJoin:
                    return DbJoinType.FullJoin;
                default:
                    throw new NotSupportedException();
            }
        }
        public static List<List<T>> InBatches<T>(List<T> source, int batchSize)
        {
            List<List<T>> batches = new List<List<T>>();

            List<T> batch = new List<T>(source.Count > batchSize ? batchSize : source.Count);
            for (int i = 0; i < source.Count; i++)
            {
                var item = source[i];
                batch.Add(item);
                if (batch.Count >= batchSize)
                {
                    batches.Add(batch);
                    batch = new List<T>();
                }
            }

            if (batch.Count > 0)
            {
                batches.Add(batch);
            }

            return batches;
        }
        public static bool IsToStringableNumericType(Type type)
        {
            type = ReflectionExtension.GetUnderlyingType(type);
            return ToStringableNumericTypes.ContainsKey(type);
        }
        public static string ToMethodString(this MethodInfo method)
        {
            StringBuilder sb = new StringBuilder();
            ParameterInfo[] parameters = method.GetParameters();

            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo p = parameters[i];

                if (i > 0)
                    sb.Append(",");

                string s = null;
                if (p.IsOut)
                    s = "out ";

                sb.AppendFormat("{0}{1} {2}", s, p.ParameterType.Name, p.Name);
            }

            return string.Format("{0}.{1}({2})", method.DeclaringType.Name, method.Name, sb.ToString());
        }
        public static Type GetFuncDelegateType(Type[] typeArguments)
        {
            int parameters = typeArguments.Length;
            Type funcType = null;
            switch (parameters)
            {
                case 3:
                    funcType = typeof(Func<,,>);
                    break;
                case 4:
                    funcType = typeof(Func<,,,>);
                    break;
                case 5:
                    funcType = typeof(Func<,,,,>);
                    break;
                case 6:
                    funcType = typeof(Func<,,,,,>);
                    break;
                default:
                    throw new NotSupportedException();
            }

            return funcType.MakeGenericType(typeArguments);
        }
    }
}
