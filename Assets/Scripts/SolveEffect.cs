using UnityEngine;

public class SolveEffect : MonoBehaviour
{
	[SerializeField] Animator animator;
	[SerializeField] ParticleSystem particles;
	[SerializeField] AudioSource audioSource;

	[SerializeField] RubikCube rubikCube;

	private void Awake()
	{
		rubikCube.OnSolved += Play;		
	}

	public void Play()
	{
		if(animator != null)
			animator.SetTrigger("Play");
		if (particles != null)
			particles.Play();
		if (audioSource != null)
			audioSource.Play();
	}
}
