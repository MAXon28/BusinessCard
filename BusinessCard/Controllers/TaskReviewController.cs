using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BusinessCard.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskReviewController : Controller
    {
        private readonly ITaskReviewService _taskReviewService;

        public TaskReviewController(ITaskReviewService taskReviewService)
        {
            _taskReviewService = taskReviewService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="taskUrl"></param>
        /// <param name="rating"></param>
        /// <param name="text"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = Roles.User)]
        public async Task<string> AddReview(int taskId, string taskUrl, int rating, string text, int serviceId)
            => await _taskReviewService.AddReviewAsync(taskId, taskUrl, Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value), rating, text, serviceId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetLastSevenReviews(int id)
        {
            return Json(/*await _taskReviewService.GetFortyReviewsAsync(id)*/"");
        }
    }
}