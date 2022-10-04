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
using Cirrus.Threading;
using Cirrus.Arpg.Entities.Characters;
using Cirrus.Arpg.Entities;

namespace Cirrus.Arpg.AI
{
    [Serializable]
    public class Director : ScriptableObjectBase
    {
        [SerializeField]
        [SerializeEmbedded]
        public DirectorToken[] tokens;
    }

    public class DirectorInstance
    {
        private Director _resource;
        
        private DirectorTokenInstance[] _tokens;

        private GroupInstance _group;

        private Mutex _mutex = new Mutex();

        public Action<CharacterObject, DirectorTokenInstance> onTokenAcquiredHandler;

        public Action<CharacterObject, DirectorTokenInstance> onTokenReturnedHandler;

        public DirectorInstance(Director resource, GroupInstance group)
        {
            _group = group;
            _resource = resource;
            _tokens = new DirectorTokenInstance[resource.tokens.Length];
            for(int i = 0; i < resource.tokens.Length; i++)
            {
                if(resource.tokens[i] == null) continue;
                _tokens[i] = new DirectorTokenInstance(resource.tokens[i], group);
            }

            Refill();
        }

        public void Refill()
        {
            using(new ScopedMutexAcquire(_mutex))
            {
                for(int i = 0; i < _tokens.Length; i++)
                {
                    if(_tokens[i] == null) continue;
                    _tokens[i].Refill();
                }
            }
		}

        public bool Request(CharacterObject source, IActiveAbility ability)
        {
            // If already requesting, then return early
            if(source.Ai.directorRequest != null)
               return false;

            using(new ScopedMutexAcquire(_mutex))
            {
                var token = _tokens.Any(token =>
                (token.Ability != null && token.Ability.Id == ability.Id)
                || token.Features.Intersects(ability.References)
                || token.TestFlags(ability.Flags));

                if(token != null)
                {
                    if(token.Request(source, ability, out DirectorTokenRequest request))
                    {
                        source.Ai.directorRequest = request;
                        onTokenAcquiredHandler?.Invoke(source, token);
                        return true;
                    }
                }

                return false;
            }
		}

        public void Return(CharacterObject source, IActiveAbility ability)
        {            
            if(source.Ai.directorRequest == null) return;

            using(new ScopedMutexAcquire(_mutex))
            {
                if(!ReturnAssert(source.Ai.directorRequest != null && source.Ai.directorRequest.ability == ability)) return;
                source.Ai.directorRequest.token.Return(source);
                onTokenReturnedHandler?.Invoke(source, source.Ai.directorRequest.token);
                source.Ai.directorRequest = null;
            }
        }
    }
}