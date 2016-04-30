using UnityEngine;
using System.Collections;

public class Player3DScript : MonoBehaviour {


	// This script goes through each character and sets the tags of the shildren to be the tag of the parent
	void Start () 
     {

         foreach (Transform t in transform)
         {

             //t.gameObject.tag = "theTag";

             foreach (Transform child in t)
             {

                 child.tag = t.tag;
                 foreach (Transform child2 in child)
                 {
                     child2.tag = t.tag;
                     foreach (Transform child3 in child2)
                     {
                         child3.tag = t.tag;
                         foreach (Transform child4 in child3)
                         {
                             child4.tag = t.tag;
                             foreach (Transform child5 in child4)
                             {
                                 child5.tag = t.tag;
                                 foreach (Transform child6 in child5)
                                 {
                                     child6.tag = t.tag;
                                     foreach (Transform child7 in child6)
                                     {
                                         child7.tag = t.tag;
                                     }
                                 }
                             }
                         }
                     }
                 }

             }

         }


     }






	
	// Update is called once per frame
	void Update () {
	
	}



}
