using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    public class ChannelRepository : StandardRepository<Channel>, IChannelRepository
    {
        public ChannelRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}