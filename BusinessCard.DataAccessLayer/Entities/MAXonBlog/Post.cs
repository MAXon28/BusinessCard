﻿using DapperAssistant.Annotations;
using System;

namespace BusinessCard.DataAccessLayer.Entities.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("Posts")]
    public class Post
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HeaderImageUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Channels")]
        public string ChannelId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotSqlColumn]
        public string ChannelName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotSqlColumn]
        public int TopchiksCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotSqlColumn]
        public int BookmarksCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotSqlColumn]
        public int ViewsCount { get; set; }
    }
}