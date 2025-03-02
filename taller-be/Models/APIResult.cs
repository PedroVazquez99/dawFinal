namespace taller_be.Models
{

    public enum APIStatus { ok, err };

    public class APIResult
    {

        public APIStatus status { get; set; }

        public string msg { get; set; }

        public Object data { get; set; }

        public APIResult()
        {
            status = APIStatus.ok;
            msg = "";
            data = null;
        }
    }

}
