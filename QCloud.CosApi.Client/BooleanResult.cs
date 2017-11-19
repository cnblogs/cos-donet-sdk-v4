using System;
using System.Collections.Generic;
using System.Text;

namespace QCloud.CosApi.Client
{
    public class BooleanResult
    {
        public BooleanResult() { }

        public BooleanResult(bool success, string value)
        {
            Success = success;
            Value = value;
        }

        public bool Success { get; set; }

        public string Value { get; set; }

        public static BooleanResult Succeed(string value = "")
        {
            return new BooleanResult { Success = true, Value = value };
        }

        public static BooleanResult Fail(string value = "")
        {
            return new BooleanResult { Success = false, Value = value };
        }
    }

}
