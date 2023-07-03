using UnityEngine;

class RubikGameManager : MonoBehaviour 
{
	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
			Application.Quit();
	}

}