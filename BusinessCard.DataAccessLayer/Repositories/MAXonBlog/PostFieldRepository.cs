using BusinessCard.DataAccessLayer.Entities.MAXonBlog.PostDetails;
using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonBlog
{
    internal class PostFieldRepository : StandardRepository<PostField>, IPostFieldRepository
    {
        public PostFieldRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}