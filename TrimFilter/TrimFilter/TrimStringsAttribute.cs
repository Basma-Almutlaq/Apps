using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

public class TrimStringsAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument == null) continue;

            var props = argument.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite);

            foreach (var prop in props)
            {
                var currentValue = (string?)prop.GetValue(argument);
                if (currentValue != null)
                {
                    prop.SetValue(argument, currentValue.Trim());
                }
            }
        }
    }
}