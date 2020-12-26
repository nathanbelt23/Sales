namespace Sales.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Http;
    using Common.Models;
    using Helpers;
    using Newtonsoft.Json.Linq;
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {

    // [Route("api/users")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public Response PostUser(UserRequest userRequest)
        {
            if (userRequest.ImageArray != null && userRequest.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(userRequest.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Users";

                var folder2 = "\\Content\\Users";

                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhotoURLFIN(stream, $"{"g:\\ProyectosXamarin\\Sales\\SalesBackEnd"}\\{folder2.Replace("~", "")}", file);



                if (response)
                {
                    userRequest.ImagePath = fullPath;
                }
            }

            var answer = UsersHelper.CreateUserASP(userRequest);

            if (answer.IsSuccess)
            {
                answer.IsSuccess = false;
                return answer;
            }
            else
            {
               
                return answer;
            }
        }

        [HttpPost]
        [Authorize]
        [Route("GetUser")]
        public IHttpActionResult GetUser(JObject form)
        {
            try
            {
                var email = string.Empty;
                dynamic jsonObject = form;

                try
                {
                    email = jsonObject.Email.Value;
                }
                catch
                {
                    return BadRequest("Incorrect call.");
                }

                var user = UsersHelper.GetUserASP(email);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /*
        [HttpPost]
        [Route("LoginFacebook")]
        public IHttpActionResult LoginFacebook(FacebookResponse profile)
        {
            var user = UsersHelper.GetUserASP(profile.Id);
            if (user != null)
            {
                return Ok(true);
            }

            var userRequest = new UserRequest
            {
                EMail = profile.Id,
                FirstName = profile.FirstName,
                ImagePath = profile.Picture.Data.Url,
                LastName = profile.LastName,
                Password = profile.Id,
            };

            var answer = UsersHelper.CreateUserASP(userRequest);
            return Ok(answer);
        }*/
    }
}