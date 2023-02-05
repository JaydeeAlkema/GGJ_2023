using System.Collections;
using UnityEngine;

public class RefillingWaterPuddle : MonoBehaviour
{
	[SerializeField] private ParticleSystem waterDropsParticleSystem = default;
	[SerializeField] private GameObject waterPuddle = default;
	[SerializeField] private float timeToCompletion = 15f;
	[SerializeField] private int completedScale = 2;
	[SerializeField] private Vector2 timeToCooldown = new Vector2(10, 30);
	[SerializeField] private GameObject buffPrefab = default;
	[SerializeField] private bool completed = false;
	[SerializeField] private Collider2D collider2D = default;

	private void Start()
	{
		waterDropsParticleSystem.Stop();
		StartCoroutine(SpawnBuffInSeconds());
	}

	private IEnumerator SpawnBuffInSeconds()
	{
		waterPuddle.SetActive(false);
		collider2D.enabled = false;
		completed = false;
		float timeToSpawn = Random.Range(timeToCooldown.x, timeToCooldown.y);
		yield return new WaitForSeconds(timeToSpawn);
		waterPuddle.SetActive(true);
		waterDropsParticleSystem.Play();
		float timeElapsed = 0f;
		while (!completed)
		{
			float currentScale = Mathf.Lerp(0, completedScale, timeElapsed / timeToCompletion);
			timeElapsed += Time.deltaTime;
			waterPuddle.transform.localScale = new Vector3(currentScale, currentScale, currentScale);

			if (Mathf.Approximately(currentScale, completedScale))
			{
				completed = true;
				collider2D.enabled = true;
				waterDropsParticleSystem.Stop();
			}

			yield return null;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerController playerController = collision.GetComponent<PlayerController>();
		if (playerController)
		{
			StartCoroutine(SpawnBuffInSeconds());
		}
	}
}
