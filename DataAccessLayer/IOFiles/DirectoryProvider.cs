using DataAccessLayer.Utils.Interfaces;
using System;
using System.IO;
using System.Threading;

namespace DataAccessLayer.IOFiles
{
    public class DirectoryProvider : IDirectoryProvider
    {
        private static readonly SemaphoreSlim _semaphore9 = new SemaphoreSlim(1, 1);


        public bool CreateDirectoryIfNotExist(string directoryFullPath)
        {
            _semaphore9.Wait();
            try
            {
                if (!Directory.Exists(directoryFullPath))
                {
                    Directory.CreateDirectory(directoryFullPath);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally { _semaphore9.Release(); }
        }

        public string[] DirectoryGetFiles(string filePath)
        {
            _semaphore9.Wait();
            try
            {
                string[] allFiles = Directory.GetFiles(filePath);
                return allFiles;
            }
            catch (Exception ex)
            {
                return new string[0];
            }
            finally { _semaphore9.Release(); }
        }

        public string[] DirectoryGetDirectories(string folderFullPath)
        {
            _semaphore9.Wait();
            try
            {
                string[] allDirectories = Directory.GetDirectories(folderFullPath);
                return allDirectories;
            }
            catch (Exception ex)
            {
                return new string[0];
            }
            finally { _semaphore9.Release(); }
        }
    }
}
