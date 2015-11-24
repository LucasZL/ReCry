using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.Collections;

public class GetCharacterStats : MonoBehaviour
{
    private string getCharacterStatsUrl = "http://www.recry.de/getcharacterstats.php";
    public Text KD;
    public Text StatsText;
    public Text Level;

    void Start()
    {
        this.StatsText.text = string.Format("{0}'s Characterstats", Utility.Username);
        StartCoroutine(getstats());
    }

    IEnumerator getstats()
    {
        WWWForm characterform = new WWWForm();
        characterform.AddField("Username", Utility.Username);

        WWW check = new WWW(getCharacterStatsUrl,characterform);

        yield return check;

        var json = JSON.Parse(check.text);

        if (string.IsNullOrEmpty(check.text))
        {
            Debug.LogError("No Connection to database");
        }
        else
        {
            float killdeath = float.Parse(json["KD"]);
            this.KD.text = killdeath.ToString();
            int characterLevel = int.Parse(json["Level"]);
            this.Level.text = characterLevel.ToString();
        }
    }
}
