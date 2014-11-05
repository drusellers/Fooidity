namespace Fooidity.CodeSwitches
{
    using System;
    using System.Security.Principal;
    using System.Threading;
    using Contracts;


    public class EnabledForThreadPrincipalInRoleCodeSwitch<TFeature> :
        ICodeSwitch<TFeature>
        where TFeature : struct, ICodeFeature
    {
        readonly Lazy<bool> _enabled;
        readonly CodeSwitchEvaluatedObservable<TFeature> _evaluated;
        readonly string _role;

        public EnabledForThreadPrincipalInRoleCodeSwitch(string role)
        {
            _role = role;
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

        bool GetEnabled()
        {
            IPrincipal principal = Thread.CurrentPrincipal;
            if (principal == null)
                return false;

            return principal.IsInRole(_role);
        }

        bool Evaluate()
        {
            bool enabled = GetEnabled();

            _evaluated.Evaluated(enabled);

            return enabled;
        }
    }
}