using UnityEngine;
public class CameraController : MonoBehaviour
{
	[SerializeField] CharacterManager cm;
	[SerializeField] Transform player;
	void Update()
	{
		if (cm.isPossessing)
		{
			transform.position = new Vector3(cm.enemy.transform.position.x, cm.enemy.transform.position.y + 10, transform.position.z);
		}
		else if (cm.isGhost)
		{
			transform.position = new Vector3(cm.ghost.transform.position.x, cm.ghost.transform.position.y + 5, transform.position.z);
		}		
		else
		{
			transform.position = new Vector3(cm.avatar.transform.position.x, cm.avatar.transform.position.y + 10, transform.position.z);
		}
	}
}
