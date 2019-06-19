using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;


namespace MySimpleDuplicateFileFinder
{
    public class HashesCalculator 
    {
        private List<Task<Tuple<string,string>>> _tasks = new List<Task<Tuple<string, string>>>();
        
        public void AddCalculateTask(string filename)
        {
            _tasks.Add(Task.Factory.StartNew((path) =>
            {
                try
                {
                    string filePath = path as string;
                    return new Tuple<string, string>(CalculateMD5(filePath), filePath);
                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }, filename));
        }

        public Dictionary<string , string> GetCalculationResults()
        {
            var result = new Dictionary<string, string>();
            Task.WaitAll(_tasks.ToArray());
            foreach(var task in _tasks)
            {
                var taskResult = task.GetAwaiter().GetResult();
                if(taskResult != null) result[taskResult.Item2] = taskResult.Item1;
            }
            _tasks = new List<Task<Tuple<string, string>>>();
            return result;
        }

        public static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
