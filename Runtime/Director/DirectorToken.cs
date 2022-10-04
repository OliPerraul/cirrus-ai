using System.Collections;

using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Cirrus.Arpg.Abilities;
using Cirrus.Collections;
using Cirrus.Unity.Objects;
using Object = UnityEngine.Object;
using System.Threading;
using Cirrus.Objects;
using Cirrus.Unity.Randomness;
using Cirrus.Randomness;
using Cirrus.Unity.Editor;
using static Cirrus.Debugging.DebugUtils;
using Cirrus.Unity.Numerics;
using Cirrus.Arpg.Entities.Characters;
using Cirrus.Arpg.Entities;

namespace Cirrus.Arpg.AI
{
    public interface IDirectorToken : IFlagged<ActiveAbilityContentFlags>
    {
        int MaxTokens { get; }

        public HashSet<Object> Features { get; }

        public IAbility Ability { get; }
    }

    public enum DirectorTokenRequestFlags
    { 
        Stale = 1 << 0,
    }

    public class DirectorTokenRequest : IFlagged<DirectorTokenRequestFlags>
    {   
        public DirectorTokenRequestFlags Flags { get; set; }

        public CharacterObject source;

        public IActiveAbility ability;

        public DirectorTokenInstance token;

        public float priority = float.NegativeInfinity;
    }

    // TODO: let prioritize certain character in the group
    // higher chance for certain character
    public class DirectorToken : ScriptableObject, IDirectorToken, IIDed
    {
        [field: SerializeField]
        public ID Id { get; set; }

        [SerializeField]
        [Range(1,100)]
        public int maxTokens = 1;
        public int MaxTokens => maxTokens;

        [SerializeField]
        private ActiveAbilityContentFlags _flags;
        ActiveAbilityContentFlags IFlagged<ActiveAbilityContentFlags>.Flags
        {
            get => _flags;
            set => _flags = value;
        }

        //[SerializeField]
        //public RandomizationAsset<Ob>

        // TODO: reference common skill by other than skill itself (e.g. all blunt attacks for a group)
        [SerializeField]
        private List<Object> _features;
        private HashSet<Object> _features_;
        public HashSet<Object> Features => _features_ == null ?
            _features_ = new HashSet<Object>(_features) :
            _features_;

        [SerializeField]
        private MonoBehaviourBase _ability;
        public IAbility Ability => _ability == null ? null : (IAbility)_ability;

        [SerializeField]
        private DirectorTokenPriority[] _priorities;
        public DirectorTokenPriority[] Priorities => _priorities;

        public void OnValidate()
        {
            if(_priorities == null) _priorities = new DirectorTokenPriority[0];
            Array.Sort(_priorities, Comparer<DirectorTokenPriority>.Default);
        }


        //public int GetHash()
        //{
        //    HashCode hashCode = new HashCode();
        //    hashCode.Add(_ability == null ? -1 : Ability.Id);
        //    for(int i = 0; i < _references.Count; i++) hashCode.Add(((ISerial)_references[i]).Id);
        //    return hashCode.ToHashCode();
        //}
    }

    //public struct DirectorTokenOwnership
    //{
    //    public bool isStale;
    //}



    public class DirectorTokenInstance : IDirectorToken, IIDed
    {
        public int _tokens = 1;

        public DirectorToken _resource;

        private GroupInstance _group;

        // if ask again let them use it
        private BHeap<DirectorTokenRequest> _waitlist = new BHeap<DirectorTokenRequest>(SortDirection.Descending);

        // max size of number of token
        //private HashSet<EntityInstanceBase> _vips = new HashSet<EntityInstanceBase>();

        public int MaxTokens => ((IDirectorToken)_resource).MaxTokens;

		public HashSet<Object> Features => ((IDirectorToken)_resource).Features;

		public IAbility Ability => ((IDirectorToken)_resource).Ability;

		//public ActiveAbilityContentFlags Flags => ((IDirectorToken)_resource).Flags;

		ActiveAbilityContentFlags IFlagged<ActiveAbilityContentFlags>.Flags { get => ((IFlagged<ActiveAbilityContentFlags>)_resource).Flags; set => ((IFlagged<ActiveAbilityContentFlags>)_resource).Flags = value; }
		public ID Id { get => ((IIDed)_resource).Id; set => ((IIDed)_resource).Id = value; }

		public override int GetHashCode()
		{
            return Id.GetHashCode();
		}

		public override bool Equals(object obj)
		{
            return Id.Equals(((DirectorTokenInstance)obj).Id);
		}

		public DirectorTokenInstance(DirectorToken resource, GroupInstance group)
        {
            _resource = resource;
            _group = group;
        }

        public void Refill()
        {            
            if(_resource == null) return;
            _tokens = _resource.maxTokens;
        }

        // TODO: dynamic priority based on other factor such as distance
        float _GetPriority(CharacterObject source)
        {
            float weight = float.NegativeInfinity;
            for(int i = 0; i < _resource.Priorities.Length; i++)
            {
                DirectorTokenPriority prio = _resource.Priorities[i];
                if((prio.entity != null && prio.entity.Id == source.Id)
                || prio.Features.Intersects(source.References)
                || prio.Intersects(source))
                {
                    float random = prio.weight.Random();
                    if(random > weight) weight = random;
                }
            }

            return weight;
        }

        void _UpdateWaitlist(CharacterObject source, IActiveAbility ability)
        {
            // If first in waitlist
            DirectorTokenRequest request = null;
            if(source.Ai.directorWaitlistRequests.TryGetValue(this, out request))
            {
                var priority = _GetPriority(source);
                if(request.priority < priority)
                {
                    request.AddFlags(DirectorTokenRequestFlags.Stale);
                    request = new() { source = source, ability = ability, token = this, priority = priority };
                    _waitlist.Insert(request, priority);
                    source.Ai.directorWaitlistRequests[this] = request;
                }
            }
            else
            {
                float priority = _GetPriority(source);
                request = new (){ source = source, ability = ability, token = this, priority = priority };
                _waitlist.Insert(request, priority);
                source.Ai.directorWaitlistRequests.Add(this, request);
            }
        }

        public bool Request(CharacterObject source, IActiveAbility ability, out DirectorTokenRequest request)
        {
            request = null;
            if(source.Ai.directorRequest != null) return false;
            while(_waitlist.Count != 0 && _waitlist.First.Item1.TestFlags(DirectorTokenRequestFlags.Stale)) _waitlist.Pop();
            if(_tokens != 0)
            {
                if(_waitlist.Count == 0 || _waitlist.First.Item1.source == source)
                {
                    source.Ai.directorWaitlistRequests.Remove(this);
                    request = _waitlist.Count == 0 ?
                         new DirectorTokenRequest { source = source, ability = ability, token = this } :
                        _waitlist.Pop().Item1;
                       ;
                    --_tokens;
                    return true;
                }
            }

            _UpdateWaitlist(source, ability);
            return false;
        }

        public void Return(CharacterObject source)
        {
            _tokens++;
            Assert(_tokens <= _resource.maxTokens);
            if(_tokens > _resource.maxTokens)
                _tokens = _resource.maxTokens;
            //source.Ai.directorRequest = null;
        }
    }
}