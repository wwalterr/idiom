
using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.SceneManagement;

public class PrefabEnemy : MonoBehaviour {

	private Status status;

	private PlayerScript playerScript;

	private EnemyScript enemyScript;

	public static int pointsCounter;

	private GameObject player;

	public AudioSource rightSource, wrongSource, over;

	private void Start () {
		pointsCounter = 3;
		
		status = GameObject.Find ("Canvas").GetComponent<Status> ();

		player = GameObject.Find ("Player").gameObject;

		enemyScript = GameObject.Find ("Enemy").GetComponent<EnemyScript> ();

		AudioSource[] soudeSource = GetComponents<AudioSource> ();

		rightSource = soudeSource[0];

		wrongSource = soudeSource[1];

		over = soudeSource[2];

		status.SetPoints (pointsCounter);		
	}

	private void Update() {
	}

	void OnTriggerEnter (Collider obj) {
		if (obj.tag == "Player") {
			if (status.cacheWord.Split (' ') [0] == transform.Find ("Text Enemy").gameObject.GetComponent<TextMesh> ().text.ToString ().Split (' ') [0]) {
				pointsCounter += 1;

				status.SetPoints (pointsCounter);

				rightSource.Play ();

				player.transform.position = new Vector3 (0f, 1f, 0f);

				player.transform.Rotate (0f, 0f, 0f, Space.World);

				enemyScript.SetAllWords ();

				status.UpdateWordStatus ();

				if(pointsCounter >= 45) {
					SceneManager.LoadScene("Win");
				}
			} else {
				player.transform.position = new Vector3 (0f, 1f, 0f);

				player.transform.Rotate (0f, 0f, 0f, Space.World);

				pointsCounter -= 1;
				
				status.SetPoints (pointsCounter);
				
				if (pointsCounter == 0)
					SceneManager.LoadScene("Fail");

				enemyScript.SetAllWords ();

				status.UpdateWordStatus ();
			
				wrongSource.Play ();
			}

			if(pointsCounter >= 10) {
				StartCoroutine(WaitEnemy());
			}

			if(pointsCounter >= 20) {
				StartCoroutine(WaitChangeWords());
			}
		} // Player
	}

	IEnumerator WaitEnemy()
	{
		int cache = pointsCounter;

		yield return new WaitForSeconds(5);

		if(cache == pointsCounter) {
			pointsCounter -= 1;

			status.SetPoints (pointsCounter);
		}
	}

	IEnumerator WaitChangeWords()
	{
		yield return new WaitForSeconds(5);

		enemyScript.SetAllWords ();

		status.UpdateWordStatus ();
	}
}