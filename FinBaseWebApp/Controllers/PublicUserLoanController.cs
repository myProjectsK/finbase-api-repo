using FinBaseWebApp.Helpers;
using FinBaseWebApp.Models;
using FinBaseWebApp.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;      


namespace FinBaseWebApp.Controllers
{
    [Log]   
    public class PublicUserLoanController : ApiController
    {
        private readonly PublicUserLoanRepo _userLoanRepo;  
        private readonly UploadHelper uploadHelper;     

        public PublicUserLoanController()
        {
            _userLoanRepo = new PublicUserLoanRepo();   
        }

        [HttpGet]
        [Authorize]
        [Route("api/PublicUserLoan/{mobile}")]
        public IHttpActionResult Get(string mobile)
        {
            var result = _userLoanRepo.GetPublicUserLoanRepo(mobile);   
            return Ok(result);  
        }

        [HttpPost]      
        [Authorize] 
        [Route("api/PublicUserLoan")]
        public IHttpActionResult Post()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;

                var obj = JsonConvert.DeserializeObject<PublicUserLoanModel>(httpRequest.Form["JsonObjStr"]);

                if (obj == null)
                {
                    return Ok(new ResultModel()
                    {
                        Success = false,
                        Data = null,
                        ErrorMessage = "Bad Request",
                        TechDetails = "Bad Request"
                    });
                }

                DateTime currentDate = DateTime.Now;

                var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/ProjectFiles/PublicUserLoanFiles/");
                if (!System.IO.Directory.Exists(mappedPath))
                    System.IO.Directory.CreateDirectory(mappedPath);

                var files = httpRequest.Files;

                for (int i = 0; i < files.AllKeys.Length; i++)
                {
                    var fileKey = files.AllKeys[i]; // Key from the form-data      
                    var postedFile = files[i];

                    string fileExtension = Path.GetExtension(postedFile.FileName);
                    string OriginalFileName = Path.GetFileNameWithoutExtension(postedFile.FileName);

                    using (var binaryReader = new BinaryReader(postedFile.InputStream))
                    {
                        var bytes = binaryReader.ReadBytes(postedFile.ContentLength);

                        if (bytes != null && bytes.Length > 0)
                        {
                            var Filename = OriginalFileName + currentDate.ToString("dd_MMM_yyyy_HH_mm_ss") + fileExtension;
                            var filePath = Path.Combine(mappedPath, Filename);
                            File.WriteAllBytes(filePath, bytes);

                            string documentType = null;     // Determine DocumentType based on fileKey  
                            if (fileKey.IndexOf("Signature", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                documentType = "Signature";
                            }

                            if (!string.IsNullOrEmpty(documentType))        // Add document details if document type is identified
                            {   
                                obj.DocumentFiles.Add(new PublicUserLoanFilesModel
                                {
                                    MobileNo = obj.MobileNo,    
                                    DocumentFileName = Filename,    
                                    FileType = documentType     
                                }); 
                            }   
                            else
                            {   
                                throw new Exception($"Unsupported file type for key: {fileKey}");   // Log or handle unsupported files if necessary
                            }
                        }
                    }
                }

                var res = _userLoanRepo.AddPublicUserLoanRepo(obj);     
                return Ok(res);
            }
            catch (Exception ex)
            {
                return Ok(new ResultModel()
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Error Processing Request",
                    TechDetails = ex.Message + Environment.NewLine + ex.ToString() + Environment.NewLine + (ex.InnerException == null ? "" : ex.InnerException.ToString())
                });
            }
        }

        [HttpPut]
        [Authorize]
        [Route("api/PublicUserLoan/{mobile}")]
        public IHttpActionResult Put(string mobile)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;

                var obj = JsonConvert.DeserializeObject<PublicUserLoanModel>(httpRequest.Form["JsonObjStr"]);

                if (obj == null)
                {
                    return Ok(new ResultModel()
                    {
                        Success = false,
                        Data = null,
                        ErrorMessage = "Bad Request",
                        TechDetails = "Bad Request"
                    });
                }
                DateTime currentDate = DateTime.Now;

                var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/ProjectFiles/PublicUserFiles/");
                if (!System.IO.Directory.Exists(mappedPath))
                    System.IO.Directory.CreateDirectory(mappedPath);

                var files = httpRequest.Files;

                for (int i = 0; i < files.AllKeys.Length; i++)
                {
                    var fileKey = files.AllKeys[i]; // Key from the form-data       
                    var postedFile = files[i];

                    string fileExtension = Path.GetExtension(postedFile.FileName);
                    string originalFileName = Path.GetFileNameWithoutExtension(postedFile.FileName);

                    using (var binaryReader = new BinaryReader(postedFile.InputStream))
                    {
                        var bytes = binaryReader.ReadBytes(postedFile.ContentLength);
                        if (bytes != null && bytes.Length > 0)
                        {
                            var Filename = $"{originalFileName}_{currentDate:dd_MMM_yyyy_HH_mm_ss}{fileExtension}";
                            var filePath = Path.Combine(mappedPath, Filename);
                            File.WriteAllBytes(filePath, bytes);

                            string documentType = null;     // Determine DocumentType based on fileKey  
                            if (fileKey.IndexOf("SignatureProof", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                documentType = "SignatureProof";
                            }

                            if (!string.IsNullOrEmpty(documentType))        // Add document details if document type is identified
                            {
                                obj.DocumentFiles.Add(new PublicUserLoanFilesModel
                                {
                                    MobileNo = obj.MobileNo,    
                                    FileType = documentType,    
                                    DocumentFileName = Filename 
                                });
                            }
                            else
                            {
                                // Log or handle unsupported files if necessary
                                throw new Exception($"Unsupported file type for key: {fileKey}");
                            }
                        }
                    }
                }

                var res = _userLoanRepo.EditPublicUserLoanRepo(obj, mobile);    
                return Ok(res);
            }
            catch (Exception ex)
            {
                return Ok(new ResultModel()
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Error Processing Request",
                    TechDetails = ex.Message + Environment.NewLine + ex.ToString() + Environment.NewLine + (ex.InnerException == null ? "" : ex.InnerException.ToString())
                });
            }
        }

        [HttpDelete]
        [Route("api/PublicUserLoan/{mobile}")]
        public IHttpActionResult Delete(string mobile)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;

                return Ok(new ResultModel()
                {
                    Success = true,
                    Data = _userLoanRepo.DeletePublicUserLoanRepo(mobile),       
                    ErrorMessage = "User Deleted Successfully",
                    TechDetails = string.Empty
                });

            }
            catch (Exception ex)
            {
                return Ok(new ResultModel()
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Error Processing Request...",
                    TechDetails = ex.Message + Environment.NewLine + ex.ToString() + Environment.NewLine + (ex.InnerException == null ? "" : ex.InnerException.ToString())
                });
            }
        }   
    }   
}       
