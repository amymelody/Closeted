using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {

    public int numChoices;
    public int choice;

	// Use this for initialization
	void Start () 
    {
        choice = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (choice < numChoices - 1)
            {
                choice++;
                transform.position -= new Vector3(0f, 0.71f, 0f);
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (choice > 0)
            {
                choice--;
                transform.position += new Vector3(0f, 0.71f, 0f);
            }
        }
	}
}
