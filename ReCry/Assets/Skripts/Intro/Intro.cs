using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {

    public GameObject LogoImage;
    public GameObject SAEImage;
    void Start()
    {
        StartCoroutine(ChangeImage());
    }

    IEnumerator ChangeImage()
    {
        LogoImage.SetActive(false);
        SAEImage.SetActive(true);
        yield return new WaitForSeconds(4);
        SAEImage.SetActive(false);
        LogoImage.SetActive(true);
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Menu");
    }
}
