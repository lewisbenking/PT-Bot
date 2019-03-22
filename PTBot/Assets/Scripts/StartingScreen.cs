using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingScreen : MonoBehaviour
{
    private GameObject pt;
    private static Animator animator;

    void Start()
    {
        pt = GameObject.Find("PT");
        animator = pt.GetComponent<Animator>();
        animator.SetTrigger("StartingScreen");
    }

    public void UseJimBot()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitApp()
    {
        Debug.Log("Exit Pressed");
        Application.Quit();
    }
}
