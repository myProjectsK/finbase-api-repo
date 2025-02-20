using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinBaseWebApp.Queries
{
    public class PublicUserLoanQueries
    {   

        public const string GET_USERLOAN_DETAILS = "SELECT u.ApplicationNo, u.Name, u.MobileNo, u.DateOfBirth, u.Gender, u.OccupationType, u.OccupationName, " +
                                        " u.LoanType, u.Amount, u.RateOfInterest, f.DocumentFileName" +
                                        " FROM PublicUserLoan u LEFT JOIN PublicUserLoanFiles f ON u.MobileNo = d.MobileNo" +
                                        " WHERE u.MobileNo = @Phone AND u.Status = 1";

        public const string INSERT_INTO_PUBLICUSERLOAN = "INSERT INTO PublicUserLoan( [ApplicationNo], [Name], [MobileNo], [DateOfBirth], [Gender], [OccupationType], " +
            " [OccupationName], [LoanType], [Amount], [RateOfInterest], [CreatedBy], [CreatedAt], [ModifiedBy], [ModifiedAt], [Status]) VALUES " +
        "(@AppNo, @Name, @MobileNo, @DOB, @Gender, @WorkType, @WorkName, @loanType, @Amt, @ROI, @CreatedBy, @CreatedAt, @ModifiedBy, @ModifiedAt, @Status)";

        public const string INSERT_INTO_PUBLICUSERLOANFILES = "INSERT INTO PublicUserLoanFiles([MobileNo], [FileType], [DocumentFileName], " +
                            "[CreatedBy], [CreatedAt], [ModifiedBy], [ModifiedAt], [Status]) " +
                            "VALUES(@mobileNo, @fileType, @documentFileName, @CreatedBy, @CreatedAt, @ModifiedBy, @ModifiedAt, @Status)";

        public const string UPDATE_PUBLICUSERLOAN = "UPDATE PublicUserLoan SET [Name] = @Name, [MobileNo] = @MobileNo, [DateOfBirth] = @DOB," +
        " [Gender] = @Gender, [OccupationType] = @WorkType, [OccupationName] = @WorkName, [LoanType] = @loanType, [Amount] = @Amt, " +
        " [RateOfInterest] = @ROI, [ModifiedBy] = @ModifiedBy, [ModifiedAt] = @ModifiedAt, [Status] = @Status WHERE [MobileNo] = @Phone";    

        public const string UPDATE_PUBLICUSERLOANFILES = "UPDATE PublicUserLoanFiles SET [MobileNo] = @MobileNo," +
            " [DocumentFileName] = @DocumentFileName, [ModifiedBy] = @ModifiedBy, [ModifiedAt] = @ModifiedAt, [Status] = @Status" +
            " WHERE [MobileNo] = @Phone AND [FileType] = @fileType";

        public const string DELETE_PUBLICUSERLOAN = "DELETE FROM PublicUserLoan WHERE [MobileNo] = @Phone";      
    }
}
