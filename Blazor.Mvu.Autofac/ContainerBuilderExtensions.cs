namespace Blazor.Mvu.Autofac;

using System.Reflection;
using global::Autofac;

public static class ContainerBuilderExtensions
{
    public static void AddBlazorMvu(this ContainerBuilder self, params Assembly[] assemblies)
    {
        self.RegisterAssemblyTypes(assemblies)
            .AssignableTo<ISerializer>()
            .AsImplementedInterfaces()
            .AsSelf()
            .SingleInstance();
        self.RegisterAssemblyTypes(assemblies)
            .AsClosedTypesOf(typeof(IApplicationStateHolder<>))
            .AsImplementedInterfaces()
            .SingleInstance();

        self.RegisterAssemblyOpenGenericTypes(assemblies)
            .AssignableTo(typeof(IUpdater<>))
            .AsImplementedInterfaces()
            .InstancePerDependency();
        self.RegisterAssemblyTypes(assemblies)
            .AsClosedTypesOf(typeof(IUpdater<>))
            .AsImplementedInterfaces()
            .InstancePerDependency();
        self.RegisterInstance(new LoggerSettings()).AsSelf();
        self.RegisterType<Logger>().AsSelf().InstancePerDependency();
    }
}