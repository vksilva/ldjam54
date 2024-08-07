using System.Threading.Tasks;
using Busta.Extensions;
using Google.Play.Review;
using UnityEngine;

namespace Busta.AppCore.Review
{
    public class ReviewService
    {
        private ReviewManager reviewManager;
        private PlayReviewInfo playReviewInfo;

        public void Init()
        {
            reviewManager = new ReviewManager();
        }

        public async Task RequestReview()
        {
            var requestFlowOperation = reviewManager.RequestReviewFlow();
            await requestFlowOperation.Await();
            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                Debug.LogWarning($"Request flow error {requestFlowOperation.Error}");
                return;
            }
            playReviewInfo = requestFlowOperation.GetResult();
            Debug.Log($"Review requested");

            var launchFlowOperation = reviewManager.LaunchReviewFlow(playReviewInfo);
            await launchFlowOperation.Await();
            playReviewInfo = null;
            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                Debug.LogWarning($"Launch flow error {launchFlowOperation.Error}");
                return;
            }
            Debug.Log("Review launched");
        }
    }
}