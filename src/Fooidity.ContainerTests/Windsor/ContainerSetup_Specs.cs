namespace Fooidity.ContainerTests.Windsor
{
    using Autofac;
    using Castle.Windsor;
    using NUnit.Framework;


    [TestFixture]
    public class Setting_up_a_container
    {
        [Test]
        public void Should_return_new_registration_by_type()
        {
            var container = new WindsorContainer();

            container.RegisterEnabled<UseClassAv2>();

            container.RegisterSwitchedType<UseClassAv2, A, ClassA_V2, ClassA>();

            var a = container.Resolve<A>();

            Assert.IsInstanceOf<ClassA_V2>(a);
        }

        [Test]
        public void Should_return_original_registration_by_type()
        {
            var container = new WindsorContainer();

            container.RegisterDisabled<UseClassAv2>();

            container.RegisterSwitchedType<UseClassAv2, A, ClassA_V2, ClassA>();

            var a = container.Resolve<A>();

            Assert.IsInstanceOf<ClassA>(a);
        }

        [Test]
        public void Should_support_delegate_registration()
        {
            var container = new WindsorContainer();

            container.RegisterEnabled<UseClassAv2>();

            container.RegisterSwitched<UseClassAv2, A>(context => new ClassA_V2(), context => new ClassA());

            var a = container.Resolve<A>();

            Assert.IsInstanceOf<ClassA_V2>(a);
        }


        interface A
        {
        }


        class ClassA :
            A
        {
        }


        struct UseClassAv2 :
            CodeFeature
        {
        }


        class ClassA_V2 :
            A
        {
        }
    }
}