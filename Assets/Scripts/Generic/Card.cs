using UnityEngine;

public interface Card<T>
{
    public void ApplyCardValues(T item);
    public void Destroy();
}
