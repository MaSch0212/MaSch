using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MaSch.Data.Extensions
{
    public static class DbCommandExtensions
    {
        public static void AddParameterWithValue(this IDbCommand command, string paramName, object value)
        {
            var param = command.CreateParameter();
            param.ParameterName = paramName;
            param.Value = value ?? DBNull.Value;
            command.Parameters.Add(param);
        }
    }
}
