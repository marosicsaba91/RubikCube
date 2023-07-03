using UnityEngine;

class RubicGameManager : MonoBehaviour 
{
	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
			Application.Quit();
	}

}