#if GoogleMobileAds
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

namespace TDC.Ads
{
    public class CoreAdMob
    {
        public enum TType
        {
            Public,
            Developing
        }
        
        private const string advID = "ca-app-pub-1974506492939580~7773353943";

        private const string idVideoAdv = "ca-app-pub-3940256099942544/1033173712";
        private InterstitialAd videoAdv;

        private const string idRewardAdv = "ca-app-pub-3940256099942544/5224354917";
        private RewardBasedVideoAd rewardAdv;

        private const string idBannerAdv = "ca-app-pub-3940256099942544/6300978111";
        private BannerView bannerAdv;

        public static bool networkEnable = false;

        public void Initialization()
        {
            MobileAds.Initialize(advID);

            StartCoroutine(CheckNetwork());

            rewardAdv = RewardBasedVideoAd.Instance;

            LoadVideoAdv();
            LoadRewardAvb();
        }

#region Video

        public InterstitialAd CellVideoAdv()
        {
            videoAdv.Show();
            videoAdv.OnAdLoaded += OnAdvLoaded;
            videoAdv.OnAdClosed += OnAdvClosed;

            return videoAdv;
        }

        private void LoadVideoAdv()
        {
            videoAdv = new InterstitialAd(idVideoAdv);
            videoAdv.LoadAd(GetRequest(TType.Developing));
        }

        private void OnAdvLoaded(object sender, System.EventArgs args)
        {
            videoAdv.Show();
        }

        private void OnAdvClosed(object sender, System.EventArgs args)
        {
            videoAdv.Destroy();
            videoAdv = null;

            LoadVideoAdv();
        }

#endregion

#region Reward

        public RewardBasedVideoAd CellRewardAdv()
        {
            LoadRewardAvb();
            rewardAdv.Show();
            rewardAdv.OnAdLoaded += OnRewardAdvLoaded;

            return rewardAdv;
        }

        private void LoadRewardAvb()
        {
            rewardAdv.LoadAd(GetRequest(TType.Developing), idRewardAdv);
        }

        private void OnRewardAdvLoaded(object sender, System.EventArgs args)
        {
            rewardAdv.Show();
        }

#endregion

#region Banner

        public BannerView CellBannerAdv(AdPosition position = AdPosition.Bottom)
        {
            bannerAdv = new BannerView(idBannerAdv, AdSize.Banner, position);
            bannerAdv.LoadAd(GetRequest(TType.Developing));

            return bannerAdv;
        }

#endregion

        private AdRequest GetRequest(TType type = TType.Public)
        {
            switch(type)
            {
                case TType.Public: return new AdRequest.Builder().Build();
                case TType.Developing: return new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).Build();
            }

            return null;
        }

        private IEnumerator CheckNetwork()
        {
            var request = new WWW("http://www.google.com/");

            yield return request;

            networkEnable = request.isDone;

            yield return new WaitForSeconds(5f);

            StartCoroutine(CheckNetwork());
        }
    }
}
#endif