using System.Threading.Tasks;
using UnityEngine;

#if !UNITY_WEBGL
using Busta.Extensions;
using Google.Play.Review;
#endif

namespace Busta.AppCore.Review
{
    public class ReviewService
    {
#if !UNITY_WEBGL
        private ReviewManager reviewManager;
        private PlayReviewInfo playReviewInfo;
#endif

        public void Init()
        {
#if !UNITY_WEBGL
            reviewManager = new ReviewManager();
#endif
        }

        public async Task RequestReview()
        {
#if !UNITY_WEBGL
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
#else
            await Task.CompletedTask;
#endif
            
            Debug.Log("Review launched");
        }
    }
}