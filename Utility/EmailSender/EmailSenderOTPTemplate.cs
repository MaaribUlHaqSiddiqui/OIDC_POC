using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender
{
   public static  class EmailSenderOTPTemplate
    {
        public const string Title = "Your OTP Verification Code";
        public static string GetBody(string recipientName, string otp, int validityMinutes, string companyName = "Lockkeyz",string authType= "Thank you for signing up with us. ")
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <h2 style='color: #333;'>Dear {recipientName},</h2>
                    <p>{authType} To verify your email, please enter the following:</p>
                    <p style='font-size: 18px; font-weight: bold;'>One Time Password (OTP): <span style='color: #007BFF;'>{otp}</span></p>
                    <p>This OTP is valid for <strong>{validityMinutes} seconds</strong> from the receipt of this email.</p>
                    <p>If you didn’t request this email, you can safely ignore it.</p>
                    <p style='margin-top: 20px;'>Best regards,<br>{companyName}</p>
                </body>
                </html>";
        }
    }
}



