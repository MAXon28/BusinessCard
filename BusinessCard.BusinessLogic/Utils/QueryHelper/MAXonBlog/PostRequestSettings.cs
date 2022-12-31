namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    internal class PostRequestSettings : IRequestSettings
    {
        internal PostRequestSettings(int userId, int channelId, int offset, string searchText, PostRequestTypes requestType)
        {
            ForAuthorizedUser = userId != -1;
            UserId = userId;
            ChannelId = channelId;
            Offset = offset;
            WithSearchText = !string.IsNullOrEmpty(searchText);
            SearchText = searchText;
            RequestType = requestType;
        }

        /// <summary>
        /// 
        /// </summary>
        internal bool ForAuthorizedUser { get; }

        /// <summary>
        /// 
        /// </summary>
        internal int UserId { get; }

        /// <summary>
        /// 
        /// </summary>
        internal int ChannelId { get; }

        /// <summary>
        /// 
        /// </summary>
        internal int Offset { get; }

        /// <summary>
        /// 
        /// </summary>
        internal bool WithSearchText { get; }

        /// <summary>
        /// 
        /// </summary>
        internal string SearchText { get; }

        /// <summary>
        /// 
        /// </summary>
        internal PostRequestTypes RequestType { get; }
    }
}