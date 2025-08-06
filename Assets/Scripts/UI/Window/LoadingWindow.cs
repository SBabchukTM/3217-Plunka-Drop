using Cysharp.Threading.Tasks;
using CustomEventBus;
using UnityEngine;
using UnityEngine.UI;
using CustomEventBus.Signals;
using TMPro;
using UnityEngine.SceneManagement;

namespace UI.Windows
{
    public class LoadingWindow : Window
    {
        [SerializeField] private Image _progressLoading;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private float _loadSpeed = 0.3f;

        private void Start()
        {
            StartLoading().Forget();
        }

        private async UniTaskVoid StartLoading()
        {
            float progress = 0f;

            while (progress < 1f)
            {
                progress += Time.deltaTime * _loadSpeed;
                progress = Mathf.Clamp01(progress);

                _progressLoading.fillAmount = progress;

                int percent = Mathf.RoundToInt(progress * 100f);
                _progressText.text = percent + "%";

                await UniTask.Yield();
            }

            await UniTask.Delay(500);
            Hide();

            SceneManager.LoadScene(StringConstants.MENU_SCENE);
        }

    }
}
