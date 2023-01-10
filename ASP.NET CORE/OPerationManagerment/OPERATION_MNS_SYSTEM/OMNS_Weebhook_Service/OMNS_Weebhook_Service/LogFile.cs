﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMNS_Weebhook_Service
{
    public static class LogFile
    {

        public static void WriteTimeLog()
        {
            StreamWriter sw = null;
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\LogFile_OMNS.txt";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                sw = new StreamWriter(path,true);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd"));
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }

        public static string ReadDBNameLog()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\LogFile_OMNS.txt";
            string time = "";
            if (File.Exists(path))
            {
                time = File.ReadAllText(path);
            }

            return time;
        }

        public static string ReadTimeLog()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\LogFile_OMNS.txt";
            string time = "";
            if (File.Exists(path))
            {
               time = File.ReadAllText(path);
            }

            return time;
        }

        public static void WriteErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile_OMNS.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }

        public static void WriteErrorLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile_OMNS.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
    }
}