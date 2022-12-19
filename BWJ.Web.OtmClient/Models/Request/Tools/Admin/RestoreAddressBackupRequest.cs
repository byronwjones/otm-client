using BWJ.Net.Http.RequestObject;

namespace BWJ.Web.OTM.Models.Request.Tools.Admin
{
    internal class RestoreAddressBackupRequest
    {
        public FileContent newaddrfile { get; } = new FileContent { ContentType = "application/vnd.ms-excel", FileName = "backup.csv" };
        public string Upload { get; } = "Upload";
    }
}
