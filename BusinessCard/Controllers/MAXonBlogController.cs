using BusinessCard.Entities.DTO.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.Entities;

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
        {
            var isAuthorized = User.Identity.IsAuthenticated;
            return JsonConvert.SerializeObject(new
            {
                BlogInformation = await _blogService.GetBlogInformationAsync(isAuthorized ? Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value) : -1),
                AuthorizedUser = isAuthorized
            });
        }

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
                ChannelInformation = await _blogService.GetChannelInformationAsync(
                    User.Identity.IsAuthenticated
                        ? Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value) 
                        : -1, 
                    channelId)
            });

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        [Authorize]
        public async Task<string> GetSubscriptionOnChannels() 
            => JsonConvert.SerializeObject(new
            {
                Subscriptions = await _personalInformationService.GetSubscriptionOnChannelsAsync(Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value))
            });

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        [Authorize]
        public async Task<string> GetSubscriptionOnMailings()
            => JsonConvert.SerializeObject(new
            {
                Subscriptions = await _personalInformationService.GetSubscriptionOnMailingsAsync(Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value))
            });

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<string> GetPosts([FromQuery] PostFilters filters)
            => JsonConvert.SerializeObject(new
            {
                PostsInformation = await _postService.GetPostsAsync(
                    User.Identity.IsAuthenticated
                        ? Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value)
                        : -1, 
                    filters)
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId">  </param>
        /// <param name="isTopchik">  </param>
        /// <returns>  </returns>
        [HttpPost]
        [Authorize]
        public async Task<bool> SendTopchik(int postId, bool isTopchik)
            => await _personalInformationService.AddOrDeleteTopchikAsync(Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value), postId, isTopchik);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId">  </param>
        /// <param name="inBookmark">  </param>
        /// <returns>  </returns>
        [HttpPost]
        [Authorize]
        public async Task<bool> SendPostInBookmark(int postId, bool inBookmark)
            => await _personalInformationService.AddOrDeletePostInBookmarkAsync(Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value), postId, inBookmark);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId">  </param>
        /// <param name="isSubscribe">  </param>
        /// <param name="subscribeType">  </param>
        /// <returns>  </returns>
        [HttpPost]
        [Authorize]
        public async Task<bool> Subscribe(int channelId, bool isSubscribe, string subscribeType)
            => await _personalInformationService.SubscribeAsync(Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value), channelId, isSubscribe, subscribeType);

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
        {
            var isAuthenticated = User.Identity.IsAuthenticated;
            return JsonConvert.SerializeObject(new
            {
                PostInformation = await _postService.GetPostInformationAsync(
                    isAuthenticated
                        ? Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value)
                        : -1,
                    postKey),
                AuthorizedUser = isAuthenticated,
                UserName = isAuthenticated ? User.FindFirst(CookieConstants.UserName).Value : string.Empty
            });
        }

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
                CommentsInformation = await _commentService.GetFirstCommentsInformationAsync(
                    postId, 
                    commentsCount, 
                    User.Identity.IsAuthenticated
                        ? (int?)Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value)
                        : null)
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
                CommentsInformation = await _commentService.GetCommentsInformationAsync(
                    postId, 
                    lastBranchId, 
                    allNextComments,
                    User.Identity.IsAuthenticated
                        ? (int?)Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value)
                        : null)
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
                Comments = await _commentService.GetCommentsByBranchAsync(
                    postId, 
                    branchId,
                    lastCommentId,
                    User.Identity.IsAuthenticated
                        ? (int?)Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value)
                        : null)
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment">  </param>
        /// <returns>  </returns>
        [HttpPost]
        [Authorize]
        public async Task<string> CreateComment([FromBody] CommentIn comment)
        {
            comment.UserId = Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value);
            var result = await _commentService.CreateCommentAsync(comment);
            return JsonConvert.SerializeObject(new
            {
                BranchId = result.BranchId,
                CommentId = result.CommentId
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId">  </param>
        /// <returns>  </returns>
        [HttpPost]
        [Authorize]
        public async Task<bool> DeleteComment(int commentId)
            => await _commentService.DeleteCommentAsync(commentId, Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        [HttpGet]
        [Authorize]
        public async Task<string> GetUserStatistic(int userId)
            => JsonConvert.SerializeObject(new
            {
                BlogStatistic = await _personalInformationService.GetUserBlogStatisticAsync(userId)
            });
    }
}