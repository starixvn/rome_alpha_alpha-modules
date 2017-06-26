namespace Alpha.Announcements.Services
{
    using Models;
    using Specs;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public interface SpecificationFactory
    {
        IEnumerable<ISpecification<IUser>> LoadSpecifications();
    }

    public class SpecificationFactoryImpl : SpecificationFactory
    {
        private static readonly object LockObject = new object();
        private static IList<Type> specificationTypes;
        private static IList<ISpecification<IUser>> specifications = new List<ISpecification<IUser>>();

        public IEnumerable<ISpecification<IUser>> LoadSpecifications()
        {
            var specTypes = GetSpecificationTypes();

            if (!specifications.Any())
            {
                lock (LockObject)
                {
                    if (!specifications.Any())
                    {
                        AddSpecs(specTypes);
                    }
                }
            }

            return specifications;
        }

        private static void AddSpecs(IList<Type> specTypes)
        {
            foreach (var specType in specTypes)
            {
                var specification = Activator.CreateInstance(specType) as ISpecification<IUser>;
                if (specification != null
                    && specification.Combinable)
                {
                    specifications.Add(specification);
                }
            }
        }

        private static IList<Type> GetSpecificationTypes()
        {
            if (specificationTypes == null || !specificationTypes.Any())
            {
                var specType = typeof(ISpecification<IUser>);

                specificationTypes = GetRefferencedAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(type => specType.IsAssignableFrom(type) && !type.IsAbstract)
                    .ToList();
            }

            return specificationTypes;
        }

        private static IList<Assembly> GetRefferencedAssemblies()
        {
            var assemblies = new List<Assembly>();

            string[] files = Directory.GetFiles(
                AppDomain.CurrentDomain.BaseDirectory,
                "Alpha.Announcements*.dll",
                SearchOption.AllDirectories);

            foreach (var file in files)
            {
                AssemblyName assemblyName = AssemblyName.GetAssemblyName(file);

                assemblies.Add(Assembly.Load(assemblyName));
            }

            return assemblies;
        }
    }
}