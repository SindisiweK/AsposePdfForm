using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using Aspose.Pdf.Cloud.Sdk.Api;
using Aspose.Pdf.Cloud.Sdk.Model;
using Aspose.Storage.Cloud.Sdk.Api;
using Aspose.Storage.Cloud.Sdk.Model;
using Aspose.Storage.Cloud.Sdk.Model.Requests;
using iTextSharp.text.pdf;
using RestSharp.Extensions;

namespace PdfForm
{
    public  class AsposePdfFormProcessor
    {
        public readonly string _apiKey;
        public readonly string _appSid;

      
        public AsposePdfFormProcessor()
        {
            _apiKey = ConfigurationManager.AppSettings["APIKEY"];
            _appSid = ConfigurationManager.AppSettings["APPSID"];
        }
        public UploadResponse UploadPdf(string pdfPath, string fileName)
        {
            using (var stream = new FileStream(pdfPath, FileMode.Open))
            {
                var storageApi = new StorageApi(_apiKey, _appSid);
                var request = new PutCreateRequest(fileName, stream);
                var actual = storageApi.PutCreate(request);
                return actual;
            }
        }
        public FieldResponse UpdateSpecificField(string fileName, string fieldname, Field body)
        {
            var target = new PdfApi(_apiKey, _appSid);
            var apiResponse = target.PutUpdateField(fileName, fieldname, body);
            return apiResponse;
        }

        public   FieldsResponse UpdateMultipleValues(string fileName, Fields body)
        {
            var target = new PdfApi(_apiKey, _appSid);
            var apiResponse = target.PutUpdateFields(fileName, body);
            return apiResponse;
        }

        public  PdfStamper DisableFormFields(PdfReader reader, string newFile, List<string> ListOfFields)
        {
            using (var stamper = new PdfStamper(reader, new FileStream(newFile, FileMode.Create)))
            {
                var fields = stamper.AcroFields;

                SetReadOnly(fields,ListOfFields);

                stamper.Close();
                return stamper;
            }
        }

        public string UpdatePdf(string fileName)
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(fileName))
            {
                message = "Page Not Found";
            }
                return message;
        }
        public byte[] DownloadPdf(string pdfPath, string fileName)
        {
            dynamic bytesOfDownloadedPdf ;
            var storageApi = new StorageApi(_apiKey, _appSid);
            var request = new GetDownloadRequest(fileName);
            using (var response = storageApi.GetDownload(request))
            {
                var bytes = response.ReadAsBytes();
                File.WriteAllBytes(pdfPath,bytes);
                bytesOfDownloadedPdf  = File.ReadAllBytes(pdfPath);
            }

            return bytesOfDownloadedPdf ;
        }

        public RemoveFileResponse DeletePdf(string fileName)
        {
            
            var storageApi = new StorageApi(_apiKey, _appSid);
            var request = new DeleteFileRequest(fileName);
            var apiResponse = storageApi.DeleteFile(request);

            return apiResponse;
        }
        private static void SetReadOnly(AcroFields fields, IEnumerable<string> ListOfFields)
        {
            foreach (var field in ListOfFields)
            {
                fields.SetFieldProperty(field, "setfflags", PdfFormField.FF_READ_ONLY, null);
            }
        }
    }
}