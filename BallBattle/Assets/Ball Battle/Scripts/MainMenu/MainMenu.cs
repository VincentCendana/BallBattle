using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Information")]
    public string mainScene;                    // scene name for the default game scene

    [Header("Menu Animation")]
    public GameObject fieldObject;              // game board/field for rotate animation
    public float rotateSpeed;                   // field rotation speed

    private void Start()
    {
        // reset/destroy game manager instance if exists
        if (GameManager.instance != null)
        {
            Destroy(GameManager.instance.gameObject);
        }
    }

    private void FixedUpdate()
    {
        // main menu animation
        RotateAnimation();
    }

    // rotate game board/field as the main menu animation
    private void RotateAnimation()
    {
        Vector3 rotateDirection = Vector3.zero;
        rotateDirection.y += rotateSpeed * Time.fixedDeltaTime; 
        fieldObject.transform.Rotate(rotateDirection);
    }

    // go to main game scene
    public void PlayGame()
    {
        StartCoroutine(LoadGameScene());
    }

    // exit game
    public void ExitGame()
    {
        StartCoroutine(QuitApplication());
    }

    // load scene with delay
    public IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(mainScene);

        yield break;
    }

    // exit game with delay
    private IEnumerator QuitApplication()
    {
        yield return new WaitForSeconds(0.5f);
        Application.Quit();

        yield break;
    }
}
