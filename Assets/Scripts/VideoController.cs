using UnityEngine;
using System.Collections;

public class VideoController : MonoBehaviour 
{
	// Singleton instance
	public static VideoController Instance;

	// public variables
	public Transform cameraAnchor;
	public MediaPlayerCtrl srcMedia;

	// private variables
	GameObject[] ga;

	// Use this for initialization
	void Start () 
	{
		Instance = this;

		ga = GameObject.FindGameObjectsWithTag ("Text");

		HideLabels ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckVideoPlayerState ();
		CheckVideoLabels ();
	}

	void CheckVideoLabels ()
	{
		if (srcMedia.GetCurrentState ().Equals (MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING))
		{
			if (srcMedia.m_strFileName.Equals ("Loop.mp4"))
			{
				for (int i = 1; i <= 6; i++)
				{
					GameObject.Find ("0"+i).GetComponent<MeshRenderer> ().material = Resources.Load ("0"+i) as Material;
				}
			}
		}

		Ray ray = new Ray (cameraAnchor.position, cameraAnchor.forward);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit)) 
		{
			if (hit.collider.gameObject.tag.Equals ("Text")) 
			{
				if (srcMedia.m_strFileName.Equals ("Loop.mp4"))
				{
					hit.collider.gameObject.GetComponent<MeshRenderer> ().material = Resources.Load (hit.collider.gameObject.name+"_hover") as Material;	
				}
			}
		}
	}

	void CheckVideoPlayerState ()
	{
		if (srcMedia.GetCurrentState ().Equals (MediaPlayerCtrl.MEDIAPLAYER_STATE.END))
		{
			if (srcMedia.m_strFileName.Equals ("Intro.mp4"))
			{
				InteractionController.Instance.exitExperience.SetActive (true);

				StartVideoPlayer ("Entry.mp4", false);
			}
			else if (srcMedia.m_strFileName.Equals ("Entry.mp4") || srcMedia.m_strFileName.Equals ("Set_01.mp4") ||
				srcMedia.m_strFileName.Equals ("Set_02.mp4") || srcMedia.m_strFileName.Equals ("Set_03.mp4") || 
				srcMedia.m_strFileName.Equals ("Set_04.mp4") || srcMedia.m_strFileName.Equals ("Set_05.mp4") || 
				srcMedia.m_strFileName.Equals ("Set_06.mp4"))
			{
				StartVideoPlayer ("Loop.mp4", true);
				ShowLabels ();
			}
			else if (srcMedia.m_strFileName.Equals ("Outro_1.mp4"))
			{
				StartVideoPlayer ("Outro_2.mp4", true);
			}
		}
	}
		
	public void StartVideoPlayer (string fileName, bool fileLoop)
	{
		srcMedia.UnLoad ();
		srcMedia.Load (fileName);
		srcMedia.m_bLoop = fileLoop;
		srcMedia.Play ();
	}

	public void ShowLabels ()
	{
		foreach (GameObject g in ga) 
		{
			g.GetComponent<MeshRenderer>().enabled = true;
		}

		InteractionController.Instance.exitExperience.SetActive (true);
	}

	public void HideLabels ()
	{
		foreach (GameObject g in ga) 
		{
			g.GetComponent<MeshRenderer>().enabled = false;
		}

		InteractionController.Instance.exitExperience.SetActive (false);
	}
}
