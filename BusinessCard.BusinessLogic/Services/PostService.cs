using BusinessCard.BusinessLogicLayer.DTOs.Blog;
using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.BusinessLogicLayer.Utils;
using BusinessCard.BusinessLogicLayer.Utils.QueryHelper;
using BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonBlog;
using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="IPostService"/>
    public class PostService : IPostService
    {
        /// <summary>
        /// Количество постов в одном пакете (для пагинации)
        /// </summary>
        private const int PostsCountInPackage = 28;

        /// <summary>
        /// Репозиторий постов
        /// </summary>
        private readonly IPostRepository _postRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IPersonalInformationService _personalInformationService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IPostFieldRepository _postFieldRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IPostElementRepository _postElementRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly SelectionQueryBuilder _selectionQueryBuilder;

        /// <summary>
        /// Утилита для пагинации по постам
        /// </summary>
        private readonly PaginationtUtil _paginationUtil = new PaginationtUtil(PostsCountInPackage);

        public PostService(
            IPostRepository postRepository, 
            IPersonalInformationService personalInformationService, 
            IPostFieldRepository postFieldRepository,
            IPostElementRepository postElementRepository,
            ISelectionQueryBuilderFactory selectionQueryBuilderFactory)
        {
            _postRepository = postRepository;
            _personalInformationService = personalInformationService;
            _postFieldRepository = postFieldRepository;
            _postElementRepository = postElementRepository;
            _selectionQueryBuilder = selectionQueryBuilderFactory.GetQueryBuilder(QueryBuilderTypes.Posts);
        }

        public async Task<PostsInformation> GetPostsAsync(int? userId, PostFilters filters)
        {
            var postInformation = new PostsInformation();
            var postRequestSettings = GetPostRequestSettings(filters.PostsPackageNumber, filters.SearchText, filters.TypeOfRequest, userId != null ? (int)userId : -1, filters.ChannelId);

            var queryDataForPosts = _selectionQueryBuilder.GetQueryData(postRequestSettings);
            postInformation.Posts = GetPostsInDto(await _postRepository.GetPostsAsync(queryDataForPosts.SqlQuery, queryDataForPosts.Parameters));

            if (userId != null && postInformation.Posts.Count > 0)
                postInformation.PersonalInformation = await _personalInformationService.GetPersonalInformationAsync((int)userId, GetPostsId(postInformation.Posts));

            if (filters.NeedPagesCount)
            {
                _selectionQueryBuilder.TypeOfSelect = SelectTypes.Count;
                var queryDataForCount = _selectionQueryBuilder.GetQueryData(postRequestSettings);
                postInformation.PagesCount = _paginationUtil.GetPagesCount(await _postRepository.GetPostsCountAsync(queryDataForCount.SqlQuery, queryDataForCount.Parameters));
            }

            return postInformation;
        }

        public async Task<BlogInformation> GetFirstBlockOfPostsAsync(int? userId)
        {
            const int postsPackageNumber = 1;
            const string postRequestTypeStr = "ALL";
            var searchText = string.Empty;

            var postInformation = new BlogInformation();
            var postRequestSettings = GetPostRequestSettings(postsPackageNumber, searchText, postRequestTypeStr);

            var queryDataForPosts = _selectionQueryBuilder.GetQueryData(postRequestSettings);
            postInformation.Posts = GetPostsInDto(await _postRepository.GetPostsAsync(queryDataForPosts.SqlQuery, queryDataForPosts.Parameters));

            if (userId != null)
                postInformation.PersonalInformation = await _personalInformationService.GetPersonalInformationAsync((int)userId, GetPostsId(postInformation.Posts));

            _selectionQueryBuilder.TypeOfSelect = SelectTypes.Count;
            var queryDataForCount = _selectionQueryBuilder.GetQueryData(postRequestSettings);
            postInformation.PagesCount = _paginationUtil.GetPagesCount(await _postRepository.GetPostsCountAsync(queryDataForCount.SqlQuery, queryDataForCount.Parameters));

            return postInformation;
        }

        public async Task<int> GetPagesCountByChannelAsync(int channelId)
        {
            const int postsPackageNumber = 1;
            const string postRequestTypeStr = "CHANNEL";
            var searchText = string.Empty;

            var postRequestSettings = GetPostRequestSettings(postsPackageNumber, searchText, postRequestTypeStr, channelId: channelId);
            _selectionQueryBuilder.TypeOfSelect = SelectTypes.Count;
            var queryData = _selectionQueryBuilder.GetQueryData(postRequestSettings);
            return _paginationUtil.GetPagesCount(await _postRepository.GetPostsCountAsync(queryData.SqlQuery, queryData.Parameters));
        }

        public IEnumerable<int> GetPostsId(IEnumerable<PostDto> posts)
            => posts.Select(post => post.Id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postsPackageNumber">  </param>
        /// <param name="searchText">  </param>
        /// <param name="postRequestTypeStr">  </param>
        /// <param name="userId">  </param>
        /// <param name="channelId">  </param>
        /// <returns>  </returns>
        private PostRequestSettings GetPostRequestSettings(int postsPackageNumber, string searchText, string postRequestTypeStr, int userId = -1, int channelId = -1)
        {
            var offset = _paginationUtil.GetOffset(postsPackageNumber);
            var postRequestType = postRequestTypeStr.ToEnum<PostRequestTypes>();
            return new PostRequestSettings(userId, channelId, offset, searchText, postRequestType);
        }

        public async Task<PostInformation> GetPostInformationAsync(int? userId, string postKey)
        {
            var post = await _postRepository.GetPostAsync(postKey);
            var postDetails = await GetPostDetailsAsync(postKey);
            var personalInformation = userId is null ? null : await _personalInformationService.GetPersonalInformationAsync((int)userId, new List<int> { post.Id });

            return new PostInformation
            {
                Post = GetPostInDto(post),
                PostDetails = postDetails,
                PersonalInformation = personalInformation
            };
        }

        public List<PostDto> GetPostsInDto(IEnumerable<Post> posts)
            => posts.Select(p => GetPostInDto(p)).ToList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="post">  </param>
        /// <returns>  </returns>
        private PostDto GetPostInDto(Post post)
            => new PostDto
            {
                Id = post.Id,
                Key = post.PostKey,
                Name = post.Name,
                Description = post.Description,
                Date = post.PublicationDate.ToString("M"),
                HeaderImageUrl = post.HeaderImageUrl,
                ChannelId = post.ChannelId,
                ChannelName = post.ChannelName,
                TopchiksCount = post.TopchiksCount,
                BookmarksCount = post.BookmarksCount,
                ViewsCount = post.ViewsCount,
                CommentsCount = post.CommentsCount
            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postKey">  </param>
        /// <returns>  </returns>
        private async Task<List<PostDetail>> GetPostDetailsAsync(string postKey)
        {
            var postElements = await _postElementRepository.GetPostElementsAsync(postKey);
            return postElements.OrderBy(p => p.Position).Select(p => new PostDetail
            {
                DetailType = p.FieldName,
                Data = p.Value,
                Description = p.Description
            }).ToList();
        }
    }
}