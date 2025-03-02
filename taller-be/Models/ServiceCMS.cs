namespace taller_be.Models
{
    public class ServiceCMS
    {
        public static object getAbout()
        {
            Dictionary<string, string> about = new Dictionary<string, string>();
            about.Add("titulo", Resources.Textos.about_titulo);
            about.Add("texto", Resources.Textos.about_texto);
            return about;
        }

        public static object getListaURL()
        {

            Dictionary<string, string> listado = new Dictionary<string, string>();
            listado.Add("Universidad de Alicante", "https://www.ua.es/");
            listado.Add("Universidad de Valencia", "https://www.uv.es/");
            listado.Add("Universidad de Castellón", "https://www.uji.es/");
            
            return listado;
        }
    }
}
