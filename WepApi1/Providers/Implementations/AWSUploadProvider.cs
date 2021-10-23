using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace WepApi1.Providers.Implementations
{
    public class AWSUploadProvider : FileUploader
    {
        AmazonS3Client client;

        public AWSUploadProvider()
        {
            this.client = new AmazonS3Client();
        }

        public async Task<IUploadedFile> upload(IFile file)
        {
            string key = generateKey(16, file.name);

            PutObjectRequest putRequest = new PutObjectRequest();
            putRequest.BucketName = "aleatshop";
            putRequest.Key = key;
            putRequest.InputStream = file.content;
            putRequest.ContentType = file.type;
            putRequest.CannedACL = S3CannedACL.PublicRead;

            await client.PutObjectAsync(putRequest);

            IUploadedFile fileUploaded = new IUploadedFile();
            fileUploaded.Key = key;
            fileUploaded.Location = $"https://aleatshop.s3.sa-east-1.amazonaws.com/{key}";

            return fileUploaded;
        }

        public async Task<IUploadedFile> update(IUpdateFile file)
        {
            PutObjectRequest putRequest = new PutObjectRequest();
            putRequest.BucketName = "aleatshop";
            putRequest.Key = file.key;
            putRequest.InputStream = file.content;
            putRequest.ContentType = file.type;
            putRequest.CannedACL = S3CannedACL.PublicRead;

            await client.PutObjectAsync(putRequest);

            IUploadedFile fileUploaded = new IUploadedFile();
            fileUploaded.Key = file.key;
            fileUploaded.Location = $"https://aleatshop.s3.sa-east-1.amazonaws.com/{file.key}";

            return fileUploaded;
        }

        public async Task<string> delete(string key)
        {
            DeleteObjectRequest deleteRequest = new DeleteObjectRequest();
            deleteRequest.BucketName = "aleatshop";
            deleteRequest.Key = key;

            await client.DeleteObjectAsync(deleteRequest);
            return key;
        }

        private string generateKey(int tamanho, string fileName)
        {
            var random = new Random();
            var hash = new string(
                Enumerable.Repeat(fileName, tamanho)
                    .Select(s => s[random.Next(s.Length)])
                    .ToArray());


            return $"{hash}-{fileName}";
        }
    }
}
