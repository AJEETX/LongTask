using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Engine;

namespace Webby.Controllers
{
    public class HomeController : Controller
    {
        private static CancellationTokenSource _cancellationTokenSource = null; Task _longTask = null;
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Run(CancellationTokenSource cancellationTokenSource)
        {
            try {
                _cancellationTokenSource = cancellationTokenSource;
                var longTask = new LongRunningTask();
                _longTask= Task.Factory.StartNew(() => { longTask.Process(_cancellationTokenSource.Token); }, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                _longTask.Wait();
            }
            catch (AggregateException ae) {
                ae.Flatten().Handle(ex => {
                    if (ex is Exception) {
                        Task.Run(() => {
                            return Json("Specific OperationCanceledException", JsonRequestBehavior.AllowGet);
                        }); throw ex;
                    }
                    else {
                        return false;
                    }
                });
            }   return Json("The Operation Started successfully .", JsonRequestBehavior.AllowGet);
        }
        public JsonResult Cancel()
        {
            try
            {
                _cancellationTokenSource.Cancel();
            }
            catch (AggregateException ae)
            {
                ae.Flatten().Handle(ex =>
                {
                    if (ex is Exception)
                    {
                        Task.Run(() =>
                        {
                            return Json("Specific OperationCanceledException", JsonRequestBehavior.AllowGet);
                        }); throw ex;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
            return Json("The Operation Cancelled successfully .", JsonRequestBehavior.AllowGet);
        }
    }
}