using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BowlingScore.Web.Controllers
{
    public class ScoreController : Controller
    {
        // GET: Score
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}