namespace BloodWork.Commons
{
    public class Utils
    {
        public static bool IsChanged<Type>(ref Type variable, in Type newValue)
        {
            bool changed = !variable.Equals(newValue);

            variable = newValue;

            return changed;
        }
    }
}