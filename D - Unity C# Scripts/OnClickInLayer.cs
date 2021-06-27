using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickInLayer : MonoBehaviour
{
 
public GameObject Geometry;
public Transform NewParent;
public Transform Pharmacy;
public Transform Bank;
public Transform Food_and_drinks;
public Transform Stores;

public void SetNewParent (int ParentLayerIndex)
{
     if (ParentLayerIndex == 0)
     {
          NewParent = Food_and_drinks;
     }

     if (ParentLayerIndex == 1)
     {
          NewParent = Bank;
     }

     if (ParentLayerIndex == 2)
     {
          NewParent = Pharmacy;
     }

     if (ParentLayerIndex == 3)
     {
          NewParent = Stores;
     }    
     
} 

public void Update()
{
     if (Input.GetButtonDown("Fire1"))
          PlaceNewAmenity(Input.mousePosition, NewParent);
}
 
public void PlaceNewAmenity(Vector2 mousePosition, Transform newParent)
{
     RaycastHit hit = RayFromCamera(mousePosition, 1000.0f);
     GameObject.Instantiate(Geometry, hit.point, Quaternion.identity, NewParent);
}
 
public RaycastHit RayFromCamera(Vector3 mousePosition, float rayLength)
{
     RaycastHit hit;
     Ray ray = Camera.main.ScreenPointToRay(mousePosition);
     Physics.Raycast(ray, out hit, rayLength);
     return hit;
}
}
