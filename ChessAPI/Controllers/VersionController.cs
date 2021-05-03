using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChessAPI.Controllers
{
    public class Version
    {
        public string name = "ChessAPI";
        public string version = "0.1";
    }

    public class VersionController : Controller
    {

        // GET: Version
        public Version GetVersion()
        {
            Version version = new Version();
            return version;
        }
    }
}