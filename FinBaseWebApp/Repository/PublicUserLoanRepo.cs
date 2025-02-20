using FinBaseWebApp.Models;     
using FinBaseWebApp.Readers;
using FinBaseWebApp.Helpers;        
using System;   
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

namespace FinBaseWebApp.Repository
{
    public class PublicUserLoanRepo
    {

        private readonly PublicUserLoanDAO _userLoanDAO;    

        public PublicUserLoanRepo()
        {
            _userLoanDAO = new PublicUserLoanDAO();     
        }

        public ResultModel GetPublicUserLoanRepo(string mobile)
        {
            try
            {
                var result = _userLoanDAO.GetUserLoanInfo(mobile);    

                return new ResultModel()
                {
                    Success = true,
                    Data = result,
                    ErrorMessage = string.Empty,
                    TechDetails = string.Empty
                };
            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Error getting data",
                    TechDetails = ex.Message + Environment.NewLine + ex.ToString() + Environment.NewLine + (ex.InnerException == null ? "" : ex.InnerException.ToString())
                };
            }
        }

        public ResultModel AddPublicUserLoanRepo(PublicUserLoanModel user)
        {
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                try
                {
                    user.ApplicationNo = HashHelper.GenerateApplicationNumber();    
                    user.CreatedBy = user.Name;
                    user.CreatedAt = DateTime.Now;
                    user.ModifiedBy = user.Name;
                    user.ModifiedAt = DateTime.Now;
                    user.Status = true;

                    var result = false;
                    var isDocInserted = false;

                    var isUserInserted = _userLoanDAO.InsertPublicUserLoan(user);   
                    if (!isUserInserted)    
                        throw new Exception("Failed to insert user into PublicUser table");

                    foreach (var doc in user.DocumentFiles)
                    {
                        doc.CreatedBy = user.Name;
                        doc.CreatedAt = DateTime.Now;
                        doc.ModifiedBy = user.Name;
                        doc.ModifiedAt = DateTime.Now;
                        doc.Status = true;

                        isDocInserted = _userLoanDAO.InsertPublicUserLoanFile(doc);     
                        if (!isDocInserted)
                            throw new Exception($"Failed to insert document: {doc.DocumentFileName}");
                    }

                    /*if (isUserInserted == true && isDocInserted == true)
                    {
                        result = true;
                    }*/
                    transaction.Complete();

                    return new ResultModel()
                    {
                        Success = true,
                        Data = true,
                        ErrorMessage = string.Empty,
                        TechDetails = string.Empty
                    };
                }
                catch (Exception ex)
                {
                    transaction.Dispose();

                    return new ResultModel()
                    {
                        Success = false,
                        Data = null,
                        ErrorMessage = "Error getting data",
                        TechDetails = ex.Message + Environment.NewLine + ex.ToString() + Environment.NewLine + (ex.InnerException == null ? "" : ex.InnerException.ToString())
                    };
                }
            }
        }

        public ResultModel EditPublicUserLoanRepo(PublicUserLoanModel user, string mobile)
        {
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                try
                {   
                    user.ModifiedBy = mobile;
                    user.ModifiedAt = DateTime.Now;
                    user.Status = true;

                    var result = false;
                    var isDocUpdated = false;

                    var isUserUpdated = _userLoanDAO.UpdatePublicUserLoan(user, mobile);    
                    if (!isUserUpdated)
                        throw new Exception("Failed to update user into PublicUser table");

                    foreach (var doc in user.DocumentFiles)     
                    {
                        doc.ModifiedBy = doc.MobileNo;
                        doc.ModifiedAt = DateTime.Now;
                        doc.Status = true;

                        isDocUpdated = _userLoanDAO.UpdatePublicUserLoanFile(doc, mobile);  
                        if (!isDocUpdated)
                            throw new Exception($"Failed to update document: {doc.DocumentFileName}");
                    }   

                    transaction.Complete();

                    return new ResultModel()
                    {
                        Success = true,
                        Data = true,
                        ErrorMessage = string.Empty,
                        TechDetails = string.Empty
                    };
                }
                catch (Exception ex)
                {
                    return new ResultModel()
                    {
                        Success = false,
                        Data = null,
                        ErrorMessage = "Error getting data",
                        TechDetails = ex.Message + Environment.NewLine + ex.ToString() + Environment.NewLine + (ex.InnerException == null ? "" : ex.InnerException.ToString())
                    };
                }
            }
        }

        public ResultModel DeletePublicUserLoanRepo(string mobile)
        {
            try
            {
                var result = _userLoanDAO.DeletePublicUserLoan(mobile);     

                return new ResultModel()
                {
                    Success = true, 
                    Data = result,  
                    ErrorMessage = string.Empty,
                    TechDetails = string.Empty
                };
            }
            catch (Exception ex)
            {
                return new ResultModel()
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Error getting data",
                    TechDetails = ex.Message + Environment.NewLine + ex.ToString() + Environment.NewLine + (ex.InnerException == null ? "" : ex.InnerException.ToString())
                };
            }
        }
    }
}
