using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web;
using System.Threading.Tasks;
using WEbAPiIdentity.Models;

namespace ImagesInWebAPIDemo.Controllers
{
    public class ImagesController : ApiController
    {
        public List<int> Get()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var data = from i in db.Image
                       select i.Id;
            return data.ToList();
        }

        public HttpResponseMessage Get(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var data = from i in db.Image
                       where i.Id == id
                       select i;
            Image img = (Image)data.SingleOrDefault();
            byte[] imgData = img.ImageData;
            MemoryStream ms = new MemoryStream(imgData);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
            return response;

        }

        public Task<IEnumerable<string>> Post()
        {
            if (Request.Content.IsMimeMultipartContent())
            {

                string fullPath = HttpContext.Current.Server.MapPath("~/images");
                MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(fullPath);
                var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);
                    var fileInfo = streamProvider.FileData.Select(i =>
                    {
                        var name = streamProvider.FileData.Select(k => k.Headers.ContentDisposition.FileName);
                        
                        var  info = new FileInfo(i.LocalFileName);
                        
                        ApplicationDbContext db = new ApplicationDbContext();
                        Image img = new Image();
                        img.ImageData = File.ReadAllBytes(info.FullName);
                        db.Image.Add(img);
                        db.SaveChanges();
                        return "File uploaded successfully!";
                    });
                    return fileInfo;
                });
                return task;
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid Request!"));
            }
        }

    }
}