namespace BloodWork.Commons
{
    public class Utils
    {
        // Assigns newValue to the variable reference and returns a boolean
        // whether the variable reference has changed
        public static bool IsChanged<Type>(ref Type variable, in Type newValue)
        {
            bool changed = !variable.Equals(newValue);
            
            variable = newValue;

            return changed;
        }

        // Assigns newValue to the variable reference and returns a boolean
        // whether the variable reference has changed and the changed value
        // is equal to expectedValue
        public static bool IsChangedTo<Type>(ref Type variable, in Type newValue, in Type expectedValue)
        {
            bool changed = IsChanged(ref variable, newValue);

            return changed && variable.Equals(expectedValue);
        }
    }
}
