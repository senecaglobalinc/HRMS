using HRMS.KRA.Entities;
using JwtUtils;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace HRMS.KRA.API.Auth
{
	public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public bool AuthenticationRequired { get; set; }

        /// <summary>  
        /// This will Authorize User  
        /// </summary>  
        /// <returns></returns>  
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
             List<AuthenticationClients> authenticationClients = GetAuthenticationClients();
            if (AuthenticationRequired)
            {
                if (filterContext != null)
                {
                    Microsoft.Extensions.Primitives.StringValues authTokens;
                    filterContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authTokens);
                    var _token = authTokens.FirstOrDefault();
                    if (_token != null)
                    {
                        string authToken = _token.Replace("Bearer ", "");
                        var handler = new JwtSecurityTokenHandler();
                        var decodedValue = handler.ReadJwtToken(authToken);
                        var emailValue = decodedValue.Claims.Single(x => x.Type == "email").Value;
                        var expTime = decodedValue.Claims.Single(x => x.Type == "exp").Value;
                        var clientName = decodedValue.Claims.Single(x => x.Type == "client").Value;
                       
                        var tokenValues = authenticationClients?.Find(x => x.clientName.ToLower() == clientName.ToLower());
                        string signatureKey = $"{tokenValues.clientId}:{tokenValues.clientSecret}";
                        if (authToken != null)
                        {
                            if (IsValidToken(authToken, signatureKey))
                            {
                                var ticks = long.Parse(expTime);
                                var tokenTicks = ticks;
                                var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;
                                var currentTime = DateTime.Now.ToUniversalTime();
                                var valid = tokenDate >= currentTime;
                                // Validating token expiration time
                                if (valid)
                                {
                                    filterContext.HttpContext.Response.Headers.Add("authToken", authToken);
                                    filterContext.HttpContext.Response.Headers.Add("UserEmailId", emailValue);
                                    filterContext.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");
                                    filterContext.HttpContext.Response.Headers.Add("storeAccessiblity", "Authorized");
                                    return;
                                }
                                else
                                {
                                    filterContext.Result = new JsonResult("NotAuthorized")
                                    {
                                        Value = new
                                        {
                                            Status = "Error",
                                            Message = "Token expired"
                                        },
                                    };
                                }
                            }
                            else
                            {
                                filterContext.HttpContext.Response.Headers.Add("authToken", authToken);
                                filterContext.HttpContext.Response.Headers.Add("AuthStatus", "NotAuthorized");

                                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                                filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Not Authorized";
                                filterContext.Result = new JsonResult("NotAuthorized")
                                {
                                    Value = new
                                    {
                                        Status = "Unauthorized",
                                        statusCode = "401",
                                        Message = "Invalid Token"
                                    },
                                };
                            }
                        }
                    }
                    else
                    {
                        filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                        filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide authToken";
                        filterContext.Result = new JsonResult("Please Provide authToken")
                        {
                            Value = new
                            {
                                Status = "Error",
                                Message = "Please Provide authToken"
                            },
                        };
                    }
                }
            }
            else
            {
                return;
            }
        }
        private static string CreateSha256Hash(string text)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
        private List<AuthenticationClients> GetAuthenticationClients()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.qa.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.uat.json", optional: true, reloadOnChange: true).Build();
            AuthenticationRequired = configuration.GetValue<bool>("AuthenticationServer:EnableAuthentication");
            return configuration.GetSection("AuthenticationServer:Clients").Get<List<AuthenticationClients>>();
        }
        public bool IsValidToken(string authToken, string signatureKey)
        {
            //validate Token here  
            string publicKey = CreateSha256Hash(signatureKey);
            if (JWT.HS256.ValidateSignature(authToken, publicKey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
