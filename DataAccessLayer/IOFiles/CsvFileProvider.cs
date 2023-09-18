using CsvHelper;
using DataAccessLayer.IOFiles.Models;
using DataAccessLayer.Utils.Interfaces;
using System.Globalization;
using System.IO;
using System.Threading;
using System;

namespace DataAccessLayer.IOFiles
{
    public class CsvFileProvider : ICsvFileProvider
    {
        private static readonly SemaphoreSlim _semaphore7 = new SemaphoreSlim(1, 1);


        public bool CreateCsvFile(string fileFullPathName)
        {
            _semaphore7.Wait();
            try
            {
                using (FileStream fileStream = new FileStream(fileFullPathName, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        writer.Flush();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally { _semaphore7.Release(); }
        }


        public bool CreateHeaderCsvFile(string fileFullPathName)
        {
            _semaphore7.Wait();
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(fileFullPathName))
                {
                    using (CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                    {
                        csvWriter.WriteHeader<CsvDataFormat>();
                        csvWriter.NextRecord();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally { _semaphore7.Release(); }
        }

        public bool AddSingleRecordToCsvFile(string fileFullPathName, string record)
        {
            _semaphore7.Wait();
            try
            {
                using (FileStream fileStream = new FileStream(fileFullPathName, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        if (record != string.Empty)
                        {
                            streamWriter.WriteLine(record);
                        }

                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally { _semaphore7.Release(); }
        }
    }
}
