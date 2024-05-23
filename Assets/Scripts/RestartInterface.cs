#region

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#endregion

public class RestartInterface : MonoBehaviour
{
    [SerializeField] private Button restartButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().name); });
    }

    private void Start()
    {
        AllCharactersController.OnAllPlayersDead += AllCharactersController_OnAllPlayersDead;

        Hide();
    }

    private void AllCharactersController_OnAllPlayersDead(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}