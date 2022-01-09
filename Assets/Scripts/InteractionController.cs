using UnityEngine;
using System.Collections;

public class InteractionController : MonoBehaviour 
{
	// Singleton instance
	public static InteractionController Instance;

	// public variables
	public Transform cameraAnchor;
	public GameObject exitExperience;

	// private variables
	Vector3 pos;

	// Use this for initialization
	void Start () 
	{
		Instance = this;

		pos = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckUserInteraction ();
	}

	void CheckUserInteraction ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			pos = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp (0))
		{
			var delta = Input.mousePosition-pos;

			if (delta == new Vector3 (0.0f, 0.0f, 0.0f))
			{
				CheckCollisionsAndEmitRayCast ();
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape))
		{
			VideoController.Instance.StartVideoPlayer ("Intro.mp4", false);
		}
	}

	void CheckCollisionsAndEmitRayCast ()
	{
		Ray ray = new Ray (cameraAnchor.position, cameraAnchor.forward);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit))
		{
			if (hit.collider.gameObject.tag.Equals ("Text"))
			{
				if (VideoController.Instance.srcMedia.m_strFileName.Equals ("Loop.mp4"))
				{
					hit.collider.gameObject.GetComponent<MeshRenderer> ().material = Resources.Load (hit.collider.gameObject.name+"_selected") as Material;

					VideoController.Instance.StartVideoPlayer ("Set_"+hit.collider.gameObject.name+".mp4", false);
				}
			}
			else if (hit.collider.gameObject.tag.Equals ("Exit"))
			{
				VideoController.Instance.StartVideoPlayer ("Outro_1.mp4", false);
				VideoController.Instance.HideLabels ();
			}
		}
	}
}
