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
        SAEImage.SetActive(false);
        yield return new WaitForSeconds(2);
        LogoImage.SetActive(false);
        SAEImage.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Menu");
    }
}
