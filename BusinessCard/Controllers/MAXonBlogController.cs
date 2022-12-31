using BusinessCard.BusinessLogicLayer.DTOs.Blog;
using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BusinessCard.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class MAXonBlogController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IBlogService _blogService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IPersonalInformationService _personalInformationService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IChannelService _channelService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IPostService _postService;

        /// <summary>
        /// 
        /// </summary>
        private readonly ICommentService _commentService;

        public MAXonBlogController(IBlogService blogService, IPersonalInformationService personalInformationService, IChannelService channelService, IPostService postService, ICommentService commentService)
        {
            _blogService = blogService;
            _personalInformationService = personalInformationService;
            _channelService = channelService;
            _postService = postService;
            _commentService = commentService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public IActionResult Posts() => View();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<string> GetBlogInformation() 
            => JsonConvert.SerializeObject(new
            {
                BlogInformation = await _blogService.GetBlogInformationAsync(1),
                AuthorizedUser = true
            });

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<string> GetChannels()
            => JsonConvert.SerializeObject(new
            {
                ChannelsInformation = await _channelService.GetChannelsAsync()
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<string> GetChannelInformation([FromQuery] int channelId) 
            => JsonConvert.SerializeObject(new
            {
                ChannelInformation = await _blogService.GetChannelInformationAsync(1, channelId)
            });

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<string> GetSubscriptionOnChannels() 
            => JsonConvert.SerializeObject(new
            {
                Subscriptions = await _personalInformationService.GetSubscriptionOnChannelsAsync(1)
            });

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<string> GetSubscriptionOnMailings()
            => JsonConvert.SerializeObject(new
            {
                Subscriptions = await _personalInformationService.GetSubscriptionOnMailingsAsync(1)
            });

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<string> GetPosts([FromQuery] PostFilters filters)
            => JsonConvert.SerializeObject(new
            {
                PostsInformation = await _postService.GetPostsAsync(1, filters)
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId">  </param>
        /// <param name="isTopchik">  </param>
        /// <returns>  </returns>
        [HttpPost]
        public async Task<bool> SendTopchik(int postId, bool isTopchik)
            => await _personalInformationService.AddOrDeleteTopchikAsync(1, postId, isTopchik);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId">  </param>
        /// <param name="inBookmark">  </param>
        /// <returns>  </returns>
        [HttpPost]
        public async Task<bool> SendPostInBookmark(int postId, bool inBookmark)
            => await _personalInformationService.AddOrDeletePostInBookmarkAsync(1, postId, inBookmark);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId">  </param>
        /// <param name="isSubscribe">  </param>
        /// <param name="subscribeType">  </param>
        /// <returns>  </returns>
        [HttpPost]
        public async Task<bool> Subscribe(int channelId, bool isSubscribe, string subscribeType)
            => await _personalInformationService.SubscribeAsync(1, channelId, isSubscribe, subscribeType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">  </param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Post(string id)
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postKey">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<string> GetPostInformation([FromQuery] string postKey)
            => JsonConvert.SerializeObject(new
            {
                PostInformation = await _postService.GetPostInformationAsync(1, postKey),
                AuthorizedUser = true,
                UserName = "Максим"
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">  </param>
        /// <param name="commentsPackageNumber">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<string> GetFirstCommentsInformation([FromQuery] int postId, [FromQuery] int commentsCount)
            => JsonConvert.SerializeObject(new
            {
                CommentsInformation = await _commentService.GetFirstCommentsInformationAsync(postId, commentsCount, 1)
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId">  </param>
        /// <param name="lastBranchId">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<string> GetCommentsInformation([FromQuery] int postId, [FromQuery] int lastBranchId, [FromQuery] bool allNextComments = false)
            => JsonConvert.SerializeObject(new
            {
                CommentsInformation = await _commentService.GetCommentsInformationAsync(postId, lastBranchId, allNextComments, 1)
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId">  </param>
        /// <param name="branchId">  </param>
        /// <param name="lastCommentId">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<string> GetCommentsByBranch([FromQuery] int postId, [FromQuery] int branchId, [FromQuery] int lastCommentId)
            => JsonConvert.SerializeObject(new
            {
                Comments = await _commentService.GetCommentsByBranchAsync(postId, branchId, lastCommentId, 1)
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment">  </param>
        /// <returns>  </returns>
        [HttpPost]
        public async Task<string> CreateComment([FromBody] CommentIn comment)
        {
            comment.UserId = 1;
            var result = await _commentService.CreateCommentAsync(comment);
            return JsonConvert.SerializeObject(new
            {
                BranchId = result.BranchId,
                CommentId = result.CommentId
            });
        }

        [HttpPost]
        public async Task<bool> DeleteComment(int commentId)
            => await _commentService.DeleteCommentAsync(commentId, 1);
    }
}