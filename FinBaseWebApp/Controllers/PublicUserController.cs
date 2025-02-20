using FinBaseWebApp.Helpers;
using FinBaseWebApp.Models;
using FinBaseWebApp.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace FinBaseWebApp.Controllers
{
    [Log]
    public class PublicUserController : ApiController
    {

        private readonly PublicUserRepository _publicUserRepo;
        private readonly UploadHelper uploadHelper;     

        public PublicUserController()
        {
            _publicUserRepo = new PublicUserRepository();       
        }   
            
        [HttpGet]
        [Authorize]
        [Route("api/PublicUser/{UserName}")]    
        public IHttpActionResult GetSinglePublicUser(string UserName)
        {
            var result = _publicUserRepo.GetSinglePublicUserRepo(UserName);
            return Ok(result);      
        }

/*      [HttpGet]
        [Authorize]
        [Route("api/PublicUser/{UserName}")]
        public IHttpActionResult GetSingleUserProfile(string UserName)
      
        {
            var result = _publicUserRepo.GetUserProfileRepo(UserName);
            return Ok(result);
        }  */

        [HttpGet]
        [Route("api/PublicUserDocuments/{fileName}")]
        public IHttpActionResult GetUserDocuments(string fileName)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                var currentUser = identity.Name; // Logged-in user's identity

                // Validate that the file belongs to the logged-in user
                /*bool isAuthorized = new UploadHelper().ValidateUserAccessToFile(currentUser, fileName);
                if (!isAuthorized)
                {
                    return Unauthorized();
                }*/
                // Validate the file name to prevent directory traversal attacks
                if (fileName.Contains("..") || Path.IsPathRooted(fileName))
                {
                    return BadRequest("Invalid file name.");
                }
                // Path where documents are stored
                var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/ProjectFiles/PublicUserFiles/");
                var filePath = Path.Combine(mappedPath, fileName);
                System.Diagnostics.Debug.WriteLine($"Received File Name: {fileName}");
                // Log the resolved file path for debugging
                System.Diagnostics.Debug.WriteLine($"Mapped Path: {mappedPath}");
                System.Diagnostics.Debug.WriteLine($"File Path: {filePath}");   
                System.Diagnostics.Debug.WriteLine($"Does Mapped Path Exist? {Directory.Exists(mappedPath)}");
                //var filesInDirectory = Directory.GetFiles(mappedPath);    
                var filesInDirectory = Directory.GetFiles(mappedPath);
                bool fileFound = false;

                foreach (var file in filesInDirectory)
                {
                    var actualFileName = Path.GetFileName(file);
                    System.Diagnostics.Debug.WriteLine($"Comparing '{actualFileName}' with '{fileName}'");

                    if (string.Equals(actualFileName, fileName, StringComparison.OrdinalIgnoreCase))
                    {
                        System.Diagnostics.Debug.WriteLine($"Match Found: {actualFileName}");
                        fileFound = true;
                        break;
                    }
                }

                if (!fileFound)
                {
                    System.Diagnostics.Debug.WriteLine("File not found in directory.");
                    return NotFound();
                }

                var matchingFile = filesInDirectory.FirstOrDefault(f => Path.GetFileName(f).Equals(fileName, StringComparison.OrdinalIgnoreCase));
                System.Diagnostics.Debug.WriteLine($"Found File: {matchingFile}");
                if (matchingFile == null)
                {
                    System.Diagnostics.Debug.WriteLine("File not found in directory.");
                    return NotFound();
                }
                System.Diagnostics.Debug.WriteLine($"Found File: {matchingFile}");

                // Attempt to open the file in read mode
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    // If successful, the file is not locked or inaccessible
                    System.Diagnostics.Debug.WriteLine($"File {filePath} is accessible.");
                }
                System.Diagnostics.Debug.WriteLine($"File {System.IO.File.Exists(filePath)} exists");     
                // Check if the file exists
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }
                // Read the file as a byte array
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var mimeType = MimeMapping.GetMimeMapping(fileName);
                // Return the file as a response
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(fileBytes)
                };
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);

                // Set disposition based on client requirements
                bool asAttachment = true; // Change to false for inline viewing
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue(asAttachment ? "attachment" : "inline")
                {
                    FileName = fileName
                };

                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                return Ok(new ResultModel
                {
                    Success = false,
                    Data = null,
                    ErrorMessage = "Error processing request.",
                    TechDetails = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("api/PublicUser")]   
        public IHttpActionResult PostPublicUser()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;

                var obj = JsonConvert.DeserializeObject<PublicUserModel>(httpRequest.Form["JsonObjStr"]);       

                if(obj == null)
                {
                    return Ok(new ResultModel() {   
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

                for(int i = 0; i < files.AllKeys.Length; i++)
                {
                    var fileKey = files.AllKeys[i]; // Key from the form-data      
                    var postedFile = files[i];

                    string fileExtension = Path.GetExtension(postedFile.FileName);
                    string OriginalFileName = Path.GetFileNameWithoutExtension(postedFile.FileName);    

                    using (var binaryReader = new BinaryReader(postedFile.InputStream))
                    {
                        var bytes = binaryReader.ReadBytes(postedFile.ContentLength);   

                        if(bytes != null && bytes.Length > 0)
                        {
                            var Filename = OriginalFileName + currentDate.ToString("dd_MMM_yyyy_HH_mm_ss") + fileExtension;     
                            var filePath = Path.Combine(mappedPath, Filename);
                            File.WriteAllBytes(filePath, bytes);

                            string documentType = null;     // Determine DocumentType based on fileKey  
                            if (fileKey.IndexOf("Aadhaar", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                documentType = "Aadhaar";
                            }
                            else if (fileKey.IndexOf("Pan", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                documentType = "PanCard";
                            }

                            if (!string.IsNullOrEmpty(documentType))        // Add document details if document type is identified
                            {
                                obj.DocumentFiles.Add(new PublicUserDocuments
                                {
                                    MobileNo = obj.MobileNo,
                                    EmailId = obj.EmailId,
                                    DocumentFileName = Filename,
                                    DocumentType = documentType
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

                var res = _publicUserRepo.AddPublicUserRepo(obj, identity.Name);
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
        [Route("api/PublicUser/{UserName}")]   
        public IHttpActionResult PutPublicUser(string UserName)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;

                var obj = JsonConvert.DeserializeObject<PublicUserModel>(httpRequest.Form["JsonObjStr"]);

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
                            if (fileKey.IndexOf("Aadhaar", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                documentType = "Aadhaar";
                            }
                            else if (fileKey.IndexOf("Pan", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                documentType = "PanCard";
                            }

                            if (!string.IsNullOrEmpty(documentType))        // Add document details if document type is identified
                            {
                                obj.DocumentFiles.Add(new PublicUserDocuments
                                {   
                                    MobileNo = obj.MobileNo,    
                                    EmailId = obj.EmailId,  
                                    DocumentFileName = Filename,    
                                    DocumentType = documentType     
                                });
                            }
                            else {
                                // Log or handle unsupported files if necessary
                                throw new Exception($"Unsupported file type for key: {fileKey}");      
                            }
                        }
                    }
                }

                var res = _publicUserRepo.EditPublicUserRepo(obj, UserName);   
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
        [Route("api/PublicUser/{MobileNo}")]   
        public IHttpActionResult DeletePublicUser(string MobileNo)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;

                return Ok(new ResultModel()
                {
                    Success = true,     
                    Data = _publicUserRepo.DeletePublicUserRepo(MobileNo, identity.Name),
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

