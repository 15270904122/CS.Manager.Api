using AutoMapper;
using CS.Manager.Infrastructure.Page;
using CS.Manager.Infrastructure.Swagger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace CS.Manager.Infrastructure.Utils
{
    public class AutoMapperHelper
    {
        /// <summary>
        /// 创建 Dto 对象映射
        /// </summary>
        public static void CreateDtoMappings(IMapperConfigurationExpression configuration)
        {
            //configuration.EnableNullPropagationForQueryMapping = false;
            //configuration.AllowNullDestinationValues = false;
            configuration.ForAllMaps((map, c) =>
            {
                foreach (var property in map.DestinationType.GetProperties())
                {
                    var attr = property.GetAttribute<MapFromAttribute>();
                    if (attr?.PropertyPath?.Length > 0)
                    {
                        c.ForMember(property.Name, m =>
                        {
                            var path = string.Join(".", attr.PropertyPath);
                            var exp = DynamicExpressionParser.ParseLambda(map.SourceType, null, path);

                            var m0 = m.GetType()
                                .GetProperty("PropertyMapActions", BindingFlags.Instance | BindingFlags.NonPublic)?
                                .GetValue(m) as List<Action<PropertyMap>>;
                            var m1 = typeof(PropertyMap).GetMethod(nameof(PropertyMap.MapFrom), new Type[] { typeof(LambdaExpression) });
                            m0?.Add(t => m1.Invoke(t, new object[] { exp }));
                        });
                    }
                }
            });

            configuration.CreateMap<Enum, DisplayItem>()
                .ConvertUsing(p => new DisplayItem
                {
                    Id = p,
                    Name = p.DisplayName(),
                    Value = p.ToString("G"),
                    ShortName = p.DisplayShortName(),
                    Description = p.DisplayDescription(),
                    GroupName = p.DisplayGroupName(),
                    Order = p.DisplayOrder(),
                    Type = p.DisplayPrompt(),
                });
            configuration.CreateMap<Type, IEnumerable<DisplayItem>>()
                .ConvertUsing(p => Enum.GetValues(p).Cast<Enum>().Select(t => t.MapTo<DisplayItem>()));

            configuration.CreateMap<string, JToken>()
                .ConvertUsing(p => JsonConvert.DeserializeObject<JToken>(p ?? string.Empty));
            configuration.CreateMap<string, JObject>()
                .ConvertUsing(p => JsonConvert.DeserializeObject<JObject>(p ?? string.Empty));
            configuration.CreateMap<string, JArray>()
                .ConvertUsing(p => JsonConvert.DeserializeObject<JArray>(p ?? string.Empty));


            configuration.CreateMap<Result.Result, PageInfo>()
                .ForMember(p => p.PageIndex, m => m.MapFrom(p => p.Code));

        }
    }
}
