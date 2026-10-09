using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Expelled.Player;

namespace Expelled.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private AudioSource _clickSound;

        private UIDocument _document;
        private Button _startButton;
        private Button _settingsButton;
        private Button _quitButton;

        private List<Button> _menuButtons = new List<Button>();

        private void Awake() {
            _document = GetComponent<UIDocument>();
            var root = _document.rootVisualElement;

            _startButton = root.Q<Button>("Play");
            _settingsButton = root.Q<Button>("Settings");
            _quitButton = root.Q<Button>("Quit");

            _startButton?.RegisterCallback<ClickEvent>(OnStartButtonClick);
            _settingsButton?.RegisterCallback<ClickEvent>(OnSettingsButtonClick);
            _quitButton?.RegisterCallback<ClickEvent>(OnQuitButtonClick);
        }

        private void OnDisable() {
            _startButton?.UnregisterCallback<ClickEvent>(OnStartButtonClick);
            _settingsButton?.UnregisterCallback<ClickEvent>(OnSettingsButtonClick);
            _quitButton?.UnregisterCallback<ClickEvent>(OnQuitButtonClick);
        }

        private void OnStartButtonClick(ClickEvent evt) {
            _clickSound?.Play();
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            PlayerState.Reset();
            SceneManager.LoadScene("StoryScene");
        }

        private void OnSettingsButtonClick(ClickEvent evt) {
            _clickSound?.Play();
            SceneManager.LoadScene("SettingsScene");
        }

        private void OnQuitButtonClick(ClickEvent evt) {
            _clickSound?.Play();
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
