using Microsoft.EntityFrameworkCore;
using Project.Domain.Models;
using System;
using System.Linq;

namespace project.Web
{
    public class DatabaseCreator
    {
        protected ProjectContext _dbContext;

        public DatabaseCreator(ProjectContext projectContext)
        {
            this._dbContext = projectContext;
        }

        public void CreateDatabase()
        {
        }
    }
}
