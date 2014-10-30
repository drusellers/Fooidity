namespace Fooidity.ContainerTests.Windsor
{
    using Castle.Windsor;
    using NUnit.Framework;
    using Shouldly;


    [TestFixture]
    public class A_default_disabled_foodid
    {
        [Test]
        public void Should_return_as_default()
        {
            var container = new WindsorContainer();

            container.DisableCodeSwitchesByDefault();

            var fooId = container.Resolve<CodeSwitch<Active>>();

            fooId.Enabled.ShouldBe(false);
        }

        [Test]
        public void Should_return_as_specific()
        {
            var container = new WindsorContainer();

            container.DisableCodeSwitchesByDefault();

            container.RegisterEnabled<Active>();

            var fooId = container.Resolve<CodeSwitch<Active>>();

            fooId.Enabled.ShouldBe(true);
        }

        [Test]
        [Ignore("Enable/Disable not yet supported")]
        public void Should_convert_to_specific()
        {
            var container = new WindsorContainer();

            container.DisableCodeSwitchesByDefault();

            var fooId = container.Resolve<CodeSwitch<Active>>();

            Assert.IsFalse(fooId.Enabled);

            container.Enable<Active>();

            fooId = container.Resolve<CodeSwitch<Active>>();

            fooId.Enabled.ShouldBe(true);
        }


        struct Active :
            CodeFeature
        {
        }
    }
}