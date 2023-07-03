using UnityEngine;
using UnityEngine.UI;

public class RubikUI : MonoBehaviour
{
	[SerializeField] RubikCube rubikCube;
	[SerializeField] Button solveButton;
	[SerializeField] Button scrambleButton;
	[SerializeField] RubikDance dance;

	void Start()
    {
		solveButton.onClick.AddListener(Solve);
		scrambleButton.onClick.AddListener(Scramble);

	}
	void Solve()
	{
		rubikCube.Solve();
		dance.StartDance();
	}
	void Scramble()
	{
		rubikCube.Scramble();
		dance.StartDance();
	}
}
