using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BowlingScore.Web.Infrastructure;
using BowlingScore.Web.Models;

namespace BowlingScore.Web.Controllers
{
    public class ScoreController : ApplicationController
    {
        // GET: Score
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("Score/Calculate")]
        [HttpPost]
        public ActionResult Calculate(BowlingSession session)
        {
            return JsonNet(new BowlingSessionViewModel(session));
        }

        private class BowlingSessionViewModel
        {
            public BowlingSessionViewModel(BowlingSession session)
            {
                Score = session.Score;
            }

            public int Score { get; set; }
        }
    }
}