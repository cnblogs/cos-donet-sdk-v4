using System;
using QCloud.CosApi.Common;
using QCloud.CosApi.Api;
using System.Collections.Generic;

namespace QCloud.CosApi.Demo
{
    [Obsolete]
    class Program
    {
        const int APP_ID = 1253895404;
        const string SECRET_ID = "AKID2vb4LPlrGSQAi3G4kEVZP2SfeQyJsN6b";
        const string SECRET_KEY = "OZXIqAICsLbeT4SdaJp7C9sQkqiOr67v";

        static void Main(string[] args)
        {
            try
            {
                var result = "";

                const string bucketName = "cnblogsimages2018";
                const string localPath = @"C:\temp\logo.png";
                const string remotePath = "/test/logo.png";
                const string folder = "/sdktest/";

                //创建cos对象
                var cos = new CosCloud(APP_ID, SECRET_ID, SECRET_KEY);
                

                //上传文件（不论文件是否分片，均使用本接口）
                var uploadParasDic = new Dictionary<string, string>();
                //uploadParasDic.Add(CosParameters.PARA_BIZ_ATTR, "");
                //uploadParasDic.Add(CosParameters.PARA_INSERT_ONLY, "0");
                //uploadParasDic.Add(CosParameters.PARA_SLICE_SIZE,SLICE_SIZE.SLIZE_SIZE_3M.ToString());
                result = cos.UploadFile(bucketName, remotePath, localPath, uploadParasDic);
                Console.WriteLine("上传文件:" + result);                

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //Console.ReadKey();
        }
    }
}
