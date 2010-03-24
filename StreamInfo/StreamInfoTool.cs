﻿//BluRip - one click BluRay/m2ts to mkv converter
//Copyright (C) 2009-2010 _hawk_

//This program is free software; you can redistribute it and/or
//modify it under the terms of the GNU General Public License
//as published by the Free Software Foundation; either version 2
//of the License, or (at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program; if not, write to the Free Software
//Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

//Contact: hawk.ac@gmx.net

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace BluRip
{
    public class StreamInfoTool : ExternalTool
    {
        private UserSettings settings = null;
        private List<TitleInfo> result = null;
        private string titlePath = "";
        private List<string> videoTypes = null;
        private List<string> ac3AudioTypes = null;
        private List<string> dtsAudioTypes = null;

        public StreamInfoTool(UserSettings settings, ref List<TitleInfo> result, string path, List<string> videoTypes, List<string> ac3AudioTypes, List<string> dtsAudioTypes)
            : base()        
        {
            try
            {
                this.titlePath = path;
                this.settings = settings;
                result = new List<TitleInfo>();
                this.result = result;
                this.Path = settings.eac3toPath;
                this.Parameter = "\"" + titlePath + "\"";
                this.videoTypes = videoTypes;
                this.ac3AudioTypes = ac3AudioTypes;
                this.dtsAudioTypes = dtsAudioTypes;
            }
            catch (Exception)
            {
            }
        }

        protected override void StartInfo()
        {
            Info("Getting playlist info...");
            Info("");
        }

        protected override void EndInfo()
        {
            Info("Done.");
        }

        private void InfoMsg(object sender, MsgEventArgs e)
        {
            Info(e.Message);
        }

        private void LogMsg(object sender, MsgEventArgs e)
        {
            Log(e.Message);
        }

        protected override bool StartProcessing()
        {
            if (Ok && ExitCode == 0) return true;
            else return false;
        }

        protected override void Process()
        {
            try
            {
                string res = Output;

                res = res.Replace("\b", "");

                string[] tmp = res.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < tmp.Length; i++)
                {
                    tmp[i] = tmp[i].Trim();
                }
                for (int i = 0; i < tmp.Length; i++)
                {
                    if (Regex.IsMatch(tmp[i], "^[0-9.*].*\\)"))
                    {
                        string[] tmp2 = tmp[i].Split(new char[] { ')' }, StringSplitOptions.RemoveEmptyEntries);
                        SubStreamInfoTool ssit = new SubStreamInfoTool(settings, ref result, titlePath, tmp2[0], videoTypes, ac3AudioTypes, dtsAudioTypes);
                        ssit.OnInfoMsg += new InfoEventHandler(InfoMsg);
                        ssit.OnLogMsg += new LogEventHandler(LogMsg);
                        ssit.Start();
                        ssit.WaitForExit();
                        if (!ssit.Successfull) return;
                    }
                }
                successfull = true;
            }
            catch (Exception)
            {                
            }
        }
    }
}