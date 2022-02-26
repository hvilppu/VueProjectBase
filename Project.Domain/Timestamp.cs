using System;
namespace Project.Domain
{
    public static class Timestamp
    {
        /// <summary>
        /// Get current UTC Timestamp
        /// </summary>
        /// <returns></returns>
        public static DateTime GetTS()
        {
            return DateTime.UtcNow;
        }

        /// <summary>
        /// Set creation and update timestamps for new IModel
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static void SetAddTimestamp(IModel entity, string userName)
        {
            var TS = GetTS();
            entity.CreationDate = TS;
            entity.Modifier = userName;
        }

        /// <summary>
        /// Set update timestamp for IModel
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userName"></param>
        public static void SetUpdateTimestamp(IModel entity, string userName)
        {
            entity.LastModified = GetTS();
            entity.Modifier = userName;
        }

        /// <summary>
        /// Set remove timestamp for IModel
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userName"></param>
        public static void SetRemoveTimestamp(IModel entity, string userName)
        {
            entity.LastModified = GetTS();
            entity.Modifier = userName;
        }
    }
}