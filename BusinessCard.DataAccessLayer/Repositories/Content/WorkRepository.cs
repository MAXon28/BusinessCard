﻿using BusinessCard.DataAccessLayer.Entities.Content;
using BusinessCard.DataAccessLayer.Interfaces.Content;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.Content
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkRepository : StandardRepository<Work>, IWorkRepository
    {
        public WorkRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}