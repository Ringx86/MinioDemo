using Minio;
using Minio.DataModel;
using System;
using System.Linq;

namespace MinioDemo
{
    class Program
    {
        private static MinioClient minio = new MinioClient("127.0.0.1:9000", "admin", "password");
        static void Main(string[] args)
        {
            
            try
            {
                var bucketList = minio.ListBucketsAsync().Result;
                //获取分类列表
                foreach (Bucket bucket in bucketList.Buckets)
                {
                    Console.WriteLine("-" + bucket.Name + " " + bucket.CreationDateDateTime);
                    //获取文件列表
                    minio.ListObjectsAsync(bucket.Name).Subscribe(new MyObserver());
                    
                }

                Console.Read();

                //上传文件
                var bucketName = bucketList.Buckets.FirstOrDefault().Name;
                /*

                var fileName = "test.txt";
                var filePath = @"D:\MinIO\test\test.txt";
                var contentType = "";
                minio.PutObjectAsync(bucketName, fileName, filePath).GetAwaiter().GetResult();
                Console.WriteLine("Successfully uploaded " + fileName);
                */

                //bucket policy
                var policy = minio.GetPolicyAsync(bucketName).Result;

                
                Console.WriteLine(policy);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.Read();
        }
        
    }

    public class MyObserver : IObserver<Item>
    {
        public void OnCompleted()
        {
            Console.WriteLine("Observer Completed");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("Observer Error" + error.Message);
        }

        public void OnNext(Item value)
        {
            Console.WriteLine("---" + value.Key + " " + value.LastModifiedDateTime + " Expired Time: ");
        }
    }
}
