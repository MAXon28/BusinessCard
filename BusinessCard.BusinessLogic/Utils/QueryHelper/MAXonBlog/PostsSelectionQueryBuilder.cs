using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonBlog
{
    /// <summary>
    /// Строитель запроса выборки постов
    /// </summary>
    public class PostsSelectionQueryBuilder : SelectionQueryBuilder
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
        private PostRequestSettings _postRequestSettings;

        public PostsSelectionQueryBuilder()
        {
            _filterBuildersDictionary = new Dictionary<string, IFilterBuilder>
            {
                [FilterConstants.PostName] = new PostNameFilterBuilder(),
                [FilterConstants.User] = new UserIdFilterBuilder(),
                [FilterConstants.Channel] = new ChannelIdFilterBuilder()
            };
        }

        public override QueryData GetQueryData(IRequestSettings requestSettings)
        {
            _postRequestSettings = requestSettings as PostRequestSettings;
            _sqlQuery = new StringBuilder();
            _sqlQuery.Append(_sqlQueryTemplate);
            SetTableNamings(TableName, TableElementName);
            SetSelect(_postRequestSettings.Offset);
            SetJoin();
            SetWhere();

            return new QueryData(_sqlQuery.ToString(), _parameters);
        }

        /// <inheritdoc/>
        protected override void SetDataSelect(int offset)
        {
            var postsSelectionSet = $@"{TableElementName}.*,
										channel.Name,
										(SELECT COUNT(*)
										FROM Topchiks topchik
										WHERE topchik.PostId = {TableElementName}.Id) AS TopchiksCount,
										(SELECT COUNT(*)
										FROM Bookmarks bookmark
										WHERE bookmark.PostId = {TableElementName}.Id) AS BookmarksCount,
										(SELECT COUNT(*)
										FROM PostViews postView
										WHERE postView.PostId = {TableElementName}.Id) AS ViewsCount,
                                        (SELECT COUNT(*)
										FROM Comments comment
                                            INNER JOIN CommentBranches branch
                                            ON branch.Id = comment.BranchId 
										WHERE branch.PostId = {TableElementName}.Id) AS CommentsCount";

            var orderBy = $@"ORDER BY {TableElementName}.Id DESC
									   OFFSET {OffsetParameter} ROWS
									   FETCH NEXT 28 ROWS ONLY";


            _sqlQuery.Replace(SelectionSetTemplate, postsSelectionSet);
            _sqlQuery.Replace(OrderByTemplate, orderBy);

            _parameters.Add(OffsetParameter, offset, DbType.Int32, ParameterDirection.Input);
        }

        /// <inheritdoc/>
        protected override void SetCountSelect()
        {
            var postsCountSelectionSet = $@"COUNT ({TableElementName}.Id)";

            _sqlQuery.Replace(SelectionSetTemplate, postsCountSelectionSet);
            _sqlQuery.Replace(OrderByTemplate, string.Empty);
        }

        /// <inheritdoc/>
        protected override void SetJoin()
        {
            var firstJoin = $@"INNER JOIN Channels channel
						       ON channel.Id = {TableElementName}.ChannelId";

            var secondJoin = $@"INNER JOIN Bookmarks bookmark
                                ON bookmark.PostId = {TableElementName}.Id";

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

            if (string.IsNullOrEmpty(_postRequestSettings.SearchText) && _postRequestSettings.RequestType == PostRequestTypes.All)
            {
                _sqlQuery.Replace(WhereTemplate, string.Empty);
                return;
            }

            var whereBuilder = new StringBuilder(where);

            if (_postRequestSettings.RequestType == PostRequestTypes.Bookmark)
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.User], _postRequestSettings.UserId);

            if (_postRequestSettings.RequestType == PostRequestTypes.Channel)
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.Channel], _postRequestSettings.ChannelId);

            if (!string.IsNullOrEmpty(_postRequestSettings.SearchText))
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.PostName], _postRequestSettings.SearchText);

            _sqlQuery.Replace(WhereTemplate, whereBuilder.ToString());
        }
    }
}