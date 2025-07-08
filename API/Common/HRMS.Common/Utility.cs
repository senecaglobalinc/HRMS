using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace HRMS.Common
{
    /// <summary>
    /// Utility class for string manipulations
    /// </summary>
    public class Utility
    {
        #region SHA1HashStringForUTF8String
        /// <summary>
        /// SHA1HashStringForUTF8String
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns></returns>
        public static string SHA1HashStringForUTF8String(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            using (var sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(bytes);

                return HexStringFromBytes(hashBytes);
            }
        }
        #endregion

        #region HexStringFromBytes
        /// <summary>
        /// Convert an array of bytes to a string of hex digits
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>String of hex digits</returns>
        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return string.Format("0x{0:x8}", sb.ToString());
        }
        #endregion

        #region GetDateTimeInIST
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeInIST(DateTime? date)
        {
            try
            {
                //TimeZoneInfo myZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"); //Windows specific
                TimeZoneInfo myZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata"); //Linux               
                DateTime iSTDate = TimeZoneInfo.ConvertTimeFromUtc(date.GetValueOrDefault(), myZone);
                return iSTDate;
            }
            catch
            {
                throw;
            }
        }
        public static DateTime GetDateTimeInIST()
        {
            try
            {
                DateTime currentTime = new DateTime(DateTime.Now.Ticks, DateTimeKind.Unspecified);
                //TimeZoneInfo myZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"); //Windows specific
                TimeZoneInfo myZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata"); //Linux               
                DateTime iSTDate = TimeZoneInfo.ConvertTimeFromUtc(currentTime, myZone);
                return iSTDate;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region DecryptText
        /// <summary>
        /// Decrypt Text which is encoded using AES
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static string DecryptStringAES(string cipherText)
        {
            try
            {
                if (string.IsNullOrEmpty(cipherText))
                    return string.Empty;

                var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
                var iv = Encoding.UTF8.GetBytes("8080808080808080");

                string dummyData = cipherText.Trim().Replace(" ", "+");

                if (dummyData.Length % 4 > 0)
                    dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');

                var encrypted = Convert.FromBase64String(dummyData);
                var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
                return string.Format(decriptedFromJavascript);
            }
            catch
            {
                throw;
            }
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            // Create an RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream  
                                // and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }
        #endregion

        #region EncryptText
        /// <summary>
        ///  which is used Encrypt the given text using AES
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static string EncryptStringAES(string cipherText)
        {
            try
            {
                if (string.IsNullOrEmpty(cipherText))
                    return string.Empty;

                var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
                var iv = Encoding.UTF8.GetBytes("8080808080808080");

                var decriptedFromJavascript = EncryptStringToBytes(cipherText, keybytes, iv);
                return Convert.ToBase64String(decriptedFromJavascript);
            }
            catch
            {
                throw;
            }
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an Rijndael object
            // with the specified key and IV.
            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        #endregion

        #region SplitStringByCommaToList
        public static List<string> SplitStringByCommaToList(string inputString)
        {
            List<string> inputList;
            if(string.IsNullOrEmpty(inputString))
            {
                inputList = new List<string>();
            }
            else
            {
                inputList= inputString.Split(',').ToList();
            }
            return inputList;
        }

        #endregion

        #region GetNotificationTemplatePath

        public static string GetNotificationTemplatePath(string baseDirectory, List<string> subDirectories)
        {
            try
            {
                string templatePath;
                if (subDirectories.Count == 0)
                {
                    throw new ArgumentException("List of Sub Directories cannot be null or empty");
                }
                templatePath = Path.Combine(baseDirectory, Path.Combine(subDirectories.ToArray()));
                return templatePath;

            }
            catch (Exception)
            {
                throw;
            }
        }
            
        #endregion
    }
}
