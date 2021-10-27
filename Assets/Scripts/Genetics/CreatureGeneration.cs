using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Siccity.GLTFUtility;
using UnityEditor;
using Utility;
using TextureConverter;

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
            string[] importantPartTypes =
                { "Bodies", "Ears", "Heads", "Legs" };
            string[] lesserPartTypes =
                { "Accessories", "Hats", "Horns", "Masks", "Tails" };

            GeneticRepository genePool = Utility.Toolbox.Instance.GenePool;

            DNA dna = new DNA();
            foreach (string s in importantPartTypes) 
            {
                try
                {
                    BodyPart bodypart = genePool.GetRandomPart(s).Value;
                    Pattern pattern = genePool.GetRandomPattern(bodypart);
                    PartHash part = new PartHash()
                    {
                        Category = s,
                        BodyPart = bodypart.Hash,
                        Pattern = pattern.Hash
                    };
                    dna.BodyPartHashs.Add(part);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    continue;
                }
            }

            foreach (string s in lesserPartTypes)
            {
                try
                {
                    BodyPart bodypart = genePool.GetRandomPart(s).Value;
                    if (bodypart == null || UnityEngine.Random.Range(0f, 1f) < MUTATION_CHANCE)
                        continue;
                    PartHash part = new PartHash()
                    {
                        Category = s,
                        BodyPart = bodypart.Hash,
                        Pattern = genePool.GetRandomPattern(bodypart).Hash
                    };
                    dna.BodyPartHashs.Add(part);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    continue;
                }
        }

            Color[] colors = new Color[3];
            for (int i = 0; i < 3; i++)
                colors[i] = RandomColorPicker.RetriveRandomColor();
            dna.Colors = colors;

            return CreateCreature(dna);
        }

        /// <summary>
        /// Generates a creature from a DNA strand
        /// </summary>
        /// <param name="dna"></param>
        /// <returns></returns>
        public static GameObject CreateCreature(DNA dna) 
        {
            List<GameObject> bodyParts = new List<GameObject>();
            
            foreach(PartHash part in dna.BodyPartHashs)
            {
                bodyParts.Add(CreateBodyPart(part));
            }

            GameObject creature = ArmatureStitching.StitchObjects(bodyParts);
            creature.name = "Unnamed Creature";
            TextureController texCon = creature.AddComponent<TextureController>();
            texCon.colors = dna.Colors;

            return creature;
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

        private static GameObject CreateBodyPart(PartHash partBits) 
        {
            GeneticRepository genePool = Utility.Toolbox.Instance.GenePool;
            BodyPart bodyPart = genePool.GetBodyPart(partBits.BodyPart);
            Pattern pattern = genePool.GetPattern(partBits.Pattern);

            GameObject partObject = Siccity.GLTFUtility.Importer.LoadFromFile(bodyPart.FileLocation);
            partObject.name = bodyPart.Name;

            Texture2D texture2D = null;
            byte[] textureBytes = System.IO.File.ReadAllBytes(pattern.FileLocation);
            if (ImageConversion.LoadImage(texture2D, textureBytes, false)) 
            {
                partObject.GetComponent<Renderer>().material.mainTexture = texture2D;
            }

            partObject.GetComponent<Renderer>().sharedMaterial.shader = genePool.GetShader(bodyPart.Shader);

            return partObject;
        }
    }
}