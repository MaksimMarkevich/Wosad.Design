﻿#region Copyright
   /*Copyright (C) 2015 Wosad Inc

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
   */
#endregion
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Wosad.Common.CalculationLogger.Interfaces;


namespace Wosad.Reporting.ResultBuilder
{
    public class FileLocator
    {
        public string GetResourcePath(ICalcLogEntry Entry)
        {
            //this method should locate the template file based on the 
            // ICalcLogEntry reference and formula ID
            string Reference = Entry.DescriptionReference;
            string FormulaId = Entry.FormulaID;
            // implement here ....
            throw new NotImplementedException();
        }

        //this is an example of finding the path ...

        private static string getTemplateDirectory(string calcName)
        {
            //1. Get the full path to the executable DLL
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            //2. Cut the full path up to the /WosadBackEnd/
            var matches = Regex.Matches(codeBase, @"(([^\.]+)\/WosadBackEnd/)");
            string matched = "";
            foreach (var match in matches)
            {
                matched = match.ToString();
            }

            //3. Add subdirector and a calc name
            string newPath = matched + "/" + "WosadCalculators" + "/" + calcName + "/";
            //4. Build the path
            UriBuilder uri = new UriBuilder(newPath);
            string path = Uri.UnescapeDataString(uri.Path);
            string mappingDir = Path.GetDirectoryName(path) + "\\Resources\\Output\\";

            return mappingDir;
        }


    }
}
