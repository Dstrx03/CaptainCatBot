
using System.Collections.Generic;

namespace Cat.Web.Infrastructure.Platform.WebApi
{
    /**
     * <summary>
     * Basic wrap class for WebApi result of procedure that may produce errors due inner validation, constraints etc. during execution.
     * </summary>
     */
    public class CatProcedureResult
    {
        public CatProcedureResult()
        {
            ErrorMsgs = new List<string>();
        }

        public bool IsSuccess { get; set; }

        public List<string> ErrorMsgs { get; set; }

        public object Data { get; set; }



        public static CatProcedureResult Success(object data = null)
        {
            return new CatProcedureResult
            {
                IsSuccess = true,
                Data = data
            };
        }

        public static CatProcedureResult Error(string[] errors = null, object data = null)
        {
            var result = new CatProcedureResult
            {
                IsSuccess = false,
                Data = data
            };
            if (errors != null) result.ErrorMsgs.AddRange(errors);
            return result;
        }
    }
}