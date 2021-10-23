using BWJ.Net.Http.RequestObject;

namespace BWJ.Web.OTM.Models.Request.Tools.Admin
{
    public class RestoreAddressBackupRequest
    {
        public FileContent newaddrfile { get; } = new FileContent { ContentType = "application/vnd.ms-excel" };
        public string Upload { get; } = "Upload";
    }
}
