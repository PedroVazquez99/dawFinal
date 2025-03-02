using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using taller_be.Models;

namespace taller_be
{
    [Route("api/[controller]")]
    [ApiController]
    public class CMSController : ControllerBase
    {
        [HttpGet]
        [Route("about")]

        public APIResult API_getAbout()
        {
            APIResult res = new APIResult();
            try
            {
                res.status = APIStatus.ok;
                res.data = ServiceCMS.getAbout(); // obtengo los datos para luego enviarselos al front
                res.msg = "";
            }
            catch (Exception ex)
            {
                res.status = APIStatus.err;
                res.data = null;
                res.msg = ex.Message;
            }
            return res; // devuelvo los datos al front 
        }
    }
}
