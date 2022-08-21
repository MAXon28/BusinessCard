using BusinessCard.BusinessLogicLayer.DTOs.Blog;
using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.BusinessLogicLayer.Utils;
using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using System.Collections.Generic;
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
        /// Утилита для пагинации по постам
        /// </summary>
        private readonly PaginationtUtil _paginationUtil = new PaginationtUtil(PostsCountInPackage);

        public PostService(IPostRepository postRepository) => _postRepository = postRepository;

        public async Task<List<PostDto>> GetPostsAsync(int reviewsPackageNumber)
        {
            var posts = new List<PostDto>();

            foreach (var post in await _postRepository.GetPostsAsync(_paginationUtil.GetOffset(reviewsPackageNumber)))
                posts.Add(new PostDto
                {
                    Id = post.Id,
                    Name = post.Name,
                    Description = post.Description,
                    Date = post.Date.ToString("M"),
                    HeaderImageUrl = post.HeaderImageUrl,
                    ChannelName = post.ChannelName,
                    TopchiksCount = post.TopchiksCount,
                    BookmarksCount = post.BookmarksCount,
                    ViewsCount = post.ViewsCount
                });

            return posts;
        }
    }
}