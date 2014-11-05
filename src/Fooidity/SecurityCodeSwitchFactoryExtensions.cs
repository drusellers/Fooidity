﻿namespace Fooidity
{
    using CodeSwitches;


    public static class SecurityCodeSwitchFactoryExtensions
    {
        /// <summary>
        /// Enables the feature if the principal in is in the specified role
        /// </summary>
        /// <typeparam name="TFeature"></typeparam>
        /// <param name="factory"></param>
        /// <param name="role">The role required to enable the switch</param>
        /// <returns></returns>
        public static ICodeSwitch<TFeature> EnabledForPrincipalInRole<TFeature>(this ICodeSwitchFactory factory,
            string role)
            where TFeature : struct, ICodeFeature
        {
            return new EnabledForThreadPrincipalInRoleCodeSwitch<TFeature>(role);
        }

        /// <summary>
        /// Enabled if the current principal identity is authenticated
        /// </summary>
        /// <typeparam name="TFeature">The code feature</typeparam>
        /// <returns></returns>
        public static ICodeSwitch<TFeature> EnabledForAuthenticatedIdentity<TFeature>(this ICodeSwitchFactory factory)
            where TFeature : struct, ICodeFeature
        {
            return new EnabledForAuthenticatedIdentityCodeSwitch<TFeature>();
        }
    }
}