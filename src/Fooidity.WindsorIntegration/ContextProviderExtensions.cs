﻿namespace Fooidity
{
    using System;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;


    public static class ContextProviderExtensions
    {
        /// <summary>
        /// Register a default context provider that returns false for any attempts to 
        /// access the context type.
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterDefaultContextProvider(this IWindsorContainer builder)
        {
            builder.Register(Component.For(typeof(ContextProvider<,>)).ImplementedBy(typeof(DefaultContextProvider<,>)));
        }

        /// <summary>
        /// Register a context provider for the specified input type and context type
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="selector"></param>
        public static void RegisterContextProvider<TInput, TContext>(this IWindsorContainer builder, Func<TInput, TContext?> selector)
            where TContext : struct
        {
            builder.Register(Component.For<ContextProvider<TInput, TContext>>()
                .UsingFactoryMethod(_ => new ValueTypeContextProvider<TInput, TContext>(selector)));
        }

        /// <summary>
        /// Register a context provider for the specified input type and context type
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="builder"></param>
        /// <param name="selector"></param>
        public static void RegisterContextProvider<TInput, TContext>(this IWindsorContainer builder, Func<TInput, TContext> selector)
            where TContext : class
        {
            builder.Register(Component.For<ContextProvider<TInput, TContext>>()
                .UsingFactoryMethod(_=>new ObjectContextProvider<TInput,TContext>(selector)));
        }
    }
}