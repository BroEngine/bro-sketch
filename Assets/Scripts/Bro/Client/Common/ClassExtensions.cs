namespace Bro.Client
{
    public static class ClassExtensions
    {
        public static T GetAttributeOfType<T>(this object targetClass) where T:System.Attribute
        {
            var type = targetClass.GetType();
            var memInfo = type.GetMember(targetClass.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }
}