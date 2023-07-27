using System.Net;

namespace EspacioAscii
{
    class asciiApi
    {
        //api devuelve un string
        public string obtenerTextoAscii(string text,string font)
        {
            var url = $"https://asciified.thelicato.io/api/v2/ascii?text={text}&font={font}";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return text;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string textoAscii = objReader.ReadToEnd();
                            return textoAscii;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                return text;
            }
        }
    }
}