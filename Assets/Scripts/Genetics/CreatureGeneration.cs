using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Siccity.GLTFUtility;

namespace Genetics
{
    public static class CreatureGeneration 
    {
        /// <summary>
        /// The chance of a mutation between 0-1 percent
        /// </summary>
        private const float MUTATION_CHANCE = 0.15f;
        /// <summary>
        /// Returns a randomly generated creature
        /// </summary>
        /// <returns></returns>
        public static GameObject CreateCreature() 
        {
            return null;
        }

        /// <summary>
        /// Generates a creature from a DNA strand
        /// </summary>
        /// <param name="dna"></param>
        /// <returns></returns>
        public static GameObject CreateCreature(DNA dna) 
        {
            foreach(PartHash part in dna.BodyPartHashs)
            {
                //
            }
            return null;
        }

        /// <summary>
        /// Breeds two parents and returns the resulting DNA
        /// </summary>
        /// <param name="parent1"></param>
        /// <param name="parent2"></param>
        /// <returns></returns>
        public static DNA Breed(DNA parent1, DNA parent2)
        {
            /* To generate a child, take an attribute from each catagory and randomly pick between the two
             * Have a small % chance for a 'mutation' which will pick a random attribute
             */
            throw new NotImplementedException();
        }
    }
}