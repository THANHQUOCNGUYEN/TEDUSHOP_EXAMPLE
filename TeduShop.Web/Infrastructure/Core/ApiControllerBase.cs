using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeduShop.Model.Models;
using TeduShop.Service;

namespace TeduShop.Web.Infrastructure.Core
{
    //Viet cac phuong thuc chung
    public class ApiControllerBase : ApiController
    {
        private IErrorService _errorService;
        public ApiControllerBase(IErrorService errorService) //thuoc tinh nay luon de la protected hay la public
        {
            this._errorService = errorService;
        }

        private void LogError(Exception ex)
        {
            try
            {
                Error error = new Error();
                error.CreateDate = DateTime.Now;
                error.Message = ex.Message;
                error.StackTrace = ex.StackTrace;
                _errorService.Create(error);
                _errorService.Save();
            }
            catch
            {

            }
        }
        //Phuong thuc don tat ca cac ActionRequest 
        protected HttpResponseMessage CreateHttpRespone(HttpRequestMessage requestMessage,Func<HttpResponseMessage> function)
        {
            HttpResponseMessage response = null;
            try
            {
                response = function.Invoke();
            }
            catch(DbEntityValidationException ex)
            {
                foreach(var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{ eve.Entry.State}\" has the following validation errors:"
                        );
                    foreach(var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine($"- Property: \"{ve.PropertyName}\",Error: \" ve.ErrorMessage\"");
                    }
                }
                LogError(ex);//ghi log sản phẩm
                response = requestMessage.CreateResponse(HttpStatusCode.BadRequest, ex.Message);//400
            }
            return response;
        }
    }
}
