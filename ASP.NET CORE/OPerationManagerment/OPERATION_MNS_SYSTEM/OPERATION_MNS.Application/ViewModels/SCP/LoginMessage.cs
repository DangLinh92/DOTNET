using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OPERATION_MNS.Application.ViewModels.SCP
{
    public class RequestParam
    {
        public string divisionCd { get; set; }
        public string siteCd { get; set; }
        public string userId { get; set; }
        public string authId { get; set; }
        public string product { get; set; }
        public string locale { get; set; }
        public string errCode { get; set; }
        public string errMsg { get; set; }
        public string authtoken { get; set; }
    }
    public class LoginMessage
    {
        public string code { get; set; }
        public string service { get; set; }
        public string name { get; set; }
        public string locale { get; set; }
        public string divisionCd { get; set; }
    }

    public class LoginParam
    {
        public string userId1 { get; set; }
        public string password1 { get; set; }
        public string locale1 { get; set; }

        [JsonPropertyName("SAVE-ID")]
        public string SAVEID { get; set; }
        public string divisionCd1 { get; set; }
        public string product { get; set; }
    }

    public class SCPLoginModel
    {
        public List<LoginMessage> loginMessage { get; set; }
        public long endDate { get; set; }
        public string existAuth { get; set; }
        public string multiLoginYn { get; set; }
        public object description { get; set; }
        public string locale { get; set; }
        public string sessionremainder { get; set; }
        public string authId { get; set; }
        public string useYn { get; set; }
        public long passwordUpdateDate { get; set; }
        public string divisionCd { get; set; }
        public string authtoken { get; set; }
        public string empNo { get; set; }
        public string userName { get; set; }
        public string userId { get; set; }
        public string isduple { get; set; }
        public string sessionexpired { get; set; }
        public string siteCd { get; set; }
        public string sessioncurrent { get; set; }
        public string userName2 { get; set; }
        public string sessionacquired { get; set; }
        public long startDate { get; set; }
    }
}
