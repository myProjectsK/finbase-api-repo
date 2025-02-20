using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinBaseWebApp.Queries
{
    public class LogQueries
    {
        public const string INSERT_INTO_APILOGS = "INSERT INTO ApiLogs " +
        "(Controller, MethodName, UserId, Parameters, PostData, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt, Status) " +
        "VALUES(@Controller, @MethodName, @UserId, @Parameters, @PostData, @CreatedBy, @CreatedAt, @ModifiedBy, @ModifiedAt, @Status); ";    
    }
}
