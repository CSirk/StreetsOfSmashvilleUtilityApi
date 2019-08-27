using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StreetsOfSmashvilleUtilityApi.Api.Controllers
{
    public class RegistryController : ApiController
    {
        [HttpGet]
        [Route("api/registry/getvoices")]
        public List<string> GetCollection()
        {
            var listOfKeys = new List<string>();

            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Speech\Voices\Tokens"))
            {
                foreach (var keyName in key.GetSubKeyNames())
                {
                    listOfKeys.Add(keyName);
                }
            }

            return listOfKeys;
        }

        [HttpGet]
        [Route("api/registry/getosversion")]
        public string GetOsVersion()
        {
            var os = "";
            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion"))
            {
                os = key.GetValue("ProductName").ToString();
            }

            return os;
        }
    }
}
