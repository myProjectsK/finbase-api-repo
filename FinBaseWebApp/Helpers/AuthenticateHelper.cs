﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinBaseWebApp.Helpers
{
    public class AuthenticateHelper
    {

        public bool IsEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}

