
using UnityEngine;
using System.Collections;

//add using System.Collections.Generic; to use the generic list format
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    //-----------------------------------------------------------------------
    // Class Fields
    //-----------------------------------------------------------------------

	//vector to hold the center 
	public Vector3 centroid; 
	//to hold the average flock direction
	public Vector3 avgFlockDirec;

	public int numFlock = 10;

    public GameObject butterfly;
    public GameObject target;

	public GameObject butterflyPrefab;
	public GameObject[] butterflyFollowers;
    public GameObject targetPrefab;

    public Camera[] cameras;
    private int currentCameraIndex;
   

    //-----------------------------------------------------------------------
    // Start and Update
    //-----------------------------------------------------------------------
    void Start () {

        //camera number
        currentCameraIndex = 0;

        butterflyFollowers = new GameObject[numFlock];

        //Create the target (noodle)
        Vector3 pos = new Vector3(0, 40, 0);
        //make bird 
        target = (GameObject)Instantiate(targetPrefab, pos, Quaternion.identity);
        
        //make leader butterfly
		Vector3 pos1 = new Vector3 (-10,20,70);
		butterfly = (GameObject)Instantiate (butterflyPrefab, pos1, Quaternion.identity);

        //make follower butterflies
		for(int i = 0; i < butterflyFollowers.Length; i++) {
			 
			 butterflyFollowers[i] = (GameObject)Instantiate (butterflyPrefab, pos1, Quaternion.identity);
		}

       
        //Turn all cameras off, except the first default one
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }
        if (cameras.Length > 0)
        {
            cameras[0].gameObject.SetActive(true);
        }
    }
	

	void Update (){
		//target for leader butterfly
		butterfly.GetComponent<Seeker> ().seekerTarget = target;

		//target for follower butterflies
		for (int i = 0; i < butterflyFollowers.Length; i++) {
			butterflyFollowers[i].GetComponent<Seeker>().seekerTarget = butterfly;
		}

		CalcCentroid ();
		CalcFlockDirection ();

        //C To switch cameras
        if (Input.GetKeyDown(KeyCode.C)) 
        {
            currentCameraIndex++;
             
            if (currentCameraIndex < cameras.Length)
            {
                cameras[currentCameraIndex - 1].gameObject.SetActive(false);
                cameras[currentCameraIndex].gameObject.SetActive(true);
               
            }
            else
            {
                cameras[currentCameraIndex - 1].gameObject.SetActive(false);
                currentCameraIndex = 0;
                cameras[currentCameraIndex].gameObject.SetActive(true);

            }
        }

    }

    //calculate centroid
	void CalcCentroid(){

		//reset the position
		Vector3 position = Vector3.zero;

		//foreach loop to add the position to the obj position
		foreach (GameObject obj in butterflyFollowers) {
			position += obj.transform.position;
		}

		centroid = position / numFlock;

		//debug lines 
		for (int i = 0; i < butterflyFollowers.Length; i++) {
			Debug.DrawLine (butterflyFollowers[i].transform.position, centroid, Color.red);
		}
	}
	
    //calculate flock direction
	void CalcFlockDirection()
	{
		//reset the position
		avgFlockDirec = Vector3.zero;

		//foreach loop to add the objs forward vector 
		foreach (GameObject obj in butterflyFollowers) {
			avgFlockDirec += obj.transform.forward;
		}
		//normalize to get the direction
		avgFlockDirec.Normalize ();
	} 
}

