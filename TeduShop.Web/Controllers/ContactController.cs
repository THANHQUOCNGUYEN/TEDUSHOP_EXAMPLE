using AutoMapper;
using BotDetect.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TeduShop.Commom;
using TeduShop.Common;
using TeduShop.Model.Models;
using TeduShop.Service;
using TeduShop.Web.Infrastructure.Exstensions;
using TeduShop.Web.Models;

namespace TeduShop.Web.Controllers
{
    public class ContactController : Controller
    {
        IContactDetailService _contactDetailService;
        IFeedbackService _feedbackService;
        public ContactController(IContactDetailService contactDetailService,IFeedbackService feedbackService)
        {
            this._contactDetailService = contactDetailService;
            this._feedbackService = feedbackService;
        }
        
        // GET: Contact
        public ActionResult Index()
        {
            var model = _contactDetailService.GetDefaultContact(); //thang nay tra ve ContactDetail phai chuyen sang contactDetailViewModel
            var contactViewModel = Mapper.Map<ContactDetail, ContactDetailViewModel>(model);//chuyen contactDetail sang viewModel
            
            FeedbackViewModel viewModel = new FeedbackViewModel();
            viewModel.contactDetail = contactViewModel;
            return View(viewModel);
        }


        [HttpPost]
        [CaptchaValidation("CaptchaCode","ContactCaptcha","Mã xác nhận không đúng")]
        public ActionResult SendFeedback(FeedbackViewModel feedbackViewModel)
        {
            if(ModelState.IsValid)
            {
                Feedback newFeedback = new Feedback();
                newFeedback.UpdateFeedback(feedbackViewModel);
                _feedbackService.Create(newFeedback);
                _feedbackService.Save();

                ViewData["SuccessMsg"] = "Gửi phản hồi thành công !";


                //gửi email đến khách hàng
                string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/contact_template.html"));
                content.Replace("{{Name}}",feedbackViewModel.Name);
                content.Replace("{{Email}}", feedbackViewModel.Email);
                content.Replace("{{Message}}", feedbackViewModel.Message);

                var adminEmail = ConfigHelper.GetByKey("AdminEmail");
                //SEND eMAIL XỬ LÝ BACEND
                MailHelper.SendMail(adminEmail, "Thông tin liên hệ từ website", content);

            }


            feedbackViewModel.contactDetail = GetDetail();
            var abc = new FeedbackViewModel();
            return View("Index",feedbackViewModel);//dung chung view cua thang Index
        }


        public ContactDetailViewModel GetDetail()
        {
            var model = _contactDetailService.GetDefaultContact();
            var contactViewModel = Mapper.Map<ContactDetail, ContactDetailViewModel>(model);
            return contactViewModel;
        }
    }
}   