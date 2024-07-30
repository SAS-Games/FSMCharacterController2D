using UnityEngine;

public class MovingPlatformPlayerMovement : MonoBehaviour
{
    private Transform _player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_player == null && collision.transform.CompareTag("Player"))
        {
            _player = collision.transform;
            _player.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (_player == collision.transform)
            {
                _player.transform.SetParent(null);
                _player = null;
            }
        }
    }
}
