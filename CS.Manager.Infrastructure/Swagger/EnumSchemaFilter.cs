using AutoMapper;
using CS.Manager.Infrastructure.Utils;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CS.Manager.Infrastructure.Swagger
{
    /// <summary>
    /// 枚举参数示例
    /// </summary>
    public class EnumSchemaFilter : ISchemaFilter
    {
        /// <inheritdoc />
        public void Apply(Schema model, SchemaFilterContext context)
        {
            var enumType = context.SystemType.GetNonNullableType();
            if (enumType.IsEnum)
            {
                var enums = enumType.MapTo<IEnumerable<DisplayItem>>();
                //Mapper.Map<Type, IEnumerable<DisplayItem>>(enumType);

                model.Extensions["Enums"] = JObject.FromObject(enums.ToDictionary(p => p.Name, p => p.Id));
            }
        }
    }
}
