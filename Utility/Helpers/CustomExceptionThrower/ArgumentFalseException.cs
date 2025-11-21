using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.CustomExceptionThrower
{
    public class ArgumentFalseException: ArgumentException
    {
        

        public ArgumentFalseException(string? paramName)
            : base( paramName)
        {
           
        }

        

        

        public static void ThrowIfFalse(bool argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (!argument)
            {
                Throw(paramName);
            }
        }
        [DoesNotReturn]
        internal static void Throw(string? paramName) =>
            throw new ArgumentFalseException(paramName);
    }
}
