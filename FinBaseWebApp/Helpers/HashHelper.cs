using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace FinBaseWebApp.Helpers
{
    public class HashHelper
    {   
        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);        
        }

        public static string GenerateApplicationNumber()
        {
            // Generate a GUID
            Guid guid = Guid.NewGuid();

            // Convert GUID to numeric and truncate to fit the required range
            string numericGuid = Math.Abs(guid.GetHashCode()).ToString();

            // Ensure it starts with 1001 and meets size requirements
            return numericGuid.PadLeft(4, '1').Substring(0, 9);
        }   

        public static string GetHashedPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);       
            return hashedPassword;
        }
    }
}
