using BWJ.Web.OTM.Exceptions;
using BWJ.Web.OTM.Internal.Http;
using BWJ.Web.OTM.Models.Request.Tools.Admin;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace BWJ.Web.OTM.Topic.Tools
{
    public interface IAdminTopic
    {
        Task<Stream> GetAddressBackup(string session);
        Task RestoreAddressBackup(byte[] fileData, string session);
    }

    internal class AdminTopic : IAdminTopic
    {
        private readonly OtmHttpClient _client;

        internal AdminTopic(OtmHttpClient client)
        {
            _client = client;
        }

        public async Task<Stream> GetAddressBackup(string session)
        {
            if (string.IsNullOrWhiteSpace(session))
            {
                throw new ArgumentException(nameof(session));
            }

            var response = await _client
                .Get("Backup")
                .IncludeQuery(new GetBackupQuery { what = "A" })
                .IncludeSession(session)
                .SendAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
            else
            {
                throw new StatusCodeUnexpectedException(response.StatusCode, response.RequestMessage.RequestUri.ToString(), string.Empty);
            }
        }

        public async Task RestoreAddressBackup(byte[] fileData, string session)
        {
            if ((fileData?.Length ?? 0) == 0)
            {
                throw new ArgumentNullException(nameof(fileData));
            }
            if (string.IsNullOrWhiteSpace(session))
            {
                throw new ArgumentException(nameof(session));
            }

            var request = new RestoreAddressBackupRequest();
            request.newaddrfile.Content = fileData;

            var response = await _client
                .Post("RestoreBackupAdmin")
                .Form(request)
                .IncludeSession(session)
                .SendAsync();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new StatusCodeUnexpectedException(response.StatusCode, response.RequestMessage.RequestUri.ToString(), string.Empty);
            }
        }
    }
}
