namespace Unleash.Strategies
{
    using System.Collections.Generic;
    using System.Linq;
    using Unleash.Internal;

    public class PhoneNumbertrategy : IStrategy
    {
        private static readonly string StrategyName = "appSetting_PhoneNumber";

        internal static readonly string PARAM = "PhoneNumbers";

        public string Name => StrategyName;

        public string GetPhoneNumbers(Dictionary<string, string> parameters, UnleashContext context = null)
        {

            if (parameters.TryGetValue(PARAM, out var phoneNumber))
            {
                return phoneNumber;
            }

            return string.Empty;
        }

        public bool IsEnabled(Dictionary<string, string> parameters, UnleashContext context = null)
        {
            return false;
        }

        public bool IsEnabled(Dictionary<string, string> parameters, UnleashContext context, IEnumerable<Constraint> constraints)
        {
            //return StrategyUtils.IsEnabled(this, parameters, context, constraints);
            return false;
        }
    }
}