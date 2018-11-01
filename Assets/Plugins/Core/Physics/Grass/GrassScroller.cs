//
// Scroller script for Grass
//

using UnityEngine;
using UnityEngine.Serialization;

namespace Kvant
{
    [RequireComponent(typeof(Grass))]
    [AddComponentMenu("Kvant/Grass Scroller")]
    public class GrassScroller : MonoBehaviour
    {
        [SerializeField] [FormerlySerializedAs("speed")]
        private float _speed;

        [SerializeField] [FormerlySerializedAs("yawAngle")]
        private float _yawAngle;

        public float yawAngle
        {
            get { return _yawAngle; }
            set { _yawAngle = value; }
        }

        public float speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private void Update()
        {
            var r = _yawAngle * Mathf.Deg2Rad;
            var dir = new Vector2(Mathf.Cos(r), Mathf.Sin(r));
            GetComponent<Grass>().offset += dir * _speed * Time.deltaTime;
        }
    }
}