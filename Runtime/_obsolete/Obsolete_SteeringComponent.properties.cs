//using Cirrus.Unity.Objects;
//using Cirrus.Arpg.Entities.Characters.Controls;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace Cirrus.Arpg.AI
//{
//	public partial class SteeringComponent
//	{

//		[Header("3D Settings")]
//		/// <summary>
//		/// Determines if the character should follow the ground or can fly any where in 3D space
//		/// </summary>
//		public bool IsAirbornAllowed;

//		[Header("3D Grounded Settings")]
//		/// <summary>
//		/// If the character should try to stay grounded
//		/// </summary>
//		public bool stayGrounded = true;

//		/// <summary>
//		/// How far the character should look below him for ground to stay grounded to
//		/// </summary>
//		public float groundFollowDistance = 0.1f;

//		/// <summary>
//		/// The sphere cast mask that determines what layers should be consider the ground
//		/// </summary>
//		public LayerMask groundCheckMask = UnityEngine.Physics.DefaultRaycastLayers;

//		/// <summary>
//		/// The maximum slope the character can climb in degrees
//		/// </summary>
//		public float slopeLimit = 80f;

//		//private 
//		private CapsuleCollider _collider = null;
//		private CapsuleCollider _Collider => _collider == null ?
//			_collider = _Character.Kinematics.Motor.Capsule :
//			_collider;

//		private CharacterObject _character;
//		private CharacterObject _Character =>
//			_character == null ?
//			_character = GetComponentInParent<CharacterObject>() :
//			_character;

//		private CharacterControl _control;
//		private CharacterControl _Control =>
//			_control == null ?
//			_control = _Character.GetComponentInChildren<CharacterControl>() :
//			_control;

//		private SteeringSupportComponent2 _support;
//		public SteeringSupportComponent2 Internal =>
//			_support == null ?
//			_support = GetComponent<SteeringSupportComponent2>() :
//			_support;


//		/// <summary>
//		/// The radius for the current game object (either the radius of a sphere or circle
//		/// collider). If the game object does not have a sphere or circle collider this 
//		/// will return -1.
//		/// </summary>
//		public float Radius => _Collider != null ?
//			Mathf.Max(
//				_Character.Transform.localScale.x, 
//				_Character.Transform.localScale.y,
//				_Character.Transform.localScale.z) * _Collider.radius :
//			-1;

//		/// <summary>
//		/// The current ground normal for this character. This value is only used by 3D 
//		/// characters who cannot fly.
//		/// </summary>
//		[System.NonSerialized]
//		public Vector3 WallNormal = Vector3.zero;

//		/// <summary>
//		/// The current movement plane normal for this character. This value is only
//		/// used by 3D characters who cannot fly.
//		/// </summary>
//		[System.NonSerialized]
//		public Vector3 MovementNormal = Vector3.up;


//		/// <summary>
//		/// The position that should be used for most movement AI code. For 2D chars the position will 
//		/// be on the X/Y plane. For 3D grounded characters the position is on the X/Z plane. For 3D
//		/// flying characters the position is in full 3D (X/Y/Z).
//		/// </summary>
//		public Vector3 Position => IsAirbornAllowed ?
//			_Character.Position :
//			new Vector3(_Character.Position.x, 0, _Character.Position.z);

//		/// <summary>
//		/// Gets the position of the collider (which can be offset from the transform position).
//		/// </summary>
//		public Vector3 ColliderPosition =>
//			Transform.TransformPoint(_Collider.center) + _Character.Position - _Character.Position;


//		public Quaternion DesiredRotation
//		{
//			get => _Control.ControlRotation;

//			set => _Control.ControlRotation = value;
//		}

//		public Vector3 DesiredVelocity
//		{
//			get => _Control.ControlVelocity;

//			set => _Control.ControlVelocity = value;			
//		}

//		/// <summary>
//		/// The actual velocity of the underlying unity rigidbody.
//		/// </summary>
//		public Vector3 Velocity
//		{
//			get => _Character.Kinematics.Velocity;
//		}

//		public Transform Transform => _Character.Transform;

//	}
//}
