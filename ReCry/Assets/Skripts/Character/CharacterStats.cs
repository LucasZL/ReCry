using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterStats : MonoBehaviour {

    public float Armor;
    public float Life;
    public Text LifeText;

	// Use this for initialization
	void Start ()
    {
        this.Life = 100;
        this.LifeText = GameObject.FindWithTag("LifeText").GetComponent<Text>() as Text;
        this.LifeText.text = Life.ToString();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Life <= 0)
        {
            Debug.Log("TOT");
        }
	}


    public void GetDamage()
    {
        this.Life -= 100;
    }


}
