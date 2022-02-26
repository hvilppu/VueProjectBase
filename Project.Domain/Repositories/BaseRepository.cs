using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Project.Common.Services;
using Project.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Project.Domain.Repositories
{
    public class BaseRepository : IDisposable
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected ProjectContext _dbContext;
        protected HttpContext _httpContext;


        public BaseRepository(ProjectContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;

            // Can be null in test cases
            if (httpContextAccessor != null)
                _httpContext = httpContextAccessor.HttpContext;
        }


        protected Users currentUser
        {
            get { return this._dbContext.Users.Where(u => u.Id == this.userId).FirstOrDefault(); }
        }

        protected int? userId
        {
            get
            {
                if (_httpContext == null)
                    return null;

                var userIdRaw =
                    _httpContext.User.Claims.FirstOrDefault(c => c.Type == AuthenticationService.CLAIM_USER_ID);
                if (userIdRaw != null)
                    return int.Parse(userIdRaw.Value);

                return null;
            }
        }

        protected string userName
        {
            get
            {
                if (_httpContext == null)
                    return null;

                var userIdRaw =
                    _httpContext.User.Claims.FirstOrDefault(c => c.Type == AuthenticationService.CLAIM_USER_NAME);
                if (userIdRaw != null)
                    return userIdRaw.Value;

                return null;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _dbContext = null;
        }

        // <summary>
        /// Default EF Add
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="commit"></param>
        public T Add<T>(IModel entity, bool commit = true) where T : IModel
        {
            if (userId != null && userId > 0)
                Timestamp.SetAddTimestamp(entity, userName);

            _dbContext.Add(entity);

            if (commit)
                _dbContext.SaveChanges();

            return (T) entity;
        }

        // <summary>
        /// Default EF Add for non IModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public object AddDefault<T>(object entity)
        {
            _dbContext.Add(entity);
            _dbContext.SaveChanges();
            return (T) entity;
        }

        public static T Clone<T>(T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        /// <summary>
        /// Default EF Remove
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Remove<T>(object entity)
        {
            _dbContext.Remove(entity);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Default EF Remove Logical
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void RemoveLogical<T>(IModel entity)
        {
            if (entity == null) return;

            entity.Deleted = true;

            if (userId != null && userId > 0)
                Timestamp.SetRemoveTimestamp(entity, userName);

            _dbContext.Update(entity);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Default EF Update
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public T Update<T>(IModel entity, bool skipStamp = false)
        {
            if ((userId != null && userId > 0) || skipStamp)
                Timestamp.SetUpdateTimestamp(entity, userName);

            entity.Active ??= true;

            _dbContext.Update(entity);
            _dbContext.SaveChanges();

            return (T) entity;
        }

        // <summary>
        /// Default EF Update for non IModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public object UpdateDefault<T>(object entity)
        {
            _dbContext.Update(entity);
            _dbContext.SaveChanges();
            return (T) entity;
        }

        public string UserString(Users user)
        {
            if (user != null)
                return "UserID: " + user.Id.ToString();

            else
            {
                Log.Error("Unknown user at EntityProvider.Instance.UserString(User user)");
                return "Unknown user";
            }
        }

        public string StringAsCulture(IStringLocalizer localizer, string culture, string key)
        {
            try
            {
                return localizer.WithCulture(new CultureInfo(culture))[key];
            }

            catch
            {
                return key;
            }
        }
    }
}