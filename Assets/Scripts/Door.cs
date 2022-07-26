using System.Threading.Tasks;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Rigidbody body;
    public float speed;
    public int time;
    public async void Opendoor()
    {
        body.velocity = new Vector3(0, speed, 0);
        await Task.Delay(time);
        body.velocity = Vector3.zero;
    }
}
