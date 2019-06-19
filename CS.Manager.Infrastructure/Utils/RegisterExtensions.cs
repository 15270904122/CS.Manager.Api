using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CS.Manager.Infrastructure.Utils
{
    public static class RegisterExtensions
    {
        /// <summary>
        /// 批量注册服务
        /// </summary>
        /// <param name="services">DI服务</param>
        /// <param name="assemblys">需要批量注册的程序集集合</param>
        /// <param name="baseType">基础类/接口</param>
        /// <param name="serviceLifetime">服务生命周期</param>
        /// <returns></returns>
        //public static IServiceCollection BatchRegisterService(this IServiceCollection services, Assembly[] assemblys, Type baseType)
        //{
        //    List<Type> typeList = new List<Type>();  //所有符合注册条件的类集合
        //    foreach (var assembly in assemblys)
        //    {
        //        //筛选当前程序集下符合条件的类
        //        var types = assembly.GetTypes().Where(t => !t.IsInterface && !t.IsSealed && !t.IsAbstract && baseType.IsAssignableFrom(t));
        //        if (types != null && types.Count() > 0)
        //            typeList.AddRange(types);
        //    }
        //    if (typeList.Count() == 0)
        //        return services;

        //    var typeDic = new Dictionary<Type, Type[]>(); //待注册集合
        //    foreach (var type in typeList)
        //    {
        //        var interfaces = type.GetInterfaces();   //获取接口
        //        typeDic.Add(type, interfaces);
        //    }
        //    if (typeDic.Keys.Count() > 0)
        //    {
        //        foreach (var instanceType in typeDic.Keys)
        //        {
        //            foreach (var interfaceType in typeDic[instanceType])
        //            {
        //                //根据指定的生命周期进行注册
        //                switch (serviceLifetime)
        //                {
        //                    case ServiceLifetime.Scoped:
        //                        services.AddScoped(interfaceType, instanceType);
        //                        break;
        //                    case ServiceLifetime.Singleton:
        //                        services.AddSingleton(interfaceType, instanceType);
        //                        break;
        //                    case ServiceLifetime.Transient:
        //                        services.AddTransient(interfaceType, instanceType);
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //    return services;
        //}
    }
}
