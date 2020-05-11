using System;
using System.Collections.Generic;
using System.Text;

namespace SJMC.Models
{
    public class ApiResult<T>
    {
        public ApiResult(string rawResult, int code, T result)
        {
            RawResult = rawResult;
            Code = code;
            Result = result;
        }

        public string RawResult { get; private set; }

        public int Code { get; private set; }

        public T Result { get; private set; }

        public bool IsSuccessful
        {
            get { return Code >= 200 && Code < 300; }
        }
    }
}