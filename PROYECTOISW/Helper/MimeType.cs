namespace PROYECTOISW.Helper
{
    public static class MimeTypeHelper
    {
        public static string GetMimeType(byte[] fileData)
        {
            {
                var pngHeader = new byte[] { 137, 80, 78, 71 }; // Firma PNG
                var jpgHeader1 = new byte[] { 255, 216, 255, 224 }; // Firma JPG (start)
                var jpgHeader2 = new byte[] { 255, 216, 255, 225 }; // Firma JPG (start)
                if (fileData.Take(4).SequenceEqual(pngHeader))
                {
                    return "image/png";
                }
                else if (fileData.Take(4).SequenceEqual(jpgHeader1) || fileData.Take(4).SequenceEqual(jpgHeader2))
                {
                    return "image/jpg";
                }
                return "application/octet-stream";
            }
        }
    }
}
