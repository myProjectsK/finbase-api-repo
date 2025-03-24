using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinBaseWebApp.Queries
{
    public class AuthenticateModuleQueries
    {

        public const string GET_USER_DETAILS = "SELECT u.UserId, u.EmailId, u.Name, r.RoleName " +
                            " FROM Users u LEFT JOIN UserRoles r ON u.RoleId = r.RoleId " +
                            " WHERE (u.MobileNo = @UserName OR u.EmailId = @UserName) AND u.Status = 1";    

        public const string GET_LOGINUSER_FROM_USERNAME_AND_PASSWORD = "SELECT u.UserId, u.MobileNo, u.EmailId, u.Name, u.PasswordHash, r.RoleName " +
                            " FROM Users u LEFT JOIN UserRoles r ON u.RoleId = r.RoleId " +
                            " WHERE (u.MobileNo = @UserName OR u.EmailId = @UserName) AND u.Status = 1";    

        public const string GET_ALL_REFRESHTOKEN_DETAILS = "SELECT * FROM REFRESHTOKEN";

        public const string GET_TOKEN_BY_ID = "SELECT * FROM REFRESHTOKEN WHERE [TokenID] = @ID";

        public const string GET_TOKEN_BY_USERNAME = "SELECT * FROM REFRESHTOKEN";

        public const string CHECK_TOKEN_BY_ID = "SELECT * FROM REFRESHTOKEN WHERE [TokenID] = @tokenid";

        public const string CHECK_TOKEN_BY_USERNAME = "SELECT * FROM REFRESHTOKEN WHERE [UserName] = @Username";

        public const string INSERT_REFRESH_TOKEN = "INSERT INTO REFRESHTOKEN([TokenID], [UserName], [IssuedDate], [ExpiredDate], [TokenTicket]) VALUES " +
            "(@Id, @UserName, @IssuedDateTime, @ExpiredDateTime, @ProtectedTicket)";


        public const string DELETE_REFRESH_TOKEN_FROM_ID = "SELECT * FROM REFRESHTOKEN WHERE [TokenID] = @Id";      
    }
}

