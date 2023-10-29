using System;
using System.Runtime.CompilerServices;

namespace Bro.Client.Context
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequireElementInContextAttribute : System.Attribute
    {
        public readonly string OwnerClassName;
        public readonly Type RequiredType;
        public RequireElementInContextAttribute( Type requiredType, [CallerMemberName] string ownerClassName = "undefined")
        {
            OwnerClassName = ownerClassName;
            RequiredType = requiredType;
        }
    }
}