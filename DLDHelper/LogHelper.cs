using PostSharp.Aspects;
using PostSharp.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLDHelper
{
    [Serializable]
    public sealed class LogHelperAttribute : OnMethodBoundaryAspect
    {
        public LogHelperAttribute(Target target) { m_Target = target; }
        public Target m_Target { get; set; }

        public enum Target
        {
            Log,
            Console
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            string message = string.Format("Leaving: {0}.{1}", args.Method.DeclaringType.Name, args.Method.Name);
            if(m_Target.HasFlag(Target.Log))
            {
                AppLog.Log(message);
            }
            if (m_Target.HasFlag(Target.Console))
            {
                System.Console.WriteLine(message);
            }       
        }
    }
}
