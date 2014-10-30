namespace Fooidity.ContainerTests.Windsor
{
    using System;
    using System.Collections.Generic;
    using AutofacIntegration;
    using Castle.MicroKernel.Lifestyle;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Contexts;
    using Events;
    using Features;
    using NUnit.Framework;


    [TestFixture]
    public class Configuring_the_container_for_user_contexts
    {
        [Test]
        [Ignore("Need to research this for Windsor")]
        public void No_context_should_throw_the_proper_exception()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                using (var scope = _container.BeginScope())
                {
                    var codeSwitch = _container.Resolve<CodeSwitch<UseNewCodePath>>();
                }
            });

            Assert.IsInstanceOf<ContextSwitchException>(exception);
        }

        [Test]
        [Ignore("Need to research this for Windsor")]
        public void Should_be_enabled_for_specified_user()
        {
            //using (var scope =_container.BeginLifetimeScope(x => x.RegisterInstance(new UserContext {Name = "Chris"})))
            using (var scope =_container.BeginScope())
            {
                var codeSwitch = _container.Resolve<CodeSwitch<UseNewCodePath>>();

                Assert.IsTrue(codeSwitch.Enabled);

                var repository = _container.Resolve<Repository>();

                Assert.AreEqual("No", repository.IsDbEnabled);

                IEnumerable<CodeSwitchEvaluated> codeSwitchesEvaluated = _container.GetCodeSwitchesEvaluated();

                foreach (CodeSwitchEvaluated evaluated in codeSwitchesEvaluated)
                    Console.WriteLine("{0}: {1}", evaluated.Id, evaluated.Enabled);
            }
        }

        [Test]
        [Ignore("Need to research this for Windsor")]
        public void Should_use_the_default_off_value()
        {
            //using (var scope = _container.BeginS(x => x.RegisterInstance(new UserContext {Name = "David"})))
            using (var scope = _container.BeginScope())
            {
                var codeSwitch = _container.Resolve<CodeSwitch<UseNewCodePath>>();

                Assert.IsFalse(codeSwitch.Enabled);

                IEnumerable<CodeSwitchEvaluated> codeSwitchesEvaluated = _container.GetCodeSwitchesEvaluated();

                foreach (CodeSwitchEvaluated evaluated in codeSwitchesEvaluated)
                    Console.WriteLine("{0}: {1}", evaluated.Id, evaluated.Enabled);
            }
        }

        IWindsorContainer _container;

        [TestFixtureTearDown]
        public void Teardown()
        {
            _container.Dispose();
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            var _container = new WindsorContainer();

            _container.Install(new ConfigurationCodeFeatureCacheInstaller());
            _container.Install(new ConfigurationContextFeatureCacheInstaller<UserContext, UserContextKeyProvider>());

            _container.RegisterCodeSwitch<DbEnabled>();
            _container.RegisterContextSwitch<UseNewCodePath, UserContext>();

            _container.EnableCodeSwitchTracking();

            _container.Register(Component.For<Repository>());
        }


        class Repository
        {
            readonly CodeSwitch<DbEnabled> _dbEnabled;

            public Repository(CodeSwitch<DbEnabled> dbEnabled)
            {
                _dbEnabled = dbEnabled;
            }

            public string IsDbEnabled
            {
                get { return _dbEnabled.Enabled ? "Yes" : "No"; }
            }
        }
    }
}