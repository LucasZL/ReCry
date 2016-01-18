//
//  AIAgent.cs
//  ReCry
//  
//  Created by Lucas Zacharias-Langhans, Kevin Holst on 17.01.2015
//  Copyright (c) 2015 ReCry. All Rights Reserved.
//

using UnityEngine;
using System.Collections;

public class AIAgent : MonoBehaviour 
{
	public GameObject startPoint;
	public GameObject startPointHigh;
	public GameObject endPoint;
	public GameObject endPointHigh;
	private float startHeight;
	private float endHeight;

	public float timer;
	private float timer1;
	private float timer2;
	private float timer3;
	private float timer4;
	private float timer5;
	private float timer6;
	private float timer7;
	private float timer8;

	public bool active = false;

	float range = 20.0f;
	LineRenderer line;
	public Material lineMaterial;
	
	void Start () 
	{
		startHeight = startPoint.transform.position.y;
		endHeight = endPoint.transform.position.y;

		line = GetComponent<LineRenderer>();
		line.SetVertexCount(2);
		line.material = lineMaterial;
		line.SetWidth(0.1f, 0.25f);
	}
	
	void FixedUpdate() 
	{
		if (timer <= 5.0f) 
		{
			line.enabled = false;
			timer1 += Time.deltaTime;
			transform.position = Vector3.Lerp(startPoint.transform.position, startPointHigh.transform.position, timer1 / 5.0f);
		}
		else if (timer <= 15.0f) 
		{
			line.enabled = false;
			timer2 += Time.deltaTime;
			transform.position = Vector3.Lerp(startPointHigh.transform.position, endPointHigh.transform.position, timer2 / 10.0f);
		}
		else if (timer <= 20.0f) 
		{
			line.enabled = false;
			timer3 += Time.deltaTime;
			transform.position = Vector3.Lerp(endPointHigh.transform.position, endPoint.transform.position, timer3 / 5.0f);
		}
		else if (timer <= 35.0f) 
		{
			Ray ray = new Ray(transform.position, transform.forward * 40);
			RaycastHit hit;
			transform.Rotate(new Vector3(0,40,0) * Time.deltaTime);
			transform.position = new Vector3(endPoint.transform.position.x, endHeight, endPoint.transform.position.z);
			line.enabled = true;
			line.SetPosition(0, ray.origin);

			if(Physics.Raycast(ray, out hit, 40))
			{
				line.SetPosition(1, hit.point);
				if(hit.rigidbody)
				{
					hit.rigidbody.AddForceAtPosition(transform.forward * 10, hit.point);
				}
			}
			else
			{
				line.SetPosition(1, ray.GetPoint(40));
			}
		}
		else if (timer <= 40.0f) 
		{
			line.enabled = false;
			timer5 += Time.deltaTime;
			transform.position = Vector3.Lerp(endPoint.transform.position, endPointHigh.transform.position, timer5 / 5.0f);
		}
		else if (timer <= 50.0f) 
		{
			line.enabled = false;
			timer6 += Time.deltaTime;
			transform.position = Vector3.Lerp(endPointHigh.transform.position, startPointHigh.transform.position, timer6 / 10.0f);
		}
		else if (timer <= 55.0f) 
		{
			line.enabled = false;
			timer7 += Time.deltaTime;
			transform.position = Vector3.Lerp(startPointHigh.transform.position, startPoint.transform.position, timer7 / 5.0f);
		}
		else if (timer <= 70.0f) 
		{
			Ray ray = new Ray(transform.position, transform.forward * 40);
			RaycastHit hit;
			transform.Rotate(new Vector3(0,40,0) * Time.deltaTime);
			transform.position = new Vector3(startPoint.transform.position.x, startHeight, startPoint.transform.position.z);
			line.enabled = true;
			line.SetPosition(0, ray.origin);
			
			if(Physics.Raycast(ray, out hit, 40))
			{
				line.SetPosition(1, hit.point);
				if(hit.rigidbody)
				{
					hit.rigidbody.AddForceAtPosition(transform.forward * 10, hit.point);
				}
			}
			else
			{
				line.SetPosition(1, ray.GetPoint(40));
			}
		}
		else if (timer <= 80.0f) 
		{
			timer = 0.0f;
			timer1 = 0.0f;
			timer2 = 0.0f;
			timer3 = 0.0f;
			timer4 = 0.0f;
			timer5 = 0.0f;
			timer6 = 0.0f;
			timer7 = 0.0f;
			timer8 = 0.0f;
		}
	}

	void Update()
	{
		timer += Time.deltaTime;
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(timer);
		}
		else
		{
			timer = (float)stream.ReceiveNext();
		}
	}
}
