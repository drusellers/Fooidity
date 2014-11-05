namespace Fooidity.CodeSwitches
{
    using System;
    using System.Security.Principal;
    using System.Threading;
    using Contracts;


    public class EnabledForAuthenticatedIdentityCodeSwitch<TFeature> :
        ICodeSwitch<TFeature>
        where TFeature : struct, ICodeFeature
    {
        readonly Lazy<bool> _enabled;
        readonly CodeSwitchEvaluatedObservable<TFeature> _evaluated;

        public EnabledForAuthenticatedIdentityCodeSwitch()
        {
            _evaluated = new CodeSwitchEvaluatedObservable<TFeature>();
            _enabled = new Lazy<bool>(Evaluate);
        }

        public bool Enabled
        {
            get { return _enabled.Value; }
        }

        public IDisposable Subscribe(IObserver<ICodeSwitchEvaluated> observer)
        {
            return _evaluated.Connect(observer);
        }

        static bool GetEnabled()
        {
            IPrincipal principal = Thread.CurrentPrincipal;
            if (principal == null)
                return false;

            return principal.Identity.IsAuthenticated;
        }

        bool Evaluate()
        {
            bool enabled = GetEnabled();

            _evaluated.Evaluated(enabled);

            return enabled;
        }
    }
}