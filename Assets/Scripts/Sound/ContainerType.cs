using Sound.Structure;
using Sound.Source.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    public enum ContainerType
    {
        Unknown,
        ResourceClip,
        ResourceList,
        SoundClip,
        RandomContainer,
        SwitchContainer
    }
    public static class ContainerTypeExtensions 
    {
        public static ISound Resolve(this ContainerType type) 
        {
            switch (type) 
            {
                case ContainerType.ResourceClip:
                    return new ResourceClip();
                case ContainerType.ResourceList:
                    return new ResourceList();
                case ContainerType.SoundClip:
                    return new SoundClip();
                case ContainerType.RandomContainer:
                    return new RandomContainer();
                case ContainerType.SwitchContainer:
                    return new SwitchContainer();
                default:
                    return null;
            }
        }

        public static ContainerType GetTypeEnum(this ISound type) 
        {
            if(type is ResourceClip)
                return ContainerType.ResourceClip;
            if (type is SoundClip)
                return ContainerType.SoundClip;
            if (type is RandomContainer)
                return ContainerType.RandomContainer;
            if (type is ResourceList)
                return ContainerType.ResourceList;
            if (type is SwitchContainer)
                return ContainerType.SwitchContainer;
            return ContainerType.Unknown;
        }
    }
}