using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Twitter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly ILogger<UploadController> logger;
        public UploadController(ILogger<UploadController> logger)
        {
            this.logger = logger;
        }
        /// <summary>
        /// Upload photo
        /// </summary>
        /// <returns></returns>
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("[Message] : " + ex.Message);
                logger.LogInformation("[In method] : " + ex.TargetSite);
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete]
        public ActionResult DeletePhoto(string path)
        {
            path = path.Substring(path.LastIndexOf('/') + 1);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            return Ok();
        }
    }
}
