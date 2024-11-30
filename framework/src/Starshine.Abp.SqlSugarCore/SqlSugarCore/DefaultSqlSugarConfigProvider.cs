using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;

namespace Starshine.Abp.SqlSugarCore.SqlSugarCore
{
    public class DefaultSqlSugarConfigProvider : ISqlSugarConfigProvider
    {
        public Task<ConnectionConfig> BuildConnectionConfig(DbSettingsOptions dbConnectionConfig)
        {
            var configureExternalServices = new ConfigureExternalServices
            {
                EntityNameService = (type, entity) => // 处理表
                {
                    if (config.EnableUnderLine && !entity.DbTableName.Contains('_'))
                        entity.DbTableName = UtilMethods.ToUnderLine(entity.DbTableName); // 驼峰转下划线

                },
                EntityService = (type, column) => // 处理列
                {
                    if (type.GetIndexParameters().Length > 0)
                    {
                        column.IsIgnore = true;
                        return;
                    }
                    if (column.IsPrimarykey == false && type.PropertyType.IsGenericType
                        && type.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        column.IsNullable = true;
                    }


                    if (config.DbType == DbType.Oracle)
                    {
                        if (type.PropertyType == typeof(long) || type.PropertyType == typeof(long?))
                            column.DataType = "number(18)";
                        if (type.PropertyType == typeof(bool) || type.PropertyType == typeof(bool?))
                            column.DataType = "number(1)";
                    }
                    if (!type.IsDefined(typeof(SugarColumn), false))
                    {
                        if (config.EnableUnderLine && !column.IsIgnore && !column.DbColumnName.Contains('_'))
                            column.DbColumnName = UtilMethods.ToUnderLine(column.DbColumnName); // 驼峰转下划线
                        if (type.ReflectedType?.IsAssignableTo<ISoftDelete>() ?? false && type.Name.Equals(nameof(ISoftDelete.IsDeleted)))
                        {
                            column.DefaultValue = "false";
                            column.ColumnDescription = "是否删除";
                        }
                        else if (type.ReflectedType?.IsAssignableTo<IHasDeletionTime>() ?? false && type.Name.Equals(nameof(IHasDeletionTime.DeletionTime)))
                        {
                            column.IsNullable = true;
                            column.ColumnDescription = "删除时间";
                            column.DefaultValue = "false";
                        }
                        else if (type.ReflectedType?.IsAssignableTo<IHasDeletionTime>() ?? false && type.Name.Equals(nameof(IHasDeletionTime.DeletionTime)))
                        {
                            column.IsNullable = true;
                            column.ColumnDescription = "删除时间";
                            column.DefaultValue = "false";
                        }
                        else if (type.ReflectedType?.IsAssignableTo<IHasCreationTime>() ?? false && type.Name.Equals(nameof(IHasCreationTime.CreationTime)))
                        {
                            column.IsNullable = false;
                            column.ColumnDescription = "创建时间";
                            column.DefaultValue = "false";
                            b.Property(nameof(IHasCreationTime.CreationTime))
                                .IsRequired()
                                .HasColumnName(nameof(IHasCreationTime.CreationTime));
                        }

                    }
                }
            };

            config.ConfigureExternalServices = configureExternalServices;
            config.InitKeyType = InitKeyType.Attribute;
            config.IsAutoCloseConnection = true;
            config.MoreSettings = new ConnMoreSettings
            {
                IsAutoRemoveDataCache = true,
                SqlServerCodeFirstNvarchar = true // 采用Nvarchar
            };
            return config;
        }
    }
}
