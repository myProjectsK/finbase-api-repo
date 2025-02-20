using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinBaseWebApp.Queries
{
    public class PublicUserQueries
    {

        public const string GET_PUBLICUSER_FROM_USERNAME_AND_PASSWORD = "SELECT [MobileNo], [EmailId], [Name] FROM PUBLICUSER " +
            "WHERE ([MobileNo] = @UserName OR [EmailId] = @UserName) AND [Password] = @Password AND STATUS = 1";

        /*public const string GET_PUBLICUSER = "SELECT [Name], [DateOfBirth], [Gender], [MobileNo], [Address], [EmailId] " +
            "FROM PUBLICUSER WHERE ([MobileNo] = @UserName OR [EmailId] = @UserName) AND Status = 1";*/

        public const string GET_USER_DETAILS = "SELECT u.Name, u.DateOfBirth, u.Gender, u.MobileNo, u.Address, u.EmailId, d.DocumentFileName" +
                                                " FROM PUBLICUSER u LEFT JOIN PublicUserDocuments d ON u.MobileNo = d.MobileNo" +
                                                " WHERE (u.MobileNo = @UserName OR u.EmailId = @UserName) AND u.Status = 1";

        public const string GET_USER_AUTHORIZATION = "SELECT DocumentFileName FROM PublicUserDocuments WHERE MobileNo = @UserName AND" +
                                                    " DocumentFileName = @fileName";     

        public const string INSERT_INTO_PUBLICUSER = "INSERT INTO PUBLICUSER([MobileNo], [Name], [DateOfBirth], [Gender], [Address], [EmailId], " +
            "[Password], [CreatedBy], [CreatedAt], [ModifiedBy], [ModifiedAt], [Status]) VALUES " +
            "(@MobileNo, @Name, @DOB, @Gender, @Address, @EmailId, @Password, @CreatedBy, @CreatedAt, @ModifiedBy, @ModifiedAt, @Status)";

        public const string INSERT_INTO_PUBLICUSERDOCUMENTS = "INSERT INTO PublicUserDocuments([MobileNo], [DocumentType], [DocumentFileName], " +
            "[CreatedBy], [CreatedAt], [ModifiedBy], [ModifiedAt], [Status]) " +
            "VALUES(@MobileNo, @DocumentType, @DocumentFileName, @CreatedBy, @CreatedAt, @ModifiedBy, @ModifiedAt, @Status)";

        public const string UPDATE_PUBLICUSER = "UPDATE PUBLICUSER SET [MobileNo] = @MobileNo, [Name] = @Name," +
            " [DateOfBirth] = @DOB, [Gender] = @Gender, [Address] = @Address, [EmailId] = @EmailId, " +
            "[ModifiedBy] = @ModifiedBy, [ModifiedAt] = @ModifiedAt, [Status] = @Status WHERE ([MobileNo] = @UserName OR [EmailId] = @UserName)";

        public const string UPDATE_PUBLICUSERDOCUMENTS = "UPDATE PublicUserDocuments SET [MobileNo] = @MobileNo, [EmailId] = @EmailId," +
            " [DocumentFileName] = @DocumentFileName, [ModifiedBy] = @ModifiedBy, [ModifiedAt] = @ModifiedAt, [Status] = @Status" +
            " WHERE ([MobileNo] = @UserName OR [EmailId] = @UserName) AND [DocumentType] = @DocumentType";    

        public const string DELETE_PUBLICUSER = "DELETE FROM PUBLICUSER WHERE [MobileNo] = @MobileNo";     
    }
}

