using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Aspose.Storage.Cloud.Sdk.Api;
using Aspose.Pdf;
using Aspose.Pdf.Cloud.Sdk.Client;
using Aspose.Pdf.Cloud.Sdk.Model;
using Aspose.Storage.Cloud.Sdk.Model.Requests;
using NUnit.Framework;
using iTextSharp.text.pdf;
using Aspose.Pdf.Cloud.Sdk.Api;
using Aspose.Storage.Cloud.Sdk.Model;

namespace PdfForm
{
    [TestFixture]
    public class AsposePdfTest
    {
      
        [Test]
        public void UploadFormToCloud_GivenFileNameAndPath_ShouldUploadAPdfDocumentToCloud()
        {
            //Arrange
            var fileName = "EditableForm1.pdf";
            var baseDirectory = TestContext.CurrentContext.TestDirectory;
            var pdfPath = Path.Combine(baseDirectory, fileName);

            var sut = CreateAsposePdfForm();
            //Act
            var actual =sut.UploadPdf(pdfPath,fileName);

            //Assert
            Assert.AreEqual(200, actual.Code);
        }
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void GetAFile_GivenInvalidFileName_ShouldReturnErrormessageOfPdfNotFound(string fileName)
        {
            //Arrange
            var asposePdfForm = CreateAsposePdfForm();
            //Act
            var actual = asposePdfForm.UpdatePdf(fileName);

            //Assert
            Assert.AreEqual("Page Not Found", actual);
        }
        [Test]
        public void UpdateSpecificField_GivenFirstNameField_ShouldUpdatePdfWithNewFieldValue()
        {

            //Arrange
            string fileName = "EditableForm1.pdf";
            var fieldname = "FirstName";

            var asposePdfForm = CreateAsposePdfForm();
            Field body = new Field
            {
                Name = "FirstName",
                Values = new List<string> { "Sindy" }
            };

            //Act
            var actual = asposePdfForm.UpdateSpecificField(fileName, fieldname,body);

            //Assert
            Assert.AreEqual("OK",actual.Status);
        }
       
        [Test]
        public void UpdateFormPdf_GivenMultipleFieldValues_ShouldUpdatePdfWithNewValues()
        {
            //Arrange
            var asposePdfForm = CreateAsposePdfForm();
            var fileName = "EditableForm2.pdf";
            Fields body = GetMultipleValuesToBeUpdated();

            //Act
            var actual = asposePdfForm.UpdateMultipleValues(fileName, body);

            //Assert
            Assert.AreEqual("OK", actual.Status);
        }
      
        [Test]
        public void DisableFormFields_GivenValuesToDisable_ShouldAvoidValuesFromBeingEditable()
        {
          
            //Arrange
            var formFile ="EditableForm2.pdf";
            var newFile = "NewEditableForm.pdf";
            var baseDirectory = TestContext.CurrentContext.TestDirectory;
            var oldPath = Path.Combine(baseDirectory, formFile);
            var newPath = Path.Combine(baseDirectory, newFile);

            var reader = new PdfReader(oldPath);

            var sut = CreateAsposePdfForm();
            var fieldToSetToReadOnly = new List<string>{ "FirstName","Surname","DateOfBirth" };

            //Act
            sut.DisableFormFields(reader, newPath, fieldToSetToReadOnly);
            //Assert
            Assert.AreNotSame(formFile,newFile);

        }

        [Test]
        public void DownloadPdf_GivenFileNameAndPath_ShouldDownloadExistingPdfFromCloud()
        {
            //Arrange
            var fileName = "EditableForm1.pdf";
            var baseDirectory = TestContext.CurrentContext.TestDirectory;
            var path = Path.Combine(baseDirectory, fileName);

            var asposePdfForm = CreateAsposePdfForm();
            //Act
            var actual = asposePdfForm.DownloadPdf(path, fileName);

            //Assert
            Assert.AreEqual(6179728, actual.Length);

        }

        [Test]
        public void DeletePdf_GivenADirectory_ShouldReturnAvailableDocuments()
        {
            //Arrange
            var fileName = "EditableForm1.pdf";
            
            var asposePdfForm = CreateAsposePdfForm();
            //Act
            var actual = asposePdfForm.DeletePdf(fileName);

            //Assert
            Assert.AreEqual("OK",actual.Status);
        }
       
        public AsposePdfFormProcessor CreateAsposePdfForm()
        {
            return new AsposePdfFormProcessor();
        }
        private static Fields GetMultipleValuesToBeUpdated()
        {
            return new Fields()
            {
                List = new List<Field>
                {
                    new Field()
                    {
                        Name = "FirstName",
                        Values = new List<string> {"Zinhle"}
                    },
                    new Field()
                    {
                        Name = "Surname",
                        Values = new List<string> {"Pantshwa"}
                    },
                    new Field()
                    {
                        Name = "DateOfBirth",
                        Values = new List<string> {"1998-10-04"}
                    },
                    new Field()
                    {
                        Name = "GrossSalary",
                        Values = new List<string> {"25000"}
                    },
                    new Field()
                    {
                        Name = "Tax",
                        Values = new List<string> {"3000"}
                    },
                    new Field()
                    {
                        Name = "Accomodation",
                        Values = new List<string> {"4000"}
                    },
                    new Field()
                    {
                        Name = "CellPhone",
                        Values = new List<string> {"350"}
                    },
                    new Field()
                    {
                        Name = "CreditCard",
                        Values = new List<string> {"2600"}
                    },
                    new Field()
                    {
                        Name = "OtherDept",
                        Values = new List<string> {"2100"}
                    }
                }
            };
        }
    }
}