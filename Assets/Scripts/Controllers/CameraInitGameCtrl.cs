using UnityEngine;
using System.Collections;

public class CameraInitGameCtrl : MonoBehaviour {

    GameObject MonsterGo = null;
	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {
       /* RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); ;
        Monster Monster;
        
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.name.Contains("TeamA"))
            {
                MonsterGo = hit.collider.gameObject;
                MonsterGo.transform.parent.transform.FindChild("InfosMonsters").gameObject.SetActive(true);
                Monster = Field.Instance.GetMonsterFromGo(MonsterGo);
                //print("Monster" + Monster);
                //Monster = Field.Instance.GetMonsterFromGo(hit.collider.gameObject);
                if (Input.GetMouseButtonDown(0))
                {
                    MonsterGo.transform.renderer.material.color = Color.red;
                    //Monster.isSelected = true;

                }
            }
        }
        else
        {
            if (MonsterGo != null)
            {
                MonsterGo.transform.parent.transform.FindChild("InfosMonsters").gameObject.SetActive(false);
            }
        }*/
    }
}
