namespace Fooidity.ContainerTests.Windsor
{
    using System;
    using System.Linq;
    using Autofac;
    using Castle.Core;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Context;
    using Castle.MicroKernel.Lifestyle;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Features;
    using NUnit.Framework;
    using Shouldly;
    using Subjects;

    public class A_conditional_class_dependency
    {
        [Test]
        public void Should_use_the_new_methods()
        {
            var container = new WindsorContainer();

            container.RegisterEnabled<UseNewMethod>();

            container.Register(
                Component.For<ConditionalClass>()
                );

            var conditionalClass = container.Resolve<ConditionalClass>();

            Assert.AreEqual("V2: 42, Test", conditionalClass.FunctionCall(42, "Test"));
        }

        [Test]
        public void Should_use_the_old_methods()
        {
            var container = new WindsorContainer();

            container.RegisterDisabled<UseNewMethod>();

            container.Register(
                Component.For<ConditionalClass>()
                );

            var conditionalClass = container.Resolve<ConditionalClass>();

            Assert.AreEqual("Old: 42, Test", conditionalClass.FunctionCall(42, "Test"));
        }

        [Test]
        public void Should_use_the_old_methods_by_default()
        {
            var container = new WindsorContainer();

            container.DisableCodeSwitchesByDefault();

            container.Register(
                Component.For<ConditionalClass>()
                );

            var conditionalClass = container.Resolve<ConditionalClass>();

            Assert.AreEqual("Old: 42, Test", conditionalClass.FunctionCall(42, "Test"));
        }

        [Test]
        public void Should_use_the_new_methods_by_default()
        {
            var container = new WindsorContainer();

            container.EnableCodeSwitchesByDefault();

            container.Register(
                Component.For<ConditionalClass>()
                );

            var conditionalClass = container.Resolve<ConditionalClass>();

            Assert.AreEqual("V2: 42, Test", conditionalClass.FunctionCall(42, "Test"));
        }

        [Test]
        [Ignore("Need to solve this one.")]
        //[ExpectedException(typeof(NotSupportedException))]
        public void Should_be_able_to_enable_a_fooid_after_creation()
        {
            var container = new WindsorContainer();

            container.RegisterDisabled<UseNewMethod>();

            container.Register(
                Component.For<ConditionalClass>()
                );


            var conditionalClass = container.Resolve<ConditionalClass>();

            Assert.AreEqual("Old: 42, Test", conditionalClass.FunctionCall(42, "Test"));

            container.Enable<UseNewMethod>();

            conditionalClass = container.Resolve<ConditionalClass>();

            Assert.AreEqual("V2: 42, Test", conditionalClass.FunctionCall(42, "Test"));

        }

        [Test]
        public void Should_be_able_to_toggle_a_switch()
        {
            var container = new WindsorContainer();

            container.RegisterToggle<UseNewMethod>();
            container.EnableCodeSwitchTracking();

            using (var scope = container.BeginScope())
            {
                container.Resolve<CodeSwitch<UseNewMethod>>().Enabled.ShouldBe(false);

                container.GetCodeSwitchesEvaluated().Count().ShouldBe(1);
            }

            using (var scope = container.BeginScope())
            {
                container.Resolve<IToggleCodeSwitch<UseNewMethod>>().Enable();

                var x = container.Resolve<CodeSwitch<UseNewMethod>>();
                x.Enabled.ShouldBe(true);

                container.GetCodeSwitchesEvaluated().Count().ShouldBe(1);
            }

        }
    }
}