using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OneBtnInput input;
    [SerializeField] private GameObject shieldPrefab;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private float dashForce = 30f;
    [SerializeField] private float gravityMultiplier = 3.35f;
    [SerializeField, Range(0f, 1f)] private float dashFactor = 0.4f;

    [Header("Bounce Settings")]
    [SerializeField] private float bounceForce = 200;
    [SerializeField] private float maxBounceDelay = 0.5f; // max seconds between dash & collision to count as bounce

    //Status & Health
    private bool isAlive = true;
    public bool isGrounded = false;
    private bool isDashing = false;
    private bool doubleJumpDone = false;
    private Rigidbody rb;

    private GameObject activeShield;
    private float lastDashTime;

    private Vector3 lastDashDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Prevent tipping over

        // Subscribe to input
        input.OnSingleClick += Jump;
        input.OnDoubleTap += Dash;
        input.OnHold += Shield;
    }

    private void Update()
    {
        if (!isAlive) return;

        // Constant forward movement — only when not dashing or bouncing
        if (!isDashing)
        {
            if (isGrounded)
                // Apply horizontal movement only while grounded
                rb.velocity = new Vector3(moveSpeed, rb.velocity.y, 0);
            //else if (!Physics.Raycast(transform.position, Vector3.right * Mathf.Sign(moveSpeed), 0.6f))
            //    rb.AddForce(Vector3.right, ForceMode.Impulse);
        }

        // Extra gravity for better jump feel
        if (!isGrounded && !isDashing)
            rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
    }

    private void FixedUpdate()
    {
        // Ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        if (isGrounded)
            doubleJumpDone = false;

        //draw movement
        Debug.DrawRay(transform.position, Vector3.right * Mathf.Sign(moveSpeed), Color.red, 0.6f);

        /*// Wall unstick — if moving toward wall and barely moving horizontally
        if (!isGrounded && !isDashing)
        {
            // Slight downward nudge if touching wall
            if (Physics.Raycast(transform.position, Vector3.right * Mathf.Sign(moveSpeed), 0.6f))
                rb.AddForce(Vector3.down, ForceMode.VelocityChange);
        }*/
    }


    // ---------- ACTIONS ----------
    private void Jump()
    {
        if (isDashing || doubleJumpDone)
            return;

        if (!isGrounded)
            doubleJumpDone = true;

        rb.velocity = new Vector3(rb.velocity.x, 0, 0); // reset vertical speed
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void Dash()
    {
        if (isGrounded || isDashing)
            return;

        StartCoroutine(DashUntilCollision());
    }

    private IEnumerator DashUntilCollision()
    {
        if (Physics.Raycast(transform.position, Vector3.right * Mathf.Sign(moveSpeed), 0.6f))
            yield break;

        isDashing = true;
        lastDashTime = Time.time;

        Vector3 dashDir = (Vector3.right + dashFactor * Vector3.down).normalized;
        lastDashDir = dashDir;

        rb.velocity = Vector3.zero;

        while (isDashing)
        {
            rb.velocity = dashDir * dashForce;
            yield return null;
        }
    }


    private void Shield()
    {
        if (isGrounded || activeShield != null) return; // Only one at a time

        activeShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
        activeShield.transform.SetParent(transform);
    }

    // ---------- COLLISIONS ----------
    private void OnCollisionEnter(Collision collision)
    {
        if (!isDashing) return;

        float timeSinceDash = Time.time - lastDashTime;
        ContactPoint contact = collision.contacts[0];
        Vector3 normal = contact.normal;
        Vector3 contactPoint = contact.point;

        // Too old -> stop dash (do not consume shield)
        if (timeSinceDash > maxBounceDelay)
        {
            isDashing = false;
            Debug.Log($"Dash too old: {timeSinceDash:F3}s (max {maxBounceDelay:F3})");
            Debug.DrawRay(contactPoint, normal * 1f, Color.red, 2f);
            return;
        }

        // No shield -> stop dash
        if (activeShield == null)
        {
            isDashing = false;
            Debug.Log("No shield: stopping dash");
            Debug.DrawRay(contactPoint, normal * 1f, Color.red, 2f);
            return;
        }

        // Valid bounce: show debug rays for inspection
        Debug.Log("Valid bounce (consuming shield)");
        Debug.DrawRay(contactPoint, normal * 1f, Color.red, 3f);       // surface normal
        Debug.DrawRay(contactPoint, lastDashDir * 2f, Color.blue, 3f); // incoming
                                                                       // consume shield
        Destroy(activeShield);
        activeShield = null;

        StopAllCoroutines();
        StartCoroutine(Bounce(normal, contactPoint));
    }

    private IEnumerator Bounce(Vector3 surfaceNormal, Vector3 contactPoint)
    {
        isDashing = true;
        lastDashTime = Time.time;

        // get a stable incoming direction (prefer stored lastDashDir; fallback to rb.velocity)
        Vector3 inDir = lastDashDir.sqrMagnitude > 0.0001f ? lastDashDir.normalized : rb.velocity.normalized;

        // reflect using the surface normal
        Vector3 reflectDir = Vector3.Reflect(inDir, surfaceNormal).normalized;

        // SANITY: ensure reflectDir points away from the surface
        // If dot < 0 => reflectDir is pointing into the surface (bad), flip it.
        if (Vector3.Dot(reflectDir, surfaceNormal) < 0f)
        {
            reflectDir = -reflectDir;
        }

        // small outward bias so floor bounces go more upward and wall bounces go more outward
        float vertical = Mathf.Abs(surfaceNormal.y);
        if (vertical > 0.5f) // mostly floor-like surface
        {
            reflectDir = (reflectDir + Vector3.up * 0.25f).normalized;
        }
        else // mostly wall-like surface
        {
            // push horizontally away from wall (sign depends on normal.x)
            reflectDir = (reflectDir + Vector3.right * -Mathf.Sign(surfaceNormal.x) * 0.25f).normalized;
        }

        // debug rays (contact point)
        Debug.DrawRay(contactPoint, surfaceNormal * 1.5f, Color.red, 3f);
        Debug.DrawRay(contactPoint, inDir * 2.5f, Color.blue, 3f);
        Debug.DrawRay(contactPoint, reflectDir * 2.5f, Color.green, 3f);

        // apply bounce impulse
        rb.velocity = Vector3.zero;
        rb.drag = 0f;
        rb.AddForce(reflectDir * bounceForce, ForceMode.VelocityChange);

        // remember direction for chaining
        lastDashDir = reflectDir;

        // short buffer so immediate re-collisions don't override everything
        float buffer = 0.12f;
        float start = Time.time;
        while (Time.time - start < buffer)
            yield return null;

        // wait until grounded to return to normal movement (keeps airborne chaining possible)
        yield return new WaitUntil(() => isGrounded);

        isDashing = false;
    }

    // ---------- DAMAGE ----------
    public void TakeDamage()
    {
        if (activeShield != null)
        {
            Destroy(activeShield);
            activeShield = null;
        }
        else
        {
            isAlive = false;
            rb.velocity = Vector3.zero;
            Debug.Log("Player Dead");
        }
    }
}
