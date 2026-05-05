using InfoBot.Models;
using InfoBot.Models;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace InfoBot.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // ✅ Get all error codes from DB
        public JsonResult GetErrorCodes()
        {
            using (DemoDBEntities db = new DemoDBEntities())
            {
                var errorList = db.ims_error_handle
                                  .Select(e => new
                                  {
                                      e.error_code,
                                      e.description,
                                      video_path = e.video_path
                                  })
                                  .ToList();

                return Json(errorList, JsonRequestBehavior.AllowGet);
            }
        }

        // ✅ Send email via SMTP to kitturshashank@gmail.com
        [HttpPost]
        public ActionResult SendEmail(string errorCode, string description)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(
                        "ims2000.v2@gmail.com",
                        "Creintreos Software Team"
                    );
                    mail.To.Add("software@cautomate.com");
                    mail.Subject = $"Error Info: {errorCode}";
                    mail.Body = $"{description}";
                    mail.IsBodyHtml = false;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(
                            "ims2000.v2@gmail.com",
                            "xmiaxtnxcubimehm"   // Gmail App Password
                        );

                        smtp.Send(mail);
                    }
                }

                return Content("Email sent successfully!");
            }
            catch (SmtpException ex)
            {
                return Content("SMTP Error: " + ex.StatusCode + " - " + ex.Message);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}
