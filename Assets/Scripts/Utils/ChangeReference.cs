namespace BloodWork.Utils
{
    /// <summary>
    /// Use it to keep track of how a variable's value has changed.
    /// </summary>
    /// <typeparam name="Type">Variable's type</typeparam>
    public class ChangeReference<Type>
    {
        public Type OldValue  { get; }
        public Type NewValue  { get; }
        public bool IsChanged { get; }

        /// <summary>
        /// Assigns newValue to the variableReference.
        /// </summary>
        /// <param name="variableReference">Variable Reference</param>
        /// <param name="newValue">New Value</param>
        public ChangeReference(ref Type variableReference, in Type newValue)
        {
            IsChanged = !variableReference.Equals(newValue);

            if (!IsChanged)
                return;

            OldValue = variableReference;
            NewValue = newValue;

            variableReference = newValue;
        }

        /// <summary>
        /// Returns boolean value whether variable reference was changed, and it was changed to the target value.
        /// </summary>
        /// <param name="targetValue">Target Value</param>
        /// <returns>true if reference was changed to target value, false otherwise</returns>
        public bool IsChangedTo(Type targetValue)
        {
            return IsChanged && NewValue.Equals(targetValue);
        }

        /// <summary>
        /// Returns boolean value whether variable reference was changed, and it was changed from the target value.
        /// </summary>
        /// <param name="targetValue">Target Value</param>
        /// <returns>true if reference was changed from target value, false otherwise</returns>
        public bool IsChangedFrom(Type targetValue)
        {
            return IsChanged && OldValue.Equals(targetValue);
        }
    }

    /// <summary>
    /// Use static methods to keep track of how a variable's value has changed. 
    /// </summary>
    public static class ChangeReference
    {
        /// <summary>
        /// Static method of instantiating ChangeReference object.
        /// </summary>
        /// <param name="variableReference">Variable Reference</param>
        /// <param name="newValue">New Value</param>
        /// <returns>Instance of ChangeReference</returns>
        public static ChangeReference<Type> Of<Type>(ref Type variableReference, in Type newValue)
        {
            return new ChangeReference<Type>(ref variableReference, newValue);
        }

        /// <summary>
        /// Shorthand of instantiating ChangeReference and calling IsChanged method. 
        /// </summary>
        /// <param name="variableReference">Variable Reference</param>
        /// <param name="newValue">New Value</param>
        /// <returns>true if reference was changed, false otherwise</returns>
        public static bool IsChanged<Type>(ref Type variableReference, in Type newValue)
        {
            return Of(ref variableReference, newValue).IsChanged;
        }

        /// <summary>
        /// Shorthand of instantiating ChangeReference and calling IsChangedTo method. 
        /// </summary>
        /// <param name="variableReference">Variable Reference</param>
        /// <param name="newValue">New Value</param>
        /// <param name="targetValue">Target Value</param>
        /// <returns>true if reference was changed to target value, false otherwise</returns>
        public static bool IsChangedTo<Type>(ref Type variableReference, in Type newValue, Type targetValue)
        {
            return Of(ref variableReference, newValue).IsChangedTo(targetValue);
        }

        /// <summary>
        /// Shorthand of instantiating ChangeReference and calling IsChangedFrom method. 
        /// </summary>
        /// <param name="variableReference">Variable Reference</param>
        /// <param name="newValue">New Value</param>
        /// <param name="targetValue">Target Value</param>
        /// <returns>true if reference was changed from target value, false otherwise</returns>
        public static bool IsChangedFrom<Type>(ref Type variableReference, in Type newValue, Type targetValue)
        {
            return Of(ref variableReference, newValue).IsChangedFrom(targetValue);
        }
    }
}
