using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {

    public GameObject SAEImage;
    void Start()
    {
        StartCoroutine(ChangeImage());
    }

    IEnumerator ChangeImage()
    {
        SAEImage.SetActive(true);
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Menu");
    }
}
