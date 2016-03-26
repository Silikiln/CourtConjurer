using UnityEngine;
using System.Collections;

public class SceneHandler : MonoBehaviour {
    public GameObject ritualToOpen;
	// Use this for initialization
	void Start () {
        //launch ritual stuff
        ritualToOpen.GetComponent<Ritual>().ShowRitual();
	}
	
}
