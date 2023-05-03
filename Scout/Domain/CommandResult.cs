using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Domain
{
    public class CommandResult
    {
        public Guid Operation { get; private set; }
        public bool IsSuccess { get; private set; }
        public object ResultObject { get; private set; }
        public string ResultMessage { get; private set; }

        internal CommandResult(object resultObject, bool isSuccess, string resultMessage = "", Guid operation = default)
        {
            ResultObject = resultObject;
            IsSuccess = isSuccess;
            ResultMessage = resultMessage;
            Operation = operation;
        }
        internal CommandResult(object resultObject)
        {
            ResultObject = resultObject;
        }
        internal CommandResult()
        {
            IsSuccess = false;
        }
       
        public static CommandResult Error() 
        {
            return new CommandResult();
        }
    }
}
