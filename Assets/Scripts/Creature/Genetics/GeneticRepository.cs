using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Genetics
{
    public class GeneticRepository
    {
        public Dictionary<string, Dictionary<string, BodyPart>> Repository { get; private set; }
        private List<Pattern> Patterns;

        public GeneticRepository() 
        {
            Repository = new Dictionary<string, Dictionary<string, BodyPart>>();
            Patterns = new List<Pattern>();
        }

        /// <summary>
        /// Adds pattern <paramref name="p"/> to the repository
        /// </summary>
        /// <param name="p"></param>
        public void AddPattern(Pattern p) 
        {
            Patterns.Add(p);
        }

        /// <summary>
        /// Returns a random body part in the folder <paramref name="bodyPartType"/>
        /// </summary>
        /// <param name="bodyPartType"></param>
        /// <returns>KeyValuePair<Hash,PartData></Hash></returns>
        public KeyValuePair<string, BodyPart> GetRandomPart (string bodyPartType) 
        {
            int randomNum = Random.Range(0, Repository[bodyPartType].Count);
            Dictionary<string, BodyPart> partList = Repository[bodyPartType];
            var toReturn = partList.ElementAt(randomNum);
            return toReturn;
        }

        /// <summary>
        /// Returns true if a parttype exists in the repository with parts in it.
        /// </summary>
        /// <param name="partType"></param>
        /// <returns></returns>
        public bool HasParts(string partType) 
        {
            if(Repository.ContainsKey(partType))
                return Repository[partType].Count > 0;
            return false;
        }

        /// <summary>
        /// Returns a random pattern avaliable in <paramref name="bodyPart"/>
        /// </summary>
        /// <param name="bodyPart"></param>
        /// <returns></returns>
        public Pattern GetRandomPattern(BodyPart bodyPart)
        {
            int ranNum = Random.Range(0, bodyPart.Patterns.Count);
            string patternName = bodyPart.Patterns[ranNum];
            var toReturn = Patterns.First(p => p.Name.ToLower() == patternName.ToLower());
            return toReturn;
        }

        /// <summary>
        /// Adds a list of <paramref name="bodyPartType"/> to the repository 
        /// </summary>
        /// <param name="bodyPartType"></param>
        /// <returns></returns>
        public Dictionary<string, BodyPart> AddPartList(string bodyPartType) 
        {
            if(!Repository.ContainsKey(bodyPartType))
                Repository.Add(bodyPartType, new Dictionary<string, BodyPart>());
            return GetPartList(bodyPartType);
        }

        /// <summary>
        /// Returns the dictionary of the <paramref name="bodyPartType"/> repository
        /// </summary>
        /// <param name="bodyPartType"></param>
        /// <returns></returns>
        public Dictionary<string, BodyPart> GetPartList(string bodyPartType) 
        {
            return Repository[bodyPartType];
        }

        /// <summary>
        /// Returns a body part based on its <paramref name="hash"/> code
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public BodyPart GetBodyPart(string hash) 
        {
            BodyPart part = null;
            foreach (Dictionary<string, BodyPart> partList in Repository.Values)
            {
                if (partList.TryGetValue(hash, out part))
                    return part;
            }
            return part;
        }

        /// <summary>
        /// Returns a body part based on its <paramref name="name"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BodyPart GetBodyPartByName(string name)
        {
            string partName = System.IO.Path.GetFileNameWithoutExtension(name);
            return GetBodyPart(Importer.GetHashString(partName));
        }

        /// <summary>
        /// Returns a pattern based on its <paramref name="hash"/> code
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public Pattern GetPattern(string hash) 
        {
            return Patterns.First(p => p.Hash == hash);
        }

        /// <summary>
        /// Returns a pattern based on its <paramref name="name"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Pattern GetPatternByName(string name)
        {
            return GetPattern(Importer.GetHashString(name));
        }

        /// <summary>
        /// Returns the shader to get based on the <paramref name="shaderEnum"/>
        /// </summary>
        /// <param name="shader"></param>
        /// <returns></returns>
        public Shader GetShader(ShaderEnum shaderEnum) 
        {
            switch (shaderEnum) 
            {
                default:
                case ShaderEnum.Default:
                    return Shader.Find("Shader Graphs/Creature");
                case ShaderEnum.Fur:
                    return Shader.Find("Shader Graphs/Fur");
            }
        }
    }
}
