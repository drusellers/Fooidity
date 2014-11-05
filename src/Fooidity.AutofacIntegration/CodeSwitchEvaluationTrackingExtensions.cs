﻿namespace Fooidity
{
    using System;
    using System.Collections.Generic;
    using Autofac;
    using Contracts;


    public static class CodeSwitchEvaluationTrackingExtensions
    {
        public static void EnableCodeSwitchTracking(this ContainerBuilder builder)
        {
            builder.RegisterType<CodeSwitchEvaluationObserver>()
                .As<IObserver<ICodeSwitchEvaluated>>()
                .As<ICodeSwitchesEvaluated>()
                .InstancePerLifetimeScope();
        }

        public static IEnumerable<ICodeSwitchEvaluated> GetEvaluatedCodeSwitches(this ILifetimeScope scope)
        {
            ICodeSwitchesEvaluated result;
            if (scope.TryResolve(out result))
                return result;

            throw new FooidityException(
                "Code switch tracking is not enabled. Enable it while building the container using EnableCodeSwitchTracking");
        }
    }
}