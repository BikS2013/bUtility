using SimpleInjector;
using SimpleInjector.Advanced;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.SimpleInjector
{
    /// <summary>
    /// example implementation from http://simpleinjector.readthedocs.io/en/latest/extensibility.html
    /// 
    /// Changes the constructor resolution behavior to always select the constructor 
    /// with the most parameters from the list of constructors with only resolvable parameters
    /// 
    /// </summary>
    public class MostResolvableConstructorBehavior: IConstructorResolutionBehavior
    {
        private readonly Container container;

        public MostResolvableConstructorBehavior(Container container)
        {
            this.container = container;
        }

        private bool IsCalledDuringRegistrationPhase
        {
            [DebuggerStepThrough]
            get { return !this.container.IsLocked(); }
        }

        [DebuggerStepThrough]
        public ConstructorInfo GetConstructor(Type service, Type implementation)
        {
            var constructors = implementation.GetConstructors();
            if (constructors.Length == 1 || IsCalledDuringRegistrationPhase) return constructors.FirstOrDefault();

            return constructors.Select(ctor => new { parameters = ctor.GetParameters(), ctor = ctor })
                .Where(i => i.parameters.All(p => this.CanBeResolved(p, service, implementation)))
                .OrderByDescending(i => i.parameters.Length).First().ctor;

            //return (
            //    from ctor in constructors
            //    let parameters = ctor.GetParameters()
            //    where this.IsCalledDuringRegistrationPhase
            //        || constructors.Length == 1
            //        || parameters.All(p => this.CanBeResolved(p, service, implementation))
            //    orderby parameters.Length descending
            //    select ctor)
            //    .First();
        }

        [DebuggerStepThrough]
        private bool CanBeResolved(ParameterInfo p, Type service, Type implementation)
        {
            return this.container.GetRegistration(p.ParameterType) != null ||
                this.CanBuildType(p, service, implementation);
        }

        [DebuggerStepThrough]
        private bool CanBuildType(ParameterInfo p, Type service, Type implementation)
        {
            try
            {
                this.container.Options.DependencyInjectionBehavior.BuildExpression(
                    new InjectionConsumerInfo(service, implementation, p));
                return true;
            }
            catch (ActivationException)
            {
                return false;
            }
        }
    }
}
