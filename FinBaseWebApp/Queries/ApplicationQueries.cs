using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinBaseWebApp.Queries
{
    public class ApplicationQueries
    {   

        public const string GET_APPLICATION_DETAILS = "SELECT a.ApplicationNo, a.Name, a.MobileNo, a.DateOfBirth, a.Gender, a.OccapationType, a.OccupationName, " +
                                        " a.LoanType, a.Amount, a.RateOfInterest, a.CreatedAt, f.DocumentFileName" +
                                        " FROM Applications a LEFT JOIN ApplicationFiles f ON a.MobileNo = f.MobileNo" +
                                        " WHERE a.MobileNo = @Phone AND a.Status = 1";

        public const string INSERT_INTO_APPLICATIONS = "INSERT INTO Applications( [ApplicationNo], [Name], [MobileNo], [DateOfBirth], [Gender], [OccupationType], " +
            " [OccupationName], [LoanType], [Amount], [RateOfInterest], [CreatedBy], [CreatedAt], [ModifiedBy], [ModifiedAt], [Status]) VALUES " +
        "(@AppNo, @Name, @MobileNo, @DOB, @Gender, @WorkType, @WorkName, @loanType, @Amt, @ROI, @CreatedBy, @CreatedAt, @ModifiedBy, @ModifiedAt, @Status)";

        public const string INSERT_INTO_APPLICATIONFILES = "INSERT INTO ApplicationFiles([MobileNo], [FileType], [DocumentFileName], " +
                            "[CreatedBy], [CreatedAt], [ModifiedBy], [ModifiedAt], [Status]) " +
                            "VALUES(@mobileNo, @fileType, @documentFileName, @CreatedBy, @CreatedAt, @ModifiedBy, @ModifiedAt, @Status)";

        public const string UPDATE_APPLICATIONS = "UPDATE Applications SET [Name] = @Name, [MobileNo] = @MobileNo, [DateOfBirth] = @DOB," +
        " [Gender] = @Gender, [OccupationType] = @WorkType, [OccupationName] = @WorkName, [LoanType] = @loanType, [Amount] = @Amt, " +
        " [RateOfInterest] = @ROI, [ModifiedBy] = @ModifiedBy, [ModifiedAt] = @ModifiedAt, [Status] = @Status WHERE [MobileNo] = @Phone";    

        public const string UPDATE_APPLICATIONFILES = "UPDATE ApplicationFiles SET [MobileNo] = @MobileNo," +
            " [DocumentFileName] = @DocumentFileName, [ModifiedBy] = @ModifiedBy, [ModifiedAt] = @ModifiedAt, [Status] = @Status" +
            " WHERE [MobileNo] = @Phone AND [FileType] = @fileType";

        public const string DELETE_APPLICATIONS = "DELETE FROM Applications WHERE [MobileNo] = @Phone";      
    }
}
