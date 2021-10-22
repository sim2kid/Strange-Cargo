using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Genetics
{
    public class GeneticRepository
    {
        private Dictionary<string, Dictionary<string, BodyPart>> Repository;
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
        /// <returns></returns>
        public KeyValuePair<string, BodyPart> GetRandomPart (string bodyPartType) 
        {
            return Repository[bodyPartType].ElementAt(Random.Range(0, Repository[bodyPartType].Count));
        }

        /// <summary>
        /// Returns a random pattern avaliable in <paramref name="bodyPart"/>
        /// </summary>
        /// <param name="bodyPart"></param>
        /// <returns></returns>
        public Pattern GetRandomPattern(BodyPart bodyPart)
        {
            string patternName = bodyPart.Patterns[Random.Range(0, bodyPart.Patterns.Count)];
            return Patterns.First(p => p.Name == patternName);
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
        /// Returns a pattern based on its <paramref name="hash"/> code
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public Pattern GetPattern(string hash) 
        {
            return Patterns.First(p => p.Hash == hash);
        }
        
    }
}
