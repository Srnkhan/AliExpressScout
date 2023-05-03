using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Domain
{
    public enum CommandType
    {
        GetInformation = 0,
        Click = 1,
        GetText = 2,
        GoBack = 3,
        ScrolDown = 4,
        CheckComponent = 5,
        GetInnerText = 6,
        JsQuery = 7,
        GoUrl = 8,
    }
}
