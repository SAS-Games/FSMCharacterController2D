using SAS.Utilities.TagSystem;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.SceneManagement
{
    public interface ISceneLoader : IBindable
    {

    }
    public class SceneLoader : MonoBehaviour, ISceneLoader
    {
        [SerializeField] Image loadingBar;
        [SerializeField] float fillSpeed = 0.5f;
        [SerializeField] Canvas loadingCanvas;
        [SerializeField] Camera loadingCamera;
        [SerializeField] SceneGroup[] sceneGroups;

        float targetProgress;
        bool isLoading;

        public readonly SceneGroupManager manager = new SceneGroupManager();

        void Awake()
        {
            // TODO can remove
            //manager.OnSceneLoaded += sceneName => Debug.Log("Loaded: " + sceneName);
            //manager.OnSceneUnloaded += sceneName => Debug.Log("Unloaded: " + sceneName);
            //manager.OnSceneGroupLoaded += () => Debug.Log("Scene group loaded");
        }

        async void Start()
        {
            await LoadSceneGroup(0);
        }

        void Update()
        {
            if (!isLoading) return;

            float currentFillAmount = loadingBar.fillAmount;
            float progressDifference = Mathf.Abs(currentFillAmount - targetProgress);

            float dynamicFillSpeed = progressDifference * fillSpeed;

            loadingBar.fillAmount = Mathf.Lerp(currentFillAmount, targetProgress, Time.deltaTime * dynamicFillSpeed);
        }

        public async Task LoadSceneGroup(string groupName, bool ignoreOptional = false)
        {
            int index = Array.FindIndex(sceneGroups, sceneGroup => sceneGroup.GroupName == groupName);
            if (index == -1) return;
            await LoadSceneGroup(index, ignoreOptional);
        }

        public async Task LoadSceneGroup(int index, bool ignoreOptional = false)
        {
            if (loadingBar)
                loadingBar.fillAmount = 0f;
            targetProgress = 1f;

            if (index < 0 || index >= sceneGroups.Length)
            {
                Debug.LogError("Invalid scene group index: " + index);
                return;
            }

            LoadingProgress progress = new LoadingProgress();
            progress.Progressed += target => targetProgress = Mathf.Max(target, targetProgress);

            EnableLoadingCanvas();
            await manager.LoadScenes(sceneGroups[index], progress, false, ignoreOptional);
            EnableLoadingCanvas(false);
        }

        void EnableLoadingCanvas(bool enable = true)
        {
            if (!loadingCanvas)
                return;
            isLoading = enable;
            loadingCanvas.gameObject.SetActive(enable);
            if (loadingCamera)
                loadingCamera.gameObject.SetActive(enable);
        }

        public void OnInstanceCreated()
        {
        }
    }

    public class LoadingProgress : IProgress<float>
    {
        public event Action<float> Progressed;

        const float ratio = 1f;

        public void Report(float value)
        {
            Progressed?.Invoke(value / ratio);
        }
    }
}