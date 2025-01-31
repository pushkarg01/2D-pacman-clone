using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    public Movement move { get; private set; }

    private void Awake()
    {
        this.move = GetComponent<Movement>();
    }

    #region Update Movements

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.move.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.move.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.move.SetDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.move.SetDirection(Vector2.right);
        }

        float angle = Mathf.Atan2(this.move.direction.y, this.move.direction.x);
        this.transform.rotation =Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }
    #endregion

    public void ResetState()
    {
        this.gameObject.SetActive(true);
        this.move.ResetState();
    }
}
