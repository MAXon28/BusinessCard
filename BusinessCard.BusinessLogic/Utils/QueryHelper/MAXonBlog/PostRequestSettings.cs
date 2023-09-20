using BusinessCard.BusinessLogicLayer.Utils.Enums;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    internal class PostRequestSettings : RequestSettings
    {
        public PostRequestSettings(
            int lastPostId,
            int postsCountInPackage,
            string searchText,
            int userId,
            int channelId,
            int offset,
            PostRequestTypes requestType) : base(lastPostId, postsCountInPackage, searchText)
        {
            UserId = userId;
            ChannelId = channelId;
            Offset = offset;
            RequestType = requestType;
        }

        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// 
        /// </summary>
        public int ChannelId { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// 
        /// </summary>
        public PostRequestTypes RequestType { get; }
    }
}