using Application.MainModule;
using Application.MainModule.Interface;
using Application.MainModule.RestSharp;

using Domain.MainModule.Entity;
using Domain.MainModule.IRepository;
using Domain.MainModule.IUnitOfWork;
using FluentValidation;
using Infrastructure.Data.MainModule.Repository;
using Infrastructure.Data.MainModule.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IoC;

public static class IocContainer
{
    public static IServiceCollection AddDependencyInjectionInterfaces(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IRestWsHelper, RestWsHelper>();

        services.AddDependencyInjectionAppService();
        services.AddDependencyInjectionRepository();
        services.AddDependencyInjectionValidationsDto();

        return services;
    }

    private static void AddDependencyInjectionAppService(this IServiceCollection services)
    {
        services.AddScoped<IPostulantAppService, PostulantAppService>();
        services.AddScoped<IChoreAppService, ChoreAppService>();
    }

    private static void AddDependencyInjectionRepository(this IServiceCollection services)
    {
        services.AddScoped<IPostulantRepository, PostulantRepository>();
        services.AddScoped<IChoreRepository, ChoreRepository>();
    }

    private static void AddDependencyInjectionValidationsDto(this IServiceCollection services)
    {
        
    }
}