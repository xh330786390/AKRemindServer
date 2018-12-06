using AKRemindServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace AKRemindServer.Common
{
    public class FileHelper
    {
        private UTF8Encoding UTF8 = new System.Text.UTF8Encoding(false);

        private static string projectPath = System.AppDomain.CurrentDomain.BaseDirectory;
        private string fileName = "ak.json";

        private string filePath = string.Empty;
        private static FileHelper _instance = null;
        private static readonly object obj = new object();

        private string path = string.Empty;

        public FileHelper()
        {
            path = projectPath + @"\data";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            filePath = Path.Combine(path, "ak.json");
        }


        public static FileHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        _instance = new FileHelper();
                    }
                }
                return _instance;
            }
        }

        public void Write(string json)
        {
            File.WriteAllText(filePath, json, this.UTF8);
        }

        public AkSettingModel Read()
        {
            AkSettingModel model = new AkSettingModel();
            model.PeriodNum = 7;
            model.UpdateNum = 7;

            string json = string.Empty;
            if (File.Exists(filePath))
            {
                try
                {
                    json = File.ReadAllText(filePath, this.UTF8);
                    if (!string.IsNullOrEmpty(json))
                    {
                        model = JsonConvert.DeserializeObject<AkSettingModel>(json);
                    }
                }
                catch (Exception)
                {
                }
            }

            AkConfig.UpdateNum = model.UpdateNum;
            AkConfig.PeriodNum = model.PeriodNum;
            return model;
        }
    }
}
