using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Collections;

[XmlRoot]
public class Login : MonoBehaviour
{
    private InputField username;
    private InputField password;
    private Toggle saveUserName;
    private string url = "http://recry.de/Login.php";

    void Start()
    {
        this.username = GameObject.Find("Username").GetComponent<InputField>() as InputField;
        this.password = GameObject.Find("Password").GetComponent<InputField>() as InputField;
        this.saveUserName = GameObject.Find("SaveUser").GetComponent<Toggle>() as Toggle;

        DeserializeData();
    }

    public void LoginButton()
    {
        if (saveUserName.isOn)
        {
            StartCoroutine(CheckLogin());
            SerializeData();
        }
        else
        {
            StartCoroutine(CheckLogin());
        }


    }

    IEnumerator CheckLogin()
    {
        WWWForm form = new WWWForm();
        form.AddField("Username", this.username.text);
        form.AddField("Password", this.password.text);

        Utility.Username = this.username.text;

        WWW check = new WWW(url, form);

        yield return check;

        var json = JSON.Parse(check.text);

        if (!string.IsNullOrEmpty(check.error))
        {
            Debug.Log(check.error);
        }
        else
        {
            int adminNumber = int.Parse(json["IsAdmin"]);
            if (adminNumber == 1)
            {
                Debug.Log("Admin");
                Application.LoadLevel("MainMenu");
            }
            else
            {
                Application.LoadLevel("MainMenu");
            }
        }
    }

    void SerializeData()
    {
        XmlSerializer serialize = new XmlSerializer(typeof(string));

        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;

        using (XmlWriter writer = XmlWriter.Create("Usersettings.xml", settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("Login");
            writer.WriteElementString("User", this.username.text);
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
    }

    void DeserializeData()
    {
        if (File.Exists("Usersettings.xml"))
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            using (XmlReader reader = XmlReader.Create("Usersettings.xml", settings))
            {
                XmlDocument document = new XmlDocument();
                document.Load(reader);
                Debug.Log(document.OuterXml);
            }


        }
    }

}
