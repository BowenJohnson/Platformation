using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class moves background objects at different rates relative to camera
// to create the illusion of depth in a 2-D world
public class Parallaxer : MonoBehaviour
{
    [SerializeField] private Vector2 _parallaxMultiplier;

    // included toggle for different directions in Unity
    [SerializeField] private bool _infiniteX;
    [SerializeField] private bool _infiniteY;

    // reference to the camera
    private Transform _cameraTransform;

    // store camera's last position
    private Vector3 _lastCameraPosition;

    // how much the camera moved since last frame
    private Vector3 _movementChange;

    // camera's offset position in the X direction
    private float _offsetX;

    // camera's offset position in the Y direction
    private float _offsetY;

    // reference to the sprite, textures, and size
    private Sprite _sprite;
    private Texture2D _texture;
    private float _sizeX;
    private float _sizeY;

    private void Start()
    {
        // set transform to main camera
        _cameraTransform = Camera.main.transform;

        // set camera's current position as last
        _lastCameraPosition = _cameraTransform.position;

        // save references to sprite
        _sprite = GetComponent<SpriteRenderer>().sprite;

        // grab texture from that sprite
        _texture = _sprite.texture;

        // solve for the texture unit size on the x
        _sizeX = _texture.width / _sprite.pixelsPerUnit;

        // solve for the texture unit size on the y
        _sizeY = _texture.height / _sprite.pixelsPerUnit;
    }

    // this update will run after the camera has moved so the
    // variable updates will follow the camera
    private void LateUpdate()
    {
        // update the camera movement
        _movementChange = _cameraTransform.position - _lastCameraPosition;

        // update this transform by adding the delta movement times the multiplier
        // so it can move at a speed relative to the camera speed
        transform.position += new Vector3(_movementChange.x * _parallaxMultiplier.x, _movementChange.y * _parallaxMultiplier.y);

        // reset the last camera position to the current
        _lastCameraPosition = _cameraTransform.position;

        // included toggle for different directions in Unity
        if (_infiniteX)
        {
            // if the difference between the camera postion and the texture unit size
            // in the x direction then relocate the transform to make it appear seamless
            // using an absolute value so that the parallax happens in the negative direction also
            if (Mathf.Abs(_cameraTransform.position.x - transform.position.x) >= _sizeX)
            {
                // calculate offset of camera to be added to the transform position
                _offsetX = (_cameraTransform.position.x - transform.position.x) % _sizeX;

                transform.position = new Vector3(_cameraTransform.position.x + _offsetX, transform.position.y);
            }
        }

        if (_infiniteY)
        {
            // same logic as above just in the Y direction for going up
            if (Mathf.Abs(_cameraTransform.position.y - transform.position.y) >= _sizeY)
            {
                // calculate offset of camera to be added to the transform position
                _offsetY = (_cameraTransform.position.y - transform.position.y) % _sizeY;

                transform.position = new Vector3(_cameraTransform.position.y + _offsetY, transform.position.y);
            }
        }
    }
}
