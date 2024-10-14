using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GoogleMobileAds.Api;

namespace BonusLibrary
{
    public class LibraryMath
    {
        public void Addition(Transform runnerFLs, int bonusAmount, Transform spawnPoint)
        {
            int count = 0;

            foreach (Transform runnerFL in runnerFLs)
            {
                if (count < bonusAmount)
                {
                    if (!runnerFL.gameObject.activeInHierarchy)
                    {
                        runnerFL.position = spawnPoint.position;
                        runnerFL.gameObject.SetActive(true);

                        GameManager.instance.ActiveSpawnEffects(runnerFL);

                        count++;
                    }
                }

                else break;
            }

            GameManager.instance.CalculateRunnerFLCount(BonusType.Addition, bonusAmount);
        }

        public void Difference(Transform runnerFLs, int bonusAmount)
        {
            int runnerFLCount = GameManager.instance.GetRunnerFLCount();

            if (runnerFLCount < bonusAmount)
            {
                foreach (Transform runnerFL in runnerFLs)
                {
                    if (runnerFL.gameObject.activeInHierarchy)
                    {
                        GameManager.instance.ActiveDeadEffects(runnerFL);

                        runnerFL.position = Vector3.zero;
                        runnerFL.gameObject.SetActive(false);
                    }
                }

                GameManager.instance.ResetRunnerFLCount();
            }

            else
            {
                int count = 0;

                foreach (Transform runnerFL in runnerFLs)
                {
                    if (count != bonusAmount)
                    {
                        if (runnerFL.gameObject.activeInHierarchy)
                        {
                            GameManager.instance.ActiveDeadEffects(runnerFL);

                            runnerFL.position = Vector3.zero;
                            runnerFL.gameObject.SetActive(false);
                            count++;
                        }
                    }

                    else break;
                }

                GameManager.instance.CalculateRunnerFLCount(BonusType.Difference, bonusAmount);
            }
        }

        public void Product(Transform runnerFLs, int bonusAmount, Transform spawnPoint)
        {
            int count = 0;
            int runnerFLCount = GameManager.instance.GetRunnerFLCount();

            int runnersToAdd = (runnerFLCount * bonusAmount) - runnerFLCount;

            foreach (Transform runnerFL in runnerFLs)
            {
                if (count < runnersToAdd)
                {
                    if (!runnerFL.gameObject.activeInHierarchy)
                    {
                        runnerFL.position = spawnPoint.position;
                        runnerFL.gameObject.SetActive(true);

                        GameManager.instance.ActiveSpawnEffects(runnerFL);

                        count++;
                    }
                }

                else break;
            }

            GameManager.instance.CalculateRunnerFLCount(BonusType.Product, bonusAmount);
        }

        public void Division(Transform runnerFLs, int bonusAmount)
        {
            int runnerFLCount = GameManager.instance.GetRunnerFLCount();

            if (runnerFLCount <= bonusAmount)
            {
                foreach (Transform runnerFL in runnerFLs)
                {
                    if (runnerFL.gameObject.activeInHierarchy)
                    {
                        GameManager.instance.ActiveDeadEffects(runnerFL);

                        runnerFL.position = Vector3.zero;
                        runnerFL.gameObject.SetActive(false);
                    }
                }

                GameManager.instance.ResetRunnerFLCount();
            }

            else
            {
                int divideValue = runnerFLCount / bonusAmount;
                int runnersToRemove = runnerFLCount - divideValue;

                int count = 0;

                foreach (Transform runnerFL in runnerFLs)
                {
                    if (count != runnersToRemove)
                    {
                        if (runnerFL.gameObject.activeInHierarchy)
                        {
                            GameManager.instance.ActiveDeadEffects(runnerFL);

                            runnerFL.position = Vector3.zero;
                            runnerFL.gameObject.SetActive(false);
                            count++;
                        }
                    }

                    else break;
                }

                GameManager.instance.CalculateRunnerFLCount(BonusType.Division, runnersToRemove);
            }
        }
    }

    public class DataManage
    {
        public void SaveData_Int(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public void SaveData_Float(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public void SaveData_String(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public int GetData_Int(string key)
        {
            return PlayerPrefs.GetInt(key);
        }

        public float GetData_Float(string key)
        {
            return PlayerPrefs.GetFloat(key);
        }

        public string GetData_String(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public void CheckCurrentLevel(string keyLevel)
        {
            if (!PlayerPrefs.HasKey(keyLevel))
            {
                PlayerPrefs.SetInt(keyLevel, 5);
                PlayerPrefs.SetInt("ScoreKey", 100);

                PlayerPrefs.SetInt("HatKey", -1);
                PlayerPrefs.SetInt("WeaponKey", -1);
                PlayerPrefs.SetInt("ColorKey", -1);

                PlayerPrefs.SetFloat("MenuVol", 1);
                PlayerPrefs.SetFloat("SfxVol", 1);
                PlayerPrefs.SetFloat("GameVol", 1);

                PlayerPrefs.SetString("LanguageKey", "Eng");

                PlayerPrefs.SetInt("AdsCount", 1);
            }
        }
    }




    // ITEM DATA

    [Serializable]
    public class ItemInfo
    {
        public int groupItemIndex;
        public int itemIndex;
        public string itemName;
        public int price;
        public bool purchaseStatus;
    }

    public class HugeData
    {
        private List<ItemInfo> itemList;
        private List<LanguageData> languageList;

        public void CreateData(List<ItemInfo> itemInfo, List<LanguageData> languageInfo)
        {
            if (!File.Exists(Application.persistentDataPath + "/ItemData.gd"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + "/ItemData.gd");

                bf.Serialize(file, itemInfo);
                file.Close();
            }

            if (!File.Exists(Application.persistentDataPath + "/LanguageData.gd"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + "/LanguageData.gd");

                bf.Serialize(file, languageInfo);
                file.Close();
            }
        }

        public void SaveData(List<ItemInfo> itemInfo)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.OpenWrite(Application.persistentDataPath + "/ItemData.gd");

            bf.Serialize(file, itemInfo);
            file.Close();
        }

        public void LoadData()
        {
            if (File.Exists(Application.persistentDataPath + "/ItemData.gd"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/ItemData.gd", FileMode.Open);

                itemList = (List<ItemInfo>)bf.Deserialize(file);
                file.Close();
            }

            if (File.Exists(Application.persistentDataPath + "/LanguageData.gd"))
            {
                Debug.Log(Application.persistentDataPath);
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/LanguageData.gd", FileMode.Open);

                languageList = (List<LanguageData>)bf.Deserialize(file);
                file.Close();
            }
        }

        public List<ItemInfo> GetItemList()
        {
            return itemList;
        }

        public List<LanguageData> GetLanguageList()
        {
            return languageList;
        }
    }




    // LANGUAGE DATA

    [Serializable]
    public class LanguageData
    {
        public int languageCount;

        public List<LanguageString> languages_Eng = new List<LanguageString>();
        public List<LanguageString> languages_VN = new List<LanguageString>();
    }

    [Serializable]
    public class LanguageString
    {
        public string letter;
    }




    // ADVERTISEMENT

    public class AdvertisingManage
    {
        private InterstitialAd interstitialAd;
        private RewardedAd rewardedAd;

        public void LoadInterstitialAd()
        {

#if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/1033173712";

#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/4411468910";

#else
            string adUnitId = "unused";

#endif

            // Clean up the old ad before loading a new one.
            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
                interstitialAd = null;
            }

            Debug.Log("Loading the interstitial ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            InterstitialAd.Load(adUnitId, adRequest,
                (InterstitialAd ad, LoadAdError error) =>

                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("interstitial ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Interstitial ad loaded with response : "
                              + ad.GetResponseInfo());

                    interstitialAd = ad;
                });
        }

        public void ShowInterstitialAd()
        {
            if (PlayerPrefs.GetInt("AdsCount") == 2)
            {
                if (interstitialAd != null && interstitialAd.CanShowAd())
                {
                    PlayerPrefs.SetInt("AdsCount", 1);

                    Debug.Log("Showing interstitial ad.");
                    interstitialAd.Show();
                }
            }

            else
            {
                PlayerPrefs.SetInt("AdsCount", PlayerPrefs.GetInt("AdsCount") + 1);

                Debug.LogWarning("Interstitial ad is not ready yet.");
            }
        }

        public void LoadRewardedAd()
        {

#if UNITY_ANDROID
            string _adUnitId = "ca-app-pub-3940256099942544/5224354917";

#elif UNITY_IPHONE
            string _adUnitId = "ca-app-pub-3940256099942544/1712485313";

#else
            string _adUnitId = "unused";

#endif

            // Clean up the old ad before loading a new one.
            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
                rewardedAd = null;
            }

            Debug.Log("Loading the rewarded ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            RewardedAd.Load(_adUnitId, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Rewarded ad loaded with response : "
                              + ad.GetResponseInfo());

                    rewardedAd = ad;
                });

            //RegisterEventHandlers(rewardedAd);
        }

        public void ShowRewardedAd()
        {
            const string rewardMsg =
                "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

            if (rewardedAd != null && rewardedAd.CanShowAd())
            {
                rewardedAd.Show((Reward reward) =>
                {
                    // TODO: Reward the user.
                    Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                });
            }
        }

        private void RegisterEventHandlers(RewardedAd ad)
        {
            // Raised when an impression is recorded for an ad.
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Rewarded ad recorded an impression.");
            };

            // Raised when a click is recorded for an ad.
            ad.OnAdClicked += () =>
            {
                Debug.Log("Rewarded ad was clicked.");
            };

            // Raised when an ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Rewarded ad full screen content opened.");
            };

            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded ad full screen content closed.");
            };

            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content " +
                               "with error : " + error);
            };
        }
    }
}