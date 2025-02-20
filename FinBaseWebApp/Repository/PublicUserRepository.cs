using FinBaseWebApp.Models;
using FinBaseWebApp.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Util;

namespace FinBaseWebApp.Repository
{
    public class PublicUserRepository
    {

        private readonly PublicUserDAO _publicDAO;      

        public PublicUserRepository()
        {
            _publicDAO = new PublicUserDAO();     
        }       

        public async Task<PublicUserModel> LoginUser(string UserName, string Password)
        {
            return await _publicDAO.Public_LoginUser(UserName, Password);       
        }

        public ResultModel GetSinglePublicUserRepo(string UserName)
        {
            try
            {
                var result = _publicDAO.GetUserProfile(UserName);

                return new ResultModel()
                {
                    Success = true,
                    Data = result,
                    ErrorMessage = string.Empty,
                    TechDetails = string.Empty
                };  
            }catch(Exception ex)
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

/*        public ResultModel GetUserProfileRepo(string UserName)
        {
            try
            {
                var result = _publicDAO.GetUserProfile(UserName);

                if (result == null)
                {
                    return new ResultModel
                    {
                        Success = false,
                        Data = null,
                        ErrorMessage = "User not found.",
                        TechDetails = "No user data found for the given username."
                    };
                }

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
        }*/

        public ResultModel AddPublicUserRepo(PublicUserModel user, string LoginUser)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    user.CreatedBy = user.Name;     
                    user.CreatedAt = DateTime.Now;
                    user.ModifiedBy = user.Name;
                    user.ModifiedAt = DateTime.Now;
                    user.Status = true;

                    var result = false;     
                    var isDocInserted = false;      

                    var isUserInserted = _publicDAO.InsertPublicUser(user);
                    if (!isUserInserted)
                        throw new Exception("Failed to insert user into PublicUser table");

                    // Insert documents into PublicUserDocuments table
                    foreach (var doc in user.DocumentFiles)
                    {
                        doc.CreatedBy = user.Name;      
                        doc.CreatedAt = DateTime.Now;
                        doc.ModifiedBy = user.Name;
                        doc.ModifiedAt = DateTime.Now;
                        doc.Status = true;

                        isDocInserted = _publicDAO.InsertPublicUserDocument(doc);
                        if (!isDocInserted)
                            throw new Exception($"Failed to insert document: {doc.DocumentFileName}");
                    }       

                    if(isUserInserted == true && isDocInserted == true)
                    {
                        result = true;    
                    }

                    transaction.Complete();     

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

        public ResultModel EditPublicUserRepo(PublicUserModel user, string UserName)
        {
            using (var transaction = new TransactionScope(TransactionScopeOption.Required, 
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted } ))
            {
                try
                {
                    user.ModifiedBy = UserName;
                    user.ModifiedAt = DateTime.Now;
                    user.Status = true;

                    var result = false;
                    var isDocUpdated = false;

                    var isUserUpdated = _publicDAO.UpdatePublicUser(user, UserName);
                    if (!isUserUpdated)
                        throw new Exception("Failed to update user into PublicUser table");

                    foreach (var doc in user.DocumentFiles)         // Insert documents into PublicUserDocuments table
                    {
                        doc.ModifiedBy = UserName;  
                        doc.ModifiedAt = DateTime.Now;
                        doc.Status = true;  

                        isDocUpdated = _publicDAO.UpdatePublicUserDocument(doc, UserName);
                        if (!isDocUpdated)
                            throw new Exception($"Failed to update document: {doc.DocumentFileName}");
                    }

                    /*if (isUserUpdated && isDocUpdated)
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

        public ResultModel DeletePublicUserRepo(string MobileNo, string LoginUser)
        {
            try
            {
                var result = _publicDAO.DeletePublicUser(MobileNo, LoginUser);     

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

