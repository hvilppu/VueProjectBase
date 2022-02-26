namespace Project.Domain.Dtos
{
    public abstract class BaseDto
    {
        public abstract T FillFrom<T>(object source) where T : class;
        public abstract T FillTo<T>(T toFill) where T : class;
        public bool IsDirty { get; set; }
        public class PropertyCopier<TFrom, TTo> where TFrom : class where TTo : class
        {
            public static void CopyProperties(TFrom from, TTo to) 
            {
                var fromProperties = from.GetType().GetProperties();
                var toProperties = to.GetType().GetProperties();

                foreach (var fromProperty in fromProperties)
                {
                    foreach (var toProperty in toProperties)
                    {
                        if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
                        {
                            if (fromProperty.GetValue(from) != null)
                                toProperty.SetValue(to, fromProperty.GetValue(from));

                            break;
                        }
                    }
                }
            }
        }

    }
}
