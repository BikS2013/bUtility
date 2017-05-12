using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace bUtility.SimpleInjector.Test
{
    [TestClass]
    public class MostResolvableConstructorBehaviorTest
    {
        [TestMethod]
        public void Step1()
        {
            var container = new Container();

            container.Register<IDependency1, Dependency1>();
            container.Register<IService1, Service1>();

            container.Verify(VerificationOption.VerifyAndDiagnose);
        }

        IDependency1 GetDep1()
        {
            return new Dependency1();
        }

        [TestMethod]
        public void Step2()
        {
            var container = new Container();

            container.Register<IDependency1>( ()=> new Dependency1() );
            container.Register<IService1, Service1>();

            container.Verify(VerificationOption.VerifyAndDiagnose);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Step3()
        {
            var container = new Container();

            container.Register<IDependency1>(() => null );
            container.Register<IService1, Service1>();

            container.Verify(VerificationOption.VerifyAndDiagnose);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Step4()
        {
            var container = new Container();

            container.Register<IDependency1>(() => new Dependency1());
            container.Register<IService1, Service1a>();

            container.Verify(VerificationOption.VerifyAndDiagnose);
        }
        [TestMethod]
        public void Step5()
        {
            var container = new Container();
            container.Options.ConstructorResolutionBehavior = new MostResolvableConstructorBehavior(container);

            container.Register<IDependency1>(() => new Dependency1());
            container.Register<IService1, Service1a>();
            container.Verify(VerificationOption.VerifyAndDiagnose);
        }
        [TestMethod]
        public void Step6()
        {
            var container = new Container();
            container.Options.ConstructorResolutionBehavior = new MostResolvableConstructorBehavior(container);

            container.Register<IService1, Service1a>();
            container.Verify(VerificationOption.VerifyAndDiagnose);
        }

        [TestMethod]
        public void Step7()
        {
            var container = new Container();
            container.Options.ConstructorResolutionBehavior = new MostResolvableConstructorBehavior(container);

            container.Register<IDependency1>(() => new Dependency1());
            container.Register<IDependency2>(() => new Dependency2());
            container.Register<IService1, Service1a>();
            container.Verify(VerificationOption.VerifyAndDiagnose);

        }

        [TestMethod]
        public void Step8()
        {
            var container = new Container();
            container.Options.ConstructorResolutionBehavior = new MostResolvableConstructorBehavior(container);

            container.Register<IDependency1>(() => new Dependency1());
            container.Register<IDependency2>(() => new Dependency2());
            container.Register<IDependency3>(() => new Dependency3());
            container.Register<IService1, Service1a>();
            container.Verify(VerificationOption.VerifyAndDiagnose);
        }
    }
}
