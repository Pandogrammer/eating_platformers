using UnityEngine;

namespace Foods
{
    public interface Modeled<T>
    {
        void UpdateModel(T model);
    }
}