using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RemoteAdvisor.Models;
using RemoteAdvisor.Models.Data;
using RemoteAdvisor.UCAP;
using System.Threading.Tasks;

namespace RemoteAdvisor.Controllers
{
    public class HomeController : Controller
    {
        private RemoteSessionsModel db = new RemoteSessionsModel();
        public ActionResult Index()
        {
            DashboardViewModel model = new DashboardViewModel();
            return View(model);
        }
        public ActionResult Create()
        {
            CreateViewModel model = new CreateViewModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                SkypeApiClient client = new SkypeApiClient();
                MeetingInput mi = new MeetingInput();
                mi.Subject = model.Subject;
                var meeting = await client.CreateSkypeAdhocMeetingAsync(mi);
                RemoteAdvisorSession newSession = new RemoteAdvisorSession();
                newSession.SessionId = Guid.NewGuid();
                newSession.Subject = model.Subject;
                newSession.Customer = model.CustomerName;
                newSession.OnlineMeetingUri = meeting.OnlineMeetingUri;
                newSession.JoinUrl = meeting.JoinUrl;
                newSession.CreateDate = DateTime.Now;
                db.RemoteAdvisorSessions.Add(newSession);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<ActionResult> Join(Guid identifier)
        {
            var session = db.RemoteAdvisorSessions.Where(c => c.SessionId == identifier).First();
            SkypeApiClient client = new SkypeApiClient();
            MeetingInput mi = new MeetingInput();
            mi.AccessLevel = "Everyone";
            mi.Description = session.Subject;
            mi.MeetingIdentifier = session.JoinUrl;
            var token = await client.GetAnonymousTokenAsync(mi);
            JoinViewModel jvm = new JoinViewModel();
            jvm.AnonymousToken = token.AnonToken;
            jvm.DiscoverUri = token.DiscoverUrl;
            jvm.JoinUrl = session.JoinUrl;
            jvm.Customer = session.Customer;
            jvm.Subject = session.Subject;
            jvm.OnlineMeetingUri = session.OnlineMeetingUri;
            return View(jvm);
        }
        public ActionResult GettingStarted()
        {
            return View();
        }
    }
}