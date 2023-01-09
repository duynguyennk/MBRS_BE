using FakeItEasy;
using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;
using MBRS_API.Services.Service;
using MBRS_API_DEMO.Models;
using MBRS_API_DEMO.Repositories.IRepository;
using MBRS_API_DEMO.Services.Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBRS_API.Tests.Services
{
    public class ManageFeedbackOrderCustomerService_Test
    {
        private IConfiguration _configuration;
        private IManageFeedbackOrderCustomerRepository _manageFeedbackOrderCustomerRepository;
        private ManageFeedbackOrderCustomerService _manageFeedbackOrderCustomerService;

        public ManageFeedbackOrderCustomerService_Test()
        {
            _configuration = A.Fake<IConfiguration>();
            _manageFeedbackOrderCustomerRepository = A.Fake<IManageFeedbackOrderCustomerRepository>();
            _manageFeedbackOrderCustomerService = new ManageFeedbackOrderCustomerService(_configuration, _manageFeedbackOrderCustomerRepository);
        }

        [Fact]
        public void ManageFeedbackOrderCustomerService_createRoomFeedback_ReturnSuccess()
        {
            var feedback = A.Fake<FeedbackRoom>();
            feedback = new FeedbackRoom
            {
                orderID = 1,
                contentRating = "Good",
                numberRatingConveniences = 1,
                numberRatingEmployee = 1,
                numberRatingHygiene = 1 ,
                numberRatingInterior = 1,
                numberRatingService = 1,
                numberRatingView = 1
            };
            var listFeedback = A.Fake<List<FeedbackRoom>>();
            listFeedback.Add(feedback);
            A.CallTo(() => _manageFeedbackOrderCustomerRepository.createRoomFeedback(feedback)).Returns(1);
            var result = _manageFeedbackOrderCustomerService.createRoomFeedback(feedback);
            Assert.Equal(1, result);
        }

        [Fact]
        public void ManageFeedbackOrderCustomerService_getRatingRoomByRatingID_ReturnEqualCount()
        {
            var feedback = A.Fake<FeedbackRoom>();
            feedback = new FeedbackRoom
            {
                orderID = 1,
                contentRating = "Good",
                numberRatingConveniences = 1,
                numberRatingEmployee = 1,
                numberRatingHygiene = 1,
                numberRatingInterior = 1,
                numberRatingService = 1,
                numberRatingView = 1
            };
            var listFeedback = A.Fake<List<FeedbackRoom>>();
            listFeedback.Add(feedback);
            A.CallTo(() => _manageFeedbackOrderCustomerRepository.getRatingRoomByRatingID(feedback.orderID)).Returns(listFeedback);
            var result = _manageFeedbackOrderCustomerService.getRatingRoomByRatingID(feedback.orderID);
            Assert.Equal(listFeedback.Count, result.Count);
        }

        [Fact]
        public void ManageFeedbackOrderCustomerService_createServiceFeedback_ReturnSuccess()
        {
            var feedback = A.Fake<FeedbackService>();
            feedback = new FeedbackService
            {
                orderID = 1,
                contentRating = "very good",
                ratingNumber = 1,
                selectedOption = 2
            };
            var listFeedback = A.Fake<List<FeedbackService>>();
            listFeedback.Add(feedback);
            if (feedback.selectedOption == 2)
            {
                A.CallTo(() => _manageFeedbackOrderCustomerRepository.createFoodFeedback(feedback)).Returns(1);
            }
            else
            {
                A.CallTo(() => _manageFeedbackOrderCustomerRepository.createBikeFeedback(feedback)).Returns(1);
            }
                
            var result = _manageFeedbackOrderCustomerService.createServiceFeedback(feedback);
            Assert.Equal(1, result);
        }

        [Fact]
        public void ManageFeedbackOrderCustomerService_getRatingServiceByRatingID_ReturnEqualCount()
        {
            var feedback = A.Fake<FeedbackService>();
            feedback = new FeedbackService
            {
                orderID = 1,
                contentRating = "very good",
                ratingNumber = 1,
                selectedOption = 2
            };
            var listFeedback = A.Fake<List<FeedbackService>>();
            listFeedback.Add(feedback);
            if (feedback.selectedOption == 2)
            {
                A.CallTo(() => _manageFeedbackOrderCustomerRepository.getRatingFoodByRatingID(feedback.orderID)).Returns(listFeedback);
            }
            else
            {
                A.CallTo(() => _manageFeedbackOrderCustomerRepository.getRatingBikeByRatingID(feedback.orderID)).Returns(listFeedback);
            }
            var result = _manageFeedbackOrderCustomerService.getRatingServiceByRatingID(feedback.orderID,feedback.selectedOption);
            Assert.Equal(listFeedback.Count, result.Count);
        }
    }
}
