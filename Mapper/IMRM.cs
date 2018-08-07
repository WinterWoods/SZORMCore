using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using SZORM.Core;
using SZORM.Core.Emit;
using SZORM.Extensions;
using SZORM.Infrastructure;

namespace SZORM.Mapper
{
    public interface IMRM
    {
        void Map(object instance, IDataReader reader, int ordinal);
    }

    static class MRMHelper
    {
        public static IMRM CreateMRM(MemberInfo member, MappingTypeInfo mappingTypeInfo)
        {
            if (mappingTypeInfo.DbValueConverter == null || member.GetMemberType().GetUnderlyingType().IsEnum /* 枚举比较特殊 */)
            {
                Type type = ClassGenerator.CreateMRMType(member);
                IMRM obj = (IMRM)type.GetConstructor(Type.EmptyTypes).Invoke(null);
                return obj;
            }
            else
            {
                return new MRM2(member, mappingTypeInfo.DbValueConverter);
            }
        }
    }

    class MRM : IMRM
    {
        Action<object, IDataReader, int> _mapper;
        public MRM(MemberInfo member)
        {
            this._mapper = DelegateGenerator.CreateSetValueFromReaderDelegate(member);
        }

        public void Map(object instance, IDataReader reader, int ordinal)
        {
            this._mapper(instance, reader, ordinal);
        }
    }

    class MRM2 : IMRM
    {
        Action<object, object> _valueSetter;
        IDbValueConverter _dbValueConverter;
        public MRM2(MemberInfo member, IDbValueConverter dbValueConverter)
        {
            this._dbValueConverter = dbValueConverter;
            this._valueSetter = DelegateGenerator.CreateValueSetter(member);
        }

        public void Map(object instance, IDataReader reader, int ordinal)
        {
            object val = reader.GetValue(ordinal);
            val = this._dbValueConverter.Convert(val);
            this._valueSetter(instance, val);
        }
    }
}
