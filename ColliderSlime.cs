using UnityEngine;

public class DeterministicCollider : MonoBehaviour
{
    public int width = 1;
    public int height = 1;

    private int[] collisionGrid;
    private int velocityX;
    private int velocityY;
    private int x; // Declare x as a class-level variable
    private int y; // Declare y as a class-level variable
    public DeterministicCollider other; // Reference to the other object

    void Start()
    {
        collisionGrid = new int[width * height];
    }

    void FixedUpdate()
    {
        // Update the collision grid's position based on the GameObject's position
        x = (int)transform.position.x;
        y = (int)transform.position.y;

        // Update the collision grid
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int gridIndex = i + j * width;
                collisionGrid[gridIndex] = 1; // Mark the grid cell as occupied
            }
        }

        // Check for collisions with other objects
        if (IsColliding(other))
        {
            PreventCollision(other);
        }

        // Update the position of the object based on its velocity
        x += velocityX;
        y += velocityY;
        transform.position = new Vector3(x, y, 0);
    }

    public bool IsColliding(DeterministicCollider other)
    {
        // Check if the two objects are colliding by comparing their collision grids
        int otherX = other.x;
        int otherY = other.y;

        // Check if the objects are overlapping in the x and y directions
        if (x < otherX + other.width && x + width > otherX && y < otherY + other.height && y + height > otherY)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int gridIndex = i + j * width;
                    if (collisionGrid[gridIndex] == 1 && other.collisionGrid[gridIndex] == 1)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void PreventCollision(DeterministicCollider other)
    {
        // Calculate the overlap between the two objects
        int overlapX = Mathf.Min(x + width, other.x + other.width) - Mathf.Max(x, other.x);
        int overlapY = Mathf.Min(y + height, other.y + other.height) - Mathf.Max(y, other.y);

        if (overlapX > overlapY)
        {
            // Move the object in the x direction
            if (x < other.x)
            {
                x -= overlapX;
            }
            else
            {
                x += overlapX;
            }
        }
        else
        {
            // Move the object in the y direction
            if (y < other.y)
            {
                y -= overlapY;
            }
            else
            {
                y += overlapY;
            }
        }
    }
}