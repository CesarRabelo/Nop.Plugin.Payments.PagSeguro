using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Payments;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using NopBrasil.Plugin.Payments.PagSeguro.Models;
using NopBrasil.Plugin.Payments.PagSeguro.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NopBrasil.Plugin.Payments.PagSeguro.Controllers
{
    public class PaymentPagSeguroController : BasePaymentController
    {
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IPagSeguroService _pagSeguroService;
        private readonly PagSeguroPaymentSetting _pagSeguroPaymentSetting;

        public PaymentPagSeguroController(ISettingService settingService, IWebHelper webHelper, IPagSeguroService pagSeguroService, PagSeguroPaymentSetting pagSeguroPaymentSetting)
        {
            this._settingService = settingService;
            this._webHelper = webHelper;
            this._pagSeguroService = pagSeguroService;
            this._pagSeguroPaymentSetting = pagSeguroPaymentSetting;
        }

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public ActionResult Configure()
        {
            var model = new ConfigurationModel() { PagSeguroToken = _pagSeguroPaymentSetting.PagSeguroToken, PagSeguroEmail = _pagSeguroPaymentSetting.PagSeguroEmail, PaymentMethodDescription = _pagSeguroPaymentSetting.PaymentMethodDescription };
            return View(@"~/Plugins/Payments.PagSeguro/Views/PaymentPagSeguro/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            _pagSeguroPaymentSetting.PagSeguroToken = model.PagSeguroToken;
            _pagSeguroPaymentSetting.PagSeguroEmail = model.PagSeguroEmail;
            _pagSeguroPaymentSetting.PaymentMethodDescription = model.PaymentMethodDescription;
            _settingService.SaveSetting(_pagSeguroPaymentSetting);

            return View(@"~/Plugins/Payments.PagSeguro/Views/PaymentPagSeguro/Configure.cshtml", model);
        }

        [Area(AreaNames.Admin)]
        public ActionResult PaymentInfo() => View("~/Plugins/Payments.PagSeguro/Views/PaymentPagSeguro/PaymentInfo.cshtml");

        [NonAction]
        public IList<string> ValidatePaymentForm(FormCollection form) => new List<string>();

        [NonAction]
        public ProcessPaymentRequest GetPaymentInfo(FormCollection form) => new ProcessPaymentRequest();
    }
}
