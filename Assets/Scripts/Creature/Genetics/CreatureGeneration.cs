using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Siccity.GLTFUtility;
using UnityEditor;
using Utility;
using TextureConverter;
using Creature;

namespace Genetics
{
    public static class CreatureGeneration 
    {
        /// <summary>
        /// The chance of a mutation between 0-1 percent
        /// </summary>
        private const float MUTATION_CHANCE = 0.15f;
        private const string AnimationControllerLocation = "Animation/DemoController";

        private static string[] ImportantPartTypes =
            { "Bodies", "Ears", "Heads", "BackLegs", "FrontLegs" };
        private static string[] LesserPartTypes =
            { "Accessories", "Hats", "Horns", "Masks", "Tails" };

        /// <summary>
        /// Returns a randomly generated creature
        /// </summary>
        /// <returns></returns>
        public static GameObject CreateCreature() 
        {
            GeneticRepository genePool = Utility.Toolbox.Instance.GenePool;

            DNA dna = new DNA();
            foreach (string s in ImportantPartTypes) 
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
                catch
                {
                    continue;
                }
            }

            foreach (string s in LesserPartTypes)
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
                catch 
                {
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

            GameObject creature = new GameObject(); 
            GameObject mesh = ArmatureStitching.StitchObjects(bodyParts);
            mesh.name = "CreatureMesh";
            mesh.transform.parent = creature.transform;
            mesh.transform.rotation = Quaternion.Euler(Vector3.zero);

            creature.name = "Unnamed Creature";
            TextureController texCon = creature.AddComponent<TextureController>();
            texCon.colors = dna.Colors;

            Animator a = mesh.AddComponent<Animator>();
            RuntimeAnimatorController rac = Resources.Load(AnimationControllerLocation) as RuntimeAnimatorController;
            a.runtimeAnimatorController = rac;

            CreatureController c = creature.AddComponent<CreatureController>();
            c.SetUp(dna, a);


            creature.AddComponent<NavMeshMovement>();

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
            partObject.name = "TempObj";
            partObject.transform.localScale = Vector3.one * bodyPart.Scale;

            Texture2D texture2D = new Texture2D(1,1);
            byte[] textureBytes = System.IO.File.ReadAllBytes(pattern.FileLocation);
            Renderer[] models = partObject.transform.GetComponentsInChildren<Renderer>();

            bool useTexture = ImageConversion.LoadImage(texture2D, textureBytes, false);
            
            foreach (Renderer renderer in models) 
            {
                if (useTexture)
                {
                    renderer.gameObject.AddComponent<MaterialConversion>()
                        .SetMainTexture(texture2D, partBits.Pattern);
                }
                renderer.sharedMaterial.shader = genePool.GetShader(bodyPart.Shader);
            }

            return partObject;
        }
    }
}