using UnityEngine;

[ExecuteAlways]
public class LookAt : MonoBehaviour
{
	public Transform target;
	public bool lookOnlyOnAwake;

	public void Update()
	{
		transform.LookAt(target);
	} 
}