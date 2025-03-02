using Microsoft.AspNetCore.Mvc;
using taller_be.Models;

namespace taller_be
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListasController : ControllerBase
    {
        [HttpGet]
        [Route("otros")]
        public APIResult API_getAbout()
        {
            APIResult res = new APIResult();
            try
            {
                res.status = APIStatus.ok;
                res.data = ServiceCMS.getListaURL(); // obtengo los datos para luego enviarselos al front
                if(res.data == null)
                {
                    
                    res.msg = "No hay rutas";
                }
                else
                {
                    res.msg = "Rutas obtenidas correctamente.";
                }
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
