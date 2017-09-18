using System.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RenameFiles.Models;

namespace RenameFiles.Managers
{
    public class CompanyStamp_tabManager
    {
        private renamefilesEntities db = new renamefilesEntities();
        public CompanyStamp_tab Find(string filenumber)
        {
            return db.CompanyStamp_tab.FirstOrDefault(model => model.FileNumber == filenumber);
        }
    }
}