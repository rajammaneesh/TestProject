using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DCode.Common
{
    public class SessionHelper
    {
        public static void Save(string sessionVariableName, object value)
        {
            HttpContext.Current.Session[sessionVariableName] = value;
        }

        public static object Retrieve(string sessionVariableName)
        {
            if (HttpContext.Current.Session[sessionVariableName] != null)
            {
                return HttpContext.Current.Session[sessionVariableName];
            }
            return null;
        }

        public static void ClearSessionVariables()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}
