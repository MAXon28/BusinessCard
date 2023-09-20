using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.BusinessLogicLayer.Utils.Enums;
using BusinessCard.BusinessLogicLayer.Utils.Extensions;
using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using BusinessCard.Entities.DTO.Blog;
using DapperAssistant;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="IPersonalInformationService"/>
    internal class PersonalInformationService : IPersonalInformationService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IUserStatisticRepository _userStatisticRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ITopchikRepository _topchikRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IBookmarkRepository _bookmarkRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IChannelSubscriptionRepository _channelSubscriptionRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IMailingSubscriptionRepository _mailingSubscriptionRepository;

        public PersonalInformationService(
            IUserStatisticRepository userStatisticRepository,
            ITopchikRepository topchikRepository,
            IBookmarkRepository bookmarkRepository,
            IChannelSubscriptionRepository channelSubscriptionRepository,
            IMailingSubscriptionRepository mailingSubscriptionRepository)
        {
            _userStatisticRepository = userStatisticRepository;
            _topchikRepository = topchikRepository;
            _bookmarkRepository = bookmarkRepository;
            _channelSubscriptionRepository = channelSubscriptionRepository;
            _mailingSubscriptionRepository = mailingSubscriptionRepository;
        }

        public async Task<PersonalInformation> GetPersonalInformationAsync(int userId, IEnumerable<int> postsId)
            => new() { StatisticsByPost = await _userStatisticRepository.GetUserStatisticByPostsAsync(userId, postsId) };

        public async Task<PersonalInformation> GetPersonalInformationAsync(int userId, int channelId, IEnumerable<int> postsId)
            => new()
            {
                SubscriptionsDictionary = await _userStatisticRepository.GetUserSubscriptionsByPostsAsync(userId, channelId),
                StatisticsByPost = await _userStatisticRepository.GetUserStatisticByPostsAsync(userId, postsId)
            };

        public async Task<Dictionary<int, ChannelDto>> GetSubscriptionOnChannelsAsync(int userId)
        {
            var querySettings = new QuerySettings
            {
                ConditionField = "UserId",
                ConditionType = ConditionType.EQUALLY,
                ConditionFieldValue = userId,
                NeedSortDescendingOrder = true
            };

            var data = await _channelSubscriptionRepository.GetWithConditionAsync(querySettings);

            var subscriptionOnChannels = new Dictionary<int, ChannelDto>();
            foreach (var subscriptionOnChannel in data)
                subscriptionOnChannels.Add(subscriptionOnChannel.Id, new ChannelDto
                {
                    Id = subscriptionOnChannel.Channel.Id,
                    Name = subscriptionOnChannel.Channel.Name,
                    Color = subscriptionOnChannel.Channel.Color
                });
            return subscriptionOnChannels;
        }

        public async Task<Dictionary<int, ChannelDto>> GetSubscriptionOnMailingsAsync(int userId)
        {
            var querySettings = new QuerySettings
            {
                ConditionField = "UserId",
                ConditionType = ConditionType.EQUALLY,
                ConditionFieldValue = userId,
                NeedSortDescendingOrder = true
            };

            var data = await _mailingSubscriptionRepository.GetWithConditionAsync(querySettings);

            var subscriptionOnMailings = new Dictionary<int, ChannelDto>();
            foreach (var subscriptionOnMailing in data)
                subscriptionOnMailings.Add(subscriptionOnMailing.Id, new ChannelDto
                {
                    Id = subscriptionOnMailing.Channel.Id,
                    Name = subscriptionOnMailing.Channel.Name,
                    Color = subscriptionOnMailing.Channel.Color
                });
            return subscriptionOnMailings;
        }

        public async Task<bool> AddOrDeleteTopchikAsync(int userId, int postId, bool isTopchik)
            => isTopchik
            ? await _topchikRepository.AddAsync(GetTopchik(userId, postId)) == 1
            : await _topchikRepository.DeleteTopchikAsync(userId, postId) == 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="postId">  </param>
        /// <returns>  </returns>
        private Topchik GetTopchik(int userId, int postId)
            => new()
            {
                UserId = userId,
                PostId = postId
            };

        public async Task<bool> AddOrDeletePostInBookmarkAsync(int userId, int postId, bool inBookmark)
            => inBookmark
            ? await _bookmarkRepository.AddAsync(GetBookmark(userId, postId)) == 1
            : await _bookmarkRepository.DeletePostFromBookmarkAsync(userId, postId) == 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="postId">  </param>
        /// <returns>  </returns>
        private Bookmark GetBookmark(int userId, int postId)
            => new()
            {
                UserId = userId,
                PostId = postId
            };

        public async Task<bool> SubscribeAsync(int userId, int channelId, bool isSubscribe, string subscribeTypeStr)
            => subscribeTypeStr.ToEnum<SubscribeTypes>() switch
            {
                SubscribeTypes.OnChannel => isSubscribe
                                            ? await _channelSubscriptionRepository.AddAsync(GetSubscription<ChannelSubscription>(userId, channelId)) == 1
                                            : await _channelSubscriptionRepository.DeleteChannelFromSubscriptionAsync(userId, channelId) == 1,

                SubscribeTypes.OnMailing => isSubscribe
                                            ? await _mailingSubscriptionRepository.AddAsync(GetSubscription<MailingSubscription>(userId, channelId)) == 1
                                            : await _mailingSubscriptionRepository.DeleteChannelFromSubscriptionAsync(userId, channelId) == 1,

                _ => throw new Exception()
            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="channelId">  </param>
        /// <typeparam name="T">  </typeparam>
        /// <returns>  </returns>
        private T GetSubscription<T>(int userId, int channelId) where T : Subscription, new()
            => new()
            {
                UserId = userId,
                ChannelId = channelId
            };

        public async Task<UserBlogStatistic> GetUserBlogStatisticAsync(int userId)
        {
            const string channelSubscriptionsKey = "ChannelSubscriptionsCount";
            const string mailingSubscriptionsKey = "MailingSubscriptionsCount";
            const string bookmarksKey = "BookmarksCount";

            var userStaitistics = await _userStatisticRepository.GetUserStatisticInBlogAsync(userId);
            return new()
            {
                ChannelSubscriptionsCount = userStaitistics[channelSubscriptionsKey],
                MailingSubscriptionsCount = userStaitistics[mailingSubscriptionsKey],
                BookmarksCount = userStaitistics[bookmarksKey]
            };
        }
    }
}