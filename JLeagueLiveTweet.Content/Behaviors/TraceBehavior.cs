using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace MinatoProject.Apps.JLeagueLiveTweet.Content.Behaviors
{
    /// <summary>
    /// 
    /// </summary>
    public class TraceBehavior : IInterceptionBehavior
    {
        /// <summary>
        /// 
        /// </summary>
        public bool WillExecute => true;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Enumerable.Empty<Type>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="getNext"></param>
        /// <returns></returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var logger = LogManager.GetLogger(input.MethodBase.DeclaringType.FullName);
            logger.Info($"▼ {input.MethodBase.DeclaringType.Name}.{input.MethodBase.Name}");
            var result = getNext()(input, getNext);
            logger.Info($"▲ {input.MethodBase.DeclaringType.Name}.{input.MethodBase.Name}");
            return result;
        }
    }
}
