using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchHandle : MonoBehaviour {

    [SerializeField] Rigidbody2D rb, anchor, playerRb;
    [SerializeField] SpriteRenderer rend;
    [SerializeField] SpringJoint2D spring;
    [SerializeField] bool isPressed;
    [SerializeField] float releaseTime = 0.15f;
    [SerializeField] float maxDragDistance = 2;
    [SerializeField] Transform player;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] LineRenderer lineRend;
    [SerializeField] Animator anim;


    private void OnMouseDown()
    {
        if (playerManager.isGrounded)
        {
            isPressed = true;
            rb.isKinematic = true;
            lineRend.enabled = true;
        }
    }

    private void OnMouseUp()
    {
        if (playerManager.isGrounded)
        {
            isPressed = false;
            rb.isKinematic = false;
            StartCoroutine(Release());
            lineRend.enabled = false;
        }
    }

    private void Update()
    {
        DragHandle();

        ///for debug purposes so i don't have to stop the simulation and start again.
        //"Soft" reset
        if (Input.GetKeyDown(KeyCode.N))
        {
            ResetHandle();
        }
        //"Full" reset.
        if (Input.GetButtonDown("Jump"))
        {
            playerRb.gameObject.transform.position = new Vector2(-2, -3);
            playerRb.velocity = Vector2.zero;
            ResetHandle();
        }
    }

    void DragHandle()
    {
        if (playerManager.isGrounded == false || playerManager.isAlive == false)            //If the player is in the air or dead, don't render the handle
        {
            rend.enabled = false;
        }
        else
        {
            rend.enabled = true;
        }

        if (isPressed && playerManager.isGrounded)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(mousePos, anchor.position) > maxDragDistance)
            {
                rb.position = anchor.position + (mousePos - anchor.position).normalized * maxDragDistance;
            }
            else
            {
                rb.position = mousePos;
            }

            lineRend.SetPosition(0, player.position);
            Vector2 dir = (anchor.position - rb.position).normalized;
            float distance = Vector2.Distance(anchor.position, rb.position);
            Vector2 PlayerPos = player.position; 

            lineRend.SetPosition(1,  PlayerPos + dir*distance*distance*1.8f);
            
            
            //lineRend.SetPosition(1, anchor.position*2 - rb.position*2 + new Vector2(player.position.x, player.position.y));
        }
    }



    IEnumerator Release()
    {
        yield return new WaitForSeconds(releaseTime);
        if (playerManager.isAlive)
        {
            playerRb.velocity = rb.velocity;                    //Don't launch a dead player.
        }
        playerRb.AddTorque(rb.velocity.x*-4f);
        ResetHandle();
        player.gameObject.GetComponent<PlayerManager>().waterRiseSpeed += 0.05f;
    }

    private void ResetHandle()
    {
        rb.velocity = Vector2.zero;
        spring.enabled = true;
        transform.position = anchor.position;
    }

    ///One fraction of a second after release, the current velocity of the handle is applied to the player
    ///character and the handle is immetiately reset. That way we don't have to worry about transferring
    ///forces or having the player character be in an awkward position.

}
