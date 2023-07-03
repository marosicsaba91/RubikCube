using UnityEngine;
using UnityEngine.UI;

public class RubicUI : MonoBehaviour
{
	[SerializeField] RubicCube rubicCube;
	[SerializeField] Button solveButton;
	[SerializeField] Button scrambleButton;
	[SerializeField] RubicsDance dance;

	void Start()
    {
		solveButton.onClick.AddListener(Solve);
		scrambleButton.onClick.AddListener(Scramble);

	}
	void Solve()
	{
		rubicCube.Solve();
		dance.StartDance();
	}
	void Scramble()
	{
		rubicCube.Scramble();
		dance.StartDance();
	}
}
