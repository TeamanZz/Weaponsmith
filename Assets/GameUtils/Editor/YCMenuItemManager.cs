using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using AppLovinMax.Scripts.IntegrationManager.Editor;
using Network = AppLovinMax.Scripts.IntegrationManager.Editor.Network;
using System.Net;

namespace YsoCorp {
    namespace GameUtils {

        public class YCMenuItemManager {

            static string WEBSITE_URL = "https://gameutils.ysocorp.com/";

            static YCConfig YCCONFIGDATA;

            // GAMEUTILS
            private struct DataData {
                public GUUpdateData data;
            }

            private struct GUUpdateData {
                public bool isUpToDate;
                public string version;
                public string url;
            }

            // MAX
            static bool MAX_UPGRADING = false;
            static List<Network> MAX_NETWORKS_UPGRADING = new List<Network>();
            static List<string> MAX_NETWORKS_PATHS = new List<string>();
            static Network MAX_MAIN_UPGRADING = null;


            [InitializeOnLoadMethod]
            private static void Init() {
                if (YCCONFIGDATA == null) {
                    YCCONFIGDATA = Resources.Load<YCConfig>("YCConfigData");
                }
            }

            // ----------------------------------- Menu Items -----------------------------------

            [MenuItem("GameUtils/Open game's page", false, 1001)]
            static void MenuOpenGamePage() {
                if (YCCONFIGDATA.gameYcId != "") {
                    Application.OpenURL(WEBSITE_URL + YCCONFIGDATA.gameYcId + "/settings");
                } else {
                    EditorUtility.DisplayDialog("Game's web page", "Please enter your Game Yc ID in the YCConfigData.", "Ok");
                }
            }

            [MenuItem("GameUtils/Update GameUtils", false, 2001)]
            static void MenuUpdateGameutils() {
                UpdateGameutils();
            }

            [MenuItem("GameUtils/Upgrade Applovin", false, 2002)]
            static void MenuUpgradeMax() {
                if (MAX_MAIN_UPGRADING == null && MAX_NETWORKS_UPGRADING.Count == 0) {
                    StartUpgradeMax();
                } else {
                    Debug.Log("Applovin is currently upgrading");
                }
            }

            [MenuItem("GameUtils/Import Config", false, 2003)]
            static void MenuImportConfig() {
                YCCONFIGDATA.EditorImportConfig();
            }

            [MenuItem("GameUtils/Tools/Display Debug Window", false, 4001)]
            static void MenuToolDebugWindow() {
                YCDebugWindow window = EditorWindow.GetWindow<YCDebugWindow>(false, "GameUtils Debug Window");
                YCDebugWindow.Init();
                window.Show();
            }

            // ----------------------------------- Helping Methods -----------------------------------

            // UPDATE GAMEUTILS --------------------
            #region Update_Gameutils

            private static void UpdateGameutils() {
                GUUpdateData data = GetGUUpdateData();
                if (data.version != null) {
                    string dialogTitle = "";
                    string dialogMsg = "";
                    if (data.isUpToDate == false) {
                        dialogTitle = "GameUtils update available";
                        dialogMsg = "The version " + data.version + " will be downloaded";
                    } else {
                        dialogTitle = "GameUtils is up to date";
                        dialogMsg = "Game Utils is already up to date.\nDo you want to reimport the package?";
                    }
                    bool dialog = EditorUtility.DisplayDialog(dialogTitle, dialogMsg, "Yes", "Cancel");
                    if (dialog) {
                        YCEditorCoroutine.StartCoroutine(ImportGameutilsPackage(data));
                    }
                }
            }

            private static GUUpdateData GetGUUpdateData() {
                if (YCCONFIGDATA.gameYcId != "") {
                    string url = RequestManager.GetUrlEmptyStatic("games/sdk-version/" + YCCONFIGDATA.gameYcId + "?currentVersion=" + YCConfig.VERSION, true);
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    request.Method = "Get";
                    request.ContentType = "application/json";
                    try {
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                            using (var reader = new StreamReader(response.GetResponseStream())) {
                                GUUpdateData data = Newtonsoft.Json.JsonConvert.DeserializeObject<DataData>(reader.ReadToEnd()).data;
                                if (data.version != "") {
                                    return data;
                                } else {
                                    EditorUtility.DisplayDialog("Game Utils Update", "Error. Check your Game Yc ID or your connection.", "Ok");
                                    return new GUUpdateData();
                                }
                            }
                        }
                    } catch (Exception) {
                        EditorUtility.DisplayDialog("Game Utils Update", "Error. Check your Game Yc Id or your connection.", "Ok");
                        return new GUUpdateData();
                    }
                } else {
                    EditorUtility.DisplayDialog("Game Utils Update", "Please enter your Game Yc Id in the YCConfigData.", "Ok");
                    return new GUUpdateData();
                }
            }

            private static IEnumerator ImportGameutilsPackage(GUUpdateData data) {
                var path = Path.Combine(Application.temporaryCachePath, "Gameutils_" + data.version.Replace('.', '_') + ".unitypackage");
                var downloadHandler = new DownloadHandlerFile(path);

                UnityWebRequest webRequest = new UnityWebRequest(data.url) {
                    method = UnityWebRequest.kHttpVerbGET,
                    downloadHandler = downloadHandler
                };

                var operation = webRequest.SendWebRequest();

                while (!operation.isDone) {
                    yield return new WaitForSeconds(0.1f);
                }


#if UNITY_2020_1_OR_NEWER
                if (webRequest.result != UnityWebRequest.Result.Success)
#else           
                if (webRequest.isNetworkError || webRequest.isHttpError)
#endif          
                {
                    Debug.LogError("The version could not be downloaded.");
                } else {
                    AssetDatabase.ImportPackage(path, true);
                }
            }

            #endregion

            // UPGRADE MAX --------------------
            #region Upgrade_MAX
            private static void StartUpgradeMax() {
                string msg = "";
                var loadDataCoroutine = AppLovinEditorCoroutine.StartCoroutine(AppLovinIntegrationManager.Instance.LoadPluginData(data => {
                    if (data.AppLovinMax.CurrentToLatestVersionComparisonResult == MaxSdkUtils.VersionComparisonResult.Lesser) {
                        MAX_MAIN_UPGRADING = data.AppLovinMax;
                        msg += "The main plugin\n";
                        MAX_UPGRADING = true;
                    }
                    for (int i = 0; i < data.MediatedNetworks.Length; i++) {
                        if (IsMaxNetworkInstalled(data.MediatedNetworks[i]) && data.MediatedNetworks[i].CurrentToLatestVersionComparisonResult == MaxSdkUtils.VersionComparisonResult.Lesser) {
                            MAX_NETWORKS_UPGRADING.Add(data.MediatedNetworks[i]);
                            msg += data.MediatedNetworks[i].DisplayName + "\n";
                            MAX_UPGRADING = true;
                        }
                    }
                    if (MAX_UPGRADING) { // Anything needs an update

                        msg = "Pending upgrades : \n\n" + msg;

                        bool dialog = EditorUtility.DisplayDialog("Applovin Upgrade", msg, "Ok", "Cancel");
                        if (dialog) {
                            Debug.Log("Upgrading Applovin ...");
                            YCEditorCoroutine.StartCoroutine(ImportAllNetworks());
                            List<Network> tmpNetworks = MAX_NETWORKS_UPGRADING;
                            for (int i = 0; i < tmpNetworks.Count; i++) {
                                Network currentNetwork = tmpNetworks[i];
                                YCEditorCoroutine.StartCoroutine(DownloadMaxNetwork(tmpNetworks[i], () => MAX_NETWORKS_UPGRADING.Remove(currentNetwork)));
                            }
                            if (MAX_MAIN_UPGRADING != null) {
                                YCEditorCoroutine.StartCoroutine(DownloadMaxMainPlugin(MAX_MAIN_UPGRADING));
                            }
                        } else {
                            MAX_MAIN_UPGRADING = null;
                            MAX_NETWORKS_UPGRADING.Clear();
                            MAX_NETWORKS_PATHS.Clear();
                            Debug.Log("Applovin upgrading canceled");
                        }
                    } else {
                        EditorUtility.DisplayDialog("Applovin Upgrade", "Applovin is up to date.", "Ok");
                    }
                }));
            }

            private static bool IsMaxNetworkInstalled(Network network) {
                return string.IsNullOrEmpty(network.CurrentVersions.Unity) == false;
            }

            private static IEnumerator DownloadMaxNetwork(Network network, Action action) {
                var path = Path.Combine(Application.temporaryCachePath, network.DisplayName + ".unitypackage");
                var downloadHandler = new DownloadHandlerFile(path);

                UnityWebRequest webRequest = new UnityWebRequest(network.DownloadUrl) {
                    method = UnityWebRequest.kHttpVerbGET,
                    downloadHandler = downloadHandler
                };

                var operation = webRequest.SendWebRequest();

                while (!operation.isDone) {
                    yield return new WaitForSeconds(0.1f);
                }


#if UNITY_2020_1_OR_NEWER
                if (webRequest.result != UnityWebRequest.Result.Success)
#else           
                if (webRequest.isNetworkError || webRequest.isHttpError)
#endif          
                {
                    Debug.LogError("The network " + network.DisplayName + " could not be downloaded.");
                } else {
                    MAX_NETWORKS_PATHS.Add(path);
                }
                action();
            }

            private static IEnumerator DownloadMaxMainPlugin(Network applovin) {
                while (MAX_NETWORKS_UPGRADING.Count != 0) {
                    yield return new WaitForSeconds(0.1f);
                }
                YCEditorCoroutine.StartCoroutine(DownloadMaxNetwork(applovin, () => MAX_MAIN_UPGRADING = null));
            }

            private static IEnumerator ImportAllNetworks() {
                while (MAX_NETWORKS_UPGRADING.Count > 0 || MAX_MAIN_UPGRADING != null) {
                    yield return null;
                }

                foreach (string path in MAX_NETWORKS_PATHS) {
                    AssetDatabase.ImportPackage(path, false);
                }
                MAX_NETWORKS_PATHS.Clear();
            }
            #endregion

        }
    }
}