﻿using Newtonsoft.Json;

namespace BWJ.Web.OTM.Models.Request.Authentication
{
    internal class LoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }

        [JsonProperty("submit-login")]
        public string SubmitLogin { get; } = string.Empty;
    }
}
