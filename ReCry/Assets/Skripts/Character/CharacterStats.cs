using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterStats : MonoBehaviour {

    public float Armor;
    public float Life;
    private Text LifeText;

	// Use this for initialization
	void Start ()
    {
        this.Life = 100;
        this.LifeText = GameObject.FindWithTag("LifeText").GetComponent<Text>() as Text;
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateText();
        if (Life <= 0)
        {
            Debug.Log("TOT");
        }
	}


    public void GetDamage()
    {
        this.Life -= 100;
    }

    void UpdateText()
    {
        this.LifeText.text = string.Format("Life: {0}", Life);
    }


}
