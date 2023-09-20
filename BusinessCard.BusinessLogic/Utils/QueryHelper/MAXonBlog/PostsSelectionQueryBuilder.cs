using BusinessCard.BusinessLogicLayer.Interfaces.Utils.QueryHelper;
using BusinessCard.BusinessLogicLayer.Utils.Enums;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonBlog
{
    /// <summary>
    /// Строитель запроса выборки постов
    /// </summary>
    internal class PostsSelectionQueryBuilder : SelectionQueryBuilder
    {
        /// <summary>
        /// Название таблицы
        /// </summary>
        private const string TableName = "Posts";

        /// <summary>
        /// Название переменной соответствующей таблицы
        /// </summary>
        private const string TableElementName = "post";

        /// <summary>
        /// 
        /// </summary>
        private const string JoinTableElementName = "bookmark";

        /// <summary>
        /// Сдвиг на конкретное число (для пагинации)
        /// </summary>
        private int _offset;

        /// <summary>
        /// 
        /// </summary>
        private PostRequestSettings _postRequestSettings;

        public PostsSelectionQueryBuilder()
        {
            _filterBuildersDictionary = new Dictionary<string, IFilterBuilder>
            {
                [FilterConstants.Id] = new IdFilterBuilder(TableElementName),
                [FilterConstants.User] = new UserIdFilterBuilder(JoinTableElementName),
                [FilterConstants.PostName] = new PostNameFilterBuilder(),
                [FilterConstants.Channel] = new ChannelIdFilterBuilder()
            };
        }

        /// <inheritdoc/>
        public override QueryData GetQueryData(RequestSettings requestSettings)
        {
            _dataCountInPackage = requestSettings.CountInPackage;

            _postRequestSettings = requestSettings as PostRequestSettings;
            _offset = _postRequestSettings.Offset;

            _sqlQuery = new StringBuilder();
            _sqlQuery.Append(_sqlQueryTemplate);
            SetTableNamings(TableName, TableElementName);
            SetSelect();
            SetJoin();
            SetWhere();
            if (TypeOfSelect == SelectTypes.Data)
            {
                SetTop(_offset);
                SetOrderBy();
            }

            return new(_sqlQuery.ToString(), _parameters);
        }

        /// <inheritdoc/>
        protected override void SetDataSelect()
        {
            var postsSelectionSet = $@"{TableElementName}.*,
										channel.Name,
										(SELECT COUNT(*)
										FROM Topchiks topchik
										WHERE topchik.PostId = {TableElementName}.Id) AS TopchiksCount,
										(SELECT COUNT(*)
										FROM Bookmarks {JoinTableElementName}
										WHERE {JoinTableElementName}.PostId = {TableElementName}.Id) AS BookmarksCount,
										(SELECT COUNT(*)
										FROM PostViews postView
										WHERE postView.PostId = {TableElementName}.Id) AS ViewsCount,
                                        (SELECT COUNT(*)
										FROM Comments comment
                                            INNER JOIN CommentBranches branch
                                            ON branch.Id = comment.BranchId 
										WHERE branch.PostId = {TableElementName}.Id) AS CommentsCount";

            _sqlQuery.Replace(SelectionSetTemplate, postsSelectionSet);
        }

        /// <inheritdoc/>
        protected override void SetCountSelect()
        {
            var postsCountSelectionSet = $@"COUNT ({TableElementName}.Id)";

            _sqlQuery.Replace(SelectionSetTemplate, postsCountSelectionSet);
            _sqlQuery.Replace(TopTemplate, string.Empty);
            _sqlQuery.Replace(OrderByTemplate, string.Empty);
        }

        /// <inheritdoc/>
        protected override void SetJoin()
        {
            var firstJoin = $@"INNER JOIN Channels channel
						       ON channel.Id = {TableElementName}.ChannelId";

            var secondJoin = $@"INNER JOIN Bookmarks {JoinTableElementName}
                                ON {JoinTableElementName}.PostId = {TableElementName}.Id";

            if (TypeOfSelect == SelectTypes.Data)
                _sqlQuery.Replace(FirstJoinTemplate, firstJoin);
            else
                _sqlQuery.Replace(FirstJoinTemplate, string.Empty);

            if (_postRequestSettings.RequestType == PostRequestTypes.Bookmark)
                _sqlQuery.Replace(SecondJoinTemplate, secondJoin);
            else
                _sqlQuery.Replace(SecondJoinTemplate, string.Empty);
        }

        /// <inheritdoc/>
        protected override void SetWhere()
        {
            const string where = "WHERE ";

            if (_offset != -1 && string.IsNullOrEmpty(_postRequestSettings.SearchText) && _postRequestSettings.RequestType == PostRequestTypes.All)
            {
                _sqlQuery.Replace(WhereTemplate, string.Empty);
                return;
            }

            var whereBuilder = new StringBuilder(where);

            if (_offset == -1)
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.Id], _postRequestSettings.LastElementId);

            if (_postRequestSettings.RequestType == PostRequestTypes.Bookmark)
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.User], _postRequestSettings.UserId);

            if (_postRequestSettings.RequestType == PostRequestTypes.Channel)
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.Channel], _postRequestSettings.ChannelId);

            if (string.IsNullOrEmpty(_postRequestSettings.SearchText) is false)
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.PostName], _postRequestSettings.SearchText);

            _sqlQuery.Replace(WhereTemplate, whereBuilder.ToString());
        }

        /// <inheritdoc/>
        protected override void SetOrderBy()
        {
            var needOffset = _offset != -1;
            string orderBy;
            if (needOffset)
            {
                orderBy = $@"ORDER BY {TableElementName}.Id DESC
					 OFFSET {OffsetParameter} ROWS
					 FETCH NEXT {_dataCountInPackage} ROWS ONLY";
                _parameters.Add(OffsetParameter, _offset, DbType.Int32, ParameterDirection.Input);
            }
            else
            {
                orderBy = $@"ORDER BY {TableElementName}.Id DESC";
            }

            _sqlQuery.Replace(OrderByTemplate, orderBy);
        }
    }
}