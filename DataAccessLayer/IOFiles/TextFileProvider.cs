using DataAccessLayer.Utils.Interfaces;
using System;
using System.IO;
using System.Threading;

namespace DataAccessLayer.IOFiles
{
    public class TextFileProvider : ITextFileProvider
    {
        private static readonly SemaphoreSlim _semaphore6 = new SemaphoreSlim(1, 1);

        public bool WriteToTextFile(string fullFilePath, string htmlFileContent)
        {
            _semaphore6.Wait();
            try
            {
                File.WriteAllText(fullFilePath, htmlFileContent);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally { _semaphore6.Release(); }
        }

        public string ReadFromTextFile(string fullFilePath)
        {
            _semaphore6.Wait();
            try
            {
                string content = (string.IsNullOrEmpty(fullFilePath)) ? string.Empty : File.ReadAllText(fullFilePath);
                return content;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            finally { _semaphore6.Release(); }
        }
    }
}
