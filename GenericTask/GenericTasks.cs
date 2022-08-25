using System.ComponentModel;

namespace GenericTask
{
    public enum Gender : int
    {
        Male = 1,
        Female = 2,
        Other = 3
    }
    public enum Weekday
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public class GenericTasks
    {
        public static TEnum MapValueToEnum<TEnum, TArg>(TArg value) where TEnum : struct
        {
            TEnum result;

            if (!Enum.TryParse(value?.ToString(), true, out result))
                throw new Exception($"Value '{value}' is not part of {result.GetType()} enum");

            return result;
        }
    }
}