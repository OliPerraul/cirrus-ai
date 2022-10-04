using Cirrus.Arpg.AI;
using Cirrus.Arpg.Entities.Characters.Controls;
using Cirrus.Unity.Objects;
using System;
using System.Collections.Generic;

using UnityEngine;

namespace Cirrus.Arpg.AI
{
	// NOTES:
	// We can do a navmesh raycast! :)
	// To validate our steering behaviours
	//  bool blocked = NavMesh.Raycast(transform.position, transform.position + velocity.normalized * this.transform.Find("InfluenceBox").GetComponent<BoxCollider>().size.z, out hit, NavMesh.AllAreas);

	public partial class SteeringComponent
	{
		[SerializeField]
		public float GizmosDistance = 2f;

		[SerializeField]
		public float GizmosSize = 2f;

		[Header("Context Steering")]
		[NonSerialized]
		public DiscreteDirections resolution = DiscreteDirectionUtils.MidResDirections;

		[field: SerializeField]
		public float LinearAcceleration { get; private set; } = 1.5f;

		public List<SteeringNodeInstanceBase> nodes = new List<SteeringNodeInstanceBase>();

		//[field: SerializeField]
		//public float RotationLerpSpeed { get; private set; } = 20f;

		//[field: Header("Implementation Properties")]
		//public Quaternion DesiredLerpRotation { get; set; }

		public Vector3 acceleration;

		public Transform Transform => CharacterObject.Transform;

#if CIRRUS_ARPG_CHARACTER_TRANSFORM_LIBRARY
		public Vector3 BodyMiddlePosition => CharacterObject.CharacterTransforms.BodyMiddlePosition;
#else
		public Vector3 BodyMiddlePosition => Vector3.zero;
#endif

		private CharacterControlComponent _control;

		public CharacterControlComponent Control =>
			_control == null ?
			_control = CharacterObject.GetComponentInChildren<CharacterControlComponent>() :
			_control;

		private ObjectWrapper<CapsuleCollider> _collider = null;

		public CapsuleCollider Collider =>
			_collider == null ?
			_collider = CharacterObject.Transform.GetComponentInChildren<CapsuleCollider>() :
			_collider;

		/// <summary>
		/// Gets the position of the collider (which can be offset from the transform position).
		/// </summary>
		public Vector3 ColliderPosition
			=> Transform.TransformPoint(Collider.center) + CharacterObject.RigidbodyPosition - CharacterObject.Position;

		public Quaternion TransformRotation
		{
			get => CharacterObject.Transform.rotation;

			set => CharacterObject.Transform.rotation = value;
		}

		public Vector3 RigidbodyVelocity => CharacterObject.Kinematics.Velocity;

	}
}