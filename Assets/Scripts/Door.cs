using System.Threading.Tasks;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Rigidbody body;
    public float speed;
    public int time;
    [SerializeField] int waitTime;
    public async void Opendoor(int direction)
    {
        await Task.Delay(waitTime);
        if (direction == 0)
        {
            body.velocity = new Vector3(0, speed, 0);
            await Task.Delay(time);
            body.velocity = Vector3.zero;
        }
        if (direction == 1)
        {
            body.velocity = new Vector3(speed,0 , 0);
            await Task.Delay(time);
            body.velocity = Vector3.zero;
        }
        if (direction == 2)
        {
            body.velocity = new Vector3(0, -speed, 0);
            await Task.Delay(time);
            body.velocity = Vector3.zero;
        }
        if (direction == 3)
        {
            body.velocity = new Vector3(-speed, 0, 0);
            await Task.Delay(time);
            body.velocity = Vector3.zero;
        }
    }
}
