using UnityEngine;
using System.Collections;

public class MapRotation : MonoBehaviour {
    [Range(0, 86400)]
    public float SecondsOfDay = 43200;
    [Range(0.1f, 60.0f)]
    public float DayDuration = 5;
    private float dayDurationSeconds;
    private GameObject DirectionalLight;
    private float timer;
    private int updateLock = 40;
    private int updateLockCorrection;
    float updateRaise;

    void Start()
    {
        updateLockCorrection = 1000 / updateLock;
        dayDurationSeconds = DayDuration * 60;
        updateRaise = 86400 / dayDurationSeconds;
        DirectionalLight = this.gameObject;
        float xRotation = SecondsOfDay / 240.0f;
    }

    void Update()
    {
        this.timer += Time.deltaTime * 1000;
        if (this.timer >= updateLock)
        {
            if ((SecondsOfDay + updateRaise) / updateLockCorrection > 86400)
            {
                SecondsOfDay = 0 + ((86400 - SecondsOfDay + updateRaise) / updateLockCorrection);

            }
            else
            {
                SecondsOfDay += updateRaise / updateLockCorrection;
            }

            float xRotation = SecondsOfDay / 240.0f;
            DirectionalLight.transform.rotation = Quaternion.Euler(new Vector3(0, xRotation, 90));
            this.timer = 0;
        }
    }
}
