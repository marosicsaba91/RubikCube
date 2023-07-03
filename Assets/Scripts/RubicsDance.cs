using System.Collections;
using UnityEngine;

public class RubicsDance : MonoBehaviour
{
	[SerializeField] RubicCube rubicCube;
	[SerializeField] float subCubeDuration = 0.1f;
	[SerializeField] float subCubeDanceDuration = 1.5f;
	[SerializeField] float fullDistance = 0.5f;
	[SerializeField] AnimationCurve distanceOverTime;
	[SerializeField] AnimationCurve rotationOverTime;
	[SerializeField] AnimationCurve ScaleOverTime;

	[SerializeField] bool danceOnStart = false;
	[SerializeField] bool danceOnSolved = false;

	void Start()
	{
		if (danceOnSolved)
			rubicCube.OnSolved += StartDance;

		if (danceOnStart)
			StartDance();
	}

	public void StartDance() => StartCoroutine(Dance());

	IEnumerator Dance()
	{
		if (!rubicCube.enabled)
			yield break;
		rubicCube.enabled = false;

		float wait = 0;
		foreach (RubicSubBlock subBlock in rubicCube.subBlocks)
		{
			StartCoroutine(Dance(subBlock, wait));
			wait += subCubeDuration;
		}

		yield return new WaitForSeconds(subCubeDanceDuration + wait);
		rubicCube.enabled = true;
	}

	IEnumerator Dance(RubicSubBlock subBlock, float waitTime)
	{
		float time = 0;
		Vector3 startPosition = subBlock.transform.localPosition;
		Quaternion startRotation = subBlock.transform.localRotation;
		Vector3 startScale = subBlock.transform.localScale;
		Vector3 direction = startPosition;


		SetPose(subBlock, time / subCubeDanceDuration, startPosition, startRotation, startScale, direction);

		yield return new WaitForSeconds(waitTime);

		while (time < subCubeDanceDuration)
		{
			SetPose(subBlock, time / subCubeDanceDuration, startPosition, startRotation, startScale, direction);

			time += Time.deltaTime;
			yield return null;
		}
		subBlock.transform.SetLocalPositionAndRotation(startPosition, startRotation);
		subBlock.transform.localScale = startScale;
	}

	private void SetPose(RubicSubBlock subBlock, float time, Vector3 startPosition, Quaternion startRotation, Vector3 startScale, Vector3 direction)
	{
		float distance = fullDistance * distanceOverTime.Evaluate(time);
		float rotationAngle = 360 * rotationOverTime.Evaluate(time);
		Vector3 position = startPosition + direction * distance;
		Quaternion rotation = startRotation * Quaternion.AngleAxis(rotationAngle, direction);
		float scale = ScaleOverTime.Evaluate(time);

		subBlock.transform.SetLocalPositionAndRotation(position, rotation);
		subBlock.transform.localScale = startScale * scale;
	}
}
