namespace Fooidity.ContainerTests.Windsor
{
    using System;
    using System.Linq;
    using Features;
    using NUnit.Framework;

    namespace ExceptionLogging_Specs
    {
        using Castle.MicroKernel.Lifestyle;
        using Castle.MicroKernel.Registration;
        using Castle.Windsor;
        using Contracts;


        interface ILogger
        {
            void LogError(Exception exception);
        }


        class Logger :
            ILogger
        {
            readonly ICodeSwitchesEvaluated _evaluated;

            public Logger(ICodeSwitchesEvaluated evaluated)
            {
                _evaluated = evaluated;
            }

            public void LogError(Exception exception)
            {
                CodeSwitchEvaluated[] switches = _evaluated.ToArray();

                Console.WriteLine("{0}\n{1}", exception.Message, string.Join(Environment.NewLine, switches.Select(x => x.CodeFeatureId)));
            }
        }


        interface IClass
        {
            void MakeHappy();
        }


        class TestClass :
            IClass
        {
            readonly ILogger _logger;
            readonly CodeSwitch<UseNewCodePath> _useNewCodePath;

            public TestClass(ILogger logger, CodeSwitch<UseNewCodePath> useNewCodePath)
            {
                _logger = logger;
                _useNewCodePath = useNewCodePath;
            }

            public void MakeHappy()
            {
                try
                {
                    if(!_useNewCodePath.Enabled)
                        throw new InvalidOperationException("Code path not enabled!");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex);
                }
            }
        }

        class TestClassV2 :
            IClass
        {
            readonly ILogger _logger;
            readonly CodeSwitch<UseNewCodePath> _useNewCodePath;

            public TestClassV2(ILogger logger, CodeSwitch<UseNewCodePath> useNewCodePath)
            {
                _logger = logger;
                _useNewCodePath = useNewCodePath;
            }

            public void MakeHappy()
            {
                try
                {
                    if(!_useNewCodePath.Enabled)
                        throw new InvalidOperationException("Code path not enabled from V2!");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex);
                }
            }
        }


        [TestFixture]
        public class When_an_exception_occurs_within_a_switched_code_path
        {
            [Test]
            public void Should_capture_the_switch_states()
            {
                using (var scope = _container.BeginScope())
                {
                    var testClass = _container.Resolve<IClass>();

                    testClass.MakeHappy();
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
                _container = new WindsorContainer();

                _container.RegisterToggle<UseNewCodePath>();
                _container.RegisterToggle<UseNewClass>();
                _container.EnableCodeSwitchTracking();
                _container.RegisterSwitchedType<UseNewClass, IClass, TestClassV2, TestClass>();

                _container.Register(
                    Component.For<ILogger>().ImplementedBy<Logger>()
                    );
            }
        }
    }
}