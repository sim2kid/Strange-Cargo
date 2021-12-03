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
                BodyPart bodypart = new BodyPart();
                if (!genePool.HasParts(s)) 
                {
                    Console.LogError($"No parts have been found in the {s} part folder! This is a required body part and creatures will look bad without it. Make sure you have parts in your parts folder.");
                    continue;
                }
                try
                {
                    bodypart = genePool.GetRandomPart(s).Value;
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
                    Console.LogError($"Body Part {bodypart.Name} in {s} could not be initialized. This errored with this exception:\n{e}");
                    continue;
                }
            }

            foreach (string s in LesserPartTypes)
            {
                BodyPart bodypart = new BodyPart();
                if (!genePool.HasParts(s))
                {
                    Console.Log($"Body Part Repository, {s}, does not exist with parts in the StreamingAssets folder. Skipping...");
                    continue;
                }
                try
                {
                    bodypart = genePool.GetRandomPart(s).Value;
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
                    Console.LogError($"Body Part {bodypart.Name} in {s} could not be initialized. This errored with this exception:\n{e}");
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
                foreach(GameObject g in CreateBodyPart(part))
                    bodyParts.Add(g);
            }

            GameObject creature = new GameObject(); 
            GameObject mesh = ArmatureStitching.StitchObjects(bodyParts);
            mesh.name = "CreatureMesh";
            mesh.transform.parent = creature.transform;
            mesh.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));

            creature.name = "Unnamed Creature";
            TextureController texCon = creature.AddComponent<TextureController>();
            texCon.colors = dna.Colors;

            Animator a = mesh.AddComponent<Animator>();
            RuntimeAnimatorController rac = Resources.Load(AnimationControllerLocation) as RuntimeAnimatorController;
            a.runtimeAnimatorController = rac;

            CreatureAnimationControllerDemo cACD = creature.AddComponent<CreatureAnimationControllerDemo>();

            Face[] faceParts = creature.GetComponentsInChildren<Face>();
            if (faceParts.Length != 0) 
            {
                FaceTexture face = creature.AddComponent<FaceTexture>();
                foreach (Face f in faceParts) 
                {
                    if (f.IsMouth)
                    {
                        face.Mouth = f.gameObject;
                    }
                    else if (f.IsEyes) 
                    {
                        face.Eyes = f.gameObject;
                    }
                }
            }

            FeetSound[] feet = creature.GetComponentsInChildren<FeetSound>();
            string frontFeetSound = string.Empty, backFeetSound = string.Empty;
            foreach (FeetSound foot in feet)
            {
                if (!string.IsNullOrWhiteSpace(foot.Sound)) 
                {
                    if (foot.IsFrontFeet)
                    {
                        frontFeetSound = foot.Sound;
                    }
                    else
                    {
                        backFeetSound = foot.Sound;
                    }
                }
                FeetSound.Destroy(foot);
            }

            BoneToPick[] boneModi = creature.GetComponentsInChildren<BoneToPick>();
            foreach (BoneToPick bone in boneModi) 
            {
                if (!string.IsNullOrWhiteSpace(bone.BoneOffset))
                {
                    Transform pick = ArmatureStitching.FindChildByName(bone.BoneOffset, creature.transform);
                    pick.position += bone.Offset;
                }
                BoneToPick.Destroy(bone);
            }

            CreatureController c = creature.AddComponent<CreatureController>();
            c.SetUp(dna, a, Guid.NewGuid().ToString(), frontFeetSound, backFeetSound);

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

        private static List<GameObject> CreateBodyPart(PartHash partBits) 
        {
            List<GameObject> g = new List<GameObject>();
            GeneticRepository genePool = Utility.Toolbox.Instance.GenePool;
            BodyPart bodyPart = genePool.GetBodyPart(partBits.BodyPart);
            Pattern pattern = genePool.GetPattern(partBits.Pattern);

            GameObject partObject = Siccity.GLTFUtility.Importer.LoadFromFile(bodyPart.FileLocation);
            partObject.name = "TempObj";
            partObject.transform.localScale = Vector3.one * bodyPart.Scale;

            Texture2D texture2D = new Texture2D(1,1);
            byte[] textureBytes = System.IO.File.ReadAllBytes(pattern.FileLocation);
            Renderer[] models = partObject.transform.GetComponentsInChildren<Renderer>();
            if (models.Length == 0)
                Console.LogError($"Body part {bodyPart.Name} in {bodyPart.Type} had no skinned meshes. This body part will not render correctly. Please make sure your model is correct.");

            bool useTexture = ImageConversion.LoadImage(texture2D, textureBytes, false);

            if (!string.IsNullOrWhiteSpace(bodyPart.Eyes)) 
            {
                BodyPart eyes = genePool.GetBodyPartByName(bodyPart.Eyes);
                GameObject eyesObject = Siccity.GLTFUtility.Importer.LoadFromFile(eyes.FileLocation);
                g.Add(eyesObject);
                eyesObject.AddComponent<Face>().IsEyes = true;
            }

            if (!string.IsNullOrWhiteSpace(bodyPart.Mouth))
            {
                BodyPart mouth = genePool.GetBodyPartByName(bodyPart.Mouth);
                GameObject mouthObject = Siccity.GLTFUtility.Importer.LoadFromFile(mouth.FileLocation);
                g.Add(mouthObject);
                mouthObject.AddComponent<Face>().IsMouth = true;
            }

            int i = 0;
            foreach (Renderer renderer in models) 
            {
                if (i == 0) 
                {
                    if (!string.IsNullOrWhiteSpace(bodyPart.OffsetBone) && bodyPart.Offset != null) 
                        renderer.gameObject.AddComponent<BoneToPick>().Populate(bodyPart.OffsetBone, bodyPart.Offset);
                    if (bodyPart.Type == "FrontLegs" || bodyPart.Type == "BackLegs")
                        renderer.gameObject.AddComponent<FeetSound>().Populate(bodyPart.Sound, bodyPart.Type == "FrontLegs");
                }
                i++;
                if (useTexture && texture2D.width != 1)
                {
                    renderer.gameObject.AddComponent<MaterialConversion>()
                        .SetMainTexture(texture2D, partBits.Pattern);
                } 
                else 
                {
                    Console.LogWarning($"Pattern Texture {pattern.Name} could not be loaded into the material. The load result is {useTexture}, and the 2D Texture size is ({texture2D.width}, {texture2D.height}).");
                }
                renderer.sharedMaterial.shader = genePool.GetShader(bodyPart.Shader);
            }

            g.Add(partObject);
            return g;
        }
    }
}